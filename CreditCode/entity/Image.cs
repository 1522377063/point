using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Point.entity
{
    public class Image
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public int maxlevel { get; set; }
        public int minlevel { get; set; }
        public bool candelete { get; set; }
    }
}