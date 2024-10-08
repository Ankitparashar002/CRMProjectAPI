﻿using CRMProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CRMDashboardController : ControllerBase
    {
        private readonly TaskDbContext context;

        public CRMDashboardController(TaskDbContext context)
        {
            this.context = context;
            
        }

       
        [HttpGet]
        public async Task<ActionResult<List<TaskDashboard>>> GetTask()
        {
            var data = await context.TaskDashboards.ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<TaskDashboard>> CreateTask(TaskDashboard std)
        {
            await context.TaskDashboards.AddAsync(std);
            await context.SaveChangesAsync();
            return Ok(std);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDashboard>> GetTaskById(int id)
        {
            var Task = await context.TaskDashboards.FindAsync(id);
            if (Task == null)
            {
                return NotFound();
            }
            return Task;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<TaskDashboard>> UpdateTask(TaskDashboard taskDashboard,int id)
        {
            var find = await context.TaskDashboards.FindAsync(id);
            if (find == null)
            {
                return BadRequest("Fail to show");
            }

            find.Task=taskDashboard.Task;
            find.Type=taskDashboard.Type;
            find.Labels=taskDashboard.Labels;

            await context.SaveChangesAsync();
            return Ok();

        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskDashboard>> DeleteTask(int id)
        {
            var std = await context.TaskDashboards.FindAsync(id);
            if (std == null)
            {
                return NotFound();
            }
            context.TaskDashboards.Remove(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }
    }
}
