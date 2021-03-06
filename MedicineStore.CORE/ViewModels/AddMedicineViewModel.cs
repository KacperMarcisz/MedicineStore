﻿namespace MedicineStore.CORE.ViewModels
{
    public class AddMedicineViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal SpecialGrossPrice { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
