using Microsoft.AspNetCore.Mvc;
using N5.Microservices.User.Domain;

[ApiController]
[Route("[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(ILogger<EmployeesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<Employee> Get()
    {

    }

    [HttpGet]
    [Route("{id}")]
    public IEnumerable<Employee> GetById(int id)
    {

    }

    [HttpPost]
    public IEnumerable<Employee> Create()
    {

    }
}