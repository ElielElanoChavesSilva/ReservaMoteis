using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MotelsController : ApiController
    {
        private readonly IMotelService _motelService;
        private readonly ISuiteService _suiteService;

        public MotelsController(
            IMotelService motelService,
            ISuiteService suiteService)
        {
            _motelService = motelService;
            _suiteService = suiteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMotelDTO>>> FindAllAsync()
        {
            var motels = await _motelService.FindAllAsync();
            return Ok(motels);
        }

        [HttpGet("{motelId}")]
        public async Task<ActionResult<GetMotelDTO>> FindById(long motelId)
        {
            var motel = await _motelService.FindByIdAsync(motelId);

            return Ok(motel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<GetMotelDTO>> AddAsync(MotelDTO motelDto)
        {
            var newMotel = await _motelService.AddAsync(motelDto);
            return CreatedAtAction(nameof(FindById), new { motelId = newMotel.Id }, newMotel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{motelId}")]
        public async Task<IActionResult> UpdateAsync(long motelId, MotelDTO motelDto)
        {
            await _motelService.UpdateAsync(motelId, motelDto);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{motelId}")]
        public async Task<IActionResult> DeleteAsync(long motelId)
        {
            await _motelService.DeleteAsync(motelId);
            return NoContent();
        }

        [HttpGet("{motelId}/suites/available")]
        public async Task<ActionResult<IEnumerable<GetSuiteDTO>>> FindAllAvailable(long motelId, [FromQuery] string? name,
            [FromQuery] DateTime? checkin, [FromQuery] DateTime? checkout)
        {
            var suites = await _suiteService.FindAllAvailable(motelId, name, checkin, checkout);
            return Ok(suites);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{motelId}/suites")]
        public async Task<ActionResult<GetSuiteDTO>> AddAsync(long motelId, SuiteDTO suiteDto)
        {
            var newSuite = await _suiteService.AddAsync(motelId, suiteDto);
            return Created($"api/Suites/{newSuite.Id}", newSuite.Id);
        }
    }
}
