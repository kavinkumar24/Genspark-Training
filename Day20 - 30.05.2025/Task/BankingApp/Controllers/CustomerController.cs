using Microsoft.AspNetCore.Mvc;
using BankingApp.Models;
using BankingApp.Interfaces;
using BankingApp.Models.DTOs;


namespace BankingApp.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class CustomerController : ControllerBase
   {
       private readonly ICustomerService _customerService;
      
       public CustomerController(ICustomerService customerService)
       {
           _customerService = customerService;
       }


       [HttpPost]
       public async Task<ActionResult<Customer>> PostCustomer([FromBody] CustomerAddRequestDto dto)
       {
           try
           {
               if (dto == null)
               {
                   return BadRequest("Customer data is null");
               }


               var customer = await _customerService.CreateCustomer(dto);
               if (customer == null)
               {
                   return StatusCode(500, "Failed to create customer");
               }


               return Ok(customer);
           }
           catch (Exception ex)
           {
               return StatusCode(500, $"Internal server error: {ex.Message}");
           }
       }


       [HttpPut("{id}")]
       public async Task<ActionResult<Customer>> UpdateCustomer(int id, [FromBody] UpdateCustomerRequestDto dto)
       {
           try
           {
               if (dto == null)
               {
                   return BadRequest("Customer data is null");
               }


               var updatedCustomer = await _customerService.UpdateCustomer(id, dto);
               if (updatedCustomer == null)
               {
                   return NotFound($"Customer with id {id} not found");
               }


               return Ok(updatedCustomer);
           }
           catch (Exception ex)
           {
               return StatusCode(500, $"Internal server error: {ex.Message}");
           }
       }


       [HttpDelete("{id}")]
       public async Task<ActionResult<Customer>> DeleteCustomer(int id)
       {
           try
           {
               var deletedCustomer = await _customerService.DeleteCustomer(id);
               if (deletedCustomer == null)
               {
                   return NotFound($"Customer with id {id} not found");
               }


               return Ok(deletedCustomer);
           }
           catch (Exception ex)
           {
               return StatusCode(500, $"Internal server error: {ex.Message}");
           }
       }


       [HttpGet]
       public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
       {
           try
           {
               var customers = await _customerService.GetAllCustomers();
               return Ok(customers);
           }
           catch (Exception ex)
           {
               return StatusCode(500, $"Internal server error: {ex.Message}");
           }
       }
   }
}
