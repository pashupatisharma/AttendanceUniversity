using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class EmployeeLeaveSummaryList
    {
        // Properties
        public int LeaveTypeId { get; set; }

        public string LeaveTypeName { get; set; }

        public decimal LastYear { get; set; }

        public decimal ThisYear { get; set; }

        public decimal Total { get; set; }

        public decimal TakenLeave { get; set; }

        public decimal TakenLeaveYearly { get; set; }

        public decimal RemainingLeave { get; set; }

        public int EmployeeId { get; set; }

        public decimal OpenningBalance { get; set; }

        public decimal NoOfLeave { get; set; }

        public int LeaveApplicationId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public bool IsHalfDay { get; set; }

        public int HolidayCalendarId { get; set; }

        public string HolidayTypeName { get; set; }
    }


}