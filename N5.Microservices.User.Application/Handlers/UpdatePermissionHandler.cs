using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;

namespace N5.Microservices.User.Application.Handlers;
public class UpdatePermissionHandler(IPermissionRepository permissionRepository) : IRequestHandler<UpdatePermissionCommand>
{
    public async Task Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        await permissionRepository.UpdatePermission(request.permissionDto.Adapt<Permission>());
    }
}
