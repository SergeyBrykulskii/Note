using Microsoft.EntityFrameworkCore.Storage;
using Note.Domain.Entity;
using Note.Domain.Interfaces.Repositories;

namespace Note.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task<int> SaveChangesAsync();

    IBaseRepository<User> UserRepository { get; set; }
    IBaseRepository<Role> RoleRepository { get; set; }
    IBaseRepository<UserRole> UserRoleRepository { get; set; }
}
