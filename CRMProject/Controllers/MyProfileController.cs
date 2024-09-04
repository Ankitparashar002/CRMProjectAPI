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
        private readonly string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

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

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine(imagePath, file.FileName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // You can return the file URL or some other response here
            return Ok(new { FilePath = filePath });
        }


    }
}
