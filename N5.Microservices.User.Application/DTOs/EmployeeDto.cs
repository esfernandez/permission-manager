using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.DTOs;
public class EmployeeDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string IdentifierCode { get; set; }
    public string Email { get; set; }
}
