using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Note.Application.Resources;
using Note.Domain.Dto.Report;
using Note.Domain.Entity;
using Note.Domain.Enum;
using Note.Domain.Interfaces.Repositories;
using Note.Domain.Interfaces.Services;
using Note.Domain.Interfaces.Validations;
using Note.Domain.Result;
using Serilog;

namespace Note.Application.Services;

public class ReportService : IReportService
{
    private readonly IBaseRepository<Report> _reportRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly ILogger _logger;
    private readonly IReportValidator _reportValidator;
    private readonly IMapper _mapper;

    public ReportService(IBaseRepository<Report> reportRepository, ILogger logger, IBaseRepository<User> userRepository,
        IReportValidator reportValidator, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _logger = logger;
        _userRepository = userRepository;
        _reportValidator = reportValidator;
        _mapper = mapper;
    }

    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto reportDto)
    {
        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == reportDto.UserId);
            var report = await _reportRepository.GetAll().FirstOrDefaultAsync(r => r.Name == reportDto.Name);
            var result = _reportValidator.CreateValidator(report, user);

            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            report = new Report()
            {
                Name = reportDto.Name,
                Description = reportDto.Description,
                UserId = reportDto.UserId,
            };

            await _reportRepository.CreateAsync(report);

            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };

        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
    {
        try
        {
            var report = await _reportRepository.GetAll().FirstOrDefaultAsync(r => r.Id == id);
            var result = _reportValidator.ValidateOnNull(report);

            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            await _reportRepository.RemoveAsync(report);

            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<ReportDto>> GetReportByIdAsync(long id)
    {
        ReportDto? report;

        try
        {
            report = /*await*/ _reportRepository.GetAll()
                .AsEnumerable()
                .Where(r => r.Id == id)
                .Select(r => new ReportDto(r.Id, r.Name, r.Description, r.CreatedAt.ToLongDateString()))
                //.FirstOrDefaultAsync(r => r.Id == id);
                .FirstOrDefault(r => r.Id == id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }

        if (report == null)
        {
            _logger.Warning($"отчет с {id} не найден");
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound
            };
        }

        return new BaseResult<ReportDto>()
        {
            Data = report
        };
    }

    public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
    {
        ReportDto[] reports;

        try
        {
            reports = await _reportRepository.GetAll()
                .Where(r => r.UserId == userId)
                .Select(r => new ReportDto(r.Id, r.Name, r.Description, r.CreatedAt.ToLongDateString()))
                .ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }

        // кажется это лишнее, нужно подумать
        if (reports.Length == 0)
        {
            _logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);
            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportsNotFound,
                ErrorCode = (int)ErrorCodes.ReportsNotFound
            };
        }
        return new CollectionResult<ReportDto>()
        {
            Data = reports,
            Count = reports.Length
        };
    }

    public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto reportDto)
    {
        try
        {
            var report = await _reportRepository.GetAll().FirstOrDefaultAsync(r => r.Id == reportDto.Id);
            var result = _reportValidator.ValidateOnNull(report);

            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            report!.Name = reportDto.Name;
            report.Description = reportDto.Description;

            await _reportRepository.UpdateAsync(report!);

            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);

            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalserverError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }
}
