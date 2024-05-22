using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Note.Domain.Entity;

namespace Note.DAL.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasData(new List<UserRole>
        {
            new()
            {
                UserId = 1,
                RoleId = 2
            }
        });
    }
}
