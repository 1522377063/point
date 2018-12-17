using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
        /// <summary>
        /// 手机端报送 新流程 问题提交
        /// </summary>
        /// http://localhost:25605/handler.ashx?method=save_question_detial&type=webservice&id=1&title=灵桥大修工程&question=问题问题&sub_people=提交人&fenshu=100+&xmzl=1&aqgl=2&hbcs=3&gqjd=4&shyx=5
        /// <returns></returns>
        public static string send_project(string img_data, string id, string name, string mobile, string title, string question, string sub_people, string fenshu, string xmzl, string aqgl, string hbcs, string gqjd, string shyx)
        {
            string picname = "";
            string pic_path = "";
            string imgsrc = "";
            int byte_length = 0;
            string[] imgs;
            imgs = img_data.Split('&');
         


            // 根据 原项目的runid 查找 proj里的变量信息 
            string sql = "SELECT `ID` as 'F_proID',`F_projName`, `F_status`,  `F_pos`, `F_projClass`, `F_projClass2`,`F_hideType`, `F_org`, b.ORGID as  'F_orgID',`F_projType`, `F_personLiable`, `F_plTele`,`F_coorg`, `F_begin`, `F_end`, `F_invest`, `F_threeYInvest`, `F_content`, `F_remarks` from w_t_proj a left join sys_org b on a.F_org = b.ORGNAME where ((`a`.`F_stage` = 3) and (`a`.`F_status` <> '屏蔽')) and F_projName like '%" + title + "%'";
            //DataTable dtselect = MySqlHelper.ExecuteDataset(m_ConStr, sql).Tables[0];
            //DataRow row_select = dtselect.Rows[0];


            //根据 接口要求的 格式 生成json
            fieldsNode_new_problem tn = new fieldsNode_new_problem()
            {
                proID = "1",
                projName = "1",
                status = "1",
                pos = "1",
                projClass = "1",
                projClass2 = row_select["F_projClass2"].ToString(),
                org = row_select["F_org"].ToString(),

                projType = row_select["F_projType"].ToString(),
                personLiable = row_select["F_personLiable"].ToString(),
                plTele = row_select["F_plTele"].ToString(),
                coorg = row_select["F_coorg"].ToString(),
                begin = row_select["F_begin"].ToString(),
                end = row_select["F_end"].ToString(),
                invest = row_select["F_invest"].ToString(),
                threeYInvest = row_select["F_threeYInvest"].ToString(),
                content = row_select["F_content"].ToString(),
                remarks = row_select["F_remarks"].ToString(),
                hideType = row_select["F_hideType"].ToString(),

                reply_organization = row_select["F_org"].ToString(),
                reply_organizationID = row_select["F_orgID"].ToString(),

                // 报送人信息 
                reporter = name,
                reID = id,
                reTele = mobile,

                problem = question,
                imgsrc = imgsrc,
                xmzl = xmzl,
                aqgl = aqgl,
                hbcs = hbcs,
                gqjd = gqjd,
                shyx = shyx,
                fenshu = fenshu
            };
            //Console.WriteLine(JsonConvert.SerializeObject(main.ToString()).ToString());
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "GB2312", null);
            doc.AppendChild(dec);
            XmlElement root = doc.CreateElement("req");
            doc.AppendChild(root);
            // 2个必填一个
            root.SetAttribute("actDefId", "xmwttj:1:80000000498934"); // 自定义流程ID 
            root.SetAttribute("flowKey", "");
            root.SetAttribute("subject", "问题提交 - " + row_select["F_projName"].ToString());  // 流程实例的标题
            root.SetAttribute("account", "admin"); //（必填）启动流程的用户账号 ============

            // businessKey 生成规则 290 + 年月入小时分秒
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            root.SetAttribute("businessKey", "290" + date);

            XmlElement data = doc.CreateElement("data");
            root.AppendChild(data);

            //创建CDATA
            XmlCDataSection cdata = doc.CreateCDataSection(JsonConvert.SerializeObject(json));
            data.AppendChild(cdata);

            // 流程变量  可以不加 但是不能为空，要去掉就直接 连同 var的<>都去掉
            //XmlElement var = doc.CreateElement("var");
            //var.SetAttribute("varName", "name");
            //var.SetAttribute("varVal", "王骏");
            //var.SetAttribute("varType", "String");
            //var.SetAttribute("dateFormat", "4");
            //root.AppendChild(var);

            // 保存 项目提交的xml 测试时候查看报文用
            //doc.Save(@"d:\send_newproj.xml"); 

            string xml = "";
            SR147.ProcessServiceClient psc = new SR147.ProcessServiceClient();
            xml = psc.start(doc.OuterXml); //提交新项目

            // 提交新项目返回 xml例子
            //xml = "<run actDefId='xxmbs:1:10000000431118' actInstId='10000000601074' businessKey='290160806113623' creator='超级管理员' processName='项目报送' startOrgName='市住建委' subject='项目报送-恒大山水城项目' createtime='2016-08-06 11:36:01' creatorId='1' defId='10000000431114' formDefId='10000000431084' runId='10000000601072' startOrgId='10000000460036' status='1'/>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            //查找<users>
            XmlNode run = xmlDoc.SelectSingleNode("run");
            XmlElement xe = (XmlElement)run;
            String runId = xe.GetAttribute("runId");

            // 返回新项目的runid 可以通过runid 操作之后的 donext
            //return runId;
            return "true";
        }
    }
    public class mainNode_ZH
    {
        public fieldsNode_month fields { get; set; }
    }
    public class jsonNode_ZH
    {
        public mainNode_ZH main { get; set; }
        public JArray sub { get; set; }
        public JArray opinion { get; set; }
    }
    public class fieldsNode_month
    {
        public string CompanyName { get; set; }
        public string Name { get; set; }
        public int ifPass { get; set; }
    }
}
