using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace eAttendance.Models
{
    public class DepartmentSetup:BaseModel
    {
    

        // Properties
        [Key()]
        public int DepartmentId { get; set; }

        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }

        [Display(Name = "विभागको नाम "), Required]
        public string DepartmentName { get; set; }

        [Display(Name = "उपनाम")]
        public string Alias { get; set; }

        [Display(Name = "विभाग अन्तर्गत")]
        public int ParentDepartmentId { get; set; }

        public IEnumerable<SelectListItem> ParentDepartment { get; set; }

        [Display(Name = "विवरण")]
        public string Description { get; set; }

        [Display(Name = "हेराउने क्रम ")]
        public int DisplayOrder { get; set; }

        [Display(Name = "स्थिति")]
        public int Status { get; set; }

        [NotMapped()]
        public List<DepartmentSetup> DepartmentSetupList { get; set; }
    }
}