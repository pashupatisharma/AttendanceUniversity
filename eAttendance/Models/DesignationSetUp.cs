using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{//Post
    public class DesignationSetUp : BaseModel
    {


        [Key()]
        public int DesignationId { get; set; }

        [Display(Name = "श्रेणी")]
        public int LevelId { get; set; }
        public virtual LevelSetUp LevelSetUp { get; set; }

        [Display(Name = "पदको नाम "), Required]
        public string DesignationName { get; set; }

        [Display(Name = "उपनाम")]
        public string Alias { get; set; }

        [Display(Name = "विवरण")]
        public string Description { get; set; }

        [Display(Name = "हेराउने क्रम")]
        public int DisplayOrder { get; set; }



        public int Status { get; set; }
    }
}