using BookMotelsApplication.DTOs;
using BookMotelsApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/suites")]
    public class SuitesController : ControllerBase
    {
        private readonly SuiteService _suiteService;

        public SuitesController(SuiteService suiteService)
        {
            _suiteService = suiteService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuiteDTO>>> FindAllAsync()
        {
            var suites = await _suiteService.FindAllAsync();
            return Ok(suites);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SuiteDTO>> FindByIdAsync(long id)
        {
            var suite = await _suiteService.FindByIdAsync(id);

            return Ok(suite);
        }

        [HttpPost]
        public async Task<ActionResult<SuiteDTO>> AddAsync(SuiteDTO suiteDto)
        {
            var newSuite = await _suiteService.AddAsync(suiteDto);
            return CreatedAtAction(nameof(FindByIdAsync), new { id = newSuite.Id }, newSuite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            var updatedSuite = await _suiteService.UpdateAsync(id, suiteDto);
            if (updatedSuite == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            await _suiteService.DeleteAsync(id);
            return NoContent();
        }
    }
}
