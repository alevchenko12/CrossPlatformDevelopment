namespace LAB6.Models
{
    public class Retailer
    {
        public int RetailerId { get; set; }
        public string RetailerName { get; set; }
        public string RetailerAddress { get; set; }
        public string RetailerWebSiteUrl { get; set; }
        public string OtherRetailerDetails { get; set; }

        public ICollection<RetailerProductPrice> RetailerProductPrices { get; set; }
        public ICollection<SpecialOffer> SpecialOffers { get; set; }
        public ICollection<CustomerOrderProduct> CustomerOrderProducts { get; set; }
    }
}
