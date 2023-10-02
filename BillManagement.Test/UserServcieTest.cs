using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services;
using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace BillManagement.Test;

public class UserServiceTest : IDisposable
{
    private readonly BillDbContext _billDbContext;
    private readonly Mock<IHashService> _hashService;
    private readonly IUserService _subject;

    public UserServiceTest()
    {

        _billDbContext = new BillDbContext(new DbContextOptionsBuilder<BillDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options);

        _hashService = new Mock<IHashService>();
        _subject = new UserService(_billDbContext, _hashService.Object);
    }

    [Fact]
    public async Task CreateUser_WithNonExistingUsername_ShouldReturnSuccess()
    {
        // Arrange
        string username = "newuser";
        string password = "password123";
        string passwordHash = "passwordhash";
        _hashService.Setup(x=>x.Hash(password)).Returns(passwordHash);

        // Act
        var result = await _subject.CreateUser(username, password);

        // Assert
        User user = await _billDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        Assert.NotNull(user);
        Assert.Equal(username, user.Username);
        Assert.Equal(passwordHash, user.Password);
        Assert.Equal(UserCreationResult.Success, result);
    }

    [Fact]
    public async Task CreateUser_WithExistingUsername_ShouldReturnConflict()
    {
        // Arrange
        string existingUsername = "existinguser";
        string password = "password123";

        _billDbContext.Users.Add(new User { Username = existingUsername, Password = password });
        _billDbContext.SaveChanges();

        // Act
        var result = await _subject.CreateUser(existingUsername, password);

        // Assert
        Assert.Equal(UserCreationResult.Conflict, result);
    }


    public void Dispose() => _billDbContext.Dispose();
}
