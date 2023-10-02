namespace BillManagement.Data.Models;

public class Payment
{
    public long Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    public virtual User User { get; set; }
    public virtual Bill Bill { get; set; }
}