﻿using MySql.Data.MySqlClient;
using Point.enums;
using Point.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Point.entity;
using Point.common;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Point.service
{
    public class PanoramaSQL : PanoramaAPI
    {
        private string strSql;
        private MySqlParameter[] mySqlParameters;
        private string result = "";

        public string AddPanorama(string name, string url)
        {
            strSql = "INSERT INTO `panorama`(`url`,`name`) VALUES(@url,@name)";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@url",MySqlDbType.VarChar,255) {Value = url},
                new MySqlParameter("@name",MySqlDbType.VarChar,20) {Value = name},
            };
            ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
            return ResultUtil.getStandardResult((int)Status.Normal, "插入成功", null);
        }

        public string GetPanoramaList()
        {
            strSql = "SELECT * FROM `panorama` WHERE `panorama`.isdelete !=1";
            List<Panorama> listImages = ResultUtil.getResultList<Panorama>(strSql);
            return ResultUtil.getStandardResult((int)Status.Normal, "获取成功", listImages);
        }

        public string AddOrUpdatePanorama(string json)
        {
            try
            {
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                //声明更新用的sql
                StringBuilder sql = new StringBuilder();
                StringBuilder sqlmy = new StringBuilder(@"SELECT * FROM `panorama`");
              
                //查询所有字段的字段名dt2,sqlmy
                //存在id则update否则insert dtr,dt,sql2
                DataTable dt2 = ResultUtil.getDataTable(sqlmy.ToString());
                JToken jid = jo["id"];
                if (jid != null)
                {

                    string id = jo["id"].ToString();
                    sql.Append(string.Format(@"update `panorama` set "));
                    foreach (KeyValuePair<string, JToken> kyp in jo)
                    {
                        foreach (DataColumn dc in dt2.Columns)
                        {
                            if (kyp.Key.Equals(dc.ColumnName) && !string.IsNullOrEmpty(kyp.Value.ToString()))
                            {
                                sql.Append(string.Format(@"{0}={1},", kyp.Key, string.Format(@"'{0}'", kyp.Value)));
                            }
                        }
                    }
                    string sqls = sql.ToString();
                    string sqlsnew = sqls.Remove(sqls.Length - 1);
                    sqlsnew = sqlsnew + " where id=" + id + ";";
                    //执行sql
                    ResultUtil.insOrUpdOrDel(sqlsnew);
                    result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Update), @"success,SQL" + sqlsnew);
                }
                else
                {
                    sql.Append(string.Format(@"insert into `panorama` set "));
                    foreach (KeyValuePair<string, JToken> kyp in jo)
                    {
                        foreach (DataColumn dc in dt2.Columns)
                        {
                            if (kyp.Key.Equals(dc.ColumnName) && !string.IsNullOrEmpty(kyp.Value.ToString()))
                            {
                                sql.Append(string.Format(@"{0}={1},", kyp.Key, string.Format(@"'{0}'", kyp.Value)));
                            }
                        }
                    }
                    string sqls = sql.ToString();
                    string sqlsnew = sqls.Remove(sqls.Length - 1);
                    //执行sql
                    ResultUtil.insOrUpdOrDel(sqlsnew);
                    result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Insert), @"success,SQL" + sqlsnew);
                }
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("注册异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.InsertFailure), ex.Message);
            }
            return result;
        }
    }
}