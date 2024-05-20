using Microsoft.AspNetCore.Mvc;
using Note.Domain.Dto;
using Note.Domain.Dto.User;
using Note.Domain.Interfaces.Services;
using Note.Domain.Result;

namespace Note.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto registerUserDto)
        {

            var response = await _authService.Register(registerUserDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto loginUserDto)
        {
            var response = await _authService.Login(loginUserDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
