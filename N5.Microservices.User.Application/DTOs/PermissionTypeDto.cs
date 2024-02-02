using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.DTOs;
public class PermissionTypeDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}
