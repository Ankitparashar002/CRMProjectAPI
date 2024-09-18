using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CRMInventoryController : ControllerBase
    {
        private readonly TaskDbContext context;
        private readonly IMapper mapper;

        public CRMInventoryController(TaskDbContext context, IMapper _mapper)
        {
            this.context = context;
            this.mapper = _mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<InventoryDto>>> GetTask()
        {
            // Get all inventories and customers from the database
            var inventories = await context.Inventories.ToListAsync();
            var customers = await context.Customers.ToListAsync();

            // Filter inventories where the Inventory.CustomerId matches a Customer's Id and Customer's status is true
            var filteredInventories = inventories
                .Where(i => customers.Any(c => c.InventoryStatus == true))
                .ToList();

            // Map filtered inventories to InventoryDto
            var inventoryDtos = mapper.Map<List<InventoryDto>>(filteredInventories);

            return Ok(inventoryDtos);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryDto>> CreateInventory(InventoryDto inventoryDto)
        {
            try
            {
                var inventory = mapper.Map<Inventory>(inventoryDto);
                await context.Inventories.AddAsync(inventory);
                await context.SaveChangesAsync();
                var createdInventoryDto = mapper.Map<InventoryDto>(inventory);

                return CreatedAtAction(nameof(GetInventoryById), new { id = inventory.Id }, createdInventoryDto);
            }
            catch (ArgumentNullException ex)
            {
                // Handle specific exception
                return BadRequest(new { message = "Invalid input data", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific exception
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Database operation failed", details = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryDto>> GetInventoryById(int id)
        {
            var inventory = await context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound();
            }

            var inventoryDto = mapper.Map<InventoryDto>(inventory);
            return Ok(inventoryDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateInventory(int id, InventoryDto inventoryDto)
        {
            if (id != inventoryDto.Id)
            {
                return BadRequest("Inventory ID mismatch");
            }

            var existingInventory = await context.Inventories.FindAsync(id);
            if (existingInventory == null)
            {
                return NotFound();
            }

            // Map updated properties
            mapper.Map(inventoryDto, existingInventory);

            try
            {
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency issues
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Database update failed", details = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred", details = ex.Message });
            }
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Inventory>> UpdateInventory(Inventory inventory, int id)
        {
            var find = await context.Inventories.FindAsync(id);
            if (find == null)
            {
                return BadRequest("Fail to find");
            }
            find.PropertyType=inventory.PropertyType;
            find.PropertyStatus=inventory.PropertyStatus;
            find.Address=inventory.Address;
            find.Location=inventory.Location;
            find.Floor=inventory.Floor;
            find.Bed=inventory.Bed;
            find.Rent=inventory.Rent;
            find.PlotSize=inventory.PlotSize;
            find.ParkFacing=inventory.ParkFacing;
            find.Lift=inventory.Lift;
            find.StiltParking=inventory.StiltParking;
            find.StaffRoom=inventory.StaffRoom;
            find.Remarks=inventory.Remarks;

            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Inventory>> DeleteInventory(int id)
        {
            var std = await context.Inventories.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.Inventories.Remove(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }

    }
}
