using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Note.Application.Resources;
using Note.Domain.Dto.Role;
using Note.Domain.Dto.UserRole;
using Note.Domain.Entity;
using Note.Domain.Enum;
using Note.Domain.Interfaces.Repositories;
using Note.Domain.Interfaces.Services;
using Note.Domain.Interfaces.UnitOfWork;
using Note.Domain.Result;
using System.Data;

namespace Note.Application.Services;

public class RoleService : IRoleService
{
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IBaseRepository<Role> roleRepository, IBaseRepository<User> userRepository,
        IMapper mapper, IBaseRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
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
            await _userRoleRepository.SaveChangesAsync();

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

        _roleRepository.Remove(role);
        await _roleRepository.SaveChangesAsync();

        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto userRoleDto)
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

        var role = user.Roles?.FirstOrDefault(x => x.Id == userRoleDto.RoleId);

        if (role == null)
        {
            return new BaseResult<UserRoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        var userRole = await _userRoleRepository.GetAll().
            Where(x => x.UserId == user.Id)
            .FirstOrDefaultAsync(x => x.RoleId == role.Id);

        if (userRole == null)
        {
            return new BaseResult<UserRoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        _userRoleRepository.Remove(userRole);
        await _userRoleRepository.SaveChangesAsync();

        return new BaseResult<UserRoleDto>
        {
            Data = new UserRoleDto
            {
                Login = user.Login,
                RoleName = role.Name,
            }
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

        var updatedRole = _roleRepository.Update(role);
        await _roleRepository.SaveChangesAsync();

        return new BaseResult<RoleDto>
        {
            Data = _mapper.Map<RoleDto>(updatedRole)
        };
    }

    public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto userRoleDto)
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

        var role = user.Roles?.FirstOrDefault(x => x.Id == userRoleDto.OldRoleId);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        var newRole = await _roleRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Id == userRoleDto.NewRoleId);
        if (newRole == null)
        {
            return new BaseResult<UserRoleDto>
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                var userRole = await _unitOfWork.UserRoleRepository.GetAll().
                    Where(x => x.UserId == user.Id)
                    .FirstAsync(x => x.RoleId == role.Id);

                _unitOfWork.UserRoleRepository.Remove(userRole);

                var newUserRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = newRole.Id,
                };

                await _unitOfWork.UserRoleRepository.CreateAsync(newUserRole);

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }
        throw new NotImplementedException();
    }
}
