using BookMotelsApplication.DTOs.Suite;
using BookMotelsApplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookMotelsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuitesController : ControllerBase
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

        [HttpPost]
        public async Task<ActionResult<GetSuiteDTO>> AddAsync(SuiteDTO suiteDto)
        {
            var newSuite = await _suiteService.AddAsync(suiteDto);
            return CreatedAtAction(nameof(FindById), new { id = newSuite.Id }, newSuite);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, SuiteDTO suiteDto)
        {
            await _suiteService.UpdateAsync(id, suiteDto);
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
