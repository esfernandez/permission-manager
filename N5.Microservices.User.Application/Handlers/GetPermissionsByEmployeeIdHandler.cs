using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.Application.Queries;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Queries;

namespace N5.Microservices.User.Application.Handlers;
public class GetPermissionsByEmployeeIdHandler(IPermissionRepository repo, IMediator mediator) 
    : IRequestHandler<GetPermissionsByEmployeeIdQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await mediator.Send(new GetEmployeeByIdQuery(request.employeeId), cancellationToken);

        var permissions = (await repo.GetPermissions(employee.Adapt<Employee>()))
            ?? throw new NotFoundException("Employee not exist");

        return permissions.Adapt<IEnumerable<PermissionDto>>();
    }
}