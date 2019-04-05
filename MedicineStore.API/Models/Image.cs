namespace MedicineStore.API.Models
{
    using System;

    public class Image
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public int MedicineId { get; set; }
        public virtual Medicine Medicine { get; set;}
        public string Url { get; set; }
        public string PublicId { get; set; }
    }
}
