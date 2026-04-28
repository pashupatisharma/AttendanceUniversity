using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{ //Shreeni
    public class LevelSetUp : BaseModel
    {
        [Key()]
   

        // Properties
        public int LevelId { get; set; }

        [Display(Name = "श्रेणीको नाम "), Required]
        public string LevelName { get; set; }

        [Display(Name = "छोटकरी नाम ")]
        public string Alias { get; set; }

        [Display(Name = "विवरण ")]
        public string Description { get; set; }

        [Display(Name = "हेराउने क्रम ")]
        public int DisplayOrder { get; set; }


        [Display(Name = "Status")]
        public int Status { get; set; }

        [NotMapped()]
        public List<LevelSetUp> SetupLevelList { get; set; }
    }
}