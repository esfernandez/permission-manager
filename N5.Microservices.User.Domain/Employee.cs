using N5.Microservices.User.Domain.Abstractions;

namespace N5.Microservices.User.Domain;

public class Employee : Entity<Guid>
{
    
    public string Name { get; set; }
    public string LastName { get; set; }

    /// <summary>
    /// Esto puede ser DNI, CUIT, CUIL, RUT, etc.
    /// </summary>
    public string IdentifierCode { get; set; }
    public string Email { get; set; }

    public IEnumerable<Permission> Permissions { get; set; }
}
