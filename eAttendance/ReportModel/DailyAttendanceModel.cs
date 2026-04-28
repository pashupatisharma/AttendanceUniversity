using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ReportModel
{
  public class DailyAttendanceModel
{
    // Properties
    public int EmployeeId { get; set; }

    public string Name { get; set; }

    public string LevelName { get; set; }

    public string DesignationName { get; set; }

    public string ShiftTime { get; set; }

    public DateTime InDateTime { get; set; }

    public DateTime OutDateTime { get; set; }

    public string InDuration { get; set; }

    public string OutDuration { get; set; }

    public string Remarks { get; set; }

    public List<DailyAttendanceModel> DailyAttendanceModelList { get; set; }

    public List<EmployeeAttendanceList> EmployeeAttendanceList { get; set; }

    public string Level { get; set; }

    public string Designation { get; set; }

    public string EmployeeName { get; set; }

    public string EmployeeNo { get; set; }
}

 

 

}