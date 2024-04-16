using AutoMapper;
using Note.Domain.Dto.Report;
using Note.Domain.Entity;

namespace Note.Application.Mapping;

public class ReportMapping : Profile
{
    public ReportMapping()
    {
        CreateMap<Report, ReportDto>().ReverseMap();
    }
}
