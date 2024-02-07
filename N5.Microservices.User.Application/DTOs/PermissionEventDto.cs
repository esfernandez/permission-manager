using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.DTOs;
public class PermissionEventDto
{
    // Solicitados por consigna de ejercicio
    public Guid Id { get; set; }
    public string Action { get; set; }

    // Datos adicionales
    public Guid EmployeeId { get; set; }
}
