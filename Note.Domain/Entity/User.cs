using Note.Domain.Interfaces;

namespace Note.Domain.Entity;

public class User : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    public long Login { get; set; }
    public long Password { get; set; }
    public List<Report> Reports { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public long UpdatedBy { get; set; }
}
