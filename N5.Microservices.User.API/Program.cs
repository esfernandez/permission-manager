using Microsoft.EntityFrameworkCore;
using N5.Microservices.User.API;
using N5.Microservices.User.Application;
using N5.Microservices.User.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.DefineApplication();

builder.Services.InjectDependencies();

builder.Services
    .Configure<ElasticsearchOptions>(builder.Configuration.GetSection("ConnectionStrings:Elastic"))
    .Configure<KafkaOptions>(builder.Configuration.GetSection("ConnectionStrings:Kafka"));

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

app.Services.UseInfrastructure();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
