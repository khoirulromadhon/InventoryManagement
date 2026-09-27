using InventoryManagement.Models;
using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Service
{
    public class GoodService : IGoodService
    {
        private readonly AppDbContext _context;

        public GoodService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VmGood>> GetAllGoods(string keyword, int cursor)
        {
            try
            {
                var goods = await _context.Database
                .SqlQuery<VmGood>($"""
                    EXEC GetGood
                       @Keyword = {keyword},  
                       @CursorId = {cursor}
                    """)
                .ToListAsync();

                return goods;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving goods.", ex);
            }
        }
    }
}
