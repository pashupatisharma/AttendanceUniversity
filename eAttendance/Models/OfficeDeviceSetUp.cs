using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Models
{
    public class OfficeDeviceSetUp
    {
        [Key()]
        public int OfficeDeviceId { get; set; }

        [Display(Name = "कार्यालयको नाम  ")]
        public  int OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }

        public virtual IEnumerable<SelectListItem> Offices { get; set; }

        public int DeviceType { get; set; }

        [Display(Name = "लाइसेन्स नम्बर ")]
        public string LicenseNo { get; set; }

        [Display(Name = "Serail No ")]
        public string DeviceNo { get; set; }

        [Display(Name = "उपकरण आईपी ")]
        public string DeviceIp { get; set; }

        [Display(Name = "पोर्ट ")]
        public string Port { get; set; }

        [Display(Name = "स्थिति ")]
        public int Status { get; set; }
        [NotMapped]

        public string LastImportDate_Loc { get; set; }


        public DateTime LastImportDate { get; set; }

        [NotMapped()]
        public List<OfficeDeviceSetUp> SetupOfficeDeviceList { get; set; }
    }
}