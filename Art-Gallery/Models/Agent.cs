using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class Agent
    {
        public int AgentId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public bool Active { get; set; }
    }
}