using Moq;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.Application.Handlers;
using N5.Microservices.User.Application.Queries;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.UnitTest.Application;

[TestClass]
public class GetPermissionsByEmployeeIdHandlerTests
{
    [TestMethod]
    public async Task HappyPath_RaiseEvent()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        PermissionEventDto? permissionEventDtoCallback = null;

        Employee employee = new Employee()
        {
            Id = employeeId,
            Name = "Employee name Test",
            LastName = "Employee lastname Test",
            Email = "test@test.com"
        };

        IEnumerable<Permission> permissions = new List<Permission>()
        {
            new Permission() 
            { 
                Id = permissionId, 
                Employee = employee,
                Granted = true,
                Type = new PermissionType() 
                { 
                    Id = Guid.NewGuid(),
                    Code = "CODE_TYPE_PERMISSION",
                    Name = "Type permission"
                }
            }
        };

        var permissionRepositoryMock = new Mock<IPermissionRepository>();
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        var eventProducerMock = new Mock<IEventProducer>();

        employeeRepositoryMock
            .Setup(x => x.GetById(employeeId))
            .Returns(() => Task.FromResult<Employee?>(employee));

        permissionRepositoryMock
            .Setup(x => x.GetPermissions(It.IsAny<Employee>()))
            .Returns(Task.FromResult(permissions));

        var req = new GetPermissionsByEmployeeIdQuery(employeeId);

        eventProducerMock.Setup(x => x.SendEvent("permissions", It.IsAny<object>()))
            .Returns(Task.CompletedTask)
            .Callback((string topic, object dto) =>
            {
                permissionEventDtoCallback = (PermissionEventDto)dto;
            });

        var handler = new GetPermissionsByEmployeeIdHandler(
            permissionRepositoryMock.Object,
            employeeRepositoryMock.Object,
            eventProducerMock.Object);

        // Act
        var permissionsReturned = await handler.Handle(req, CancellationToken.None);

        // Assert

        Assert.IsNotNull(permissionEventDtoCallback);
        Assert.IsTrue(permissionsReturned.Any());
        Assert.AreEqual(permissionsReturned.First().Id, permissionId);
    }

    [TestMethod]
    [ExpectedException(typeof(NotFoundException))]
    public async Task EmployeeNotExist_ThrowException()
    {
        // Arrange
        var employeeId = Guid.NewGuid();

        var permissionRepositoryMock = new Mock<IPermissionRepository>();
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        var eventProducerMock = new Mock<IEventProducer>();

        employeeRepositoryMock
            .Setup(x => x.GetById(employeeId))
            .Returns(() => Task.FromResult<Employee?>(null));

        var req = new GetPermissionsByEmployeeIdQuery(employeeId);

        var handler = new GetPermissionsByEmployeeIdHandler(
            permissionRepositoryMock.Object,
            employeeRepositoryMock.Object,
            eventProducerMock.Object);

        // Act
        var permissionReturned = await handler.Handle(req, CancellationToken.None);

    }
}
