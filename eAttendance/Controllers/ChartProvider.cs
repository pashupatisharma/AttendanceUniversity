using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using eAttendance.Models;
using eAttendance.ViewModel;

namespace eAttendance.Controllers
{
    public class ChartProvider
    {
        internal AttendanceCountModel GetTodayAttendanceCount(int? officeIdByUserName, DateTime today)
        {

            if(officeIdByUserName ==null)
            {
                AttendanceCountModel obj=new AttendanceCountModel();
                return obj;
            }
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                string s = "SpTodayAttendanceCount" + " " + "'" + today + "'" + "," + officeIdByUserName;
                ((IObjectContextAdapter)entities).ObjectContext.CommandTimeout = 180;
                var count = entities.Database.SqlQuery<AttendanceCountModel>(s).FirstOrDefault();
                return count;

            }

        }
    }
}
