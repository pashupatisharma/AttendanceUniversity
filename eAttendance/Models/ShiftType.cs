using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class ShiftType
    {
        [Key]
        public int ShiftTypeId { get; set; }

        [Required] 
        public string Name { get; set; } // Summer / Winter

        public bool IsActive { get; set; }

        public int Status { get; set; }

    }
}