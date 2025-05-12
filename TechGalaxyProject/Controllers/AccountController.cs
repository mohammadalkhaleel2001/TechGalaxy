/*******************************************
 * AccountController - Cleaned to Match UI *
 *******************************************/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechGalaxyProject.Data;
using TechGalaxyProject.Data.Models;
using TechGalaxyProject.Models;

namespace TechGalaxyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            AppDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _db = db;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNewUser([FromForm] dtoNewUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.Role != "Expert" && user.Role != "Learner")
                return BadRequest("Role must be either 'Expert' or 'Learner'");

            if (user.Role == "Expert")
            {
                if (string.IsNullOrWhiteSpace(user.Specialty))
                    return BadRequest("Specialty is required for Experts.");

                if (user.CertificateFile == null || user.CertificateFile.Length == 0)
                    return BadRequest("Certificate file is required for Experts.");
            }

            var appUser = new AppUser
            {
                UserName = user.userName,
                Email = user.email,
                Role = user.Role,
                IsVerified = user.Role == "Expert" ? false : true
            };

            var result = await _userManager.CreateAsync(appUser, user.password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(user.Role))
                await _roleManager.CreateAsync(new IdentityRole(user.Role));

            await _userManager.AddToRoleAsync(appUser, user.Role);

            if (user.Role == "Expert")
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "certificates");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(user.CertificateFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await user.CertificateFile.CopyToAsync(stream);
                }

                var verificationRequest = new ExpertVerificationRequest
                {
                    UserId = appUser.Id,
                    Specialty = user.Specialty!,
                    CertificatePath = "/uploads/certificates/" + fileName,
                    SubmittedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _db.ExpertVerificationRequests.Add(verificationRequest);
                await _db.SaveChangesAsync();
            }

            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(login.userName);
            if (user == null)
                return BadRequest("User not found");

            if (!await _userManager.CheckPasswordAsync(user, login.password))
                return Unauthorized("Invalid password");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = _configuration["JWT:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                return BadRequest("JWT Secret Key not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.UserName,
                user.Email,
                user.Role
            });
        }

        [HttpPost("CheckUserExists")]
        public async Task<IActionResult> CheckUserExists(dtoCheckUser user)
        {
            var existingUser = await _userManager.FindByNameAsync(user.userName);
            return Ok(new { exists = existingUser != null });
        }
    }
}
