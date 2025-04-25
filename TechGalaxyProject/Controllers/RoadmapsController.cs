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
    
    public class RoadmapsController : ControllerBase
    {
        public RoadmapsController (AppDbContext db)
        {
            _db = db;
        }
        private readonly AppDbContext _db;
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roadmaps = await _db.roadmaps
                .Include(r => r.fields.OrderBy(f => f.Order))
                .Select(r => new RoadmapDetailsDto
                {
                    Id = r.Id,
                    Title = r.Title,
                    Description = r.Description,
                    Fields = r.fields.Select(f => new FieldDto
                    {
                        Id = f.Id,
                        Title = f.Title,
                        Description = f.Description,
                        Order = f.Order
                    }).ToList()
                })
                .ToListAsync();

            return Ok(roadmaps);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var roadmap = await _db.roadmaps
                .Include(r => r.fields.OrderBy(f => f.Order))
                .FirstOrDefaultAsync(r => r.Id == id);

            if (roadmap == null)
                return NotFound();

            var dto = new RoadmapDetailsDto
            {
                Id = roadmap.Id,
                Title = roadmap.Title,
                Description = roadmap.Description,
                Fields = roadmap.fields.Select(f => new FieldDto
                {
                    Id = f.Id,
                    Title = f.Title,
                    Description = f.Description,
                    Order = f.Order
                }).ToList()
            };

            return Ok(dto);
        }
        [HttpPost]
        [Authorize(Roles = "Expert")]

        public async Task<IActionResult> Create([FromBody] CreateRoadmapDto dto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roadmap = new Roadmap
            {
                Title = dto.Title,
                Description = dto.Description,
                CreatedBy=userId,
                
            };

            _db.roadmaps.Add(roadmap);
            await _db.SaveChangesAsync();

            return Ok(new { roadmap.Id });
        }
    }

}

