using Microsoft.EntityFrameworkCore;
using SalesMine.Core.Data;
using SalesMine.Customers.API.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesMine.Customers.API.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.AsNoTracking().ToListAsync();
        }

        public async Task<Customer> GetById(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> GetByCpf(string cpf)
        {
            return await _context.Customers.FirstOrDefaultAsync(customer => customer.Cpf.Number == cpf);
        }

        public void Create(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
