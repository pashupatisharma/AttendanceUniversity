using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class DailyAttendanceReportModel
    {
        // Properties
        public int EmployeeId { get; set; }

        public int UserId { get; set; }

        public int EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public int OfficeId { get; set; }

        public int OfficeDeviceId { get; set; }

        public string IpAddress { get; set; }

        public int MainBranchId { get; set; }

        public int BranchId { get; set; }

        public int ServiceId { get; set; }

        public int DepartmentId { get; set; }

        public int LevelId { get; set; }

        public int LeaveDisplayOrder { get; set; }

        public int DesignationDisplayOrder { get; set; }

        public int ServiceDispalyOrder { get; set; }

        public int DesignationId { get; set; }

        public DateTime InTime { get; set; }

        public DateTime OutTime { get; set; }

        public List<DailyAttendanceReportModel> DailyAttendanceReportModelList { get; set; }
    }





}