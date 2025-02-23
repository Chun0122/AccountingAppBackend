using AccountingAppBackend.DataAccess;
using AccountingAppBackend.Services.INF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AccountingAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DropdownOptionsController : ControllerBase
    {
        private readonly IDropdownOptionsService _dropdownOptionsService;

        public DropdownOptionsController(IDropdownOptionsService dropdownOptionsService)
        {
            _dropdownOptionsService = dropdownOptionsService;
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _dropdownOptionsService.GetCategoriesAsync();
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("Subcategories")]
        public async Task<IActionResult> GetSubcategories()
        {
            var response = await _dropdownOptionsService.GetSubcategoriesAsync();
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("PaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var response = await _dropdownOptionsService.GetPaymentMethodsAsync();
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }

        [HttpGet("Currencies")]
        public async Task<IActionResult> GetCurrencies()
        {
            var response = await _dropdownOptionsService.GetCurrenciesAsync();
            if (!response.Success)
            {
                return StatusCode(500, response);
            }
            return Ok(response);
        }
    }
}