namespace MedicineStore.CORE.ViewModels
{
    using System;

    public class ImageViewModel
    {
        public ImageViewModel()
        {
            DateAdded = DateTime.Now;
        }

        public string Url { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
