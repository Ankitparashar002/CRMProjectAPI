using CRMDashboardAPI.Models;
using CRMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRMLeadController : ControllerBase
    {
        private readonly TaskDbContext context;

        public CRMLeadController(TaskDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Lead>>> GetTask()
        {
            var data = await context.Leads.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Lead>> GetTaskById(int id)
        {
            var Task = await context.Leads.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Task;
        }



        [HttpPost]
        public async Task<ActionResult<Lead>> CreateTask(Lead std)
        {
            await context.Leads.AddAsync(std);
            await context.SaveChangesAsync();
            return Ok(std);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Lead>> UpdateTask(int id, Lead std)
        {
            if (id != std.Id)
            {
                return BadRequest();
            }
            context.Entry(std).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(std);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Lead>> DeleteTask(int id)
        {
            var std = await context.Leads.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.Leads.Remove(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }
    }
}
