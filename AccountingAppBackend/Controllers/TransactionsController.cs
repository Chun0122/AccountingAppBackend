using AccountingAppBackend.DataAccess;
using AccountingAppBackend.DataAccess.Models;
using AccountingAppBackend.Models;
using AccountingAppBackend.Models.DTO;
using AccountingAppBackend.Services.INF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            uint userId = (uint)Util.GetCurrentUserId(HttpContext.User);
            var response = await _transactionService.GetTransactionsAsync(userId);
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpPost("CreateTransaction")]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<Transaction>(false, null, "資料驗證失敗"));
            }

            uint userId = (uint)Util.GetCurrentUserId(HttpContext.User);
            var response = await _transactionService.CreateTransactionAsync(request, userId);
            if (!response.Success)
            {
                return StatusCode(500, response);
            }

            // 利用 CreatedAtAction 回傳 201 Created，並提供新資源的 URL
            return CreatedAtAction(nameof(GetTransaction), new { id = response.Data?.TransactionId }, response);
        }

        [HttpPut("UpdateTransaction/{transactionId}")]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, null, "資料驗證失敗"));
            }

            var response = await _transactionService.UpdateTransactionAsync(transactionId, request);
            if (!response.Success)
            {
                if (response.Message.Contains("找不到"))
                    return NotFound(response);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteTransaction/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var response = await _transactionService.DeleteTransactionAsync(transactionId);
            if (!response.Success)
            {
                if (response.Message.Contains("找不到"))
                    return NotFound(response);
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var response = await _transactionService.GetTransactionByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}