using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Repository
{
    public interface IBankRepository<T>
    {
        public Task<T> Add(T _object);

        public void Update(T _object);

        public IEnumerable<T> GetAllCustomer();

        public T GetById(Guid Id);
    
    }
}
