using eAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ViewModel
{
    public class EmployeeViewModel
    {

        public RegisterViewModel RegisterViewModel { get; set; }

        public EmployeeInfo EmployeeInfo { get; set; }

        public AssignEmployeeLeave AssignEmployeeLeave { get; set; }

        public EmployeeOfficeDetail EmployeeOfficeInfo { get; set; }

        public EmployeeShiftTime EmployeeShiftTime { get; set; }

        public LeaveTypeSetUp SetupLeaveType { get; set; }



        public int PageNo { get; set; }

        public List<EmployeeViewModel> EmployeeViewModelList { get; set; }

        public List<LeaveTypeSetUp> SetupLeaveTypeModelList { get; set; }

        public List<AssignEmployeeLeave> AssignEmployeeLeaveLlist { get; set; }



    }
}