namespace LAB5.Models
{
    public class RefColour
    {
        public string ColourCode { get; set; }
        public string ColourDescription { get; set; }

        public ICollection<ProductColour> ProductColours { get; set; }
    }
}
