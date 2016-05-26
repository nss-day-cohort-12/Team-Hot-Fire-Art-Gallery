﻿using Art_Gallery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class ArtshowViewModel
    {
        public int ArtShowId { get; set; }
        public string ArtistsRepresented { get; set; }
        public string ShowLocation { get; set; }
        public string Agents { get; set; }
        public float Overhead { get; set; }
        public List<ArtShow> ShowList { get; set; }
    }
}