using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class TransferModel
    {
        // Properties
        [Key()]
        public int TransferId { get; set; }

        [Display(Name = "लागु हुने मिति ")]
        public DateTime TransferDateTime { get; set; }

        [Display(Name = "लागु हुने मिति ")]
        public string NTransferDateTime { get; set; }

        [Display(Name = "सन्केत नम्बर")]
        public int EmployeeId { get; set; }

        [Display(Name = "कर्मचारी no")]
        public int? EmployeeNo { get; set; }

        public string EmployeeName { get; set; }

        [StringLength(100, MinimumLength = 3), Display(Name = "कर्मचारीको नाम (English)")]
        public string EmployeeNameNp { get; set; }

        [Display(Name = "कार्यालय देखि ")]
        public int FromOfficeId { get; set; }

        [Display(Name = "मुख्यशाखाको नाम")]
        public int FromMainBranchId { get; set; }

        [Display(Name = " शाखा")]
        public int FromBranchId { get; set; }

        [Display(Name = " सेवा ")]
        public int FromServiceId { get; set; }

        [Display(Name = "श्रेणी")]
        public int FromLevelId { get; set; }

        [Display(Name = " पद")]
        public int FromDesignationId { get; set; }

        [Display(Name = " कार्यालयमा")]
        public string ToOfficeName { get; set; }

        [Display(Name = "मुख्यशाखा")]
        public string ToMainBranchName { get; set; }

        [Display(Name = "शाखा")]
        public string ToBranchName { get; set; }

        [Display(Name = " सेवा")]
        public string ToServiceName { get; set; }

        [Display(Name = " श्रेणी")]
        public string ToLevelName { get; set; }

        [Display(Name = " पद")]
        public string ToDesignationName { get; set; }

        [Display(Name = " सरुवा प्रकार")]
        public int? TransferType { get; set; }

        [Display(Name = " टिप्पणी")]
        public string Remarks { get; set; }

        [Display(Name = " कार्यालयमा")]
        public int ToOfficeId { get; set; }

        [Display(Name = "मुख्यशाखा")]
        public int ToMainBranchId { get; set; }

        [Display(Name = "शाखा")]
        public int ToBranchId { get; set; }

        [Display(Name = " सेवा")]
        public int ToServiceId { get; set; }

        [Display(Name = " श्रेणी")]
        public int ToLevelId { get; set; }

        [Display(Name = " पद")]
        public int ToDesignationId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? Status { get; set; }

        [NotMapped()]
        public List<TransferModel> EmployeeTransferList { get; set; }

        [NotMapped()]
        public PagedList.IPagedList<TransferModel> OnePageOfBranches { get; set; }
    }
}