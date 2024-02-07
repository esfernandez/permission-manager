using Elastic.Clients.Elasticsearch;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using N5.Microservices.User.API.Controllers;
using N5.Microservices.User.Application.Commands;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.Application.Handlers;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure;
using N5.Microservices.User.Infrastructure.Commands;
using N5.Microservices.User.Infrastructure.Interfaces;
using N5.Microservices.User.IntegrationTests.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace N5.Microservices.User.IntegrationTests;

[TestClass]
public class PermissionsTestCases : IntegrationTestBase
{
    [TestMethod]
    public async Task SaveAndGetPermission()
    {
        Guid permissionId;
        EmployeeDto employeeDto;

        using (var scope = ServiceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            var loggerEmployee = scope.ServiceProvider.GetRequiredService<ILogger<EmployeesController>>();
            var loggerPermission = scope.ServiceProvider.GetRequiredService<ILogger<PermissionsController>>();
            var employeeController = new EmployeesController(loggerEmployee, mediator);
            var permissionController = new PermissionsController(loggerPermission, mediator);

            // Esto no iría porque la lectura a Elastic sucede en otro tiempo de asincronismo, cuando llega
            // el evento a Kafka
            //ElasticSearchClientMock
            //    .Setup(x => x.SearchAsync<Permission, Guid>(It.IsAny<Expression<Func<Permission, Guid>>>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(Task.FromResult());

            var permissionDto = new PermissionDto();

            var responseCreationEmployee = await employeeController.Create(
                new CreateEmployeeCommand(
                    "Name employee", 
                    "Lastname Employee", 
                    "test@test.com"
                )
            );
            Assert.IsInstanceOfType<OkObjectResult>(responseCreationEmployee);
            employeeDto = (EmployeeDto)((OkObjectResult)responseCreationEmployee).Value;

            var responseRequestPermission = await permissionController.RequestPermission(employeeDto.Id, permissionDto);
            Assert.IsInstanceOfType<OkObjectResult>(responseRequestPermission);
            permissionDto = (PermissionDto)((OkObjectResult)responseRequestPermission).Value;
            permissionId = permissionDto.Id;
            Assert.IsFalse(permissionDto.Granted);

            dbContext.Dispose();
        }

        using (var scope = ServiceProvider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var dbContext = scope.ServiceProvider.GetRequiredService<EmployeeDbContext>();
            var loggerEmployee = scope.ServiceProvider.GetRequiredService<ILogger<EmployeesController>>();
            var loggerPermission = scope.ServiceProvider.GetRequiredService<ILogger<PermissionsController>>();

            var permissionController = new PermissionsController(loggerPermission, mediator);

            var permissionDto = new PermissionDto
            {
                Granted = true,
                Id = permissionId
            };

            var responseUpdatePermission = await permissionController.UpdatePermission(employeeDto.Id, permissionDto);
            Assert.IsInstanceOfType<OkResult>(responseUpdatePermission);

            var permissions = dbContext.Permissions.ToList();

            Assert.AreEqual(permissions.Count(), 1);
            Assert.AreEqual(permissions.First().Id, permissionDto.Id);
            Assert.IsTrue(permissions.First().Granted);
        }
    }
}
