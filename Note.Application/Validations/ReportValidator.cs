﻿using Note.Application.Resources;
using Note.Domain.Entity;
using Note.Domain.Enum;
using Note.Domain.Interfaces.Validations;
using Note.Domain.Result;

namespace Note.Application.Validations;

internal class ReportValidator : IReportValidator
{
    public BaseResult CreateValidator(Report report, User user)
    {
        if (report != null)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.ReportAlreadyExists,
                ErrorCode = (int)ErrorCodes.ReportAlreadyExists
            };
        }

        if (user == null)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        return new BaseResult();
    }

    public BaseResult ValidateOnNull(Report? model)
    {
        if (model == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound
            };
        }

        return new BaseResult();
    }
}
