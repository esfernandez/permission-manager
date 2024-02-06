using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.Application.Queries;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.Application.Handlers;
public class GetPermissionsByEmployeeIdHandler(IPermissionRepository repo, IEmployeeRepository employeeRepository, IEventProducer eventProducer)
    : IRequestHandler<GetPermissionsByEmployeeIdQuery, IEnumerable<PermissionDto>>
{
    public async Task<IEnumerable<PermissionDto>> Handle(GetPermissionsByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.employeeId)
            ?? throw new NotFoundException("Employee not exist");

        var permissions = (await repo.GetPermissions(employee.Adapt<Employee>()));

        await eventProducer.SendEvent("permissions", new PermissionEventDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.employeeId,

            Action = "get"
        });

        return permissions.Adapt<IEnumerable<PermissionDto>>();
    }
}