
namespace MedicineStore.API.Data
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMedicineStoreRepository
    {
        Task<IEnumerable<Medicine>> GetAllMedicines();
        Task<Medicine> GetMedicine(int id);
        Task<Image> GetImage(int id);
        Task<bool> SaveAll();
    }
}
