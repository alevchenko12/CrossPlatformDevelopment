namespace LAB5.Models
{
    public class ProductColour
    {
        public int ProductId { get; set; }
        public string ColourCode { get; set; }
        public bool Availability { get; set; }

        public Product Product { get; set; }
        public RefColour RefColour { get; set; }
    }
}
