using System.Collections.Generic;

namespace MedicineStore.API.Data
{
    using Models;
    using Newtonsoft.Json;

    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
        }

        public void SeedMedicines()
        {
            var medicineData = System.IO.File.ReadAllText("Data/MedicinesSeedData.json");
            var medicines = JsonConvert.DeserializeObject<List<Medicine>>(medicineData);

            _context.Medicines.AddRange(medicines);
            _context.SaveChanges();
        }

    }
}
