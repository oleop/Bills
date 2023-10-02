using BillManagement.Data;
using BillManagement.Services;
using BillManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
string path = Environment.GetFolderPath(folder);

builder.Services.AddDbContext<BillDbContext>(options =>
{
    options.UseSqlite($"Data Source={Path.Join(path, "bills.db")}");
});

builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    BillDbContext context = serviceScope.ServiceProvider.GetRequiredService<BillDbContext>();
    await context.Database.MigrateAsync();
    await SeedData.Initialize(context);
}

app.Run();
