using Note.Domain.Interfaces;

namespace Note.Domain.Entity;

public class User : IEntityId<long>
{
    public long Id { get; set; }

    public string Login { get; set; }
    public string Password { get; set; }
    public List<Report>? Reports { get; set; }


}
