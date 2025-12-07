using BookMotelsApplication.DTOs;
using BookMotelsApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MotelsController : ControllerBase
    {
        private readonly MotelService _motelService;

        public MotelsController(MotelService motelService)
        {
            _motelService = motelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MotelDTO>>> GetAllAsync()
        {
            var motels = await _motelService.FindAllAsync();
            return Ok(motels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MotelDTO>> FindByIdAsync(long id)
        {
            var motel = await _motelService.FindByIdAsync(id);

            return Ok(motel);
        }

        [HttpPost]
        public async Task<ActionResult<MotelDTO>> AddAsync(MotelDTO motelDto)
        {
            var newMotel = await _motelService.AddAsync(motelDto);
            return CreatedAtAction(nameof(FindByIdAsync), new { id = newMotel.Id }, newMotel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, MotelDTO motelDto)
        {
            await _motelService.UpdateAsync(id, motelDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _motelService.DeleteAsync(id);
            return NoContent();
        }
    }
}
