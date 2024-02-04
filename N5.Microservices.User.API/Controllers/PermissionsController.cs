using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Queries;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Queries;
using System.Security;

namespace N5.Microservices.User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly ILogger<PermissionsController> _logger;
    private readonly IMediator _mediator;

    public PermissionsController(ILogger<PermissionsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    /// <summary>
    /// Solicita un permiso para un empleado
    /// </summary>
    /// <param name="employeeId">Id del empleado</param>
    /// <param name="permission">Objeto de permiso</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> RequestPermission([FromQuery] Guid employeeId, RequestPermissionCommand permission)
    {
        if (await EmployeeExits(employeeId))
        {
            return NotFound("Employee not exist");
        }

        return Ok(await _mediator.Send(permission));
    }

    /// <summary>
    /// Actualiza un permiso de un empleado
    /// </summary>
    /// <param name="employeeId">Id de empleado</param>
    /// <param name="permission">Objeto de permiso</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(Guid employeeId, UpdatePermissionCommand permission)
    {
        if (await EmployeeExits(employeeId))
        {
            return NotFound("Employee not exist");
        }
        await _mediator.Send(permission);

        return Ok();
    }

    /// <summary>
    /// Obtiene todos los permisos de un empleado.
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetPermissions(Guid employeeId)
    {
        if (await EmployeeExits(employeeId))
        {
            return NotFound("Employee not exist");
        }

        return Ok(await _mediator.Send(new GetPermissionsByEmployeeIdQuery(employeeId)));
    }


    private async Task<bool> EmployeeExits(Guid employeeId)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(employeeId));
        return employee != null;
    }
}