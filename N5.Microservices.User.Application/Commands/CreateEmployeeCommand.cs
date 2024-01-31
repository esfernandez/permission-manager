using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using N5.Microservices.User.Application.DTOs;

namespace N5.Microservices.User.Infrastructure.Commands;
public record CreateEmployeeCommand(string name, string lastName, string email)
    : IRequest<EmployeeDto>;
