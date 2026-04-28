using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class HolidayCalender:BaseModel
    {

        [Key()]
        public int HolidayCalendarId { get; set; }

        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }

        [Display(Name = "बिदाको प्रकार")]
        public string HolidayTypeName { get; set; }

        [Display(Name = "छोटकरी नाम")]
        public string Alias { get; set; }

        [Display(Name = "मिति देखि ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "मिति देखि "), NotMapped]
        public string NFromDate { get; set; }

        [Display(Name = "मिति सम्म ")]
        public DateTime ToDate { get; set; }

        [Display(Name = "मिति सम्म "), NotMapped]
        public string NToDate { get; set; }

        [Display(Name = "लागु")]
        public int ApplicableFor { get; set; }

      

        public int Status { get; set; }

        public List<HolidayCalender> HolidayCalendarList { get; set; }
    }
}