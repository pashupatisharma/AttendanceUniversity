using System.Collections.Generic;
using eAttendance.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace eAttendance.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    //mig
    internal sealed class Configuration : DbMigrationsConfiguration<eAttendance.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(eAttendance.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any(r => r.Name == "SuperAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "SuperAdmin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Administrator" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Employee"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Employee" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Approver"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Approver" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Recommendatory"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Recommendatory" };

                manager.Create(role);
            }

            //IList<WeekDaySetUp> WeekDayList = new List<WeekDaySetUp>();

            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 1, WeekDayName = "आईतबार", WeekDay = 1, OfficeId = 1, WorkType = 1 });
            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 2, WeekDayName = "सोमबार", WeekDay = 2, OfficeId = 1, WorkType = 1 });
            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 3, WeekDayName = "म‍गलबार", WeekDay = 3, OfficeId = 1, WorkType = 1 });
            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 4, WeekDayName = "बुधबार", WeekDay = 4, OfficeId = 1, WorkType = 1 });

            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 5, WeekDayName = "बिहिबार", WeekDay = 5, OfficeId = 1, WorkType = 1 });
            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 6, WeekDayName = "शुक्रबार", WeekDay = 6, OfficeId = 1, WorkType = 1 });
            //WeekDayList.Add(new WeekDaySetUp() { WeekDayId = 7, WeekDayName = "शनिबार", WeekDay = 7, OfficeId = 1, WorkType = 3 });




            //foreach (WeekDaySetUp std in WeekDayList)
            //    context.WeekDaySetUp.AddOrUpdate(std);


        }
    }
}
