using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.ViewModel
{
    public class AttendanceCountModel
    {
        public string Name { get; set; }

        public int Total { get; set; }

        public int TotalActive { get; set; }

        public int TotalDeactive { get; set; }

        public int? Present { get; set; }

        public int OnLeave { get; set; }

        public int OnVisit { get; set; }

        public int Absent { get; set; }

        public int Count { get; set; }
    }
}