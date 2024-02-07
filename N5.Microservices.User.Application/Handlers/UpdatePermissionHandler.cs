using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.DataAccess.Repositories;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.Application.Handlers;
public class UpdatePermissionHandler(IPermissionRepository permissionRepository, IEmployeeRepository employeeRepository, IEventProducer eventProducer) 
    : IRequestHandler<UpdatePermissionCommand>
{
    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = request.permissionDto.Adapt<Permission>();
        permission.Employee = await employeeRepository.GetById(request.employeeId) 
            ?? throw new NotFoundException("Employee not found");

        await permissionRepository.UpdatePermission(permission);
        await permissionRepository.Save();

        await eventProducer.SendEvent("permissions", new PermissionEventDto()
        {
            Id = Guid.NewGuid(),
            EmployeeId = request.employeeId,

            Action = "modify"
        });
    }
}
