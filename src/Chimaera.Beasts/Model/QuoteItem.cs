namespace Chimaera.Beasts.Model
{
    public class QuoteItem
    {
        public int QuoteItemID { get; set; }
        public int QuoteID { get; set; }
        public Product Product { get; set; }
        public Size Size { get; set; }
        public int Quantity { get; set; }
    }
}
