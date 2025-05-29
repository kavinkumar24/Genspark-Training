using System;
using BankingApp.Models;
using BankingApp.Models.DTOs;
namespace BankingApp.Interfaces;

public interface ICustomerService
{
    Task<Customer> CreateCustomer(CustomerAddRequestDto customer);
    Task<Customer> GetCustomer(int id);
    Task<IEnumerable<Customer>> GetAllCustomers();
    Task<Customer> UpdateCustomer(int id, UpdateCustomerRequestDto customer);
    Task<Customer> DeleteCustomer(int id);

}
