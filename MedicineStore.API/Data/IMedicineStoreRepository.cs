
namespace MedicineStore.API.Data
{
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMedicineStoreRepository
    {
        void Delete<T>(T entity) where T : class;
        Task<IEnumerable<Medicine>> GetAllMedicinesAsync();
        Task<Medicine> GetMedicineAsync(int id);
        Task<Image> GetImageAsync(int id);
        Task<bool> SaveAllAsync();
        Task AddMedicineAsync(Medicine medicine);
        Task DeleteAsync(int id);
        Task<Image> GetMainImageAsync(int medicineId);
        Task<Image> GetImageAsync(int id, string imageId);
        Task<Image> GetImageAsync(string imagePublicId);
    }
}
