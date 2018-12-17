/*************************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    tzz
** date：    2018/5/22
** desc：    扬尘管理实现类
** Ver.:     V1.0.0
**************************************************/

using Newtonsoft.Json.Linq;
using Point.common;
using Point.enums;
using Point.model;
using Point.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.Configuration;

namespace Point.service
{
    public class PointSQL : PointAPI
    {
        //声明SQL字符
        private string sql = "";
        public static string m_ConStr = ConfigurationManager.ConnectionStrings["MysqlConStr"].ConnectionString;
        //利用时间戳自动生成ORGID
        //声明结果字符
        private string result = "";
        public static long GetTimestamp()
        {
            TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1500, 1, 1);//ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
            return (long)ts.TotalMilliseconds; //精确到毫秒
        }
        public string getType(string type){
            try
            {
                sql = string.Format(@"select * from poiwithlayer where layerid ='"+ type + "' and isdelete=0;");
                DataTable dt = MySqlHelper.ExecuteDataset(m_ConStr, sql).Tables[0];
                //声明JArray数组
                JArray arr = new JArray();
                //遍历获取结果
                foreach (DataRow dr in dt.Rows)
                {
                    //声明JObject对象
                    JObject j = new JObject();
                    foreach (DataColumn col in dt.Columns)
                    {
                        j.Add(col.ColumnName, dr[col.ColumnName].ToString());
                    }
                    //数组添加元素
                    arr.Add(j);
                }
                result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Query), arr); 
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("注册异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.QueryFailure), ex.Message);
            }
            return result;
        }
        public string getAll()
        {
            try
            {
                sql = string.Format(@"select * from poi where isdelete=0;");
                DataTable dt = MySqlHelper.ExecuteDataset(m_ConStr, sql).Tables[0];
                //声明JArray数组
                JArray arr = new JArray();
                //遍历获取结果
                foreach (DataRow dr in dt.Rows)
                {
                    //声明JObject对象
                    JObject j = new JObject();
                    foreach (DataColumn col in dt.Columns)
                    {
                        j.Add(col.ColumnName, dr[col.ColumnName].ToString());
                    }
                    //数组添加元素
                    arr.Add(j);
                }
                result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Query), arr);
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("注册异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.QueryFailure), ex.Message);
            }
            return result;
        }
        public string getAllLayer()
        {
            try
            {
                sql = string.Format(@"select * from layer where isdelete=0;");
                DataTable dt = MySqlHelper.ExecuteDataset(m_ConStr, sql).Tables[0];
                //声明JArray数组
                JArray arr = new JArray();
                //遍历获取结果
                foreach (DataRow dr in dt.Rows)
                {
                    //声明JObject对象
                    JObject j = new JObject();
                    foreach (DataColumn col in dt.Columns)
                    {
                        j.Add(col.ColumnName, dr[col.ColumnName].ToString());
                    }
                    //数组添加元素
                    arr.Add(j);
                }
                result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Query), arr);
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("注册异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.QueryFailure), ex.Message);
            }
            return result;
        }
        public string addLayer(string json)
        {
            try
            {
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                //声明更新用的sql
                StringBuilder sql = new StringBuilder();
                StringBuilder sqlmy = new StringBuilder(@"SELECT * FROM layer");
                //查询所有字段的字段名dt2,sqlmy
                DataTable dt2 = ResultUtil.getDataTable(sqlmy.ToString());
                //存在id则update否则insert dtr,dt,sql2
                if (!string.IsNullOrEmpty(jo["id"].ToString()))
                {
                    string id = jo["id"].ToString();
                    sql.Append(string.Format(@"update layer set "));
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
                else {
                    sql.Append(string.Format(@"insert into layer set "));
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
        public string addPointOrUpdate(string json)
        {
            try
            {
                JObject jo = JsonConvert.DeserializeObject(json) as JObject;
                //声明更新用的sql
                StringBuilder sql = new StringBuilder();
                StringBuilder sqlmy = new StringBuilder(@"SELECT * FROM poi");
                //查询所有字段的字段名dt2,sqlmy
                //存在id则update否则insert dtr,dt,sql2
                DataTable dt2 = ResultUtil.getDataTable(sqlmy.ToString());
                JToken jid = jo["id"];
                if (jid!=null)
                {
                
                    string id = jo["id"].ToString();
                    sql.Append(string.Format(@"update poi set "));
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
                    else {
                    sql.Append(string.Format(@"insert into poi set "));
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

        /// <summary>
        /// 文件资料上传
        /// </summary>
        /// <param name="file">上传文件</param>
        /// <param name="parentId">父文件ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="projId">项目或单体ID</param>
        /// <param name="userName">用户名</param>
        /// <returns>string</returns>
        public string upLoadFile(HttpPostedFile file, string parentId, string typeId, string userId, string userName)
        {
            string sqlStr = null;
            //系统当前时间
            DateTime createtime = DateTime.Now;
            try
            {
                //文件存在时
                if (file.ContentLength > 0)
                {
                    //文件后缀带.
                    string extName = Path.GetExtension(file.FileName);
                    //去.后缀名
                    string saveExtName = extName.Substring(1, extName.Length - 1);
                    //文件名除后缀
                    string preFileName = Path.GetFileNameWithoutExtension(file.FileName);
                    //文件大小
                    string fileSize = CountSize(file.ContentLength);
                    //时间文件格式
                    long fileName = DateTime.Now.ToFileTime();
                    //文件名称
                    string fullName = fileName + extName;
                    //文件保存路径
                    string phyFilePath = HttpContext.Current.Server.MapPath("~/upload/" + parentId + "/");
                    //判断路径是否存在
                    if (!Directory.Exists(phyFilePath))
                    {
                        //文件路径不存在创建文件夹
                        Directory.CreateDirectory(phyFilePath);
                    }

                    //文件保存路径
                    string filePathSave = string.Format("/service/upload/{0}/{1}", parentId, fullName);
                    //文件保存
                    file.SaveAs(phyFilePath + fullName);

                    //文件管理sql
                    sqlStr = string.Format(@"insert into t_file (id,pid,typeid,filename,filepath,createtime,ext,filetype,note,creatorid,creator,totalbytes,delflag) values ({0},{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12})",
                        fileName, parentId, typeId, preFileName, filePathSave, createtime, saveExtName, "poi", fileSize, userId, userName, file.ContentLength, 0);
                    //执行数据库新增操作
                    ResultUtil.insOrUpdOrDel(sqlStr);
                }

            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("上传文件异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.InsertFailure), ex.Message);
            }

            return result;
        }

        public object getPOI(object poiid)
        {
            try
            {
                sql = "select * from poi where id =" + poiid + " and isdelete=0;";
                DataRow dr = MySqlHelper.ExecuteDataRow(m_ConStr, sql);
                sql = "select * from t_file where pid = " + poiid + " and delflag=0";
                DataTable dt = MySqlHelper.ExecuteDataset(m_ConStr, sql).Tables[0];
                //声明JObject对象
                JObject j = new JObject();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    j.Add(col.ColumnName, dr[col.ColumnName].ToString());
                }
                //声明JArray数组
                JArray img = new JArray();
                JArray sound = new JArray();
                JArray video = new JArray();
                //遍历获取结果
                foreach (DataRow r in dt.Rows)
                {
                    switch (r["typeid"].ToString())
                    {
                        case "1":
                            img.Add(r["filepath"].ToString());
                            break;
                        case "2":
                            sound.Add(r["filepath"].ToString());
                            break;
                        case "3":
                            video.Add(r["filepath"].ToString());
                            break;
                    }
                }
                j.Add("img", img);
                j.Add("sound", sound);
                j.Add("video", video);
                result = ResultUtil.getStandardResult((int)Status.Normal, EnumUtil.getMessageStr((int)Message.Query), j);
            }
            catch (Exception ex)
            {
                //异常日志
                LogHelper.WriteError("注册异常：" + ex.Message);
                //异常结果
                result = ResultUtil.getStandardResult((int)Status.Error, EnumUtil.getMessageStr((int)Message.QueryFailure), ex.Message);
            }

            return result;
        }


        #region 计算文件大小函数(保留两位小数),Size为字节大小
        /// <summary>  
        /// 计算文件大小函数(保留两位小数),Size为字节大小  
        /// </summary>  
        /// <param name="Size">初始文件大小</param>  
        /// <returns>string</returns>  
        public string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }
        #endregion

    }
}