using eAttendance.Models;
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
        //
        // GET: /LeaveReport/
        public ActionResult LeaveReport()
        {
            EmployeeAttendanceList list = new EmployeeAttendanceList();
            return base.View(list);
        }

        [HttpPost]
        public ActionResult LeaveReport(EmployeeAttendanceList model)
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
            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();

            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            if ((!string.IsNullOrEmpty(model._nFromDate) && !string.IsNullOrEmpty(model._nToDate)) && (model.OfficeId > 0))
            {

                //FiscalYearSetUp year = (from x in this.db.FiscalYearSetUp
                //                        where x.FiscalYearId == model.YearId
                //                        select x).FirstOrDefault<FiscalYearSetUp>();
                //DateTime fromDate = Convert.ToDateTime(year.FromDate);
                //DateTime toDate = Convert.ToDateTime(year.ToDate);
                string[] strArray = model._nFromDate.Split(new char[] { '-' });
                string[] strArray2 = model._nToDate.Split(new char[] { '-' });
                DateTime fromDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2])));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2])));

                
                var source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdList(fromDate, toDate, model.OfficeId,User, true);


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

                model.EmployeeAttendanceLists = source;

            }
            return PartialView("_LeaveReport", model);
        }








    }
}