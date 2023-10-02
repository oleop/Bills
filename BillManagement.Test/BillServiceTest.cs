using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services;
using BillManagement.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Test
{
    public class BillServiceTest : IDisposable
    {
        private readonly BillDbContext _billDbContext;
        private readonly BillService _subject;

        public BillServiceTest()
        {
            _billDbContext = new BillDbContext(new DbContextOptionsBuilder<BillDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

            _subject = new BillService(_billDbContext);
        }

        [Fact]
        public async Task GetBills_WhenBillsExist_ShouldReturnMappedBillResponses()
        {
            // Arrange
            User user1 = new User
            {
                Username = "user1",
                Password = "pass1"
            };

            User user2 = new User
            {
                Username = "user2",
                Password = "pass2"
            };
            Bill bill1 = new Bill
            {
                Id = 1,
                Title = "Bill 1",
                TotalAmount = 100,
                Settled = true,
                SettledAt = DateTime.UtcNow,
                Payments = new List<Payment>
                {
                    new Payment { User = user1 }
                },
                User = user1
            };

            Bill bill2 = new Bill
            {
                Id = 2,
                Title = "Bill 2",
                TotalAmount = 200,
                Settled = false,
                SettledAt = null,
                Payments = new List<Payment>
                {
                    new Payment { User = user2 }
                },
                User = user2
            };
            await _billDbContext.Users.AddRangeAsync(user1, user2);
            await _billDbContext.Bills.AddRangeAsync(bill1, bill2);
            await _billDbContext.SaveChangesAsync();

            // Act
            IEnumerable<BillResponse> result = await _subject.GetBills();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            BillResponse billResponse1 = result.FirstOrDefault(b => b.Id == 1);
            BillResponse billResponse2 = result.FirstOrDefault(b => b.Id == 2);

            Assert.NotNull(billResponse1);
            Assert.NotNull(billResponse2);

            Assert.Equal(bill1.Id, billResponse1.Id);
            Assert.Equal(bill1.Title, billResponse1.Title);
            Assert.Equal(bill1.Settled, billResponse1.Settled);
            Assert.Equal(bill1.SettledAt, billResponse1.SettledAt);
            Assert.Equal(bill1.TotalAmount, billResponse1.TotalAmount);

            Assert.Equal(bill2.Id, billResponse2.Id);
            Assert.Equal(bill2.Title, billResponse2.Title);
            Assert.Equal(bill2.Settled, billResponse2.Settled);
            Assert.Equal(bill2.SettledAt, billResponse2.SettledAt);
            Assert.Equal(bill2.TotalAmount, billResponse2.TotalAmount);
        }

        public void Dispose() => _billDbContext.Dispose();
    }
}
