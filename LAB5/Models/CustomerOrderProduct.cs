namespace LAB5.Models
{
    public class CustomerOrderProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public int Quantity { get; set; }
        public string OtherDetails { get; set; }

        public CustomerOrder CustomerOrder { get; set; }
        public Product Product { get; set; }
        public Retailer Retailer { get; set; }
    }
}
