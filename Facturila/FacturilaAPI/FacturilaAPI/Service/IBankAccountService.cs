using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services
{
    public interface IBankAccountService
    {
        Task<ICollection<BankAccountDto>> GetUserFirmBankAccounts(Guid userId);
        Task<BankAccountDto> AddOrEditBankAccount(BankAccountDto bankAccountDto, Guid userId);
    }
}
