using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class FiscalYearSetUp:BaseModel
    {
           [Key()]
        public int FiscalYearId { get; set; }

        [Required, Display(Name = " वर्ष ")]
        public string FiscalYearName { get; set; }

        [Display(Name = " वर्षको कोड ")]
        public string FiscalYearCode { get; set; }

        [Display(Name = "विवरण ")]
        public string Description { get; set; }

        [Display(Name = "मिति देखि ")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "मिति देखि "), NotMapped]
        public string NFromDate { get; set; }

        [Display(Name = "मिति सम्म  ")]
        public DateTime? ToDate { get; set; }

        [NotMapped, Display(Name = "मिति सम्म  ")]
        public string NToDate { get; set; }

        [Display(Name = "हेराउने क्रम  ")]
        public int DisplayOrder { get; set; }

        [Display(Name = "सक्रिय छ")]
        public bool IsActive { get; set; }

        [NotMapped()]
        public List<FiscalYearSetUp> SetupFiscalYearList { get; set; }

        public int Status { get; set; }
    }
       
}