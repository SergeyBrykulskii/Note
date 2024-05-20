using Microsoft.AspNetCore.Mvc;
using Note.Domain.Dto;
using Note.Domain.Interfaces.Services;
using Note.Domain.Result;

namespace Note.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
    {
        var response = await _tokenService.RefreshToken(tokenDto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

}
