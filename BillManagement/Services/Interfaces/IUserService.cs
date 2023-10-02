using BillManagement.Services.DTO;

namespace BillManagement.Services.Interfaces;

public interface IUserService
{
    public Task<UserCreationResult> CreateUser(string username, string password);
    public Task<IEnumerable<UserResponse>> GetUsers();
}