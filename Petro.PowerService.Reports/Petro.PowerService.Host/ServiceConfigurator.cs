using log4net.Config;
using Topshelf;

namespace Petro.PowerService.Host
{
    internal class ServiceConfigurator
    {
        internal static void Configure()
        {
            // Configure log4net from app.config file
            XmlConfigurator.Configure();

            //Configure DI Container
            ServiceLocator.Configure();

            // Service configuration
            HostFactory.Run(configure =>
            {
                configure.Service<ReportGenerationService>(service =>
                {
                    service.ConstructUsing(s => new ReportGenerationService());
                    service.WhenStarted(s => s.Start());
                    service.WhenStopped(s => s.Stop());
                });

                configure.RunAsLocalSystem();

                configure.UseLog4Net();

                // Service text
                configure.SetServiceName("PetroReportGenerationService");
                configure.SetDisplayName("PetroReportGenerationService");
                configure.SetDescription("PetroReportGenerationService");
            });
        }
    }
}