using BillManagement.Services.DTO;

namespace BillManagement.Services.Interfaces;

public interface IPaymentService
{
    Task<AddPaymentResult> AddPayment(AddPaymentRequest request);
}