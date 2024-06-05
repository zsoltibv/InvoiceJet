using InvoiceJet.Application.DTOs;
using InvoiceJet.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceJet.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class BankAccountController : ControllerBase
{
    private readonly IBankAccountService _bankAccountService;

    public BankAccountController(IBankAccountService bankAccountService)
    {
        _bankAccountService = bankAccountService;
    }

    [HttpGet("GetUserFirmBankAccounts/{userId}")]
    public async Task<ActionResult<FirmDto>> GetUserFirmBankAccounts(Guid userId)
    {
        var bankAccountDto = await _bankAccountService.GetUserFirmBankAccounts(userId);
        return Ok(bankAccountDto);
    }

    [HttpPut("AddOrEditBankAccount/{userId}")]
    public async Task<ActionResult<FirmDto>> AddOrEditBankAccount(BankAccountDto bankAccountDto, Guid userId)
    {
        try
        {
            var updatedOrNewBankAccount = await _bankAccountService.AddOrEditBankAccount(bankAccountDto, userId);
            return Ok(updatedOrNewBankAccount);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}