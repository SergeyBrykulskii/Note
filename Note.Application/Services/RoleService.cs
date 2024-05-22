using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Note.Application.Resources;
using Note.Domain.Dto.Role;
using Note.Domain.Entity;
using Note.Domain.Enum;
using Note.Domain.Interfaces.Repositories;
using Note.Domain.Interfaces.Services;
using Note.Domain.Result;

namespace Note.Application.Services;

public class RoleService : IRoleService
{
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IMapper _mapper;

    public RoleService(IBaseRepository<Role> roleRepository, IBaseRepository<User> userRepository,
        IMapper mapper, IBaseRepository<UserRole> userRoleRepository)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto)
    {
        var user = await _userRepository.GetAll()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Login == userRoleDto.Login);

        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();

        if (roles.All(x => x != userRoleDto.RoleName))
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == userRoleDto.RoleName);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }
            var userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };

            await _userRoleRepository.CreateAsync(userRole);

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto()
                {
                    Login = user.Login,
                    RoleName = role.Name,
                }
            };
        }

        return new BaseResult<UserRoleDto>()
        {
            ErrorMessage = ErrorMessage.UserAlreadyHasRole,
            ErrorCode = (int)ErrorCodes.UserAlreadyHasRole
        };
    }

    public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto roleDto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == roleDto.Name);

        if (role != null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists
            };
        }

        role = new()
        {
            Name = roleDto.Name,
        };

        await _roleRepository.CreateAsync(role);

        return new BaseResult<RoleDto>
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

        if (role == null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        await _roleRepository.RemoveAsync(role);

        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto roleDto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == roleDto.Id);

        if (role == null)
        {
            return new BaseResult<RoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        role.Name = roleDto.Name;

        await _roleRepository.UpdateAsync(role);
        return new BaseResult<RoleDto>
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }
}
