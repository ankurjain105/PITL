using System;
using System.Collections.Generic;
using System.Text;

namespace Petro.PowerService.BusinessLogic.Core
{
    public interface ILogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception ex, string message);
        void Fatal(string message);
    }
}