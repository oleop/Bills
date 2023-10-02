using BillManagement.Data.Models;
using BillManagement.Services.DTO;

namespace BillManagement.Services.Mappers;

public static class PaymentMapper
{
    public static PaymentResponse map(Payment payment)
    {
        return new PaymentResponse
        {
            Id = payment.Id,
            BillId = payment.Bill.Id,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            UserId = payment.User?.Id
        };
    }
}