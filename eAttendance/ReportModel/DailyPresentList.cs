using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class DailyPresentList
    {
        // Properties
        public int SNo { get; set; }

        public string DesignationName { get; set; }

        public int EmployeeId { get; set; }

        public int EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        public string ShiftTime { get; set; }

        public DateTime InDateTime { get; set; }

        public DateTime OutDateTime { get; set; }

        public string InRemarks { get; set; }

        public string OutRemarks { get; set; }

        public decimal WorkedHour { get; set; }
    }


}