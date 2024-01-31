using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.Handlers;
public class GetAllEmployeesHandler(IEmployeeRepository repo) : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private IEmployeeRepository _repo = repo;

    public async Task<IEnumerable<EmployeeDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        return (await _repo.GetAll()).Adapt<IEnumerable<EmployeeDto>>();
    }
}
