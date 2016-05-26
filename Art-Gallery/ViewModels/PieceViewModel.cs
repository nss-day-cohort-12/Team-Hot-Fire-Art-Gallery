using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class PieceViewModel
    {
        public int IndividualPieceId { get; set; }
        public int? InvoiceId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string EditionNumber { get; set; }
        public string Location { get; set; }
        public double Cost { get; set; }
        public double AskingPrice { get; set; }
        public bool Sold { get; set; }
    }
}