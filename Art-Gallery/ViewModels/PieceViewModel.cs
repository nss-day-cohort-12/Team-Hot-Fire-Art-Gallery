using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class PieceViewModel
    {
        public int IndividualPieceId { get; set; }
        public string Title { get; set; }
        public float Cost { get; set; }
        public float AskingPrice { get; set; }
        public bool Sold { get; set; }
    }
}