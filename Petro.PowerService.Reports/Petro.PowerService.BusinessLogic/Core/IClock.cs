using System;

namespace Petro.PowerService.BusinessLogic.Core
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}