using Note.Domain.Dto;
using Note.Domain.Result;
using System.Security.Claims;

namespace Note.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    Task<BaseResult<TokenDto>> RefreshToken(TokenDto tokenDto);
}
