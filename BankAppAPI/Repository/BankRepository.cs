using BankService.Data;
using BankService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Repository
{
    public class BankRepository : IBankRepository<Customer>
    {
        readonly BankDBContext _bankContext;

        public BankRepository(BankDBContext context)
        {
            _bankContext = context;
        }

        public async Task<Customer> Add(Customer _object)
        {
            var obj = await _bankContext.customers.AddAsync(_object);
            _bankContext.SaveChanges();
            return obj.Entity;
        }
               
        public IEnumerable<Customer> GetAllCustomer()
        {
            return _bankContext.customers.ToList();
        }

        public Customer GetById(Guid Id)
        {
            return _bankContext.customers.Where(x => x.id == Id).FirstOrDefault();
        }

        public void Update(Customer _object)
        {
            _bankContext.customers.Update(_object);
            _bankContext.SaveChanges();
        }        
    }
}
