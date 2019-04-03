
using MedicineStore.API.Models;
using System.Threading.Tasks;

namespace MedicineStore.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public class MedicineStoreRepository : IMedicineStoreRepository
    {
        private readonly DataContext _context;

        public MedicineStoreRepository(DataContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Medicine>> GetAllMedicines()
        {
            return await _context.Medicines.ToListAsync();
        }
    }
}
