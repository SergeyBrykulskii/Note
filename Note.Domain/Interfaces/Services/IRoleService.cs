using Note.Domain.Dto.Role;
using Note.Domain.Result;

namespace Note.Domain.Interfaces.Services;

public interface IRoleService
{
    Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto roleDto);
    Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);
    Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto roleDto);
    Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto);
}
