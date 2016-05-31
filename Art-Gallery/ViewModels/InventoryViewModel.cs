using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class InventoryViewModel
    {
        public string Title { get; set; }
        public double Cost { get; set; }
        public double AskingPrice { get; set; }
        public List<PieceViewModel> Pieces { get; set; }
        public bool Sold { get; set; }
        public double TotalSales { get; set; }
        public double TotalProfit { get; set; }
        public List<ArtWorkViewModel> WorksOfArt { get; set; }
    }
}