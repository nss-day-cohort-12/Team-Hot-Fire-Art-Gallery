using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class AgentSalesViewModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string PiecesSold { get; set; }
        public string TotalSales { get; set; }
        public string TotalProfit { get; set; }
        public bool Active { get; set; }
        public List<AgentViewModel> Agents { get; set; }
    }
}