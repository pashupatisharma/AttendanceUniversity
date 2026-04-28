using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class WeekDaySetUp
    {
        // Properties
        [Key()]
        public int WeekDayId { get; set; }

        [Display(Name = "WeekDayName  "), Required]
        public string WeekDayName { get; set; }

        [Display(Name = "WeekDay ")]
        public int WeekDay { get; set; }

        [Display(Name = "Office ")]
        public int OfficeId { get; set; }

        [Display(Name = "WorkType")]
        public int WorkType { get; set; }

       
    
    }
}