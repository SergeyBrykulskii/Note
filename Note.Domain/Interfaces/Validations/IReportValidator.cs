using Note.Domain.Entity;
using Note.Domain.Result;

namespace Note.Domain.Interfaces.Validations;

public interface IReportValidator : IBaseValidator<Report>
{
    BaseResult CreateValidator(Report report, User user);
}
