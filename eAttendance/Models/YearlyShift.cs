using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class YearlyShift : BaseModel
    {

        [Key]
        public int YearlyShiftId { get; set; }

        [Display(Name = "बर्ष"), Required]
        public int FiscalYearId { get; set; }

        [Display(Name = "शिफ्ट समय ")]
        public int ShiftId { get; set; }

        [Display(Name = "मिति दखि ")]
        public DateTime StartDate { get; set; }

        public string NStartDate { get; set; }

        [Display(Name = "मिति सम्म  ")]
        public DateTime EndDate { get; set; }

        public string NEndDate { get; set; }

        [Display(Name = "स्थिति ")]
        public int Status { get; set; }

        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }

        public int flag { get; set; }
        [NotMapped]
        public List<YearlyShift> YearlyShiftList { get; set; }
    }
}