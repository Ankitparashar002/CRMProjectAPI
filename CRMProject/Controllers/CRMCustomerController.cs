using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;
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
            try
            {
                var data = await context.Customers.ToListAsync();
                var customerDtos = mapper.Map<List<CustomerDto>>(data);

                return Ok(customerDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
        {
            try
            {
                // Check if Email or Mobile is unique
                if (await context.Customers.AnyAsync(c => c.Email == customerDto.Email))
                {
                    return Conflict("Email already exists.");
                }

                if (await context.Customers.AnyAsync(c => c.Mobile == customerDto.Mobile))
                {
                    return Conflict("Mobile number already exists.");
                }

                var customer = mapper.Map<Customer>(customerDto);
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();

                var createdCustomerDto = mapper.Map<CustomerDto>(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, createdCustomerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                var customerDto = mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("{id}/need")]
        public async Task<ActionResult<string>> GetCustomerNeedById(int id)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer.Need);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPatch("need/{id}")]
        public async Task<ActionResult<CustomerDto>> EditCustomerNeed(int id, [FromBody] string need)
        {
            try
            {
                var existingCustomer = await context.Customers.FindAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }

                existingCustomer.Need = need;
                context.Customers.Update(existingCustomer);
                await context.SaveChangesAsync();

                var updatedCustomerDto = mapper.Map<CustomerDto>(existingCustomer);
                return Ok(updatedCustomerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("Remarks/{id}")]
        public async Task<ActionResult<string>> GetCustomerRemarksById(int id)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                return Ok(customer.Need);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPatch("Remarks/{id}")]
        public async Task<ActionResult<CustomerDto>> EditCustomerRemarks(int id, [FromBody] string Remarks)
        {
            try
            {
                var existingCustomer = await context.Customers.FindAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }

                existingCustomer.Remarks = Remarks;
                context.Customers.Update(existingCustomer);
                await context.SaveChangesAsync();

                var updatedCustomerDto = mapper.Map<CustomerDto>(existingCustomer);
                return Ok(updatedCustomerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<CustomerDto>> EditCustomer(int id, CustomerDto customerDto)
        {
            try
            {
                var existingCustomer = await context.Customers.FindAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound("Customer not found.");
                }
                if (await context.Customers.AnyAsync(c => c.Email == customerDto.Email && c.Id != id))
                {
                    return Conflict("Email already exists.");
                }

                if (await context.Customers.AnyAsync(c => c.Mobile == customerDto.Mobile && c.Id != id))
                {
                    return Conflict("Mobile number already exists.");
                }

                mapper.Map(customerDto, existingCustomer);
                context.Customers.Update(existingCustomer);
                await context.SaveChangesAsync();

                var updatedCustomerDto = mapper.Map<CustomerDto>(existingCustomer);
                return Ok(updatedCustomerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerDto>> DeleteCustomer(int id)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    return NotFound("Customer not found.");
                }

                context.Customers.Remove(customer);
                await context.SaveChangesAsync();

                var customerDto = mapper.Map<CustomerDto>(customer);
                return Ok(customerDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool CustomerExists(int id)
        {
            return context.Customers.Any(e => e.Id == id);
        }
    }
}
