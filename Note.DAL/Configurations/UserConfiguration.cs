﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Note.Domain.Entity;

namespace Note.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Login).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Password).IsRequired();

        builder.HasMany(u => u.Reports)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .HasPrincipalKey(u => u.Id);

        builder.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(r => r.RoleId),
                l => l.HasOne<User>().WithMany().HasForeignKey(r => r.UserId)
            );

        builder.HasData(new List<User>()
        {
            new()
            {
                Id = 1,
                Login = "qwerty",
                Password = "qwerty"
            }
        });
    }
}
