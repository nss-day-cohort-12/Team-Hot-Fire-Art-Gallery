using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Art_Gallery.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public int IndividualPieceId { get; set; }
    }
}