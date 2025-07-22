using Microsoft.AspNetCore.Mvc;
using SpotifyMixerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SpotifyMixerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MixerController : ControllerBase
    {
        private readonly MixerDbContext _context;

        public MixerController(MixerDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMixerById(int id)
        {
            var mixer = await _context.Mixers.FindAsync(id);
            if (mixer == null)
            {
                return NotFound();
            }

            return Ok(mixer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMixer([FromBody] Mixer mixer)
        {
            if (await _context.Mixers.AnyAsync(m => m.Id == mixer.Id))
            {
                return Conflict($"Mixer with ID {mixer.Id} already exists.");
            }

            _context.Mixers.Add(mixer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMixerById), new { id = mixer.Id }, mixer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMixer(int id, [FromBody] Mixer mixer)
        {
            if (id != mixer.Id)
            {
                return BadRequest("ID in URL and body do not match.");
            }

            var existing = await _context.Mixers.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = mixer.Name;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }
    }
} 