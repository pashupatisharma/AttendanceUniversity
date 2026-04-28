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
        //
        public ActionResult DailyAbsentAttendanceReport(string nLogDate, int officeId = 0)
        {
            EmployeeAttendanceList model = new EmployeeAttendanceList();
            if (!string.IsNullOrEmpty(nLogDate))
            {
                model.nLogDate = nLogDate;
            }

            if (officeId > 0)
            {
                model.OfficeId = officeId;
            }
            DateTime date = DateTime.Now.Date;

            if (!string.IsNullOrWhiteSpace(model.nLogDate))
            {
                int yy = int.Parse(model.nLogDate.Split(new char[] { '-' })[0]);
                int mm = int.Parse(model.nLogDate.Split(new char[] { '-' })[1]);
                int dd = int.Parse(model.nLogDate.Split(new char[] { '-' })[2]);
                date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
            }
            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            if (model.OfficeId > 0)
            {

                source = ReportService.ReportService.GetEmpployeeListAccordingToOfficeAndPerDate(model.OfficeId, date, true);
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
                foreach (EmployeeAttendanceList models in source)
                {

                    var item = ReportService.ReportService.GetEmployeeAttandaneByOfficeWithInDateRange(models.EmployeeId, models.OfficeId, date, date).FirstOrDefault<EmployeeAttendanceList>();
                    if (item == null)
                    {
                        item = new EmployeeAttendanceList();
                    }
                    item.EmployeeName = models.EmployeeName;
                    item.EmployeeNameNp = models.EmployeeNameNp;
                    item.EmployeeNameAndCode = models.EmployeeNameAndCode;
                    var e =
                        db.EmployeeInfo.Where(x => x.EmployeeId == models.EmployeeId).FirstOrDefault();
                    item.EmployeeName = models.EmployeeName;
                    if (e != null)
                    {
                        item.EmployeeNameAndCodeNp = e.EmployeeNameNp + "[" + e.EmployeeNo + "]";
                    }
                    item.OfficeId = model.OfficeId;
                    item.BranchId = models.BranchId;
                    item.ServiceId = models.ServiceId;
                    item.LevelId = models.LevelId;
                    item.DesignationId = models.DesignationId;
                    list.Add(item);
                }

                list = (from x in list
                        where x.StatusType == 0
                        select x).ToList<EmployeeAttendanceList>();
            }

            model.EmployeeAttendanceLists = list;
            return base.View(model);
        }




        [HttpPost]
        public ActionResult DailyAbsentAttendanceReport(EmployeeAttendanceList model)
        {
            DateTime date = DateTime.Now.Date;
            int num = model.OfficeId;
            if (!string.IsNullOrWhiteSpace(model.nLogDate))
            {
                int yy = int.Parse(model.nLogDate.Split(new char[] { '-' })[0]);
                int mm = int.Parse(model.nLogDate.Split(new char[] { '-' })[1]);
                int dd = int.Parse(model.nLogDate.Split(new char[] { '-' })[2]);
                date = NepaliDateConverter.ConvertToEnglish(new NepaliDateConverter(yy, mm, dd));
            }
            List<EmployeeAttendanceList> list = new List<EmployeeAttendanceList>();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            if (num > 0)
            {

                source = ReportService.ReportService.GetEmpployeeListAccordingToOfficeAndPerDate(num, date, true);
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
                foreach (EmployeeAttendanceList models in source)
                {

                    var item = ReportService.ReportService.GetEmployeeAttandaneByOfficeWithInDateRange(models.EmployeeId, num, date, date).FirstOrDefault<EmployeeAttendanceList>();
                    if (item == null)
                    {
                        item = new EmployeeAttendanceList();
                    }
                    item.EmployeeName = models.EmployeeName;
                    item.EmployeeNameNp = models.EmployeeNameNp;
                    item.EmployeeNameAndCode = models.EmployeeNameAndCode;
                    var e =
                        db.EmployeeInfo.Where(x => x.EmployeeId == models.EmployeeId).FirstOrDefault();
                    item.EmployeeName = models.EmployeeName;
                    if (e != null)
                    {
                        item.EmployeeNameAndCodeNp = e.EmployeeNameNp + "[" + e.EmployeeNo + "]";
                    }
                    item.OfficeId = num;
                    item.BranchId = models.BranchId;
                    item.ServiceId = models.ServiceId;
                    item.LevelId = models.LevelId;
                    item.DesignationId = models.DesignationId;
                    list.Add(item);
                }
                list = (from x in list
                        where x.StatusType == 0
                        select x).ToList<EmployeeAttendanceList>();
            }

            model.EmployeeAttendanceLists = list;
            return base.PartialView("_DailyAbsentAttendance", model);
        }
    }
}