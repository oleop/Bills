using BillManagement.Data.Models;

namespace BillManagement.Services.DTO;

public class PaymentResponse
{
    public long Id { get; set; }
    public long BillId { get; set; }
    public decimal Amount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime PaymentDate { get; set; }
    public long? UserId { get; set; }
}