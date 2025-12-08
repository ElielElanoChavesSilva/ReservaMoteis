using BookMotelsApplication.DTOs.Reserve;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservesController : ApiController
    {
        private readonly IReserveService _reserveService;

        public ReservesController(IReserveService reserveService)
        {
            _reserveService = reserveService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<GetReserveDTO>>> FindAllAsync()
        {
            var reserves = await _reserveService.FindAllAsync();
            return Ok(reserves);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetReserveDTO>>> FindAllByUserAsync()
        {
            var reserves = await _reserveService.FindAllByUserAsync(LoggedUserId);
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
            var newReserve = await _reserveService.AddAsync(LoggedUserId, reserveDto);
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
