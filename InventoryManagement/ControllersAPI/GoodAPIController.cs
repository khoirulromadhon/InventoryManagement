using InventoryManagement.Service;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoodAPIController : ControllerBase
    {
        private readonly IGoodService _goodService;

        public GoodAPIController(IGoodService goodService)
        {
            _goodService = goodService;
        }

        [HttpGet("GetAllGoods")]
        public async Task<IActionResult> GetAllGoods([FromQuery] string? keyword, [FromQuery] int cursor = 0)
        {
            var goods = await _goodService.GetAllGoods(keyword, cursor);
            return Ok(goods);
        }
    }
}
