using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Point.entity
{
    public class Report
    {
        public int id { get; set; }
        public string report_person { get; set; }
        public string reportdate { get; set; }
        public string phone { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public int pid { get; set; }
    }
}