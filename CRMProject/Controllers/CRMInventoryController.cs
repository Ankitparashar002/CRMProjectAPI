using CRMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMInventoryController : ControllerBase
    {
        private readonly TaskDbContext context;

        public CRMInventoryController(TaskDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Inventory>>> GetTask()
        {
            var data = await context.Inventories.ToListAsync();
            return Ok(data);
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
            return Ok();
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
