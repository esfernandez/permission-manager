using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Microservices.User.Infrastructure.Queries;

namespace N5.Microservices.User.Application.Handlers;
public class RequestPermissionHandler(
        IPermissionRepository permissionRepository,
        IEmployeeRepository employeeRepository,
        IEventProducer eventProducer) 
    : IRequestHandler<RequestPermissionCommand, PermissionDto>
{
    public async Task<PermissionDto> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = request.permissionDto.Adapt<Permission>();

        permission.Employee = await employeeRepository.GetById(request.employeeId) ?? throw new NotFoundException("Employee not found");

        permission = (await permissionRepository.RequestPermission(permission));
        await permissionRepository.Save();
        await eventProducer.SendEvent("permissions", new PermissionEventDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.employeeId,

            PermissionId = permission.Id,
            Action = "request"
        });

        return permission.Adapt<PermissionDto>();
    }
}
