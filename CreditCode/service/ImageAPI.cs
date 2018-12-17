using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Point.service
{
    interface ImageAPI
    {
        string GetImageList();
        string AddImgage(string name, string url);
        string AddOrUpdateImage(string json);
    }
}
