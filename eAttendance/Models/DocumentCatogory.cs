using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class DocumentCatogory
    {

        // Properties
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int Status { get; set; }
        [NotMapped()]
        public List<DocumentCatogory> DocumentCatogoryList { get; set; }



    }
}