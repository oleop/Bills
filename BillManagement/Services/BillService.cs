using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using BillManagement.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Services;

public class BillService : IBillService
{
    private readonly BillDbContext _billDbContext;

    public BillService(BillDbContext billDbContext)
    {
        _billDbContext = billDbContext;
    }

    public async Task<IEnumerable<BillResponse>> GetBills()
    {
        List<Bill> bills = await _billDbContext.Bills.Include(b => b.User).Include(b => b.Payments)
            .ThenInclude(p => p.User).ToListAsync();
        return bills.Select(BillMapper.map);
    }

    public async Task<IEnumerable<BillResponse>> GetBills(long userId)
    {
        if (!await _billDbContext.Bills.AnyAsync(u => u.User.Id == userId))
            return null;
        var bills = await _billDbContext.Bills.Include(b => b.User).Include(b => b.Payments)
            .ThenInclude(p => p.User)
            .Where(b => b.User.Id == userId).ToListAsync();
        return bills.Select(BillMapper.map);
    }
}