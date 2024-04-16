using Note.Domain.Dto.Report;
using Note.Domain.Result;

namespace Note.Domain.Interfaces.Services;

public interface IReportService
{
    Task<CollectionResult<ReportDto>> GetReportsAsync(long userId);

    Task<BaseResult<ReportDto>> GetReportByIdAsync(long id);

    Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto);

    Task<BaseResult<ReportDto>> DeleteReportAsync(long id);

    Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto reportDto);

}
