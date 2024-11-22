namespace LAB5.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int BrandId { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductName { get; set; }
        public string OtherProductDetails { get; set; }

        public Brand Brand { get; set; }
        public RefProductType RefProductType { get; set; }
        public ICollection<ProductColour> ProductColours { get; set; }
        public ICollection<RetailerProductPrice> RetailerProductPrices { get; set; }
        public ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; }
        public ICollection<SpecialOffer> SpecialOffers { get; set; }
    }
}
