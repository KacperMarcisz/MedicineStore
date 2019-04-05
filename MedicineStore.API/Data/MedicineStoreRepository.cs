
using MedicineStore.API.Models;
using System.Threading.Tasks;

namespace MedicineStore.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class MedicineStoreRepository : IMedicineStoreRepository
    {
        private readonly DataContext _context;

        public MedicineStoreRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicinesAsync()
        {
            return await _context.Medicines.Include(x => x.Images).Where(x => x.IsActive && !x.IsDeleted).ToListAsync();
        }

        public async Task AddMedicineAsync(Medicine medicine)
        {
            await _context.Medicines.AddAsync(medicine);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var medicine = _context.Medicines.Single(x => x.Id == id);
            medicine.IsDeleted = true;

            await _context.SaveChangesAsync();
        }

        public async Task<Medicine> GetMedicineAsync(int id)
        {
            return await _context.Medicines.SingleAsync(x => x.Id == id);
        }

        public async Task<Image> GetImageAsync(int id)
        {
            return await _context.Images.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
