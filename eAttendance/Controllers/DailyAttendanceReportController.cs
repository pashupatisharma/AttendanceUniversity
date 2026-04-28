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
        public ActionResult DailyAttendanceReport()
        {

            EmployeeAttendanceList model = new EmployeeAttendanceList();
            DateTime date = DateTime.Now.Date;

            if (!string.IsNullOrWhiteSpace(model.nLogDate))
            {
                int yy = int.Parse(model.nLogDate.Split(new char[] { '-' })[0]);
                int mm = int.Parse(model.nLogDate.Split(new char[] { '-' })[1]);
                int dd = int.Parse(model.nLogDate.Split(new char[] { '-' })[2]);
                date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
            }
            else
            {
                model.nLogDate = NepaliDateConverter.ConvertToNepali(date).ToString();
            }

            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();

            IEnumerable<EmployeeAttendanceList> source;
            if (model.OfficeId > 0)
            {

                source = ReportService.ReportService.GetEmpployeeListAccordingToOfficeAndPerDate(model.OfficeId, date, true);


                int _branchId = model.BranchId;
                if (_branchId > 0)
                {

                    source = source.Where(x => x.BranchId == model.BranchId);
                }
                if (model.ServiceId > 0)
                {
                    source = source.Where(x => x.ServiceId == model.ServiceId);
                }
                if (model.LevelId > 0)
                {
                    source = source.Where(x => x.LevelId == model.LevelId);
                }

                if (model.DesignationId > 0)
                {
                    source = source.Where(x => x.DesignationId == model.DesignationId);
                }

                if (model.EmployeeId > 0)
                {
                    source = source.Where(x => x.EmployeeId == model.EmployeeId);
                }

                var list = source.ToList();

                foreach (var models in list)
                {

                    var item = ReportService.ReportService.GetEmployeeAttandaneByOfficeWithInDateRange(models.EmployeeId, model.OfficeId, date, date).FirstOrDefault<EmployeeAttendanceList>();

                    if (item == null)
                    {
                        item = new EmployeeAttendanceList();
                    }



                    item.EmployeeNameNp = models.EmployeeNameNp;
                    item.EmployeeNameAndCode = models.EmployeeNameAndCode;
                    var e =
                        db.EmployeeInfo.Where(x => x.EmployeeId == models.EmployeeId).FirstOrDefault();
                    item.EmployeeName = models.EmployeeName;
                    if (e != null)
                    {
                        item.EmployeeNameAndCodeNp = e.EmployeeNameNp + "[" + e.EmployeeNo + "]";
                    }
                    item.EmployeeId = models.EmployeeId;
                    item.OfficeId = model.OfficeId;
                    item.BranchId = models.BranchId;
                    item.ServiceId = models.ServiceId;
                    item.LevelId = models.LevelId;
                    item.DesignationId = models.DesignationId;
                    model.EmployeeAttendanceLists.Add(item);
                }


            }
            return base.View(model);
        }


        [HttpPost]
        public ActionResult DailyAttendanceReport(EmployeeAttendanceList model)
        {
            DateTime date = DateTime.Now.Date;

            if (!string.IsNullOrWhiteSpace(model.nLogDate))
            {
                int yy = int.Parse(model.nLogDate.Split(new char[] { '-' })[0]);
                int mm = int.Parse(model.nLogDate.Split(new char[] { '-' })[1]);
                int dd = int.Parse(model.nLogDate.Split(new char[] { '-' })[2]);
                date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
            }
            else
            {
                model.nLogDate = NepaliDateConverter.ConvertToNepali(date).ToString();
            }

            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();

            IEnumerable<EmployeeAttendanceList> source;
            if (model.OfficeId > 0)
            {

                source = ReportService.ReportService.GetEmpployeeListAccordingToOfficeAndPerDate(model.OfficeId, date, true);


                int _branchId = model.BranchId;
                if (_branchId > 0)
                {

                    source = source.Where(x => x.BranchId == model.BranchId);
                }
                if (model.ServiceId > 0)
                {
                    source = source.Where(x => x.ServiceId == model.ServiceId);
                }
                if (model.LevelId > 0)
                {
                    source = source.Where(x => x.LevelId == model.LevelId);
                }

                if (model.DesignationId > 0)
                {
                    source = source.Where(x => x.DesignationId == model.DesignationId);
                }

                if (model.EmployeeId > 0)
                {
                    source = source.Where(x => x.EmployeeId == model.EmployeeId);
                }

                var list = source.ToList();

                foreach (var models in list)
                {

                    var item = ReportService.ReportService.GetEmployeeAttandaneByOfficeWithInDateRange(models.EmployeeId, model.OfficeId, date, date).FirstOrDefault<EmployeeAttendanceList>();

                    if (item == null)
                    {
                        item = new EmployeeAttendanceList();
                    }



                    item.EmployeeNameNp = models.EmployeeNameNp;
                    item.EmployeeNameAndCode = models.EmployeeNameAndCode;
                    var e =
                        db.EmployeeInfo.Where(x => x.EmployeeId == models.EmployeeId).FirstOrDefault();
                    item.EmployeeName = models.EmployeeName;
                    if (e != null)
                    {
                        item.EmployeeNameAndCodeNp = e.EmployeeNameNp + "[" + e.EmployeeNo + "]";
                    }
                    item.EmployeeId = models.EmployeeId;
                    item.OfficeId = model.OfficeId;
                    item.BranchId = models.BranchId;
                    item.ServiceId = models.ServiceId;
                    item.LevelId = models.LevelId;
                    item.DesignationId = models.DesignationId;
                    model.EmployeeAttendanceLists.Add(item);
                }

                if (model.StatusType == 2)
                {
                    model.EmployeeAttendanceLists = model.EmployeeAttendanceLists.Where(x => x.StatusType == 1).ToList();
                    return base.PartialView("_DailyAttendance", model);

                }

                if (model.StatusType == 3)
                {
                    model.EmployeeAttendanceLists = model.EmployeeAttendanceLists.Where(x => x.StatusType == 0).ToList();
                    return base.PartialView("_DailyAbsentAttendance", model);

                }
                else
                {
                    return base.PartialView("_DailyAttendance", model);
                }
            }
            return base.PartialView("_DailyAttendance", model);
        }

    }
}