using Microsoft.AspNetCore.Mvc;
using SpotifyMixerApi.Models;
using SpotifyMixerApi.Repositories;
using SpotifyMixerApi.Services;

namespace SpotifyMixerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MixersController : ControllerBase
    {
        private readonly IMixerRepository _repository;
        private readonly IPlaylistOrchestrator _playlistOrchestrator;

        public MixersController(IMixerRepository repository, IPlaylistOrchestrator playlistOrchestrator)
        {
            _repository = repository;
            _playlistOrchestrator = playlistOrchestrator;
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

        [HttpGet]
        public async Task<IActionResult> GetAllMixers()
        {
            var mixers = await _repository.GetAllAsync();
            return Ok(mixers);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMixer([FromBody] Mixer mixer)
        {
            var existing = await _repository.GetByIdAsync(mixer.id);
            if (existing != null)
            {
                return Conflict($"Mixer with ID {mixer.id} already exists.");
            }

            await _repository.AddAsync(mixer);
            return CreatedAtAction(nameof(GetMixerById), new { id = mixer.id }, mixer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMixer(string id, [FromBody] Mixer mixer)
        {
            if (id != mixer.id)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMixer(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/mix")]
        public async Task<IActionResult> MixPlaylist(string id)
        {
            var mixer = await _repository.GetByIdAsync(id);
            if (mixer == null)
            {
                return NotFound($"Mixer with id '{id}' not found");
            }

            if (string.IsNullOrEmpty(mixer.SrcPlaylistId))
            {
                return BadRequest("Mixer does not have a source playlist ID configured");
            }

            try
            {
                var mixedTracks = await _playlistOrchestrator.MixPlaylistAsync(mixer.SrcPlaylistId, mixer);
                return Ok(new { 
                    mixerId = id, 
                    sourcePlaylistId = mixer.SrcPlaylistId, 
                    trackCount = mixedTracks.Count,
                    tracks = mixedTracks 
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 