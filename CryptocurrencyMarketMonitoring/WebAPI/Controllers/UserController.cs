using CryptocurrencyMarketMonitoring.Abstractions.Services;
using CryptocurrencyMarketMonitoring.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var response = await _userService.LoginAsync(login);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            await _userService.CreateAsync(user);
            return Ok();
        }


        private IUserService _userService;

    }
}