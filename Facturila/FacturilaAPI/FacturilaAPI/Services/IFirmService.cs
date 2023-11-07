using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services
{
    public interface IFirmService
    {
        Task<FirmDataDto> GetFirmDataFromAnaf(string cui);
    }
}
