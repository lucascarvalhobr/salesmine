using SalesMine.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesMine.Customers.API.Models
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAll();

        Task<Customer> GetById(Guid id);

        Task<Customer> GetByCpf(string cpf);

        void Create(Customer product);

        void Update(Customer product);
    }
}
