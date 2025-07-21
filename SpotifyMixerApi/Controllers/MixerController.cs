using Microsoft.AspNetCore.Mvc;
using SpotifyMixerApi.Models;

namespace SpotifyMixerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MixerController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetMixerById(int id)
        {
            return Ok(new { Id = id, Name = $"Mixer {id}" });
        }

        [HttpPost]
        public IActionResult CreateMixer([FromBody] Mixer mixer)
        {
            // In a real app, you would save the mixer to a database
            return CreatedAtAction(nameof(GetMixerById), new { id = mixer.Id }, mixer);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMixer(int id, [FromBody] Mixer mixer)
        {
            // In a real app, you would update the mixer in a database
            if (id != mixer.Id)
            {
                return BadRequest("ID in URL and body do not match.");
            }
            return Ok(mixer);
        }
    }
} 