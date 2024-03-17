using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Exceptions;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace FacturilaAPI.Services.Impl
{
    public class FirmService : IFirmService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly string _apiUrl;

        public FirmService(FacturilaDbContext dbContext, IConfiguration config, IMapper mapper)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
            _mapper = mapper;
            _apiUrl = config.GetSection("AppSettings")?["AnafApiUrl"] ?? throw new ArgumentNullException("AnafApiUrl is not configured");
        }

        public async Task<FirmDto> GetFirmDataFromAnaf(string cui)
        {
            try
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                var requestBody = new[]
                {
                    new
                    {
                        cui = cui,
                        data = currentDate
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(_apiUrl, requestBody);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseString);

                    var dateGenerale = json["found"]?[0]?["date_generale"];
                    if (dateGenerale != null)
                    {
                        if (dateGenerale != null)
                        {
                            string? name = dateGenerale["denumire"]?.ToString();
                            string? cuiValue = dateGenerale["cui"]?.ToString();
                            string? regCom = dateGenerale["nrRegCom"]?.ToString();
                            string? address = dateGenerale["adresa"]?.ToString();

                            if (name != null && cuiValue != null && regCom != null && address != null)
                            {
                                return new FirmDto
                                {
                                    Name = name,
                                    CUI = cuiValue,
                                    RegCom = regCom,
                                    Address = address
                                };
                            }
                        }
                    }
                }
                throw new AnafFirmNotFoundException(cui);
            }
            catch (Exception)
            {
                throw new AnafFirmNotFoundException(cui);
            }
        }

        public async Task<FirmDto> AddOrEditFirm(FirmDto firmDto, Guid userId, bool isClient)
        {
            Firm firm;

            if (firmDto.Id != 0)
            {
                // Edit existing firm
                firm = await _dbContext.Firm.FindAsync(firmDto.Id);
                if (firm == null)
                {
                    // Handle the case where the firm isn't found. Maybe throw an exception or handle it another way.
                    return null;
                }
                firm = _mapper.Map(firmDto, firm);
            }
            else
            {
                // Add new firm
                firm = _mapper.Map<Firm>(firmDto);
                _dbContext.Firm.Add(firm);
            }

            // Save changes to Firm before continuing, to ensure we have a FirmId for new firms.
            await _dbContext.SaveChangesAsync();

            if (firmDto.Id == 0 || isClient)
            {
                var existingUserFirm = await _dbContext.UserFirm
                    .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FirmId == firm.Id);

                if (existingUserFirm == null)
                {
                    _dbContext.UserFirm.Add(new UserFirm
                    {
                        UserId = userId,
                        FirmId = firm.Id,
                        IsClient = isClient
                    });
                }
                else
                {
                    existingUserFirm.IsClient = isClient;
                }
            }

            // Update the user's active firm if necessary.
            if (firmDto.Id == 0 && !isClient)
            {
                var existingUser = await _dbContext.User.FindAsync(userId);
                if (existingUser != null)
                {
                    existingUser.ActiveFirmId = firm.Id;
                }
                else
                {
                    // Handle the case where the user isn't found. Maybe throw an exception or handle it another way.
                    return null;
                }
            }

            await _dbContext.SaveChangesAsync();
            firmDto.Id = firm.Id;

            return firmDto;
        }

        public async Task<FirmDto> GetUserActiveFirmById(Guid userId)
        {
            var user = await _dbContext.User
                .Where(u => u.Id == userId)
                .Include(u => u.ActiveFirm)
                .Select(u => new { u.ActiveFirmId, u.ActiveFirm })
                .FirstOrDefaultAsync();

            if (user == null || !user.ActiveFirmId.HasValue)
            {
                return null;
            }

            return _mapper.Map<FirmDto>(user.ActiveFirm);
        }

        public async Task<ICollection<FirmDto>> GetUserClientFirmsById(Guid userId)
        {
            var userFirms = await _dbContext.UserFirm
                .Where(u => u.UserId == userId && u.IsClient)
                .Include(f => f.Firm)
                .ToListAsync();

            var firms = userFirms.Select(u => u.Firm).ToList();

            var firmDtos = _mapper.Map<ICollection<FirmDto>>(firms);

            return firmDtos;
        }
    }
}
