using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechGalaxyProject.Data;
using TechGalaxyProject.Data.Models;
using TechGalaxyProject.Models;

namespace TechGalaxyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        public FieldsController (AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;

        [HttpPost]
        [Authorize(Roles = "Expert")]
        public async Task<IActionResult> AddField(dtoAddField dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var roadmap = await _db.roadmaps.FindAsync(dto.RoadmapId);
            if (roadmap == null)
                return NotFound("Roadmap not found");

           // if (!int.TryParse(userId, out int parsedUserId))
              //  return Unauthorized();  

            if (!(roadmap.CreatedBy .Equals( userId)))
                return Forbid("You can only add fields to your own roadmaps");


            var field = new Field
            {
                Title = dto.Title,
                Description = dto.Description,
                RoadmapId = dto.RoadmapId
            };

            _db.fields.Add(field);
            await _db.SaveChangesAsync();

            return Ok(field);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Expert")]
        public async Task<IActionResult> EditField(int id, dtoEditField dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var field = await _db.fields.Include(f => f.roadmap).FirstOrDefaultAsync(f => f.Id == id);
            if (field == null)
                return NotFound("Field not found");

            //if (!int.TryParse(userId, out int parsedUserId))
                //return Unauthorized(); 

            if (!(field.roadmap.CreatedBy .Equals( userId)))
                return Forbid("You can only edit your own fields");


            field.Title = dto.Title;
            field.Description = dto.Description;

            await _db.SaveChangesAsync();

            return Ok(field);
        }
        [HttpGet("by-roadmap/{roadmapId}")]
        public async Task<IActionResult> GetFieldsByRoadmap(int roadmapId)
        {
            var fields = await _db.fields
                .Where(f => f.RoadmapId == roadmapId)
                .ToListAsync();

            return Ok(fields);
        }
    }
}
