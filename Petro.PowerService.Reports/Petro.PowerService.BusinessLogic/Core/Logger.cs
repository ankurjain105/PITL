using log4net;
using System;

namespace Petro.PowerService.BusinessLogic.Core
{
    public class Logger : ILogger
    {
        private static readonly ILog Log = LogManager.GetLogger("ReportGenerationService");

        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            Log.Error(message, ex);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Warn(string message)
        {
            Log.Warn(message);
        }
    }
}