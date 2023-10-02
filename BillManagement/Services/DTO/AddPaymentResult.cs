namespace BillManagement.Services.DTO;

public enum AddPaymentResult
{
    Success, 
    BillAlreadySettled,
    BillNotFound
}