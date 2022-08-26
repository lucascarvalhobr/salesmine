using System.Threading.Tasks;

namespace SalesMine.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
