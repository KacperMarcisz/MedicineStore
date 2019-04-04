namespace MedicineStore.CORE.ViewModels
{
    public class MedicineHeaderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal SpecialGrossPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
