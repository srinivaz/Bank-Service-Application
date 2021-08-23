using BankService.Model;
using BankService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService.Service
{

    public interface ICustomerService
    {
        Customer Authenticate(string username, string password);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetById(Guid id);
        Task<Customer> AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
    }
    public class CustomerService : ICustomerService
    {
        private readonly IBankRepository<Customer> _customer;

        public CustomerService(IBankRepository<Customer> customer)
        {
            _customer = customer;
        }

        public Customer Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var customer = _customer.GetAllCustomer().SingleOrDefault(x => x.Username == username && x.Password == password);

            // check if username exists
            if (customer == null)
                return null;
            
            // authentication successful
            return customer;
        }
        
        public IEnumerable<Customer> GetAllCustomers()
        {
            try
            {
                return _customer.GetAllCustomer();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Customer GetById(Guid id)
        {
            return _customer.GetById(id);
        }        
       
        public async Task<Customer> AddCustomer(Customer customer)
        {
            return await _customer.Add(customer);
        }
              
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                var result = _customer.GetAllCustomer().Where(x => x.Username == customer.Username).ToList();

                foreach (var item in result)
                {
                    _customer.Update(item);
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
