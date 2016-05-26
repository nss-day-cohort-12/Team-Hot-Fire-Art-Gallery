using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class ArtShow
    {
        public int ArtShowId { get; set; }
        public string ArtistsRepresented { get; set; }
        public string ShowLocation { get; set; }
        public string Agents { get; set; }
        public double Overhead { get; set; }
    }
}