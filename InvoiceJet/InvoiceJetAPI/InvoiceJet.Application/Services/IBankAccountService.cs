using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IBankAccountService
{
    Task<ICollection<BankAccountDto>> GetUserFirmBankAccounts(Guid userId);
    Task<BankAccountDto> AddOrEditBankAccount(BankAccountDto bankAccountDto, Guid userId);
}