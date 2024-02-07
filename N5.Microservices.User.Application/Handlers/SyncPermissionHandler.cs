using Mapster;
using MediatR;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.Exceptions;
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
public class SyncPermissionHandler(IPermissionRepository permissionRepository, IEmployeeRepository employeeRepository) : IRequestHandler<SyncPermissionCommand>
{
    public async Task Handle(SyncPermissionCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetById(request.employeeId);

        if (employee == null) throw new NotFoundException("Employee not found");

        await permissionRepository.SyncPermissions(employee);
        await permissionRepository.Save();
    }
}
