using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class ForgotEntry:BaseModel
    {
        [Key()]
        public int ForegotEntryId { get; set; }

        [Required, Display(Name = "कर्मचारी ")]
        public int? EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }

        [Display(Name = "चेक प्रकार  ")]
        public int CheckType { get; set; }

        [Display(Name = "समय"), Required]
        public TimeSpan ForgotTime { get; set; }

        [Display(Name = "मिति")]
        public DateTime ForgotDate { get; set; }

        [Display(Name = "मिति")]
        public string NForgotDate { get; set; }

        [Display(Name = "समय")]
        public string NForgotTime { get; set; }

        public int ShiftTimeId { get; set; }

        [Display(Name = "अनुरोध गर्नु")]
        public int RequestedBy { get; set; }

        [Display(Name = "स्विक्रित गर्नु ")]
        public int ApprovedBy { get; set; }

        [Display(Name = "टिप्पणी")]
        public string Remarks { get; set; }

        [Display(Name = "स्थिति")]
        public int Status { get; set; }

        [NotMapped]
        [Display(Name = "कार्यालय")]
        public int OfficeId { get; set; }

        public int flag { get; set; }



        [NotMapped]

        public IEnumerable<ForgotEntry> ForgotEntryList { get; set; }

        public List<Controllers.StatusListModel> StatusModelList { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string DeletedRemarks { get; set; }
        [NotMapped]
        public EmployeeOfficeDetail EmployeeOfficeDetail { get;  set; }
    }





}
