using BillManagement.Services.DTO;
using BillManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BillManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost()]
        public async Task<IActionResult> Create(CreateUserRequest createUserRequest)
        {
            UserCreationResult result =
                await _userService.CreateUser(createUserRequest.Username, createUserRequest.Password);
            if (result == UserCreationResult.Conflict)
            {
                return Conflict();
            }

            return Ok();
        }
        
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            IEnumerable<UserResponse> users = await _userService.GetUsers();

            return Ok(users);
        }
    }
}