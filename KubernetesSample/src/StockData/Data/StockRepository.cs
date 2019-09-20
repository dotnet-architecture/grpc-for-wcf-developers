using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StockData.Data
{
    public interface IStockRepository
    {
        Task<Stock> GetAsync(int id);
    }

    public class DummyStockRepository : IStockRepository
    {
        public Task<Stock> GetAsync(int id)
        {
            if (id < 1) throw new ArgumentOutOfRangeException();
            if (id > Stocks.Count) return Task.FromResult((Stock) null);
            return Task.FromResult(Stocks[id - 1]);
        }

        private static readonly List<Stock> Stocks = new List<Stock>
        {
            new Stock("ATVI", "Activision Blizzard Inc"),
            new Stock("ADBE", "Adobe Inc."),
            new Stock("AMD", "Advanced Micro Devices Inc"),
            new Stock("ALGN", "Align Technology Inc"),
            new Stock("ALXN", "Alexion Pharmaceuticals Inc"),
            new Stock("AMZN", "Amazon.com Inc"),
            new Stock("AMGN", "Amgen Inc"),
            new Stock("AAL", "American Airlines Group Inc"),
            new Stock("ADI", "Analog Devices Inc"),
            new Stock("AAPL", "Apple Inc"),
            new Stock("AMAT", "Applied Materials Inc"),
            new Stock("ASML", "ASML Holding NV"),
            new Stock("ADSK", "Autodesk Inc"),
            new Stock("ADP", "Automatic Data Processing Inc"),
            new Stock("AVGO", "Broadcom Inc"),
            new Stock("BIDU", "Baidu Inc"),
            new Stock("BIIB", "Biogen Inc"),
            new Stock("BMRN", "Biomarin Pharmaceutical Inc"),
            new Stock("CDNS", "Cadence Design Systems Inc"),
            new Stock("CELG", "Celgene Corp"),
            new Stock("CERN", "Cerner Corp"),
            new Stock("CHKP", "Check Point Software Technologies Ltd"),
            new Stock("CHTR", "Charter Communications Inc"),
            new Stock("CTRP", "Ctrip.Com International Ltd"),
            new Stock("CTAS", "Cintas Corp"),
            new Stock("CSCO", "Cisco Systems Inc"),
            new Stock("CTXS", "Citrix Systems Inc"),
            new Stock("CMCSA", "Comcast Corp"),
            new Stock("COST", "Costco Wholesale Corp"),
            new Stock("CSX", "CSX Corp"),
            new Stock("CTSH", "Cognizant Technology Solutions Corp"),
            new Stock("DLTR", "Dollar Tree Inc"),
            new Stock("EA", "Electronic Arts"),
            new Stock("EBAY", "eBay Inc"),
            new Stock("EXPE", "Expedia Group Inc"),
            new Stock("FAST", "Fastenal Co"),
            new Stock("FB", "Facebook"),
            new Stock("FISV", "Fiserv Inc"),
            new Stock("GILD", "Gilead Sciences Inc"),
            new Stock("GOOG", "Alphabet Class C"),
            new Stock("GOOGL", "Alphabet Class A"),
            new Stock("HAS", "Hasbro Inc"),
            new Stock("HSIC", "Henry Schein Inc"),
            new Stock("ILMN", "Illumina Inc"),
            new Stock("INCY", "Incyte Corp"),
            new Stock("INTC", "Intel Corp"),
            new Stock("INTU", "Intuit Inc"),
            new Stock("ISRG", "Intuitive Surgical Inc"),
            new Stock("IDXX", "IDEXX Laboratories Inc"),
            new Stock("JBHT", "J.B. Hunt Transport Services Inc"),
            new Stock("JD", "JD.com Inc"),
            new Stock("KLAC", "KLA Corp"),
            new Stock("KHC", "Kraft Heinz Co"),
            new Stock("LRCX", "Lam Research Corp"),
            new Stock("LBTYA", "Liberty Global PLC"),
            new Stock("LBTYK", "Liberty Global PLC"),
            new Stock("LULU", "Lululemon Athletica Inc"),
            new Stock("MELI", "MercadoLibre Inc"),
            new Stock("MAR", "Marriott International Inc"),
            new Stock("MCHP", "Microchip Technology Inc"),
            new Stock("MDLZ", "Mondelez International Inc"),
            new Stock("MNST", "Monster Beverage Corp"),
            new Stock("MSFT", "Microsoft Corp"),
            new Stock("MU", "Micron Technology Inc"),
            new Stock("MXIM", "Maxim Integrated Products Inc"),
            new Stock("MYL", "Mylan NV"),
            new Stock("NTAP", "NetApp Inc"),
            new Stock("NFLX", "Netflix Inc"),
            new Stock("NTES", "NetEase Inc"),
            new Stock("NVDA", "NVIDIA Corp"),
            new Stock("NXPI", "NXP Semiconductors NV"),
            new Stock("ORLY", "O'Reilly Automotive Inc"),
            new Stock("PAYX", "Paychex Inc"),
            new Stock("PCAR", "PACCAR Inc"),
            new Stock("BKNG", "Booking Holdings Inc"),
            new Stock("PYPL", "PayPal Holdings Inc"),
            new Stock("PEP", "PepsiCo Inc."),
            new Stock("QCOM", "Qualcomm Inc"),
            new Stock("REGN", "Regeneron Pharmaceuticals Inc"),
            new Stock("ROST", "Ross Stores Inc"),
            new Stock("SIRI", "Sirius XM Holdings Inc"),
            new Stock("SWKS", "Skyworks Solutions Inc"),
            new Stock("SBUX", "Starbucks Corp"),
            new Stock("SYMC", "Symantec Corp"),
            new Stock("SNPS", "Synopsys Inc"),
            new Stock("TTWO", "Take-Two Interactive Software Inc"),
            new Stock("TSLA", "Tesla Inc"),
            new Stock("TXN", "Texas Instruments Inc"),
            new Stock("TMUS", "T-Mobile US Inc"),
            new Stock("ULTA", "Ulta Beauty Inc"),
            new Stock("UAL", "United Airlines Holdings Inc"),
            new Stock("VRSN", "Verisign Inc"),
            new Stock("VRSK", "Verisk Analytics Inc"),
            new Stock("VRTX", "Vertex Pharmaceuticals Inc"),
            new Stock("WBA", "Walgreens Boots Alliance Inc"),
            new Stock("WDC", "Western Digital Corp"),
            new Stock("WDAY", "Workday Inc"),
            new Stock("WYNN", "Wynn Resorts Ltd"),
            new Stock("XEL", "Xcel Energy Inc"),
            new Stock("XLNX", "Xilinx Inc"),
        };
    }
}