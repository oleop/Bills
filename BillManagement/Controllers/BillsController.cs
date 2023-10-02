using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<BillResponse> bills = await _billService.GetBills();
            return Ok(bills);
        }
        
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(long userId)
        {
            IEnumerable<BillResponse> bills = await _billService.GetBills(userId);
            if (bills == null)
                return new NotFoundObjectResult(new { error = $"UserId {userId} does not exist." });
            return Ok(bills);
        }
    }
}