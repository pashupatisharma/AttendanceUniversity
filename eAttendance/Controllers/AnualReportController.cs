using eAttendance.ReportModel;
using eAttendance.ViewModel;
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
        //
        // GET: /AnualReport/
        public ActionResult AnualReport()
        {
            MonthlyAttendanceModel model = new MonthlyAttendanceModel
            {
                FilteredEmployeeList = null,
                EmployeeLeaveSummaryList = null
            };
            return base.View(model);
        }

        [HttpPost]
        public ActionResult AnualReport(MonthlyAttendanceModel model)
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
                    var employeeInfo = (from emp in db.EmployeeInfo
                                        join office in db.EmployeeOfficeDetail
                                            on emp.EmployeeId equals office.EmployeeId
                                        where emp.UserId == userId
                                        select new
                                        {
                                            EmployeeId = emp.EmployeeId,
                                            OfficeId = office.OfficeId
                                        })
                                       .FirstOrDefault();

                    if (employeeInfo != null)
                    {
                        model.EmployeeId = employeeInfo.EmployeeId;
                        model.OfficeId = Convert.ToInt32(employeeInfo.OfficeId);
                    }
                }
            }

            model.FilteredEmployeeList = new List<MonthlyAttendanceModel>();
            model.EmployeeLeaveSummaryList = new List<EmployeeLeaveSummaryList>();

            if ((((model.YearId != 0) && (model.OfficeId != 0)) && (model.EmployeeId != 0)))
            {
                int yy = model.YearId;
                int num2 = model.OfficeId;

                DateTime[] month = NepaliDateConverter.GetMonth(yy, 1);
                DateTime[] timeArray2 = NepaliDateConverter.GetMonth(yy, 12);
                DateTime fromDate = month[0];
                DateTime toDate = timeArray2[1];

                List<MonthlyAttendanceModel> source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdListAnnual(fromDate, toDate, num2,User, true);
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
                model.FilteredEmployeeList = source;
                model.EmployeeLeaveSummaryList = ReportService.ReportService.GetEmployeeAttendanceLeaveSummary(model.EmployeeId, fromDate, toDate);
            }
            return base.View("AnualReport", model);
        }

    }
}