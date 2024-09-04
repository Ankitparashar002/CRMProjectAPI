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
        public async Task<ActionResult<CustomerDto>> CreateTask(Customer std)
        {
            await context.Customers.AddAsync(std);
            await context.SaveChangesAsync();
           
            return CreatedAtAction(nameof(GetCustomer), new { id = std.Id }, std);

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
        public async Task<ActionResult<Customer>> EditCustomer(Customer customer, int id)
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
            await context.SaveChangesAsync();
            return Ok();
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
