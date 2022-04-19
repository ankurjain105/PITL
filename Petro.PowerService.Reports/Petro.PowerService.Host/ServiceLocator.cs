using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Petro.PowerService.BusinessLogic;
using Petro.PowerService.BusinessLogic.Configuration;
using Petro.PowerService.BusinessLogic.Core;
using Petro.PowerService.BusinessLogic.Queries;
using Petro.PowerService.BusinessLogic.Queries.GetTrades;
using Petro.PowerService.BusinessLogic.ReportWriter;
using Services;

namespace Petro.PowerService.Host
{
    public static class ServiceLocator
    {
        private static WindsorContainer _container;
        
        public static void Configure()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IClock>().ImplementedBy<Clock>().LifestyleSingleton());
            _container.Register(Component.For<ILogger>().ImplementedBy<Logger>().LifestyleSingleton());
            _container.Register(Component.For<IServiceConfiguration>().ImplementedBy<ServiceConfiguration>().LifestyleSingleton());
            _container.Register(Component.For<IQueryHandler<GetAggregatedTradesQuery, GetAggregatedTradesResult>>().ImplementedBy<GetAggregatedTradesQueryHandler>().LifestyleTransient());
            _container.Register(Component.For<IReportWriter>().ImplementedBy<CsvWriter>().LifestyleTransient());
            _container.Register(Component.For<IReportGenerator>().ImplementedBy<AggregatedTradeReportGenerator>().LifestyleTransient());
            _container.Register(Component.For<IPowerService>().ImplementedBy<Services.PowerService>().LifestyleTransient());
        }

        public static T Resolve<T>() => _container.Resolve<T>();

        public static void Replace<T>(T instance) where T : class
        {
            _container.Register(Component.For<T>().Instance(instance).IsDefault().LifestyleSingleton());
        }
    }
}