using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservesController : ControllerBase
    {
        private readonly IReserveService _reserveService;

        public ReservesController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetReserveDTO>>> FindAllAsync()
        {
            var reserves = await _reserveService.FindAllAsync();
            return Ok(reserves);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetReserveDTO>> FindById(long id)
        {
            var reserve = await _reserveService.FindByIdAsync(id);

            return Ok(reserve);
        }

        [HttpPost]
        public async Task<ActionResult<ReserveDTO>> AddAsync(ReserveDTO reserveDto)
        {
            var newReserve = await _reserveService.AddAsync(reserveDto);
            return CreatedAtAction(nameof(FindById), new { id = newReserve.Id }, newReserve);
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
