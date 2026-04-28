using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class EmployeeAttendanceList
    {
        // Properties
        public int? EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeNameNp { get; set; }

        public string EmployeeNameAndCode { get; set; }

        public string EmployeeNameAndCodeNp { get; set; }

        public int OfficeId { get; set; }
        public string OfficeName { get; set; }

        public int BranchId { get; set; }

        public int ServiceId { get; set; }

        public int LevelId { get; set; }

        public int DesignationId { get; set; }

        public int? ShiftTimeId { get; set; }

        public string ShiftTimeName { get; set; }

        public DateTime? LogDate { get; set; }
        public string nLogDate { get; set; }

        public TimeSpan? CheckIn { get; set; }

        public TimeSpan? CheckOut { get; set; }

        public decimal? TWH { get; set; }

        public string CheckInRemarks { get; set; }

        public string CheckOutRemarks { get; set; }

        public int StatusType { get; set; }

        public int WorkType { get; set; }

        public string OnLeave { get; set; }

        public string OnVisit { get; set; }

        public string Holiday { get; set; }

        public string Remarks { get; set; }

        public string EmployeeNo { get; set; }

        public string VisitTypeName { get; set; }
        public int YearId { get; set; }

        public int? Total { get; set; }
        public List<EmployeeAttendanceList> EmployeeAttendanceLists { get; set; }



        public string LeaveTypeName { get; set; }

        public decimal LastYear { get; set; }

        public decimal ThisYear { get; set; }

        public int TakenLeave { get; set; }

        public int RemainingLeave { get; set; }

        public int LeaveTypeId { get; set; }

        public decimal OpenningBalance { get; set; }

        public decimal NoOfLeave { get; set; }

        public int LeaveApplicationId { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int MonthId { get; set; }

        public string _nFromDate { get; set; }

        public string _nToDate { get; set; }

        public int StausType { get; set; }



        public int LevelDisplayOrder { get; set; }

        public int ServiceDisplayOrder { get; set; }

        public int DesignationDisplayOrder { get; set; }
    }
}