using CRMProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyProfileController : ControllerBase
    {
        private readonly TaskDbContext context;

        public MyProfileController(TaskDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<MyProfile>> GetProfile()
        {
            var data = await context.MyProfile.ToListAsync();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<MyProfile>> CreateProfile(MyProfile profile)
        {
            await context.MyProfile.AddAsync(profile);
            await context.SaveChangesAsync();
            return Ok(profile);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MyProfile>> GetProfile(int id)
        {
            var profile = await context.MyProfile.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<MyProfile>> CreateProfile(MyProfile profile,int id)
        {
           
            var find = await context.MyProfile.FindAsync(id);
            if (find == null)
            {
                return BadRequest("fail");
            }
            if (profile.FirstName != null)
            {
                find.FirstName = profile.FirstName;
                find.LastName = profile.LastName;
                find.Email = profile.Email;
                find.Mobile = profile.Mobile;
                find.Address = profile.Address;
                find.City = profile.City;
            }
            find.ProfileUrl = profile.ProfileUrl;   
            //  context.Customers.Update(customer);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
