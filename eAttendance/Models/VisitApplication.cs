using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class VisitApplication:BaseModel
    {
        [Key()]
        public int VisitApplicationId { get; set; }

        [Display(Name = " कर्मचारीको नाम")]
        public int? EmployeeId { get; set; }

        public virtual EmployeeInfo EmployeeInfo { get; set; }

        [Display(Name = "आवेदन मिति ")]
        public DateTime ApplicationDate { get; set; }

        public string NApplicationDate { get; set; }

        [Display(Name = "काजको प्रकार ")]
        public int VisitTypeId { get; set; }
        public virtual VisitTypeSetUp VistTypeSetUp { get; set; }



        [Display(Name = "विषय")]
        public string Subject { get; set; }

        [Display(Name = "मिति देखि ")]
        public DateTime FromDate { get; set; }

        [Display(Name = "मिति देखि ")]

        public string NFromDate { get; set; }

        [Display(Name = " मिति सम्म​")]
        public DateTime ToDate { get; set; }

        [Display(Name = " मिति सम्म​")]
        public string NToDate { get; set; }

        [Display(Name = "सन्देश")]
        public string Message { get; set; }

        [Display(Name = "अनुरोध गर्नु")]
        public int RequestedTo { get; set; }

        [Display(Name = "स्विक्रित गर्ने ")]
        public int ApprovedBy { get; set; }

        [Display(Name = "सिफारिस गर्ने")]
        public int Recomender { get; set; }

        [Display(Name = "सिफारिस अवस्था")]
        public int RecomenderStatus { get; set; }

        [Display(Name = "Recomended Date")]
        public DateTime? RecomendedDate { get; set; }

        [Display(Name = "Total Days")]
        public decimal TotalDays { get; set; }

        [Display(Name = "स्वीकृत  अवस्था")]
        public int ApprovedStatus { get; set; }

        public int Type { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "स्वीकृत  मिति")]
        public string NApprovedDate { get; set; }

        [NotMapped]
        [Display(Name = "कार्यालय")]
        public int OfficeId { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }

        [NotMapped()]
        public List<VisitApplication> VisitApplicationList { get; set; }
        [NotMapped()]
        public EmployeeOfficeDetail EmployeeOfficeDetail { get;  set; }
    }



    public class VisitApplicationModel
    {
        public DateTime ApplicationDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? EmployeeId { get; set; }
        [Display(Name = "स्वीकृत गर्ने")]
        public int? ApprovedBy { get; set; }

        [Display(Name = "स्वीकृत  अवस्था")]
        public int? ApprovedStatus { get; set; }
        [Display(Name = "सिफारिस गर्ने")]
        public int Recomender { get; set; }

        [Display(Name = "सिफारिस अवस्था")]
        public int RecomenderStatus { get; set; }
        [Display(Name = "सन्देश")]
        public string Message { get; set; }
        public DateTime? RecomendedDate { get; set; }
        public List<VisitApplicationModel> VisitApplicationList { get; set; }
        [Display(Name = "विषय")]
        public string Subject { get; set; }
        public int VisitApplicationId { get; set; }


        public string NFromDate { get; set; }

        public string NToDate { get; set; }

        public decimal TotalDays { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public int Status { get; set; }
    }
}