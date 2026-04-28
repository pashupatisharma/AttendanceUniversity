using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class DeviceData
    {

       [Key]
        public int DeviceEnrollDataId { get; set; }

        public int OfficeId { get; set; }

        public int OfficeDeviceId { get; set; }

        public string IpAddress { get; set; }

        public int EnrollNumber { get; set; }


        public int InOutMode { get; set; }
   

        public string EnrollName { get; set; }

        public DateTime DateTime { get; set; }

       
    }
}