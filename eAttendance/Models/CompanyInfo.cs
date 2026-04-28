using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class CompanyInfo
    {
        [Key()]
        public int CompanyInfoId { get; set; }
        public string Title { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string LogoName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Copyright { get; set; }


        public string Alias { get; set; }
    }
}