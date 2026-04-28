using eAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance
{
    public static class EmployeeProvider
    {
        public static int GetEmployeeIdByUserId(string userid)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                return entities.EmployeeInfo.Where(x => x.UserId == userid).FirstOrDefault().EmployeeId;


            }
        }

        public static string GetUserIdByUserName(string usernname)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                return entities.Users.Where(x => x.UserName == usernname).FirstOrDefault().Id;


            }
        }

        public static int? GetEmployeeIdByUserName(string userName)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userId = context.Users.Where(x => x.UserName == userName).FirstOrDefault().Id;

            var EmployeeId = context.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault();
            if (EmployeeId != null)
            {
                return EmployeeId.EmployeeId;
            }
            return null;
        }

        public static int? GetOfficeIdByUserId(string username)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userId = context.Users.Where(x => x.UserName == username).FirstOrDefault().Id;

            var EmployeeId = context.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault();
            if (EmployeeId != null)
            {
                var officeId = context.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId.EmployeeId).FirstOrDefault();
                if (officeId != null)
                {
                    return officeId.OfficeId;
                }

            }
            return null;
        }

        public static string GetOfficeNameByUserId(string username)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var userId = context.Users.Where(x => x.UserName == username).FirstOrDefault().Id;

            var EmployeeId = context.EmployeeInfo.Where(x => x.UserId == userId).FirstOrDefault();
            if (EmployeeId != null)
            {
                var officeId = context.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId.EmployeeId).FirstOrDefault()
                    .OfficeId;

                return context.OfficeSetUp.Where(x => x.OfficeId == officeId).FirstOrDefault().OfficeName;
            }

            return "";
        }

        public static int InsertEmployeeAndOfficeDetail(EmployeeInfo model, EmployeeOfficeDetail modelEmployeeOfficeInfo, EmployeeShiftTime modelEmployeeShiftTime)
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {
                modelEmployeeOfficeInfo.EmployeeId = model.EmployeeId;
                modelEmployeeOfficeInfo.CreatedDate = DateTime.Now;
                DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(modelEmployeeOfficeInfo.NEffectiveDate));
                modelEmployeeOfficeInfo.EffectiveDate = ((time.Year >= 0x779) && (time.Year <= 0x7fa)) ? time : new DateTime(0x76c, 1, 1);

             
                modelEmployeeOfficeInfo.Status = 1;
                modelEmployeeOfficeInfo.CreatedDate = DateTime.Now;
                modelEmployeeOfficeInfo.ModifiedDate = DateTime.Now;
                entities.EmployeeOfficeDetail.Add(modelEmployeeOfficeInfo);

                modelEmployeeShiftTime = new EmployeeShiftTime();
                modelEmployeeShiftTime.EmployeeId = model.EmployeeId;
                modelEmployeeShiftTime.EffectiveDate = DateTime.Now;
                modelEmployeeShiftTime.Stauts = 1;
                modelEmployeeShiftTime.CreatedDate = DateTime.Now;
                modelEmployeeShiftTime.ModifiedDate = DateTime.Now;
                entities.EmployeeShiftTime.Add(modelEmployeeShiftTime);
                entities.SaveChanges();

            }
            return model.EmployeeId;
        }



    }
}