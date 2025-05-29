using System;
using BankingApp.Models;
using BankingApp.Models.DTOs;

namespace BankingApp.Misc;

public class CustomerMapper
{
    public Customer? MapCustomerAddRequest(CustomerAddRequestDto request)
    {
        if (request == null)
        {
            return null;
        }

        return new Customer
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            DateOfBirth = request.DateOfBirth ?? DateTime.MinValue
        };
    }


    public Customer? MapCustomerUpdate(Customer customer, UpdateCustomerRequestDto request)
    {
        if (customer == null || request == null)
        {
            return null;
        }
        customer.Name = request.Name;
        customer.Email = request.Email;
        customer.PhoneNumber = request.PhoneNumber;
        customer.Address = request.Address;
        customer.DateOfBirth = request.DateOfBirth ?? DateTime.MinValue;
        return customer;
    }

}
