using Microsoft.AspNetCore.Mvc;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;

namespace SpotifyMixerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MixerController : ControllerBase
    {
        private readonly IMixerRepository _repository;

        public MixerController(IMixerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMixerById(string id)
        {
            var mixer = await _repository.GetByIdAsync(id);
            if (mixer == null)
            {
                return NotFound();
            }

            return Ok(mixer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMixer([FromBody] Mixer mixer)
        {
            var existing = await _repository.GetByIdAsync(mixer.Id);
            if (existing != null)
            {
                return Conflict($"Mixer with ID {mixer.Id} already exists.");
            }

            await _repository.AddAsync(mixer);
            return CreatedAtAction(nameof(GetMixerById), new { id = mixer.Id }, mixer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMixer(string id, [FromBody] Mixer mixer)
        {
            if (id != mixer.Id)
            {
                return BadRequest("ID in URL and body do not match.");
            }

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(mixer);
            return Ok(mixer);
        }
    }
} 