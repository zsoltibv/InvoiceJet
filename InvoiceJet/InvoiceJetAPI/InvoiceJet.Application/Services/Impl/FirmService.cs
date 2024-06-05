using System.Net.Http.Json;
using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Exceptions;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace InvoiceJet.Application.Services.Impl;

public class FirmService : IFirmService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly string _apiUrl;
    private readonly IUnitOfWork _unitOfWork;

    public FirmService(IConfiguration config, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _httpClient = new HttpClient();
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _apiUrl = config.GetSection("AppSettings")?["AnafApiUrl"] ??
                  throw new ArgumentNullException("AnafApiUrl is not configured");
    }

    public async Task<FirmDto> GetFirmDataFromAnaf(string cui)
    {
        FirmDto firmDto = new FirmDto();
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

                        int startIndex = address.IndexOf("STR.");
                        if (startIndex == -1)
                        {
                            startIndex = address.IndexOf("ŞOS.");
                        }

                        if (startIndex == -1)
                        {
                            startIndex = address.IndexOf("BLD.");
                        }

                        if (startIndex == -1)
                        {
                            startIndex = address.IndexOf("CAL.");
                        }

                        if (name != null && cuiValue != null && regCom != null && address != null)
                        {
                            firmDto.Name = name;
                            firmDto.RegCom = regCom;
                            firmDto.Cui = cuiValue;
                            firmDto.Address = address.Substring(startIndex);
                        }
                    }
                }

                var adrDomiciliuFiscal = json["found"]?[0]?["adresa_domiciliu_fiscal"];
                if (adrDomiciliuFiscal != null)
                {
                    string? county = adrDomiciliuFiscal["ddenumire_Judet"]?.ToString();
                    string? city = adrDomiciliuFiscal["ddenumire_Localitate"]?.ToString();
                    if (county != null && city != null)
                    {
                        firmDto.County = county;
                        firmDto.City = city;
                    }
                }

                return firmDto;
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
            firm = await _unitOfWork.Firms.GetByIdAsync(firmDto.Id);
            if (firm == null)
            {
                return null;
            }

            firm = _mapper.Map(firmDto, firm);
        }
        else
        {
            firm = _mapper.Map<Firm>(firmDto);
            await _unitOfWork.Firms.AddAsync(firm);
        }

        await _unitOfWork.CompleteAsync();

        if (firmDto.Id == 0 || isClient)
        {
            var existingUserFirm = await _unitOfWork.UserFirms.Query()
                .FirstOrDefaultAsync(uf => uf.UserId == userId && uf.FirmId == firm.Id);

            if (existingUserFirm == null)
            {
                await _unitOfWork.UserFirms.AddAsync(new UserFirm
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

        await _unitOfWork.CompleteAsync();

        if (firmDto.Id == 0 && !isClient)
        {
            var existingUser = await _unitOfWork.Users.Query()
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (existingUser is not null)
            {
                var activeUserFirm = await _unitOfWork.UserFirms.Query()
                    .Where(uf => uf.UserId == userId && uf.FirmId == firm.Id)
                    .FirstOrDefaultAsync();

                if (activeUserFirm == null)
                {
                    activeUserFirm = new UserFirm
                    {
                        UserId = userId,
                        FirmId = firm.Id,
                        IsClient = false
                    };
                    await _unitOfWork.UserFirms.AddAsync(activeUserFirm);
                    await _unitOfWork.CompleteAsync();
                    existingUser.ActiveUserFirmId = activeUserFirm.UserFirmId;
                }
                else
                {
                    existingUser.ActiveUserFirmId = activeUserFirm.UserFirmId;
                    // await DbSeeder.SeedDocumentSeries(_dbContext, activeUserFirm.UserFirmId);
                }
            }
            else
            {
                return null;
            }
        }

        await _unitOfWork.CompleteAsync();

        firmDto.Id = firm.Id;
        return firmDto;
    }

    public async Task<FirmDto> GetUserActiveFirmById(Guid userId)
    {
        var activeUserFirm = await _unitOfWork.Users.Query()
            .Where(u => u.Id == userId)
            .Include(u => u.ActiveUserFirm)
            .Select(u => u.ActiveUserFirm.Firm)
            .FirstOrDefaultAsync();

        if (activeUserFirm == null)
        {
            return new FirmDto();
        }

        return _mapper.Map<FirmDto>(activeUserFirm);
    }

    public async Task<ICollection<FirmDto>> GetUserClientFirmsById(Guid userId)
    {
        var userFirms = await _unitOfWork.UserFirms.Query()
            .Where(u => u.UserId.Equals(userId) && u.IsClient)
            .Include(f => f.Firm)
            .ToListAsync();

        var firms = userFirms.Select(u => u.Firm).ToList();

        var firmDtos = _mapper.Map<ICollection<FirmDto>>(firms);

        return firmDtos;
    }
}