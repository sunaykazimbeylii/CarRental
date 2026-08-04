using CarRental.Application.DTOs.AppUser;
using CarRental.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AccountsController(IAuthenticationService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto userDto)
        {
            await _service.RegisterAsync(userDto);
            return Created();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {

            return Ok(await _service.LoginAsync(userDto));
        }
    }
}
