using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Point.entity
{
    public class TFile
    {
        public Int64 id { get; set; }
        public Int64 pid { get; set; }
        public int typeid { get; set; }
        public string filetype { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public Int64 totalbytes { get; set; }
        public string ext { get; set; }
        public string note { get; set; }
        public DateTime createtime { get; set; }
        public Int64 creatorid { get; set; }
        public string creator { get; set; }
        public bool delflag { get; set; }
        public byte[] fileblob { get; set; }
        public int rid { get; set; }
    }
}