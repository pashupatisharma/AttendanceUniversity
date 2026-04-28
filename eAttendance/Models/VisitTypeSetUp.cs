using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class VisitTypeSetUp
    {
        [Key()]
        public int VisitTypeId { get; set; }

        [Display(Name = "कार्यालय")]
        public int? OfficeId { get; set; }
        public virtual OfficeSetUp OfficeSetUp { get; set; }

        [Display(Name = "काजको प्रकार "), Required]
        public string VisitTypeName { get; set; }

        [Display(Name = "छोटकरी नाम ")]
        public string Alias { get; set; }

        [Display(Name = "विवरण  ")]
        public string Description { get; set; }

        [Display(Name = "स्थिति")]
        public int Status { get; set; }



        public int flag { get; set; }



        [NotMapped]
        public IEnumerable<VisitTypeSetUp> VisitTypeSetUpList { get; set; }
    }
}