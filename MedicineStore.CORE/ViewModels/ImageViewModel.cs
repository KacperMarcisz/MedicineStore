namespace MedicineStore.CORE.ViewModels
{
    using System;
    using Microsoft.AspNetCore.Http;

    public class ImageViewModel
    {
        public ImageViewModel()
        {
            DateAdded = DateTime.Now;
        }

        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string PublicId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
