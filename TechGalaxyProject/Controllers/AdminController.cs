using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechGalaxyProject.Data;

namespace TechGalaxyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminController(AppDbContext db)
        {
            _db = db;
        }

        
        [HttpGet("verification-requests")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var requests = await _db.ExpertVerificationRequests
                .Include(r => r.User)
                .Where(r => r.status == "Pending")
                .Select(r => new
                {
                    r.Id,
                    ExpertId = r.User.Id,
                    ExpertUsername = r.User.UserName,
                    r.SubmittedAt
                })
                .ToListAsync();

            return Ok(requests);
        }

        
        [HttpPost("approve/{requestId}")]
        public async Task<IActionResult> ApproveRequest(int requestId)
        {
            var request = await _db.ExpertVerificationRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                return NotFound("Request not found");

            request.status = "Approved";
            request.ReviewedAt = DateTime.Now;
            request.ReviewedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            request.User.IsVerified = true;

            await _db.SaveChangesAsync();
            return Ok("Expert approved and verified.");
        }

        
        [HttpPost("reject/{requestId}")]
        public async Task<IActionResult> RejectRequest(int requestId, [FromBody] string adminNote)
        {
            var request = await _db.ExpertVerificationRequests
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request == null)
                return NotFound("Request not found");

            request.status = "Rejected";
            request.ReviewedAt = DateTime.Now;
            request.ReviewedBy = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            request.AdminNote = adminNote;

            await _db.SaveChangesAsync();
            return Ok("Verification request rejected.");
        }
    }
}

