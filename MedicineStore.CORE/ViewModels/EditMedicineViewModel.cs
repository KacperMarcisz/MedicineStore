namespace MedicineStore.CORE.ViewModels
{
    public class EditMedicineViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal SpecialGrossPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
