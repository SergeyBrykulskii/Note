using Note.Domain.Dto;
using Note.Domain.Dto.User;
using Note.Domain.Result;

namespace Note.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto);
    Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto);
}
