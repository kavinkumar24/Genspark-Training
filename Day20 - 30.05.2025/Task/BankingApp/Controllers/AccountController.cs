using BankingApp.Interfaces;
using BankingApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("GetAccountsByCustomerId/{customerId}")]
        public async Task<IActionResult> GetAccountsByCustomerId(int customerId)
        {
            try
            {
                var accounts = await _accountService.GetAccountsByCustomerId(customerId);
                if (accounts == null || !accounts.Any())
                {
                    return NotFound($"No accounts found for customer with ID {customerId}");
                }
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAllAccounts();
                if (accounts == null || !accounts.Any())
                {
                    return NotFound("No accounts found");
                }
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        // [HttpPut("UpdateAccount/{id}")]
        // public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account account)
        // {
        //     if (account == null)
        //     {
        //         return BadRequest("Account data is null");
        //     }

        //     try
        //     {
        //         var updatedAccount = await _accountService.UpdateAccount(id, account);
        //         if (updatedAccount == null)
        //         {
        //             return NotFound($"Account with ID {id} not found");
        //         }
        //         return Ok(updatedAccount);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
        //     }
        // }

        [HttpDelete("DeleteAccount/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var account = await _accountService.DeleteAccount(id);
                if (account == null)
                {
                    return NotFound($"Account with ID {id} not found");
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


    }
}
