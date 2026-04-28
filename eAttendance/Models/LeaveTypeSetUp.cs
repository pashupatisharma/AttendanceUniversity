using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace eAttendance.Models
{
    public class LeaveTypeSetUp : BaseModel
    {
        [Key()]
        public int LeaveTypeId { get; set; }

        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }

        [Required, Display(Name = " बिदाको प्रकार ")]
        public string LeaveTypeName { get; set; }

        [Display(Name = "छोटकरी नाम ")]
        public string Alias { get; set; }

        [Display(Name = "विवरण ")]
        public string Description { get; set; }

        [Display(Name = "लागू लागि  ")]
        public int ApplicableFor { get; set; }


        [Display(Name = "साप्ताहिक बन्द समावेश ")]
        public bool WeeklyOffInclude { get; set; }

        [Display(Name = "सार्वजनिक बिदा समावेश")]
        public bool HolidayInclude { get; set; }

        [Display(Name = "अधिकतम सीमा समावेश")]
        public bool AllowLeaveLimitExceed { get; set; }

        [Display(Name = "ब्रेवक् डाउन")]
        public int BreakDown { get; set; }

        [Display(Name = "अधिकतम सीमा ")]
        public int MaximumLimit { get; set; }

        [Display(Name = "पूर्वनिर्धारित")]
        public decimal? DefaultValue { get; set; }

        [Display(Name = "हेराउने क्रम")]
        public int DisplayOrder { get; set; }

        [Display(Name = "सेवा")]
        public int ServiceId { get; set; }

        [Display(Name = "पूर्वनिर्धारित छ ")]
        public bool IsDefault { get; set; }

        [Display(Name = "हस्तांतरणीय छ ")]
        public bool IsTransferable { get; set; }

        [Display(Name = "  प्रकार ")]
        public string Type { get; set; }
        public int Status { get; set; }

        [NotMapped()]
        public IEnumerable<LeaveTypeSetUp> SetupLeaveTypeList { get; set; }
        [NotMapped()]
        public int? LeaveApplicationId { get; set; }
        [NotMapped()]
        public decimal OpenningBalance { get; set; }
        [NotMapped()]
        public decimal NoOfLeave { get; set; }

        [NotMapped()]
        public string UniqueId1 { get; set; }
        [NotMapped()]
        public string UniqueId2 { get; set; }
    }


    public class LeaveTypeSetUpModel
    {
        public int? LeaveTypeId { get; set; }  
        public string LeaveTypeName { get; set; }
    }
}