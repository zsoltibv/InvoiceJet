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
        private readonly string _apiUrl;

        public FirmService(FacturilaDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _httpClient = new HttpClient();
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

        public async Task<FirmDto> AddOrEditFirm(FirmDto firmDto)
        {
            var existingFirm = await _dbContext.Firm.FindAsync(firmDto.Id);
            if (existingFirm != null)
            {
                existingFirm.Name = firmDto.Name;
                existingFirm.RegCom = firmDto.RegCom;
                existingFirm.Address = firmDto.Address;
                existingFirm.County = firmDto.County;
                existingFirm.City = firmDto.City;
                
                _dbContext.Firm.Update(existingFirm);
            }
            else
            {
                var newFirm = new Firm
                {
                    CUI = firmDto.CUI,
                    Name = firmDto.Name,
                    RegCom = firmDto.RegCom,
                    Address = firmDto.Address,
                    County = firmDto.County,
                    City = firmDto.City
                };

                _dbContext.Firm.Add(newFirm);
            }

            await _dbContext.SaveChangesAsync();

            return firmDto;
        }
    }
}
