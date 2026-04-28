using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
    public class DailyLeaveReportModel
{
    // Properties
    public int LeaveApplicationId { get; set; }

    public int EmployeeId { get; set; }

    public int EmployeeNo { get; set; }

    public string EmployeeName { get; set; }

    public string EmployeeNameNp { get; set; }

    public int OfficeId { get; set; }

    public int MainBranchId { get; set; }

    public int MainBranchDisplayOrder { get; set; }

    public int BranchId { get; set; }

    public int BranchDisplayOrder { get; set; }

    public int DepartmentId { get; set; }

    public int DepartmentDisplayOrder { get; set; }

    public int ServiceId { get; set; }

    public int ServiceDisplayOrder { get; set; }

    public int LevelId { get; set; }

    public int LevelDisplayOrder { get; set; }

    public int DesignationId { get; set; }

    public int DesignationDisplayOrder { get; set; }

    [Display(Name="मिति")]
    public DateTime? Date { get; set; }

    [NotMapped, Display(Name="मिति")]
    public string NDate { get; set; }

    public int LeaveTypeId { get; set; }

    public int RecommenderId { get; set; }

    public int RecommendStatus { get; set; }

    public int? ApproverId { get; set; }

    public int? ApprovedStatus { get; set; }

    public DateTime? ApplicationDate { get; set; }

    [Display(Name="मिति देखि "), NotMapped]
    public string NFromDate { get; set; }

    [Display(Name="मिति देखि ")]
    public DateTime? FromDate { get; set; }

    [Display(Name="मिति सम्म  "), NotMapped]
    public string NToDate { get; set; }

    [Display(Name="मिति सम्म  ")]
    public DateTime? ToDate { get; set; }

    public bool IsHalfDay { get; set; }

    public int Employee_Order { get; set; }

    public int LeaveStatus { get; set; }

    public List<DailyLeaveReportModel> DailyLeaveReportModelList { get; set; }
}

 

 

}