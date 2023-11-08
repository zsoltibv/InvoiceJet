using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services
{
    public interface IFirmService
    {
        Task<FirmDto> GetFirmDataFromAnaf(string cui);
    }
}
