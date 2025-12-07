using BookMotelsApplication.DTOs;
using BookMotelsApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/reserves")]
    public class ReservesController : ControllerBase
    {
        private readonly ReserveService _reserveService;

        public ReservesController(ReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReserveDTO>>> FindAllAsync()
        {
            var reserves = await _reserveService.FindAllAsync();
            return Ok(reserves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReserveDTO>> GetById(long id)
        {
            var reserve = await _reserveService.FindByIdAsync(id);

            return Ok(reserve);
        }

        [HttpPost]
        public async Task<ActionResult<ReserveDTO>> AddAsync(ReserveDTO reserveDto)
        {
            var newReserve = await _reserveService.AddAsync(reserveDto);
            return CreatedAtAction(nameof(GetById), new { id = newReserve.Id }, newReserve);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, ReserveDTO reserveDto)
        {
            await _reserveService.UpdateAsync(id, reserveDto);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _reserveService.DeleteAsync(id);
            return NoContent();
        }
    }
}
