using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Point.entity;
using Point.utils;
using Point.enums;
using Newtonsoft.Json.Linq;
using Point.common;
using Newtonsoft.Json;
using System.Text;
using System.Data;

namespace Point.service
{
    public class ModelSQL : ModelAPI
    {
        private string strSql;
        private MySqlParameter[] mySqlParameters;
        private string result = "";

        public string GetModelList()
        {
            strSql = "SELECT * FROM `model` WHERE `model`.isdelete !=1";
            List<Model> listImages = ResultUtil.getResultList<Model>(strSql);
            return ResultUtil.getStandardResult((int)Status.Normal, "模型列表", listImages);
        }

        public string AddModel(string name, string url)
        {
            strSql = "INSERT INTO model(`url`,`name`) VALUES(@url,@name)";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@url",MySqlDbType.VarChar,255) {Value = url},
                new MySqlParameter("@name",MySqlDbType.VarChar,20) {Value = name},
            };
            ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
            return ResultUtil.getStandardResult((int)Status.Normal, "获取成功", null);
        }

        public string AddOrUpdateModel(string json)
        {
            try
            {
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                //声明更新用的sql
                StringBuilder sql = new StringBuilder();
                StringBuilder sqlmy = new StringBuilder(@"SELECT * FROM `model`");
                //查询所有字段的字段名dt2,sqlmy
                //存在id则update否则insert dtr,dt,sql2
                DataTable dt2 = ResultUtil.getDataTable(sqlmy.ToString());
                JToken jid = jo["id"];
                if (jid != null)
                {

                    string id = jo["id"].ToString();
                    sql.Append(string.Format(@"update `model` set "));
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
                    sql.Append(string.Format(@"insert into `model` set "));
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