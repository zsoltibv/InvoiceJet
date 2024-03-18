using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl
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
            var user = await _dbContext.User
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var userFirm = await _dbContext.UserFirm
                .Include(ba => ba.BankAccounts)
                .FirstOrDefaultAsync(uf => uf.UserId.Equals(userId) && uf.FirmId == user.ActiveFirmId && !uf.IsClient);

            if (userFirm == null)
            {
                throw new Exception("The UserFirm relationship was not found.");
            }

            var bankAccountDtos = _mapper.Map<List<BankAccountDto>>(userFirm.BankAccounts);
            return bankAccountDtos;
        }

        public async Task<BankAccountDto> AddOrEditBankAccount(BankAccountDto bankAccountDto)
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
                //add userFirm id
                await _dbContext.BankAccount.AddAsync(bankAccount);
            }
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BankAccountDto>(bankAccount);
        }
    }
}
