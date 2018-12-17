using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data;
using Point.enums;
using Point.utils;

namespace Point.common
{
    public class JsonHelper
    {
        /// <summary>
        /// DataTable转规范化Json
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static JObject DatatableToJson(DataTable dt, string page, string rows)
        {
            //DataTable转Json
            JObject jo = new JObject();
            JArray ja = new JArray();
            int ipage = 1, irows = 99;
            int.TryParse(page, out ipage);
            int.TryParse(rows, out irows);
            for (int i = (ipage - 1) * irows; i < Math.Min(dt.Rows.Count, ipage * irows); i++)
            {
                DataRow dr = dt.Rows[i];
                JObject row = new JObject();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row.Add(dt.Columns[j].ColumnName, dr[dt.Columns[j].ColumnName].ToString());
                }
                ja.Add(row);
            }
            jo.Add("status", (int)Status.Normal);
            jo.Add("message", EnumUtil.getMessageStr((int)Message.Query));
            jo.Add("total", dt.Rows.Count);
            jo.Add("rows", ja);
            return jo;
        }
    }
}