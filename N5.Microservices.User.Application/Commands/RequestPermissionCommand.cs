using MediatR;
using N5.Microservices.User.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.Application.Commands;
public record RequestPermissionCommand(PermissionDto permissionDto): IRequest<PermissionDto>;
