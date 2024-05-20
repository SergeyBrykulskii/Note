using AutoMapper;
using Note.Domain.Dto.User;
using Note.Domain.Entity;

namespace Note.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
