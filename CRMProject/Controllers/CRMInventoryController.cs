using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
                .Where(i => customers.Any(c => c.Id == i.CustomerId && c.InventoryStatus == true))
                .ToList();

            // Map filtered inventories to InventoryDto
            var inventoryDtos = mapper.Map<List<InventoryDto>>(filteredInventories);

            return Ok(inventoryDtos);
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> CreateTask(Inventory std)
        {
            await context.Inventories.AddAsync(std);
            await context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaskById), new { id = std.Id }, std);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetTaskById(int id)
        {
            var Task = await context.Inventories.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Task;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Inventory>> UpdateTask(Inventory inventory, int id)
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
        public async Task<ActionResult<Inventory>> DeleteTask(int id)
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
