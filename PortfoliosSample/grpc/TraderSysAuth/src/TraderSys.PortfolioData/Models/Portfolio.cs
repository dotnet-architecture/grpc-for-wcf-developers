using System;
using System.Collections.Generic;

namespace TraderSys.PortfolioData.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public Guid TraderId { get; set; }
        public List<PortfolioItem> Items { get; set; }
    }
}