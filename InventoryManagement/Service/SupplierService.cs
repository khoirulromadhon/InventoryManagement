using InventoryManagement.Models;
using InventoryManagement.Service.Interface;
using InventoryManagement.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Service
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;

        public SupplierService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteSupplier(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);

                if (supplier == null)
                {
                    throw new Exception($"Supplier with ID {id} not found.");
                }

                supplier.IsDelete = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while deleting the supplier.", ex);
            }
        }

        public async Task<List<VmSupplier>> GetAllSuppliers(string keyword, int cursor)
        {
            try
            {
                var suppliers = await _context.Database
                .SqlQuery<VmSupplier>($"""
                    EXEC GetSupplier
                       @Keyword = {keyword},  
                       @CursorId = {cursor}
                    """)
                .ToListAsync();

                return suppliers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving suppliers.", ex);
            }
        }

        public async Task<VmSupplier> GetSupplierById(int id)
        {
            try
            {
                var supplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == id);

                if (supplier == null)
                {
                    throw new Exception($"Supplier with ID {id} not found.");
                }

                VmSupplier vmSupplier = new VmSupplier
                {
                    SupplierId = supplier.SupplierId,
                    SupplierName = supplier.SupplierName
                };

                return vmSupplier;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving suppliers.", ex);
            }
        }

        public async Task InsertSupplier(VmSupplier vmSupplier)
        {
            try
            {
                if (vmSupplier.SupplierId == null || vmSupplier.SupplierId == 0)
                {
                    Supplier supplier = new Supplier
                    {
                        SupplierName = vmSupplier.SupplierName
                    };

                    _context.Suppliers.Add(supplier);
                }
                else
                {
                    Supplier existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(x => x.SupplierId == vmSupplier.SupplierId);

                    if (existingSupplier == null)
                    {
                        throw new InvalidOperationException("Supplier not found");
                    }

                    existingSupplier.SupplierName = vmSupplier.SupplierName;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving suppliers.", ex);
            }
        }
    }
}
