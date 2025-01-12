using Application.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using Infrastructure.IoC;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Change IoC to Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register Application & Infrastructure Services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(connectionString);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

// Configure Autofac Module
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new InfrastructureModule());
});

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
