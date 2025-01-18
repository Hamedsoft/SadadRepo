using Application.Queries.Orders.GetAllOrders;
using Autofac.Extensions.DependencyInjection;
using Infrastructure.Extensions;
using MediatR;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructure(connectionString);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetAllOrdersQueryHandler>());

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ocelot
//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>{c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sadad API");});
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//app.UseOcelot().Wait();

app.Run();