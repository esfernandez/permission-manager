using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.DataAccess.Repositories;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.Handlers;
public class SyncPermissionHandler(IPermissionRepository permissionRepository, IMediator mediator) : IRequestHandler<SyncPermissionCommand>
{
    public async Task Handle(SyncPermissionCommand request, CancellationToken cancellationToken)
    {
        var employee = mediator.Send(new GetEmployeeByIdQuery(request.employeeId), cancellationToken);
        await permissionRepository.SyncPermissions(employee.Adapt<Employee>());
        await permissionRepository.Save();
    }
}
