using System.Threading.Tasks;

namespace Petro.PowerService.BusinessLogic.Queries
{
    public interface IQueryHandler<TQuery, TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
