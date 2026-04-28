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
        // GET: /LateInEarlyOutAttendanceReport/
        public ActionResult LateInEarlyOutAttendanceReport()
        {
            EmployeeAttendanceList model = new EmployeeAttendanceList();
            model.EmployeeAttendanceLists = new List<EmployeeAttendanceList>();
            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            if ((model.nLogDate != null) && (model.OfficeId != 0))
            {
                DateTime logDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.nLogDate));


                source = ReportService.ReportService.GetEmpployeeListAccordingToOfficeAndPerDate(model.OfficeId, logDate, true);



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
            return base.View(model);
        }


        [HttpPost]
        public ActionResult LateInEarlyOutAttendanceReport(EmployeeAttendanceList model)
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
            List<EmployeeAttendanceList> source = new List<EmployeeAttendanceList>();
            if ((model.nLogDate != null) && (model.OfficeId != 0))
            {
              // DateTime logDate = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.nLogDate));


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

              //  model.EmployeeAttendanceLists = source;



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
            return base.PartialView("_LateInEarlyOutAttendance", model);
        }
    }
}