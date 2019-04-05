
namespace MedicineStore.API.Data
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMedicineStoreRepository
    {
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
        Task<Medicine> GetMedicineAsync(int id);
        Task<Image> GetImageAsync(int id);
        Task<bool> SaveAllAsync();
        Task AddMedicineAsync(Medicine medicine);
        Task DeleteAsync(int id);
    }
}
