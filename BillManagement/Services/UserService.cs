using BillManagement.Data;
using BillManagement.Data.Models;
using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using BillManagement.Services.Mappers;
using Microsoft.EntityFrameworkCore;

namespace BillManagement.Services;

public class UserService : IUserService
{
    private readonly BillDbContext _billDbContext;
    private readonly IHashService _hashService;

    public UserService(BillDbContext billDbContext, IHashService hashService)
    {
        _billDbContext = billDbContext;
        _hashService = hashService;
    }

    public async Task<UserCreationResult> CreateUser(string username, string password)
    {
        if (await _billDbContext.Users.AnyAsync(x => x.Username == username))
        {
            return UserCreationResult.Conflict;
        }

        _billDbContext.Users.Add(new User
        {
            Username = username,
            Password = _hashService.Hash(password)
        });

        await _billDbContext.SaveChangesAsync();

        return UserCreationResult.Success;
    }

    public async Task<IEnumerable<UserResponse>> GetUsers()
    {
        List<UserResponse> users = await _billDbContext.Users.Select(u => UserMapper.map(u)).ToListAsync();
        return users;
    }
}