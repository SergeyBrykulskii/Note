using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Note.Domain.Entity;

namespace Note.DAL.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(50);

        builder.HasData(new List<Role>()
        {
            new()
            {
                Id = 1,
                Name = "User"
            },
            new()
            {
                Id = 2,
                Name = "Admin"
            },
            new()
            {
                Id = 3,
                Name = "Moderator"
            }
        });
    }
}
