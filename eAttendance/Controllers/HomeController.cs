using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;
using eAttendance.ReportModel;
using eAttendance.ViewModel;

namespace eAttendance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();




        public ActionResult GetTodayAttendanceCount()
        {
            using (ApplicationDbContext entities = new ApplicationDbContext())
            {

                int? officeIdByUserName = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
                var date = DateTime.Now.Date;
                var listall = new List<AttendanceCountModel>();
                if(officeIdByUserName==null)
                {
                    return Json(listall);

                }
                string s = "SpTodayAttendanceCount" + " " + "'" + date + "'" + "," + officeIdByUserName;
                ((IObjectContextAdapter)entities).ObjectContext.CommandTimeout = 180;
                var list = entities.Database.SqlQuery<AttendanceCountModel>(s).ToList();
                foreach (var item in list)
                {
                    AttendanceCountModel model = new AttendanceCountModel();
                    model.Total = item.Total;
                    if (item.Present == null)
                    {
                        model.Present = 0;
                    }
                    else
                    {
                        model.Present = item.Present;
                    }
                    model.Absent = model.Total - Convert.ToInt32(item.Present) - item.OnVisit - item.OnLeave;
                    model.OnVisit = item.OnVisit;
                    model.OnLeave = item.OnLeave;
                    listall.Add(model);

                }


                return Json(listall);

            }

        }

        public ActionResult Index(string nlogDate = "", int officeId = 0)
        {
            List<LeaveApplication> all;
            List<EmployeeOfficeDetail> allEmployeeOfficeInfo;
            ViewBag.Title = "Dashboard";
            DashboardViewModel model = new DashboardViewModel();
            int? officeIdByUserName = officeId;
            if (officeId == 0)
            {
                officeIdByUserName = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
            }

            DateTime today = DateTime.Today;
            if (!string.IsNullOrWhiteSpace(nlogDate))
            {
                string[] strArray = nlogDate.Split(new char[] { '-' });
                NepaliDateConverter nepalidate = new NepaliDateConverter(Convert.ToInt32(strArray[0]),
                    Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                today = NepaliDateConverter.ConvertToEnglish(nepalidate);
            }

            model.DailyAttendanceList = new List<DailyAttendanceForDashboardModel>();
            model.TodayAttendanceCount = new ChartProvider().GetTodayAttendanceCount(officeIdByUserName, today);
            ViewBag.OfficeId = officeIdByUserName;
            model.EmployeeOnLeave = this.GetEmployeeLeaveList(officeIdByUserName);
            model.EmployeeOnVisit = this.GetEmployeeVisitList(officeIdByUserName);
            if (User.IsInRole("Approver") || User.IsInRole("Recommendatory"))
            {
                if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator")|| User.IsInRole("Approver") || User.IsInRole("Recommendatory"))
                {
                    model.LeaveApplicationList = dbContext.LeaveApplication.Select(lp => new LeaveApplicationModel()
                        {
                            LeaveApplicationId = lp.LeaveApplicationId,
                            EmployeeId = lp.EmployeeId,
                            LeaveTypeId = lp.LeaveTypeId,
                            ApplicationDate = lp.ApplicationDate,
                            FromDate = lp.FromDate,

                            ToDate = lp.ToDate,

                            TotalDays = lp.TotalDays,
                            Message = lp.Message,
                            Recomender = lp.Recomender,
                            RecomenderStatus = lp.RecomenderStatus,
                            RecomendedDate = lp.RecomendedDate,
                            ApprovedBy = lp.ApprovedBy,
                            ApprovedStatus = lp.ApprovedStatus,
                            ApprovedDate = lp.ApprovedDate,
                            CreatedBy = lp.CreatedBy,

                            ModifiedBy = lp.ModifiedBy,
                            ModifiedDate = lp.ModifiedDate,
                            Status = lp.Status
                        })
                        .ToList();
                    model.VisitApplicationList = dbContext.VisitApplication.Select(va => new VisitApplicationModel()
                        {
                            VisitApplicationId = va.VisitApplicationId,
                            EmployeeId = va.EmployeeId,
                            ApplicationDate = va.ApplicationDate,
                            FromDate = va.FromDate,

                            ToDate = va.ToDate,

                            TotalDays = va.TotalDays,
                            Message = va.Message,
                            Recomender = va.Recomender,
                            RecomenderStatus = va.RecomenderStatus,
                            RecomendedDate = va.RecomendedDate,
                            ApprovedBy = va.ApprovedBy,
                            ApprovedStatus = va.ApprovedStatus,
                            ApprovedDate = va.ApprovedDate,
                            CreatedBy = va.CreatedBy,
                            CreatedDate = va.CreatedDate,
                            ModifiedBy = va.ModifiedBy,
                            ModifiedDate = va.ModifiedDate,
                            Status = va.Status

                        })
                        .ToList();
                }
                else if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    all = dbContext.LeaveApplication.ToList();
                    allEmployeeOfficeInfo = dbContext.EmployeeOfficeDetail.ToList();
                    model.LeaveApplicationList = (from lp in all
                                                  join eoi in allEmployeeOfficeInfo on lp.EmployeeId equals eoi.EmployeeId into eoi

                                                  select new LeaveApplicationModel
                                                  {
                                                      LeaveApplicationId = lp.LeaveApplicationId,
                                                      EmployeeId = lp.EmployeeId,
                                                      LeaveTypeId = lp.LeaveTypeId,
                                                      ApplicationDate = lp.ApplicationDate,
                                                      FromDate = lp.FromDate,

                                                      ToDate = lp.ToDate,

                                                      TotalDays = lp.TotalDays,
                                                      Message = lp.Message,
                                                      Recomender = lp.Recomender,
                                                      RecomenderStatus = lp.RecomenderStatus,
                                                      RecomendedDate = lp.RecomendedDate,
                                                      ApprovedBy = lp.ApprovedBy,
                                                      ApprovedStatus = lp.ApprovedStatus,
                                                      ApprovedDate = lp.ApprovedDate,
                                                      CreatedBy = lp.CreatedBy,

                                                      ModifiedBy = lp.ModifiedBy,
                                                      ModifiedDate = lp.ModifiedDate,
                                                      Status = lp.Status
                                                  }).ToList<LeaveApplicationModel>();
                    List<VisitApplication> list3 = dbContext.VisitApplication.ToList();
                    model.VisitApplicationList = (from va in list3
                                                  join eoi in allEmployeeOfficeInfo on va.EmployeeId equals eoi.EmployeeId into eoi

                                                  select new VisitApplicationModel
                                                  {
                                                      VisitApplicationId = va.VisitApplicationId,
                                                      EmployeeId = va.EmployeeId,
                                                      ApplicationDate = va.ApplicationDate,
                                                      FromDate = va.FromDate,

                                                      ToDate = va.ToDate,

                                                      TotalDays = va.TotalDays,
                                                      Message = va.Message,
                                                      Recomender = va.Recomender,
                                                      RecomenderStatus = va.RecomenderStatus,
                                                      RecomendedDate = va.RecomendedDate,
                                                      ApprovedBy = va.ApprovedBy,
                                                      ApprovedStatus = va.ApprovedStatus,
                                                      ApprovedDate = va.ApprovedDate,
                                                      CreatedBy = va.CreatedBy,
                                                      CreatedDate = va.CreatedDate,
                                                      ModifiedBy = va.ModifiedBy,
                                                      ModifiedDate = va.ModifiedDate,
                                                      Status = va.Status
                                                  }).ToList<VisitApplicationModel>();
                }
            }

            return base.View(model);
        }





        private List<LeaveApplication> GetEmployeeLeaveList(int? OfficeId)
        {

            ApplicationDbContext atn = new ApplicationDbContext();

            if (base.User.IsInRole("SuperAdmin") || base.User.IsInRole("Administrator"))
            {
                return (from el in atn.LeaveApplication
                        where (((DateTime.Today.Date >= el.FromDate) && (DateTime.Today.Date <= el.ToDate)) && (el.Status == 1)) && (el.ApprovedStatus == 2)
                        select el).ToList<LeaveApplication>();
            }



            return (from va in atn.LeaveApplication
                    join ei in atn.EmployeeOfficeDetail on va.EmployeeId equals ei.EmployeeId
                    where ((((DateTime.Today.Date >= va.FromDate) && (DateTime.Today.Date <= va.ToDate)) && (va.Status == 1)) && (va.ApprovedStatus == 2))
                          && (ei.OfficeId == OfficeId) && (ei.Status == 1)
                    select va).ToList<LeaveApplication>();


        }


        private List<VisitApplication> GetEmployeeVisitList(int? OfficeId)
        {
            ApplicationDbContext atn = new ApplicationDbContext();
            List<VisitApplication> list = new List<VisitApplication>();
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Administrator"))
            {
                return (from va in atn.VisitApplication
                        where (((DateTime.Today.Date > va.FromDate) && (DateTime.Today.Date <= va.ToDate)) && (va.Status == 1)) && (va.ApprovedStatus == 2)
                        select va).ToList<VisitApplication>();
            }
            return (from va in atn.VisitApplication
                    join ei in atn.EmployeeOfficeDetail on va.EmployeeId equals ei.EmployeeId
                    where ((((DateTime.Today.Date > va.FromDate) && (DateTime.Today.Date <= va.ToDate)) && (va.Status == 1)) && (va.ApprovedStatus == 2))
                            && (ei.OfficeId == OfficeId) && (ei.Status == 1)
                    select va).ToList<VisitApplication>();
        }



        public PartialViewResult EmployeeAttendanceTable(int? officeId = 0, string nlogDate = "")
        {
            DateTime date = DateTime.Today.Date;
            if (!string.IsNullOrWhiteSpace(nlogDate))
            {
                string[] strArray = nlogDate.Split(new char[] { '-' });
                NepaliDateConverter nepalidate = new NepaliDateConverter(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                date = NepaliDateConverter.ConvertToEnglish(nepalidate);
            }
            if (officeId == 0)
            {
                officeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
            }

            IEnumerable<EmployeeCountDashBoardModl> employeeAttendance = GetEmployeeAttendance(officeId, date);
            return this.PartialView("_EmployeeAttendanceTable", employeeAttendance);
        }


        public IEnumerable<EmployeeCountDashBoardModl> GetEmployeeAttendance(int? officeId, DateTime date)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            string s = "SpDailyAttendanceForDashboard" + " " + "'" + date + "'" + "," + officeId;
            ((IObjectContextAdapter)dbContext).ObjectContext.CommandTimeout = 180;

            var list = dbContext.Database.SqlQuery<EmployeeCountDashBoardModl>(s).ToList();
            return list;
        }







































        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AdministratorDashboard()
        {
            return View();
        }



        [HttpGet]
        public PartialViewResult OfficeAttendanceTable(string officename, string ndate, string status)
        {
            DateTime date = DateTime.Today.Date;
            if (!string.IsNullOrWhiteSpace(ndate))
            {
                string[] strArray = ndate.Split(new char[] { '-' });
                NepaliDateConverter nepalidate = new NepaliDateConverter(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
                date = NepaliDateConverter.ConvertToEnglish(nepalidate);
            }


            ApplicationDbContext dbContext = new ApplicationDbContext();
            string s = "SpAdministratorDashboard" + " " + "'" + date + "'";
            ((IObjectContextAdapter)dbContext).ObjectContext.CommandTimeout = 20;



            List<EmployeeCountDashBoardModl> officeAttendance =
                dbContext.Database.SqlQuery<EmployeeCountDashBoardModl>(s).ToList();
            if (!string.IsNullOrWhiteSpace(officename))
            {
                officeAttendance = officeAttendance.Where(x => x.OfficeName.Contains(officename)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (int.Parse(status) == 1)
                {

                    officeAttendance = officeAttendance.Where(x => x.Status==1).ToList();
                    
                }
                else if (int.Parse(status) == 2)
                {
                    officeAttendance = officeAttendance.Where(x => x.Status == 2).ToList();
                }
            }
            return this.PartialView("_OfficeAttendanceTable", officeAttendance);
        }








    }
}