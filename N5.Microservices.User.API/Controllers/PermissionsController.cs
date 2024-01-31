using Microsoft.AspNetCore.Mvc;
using N5.Microservices.User.Domain;

namespace N5.Microservices.User.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly ILogger<PermissionsController> _logger;

    public PermissionsController(ILogger<PermissionsController> logger)
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