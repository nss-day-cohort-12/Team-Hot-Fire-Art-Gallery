using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.ViewModels
{
    public class AgentViewModel
    {
        public int AgentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool Active { get; set; }
        public string PieceSold { get; set; }
        public double Sales { get; set; }
        public double Profit { get; set; }
    }
}