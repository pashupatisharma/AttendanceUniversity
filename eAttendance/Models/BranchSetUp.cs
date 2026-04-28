using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class BranchSetUp : BaseModel
    {
        [Key()]
        public int BranchId { get; set; }

        [Required, Display(Name = "शाखाको नाम ")]
        public string BranchName { get; set; }

        [Display(Name = "उपनाम ")]
        public string Alias { get; set; }

        [Display(Name = "विवरण ")]
        public string Description { get; set; }

        [Display(Name = "शाखा आन्तार्गत")]
        public int? ParentBranchId { get; set; }

        [Display(Name = "स्थिति")]
        public int Status { get; set; }

        [Display(Name = "हेराउने क्रम ")]
        public int DisplayOrder { get; set; }

        [NotMapped()]
        public List<BranchSetUp> BranchSetUpList { get; set; }

    }
}