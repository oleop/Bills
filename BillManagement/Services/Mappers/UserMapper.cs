using BillManagement.Data.Models;
using BillManagement.Services.DTO;

namespace BillManagement.Services.Mappers;

public static class UserMapper
{
    public static UserResponse map(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            UserName = user.Username
        };
    }
}