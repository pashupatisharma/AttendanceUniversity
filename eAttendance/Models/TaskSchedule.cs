using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class TaskSchedule : BaseModel
    {
        // Properties
        [Key()]
        public int TaskId { get; set; }

        [Display(Name = "EmployeeId "), Required]
        public string EmployeeId { get; set; }

        [Display(Name = "Task ")]
        public string Task { get; set; }

        [Display(Name = "Date ")]
        public DateTime Date { get; set; }

        [Display(Name = "मिति")]
        public string NDate { get; set; }

        [Display(Name = "FromTime ")]
        public string FromTime { get; set; }

        [Display(Name = "ToTime ")]
        public string ToTime { get; set; }

        [Display(Name = "स्थिति")]
        public int Status { get; set; }

      

        public int flage { get; set; }

        public List<TaskSchedule> TaskScheduleList { get; set; }

    }
}