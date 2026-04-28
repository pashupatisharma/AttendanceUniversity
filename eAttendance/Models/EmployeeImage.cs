using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmployeeImage : BaseModel
    {
        [Key]
        public int EmployeeImageId { get; set; }

        public string ImageName { get; set; }

        public string ImageType { get; set; }

        public string Size { get; set; }

        public int? EmployeeId { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }

        public int? Type { get; set; }



        public List<EmployeeImage> EmployeeImageInfoList { get; set; }
    }
}