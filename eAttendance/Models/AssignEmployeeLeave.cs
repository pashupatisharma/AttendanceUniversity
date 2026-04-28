using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class AssignEmployeeLeave : BaseModel
    {

        [Key]
        public int LeaveApplicationId { get; set; }


        [Display(Name = " वर्ष "), Required]
        public int? FiscalYearId { get; set; }
        public virtual FiscalYearSetUp FiscalYearSetUp { get; set; }

        [Display(Name = "कर्मचारी")]
        public int EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }



        [Display(Name = " कार्यालय")]
        public int OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }

        [Display(Name = "बिदा किसिम ")]
        public int LeaveTypeId { get; set; }
        public virtual LeaveTypeSetUp LeaveTypeSetUp { get; set; }



        [Display(Name = "गत सालको बिदा ")]
        public decimal OpenningBalance { get; set; }

        [Display(Name = " यस बर्षको बिदा")]
        public decimal NoOfLeave { get; set; }
        [Display(Name = "Status")]
        public int Status { get; set; }
        [NotMapped()]
        public List<AssignEmployeeLeave> AssignEmployeeLeaveList { get; set; }



        [NotMapped]

        public string LeaveTypeName { get; set; }
        //[NotMapped]
        //public List<EmployeeInfo> Employees { get; set; }


        [NotMapped]
        public List<LeaveTypeSetUp> LeaveTypes { get; set; }
        //[NotMapped()]
        //public string UniqueId1 { get; set; }
        //[NotMapped()]
        //public string UniqueId2 { get; set; }



    }





}
