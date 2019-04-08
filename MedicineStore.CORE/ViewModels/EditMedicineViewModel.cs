namespace MedicineStore.CORE.ViewModels
{
    using System.Collections.Generic;

    public class EditMedicineViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal SpecialGrossPrice { get; set; }
        public List<ImageViewModel> Images { get; set; }
    }
}
