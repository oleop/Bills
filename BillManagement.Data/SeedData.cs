using BillManagement.Data.Models;

namespace BillManagement.Data;

public class SeedData
{
    public static async Task Initialize(BillDbContext context)
    {
        if (!context.Users.Any())
        {
            List<User> users = new List<User>
            {
                new()
                {
                    Username = "john",
                    Password = "pssshash123",
                    Bills = new List<Bill>
                    {
                        new() { Title = "Bill #1", TotalAmount = 10000 },
                        new() { Title = "Bill #2", TotalAmount = 11000 },
                        new() { Title = "Bill #3", TotalAmount = 12000 },
                        new() { Title = "Bill #4", TotalAmount = 13000 },
                        new() { Title = "Bill #5", TotalAmount = 14000 }
                    }
                },
                new()
                {
                    Username = "sarah",
                    Password = "securepass",
                    Bills = new List<Bill>
                    {
                        new() { Title = "Bill #6", TotalAmount = 15000 },
                        new() { Title = "Bill #7", TotalAmount = 16000 },
                        new() { Title = "Bill #8", TotalAmount = 17000 },
                        new() { Title = "Bill #9", TotalAmount = 18000 },
                        new() { Title = "Bill #10", TotalAmount = 19000 }
                    }
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
        }
    }
}