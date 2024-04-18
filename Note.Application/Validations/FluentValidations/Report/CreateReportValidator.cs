using FluentValidation;
using Note.Domain.Dto.Report;

namespace Note.Application.Validations.FluentValidations.Report;

public class CreateReportValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportValidator()
    {
        RuleFor(r => r.Name).NotEmpty().MaximumLength(200);
        RuleFor(r => r.Description).NotEmpty().MaximumLength(2000);
    }
}
