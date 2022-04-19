namespace Petro.PowerService.BusinessLogic.Configuration
{
    public interface IServiceConfiguration
    {
        string ReportPath { get; }
        int RunIntervalInMinutes { get; }
    }
}