using BillManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Data;

public class BillDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<Payment> Payments { get; set; }


    public BillDbContext(DbContextOptions<BillDbContext> options)
        : base(options)
    {

    }
}