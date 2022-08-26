using SalesMine.Core.DomainObjects;
using System;

namespace SalesMine.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
