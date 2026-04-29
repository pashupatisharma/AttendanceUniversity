using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmployeeShiftTime : BaseModel
    {  [Key()]
        public int EmployeeShiftTimeId { get; set; }
        public int? EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int? ShiftId { get; set; }
        public virtual Shift Shift { get; set; }
        public int Stauts { get; set; }

        public int? ShiftTypeId { get; set; }
        public virtual ShiftType ShiftType { get; set; }
        public string Remarks { get; set; }

        public  string  NEffectiveDate { get; set; }
        [NotMapped()]
        public int OfficeId { get; set; }
    }
}