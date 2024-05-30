using AutoMapper;
using InvoiceJetAPI.Config;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJetAPI.Services.Impl
{
    public class BankAccountService : IBankAccountService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IMapper _mapper;

        public BankAccountService(FacturilaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<BankAccountDto>> GetUserFirmBankAccounts(Guid userId)
        {
            var activeUserFirm = await _dbContext.User
                   .Where(u => u.Id == userId)
                   .Include(u => u.ActiveUserFirm)
                        .ThenInclude(uf => uf.BankAccounts)
                   .Select(u => u.ActiveUserFirm)
                   .FirstOrDefaultAsync();

            if (activeUserFirm == null)
            {
                return new List<BankAccountDto>();
            }

            var bankAccountDtos = _mapper.Map<List<BankAccountDto>>(activeUserFirm.BankAccounts);
            return bankAccountDtos;
        }

        public async Task<BankAccountDto> AddOrEditBankAccount(BankAccountDto bankAccountDto, Guid userId)
        {
            BankAccount bankAccount;

            if (bankAccountDto.Id != 0)
            {
                bankAccount = await _dbContext.BankAccount.FirstOrDefaultAsync(ba => ba.Id == bankAccountDto.Id);
                if (bankAccount == null)
                {
                    throw new Exception("Bank account not found.");
                }
                _mapper.Map(bankAccountDto, bankAccount);
            }
            else
            {
                bankAccount = _mapper.Map<BankAccount>(bankAccountDto);
                var activeUserFirm = await _dbContext.User
                    .Where(u => u.Id == userId)
                        .Include(u => u.ActiveUserFirm)
                    .Select(u => u.ActiveUserFirm)
                    .FirstOrDefaultAsync();

                bankAccount.UserFirmId = activeUserFirm!.UserFirmId;
                await _dbContext.BankAccount.AddAsync(bankAccount);
            }
            if (bankAccount.IsActive)
            {
                var otherAccounts = await _dbContext.BankAccount
                    .Where(ba => ba.UserFirmId == bankAccount.UserFirmId && ba.Id != bankAccount.Id)
                    .ToListAsync();

                foreach (var account in otherAccounts)
                {
                    account.IsActive = false;
                }
            }
            
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BankAccountDto>(bankAccount);
        }
    }
}
