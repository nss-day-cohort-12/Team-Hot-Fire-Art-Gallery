using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class InventoryViewModel
    {
        public string Title { get; set; }
        public float Cost { get; set; }
        public float AskingPrice { get; set; }
        public List<PieceViewModel> Pieces { get; set; }
    }
}