using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankingApp.Models.DTOs;
using BankingApp.Interfaces;


namespace BankingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
       [HttpPost("credit")]
public async Task<IActionResult> CreditAmount([FromBody] TransferRequestDto transferRequest)
{
    if (transferRequest == null ||
        string.IsNullOrWhiteSpace(transferRequest.FromAccountNumber) ||
        string.IsNullOrWhiteSpace(transferRequest.ToAccountNumber) ||
        transferRequest.Amount <= 0)
    {
        return BadRequest("Invalid account number or amount.");
    }

    try
    {
        var transaction = await _transactionService.CreditAmountAsync(transferRequest);

        return Ok(transaction);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}


        // [HttpPost("debit")]
        // public async Task<IActionResult> DebitAmount([FromBody] TransferRequestDto transferRequest)
        // {
        //     if (transferRequest == null || transferRequest.Amount <= 0)
        //     {
        //         return BadRequest("Invalid transfer request.");
        //     }

        //     var transaction = await _transactionService.DebitAmountAsync(transferRequest.FromAccountId, transferRequest.ToAccountId, transferRequest.Amount);
        //     return Ok(transaction);
        // }

        [HttpGet("all-transfers")]
        public async Task<IActionResult> GetAllTransfers()
        {
            var transactions = await _transactionService.GetAllTransfersAsync();
            return Ok(transactions);
        }

        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransferById(int transactionId)
        {
            var transaction = await _transactionService.GetTransferByIdAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }
    }
}
