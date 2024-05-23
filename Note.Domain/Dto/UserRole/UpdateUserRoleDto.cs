namespace Note.Domain.Dto.UserRole;

public class UpdateUserRoleDto
{
    public string Login { get; set; }
    public long OldRoleId { get; set; }
    public long NewRoleId { get; set; }
}
