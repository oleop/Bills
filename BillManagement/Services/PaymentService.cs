using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Services;

public class PaymentService : IPaymentService
{
    private readonly BillDbContext _billDbContext;

    public PaymentService(BillDbContext billDbContext)
    {
        _billDbContext = billDbContext;
    }

    public async Task<AddPaymentResult> AddPayment(AddPaymentRequest request)
    {
        Bill bill = await _billDbContext.Bills.Include(b => b.Payments)
            .FirstOrDefaultAsync(b => b.Id == request.BillId);
        if (bill == null)
        {
            return AddPaymentResult.BillNotFound;
        }

        if (bill.Settled)
        {
            return AddPaymentResult.BillAlreadySettled;
        }

        User user = await _billDbContext.Users.FindAsync(request.UserId);

        Payment payment = new()
        {
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            User = user,
            PaymentDate = DateTime.UtcNow
        };

        Decimal billAmount = bill.Payments.Sum(p => p.Amount);
        bill.Settled = billAmount + request.Amount >= bill.TotalAmount;
        if (bill.Settled)
        {
            bill.SettledAt = DateTime.UtcNow;
        }

        bill.Payments.Add(payment);
        await _billDbContext.SaveChangesAsync();

        return AddPaymentResult.Success;
    }
}