using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class CustomerPieceViewModel
    {
        public int ArtistId { get; set; }
        public int IndividualPieceId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public bool HasSold { get; set; }
        public List<CustomerArtViewModel> AllPieces { get; set; }
    }
}