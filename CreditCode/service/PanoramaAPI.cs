using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Point.service
{
    interface PanoramaAPI
    {
        string GetPanoramaList();
        string AddPanorama(string name, string url);
        string AddOrUpdatePanorama(string json);
    }
}
