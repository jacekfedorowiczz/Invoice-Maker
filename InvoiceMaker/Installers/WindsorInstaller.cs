using AutoMapper;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using InvoiceMaker.Domain.Interfaces;
using InvoiceMaker.Infrastructure.Repositories;
using InvoiceMaker.Mappings;
using InvoiceMaker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvoiceMaker.Installers
{
    public class WindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMapper>().UsingFactoryMethod(factory =>
            {
                return new MapperConfiguration(map =>
                {
                    map.AddProfile<InvoiceMapperProfile>();

                }).CreateMapper();
            }));

            container.AddFacility<TypedFactoryFacility>();
            container.Register(Component.For<IService>().ImplementedBy<InvoiceService>().LifestyleSingleton());
            container.Register(Component.For<IRepository>().ImplementedBy<InvoiceRepository>().LifestyleSingleton());

            container.Register(Component.For<IHttpClientFactory>().UsingFactoryMethod(factory =>
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddHttpClient();
                var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(container, serviceCollection);
                return serviceProvider.GetRequiredService<IHttpClientFactory>();
            }).LifestyleSingleton());
        }
    }
}
