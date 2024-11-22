namespace LAB5.Models
{
    public class SpecialOffer
    {
        public int SpecialOfferId { get; set; }
        public int ProductId { get; set; }
        public int RetailerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string TermsAndConditions { get; set; }
        public string OtherDetails { get; set; }

        public Product Product { get; set; }
        public Retailer Retailer { get; set; }
        public ICollection<CustomerOrderSpecialOffer> CustomerOrderSpecialOffers { get; set; }
    }
}
