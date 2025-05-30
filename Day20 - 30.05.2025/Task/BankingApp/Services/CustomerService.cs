using BankingApp.Interfaces;
using BankingApp.Models;
using BankingApp.Models.DTOs;
using BankingApp.Misc;

namespace BankingApp.Services
{
    public class CustomerService : ICustomerService
    {
        CustomerMapper customerMapper;
        AccountMapper accountMapper;
        private readonly IRepository<int, Customer> _customerRepository;
        private readonly IRepository<int, Account> _accountRepository;

        private readonly IOtherFunctionalitiesInterface _otherFunctionalities;

        public CustomerService(IRepository<int, Customer> customerRepository, IRepository<int, Account> accountRepository, IOtherFunctionalitiesInterface otherFunctionalities)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _otherFunctionalities = otherFunctionalities;
            customerMapper = new CustomerMapper();
            accountMapper = new AccountMapper();
        }


        public async Task<Customer> CreateCustomer(CustomerAddRequestDto customer)
        {
            try
            {
                var newCustomer = customerMapper.MapCustomerAddRequest(customer);
                if (newCustomer == null)
                {
                    throw new ArgumentNullException(nameof(customer), "Customer data is null");
                }
                newCustomer = await _customerRepository.Add(newCustomer);
                if (newCustomer == null)
                {
                    throw new Exception("Failed to create customer");
                }
                if (customer.Accounts != null && customer.Accounts.Any())
                {
                    foreach (var accountDto in customer.Accounts)
                    {
                        var account = accountMapper.MapAccountAddRequest(accountDto);
                        if (account == null)
                        {
                            throw new ArgumentNullException(nameof(accountDto), "Account data is null");
                        }
                        account.CustomerId = newCustomer.Id;
                        await _accountRepository.Add(account);
                    }
                }
                return newCustomer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating customer: {ex.Message}", ex);
            }
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await _customerRepository.Get(id);
        }

        public async Task<Customer> DeleteCustomer(int id)
        {
            return await _customerRepository.Delete(id);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAll();
            var accounts = await _accountRepository.GetAll();
            foreach (var customer in customers)
            {
                customer.Accounts = accounts.Where(a => a.CustomerId == customer.Id).ToList();
            }
            return customers;
        }

        public async Task<Customer> UpdateCustomer(int id, UpdateCustomerRequestDto request)
        {
            try
            {
                var existingCustomer = await _customerRepository.Get(id);
                if (existingCustomer == null)
                {
                    throw new Exception("Customer not found");
                }


                var updatedCustomer = customerMapper.MapCustomerUpdate(existingCustomer, request);
                if (updatedCustomer == null)
                {
                    throw new Exception("Failed to map customer update");
                }


                var result = await _customerRepository.Update(id, updatedCustomer);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<AccountResponseDto>> GetAccountsByCustomerId(int customerId)
        {

            var accounts = await _otherFunctionalities.GetAccountsByCustomerId(customerId);
            if (accounts == null || !accounts.Any())
            {
                throw new Exception($"No accounts found for customer with ID {customerId}");
            }

            return accounts;

        }
    }
}