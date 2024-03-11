using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services
{
    public interface IFirmService
    {
        Task<FirmDto> GetFirmDataFromAnaf(string cui);
        Task<FirmDto> AddOrEditFirm(FirmDto firmDto, Guid userId, bool isClient);
        Task<FirmDto> GetUserActiveFirmById(Guid id);
    }
}
