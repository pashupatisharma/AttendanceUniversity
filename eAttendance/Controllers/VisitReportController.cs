using eAttendance.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    public partial class ReportController : Controller
    {
        [HttpPost]
        public ActionResult VisitReport(EmployeeAttendanceList model)
        {


            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            if ((!string.IsNullOrEmpty(model._nFromDate) && !string.IsNullOrEmpty(model._nToDate)) && (model.OfficeId > 0))
            {
                string[] strArray = model._nFromDate.Split(new char[] { '-' });
                string[] strArray2 = model._nToDate.Split(new char[] { '-' });
                DateTime fromDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2])));
                DateTime toDate = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(int.Parse(strArray2[0]), int.Parse(strArray2[1]), int.Parse(strArray2[2])));
                source = ReportService.ReportService.GetEmployeeBy_FromDate_ToDate_OfficeIdList(fromDate, toDate, model.OfficeId, true);


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
            return PartialView("_Visit", model);
        }

        public ActionResult VisitReport()
        {
            EmployeeAttendanceList model = new EmployeeAttendanceList();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();

            return base.View(model);
        }
    }
}