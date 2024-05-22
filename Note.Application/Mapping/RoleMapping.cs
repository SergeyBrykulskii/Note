using AutoMapper;
using Note.Domain.Dto.Role;
using Note.Domain.Entity;

namespace Note.Application.Mapping;

public class RoleMapping : Profile
{
    public RoleMapping()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}
