using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class DistrictSetUp
    {
        [Key()]
        public int DistrictId { get; set; }

        [Display(Name = "जिल्लाको कोड ")]
        public string DistrictCode { get; set; }

        [Required, Display(Name = "जिल्लाको नाम ")]
        public string DistrictName { get; set; }

        [Display(Name = "हेराउने क्रम  ")]
        public int DisplayOrder { get; set; }

        [Display(Name = "अञ्चल")]
        public int ZoneId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }


        [NotMapped]
        public List<DistrictSetUp> SetupDistrictList { get; set; }
    }
}