using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Point.service
{
    interface ModelAPI
    {
        string GetModelList();
        string AddModel(string name, string url);
        string AddOrUpdateModel(string json);
    }
}
