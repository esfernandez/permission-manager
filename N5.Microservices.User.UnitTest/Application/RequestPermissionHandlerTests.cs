using Moq;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.Application.Handlers;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Interfaces;

namespace N5.Microservices.User.UnitTest;

[TestClass]
public class RequestPermissionHandlerTests
{
    [TestMethod]
    public async Task HappyPath_RaiseEvent()
    {
        // Arrange
        var employeeId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        PermissionEventDto? permissionEventDtoCallback = null;

        var permissionRepositoryMock = new Mock<IPermissionRepository>();
        var employeeRepositoryMock = new Mock<IEmployeeRepository>();
        var eventProducerMock = new Mock<IEventProducer>();

        employeeRepositoryMock
            .Setup(x => x.GetById(employeeId))
            .Returns(() => Task.FromResult<Employee?>(new Employee()
            {
                Id = employeeId,
                Name = "Employee name Test",
                LastName = "Employee lastname Test",
                Email = "test@test.com"
            }));

        permissionRepositoryMock
            .Setup(x => x.RequestPermission(It.IsAny<Permission>()))
            .Returns((Permission obj) => 
            {
                obj.Id = permissionId;
                return Task.FromResult(obj);
            });

        var req = new RequestPermissionCommand(
            new PermissionDto()
            {
                Granted = true,
                Type = new PermissionTypeDto()
                {
                    Code = "CODE_TYPE",
                    Name = "Type name"
                }
            }, 
            employeeId);

        eventProducerMock.Setup(x => x.SendEvent("permissions", It.IsAny<object>()))
            .Returns(Task.CompletedTask)
            .Callback((string topic, object dto) =>
            {
                permissionEventDtoCallback = (PermissionEventDto)dto;
            });

        var handler = new RequestPermissionHandler(
            permissionRepositoryMock.Object,
            employeeRepositoryMock.Object,
            eventProducerMock.Object);

        // Act
        var permissionReturned =  await handler.Handle(req, CancellationToken.None);

        // Assert

        Assert.IsNotNull(permissionEventDtoCallback);
        Assert.AreEqual(permissionReturned.Id, permissionId);
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

        var req = new RequestPermissionCommand(
            new PermissionDto()
            {
                Granted = true,
                Type = new PermissionTypeDto()
                {
                    Code = "CODE_TYPE",
                    Name = "Type name"
                }
            },
            employeeId);

        var handler = new RequestPermissionHandler(
            permissionRepositoryMock.Object,
            employeeRepositoryMock.Object,
            eventProducerMock.Object);

        // Act
        var permissionReturned = await handler.Handle(req, CancellationToken.None);

    }
}