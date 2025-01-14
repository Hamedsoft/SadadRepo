using Application.Queries.Orders.GetAllOrders;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Change IoC to Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register Application & Infrastructure Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllOrdersQueryHandler>());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
