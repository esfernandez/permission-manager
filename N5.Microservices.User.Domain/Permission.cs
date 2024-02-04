using N5.Microservices.User.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Domain;
public class Permission : Entity<Guid>
{
    public bool Granted { get; set; }
    public PermissionType Type { get; set; }

    public Employee Employee { get; set; }
}
