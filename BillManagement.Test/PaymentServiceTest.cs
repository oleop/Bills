using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services;
using BillManagement.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Test
{
    public class PaymentServiceTests : IDisposable
    {
        private readonly BillDbContext _billDbContext;

        private readonly PaymentService _subject;

        public PaymentServiceTests()
        {
            _billDbContext = new BillDbContext(new DbContextOptionsBuilder<BillDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

            _subject = new PaymentService(_billDbContext);
        }

        [Fact]
        public async Task AddPayment_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            User user = new User
            {
                Username = "testuser",
                Password = "pass1"
            };

            Bill bill = new Bill
            {
                Id = 1,
                Title = "Title 1",
                TotalAmount = 500,
                Settled = false,
                Payments = new List<Payment>
    {
        new Payment
        {
            Amount = 50,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = PaymentMethod.BankTransfer,
            User = user
        }
    }
            };

            await _billDbContext.Users.AddAsync(user);
            await _billDbContext.Bills.AddAsync(bill);
            await _billDbContext.SaveChangesAsync();

            AddPaymentRequest request = new AddPaymentRequest
            {
                BillId = 1,
                UserId = 1,
                Amount = 200,
                PaymentMethod = PaymentMethod.CreditCard
            };

            // Act
            AddPaymentResult result = await _subject.AddPayment(request);

            // Assert
            Assert.Equal(AddPaymentResult.Success, result);

            // Verify that the payment was added to the bill
            var updatedBill = await _billDbContext.Bills.Include(b => b.Payments).FirstOrDefaultAsync(b => b.Id == 1);
            Assert.NotNull(updatedBill);
            Assert.False(updatedBill.Settled);
            Assert.Null(updatedBill.SettledAt);
            Assert.Equal(2, updatedBill.Payments.Count);
            Assert.Equal(200, updatedBill.Payments.Last().Amount);
        }

        [Fact]
        public async Task AddPayment_WithInvalidBillId_ShouldReturnBillNotFound()
        {
            // Arrange
            AddPaymentRequest request = new AddPaymentRequest
            {
                BillId = 99, // Invalid BillId
                UserId = 23,
                Amount = 200,
                PaymentMethod = PaymentMethod.CreditCard
            };

            // Act
            AddPaymentResult result = await _subject.AddPayment(request);

            // Assert
            Assert.Equal(AddPaymentResult.BillNotFound, result);
        }

        [Fact]
        public async Task AddPayment_WithValidData_ShouldSettleBillIfTotalAmountReached()
        {
            // Arrange
            User user = new User
            {
                Username = "testuser",
                Password = "pass1"
            };

            Bill bill = new Bill
            {
                Id = 1,
                Title = "Title 1",
                TotalAmount = 500,
                Settled = false,
                Payments = new List<Payment>
    ()
            };

            await _billDbContext.Users.AddAsync(user);
            await _billDbContext.Bills.AddAsync(bill);
            await _billDbContext.SaveChangesAsync();

          
            AddPaymentRequest request = new AddPaymentRequest
            {
                BillId = 1,
                UserId = 34,
                Amount = 500, // Equal to the bill's TotalAmount
                PaymentMethod = PaymentMethod.EmailTransfer
            };

            // Act
            AddPaymentResult result = await _subject.AddPayment(request);

            // Assert
            Assert.Equal(AddPaymentResult.Success, result);

            Bill? updatedBill = await _billDbContext.Bills.Include(b => b.Payments).FirstOrDefaultAsync(b => b.Id == 1);
            Assert.NotNull(updatedBill);
            Assert.True(updatedBill.Settled);
            Assert.NotNull(updatedBill.SettledAt);
            Assert.Single(updatedBill.Payments);
            Assert.Equal(500, updatedBill.Payments.First().Amount);
        }

        public void Dispose() => _billDbContext.Dispose();
    }
}
