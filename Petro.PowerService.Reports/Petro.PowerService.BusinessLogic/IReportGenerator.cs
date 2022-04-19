using System.Threading.Tasks;

namespace Petro.PowerService.BusinessLogic
{
    public interface IReportGenerator
    {
        Task<bool> GenerateAsync();
    }
}