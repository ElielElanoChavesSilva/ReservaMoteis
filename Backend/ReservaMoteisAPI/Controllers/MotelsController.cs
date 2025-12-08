using BookMotelsApplication.DTOs.Motel;
using BookMotelsApplication.Interfaces;
using BookMotelsDomain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MotelsController : ControllerBase
    {
        private readonly IMotelService _motelService;

        public MotelsController(IMotelService motelService)
        {
            _motelService = motelService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMotelDTO>>> FindAllAsync()
        {
            var motels = await _motelService.FindAllAsync();
            return Ok(motels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetMotelDTO>> FindById(long id)
        {
            var motel = await _motelService.FindByIdAsync(id);

            return Ok(motel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("billing-report")]
        public async Task<ActionResult<IEnumerable<BillingReportDTO>>> FindBillingReport([FromQuery] long? motelId,
            [FromQuery] int? year, [FromQuery] int? month)
        {
            var report = await _motelService.FindBillingReportAsync(motelId, year, month);
            return Ok(report);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<GetMotelDTO>> AddAsync(MotelDTO motelDto)
        {
            var newMotel = await _motelService.AddAsync(motelDto);
            return CreatedAtAction(nameof(FindById), new { id = newMotel.Id }, newMotel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, GetMotelDTO motelDto)
        {
            await _motelService.UpdateAsync(id, motelDto);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _motelService.DeleteAsync(id);
            return NoContent();
        }
    }
}
