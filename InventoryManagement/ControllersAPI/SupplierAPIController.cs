using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.ControllersAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierAPIController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierAPIController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost("InsertSupplier")]
        public async Task<IActionResult> InsertSupplier([FromBody] VmSupplier vmSupplier)
        {
            await _supplierService.InsertSupplier(vmSupplier);
            return Ok();
        }

        [HttpGet("GetAllSuppliers")]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] string? keyword, [FromQuery] int cursor = 0)
        {
            var suppliers = await _supplierService.GetAllSuppliers(keyword, cursor);
            return Ok(suppliers);
        }

        [HttpGet("GetSupplierById/{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var supplier = await _supplierService.GetSupplierById(id);
            return Ok(supplier);
        }

        [HttpDelete("DeleteSupplier/{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            await _supplierService.DeleteSupplier(id);
            return Ok();
        }
    }
}
