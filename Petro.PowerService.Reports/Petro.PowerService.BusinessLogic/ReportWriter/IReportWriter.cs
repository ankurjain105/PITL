using System.Data;
using System.Threading.Tasks;

namespace Petro.PowerService.BusinessLogic.ReportWriter
{
    public interface IReportWriter
    {
        Task WriteAsync(string fileName, DataTable data);
    }
}