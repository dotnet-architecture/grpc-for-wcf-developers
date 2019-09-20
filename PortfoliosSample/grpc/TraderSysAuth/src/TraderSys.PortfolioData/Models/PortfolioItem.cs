namespace TraderSys.PortfolioData.Models
{
    public class PortfolioItem
    {
        public int Id { get; set; }
        public int ShareId { get; set; }
        public int Holding { get; set; }
        public decimal Cost { get; set; }
    }
}