namespace LAB5.Models
{
    public class CustomerOrderSpecialOffer
    {
        public int OrderId { get; set; }
        public int SpecialOfferId { get; set; }
        public DateTime DateOrderPlaced { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public string OrderDetails { get; set; }

        public CustomerOrder CustomerOrder { get; set; }
        public SpecialOffer SpecialOffer { get; set; }
    }
}
