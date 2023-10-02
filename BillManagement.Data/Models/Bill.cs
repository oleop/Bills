namespace BillManagement.Data.Models;

public class Bill
{
    public long Id { get; set; }
    public string Title { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Settled { get; set; }
    public DateTime? SettledAt { get; set; }

    public virtual User User { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}