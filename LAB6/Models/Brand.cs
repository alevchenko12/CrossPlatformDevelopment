using Microsoft.CodeAnalysis;

namespace LAB6.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
        public string OtherBrandDetails { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
