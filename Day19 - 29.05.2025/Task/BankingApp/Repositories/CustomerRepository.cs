using System;
using BankingApp.Repositories;
using BankingApp.Contexts;
using BankingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Repositories;

public class CustomerRepository : Repository<int, Customer>
{
    public CustomerRepository(BankingContext context) : base(context)
    {
    }
    public override async Task<Customer> Get(int key)
    {
        try
        {
            if(key <= 0)
            {
                throw new ArgumentException("Customer ID must be greater than zero.");
            }
            var customer = await _bankingContext.Customers.SingleOrDefaultAsync(c => c.Id == key);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }
            return customer;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving customer with ID {key}: {ex.Message}");
        }
    }
    
    public override async Task<IEnumerable<Customer>> GetAll()
    {
        try
        {
            return await _bankingContext.Customers.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving all customers: {ex.Message}");
        }
    }

}
