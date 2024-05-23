using Microsoft.EntityFrameworkCore.Storage;
using Note.Domain.Entity;
using Note.Domain.Interfaces.Repositories;
using Note.Domain.Interfaces.UnitOfWork;

namespace Note.DAL.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IBaseRepository<User> UserRepository { get; set; }
    public IBaseRepository<Role> RoleRepository { get; set; }
    public IBaseRepository<UserRole> UserRoleRepository { get; set; }


    public UnitOfWork(ApplicationDbContext context,
        IBaseRepository<User> userRepository, IBaseRepository<Role> roleRepository,
        IBaseRepository<UserRole> userRoleRepository)
    {
        _context = context;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        UserRoleRepository = userRoleRepository;
    }
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
