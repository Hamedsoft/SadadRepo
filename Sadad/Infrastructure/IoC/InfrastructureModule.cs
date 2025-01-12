using Autofac;
using Contracts.Repositories;
using Contracts.Services;
using Infrastructure.Repositories;
using Module = Autofac.Module;

namespace Infrastructure.IoC
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // ثبت Repositoryها و سرویس‌ها در Autofac
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
        }
    }
}
