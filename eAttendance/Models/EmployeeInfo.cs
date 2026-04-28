using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmployeeInfo : BaseModel
    {
        [Key()]
        public int EmployeeId { get; set; }
        [NotMapped]
        public int? FiscalYearId { get; set; }

        [Display(Name = "सन्केत नम्बर ")]
        public string EmployeeNo { get; set; }

        [StringLength(100, MinimumLength = 3), Display(Name = "कर्मचारीको नाम"), Required]
        public string EmployeeNameNp { get; set; }

        [Required, StringLength(100, MinimumLength = 3), Display(Name = "कर्मचारीको नाम (English)")]
        public string EmployeeName { get; set; }

        [Display(Name = "जन्म मिति  ")]
        public DateTime? DateOfBirth { get; set; }

        [NotMapped, Display(Name = "जन्म मिति ")]
        public string NDateOfBirth { get; set; }

        [Display(Name = "लिंग")]
        public int? Gender { get; set; }

        [Display(Name = "वैवाहिक स्थिति ")]
        public int? MaritalStatus { get; set; }

        [Display(Name = "रक्त समूह ")]
        public int? BloodGroup { get; set; }

        [Display(Name = "इमेल")]
        public string EmailId { get; set; }

        [Display(Name = "फोन नम्बर ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "मोबाइल नम्बर")]
        public string MobileNumber { get; set; }

        [Display(Name = "नागरिकता नम्बर")]
        public string Qualification { get; set; }

        [Display(Name = "प्रयोगकर्ता ")]
        public string UserId { get; set; }

        [Display(Name = "स्थायी अञ्चल")]
        public int? PZone { get; set; }

        [Display(Name = "स्थायी जिल्ला ")]
        public int? PDistrict { get; set; }

        [Display(Name = "स्थायी गाविस ")]
        public string PVdc { get; set; }

        [Display(Name = "स्थायी वार्ड")]
        public int? PWardNo { get; set; }

        [Display(Name = "अस्थायी अञ्चल")]
        public int? TZone { get; set; }

        [Display(Name = "अस्थायी जिल्ला")]
        public int? TDistrict { get; set; }

        [Display(Name = "अस्थायी गाविस ")]
        public string TVdc { get; set; }

        [Display(Name = "अस्थायी वार्ड")]
        public int? TWardNo { get; set; }

        [Display(Name = "राष्ट्रियता ")]
        public int? NationalityId { get; set; }

        [Display(Name = "प्रवेश मिति ")]
        public DateTime? EntryDate { get; set; }

        [Display(Name = "प्रवेश मिति "), NotMapped]
        public string NEntryDate { get; set; }

        public int? DisplayOrder { get; set; }

        [Display(Name = "स्थिति ")]
        public int? Status { get; set; }
        [Display(Name = "रिपोर्टमा समावेश")]
        public int? GenerateReport { get; set; }


        [NotMapped]
        public List<LeaveTypeSetUp> LeaveTypes { get; set; }

        [NotMapped]
        public List<EmployeeInfo> EmoloyeeList { get; set; }

        [NotMapped]
        public int? LevelId { get; set; }
        [NotMapped]
        public int? OfficeId { get; set; }
        [NotMapped]
        public int? BranchId { get; set; }
        [NotMapped]
        public int? DesignationId { get; set; }
        [NotMapped]
        public int serviceId { get; set; }
        [NotMapped]
        public RegisterViewModel RegisterViewModel { get; set; }
    }


     public class EmployeeInfoReport:BaseModel
     {
         [Key()]
         public int EmployeeId { get; set; }

         [Display(Name = "सन्केत नम्बर ")]
         public string EmployeeNo { get; set; }

         [StringLength(100, MinimumLength = 3), Display(Name = "कर्मचारीको नाम"), Required]
         public string EmployeeNameNp { get; set; }

         [Required, StringLength(100, MinimumLength = 3), Display(Name = "कर्मचारीको नाम (English)")]
         public string EmployeeName { get; set; }

         [Display(Name = "जन्म मिति  ")]
         public DateTime? DateOfBirth { get; set; }

         [NotMapped, Display(Name = "जन्म मिति ")]
         public string NDateOfBirth { get; set; }

         [Display(Name = "लिंग")]
         public int? Gender { get; set; }

         [Display(Name = "वैवाहिक स्थिति ")]
         public int? MaritalStatus { get; set; }

         [Display(Name = "रक्त समूह ")]
         public int? BloodGroup { get; set; }

         [Display(Name = "इमेल")]
         public string EmailId { get; set; }

         [Display(Name = "फोन नम्बर ")]
         public string PhoneNumber { get; set; }

         [Display(Name = "मोबाइल नम्बर")]
         public string MobileNumber { get; set; }

         [Display(Name = "नागरिकता नम्बर")]
         public string Qualification { get; set; }

         [Display(Name = "प्रयोगकर्ता ")]
         public string UserId { get; set; }

         [Display(Name = "स्थायी अञ्चल")]
         public int? PZone { get; set; }

         [Display(Name = "स्थायी जिल्ला ")]
         public int? PDistrict { get; set; }

         [Display(Name = "स्थायी गाविस ")]
         public string PVdc { get; set; }

         [Display(Name = "स्थायी वार्ड")]
         public int? PWardNo { get; set; }

         [Display(Name = "अस्थायी अञ्चल")]
         public int? TZone { get; set; }

         [Display(Name = "अस्थायी जिल्ला")]
         public int? TDistrict { get; set; }

         [Display(Name = "अस्थायी गाविस ")]
         public string TVdc { get; set; }

         [Display(Name = "अस्थायी वार्ड")]
         public int? TWardNo { get; set; }

         [Display(Name = "राष्ट्रियता ")]
         public int? NationalityId { get; set; }

         [Display(Name = "प्रवेश मिति ")]
         public DateTime? EntryDate { get; set; }

         [Display(Name = "प्रवेश मिति "), NotMapped]
         public string NEntryDate { get; set; }

         public int? DisplayOrder { get; set; }

         [Display(Name = "स्थिति ")]
         public int? Status { get; set; }
         [Display(Name = "रिपोर्टमा समावेश")]
         public int? GenerateReport { get; set; }


         [NotMapped]
         public List<LeaveTypeSetUp> LeaveTypes { get; set; }

         [NotMapped]
         public List<EmployeeInfo> EmoloyeeList { get; set; }

         [NotMapped]
         public int? LevelId { get; set; }
         [NotMapped]
         public int? OfficeId { get; set; }
         [NotMapped]
         public int? BranchId { get; set; }
         [NotMapped]
         public int? DesignationId { get; set; }
         [NotMapped]
         public int? serviceId { get; set; }
     }
}

