using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class BankAccountService : IBankAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BankAccountService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<BankAccountDto>> GetUserFirmBankAccounts(Guid userId)
    {
        var activeUserFirm = await _unitOfWork.Users.Query()
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
            bankAccount = await _unitOfWork.BankAccounts.Query().FirstOrDefaultAsync(ba => ba.Id == bankAccountDto.Id);
            if (bankAccount == null)
            {
                throw new Exception("Bank account not found.");
            }

            _mapper.Map(bankAccountDto, bankAccount);
        }
        else
        {
            bankAccount = _mapper.Map<BankAccount>(bankAccountDto);
            var activeUserFirm = await _unitOfWork.Users.Query()
                .Where(u => u.Id == userId)
                .Include(u => u.ActiveUserFirm)
                .Select(u => u.ActiveUserFirm)
                .FirstOrDefaultAsync();

            bankAccount.UserFirmId = activeUserFirm!.UserFirmId;
            await _unitOfWork.BankAccounts.AddAsync(bankAccount);
        }

        if (bankAccount.IsActive)
        {
            var otherAccounts = await _unitOfWork.BankAccounts.Query()
                .Where(ba => ba.UserFirmId == bankAccount.UserFirmId && ba.Id != bankAccount.Id)
                .ToListAsync();

            foreach (var account in otherAccounts)
            {
                account.IsActive = false;
            }
        }

        await _unitOfWork.CompleteAsync();
        return _mapper.Map<BankAccountDto>(bankAccount);
    }
}