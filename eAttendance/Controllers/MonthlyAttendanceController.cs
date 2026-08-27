using eAttendance.ReportModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    public partial class ReportController : Controller
    {
        public ActionResult MonthlyAttendance()
        {
            EmployeeAttendanceList model = new EmployeeAttendanceList();
            return base.View(model);
        }
        [HttpPost]
        public ActionResult MonthlyAttendance(EmployeeAttendanceList model)
        {
            bool isFullAccess =
User.IsInRole("Admin") ||
User.IsInRole("SuperAdmin") ||
User.IsInRole("Administrator");
            if (!isFullAccess)
            {
                string userId = User.Identity.GetUserId();
                if (!string.IsNullOrEmpty(userId))
                {
                    int? officeId = (from emp in db.EmployeeInfo
                                     join office in db.EmployeeOfficeDetail
                                         on emp.EmployeeId equals office.EmployeeId
                                     where emp.UserId == userId
                                     select office.OfficeId)
                   .FirstOrDefault();
                    if (officeId != null)
                    {
                        model.OfficeId = Convert.ToInt32(officeId);
                    }
                }
            }

            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            if (((model.YearId > 0) && (model.MonthId > 0)) && (model.OfficeId > 0))
            {
                source = ReportService.ReportService.GetEmployeeBy_Year_Month_OfficeIdList(model.YearId, model.MonthId, model.OfficeId, User,true);

                int _branchId = model.BranchId;
                if (_branchId > 0)
                {

                    source = source.Where(x => x.BranchId == model.BranchId).ToList();
                }
                if (model.ServiceId > 0)
                {
                    source = source.Where(x => x.ServiceId == model.ServiceId).ToList();
                }
                if (model.LevelId > 0)
                {
                    source = source.Where(x => x.LevelId == model.LevelId).ToList();
                }

                if (model.DesignationId > 0)
                {
                    source = source.Where(x => x.DesignationId == model.DesignationId).ToList();
                }

                if (model.EmployeeId > 0)
                {
                    source = source.Where(x => x.EmployeeId == model.EmployeeId).ToList();
                }
                if(model.ShiftTypeId > 0)
                {
                    source = source.Where(x => x.ShiftTypeId == model.ShiftTypeId).ToList();
                }

                model.EmployeeAttendanceLists = source;
            }
            return PartialView("_MonthlyAttendance", model);
        }

    }
}