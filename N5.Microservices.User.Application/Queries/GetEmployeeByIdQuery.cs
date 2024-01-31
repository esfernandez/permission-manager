using MediatR;
using N5.Microservices.User.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Infrastructure.Queries;
public record GetEmployeeByIdQuery(Guid id) : IRequest<EmployeeDto>;
