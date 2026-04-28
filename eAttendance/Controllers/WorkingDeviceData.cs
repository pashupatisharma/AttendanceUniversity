using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace eAttendance.Controllers
{
    public class AttendanceLog  
    {
        [Key()]
        public int DeviceDataId { get; set; }
        public DateTime? DateTime { get; set; }

        public int OfficeDeviceId { get; set; }

        public int? EmployeeId { get; set; }

        public int? ShiftTimeId { get; set; }

        public string EnrollNumber { get; set; }

        public string VerifyMode { get; set; }

        public int Status { get; set; }

        internal string Remarks { get; set; }



        public string InOutMode { get; set; }

        public string IpAddress { get; set; }

        public int OfficeId { get; set; }

        public System.DateTime? DeletedDate { get; set; }

        public string DeletedBy { get; set; }
    }
}
