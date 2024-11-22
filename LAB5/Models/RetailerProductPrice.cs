namespace LAB5.Models
{
    public class RetailerProductPrice
    {
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public Product Product { get; set; }
        public Retailer Retailer { get; set; }
    }
}
