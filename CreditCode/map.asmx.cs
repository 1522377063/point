/*************************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    tzz
** date：    2018/5/22
** desc：    webservice前台返回服务
** Ver.:     V1.0.0
**************************************************/

using Point.service;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Xml;
using Point.utils;
using System.Web;
using Point.enums;

namespace Point
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        private  PointAPI PointAPI = new PointSQL();
        private ImageAPI ImageAPI = new ImageSQL();
        private ModelAPI ModelAPI = new ModelSQL();
        private PanoramaAPI PanoramaAPI = new PanoramaSQL();
        private IReportService reportService = new ReportService();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region 弃用的方法
        #region 地图添加点
        [WebMethod(Description = @"<h3>method: addPointOrUpdate</h3>
        <p>方法描述：<strong>地图添加点</strong></p> 
        <p>参数：{<strong>type<=>layer type,name<=>地点名称,detail<=>地点详细,longitude<=>经度,latitude<=>纬度}</p>
        <p style='color:green'>成功结果：<strong>插入成功</strong></p>
        <p style='color:red'>失败结果：<strong>插入失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void addPointOrUpdate(string json)
        {
            savePOI(json);
        }
        #endregion

        #region 按type获取点
        [WebMethod(Description = @"<h3>method: getType</h3>
        <p>方法描述：<strong>按type获取点</strong></p> 
        <p>参数：{<strong>type<=>layer type</strong>}</p>
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void getType(string type)
        {
            getPOI(type);
        }
        #endregion
        
        #region 获取所有点
        [WebMethod(Description = @"<h3>method: getAll</h3>
        <p>方法描述：<strong>获取所有点</strong></p> 
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void getAll()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.getAll());
            Context.Response.End();
        }
        #endregion
        #endregion


        #region 图层管理

        #region 获取图层列表
        [WebMethod(Description = @"<h3>method: getAlllayer</h3>
        <p>方法描述：<strong>获取所有图层清单</strong></p> 
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void getAllLayer()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.getAllLayer());
            Context.Response.End();
        }
        [WebMethod(Description = @"<h3>method: getlayer</h3>
        <p>方法描述：<strong>获取指定图层内所有点</strong></p> 
        <p>参数：{<strong>layerid<=>layer id</strong>}</p>
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void getLayer(string layerid)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.getType(layerid));
            Context.Response.End();
        }
        #endregion

        #region 添加图层
        [WebMethod(Description = @"<h3>method: addLayer</h3>
        <p>方法描述：<strong>添加layer</strong></p> 
        <p>参数：{<strong>name<=>layer name</strong>}</p>
        <p style='color:green'>成功结果：<strong>成功</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void addLayer(string json)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.addLayer(json));
            Context.Response.End();
        }
        #endregion

        #region 兴趣点管理
        [WebMethod(Description = @"<h3>method: getPOI</h3>
        <p>方法描述：<strong>获取点属性</strong></p> 
        <p>参数：{<strong>poiid<=>poi id</strong>}</p>
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void getPOI(string poiid)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.getPOI(poiid));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: savePOI</h3>
        <p>方法描述：<strong>保存点</strong></p> 
        <p>参数：{<strong>poiid<=>poi id</strong>}</p>
        <p style='color:green'>成功结果：<strong>获取成功</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void savePOI(string json)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.addPointOrUpdate(json));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: upLoadFile</h3>
        <p>方法描述：<strong>上传文件资料</strong></p> 
        <p>参数：{<strong>parentId:父节点Id;typeId:文件类型（照片1，语音2，视频3）;userId:用户Id;userName:用户名称</strong>}</p>
        <p>返回结果：<strong>{'status':200,'message':'新增成功','result':'新增成功','img':['img1','img2'...],'sound':['sound1','sound2'...],'video':['video1','video2'...],}</strong></p>
        <p>输出格式：<strong>string</strong></p></br>")]
        public void upLoadFile()
        { 
            //父ID
            string parentId = Context.Request.Form["parentId"];
            //父ID
            string typeId = Context.Request.Form["typeId"];
            //用户ID
            string userId = Context.Request.Form["userId"];
            //用户名
            string userName = Context.Request.Form["userName"];
            try
            {
                //文件
                for (int i = 0; i < Context.Request.Files.Count; i++)
                {
                    string result = PointAPI.upLoadFile(Context.Request.Files[i], parentId, typeId, userId, userName);
                }
            }
            catch (Exception ex)
            {

            }

            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PointAPI.getPOI(parentId));
            Context.Response.End();
        }
        #endregion


        #endregion

        #region 影像管理
        [WebMethod(Description = @"<h3>method: GetImageList</h3>
        <p>方法描述：<strong>获取所有影像</strong></p> 
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'获取成功','result':[{'id':1,'url':'http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png1','name':'hhhhhhhhh','maxlevel':1,'minlevel':1,'candelete':true}]}</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetImageList()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ImageAPI.GetImageList());
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: AddImgage</h3>
        <p>方法描述：<strong>添加image</strong></p> 
        <p>参数：{<strong>name<=>image name</strong>}</p>
        <p>参数：{<strong>url<=>image url</strong>}</p>
        <p style='color:green'>成功结果：<strong>成功</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void AddImgage(string name,string url)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ImageAPI.AddImgage(name,url));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: AddImgage</h3>
        <p>方法描述：<strong>添加image</strong></p> 
        <p>参数：{<strong>name<=>image name</strong>}</p>
        <p>参数：{<strong>url<=>image url</strong>}</p>
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'修改成功','result':'success,SQLupdate image set id='1',url='http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png1',name='hhhhhhhhh',maxlevel='1',minlevel='1',candelete='1',isdelete='0' where id=1;'}</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void SaveImage(string json)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ImageAPI.AddOrUpdateImage(json));
            Context.Response.End();
        }
        #endregion

        #region 模型管理
        [WebMethod(Description = @"<h3>method: GetModelList</h3>
        <p>方法描述：<strong>获取所有模型</strong></p> 
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'模型列表','result':[{'id':3,'url':'http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png1','name':'hhhhhhhhh'}]}</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetModelList()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ModelAPI.GetModelList());
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: AddModel</h3>
        <p>方法描述：<strong>添加model</strong></p> 
        <p>参数：{<strong>name<=>model name</strong>}</p>
        <p>参数：{<strong>url<=>model url</strong>}</p>
        <p style='color:green'>成功结果：<strong>成功</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void AddModel(string name, string url)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ModelAPI.AddModel(name, url));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: AddModel</h3>
        <p>方法描述：<strong>添加model</strong></p> 
        <p>参数：{<strong>name<=>model name</strong>}</p>
        <p>参数：{<strong>url<=>model url</strong>}</p>
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'修改成功','result':'success,SQLupdate `model` set id='2',url='http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png',name='hhhhhhhhh',candelete='1',isdelete='1' where id=2;'}</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void SaveModel(string json)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(ModelAPI.AddOrUpdateModel(json));
            Context.Response.End();
        }

        #endregion

        #region 全景管理
        [WebMethod(Description = @"<h3>method: GetPanoramaList</h3>
        <p>方法描述：<strong>获取所有全景</strong></p> 
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'获取成功','result':[{'id':1,'url':'http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png','longitude':1.2300000000,'latitude':1.3400000000,'name':'hhhhhhhhh','candelete':true}]}</strong></p>
        <p style='color:red'>失败结果：<strong>获取失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetPanoramaList()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PanoramaAPI.GetPanoramaList());
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: AddPanorama</h3>
        <p>方法描述：<strong>添加panorama</strong></p> 
        <p>参数：{<strong>name<=>panorama name</strong>}</p>
        <p>参数：{<strong>url<=>panorama url</strong>}</p>
        <p style='color:green'>成功结果：<strong>成功</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void AddPanorama(string name, string url)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PanoramaAPI.AddPanorama(name, url));
            Context.Response.End();
        }
        #endregion

        [WebMethod(Description = @"<h3>method: AddPanorama</h3>
        <p>方法描述：<strong>添加panorama</strong></p> 
        <p>参数：{<strong>name<=>panorama name</strong>}</p>
        <p>参数：{<strong>url<=>panorama url</strong>}</p>
        <p style='color:green'>成功结果：<strong>{'status':'200','message':'修改成功','result':'success,SQLupdate `panorama` set id='2',url='http://smartmap.gotoningbo.com/tiles/[z]/[x]/[y].png',longitude='1.23',latitude='1.34',name='hhhhhhhhh',candelete='1',isdelete='1' where id=2;'}</strong></p>
        <p style='color:red'>失败结果：<strong>失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void SavePanorama(string json)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(PanoramaAPI.AddOrUpdatePanorama(json));
            Context.Response.End();
        }


        [WebMethod(Description = @"<h3>method: getReportList</h3>
        <p>方法描述：<strong>获取report集合</strong></p> 
        <p style='color:green'>成功结果：<strong>{'status':200,'message':'获取列表成功','result':[{'id':1,'report_person':'1','reportdate':'1','phone':'1','content':'1','type':'1','pid':1,'pic':[{'id':131879593684158846,'pid':1,'typeid':6,'filetype':'poi','filename':'01','filepath':'/service/upload/1/131879593684158846.xlsx','totalbytes':18920,'ext':'xlsx','note':'18.48 K','createtime':'2018-11-29T18:02:48','creatorid':1,'creator':'1','delflag':false,'fileblob':null,'rid':1}]}]}</strong></p>
        <p style='color:red'>失败结果：<strong>删除失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetReportList()
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(reportService.getReportList());
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: GetReportListById</h3>
        <p>方法描述：<strong>删除user</strong></p> 
        <p>参数：{<strong>id<=>报告ID</strong>}</p>
        <p style='color:green'>成功结果：<strong>{'status':200,'message':'获取列表成功','result':[{'id':1,'report_person':'1','reportdate':'1','phone':'1','content':'1','type':'1','pid':1,'pic':[{'id':131879593684158846,'pid':1,'typeid':6,'filetype':'poi','filename':'01','filepath':'/service/upload/1/131879593684158846.xlsx','totalbytes':18920,'ext':'xlsx','note':'18.48 K','createtime':'2018-11-29T18:02:48','creatorid':1,'creator':'1','delflag':false,'fileblob':null,'rid':1}]}]}</strong></p>
        <p style='color:red'>失败结果：<strong>删除失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void GetReportListById(int id)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(reportService.getReportListById(id));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: CreateDept</h3>
        <p>方法描述：<strong>创建report</strong></p> 
        <p>参数：{<strong>id<=>报告ID|</strong><strong>report_person<=>报告人|</strong><strong>reportdate<=>报告时间|</strong><strong>phone<=>手机号码|</strong><strong>content<=>内容|</strong><strong>type<=>类型|</strong><strong>pid<=>poi的id|</strong>}</p>
        <p style='color:green'>成功结果：<strong>创建成功</strong></p>
        <p style='color:red'>失败结果：<strong>创建失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void AddReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(reportService.AddReport(id, report_person, reportdate, phone, content, type, pid));
            Context.Response.End();
        }

        [WebMethod(Description = @"<h3>method: UpdateDept</h3>
        <p>方法描述：<strong>修改dept</strong></p> 
        <p>参数：{<strong>id<=>报告ID|</strong><strong>report_person<=>报告人|</strong><strong>reportdate<=>报告时间|</strong><strong>phone<=>手机号码|</strong><strong>content<=>内容|</strong><strong>type<=>类型|</strong><strong>pid<=>poi的id|</strong>}</p>
        <p style='color:green'>成功结果：<strong>修改成功</strong></p>
        <p style='color:red'>失败结果：<strong>修改失败</strong></p>
        <p>输出格式：<strong>json</strong></p></br>")]
        public void UpdateReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.ContentEncoding = Encoding.GetEncoding("utf-8");
            Context.Response.Write(reportService.UpdateReport(id, report_person, reportdate, phone, content, type, pid));
            Context.Response.End();
        }

    }
}
