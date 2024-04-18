using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Note.Application.Mapping;
using Note.Application.Services;
using Note.Application.Validations;
using Note.Application.Validations.FluentValidations.Report;
using Note.Domain.Dto.Report;
using Note.Domain.Interfaces.Services;
using Note.Domain.Interfaces.Validations;

namespace Note.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportMapping));

        InitServices(services);
    }

    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IReportValidator, ReportValidator>();
        services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
        services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();
        services.AddScoped<IReportService, ReportService>();
    }
}
