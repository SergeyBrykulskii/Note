using FluentValidation;
using Note.Domain.Dto.Report;

namespace Note.Application.Validations.FluentValidations.Report;

public class UpdateReportValidator : AbstractValidator<UpdateReportDto>
{
    public UpdateReportValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Description).NotEmpty().MaximumLength(2000);
    }
}
