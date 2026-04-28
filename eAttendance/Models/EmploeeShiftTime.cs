using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmploeeShiftTime : BaseModel
    {  [Key()]
        public int EmployeeShiftTimeId { get; set; }
        public int EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int? ShiftId { get; set; }
        public virtual Shift Shift { get; set; }
        public int Stauts { get; set; }

    }
}