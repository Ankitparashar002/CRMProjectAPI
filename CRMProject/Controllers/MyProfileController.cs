using CRMProject.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRMProject.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using AutoMapper;

namespace CRMProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyProfileController : ControllerBase
    {
        private readonly TaskDbContext context;
        private readonly IConfiguration configuration;
        private readonly string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        private readonly IMapper mapper;

        public MyProfileController(TaskDbContext context, IConfiguration configuration, IMapper _mapper)
        {
            this.context = context;
            this.configuration = configuration;
            this.mapper = _mapper; 
            
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<MyProfileDto>> GetProfile()
        {
            var data = await context.MyProfile.ToListAsync();
            var customerDtos = mapper.Map<List<MyProfileDto>>(data);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult<MyProfile>> CreateProfile(MyProfile profile)
        {
           

            if (!string.IsNullOrEmpty(profile.Password))
            {
                int salt = 12;
                profile.Password = BCrypt.Net.BCrypt.HashPassword(profile.Password, salt);
            }
          
            

            await context.MyProfile.AddAsync(profile);
            await context.SaveChangesAsync();
            return Ok(profile);
        }

        [Authorize]
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
        public async Task<ActionResult<MyProfileDto>> UpdateProfile(MyProfileDto profile,int id)
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
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("update-url/{id}")]
        public async Task<ActionResult> UpdateProfileUrl(int id, UrlDto urlDto)
        {
            var find = await context.MyProfile.FindAsync(id);
            if (find == null)
            {
                return BadRequest("Profile not found");
            }

            if (!string.IsNullOrWhiteSpace(urlDto.ProfileUrl))
            {
                find.ProfileUrl = urlDto.ProfileUrl;
                await context.SaveChangesAsync();
                return Ok("Profile URL updated successfully");
            }

            return BadRequest("Invalid URL");
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest("Invalid login attempt.");
            }

            var normalizedEmail = dto.Email.ToLower();
            Console.WriteLine($"Attempting to find user with email: {normalizedEmail}");

            var user = await context.MyProfile.SingleOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail);

            if (user == null)
            {
                Console.WriteLine($"User with email {normalizedEmail} not found.");
                return Unauthorized("Email not found.");
            }

            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

            if (!isPasswordCorrect)
            {
                Console.WriteLine($"Password for user with email {normalizedEmail} is incorrect.");
                return Unauthorized("Incorrect password.");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user.Email);

            return Ok(token);
        }


        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User not authenticated");
            }

            var user = context.MyProfile.SingleOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                throw new Exception("User not found");
            }
            var refreshToken = Request.Cookies["refreshToken"];
            if(!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token");
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken ,user.Email);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                Console.WriteLine("User email is null or empty.");
                return BadRequest("User is not authenticated.");
            }

            var user = await context.MyProfile.SingleOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Clear the refresh token from the user record
            user.RefreshToken = string.Empty;
            user.TokenCreated = new DateTime(1753,1,1); 
            user.TokenExpires = new DateTime(1753, 1, 1);

            // Save the changes in the database
            await context.SaveChangesAsync();

            // Delete the refresh token cookie
            if (Request.Cookies.ContainsKey("refreshToken"))
            {
                Response.Cookies.Delete("refreshToken");
            }

            // Optionally, forcefully expire all cookies (if you have more)
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // The frontend should be responsible for removing the access token (JWT) from localStorage/sessionStorage.

            return Ok("Logout successful.");
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.NewPassword) || string.IsNullOrEmpty(dto.ConfirmPassword))
            {
                return BadRequest("All fields are required.");
            }

            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return BadRequest("New password and confirmation password do not match.");
            }

            var userEmail = User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = await context.MyProfile.SingleOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest("Current password is incorrect.");
            }

            // Hash and set new password
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await context.SaveChangesAsync();

            return Ok("Password changed successfully.");
        }



        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created =DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken , string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail))
            {
                throw new Exception("User email is not provided.");
            }

            var user = context.MyProfile.SingleOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite =SameSiteMode.None,
                Expires = newRefreshToken.Expires,
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            if (newRefreshToken.Created < new DateTime(1753, 1, 1))
            {
                newRefreshToken.Created = DateTime.Now;
            }

            if (newRefreshToken.Expires < new DateTime(1753, 1, 1))
            {
                newRefreshToken.Expires = DateTime.Now.AddDays(7);
            }

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            context.SaveChanges();
        }

        private string CreateToken(MyProfile user)
        {
            List<Claim>claims = new List<Claim> { 
            new Claim(ClaimTypes.Name,user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]));

            var creds =new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var token =new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


    }
}
