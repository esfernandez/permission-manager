using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.Application.Handlers;
public class RequestPermissionHandler(IPermissionRepository permissionRepository, IEventProducer eventProducer) 
    : IRequestHandler<RequestPermissionCommand, PermissionDto>
{
    public async Task<PermissionDto> Handle(RequestPermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = (await permissionRepository.RequestPermission(request.permissionDto.Adapt<Permission>())).Adapt<PermissionDto>();

        await eventProducer.SendEvent("permission.request", permission);

        return permission;
    }
}
