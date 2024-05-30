using InvoiceJetAPI.Models.Dto;

namespace InvoiceJetAPI.Services
{
    public interface IBankAccountService
    {
        Task<ICollection<BankAccountDto>> GetUserFirmBankAccounts(Guid userId);
        Task<BankAccountDto> AddOrEditBankAccount(BankAccountDto bankAccountDto, Guid userId);
    }
}
