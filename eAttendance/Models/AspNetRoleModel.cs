using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
   public class AspNetRoleModel
{
    // Properties
    public string Id { get; set; }

    public string Name { get; set; }

    public int? AccessType { get; set; }

    public List<AspNetRoleModel> AspNetRoleList { get; set; }

    public bool IsCheck { get; set; }
}

 
 

}