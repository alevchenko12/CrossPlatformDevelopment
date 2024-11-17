using Microsoft.CodeAnalysis;

namespace LAB6.Models
{
    public class RefProductType
    {
        public string ProductTypeCode { get; set; }
        public string ParentProductCode { get; set; }
        public string ProductTypeDescription { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
