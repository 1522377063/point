using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Point.entity;
using Point.enums;
using Point.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Point.service
{
    public class ReportService:IReportService
    {
        private string strSql;
        private MySqlParameter[] mySqlParameters;

        public string getReportList()
        {
            strSql =
                "SELECT * FROM report";
            JObject jo2 = new JObject();
            jo2.Add("status", (int)Status.Normal);
            jo2.Add("message", "获取列表成功");
            List<Report> listReport = ResultUtil.getResultList<Report>(strSql);
            JArray ja1 = new JArray();
            if (listReport != null && listReport.Count != 0)
            {
                foreach (Report report in listReport)
                {
                    JObject jo1 = new JObject();
                    strSql = "SELECT * FROM t_file WHERE rid=@rid";
                    mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@rid",MySqlDbType.Int32) {Value = report.id},
                };
                    List<TFile> listFiles = ResultUtil.getResultList<TFile>(strSql, mySqlParameters);
                    jo1.Add("id", report.id);
                    jo1.Add("report_person", report.report_person);
                    jo1.Add("reportdate", report.reportdate);
                    jo1.Add("phone", report.phone);
                    jo1.Add("content", report.content);
                    jo1.Add("type", report.type);
                    jo1.Add("pid", report.pid);
                    JArray ja = new JArray();
                    if (listFiles != null && listFiles.Count != 0)
                    {
                        foreach (TFile file in listFiles)
                        {
                            JObject jo = new JObject();
                            jo.Add("id", file.id);
                            jo.Add("pid", file.pid);
                            jo.Add("typeid", file.typeid);
                            jo.Add("filetype", file.filetype);
                            jo.Add("filename", file.filename);
                            jo.Add("filepath", file.filepath);
                            jo.Add("totalbytes", file.totalbytes);
                            jo.Add("ext", file.ext);
                            jo.Add("note", file.note);
                            jo.Add("createtime", file.createtime);
                            jo.Add("creatorid", file.creatorid);
                            jo.Add("creator", file.creator);
                            jo.Add("delflag", file.delflag);
                            jo.Add("fileblob", file.fileblob);
                            jo.Add("rid", file.rid);
                            ja.Add(jo);
                        }
                    }

                    jo1.Add("pic", ja);
                    ja1.Add(jo1);
                }
            }

            jo2.Add("result", ja1);
            //string result = ResultUtil.getStandardResult((int) Status.Normal, "获取数据成功", list);
            return jo2.ToString();
        }

        public string getReportListById(int id)
        {
            strSql =
                "SELECT * FROM report WHERE id=@id";
            mySqlParameters = new MySqlParameter[]
            {
                new MySqlParameter("@id",MySqlDbType.Int32) {Value = id},
            };
            JObject jo2 = new JObject();
            jo2.Add("status", (int)Status.Normal);
            jo2.Add("message", "获取列表成功");
            List<Report> listReport = ResultUtil.getResultList<Report>(strSql, mySqlParameters);
            JArray ja1 = new JArray();
            if (listReport != null && listReport.Count != 0)
            {
                Report report = listReport[0];
                JObject jo1 = new JObject();
                strSql = "SELECT * FROM t_file WHERE rid=@rid";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@rid",MySqlDbType.Int32) {Value = report.id},
                };
                List<TFile> listFiles = ResultUtil.getResultList<TFile>(strSql, mySqlParameters);
                jo1.Add("id", report.id);
                jo1.Add("report_person", report.report_person);
                jo1.Add("reportdate", report.reportdate);
                jo1.Add("phone", report.phone);
                jo1.Add("content", report.content);
                jo1.Add("type", report.type);
                jo1.Add("pid", report.pid);
                JArray ja = new JArray();
                if (listFiles != null && listFiles.Count != 0)
                {
                    foreach (TFile file in listFiles)
                    {
                        JObject jo = new JObject();
                        jo.Add("id", file.id);
                        jo.Add("pid", file.pid);
                        jo.Add("typeid", file.typeid);
                        jo.Add("filetype", file.filetype);
                        jo.Add("filename", file.filename);
                        jo.Add("filepath", file.filepath);
                        jo.Add("totalbytes", file.totalbytes);
                        jo.Add("ext", file.ext);
                        jo.Add("note", file.note);
                        jo.Add("createtime", file.createtime);
                        jo.Add("creatorid", file.creatorid);
                        jo.Add("creator", file.creator);
                        jo.Add("delflag", file.delflag);
                        jo.Add("fileblob", file.fileblob);
                        jo.Add("rid", file.rid);
                        ja.Add(jo);
                    }
                }

                jo1.Add("pic", ja);
                ja1.Add(jo1);
            }

            jo2.Add("result", ja1);
            //string result = ResultUtil.getStandardResult((int) Status.Normal, "获取数据成功", list);
            return jo2.ToString();
        }

        public string AddReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid)
        {
            try
            {
                strSql = "INSERT INTO report VALUES(@id,@report_person,@reportdate,@phone,@content,@type,@pid)";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id",MySqlDbType.Int32){Value=id},
                    new MySqlParameter("@report_person",MySqlDbType.VarChar,20){Value=report_person},
                    new MySqlParameter("@reportdate",MySqlDbType.VarChar,20){Value=reportdate},
                    new MySqlParameter("@phone",MySqlDbType.VarChar,20){Value=phone},
                    new MySqlParameter("@content",MySqlDbType.VarChar,255){Value=content},
                    new MySqlParameter("@type",MySqlDbType.VarChar,10){Value=type},
                    new MySqlParameter("@pid",MySqlDbType.Int32){Value=pid},
                };
                ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
                return ResultUtil.getStandardResult((int)Status.Normal, "添加成功", null);
            }
            catch (Exception ex)
            {
                return ResultUtil.getStandardResult((int)Status.Error, "添加失败", null);
            }
        }
        public string UpdateReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid)
        {
            try
            {
                strSql = "UPDATE report SET id=@id ,report_person=@report_person,reportdate=@reportdate,phone=@phone,content=@content,type=@type,pid=@pid WHERE id=@id";
                mySqlParameters = new MySqlParameter[]
                {
                    new MySqlParameter("@id",MySqlDbType.Int32){Value=id},
                    new MySqlParameter("@report_person",MySqlDbType.VarChar,20){Value=report_person},
                    new MySqlParameter("@reportdate",MySqlDbType.VarChar,20){Value=reportdate},
                    new MySqlParameter("@phone",MySqlDbType.VarChar,20){Value=phone},
                    new MySqlParameter("@content",MySqlDbType.VarChar,255){Value=content},
                    new MySqlParameter("@type",MySqlDbType.VarChar,10){Value=type},
                    new MySqlParameter("@pid",MySqlDbType.Int32){Value=pid},
                };
                ResultUtil.insOrUpdOrDel(strSql, mySqlParameters);
                return ResultUtil.getStandardResult((int)Status.Normal, "更新成功", null);
            }
            catch (Exception ex)
            {
                return ResultUtil.getStandardResult((int)Status.Error, "更新失败", null);
            }
        }
    }
}