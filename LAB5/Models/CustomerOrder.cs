namespace LAB5.Models
{
    public class CustomerOrder
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string OrderStatusCode { get; set; }
        public string OrderDetails { get; set; }
        public DateTime OrderDate { get; set; }

        public Customer Customer { get; set; }
        public ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; }
        public ICollection<CustomerOrderSpecialOffer> CustomerOrderSpecialOffers { get; set; }
    }
}
