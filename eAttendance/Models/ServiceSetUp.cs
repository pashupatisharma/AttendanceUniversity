using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class ServiceSetUp : BaseModel
    {
        [Key]
         public int ServiceId { get; set; }

        [Display(Name = "सेवाको नाम "), Required]
        public string ServiceName { get; set; }

        [Display(Name = "उपनाम")]
        public string Alias { get; set; }

        [Display(Name = "विवरण ")]
        public string Description { get; set; }

        [Display(Name = "हेराउने क्रम")]
        public int DisplayOrder { get; set; }

        [Display(Name = "स्थिति ")]
        public int Status { get; set; }


        public int flage { get; set; }

        [NotMapped()]
        public List<ServiceSetUp> ServiceSetUpList { get; set; }
    }
}