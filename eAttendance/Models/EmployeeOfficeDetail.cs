using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmployeeOfficeDetail : BaseModel
    {
        [Key()]
        public int OfficeInfoId { get; set; }

    
        [Required]
        public int? EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public DateTime EffectiveDate { get; set; }

        [NotMapped, Display(Name = "लागुहुने दिन")]
        public string NEffectiveDate { get; set; }

        [Display(Name = " कार्यालय")]
        public int? OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }


        [Display(Name = " शाखा")]
        public int? BranchId { get; set; }
        public virtual BranchSetUp BranchSetUp { get; set; }


        [Display(Name = " विभाग")]
        public int? DepartmentId { get; set; }
        public virtual DepartmentSetup DepartmentSetup { get; set; }



        [Display(Name = "श्रेणी")]
        public int? LevelId { get; set; }

        public virtual LevelSetUp LevelSetUp { get; set; }

        [Display(Name = " पद")]
        public int? DesignationId { get; set; }
        public virtual DesignationSetUp DesignationSetUp { get; set; }


        [Display(Name = " सेवा ")]
        public int? ServiceId { get; set; }
        public virtual ServiceSetUp ServiceSetUp { get; set; }



        [Display(Name = " कैफियत")]
        public string Remarks { get; set; }



        public int Status { get; set; }













    }
}