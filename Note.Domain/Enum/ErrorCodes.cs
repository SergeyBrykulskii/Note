namespace Note.Domain.Enum;

public enum ErrorCodes
{
    InternalServerError = 1,

    ReportsNotFound = 10,
    ReportNotFound = 11,
    ReportAlreadyExists = 12,

    UserNotFound = 21,
    UserAlreadyExist = 22,

    PasswordNotEqual = 31,
    WrongPassword = 33,

}
