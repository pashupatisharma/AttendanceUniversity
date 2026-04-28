using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class DailyAbsentAttendanceReportModel
    {
        // Properties
        public int EmployeeId { get; set; }

        public int EmployeeCode { get; set; }

        public string Name { get; set; }

        public string NameEn { get; set; }

        public DateTime Date { get; set; }

        public int OfficeId { get; set; }

        public int MainBranchId { get; set; }

        public int BranchId { get; set; }

        public int ServiceId { get; set; }

        public int ServiceDisplayOrder { get; set; }

        public int LevelId { get; set; }

        public string LevelName { get; set; }

        public int LevelDisplayOrder { get; set; }

        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public int DesignationDisplayOrder { get; set; }

        public int EmployeeOrder { get; set; }

        public List<DailyAbsentAttendanceReportModel> DailyAbsentAttendanceReportModelList { get; set; }
    }



}