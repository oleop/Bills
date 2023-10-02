using BillManagement.Services.DTO;

namespace BillManagement.Services.Interfaces;

public interface IBillService
{
    Task<IEnumerable<BillResponse>> GetBills();
    Task<IEnumerable<BillResponse>> GetBills(long userId);
}