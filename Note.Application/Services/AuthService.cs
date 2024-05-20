using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Note.Application.Resources;
using Note.Domain.Dto;
using Note.Domain.Dto.User;
using Note.Domain.Entity;
using Note.Domain.Enum;
using Note.Domain.Interfaces.Repositories;
using Note.Domain.Interfaces.Services;
using Note.Domain.Result;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Note.Application.Services;

public class AuthService : IAuthService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserToken> _userTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    public AuthService(IBaseRepository<User> userRepository, ILogger logger, IMapper mapper,
        IBaseRepository<UserToken> userTokenRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _mapper = mapper;
        _userTokenRepository = userTokenRepository;
        _tokenService = tokenService;
    }
    public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Login == loginUserDto.Login);

            if (user == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }

            if (!IsVerifiedPassword(user.Password, loginUserDto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.WrongPassword,
                    ErrorCode = (int)ErrorCodes.WrongPassword
                };
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, "User")
            };

            var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var accessToken = _tokenService.GenerateAccessToken(claims);

            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                };

                await _userTokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userTokenRepository.UpdateAsync(userToken);
            }

            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        if (registerUserDto.Password != registerUserDto.PasswordConfirm)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.PasswordNotEqual,
                ErrorCode = (int)ErrorCodes.PasswordNotEqual

            };
        }

        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Login == registerUserDto.Login);

            if (user != null)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.UserAlreadyExist,
                    ErrorCode = (int)ErrorCodes.UserAlreadyExist
                };
            }

            var hashUserPassword = HashPassword(registerUserDto.Password);

            user = new User()
            {
                Login = registerUserDto.Login,
                Password = hashUserPassword,
            };

            await _userRepository.CreateAsync(user);

            return new BaseResult<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes).ToLower();
    }

    private bool IsVerifiedPassword(string userPasswordHash, string userPassword)
    {
        var hash = HashPassword(userPassword);

        return hash.Equals(userPasswordHash);
    }
}
