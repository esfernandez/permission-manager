using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Queries;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;

namespace N5.Microservices.User.Application.Handlers;
public class GetPermissionsByEmployeeIdHandler(IEmployeeRepository repo) : IRequestHandler<GetPermissionsByEmployeeIdQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await repo.GetById(request.employeeId)
            ?? throw new InvalidOperationException("Employee not exist");

        return employee.Permissions.Adapt<IEnumerable<PermissionDto>>();
    }
}