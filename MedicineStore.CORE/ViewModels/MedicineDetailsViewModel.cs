namespace MedicineStore.CORE.ViewModels
{
    public class MedicineDetailsViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal SpecialGrossPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
