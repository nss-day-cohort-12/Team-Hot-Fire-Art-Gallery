using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Art_Gallery.Models;

namespace Art_Gallery.ViewModels
{
    public class CustomerArtViewModel
    {
        public int ArtworkId { get; set; }
        public int IndividualId { get; set; }
        public string ArtistName { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Medium { get; set; }
        public string Dimensions { get; set; }
        public int QtyInInventory { get; set; }
        public float Price { get; set; }
        public bool HasSold { get; set; }
        public string Location { get; set; }
        public List<ArtWork> Works { get; set; }
        public List<IndividualPiece> Pieces { get; set; }
    }
}