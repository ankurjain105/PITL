using System;

namespace Petro.PowerService.BusinessLogic.Core
{
    public class Clock : IClock
    {
        public DateTime Now => DateTime.Now;
    }
}