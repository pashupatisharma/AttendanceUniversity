using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class AspNetUserRoleModel
    {
        // Properties
        public int id { get; set; }

        public string UserId { get; set; }

        public string RoleId { get; set; }

        public int EmployeeId { get; set; }

        [Display(Name = "प्रयोगकर्ताको नाम")]
        public string EmployeeName { get; set; }

        [Display(Name = "लिंग")]
        public int Gender { get; set; }

        [Display(Name = "वैवाहिक स्थिति ")]
        public int MaritalStatus { get; set; }

        [Display(Name = "इमेल आइढी ")]
        public string EmailId { get; set; }

        [Display(Name = "फोन नम्बर ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "मोबाइल नम्बर")]
        public string MobileNumber { get; set; }

        public List<AspNetUserRoleModel> AspNetUserRoleList { get; set; }

        public List<AspNetRoleModel> AspNetRoleList { get; set; }

        public List<EmployeeInfo> EmployeeInfoList { get; set; }

        public List<EmployeeOfficeDetail> EmployeeOfficeInfoList { get; set; }

        public List<EmployeeShiftTime> EmployeeShiftTime { get; set; }



        public string Name { get; set; }

        public bool IsCheck { get; set; }
    }
}