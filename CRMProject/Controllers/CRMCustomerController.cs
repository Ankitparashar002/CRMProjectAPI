using AutoMapper;
using Azure;
using CRMProject.DTO;
using CRMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerController : ControllerBase
    {
        private readonly TaskDbContext context;
        private readonly IMapper mapper;

        public CRMCustomerController(TaskDbContext context, IMapper _mapper)
        {
            this.context = context;
            this.mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomer()
        {
            var data = await context.Customers.ToListAsync();
            var customerDtos = mapper.Map<List<CustomerDto>>(data);

            return Ok(customerDtos);
        }


        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
        {
            var customer = mapper.Map<Customer>(customerDto);
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();

            var createdCustomerDto = mapper.Map<CustomerDto>(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, createdCustomerDto);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            var Task = await context.Customers.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Task;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<CustomerDto>> EditCustomer(CustomerDto customer, int id)
        {
        
            var find = await context.Customers.FindAsync(id);
            if (find == null)
            {
                return BadRequest("fail");
            }
            if (customer.Name != null)
            {
                find.Name = customer.Name;
            }
            if(find.Email != null)
            {
                find.Email = customer.Email;
            }
            if (find.Mobile != null)
            {
                find.Mobile = customer.Mobile;
            }       
            if (customer.Property != null)
            {
                find.Property = customer.Property;
            }
            if (customer.Address != null)
            {
                find.Address = customer.Address;
            }
            if (customer.Need != null)
            {
                find.Need = customer.Need;
            }
            if (customer.Remarks != null)
            {
                find.Remarks = customer.Remarks;
            }
            //  context.Customers.Update(customer);
            var customers = mapper.Map<Customer>(customer);
             context.Customers.Update(customers);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> EditCustomer(int id, CustomerDto customerDto)
        {
            var existingCustomer = await context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            mapper.Map(customerDto, existingCustomer);
            context.Customers.Update(existingCustomer);
            await context.SaveChangesAsync();

            var updatedCustomerDto = mapper.Map<CustomerDto>(existingCustomer);
            return Ok(updatedCustomerDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteTask(int id)
        {
            var std = await context.Customers.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.Customers.Remove(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }

      

        private bool CustomerExists(int id)
        {
            return context.Customers.Any(e => e.Id == id);
        }
      }
}
