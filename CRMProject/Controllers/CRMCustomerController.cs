using Azure;
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

        public CRMCustomerController(TaskDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetCustomer()
        {
            var data = await context.Customers.ToListAsync();
            return Ok(data);
        }


        [HttpPost]
        public async Task<ActionResult<Customer>> CreateTask(Customer std)
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
            find.Name = customer.Name;
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
