using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.DTOs;
public class PermissionDto
{
    public Guid Id { get; set; }
    public bool Granted { get; set; }

    public PermissionTypeDto Type { get; set; }
}
