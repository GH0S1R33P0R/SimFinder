using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeniorProjectWeb.Models
{
    public class Ticket
    {
        public string oid { get; set; }
        public string itemID { get; set; }
        public string summary { get; set; }
        public double rating { get; set; }

    }
}