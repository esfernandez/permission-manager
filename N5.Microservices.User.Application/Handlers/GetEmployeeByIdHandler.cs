using Mapster;
using MediatR;
using N5.Microservices.User.Application.DTOs;
using N5.Microservices.User.Application.Exceptions;
using N5.Microservices.User.DataAccess.Repositories.Interfaces;
using N5.Microservices.User.Infrastructure.Queries;

namespace N5.Microservices.User.Application.Handlers;
public class GetEmployeeByIdHandler(IEmployeeRepository repo) : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
{
    private IEmployeeRepository _repo = repo;

    public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        return (await _repo.GetById(request.id) ?? throw new NotFoundException($"Employee {request.id} not exist."))
            .Adapt<EmployeeDto>();
    }
}
