using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class EmployeeAttendanceLogModel
    {

        // Properties
        public int DeviceDataId { get; set; }

        public int ForegotEntryId { get; set; }

        [Display(Name = "कर्मचारीको आईडी")]
        public int? EmployeeId { get; set; }

        public int EmployeeNo { get; set; }

        [Display(Name = "कर्मचारीको नाम")]
        public string EmployeeName { get; set; }

        public string EmployeeNameNp { get; set; }

        public string DesignationName { get; set; }

        [Display(Name = "सिफ्टको नाम  ")]
        public int? ShiftTimeId { get; set; }

        [Display(Name = "मिति")]
        public DateTime? DateTime { get; set; }

        public int? OfficeId { get; set; }

        public int? BranchId { get; set; }

        public int? ServiceId { get; set; }

        public int? LevelId { get; set; }

        public int? DesignationId { get; set; }

        public string nLogDate { get; set; }

        public int? Status { get; set; }

        [Display(Name = "विवरण ")]
        public string Remarks { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        public List<EmployeeAttendanceLogModel> EmployeeAttendanceLogModelList { get; set; }
    }
}