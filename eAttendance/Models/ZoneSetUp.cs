using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class ZoneSetUp
    {
        [Key()]
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
    }
}