using Note.Domain.Dto;
using Note.Domain.Result;

namespace Note.Domain.Interfaces.Services;

public interface IReportService
{
    Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);
}
