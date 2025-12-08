using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuitesController : ApiController
    {
        private readonly ISuiteService _suiteService;

        public SuitesController(ISuiteService suiteService)
        {
            _suiteService = suiteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSuiteDTO>>> FindAllAsync()
        {
            var suites = await _suiteService.FindAllAsync();
            return Ok(suites);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSuiteDTO>> FindById(long id)
        {
            var suite = await _suiteService.FindByIdAsync(id);

            return Ok(suite);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            await _suiteService.UpdateAsync(id, suiteDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _suiteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
