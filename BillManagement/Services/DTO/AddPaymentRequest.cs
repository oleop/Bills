using BillManagement.Data.Models;

namespace BillManagement.Services.DTO;

public class AddPaymentRequest
{
    public long BillId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public long? UserId { get; set; }
}