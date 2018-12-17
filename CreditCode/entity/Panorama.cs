using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Point.entity
{
    public class Panorama
    {
        public int id { get; set; }
        public string url { get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public string name { get; set; }
        public bool candelete { get; set; }
    }
}