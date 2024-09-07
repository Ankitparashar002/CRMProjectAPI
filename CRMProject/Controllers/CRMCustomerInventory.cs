using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMCustomerInventoryController : ControllerBase
    {
        private readonly TaskDbContext context;
        private readonly IMapper mapper;

        public CRMCustomerInventoryController(TaskDbContext context, IMapper _mapper)
        {
            this.context = context;
            this.mapper = _mapper;
        }

       

        // GetList: Returns all records of Customer-Inventory relationships
        [HttpGet("GetList")]
        public async Task<ActionResult<IEnumerable<CustomerInventoryListing>>> GetList()
        {
            try
            {
                var data = await context.CustomerInventoryListings.ToListAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving data", details = ex.Message });
            }
        }

        // GetCustomerList: Returns all inventories associated with a specific customer
        [HttpGet("GetCustomerList/{customerId}")]
        public async Task<ActionResult<IEnumerable<AddCustomerToInventoryRequestDto>>> GetCustomerList(int customerId)
        {
            try
            {
                var data = await context.CustomerInventoryListings
                                         .Where(c => c.CustomerId == customerId)
                                         .Select(c => new AddCustomerToInventoryRequestDto
                                         {
                                             InventoryId = c.InventoryId
                                         })
                                         .ToListAsync();

                if (!data.Any())
                {
                    return NotFound(new { message = "No inventories found for this customer" });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving data", details = ex.Message });
            }
        }


        [HttpGet("GetInventoryList/{inventoryId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetInventoryList(int inventoryId)
        {
            try
            {
                // Retrieve the list of customer IDs for the inventoryId
                var customerIds = await context.CustomerInventoryListings
                                        .Where(i => i.InventoryId == inventoryId)
                                        .Select(i => i.CustomerId)
                                        .ToListAsync();

                if (!customerIds.Any())
                {
                    return NotFound(new { message = "No customers found for this inventory" });
                }

                var customerList = new List<object>();

                // Loop through each customer and retrieve only the CustomerId and Name
                foreach (var customerId in customerIds)
                {
                    var customer = await context.Customers
                                                .Where(c => c.Id == customerId)
                                                .Select(c => new
                                                {
                                                    CustomerId = c.Id,
                                                    CustomerName = c.Name
                                                })
                                                .FirstOrDefaultAsync();

                    if (customer != null)
                    {
                        customerList.Add(customer);
                    }
                }

                return Ok(customerList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving data", details = ex.Message });
            }
        }


        // Create: Adds a new Customer-Inventory relationship if it doesn't already exist

        // AddInventory: Associates one or more customers with a specific inventory
        [HttpPost("AddInventory/{inventoryId}")]
        public async Task<ActionResult> AddInventory(int inventoryId, [FromBody] List<int> customerIds)
        {
            if (customerIds == null || !customerIds.Any())
            {
                return BadRequest(new { message = "No customer IDs provided" });
            }

            try
            {
                // Check for existing relationships to avoid duplication
                var existingEntries = await context.CustomerInventoryListings
                    .Where(c => c.InventoryId == inventoryId && customerIds.Contains(c.CustomerId))
                    .Select(c => c.CustomerId)
                    .ToListAsync();

                // Identify customer IDs that do not conflict
                var nonConflictingCustomerIds = customerIds.Except(existingEntries).ToList();

                if (!nonConflictingCustomerIds.Any())
                {
                    return Conflict(new { message = "All provided customer-inventory relationships already exist" });
                }

                // Create a list of CustomerInventoryListing objects for non-conflicting entries
                var customerInventoryListings = nonConflictingCustomerIds.Select(customerId => new CustomerInventoryListing
                {
                    InventoryId = inventoryId,
                    CustomerId = customerId
                }).ToList();

                // Add the new relationships to the context
                context.CustomerInventoryListings.AddRange(customerInventoryListings);
                await context.SaveChangesAsync();

                return Ok(new { message = "Customer(s) added to inventory successfully", addedCustomers = nonConflictingCustomerIds });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding customers to inventory", details = ex.Message });
            }
        }
        // AddCustomer: Associates a specific customer with one or more inventories
        [HttpPost("AddCustomer/{customerId}")]
        public async Task<ActionResult> AddCustomer(int customerId, [FromBody] List<int> inventoryIds)
        {
            if (inventoryIds == null || !inventoryIds.Any())
            {
                return BadRequest(new { message = "No Inventory IDs provided" });
            }

            try
            {
                // Check for existing relationships to avoid duplication
                var existingEntries = await context.CustomerInventoryListings
                    .Where(c => c.CustomerId == customerId && inventoryIds.Contains(c.InventoryId))
                    .Select(c => c.InventoryId)
                    .ToListAsync();

                // Identify inventory IDs that do not conflict
                var nonConflictingInventoryIds = inventoryIds.Except(existingEntries).ToList();

                if (!nonConflictingInventoryIds.Any())
                {
                    return Conflict(new { message = "All provided inventory relationships already exist for this customer" });
                }

                // Create a list of CustomerInventoryListing objects for non-conflicting entries
                var customerInventoryListings = nonConflictingInventoryIds.Select(inventoryId => new CustomerInventoryListing
                {
                    CustomerId = customerId,
                    InventoryId = inventoryId
                }).ToList();

                // Add the new relationships to the context
                context.CustomerInventoryListings.AddRange(customerInventoryListings);
                await context.SaveChangesAsync();

                return Ok(new { message = "Customer added to inventory successfully", addedInventories = nonConflictingInventoryIds });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding customer to inventories", details = ex.Message });
            }
        }

        // DeleteCustomer: Removes a specific customer from a specific inventory
        [HttpDelete("DeleteCustomer/{inventoryId}/{customerId}")]
        public async Task<ActionResult> DeleteCustomer(int inventoryId, int customerId)
        {
            try
            {
                // Check if the relationship exists
                var existingEntry = await context.CustomerInventoryListings
                    .Where(c => c.InventoryId == inventoryId && c.CustomerId == customerId)
                    .FirstOrDefaultAsync();

                if (existingEntry == null)
                {
                    return NotFound(new { message = "The specified customer-inventory relationship does not exist" });
                }

                // Remove the relationship
                context.CustomerInventoryListings.Remove(existingEntry);
                await context.SaveChangesAsync();

                return Ok(new { message = "Customer removed from inventory successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while removing the customer from inventory", details = ex.Message });
            }
        }



    }
}
