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
        if (!request.DateOfBirth.HasValue)
        {
            return null;
        }
        var dob = request.DateOfBirth.Value;
        var today = DateTime.Today;
         var age = today.Year - dob.Year;
        if (today.Month < dob.Month || (today.Month == dob.Month && today.Day < dob.Day))
        {
            age--;
        }
        if (age < 18)
        {   
            throw new ArgumentException("Customer must be at least 18 years old.");
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
