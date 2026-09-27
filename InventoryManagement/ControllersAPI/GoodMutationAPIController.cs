using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodMutationAPIController : ControllerBase
    {
        private readonly IGoodMutationService _goodMutationService;

        public GoodMutationAPIController(IGoodMutationService goodMutationService)
        {
            _goodMutationService = goodMutationService;
        }

        [HttpPost("Mutation")]
        public async Task<IActionResult> Mutation([FromBody] VmGoodMutation vmGoodMutation)
        {
            await _goodMutationService.Mutation(vmGoodMutation);
            return Ok();
        }

        [HttpGet("MutationHistory")]
        public async Task<IActionResult> MutationHistory([FromQuery] string? keyword, [FromQuery] int cursor = 0)
        {
            var mutationHistory = await _goodMutationService.MutationHistory(keyword, cursor);
            return Ok(mutationHistory);
        }
    }
}
