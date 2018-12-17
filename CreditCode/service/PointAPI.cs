/*************************************************
** Company： 宁波市安贞信息科技有限公司
** auth：    tzz
** date：    2018/5/22
** desc：    扬尘管理接口
** Ver.:     V1.0.0
**************************************************/

using System.Web;

namespace Point.service
{
    interface PointAPI
    {

        /// <summary>
        /// 向组织表传入公司数据
        /// </summary>
        /// <param name="colName">列名(多个英文逗号隔开)</param>
        /// <param name="timeInterval">时间间隔</param>
        /// <returns>string</returns>
        string addPointOrUpdate(string json);
        string addLayer(string json);
        string getType(string type);
        string getAll();
        string getAllLayer();
        string upLoadFile(HttpPostedFile file, string parentId, string typeId, string userId, string userName);
        object getPOI(object poiid);
    }
}