using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;
using eAttendance.ReportModel;

namespace eAttendance.Controllers
{
    public class UtilityController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public JsonResult CheckUserName(string UserName)
        {
            ApplicationDbContext entities = new ApplicationDbContext();

            string name = entities.Database
                 .SqlQuery<String>(" select  UserName from AspNetUsers where UserName='" +
                                   UserName + "'").FirstOrDefault();
            bool exisist = false;
            if (!string.IsNullOrEmpty(name))
            {
                exisist = true;
                return Json(exisist);
            }
            return Json(exisist);
        }
        //
        // GET: /Utility/
        public SelectList GetDesignationByLevelId(string levelId = "", bool withAll = false, bool withSelect = false)
        {

            Func<DesignationSetUp, bool> predicate = null;
            List<DesignationSetUp> source = db.DesignationSetUp.OrderBy(m => m.DisplayOrder).ToList();

            if (!string.IsNullOrEmpty(levelId) && (int.Parse(levelId) > 0))
            {
                if (predicate == null)
                {
                    predicate = w => w.LevelId == int.Parse(levelId);
                }
                source = source.Where<DesignationSetUp>(predicate).ToList<DesignationSetUp>();
            }
            if (withAll)
            {
                DesignationSetUp item = new DesignationSetUp
                {
                    DesignationId = 0,
                    DesignationName = "सबै"
                };
                source.Insert(0, item);
            }
            else if (withSelect)
            {
                DesignationSetUp item = new DesignationSetUp
                {
                    DesignationId = 0,
                    DesignationName = "छान्नुहोस् ..."
                };
                source.Insert(0, item);
            }
            return new SelectList(source, "DesignationId", "DesignationName");
        }

        public ActionResult GetEmployeeBy(int OfficeId,
       int BranchId,
       int LevelId,
        int DesignationId,
        int ServiceId,
          bool withAll,
           bool withSelect)
        {

            IEnumerable<EmployeeAttendanceList> source = db.EmployeeOfficeDetail.Where(x => x.OfficeId == OfficeId).Select(m => new EmployeeAttendanceList()
            {
                OfficeId = (int)m.OfficeId,
                LevelId = (int)m.LevelId,
                EmployeeId = (int)m.EmployeeId,
                BranchId = (int)m.BranchId,
                ServiceId = (int)m.ServiceId,
                DesignationId = (int)m.DesignationId,

            });
            List<SelectListItem> list = new List<SelectListItem>();
            if (BranchId > 0)
            {

                source = source.Where(x => x.BranchId == BranchId);
            }
            if (ServiceId > 0)
            {
                source = source.Where(x => x.ServiceId == ServiceId);
            }
            if (LevelId > 0)
            {
                source = source.Where(x => x.LevelId == LevelId);
            }

            if (DesignationId > 0)
            {
                source = source.Where(x => x.DesignationId == DesignationId);
            }

            if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {

            }
            else if (User.IsInRole("Employee"))
            {
                var username = db.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault().Id;
                var Empid = EmployeeProvider.GetEmployeeIdByUserId(username);
                source = source.Where(x => x.EmployeeId == Empid);
            }

            foreach (var item in source.ToList())
            {
                var nameAndCode = db.EmployeeInfo.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault();
                string t = "[" + nameAndCode.EmployeeNo + "]" + nameAndCode.EmployeeNameNp;

                list.Add(new SelectListItem() { Value = item.EmployeeId.ToString(), Text = t });
            }

            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin") || User.IsInRole("Administrator"))
            {


                SelectListItem item = new SelectListItem();
                item.Value = "0";
                item.Text = "र्छान्नुहोस्";

                list.Insert(0, item);
            }

            return Json(list);
        }

    }
}