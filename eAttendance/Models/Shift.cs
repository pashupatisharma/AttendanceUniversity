using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class Shift
    {
           [Key()]
        public int ShiftId { get; set; }

        [StringLength(100), Required, Display(Name = "सिफ्टको नाम  ")]
        public string ShiftName { get; set; }

        [Display(Name = "स्थिति ")]
        public bool Status { get; set; }

        [Display(Name = "हेराउने क्रम ")]
        public int DisplayOrder { get; set; }


        [NotMapped()]
        public List<Shift> ShiftList { get; set; }
    }
}