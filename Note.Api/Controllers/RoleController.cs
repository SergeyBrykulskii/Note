using Microsoft.AspNetCore.Mvc;
using Note.Domain.Dto.Role;
using Note.Domain.Interfaces.Services;
using Note.Domain.Result;
using System.Net.Mime;

namespace Note.Api.Controllers;

[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Create([FromBody] CreateRoleDto roleDto)
    {
        var response = await _roleService.CreateRoleAsync(roleDto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpDelete(nameof(id))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Delete(long id)
    {
        var response = await _roleService.DeleteRoleAsync(id);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<RoleDto>>> Update([FromBody] UpdateRoleDto roleDto)
    {
        var response = await _roleService.UpdateRoleAsync(roleDto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpPost("addRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<UserRoleDto>>> AddRoleForUser([FromBody] UserRoleDto userRoleDto)
    {
        var response = await _roleService.AddRoleForUserAsync(userRoleDto);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}
