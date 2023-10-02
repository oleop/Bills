using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddPaymentRequest request)
        {
            AddPaymentResult result = await _paymentService.AddPayment(request);
            switch (result)
            {
                case AddPaymentResult.Success:
                    return Ok();
                case AddPaymentResult.BillAlreadySettled:
                    return BadRequest("Bill already settled.");
                case AddPaymentResult.BillNotFound:
                    return NotFound("Bill not found.");
                default:
                    throw new ArgumentOutOfRangeException();
            } 
        }
    }
}