using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }

        [Display(Name = "कर्मचारीको नाम")]
        public int? EmployeeId { get; set; }

        public string DocumentName { get; set; }

        [Display(Name = "कागजातको नाम")]
        public string DocumentFullName { get; set; }

        [Display(Name = "कागजात समुह")]
        public int CategoryId { get; set; }

        [Display(Name = "कागजातको प्रकार")]
        public string ContentType { get; set; }

      

        public bool IsActive { get; set; }

        [NotMapped()]
        public List<Document> DocumentList { get; set; }

    }
}