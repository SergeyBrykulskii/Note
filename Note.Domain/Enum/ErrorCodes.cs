namespace Note.Domain.Enum;

public enum ErrorCodes
{
    InternalServerError = 1,
    InvalidRefreshToken = 2,

    ReportsNotFound = 10,
    ReportNotFound = 11,
    ReportAlreadyExists = 12,

    UserNotFound = 21,
    UserAlreadyExist = 22,

    PasswordNotEqual = 31,
    WrongPassword = 33,

    RoleAlreadyExists = 41,
    RoleNotFound = 42,
    UserAlreadyHasRole = 43,
}
