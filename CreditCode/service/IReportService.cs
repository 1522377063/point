using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Point.service
{
    interface IReportService
    {
        string getReportList();
        string getReportListById(int id);
        string AddReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid);
        string UpdateReport(int id, string report_person, string reportdate, string phone, string content, string type, int pid);
    }
}
