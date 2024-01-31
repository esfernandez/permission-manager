using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Commands;
using N5.Microservices.User.Infrastructure.Queries;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly IMediator _mediator;

    public EmployeesController(ILogger<EmployeesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> Get()
    {
        return await _mediator.Send(new GetAllEmployeesQuery());
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));

        if (employee == null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeCommand employee)
    {
        var employeeCreated = await _mediator.Send(employee);

        return Ok(employeeCreated);
    }
}