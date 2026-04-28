using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class AspNetUserModel
    {
        // Properties
        public List<AspNetUserModel> AspNetUserList { get; set; }

        public string Id { get; set; }

        [EmailAddress, Display(Name = "Email"), Required]
        public string Email { get; set; }

        [DataType(DataType.Text), Display(Name = "Confirm Email"), Compare("Email", ErrorMessage = "The Email and confirmation Email do not match.")]
        public string ConfirmEmail { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNo { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public string UserName { get; set; }
    }



}