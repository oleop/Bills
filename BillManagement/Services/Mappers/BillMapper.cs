using BillManagement.Data.Models;
using BillManagement.Services.DTO;

namespace BillManagement.Services.Mappers;

public static class BillMapper
{
    public static BillResponse map(Bill bill)
    {
        return new BillResponse()
        {
            Id = bill.Id, 
            Title = bill.Title,
            Settled = bill.Settled,
            SettledAt = bill.SettledAt,
            TotalAmount = bill.TotalAmount,
            UserId = bill.User.Id,
            Payments = bill.Payments.Select(PaymentMapper.map)
        };
    }
}