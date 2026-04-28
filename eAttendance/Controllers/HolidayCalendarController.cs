using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;
using PagedList;

namespace eAttendance.Controllers
{
    public class HolidayCalendarController : Controller
    {
        private CalendarManager _calendar = new CalendarManager();
       
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index(int selected_officeId = 0)
        {
            var dd = NepaliDateConverter.ConvertToNepali(DateTime.Now);
            WeekForMonth month = this._calendar.getCalender(dd.Month, dd.Year, selected_officeId);
            ViewBag.OfficeId = selected_officeId;
            return base.View(month);
        }






        // GET: /HolidayCalendar/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayCalender holidaycalender = await db.HolidayCalender.FindAsync(id);
            if (holidaycalender == null)
            {
                return HttpNotFound();
            }
            return View(holidaycalender);
        }

        // GET: /HolidayCalendar/Create
        public ActionResult Add()
        {
            HolidayCalender model = new HolidayCalender();
            return View(model);
        }



        [HttpPost]
        public ActionResult Add(HolidayCalender model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                if (model.OfficeId != 0)
                {
                    model.CreatedBy = userIdByUserName;
                    model.FromDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate)));
                    model.ToDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate)));
                    model.CreatedDate = DateTime.Now;
                    model.ModifiedDate = DateTime.Now;
                    model.Status = 1;
                    db.HolidayCalender.Add(model);
                    db.SaveChanges();

                }
                else
                {

                    List<OfficeSetUp> list = db.OfficeSetUp.Where(x => x.Status == 1).ToList();
                    foreach (var office in list)
                    {
                        model.CreatedBy = userIdByUserName;
                        model.FromDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate)));
                        model.ToDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate)));
                        model.CreatedDate = DateTime.Now;
                        model.ModifiedDate = DateTime.Now;
                        model.OfficeId = new int?(office.OfficeId);
                        model.Status = 1;
                        db.HolidayCalender.Add(model);
                        db.SaveChanges();
                    }
                }

              
                TempData.Add("Message", "Sucess");
            }
            catch
            {

                TempData.Add("Message", "Failed");
            }
            return base.RedirectToAction("Index", "HolidayCalendar");
        }




        // Methods
        public ActionResult _HolidayInformation(string officeId, string search, string sortOrder, int? page)
        {

            IQueryable<HolidayCalender> source = db.HolidayCalender.OrderBy(m => m.HolidayCalendarId);
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (!User.IsInRole("SuperAdmin"))
            {
                int? loginOfficeId = EmployeeProvider.GetOfficeIdByUserId(User.Identity.Name);
                if (loginOfficeId > 0)
                {
                    source = source.Where(x => x.OfficeId == loginOfficeId);
                }
            }

            ViewBag.Search = search;
            string str = sortOrder;
            if ((str != null) && (str == "name_desc"))
            {
                source = source.Where(x => x.HolidayTypeName.Contains(str));
            }


            if ((officeId != "0"))
            {
                int? offid = Convert.ToInt32(officeId);
                source = source.Where(x => x.OfficeId == offid);
            }

            int num = 10;
            int? nullable = page;
            int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            ViewBag.ViewType = 2;
            return this.PartialView("_HolidayInformation", PagedListExtensions.ToPagedList<HolidayCalender>(source, num2, num));
        }






        public ActionResult Edit(int? id)
        {

            HolidayCalender model = new HolidayCalender();
            if (id.HasValue)
            {

                model = db.HolidayCalender.First(x => x.HolidayCalendarId == id);
                model.NFromDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.FromDate), "yyyy-MM-DD");
                model.NToDate = NepaliDateConverter.ConvertToNepali(Convert.ToDateTime(model.ToDate), "yyyy-MM-DD");
            }
            return base.View(model);
        }




        [HttpPost]
        public ActionResult Edit(HolidayCalender model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                    model.ModifiedBy = userIdByUserName;
                    model.FromDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NFromDate)));
                    model.ToDate = (NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NToDate)));
                    model.ModifiedDate = DateTime.Now;
                    model.Status = 1;
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();


                    TempData.Add("Message", "Sucess");
                }
                catch
                {

                    TempData.Add("Message", "failed");
                }
                return base.RedirectToAction("Index", "HolidayCalendar");
            }
            return base.View();
        }


 
        public ActionResult SearchHolidayCalendarOfficeWise(string officeId)
        {
            int _officeId = int.Parse(officeId);
            IEnumerable<HolidayCalender> enumerable = this.db.HolidayCalender.Where<HolidayCalender>(delegate(HolidayCalender x)
            {
                int? nullable = x.OfficeId;
                int num = _officeId;
                return (nullable.GetValueOrDefault() == num) && nullable.HasValue;
            });
            return base.RedirectToAction("Index", new { selected_officeId = _officeId });
        }


        public ActionResult AsyncUpdateCalender(int month, int year, int officeId)
        {
            if (AjaxRequestExtensions.IsAjaxRequest(HttpContext.Request))
            {
                WeekForMonth month2 = this._calendar.getCalender(month, year, officeId);
                return base.Json(month2, 0);
            }
            return base.View();
        }




        public ActionResult AsyncTodayDateBS()
        {
            ActionResult actionResult;
            if (base.HttpContext.Request.IsAjaxRequest())
            {
                NepaliDateConverter model = NepaliDateConverter.ConvertToNepali(DateTime.Today.Date);
                actionResult = base.Json(model.ToString(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                actionResult = base.View();
            }
            return actionResult;
        }








        // GET: /HolidayCalendar/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HolidayCalender holidaycalender = await db.HolidayCalender.FindAsync(id);
            if (holidaycalender == null)
            {
                return HttpNotFound();
            }
            return View(holidaycalender);
        }

        // POST: /HolidayCalendar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(HolidayCalender model)
        {
            HolidayCalender holidaycalender = await db.HolidayCalender.FindAsync(model.HolidayCalendarId);
            db.HolidayCalender.Remove(holidaycalender);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
