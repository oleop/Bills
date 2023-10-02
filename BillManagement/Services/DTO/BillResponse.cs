namespace BillManagement.Services.DTO;

public class BillResponse
{
    public long Id { get; set; }
    public string Title { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Settled { get; set; }
    public DateTime? SettledAt { get; set; }
    public long UserId { get; set; }
    public IEnumerable<PaymentResponse> Payments { get; set; }
}