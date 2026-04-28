using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class OfficeSetUp : BaseModel
    {
        [Key()]
       
        public int OfficeId { get; set; }

        [StringLength(100, MinimumLength = 3), Display(Name = "कार्यालयको नाम"), Required]
        public string OfficeName { get; set; }

        [Display(Name = "कोड")]
        public string Code { get; set; }

        [Display(Name = "ठेगाना ")]
        public string Address { get; set; }

        [Display(Name = "जिल्ला")]
        public int? DistrictId { get; set; }
        public virtual DistrictSetUp DistrictSetUp { get; set; }

        [Display(Name = "फोन नम्बर ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "हेराउने क्रम")]
        public int DisplayOrder { get; set; }

        [Display(Name = "स्थिति ")]
        public int Status { get; set; }

        [Display(Name = "माथिल्लो निकाय छ?")]
        public bool HasMainBranch { get; set; }

        public int? OfficeHead { get; set; }

        public int? OfficeSubHead { get; set; }

    

        public int flag { get; set; }

        [NotMapped()]
        public List<OfficeSetUp> SetupOfficeList { get; set; }

       
    }
}