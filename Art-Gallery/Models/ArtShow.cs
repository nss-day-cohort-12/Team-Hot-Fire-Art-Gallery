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
        public int Agents { get; set; }
        public float Overhead { get; set; }
    }
}