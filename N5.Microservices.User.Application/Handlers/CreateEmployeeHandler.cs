using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Domain;
using N5.Microservices.User.Infrastructure.Commands;

namespace N5.Microservices.User.Application.Handlers;
public class CreateEmployeeHandler(IEmployeeRepository repo) : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repo = repo;

    async Task<EmployeeDto> IRequestHandler<CreateEmployeeCommand, EmployeeDto>.Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.Insert(request.Adapt<Employee>());
        await _repo.Save();
        return entity.Adapt<EmployeeDto>();
    }
}
