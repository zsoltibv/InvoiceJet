using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IFirmService
{
    Task<FirmDto> GetFirmDataFromAnaf(string cui);
    Task<FirmDto> AddOrEditFirm(FirmDto firmDto, Guid userId, bool isClient);
    Task<FirmDto> GetUserActiveFirmById(Guid id);
    Task<ICollection<FirmDto>> GetUserClientFirmsById(Guid userId);
}