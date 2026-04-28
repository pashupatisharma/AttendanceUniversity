using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eAttendance.Models;

namespace eAttendance.Controllers
{
    // [Authorize(Roles = "SuperAdmin,Administrator")]
    public class SetupShiftTimeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /SetupShiftTime/
        public ActionResult Index()
        {

            SetupShiftTime model = new SetupShiftTime();
            model.SetupShiftTimeList = new List<SetupShiftTime>();
            model.SetupShiftTimeList = db.SetupShiftTime.ToList();
            return View(model);
        }

        // GET: /SetupShiftTime/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SetupShiftTime setupshifttime = db.SetupShiftTime.Find(id);
            if (setupshifttime == null)
            {
                return HttpNotFound();
            }
            return View(setupshifttime);
        }




        public ActionResult Add(int SetupShiftId = 0, int OfficeId = 0)
        {
            List<SetupShiftTime> list = new List<SetupShiftTime>();
            if (SetupShiftId == 0)
            {
                SetupShiftId = db.Shift.FirstOrDefault().ShiftId;
            }

            List<SetupShiftTime> source = db.SetupShiftTime.Where(x => x.ShiftId == SetupShiftId && x.OfficeId == OfficeId).Where(x => x.Status == 1).ToList();
            Func<SetupShiftTime, bool> predicate = null;

            for (int i = 1; i <= 7; i++)
            {
                string str = "";
                string str2 = "";
                SetupShiftTime item = new SetupShiftTime();
                if (predicate == null)
                {
                    predicate = x => x.WeekDay == i;
                }

                var d = source.Where(x => x.WeekDay == i).FirstOrDefault();
                if (d != null)
                {
                    item.ShiftTimeId = d.ShiftTimeId;
                    item.IsWeekOff = d.IsWeekOff;
                    item.ShiftStartTime = d.ShiftStartTime;
                    item.ShiftEndTime = d.ShiftEndTime;
                    item.ShiftLateInAfter = d.ShiftLateInAfter;
                    item.ShiftEarlyOutBefore = d.ShiftEarlyOutBefore;

                }



                item.ShiftId = SetupShiftId;
                item.WeekDay = i;
                switch (i)
                {
                    case 1:
                        str = "Sunday";
                        str2 = "आइतबार";
                        break;

                    case 2:
                        str = "Monday";
                        str2 = "सोमवार";
                        break;

                    case 3:
                        str = "Tuesday";
                        str2 = "मंगलवार";
                        break;

                    case 4:
                        str = "Wednesday";
                        str2 = "बुधवार";
                        break;

                    case 5:
                        str = "Thrusday";
                        str2 = "बिहीबार";
                        break;

                    case 6:
                        str = "Friday";
                        str2 = "शुक्रवार";
                        break;

                    case 7:
                        str = "Saturday";
                        str2 = "शनिबार";
                        break;

                    default:
                        str = "";
                        str2 = "";
                        break;
                }
                item.WeekDayNameEn = str;
                item.WeekDayNameNp = str2;

                list.Add(item);
            }
            ViewBag.SelectedShiftId = SetupShiftId.ToString();
            return base.View(list);
        }









        [HttpPost]
        public ActionResult Add(int SetupShiftId, int? OfficeId, List<SetupShiftTime> list)
        {
            try
            {
                if (SetupShiftId <= 0)
                {
                    return base.RedirectToAction("Add", "SetupShiftTime");
                }
                var sname = Utility.GetShiftNameById(SetupShiftId);
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);

                if (OfficeId == null)
                {
                    List<OfficeSetUp> listoffice = db.OfficeSetUp.Where(x => x.Status == 1).ToList();
                    foreach (var office in listoffice)
                    {
                        foreach (SetupShiftTime item in list)
                        {
                            Func<SetupShiftTime, bool> predicate = null;
                            if (item.ShiftTimeId <= 0)
                            {
                                SetupShiftTime model = new SetupShiftTime
                                {
                                    ShiftId = SetupShiftId,
                                    OfficeId = new int?(office.OfficeId),
                                    ShiftName = sname,
                                    WeekDay = item.WeekDay,
                                    IsWeekOff = item.IsWeekOff,
                                    ShiftStartTime = (item.ShiftStartTime != null) ? item.ShiftStartTime : "00:00",
                                    ShiftEndTime = (item.ShiftEndTime != null) ? item.ShiftEndTime : "00:00",
                                    ShiftHours = (item.ShiftHours != null) ? item.ShiftHours : "00:00",
                                    ShiftLateInAfter = (item.ShiftLateInAfter != null) ? item.ShiftLateInAfter : "00:00",
                                    ShiftEarlyOutBefore = (item.ShiftEarlyOutBefore != null) ? item.ShiftEarlyOutBefore : "00:00",
                                    BreakStartTime = (item.BreakStartTime != null) ? item.BreakStartTime : "00-00",
                                    BreakEndTime = (item.BreakEndTime != null) ? item.BreakEndTime : "00:00",
                                    BreakDuration = (item.BreakDuration != null) ? item.BreakDuration : "00:00",
                                    BreakLateInAfter = (item.BreakLateInAfter != null) ? item.BreakLateInAfter : "00:00",
                                    BreakEarlyOutBefore = (item.BreakEarlyOutBefore != null) ? item.BreakEarlyOutBefore : "00:00",
                                    OverTimeBefore = (item.OverTimeBefore != null) ? item.OverTimeBefore : "00:00",
                                    OverTimeAfter = (item.OverTimeAfter != null) ? item.OverTimeAfter : "00:00",
                                    CreatedBy = userIdByUserName,
                                    CreatedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    Status = 1
                                };
                                db.SetupShiftTime.Add(model);
                                db.SaveChanges();
                            }
                            else
                            {

                                SetupShiftTime model = db.SetupShiftTime.Where(x => x.ShiftTimeId == item.ShiftTimeId).FirstOrDefault();
                                model.ShiftTimeId = item.ShiftTimeId;
                                model.OfficeId = new int?(office.OfficeId);
                                model.ShiftId = item.ShiftId;
                                model.WeekDay = item.WeekDay;
                                model.IsWeekOff = item.IsWeekOff;
                                model.ShiftStartTime = (item.ShiftStartTime != null) ? item.ShiftStartTime : "00:00";
                                model.ShiftEndTime = (item.ShiftEndTime != null) ? item.ShiftEndTime : "00:00";
                                model.ShiftLateInAfter = (item.ShiftLateInAfter != null) ? item.ShiftLateInAfter : "00:00";
                                model.ShiftEarlyOutBefore = (item.ShiftEarlyOutBefore != null) ? item.ShiftEarlyOutBefore : "00:00";
                                model.ModifiedBy = userIdByUserName;
                                model.ModifiedDate = DateTime.Now;
                                model.Status = 1;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    foreach (SetupShiftTime item in list)
                    {
                        Func<SetupShiftTime, bool> predicate = null;
                        if (item.ShiftTimeId <= 0)
                        {
                            SetupShiftTime model = new SetupShiftTime
                            {
                                ShiftId = SetupShiftId,
                                OfficeId = OfficeId,
                                ShiftName = sname,
                                WeekDay = item.WeekDay,
                                IsWeekOff = item.IsWeekOff,
                                ShiftStartTime = (item.ShiftStartTime != null) ? item.ShiftStartTime : "00:00",
                                ShiftEndTime = (item.ShiftEndTime != null) ? item.ShiftEndTime : "00:00",
                                ShiftHours = (item.ShiftHours != null) ? item.ShiftHours : "00:00",
                                ShiftLateInAfter = (item.ShiftLateInAfter != null) ? item.ShiftLateInAfter : "00:00",
                                ShiftEarlyOutBefore = (item.ShiftEarlyOutBefore != null) ? item.ShiftEarlyOutBefore : "00:00",
                                BreakStartTime = (item.BreakStartTime != null) ? item.BreakStartTime : "00-00",
                                BreakEndTime = (item.BreakEndTime != null) ? item.BreakEndTime : "00:00",
                                BreakDuration = (item.BreakDuration != null) ? item.BreakDuration : "00:00",
                                BreakLateInAfter = (item.BreakLateInAfter != null) ? item.BreakLateInAfter : "00:00",
                                BreakEarlyOutBefore = (item.BreakEarlyOutBefore != null) ? item.BreakEarlyOutBefore : "00:00",
                                OverTimeBefore = (item.OverTimeBefore != null) ? item.OverTimeBefore : "00:00",
                                OverTimeAfter = (item.OverTimeAfter != null) ? item.OverTimeAfter : "00:00",
                                CreatedBy = userIdByUserName,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                Status = 1
                            };
                            db.SetupShiftTime.Add(model);
                            db.SaveChanges();
                        }
                        else
                        {

                            SetupShiftTime model = db.SetupShiftTime.Where(x => x.ShiftTimeId == item.ShiftTimeId).FirstOrDefault();
                            model.ShiftTimeId = item.ShiftTimeId;

                            model.ShiftId = item.ShiftId;
                            model.WeekDay = item.WeekDay;
                            model.IsWeekOff = item.IsWeekOff;
                            model.ShiftStartTime = (item.ShiftStartTime != null) ? item.ShiftStartTime : "00:00";
                            model.ShiftEndTime = (item.ShiftEndTime != null) ? item.ShiftEndTime : "00:00";
                            model.ShiftLateInAfter = (item.ShiftLateInAfter != null) ? item.ShiftLateInAfter : "00:00";
                            model.ShiftEarlyOutBefore = (item.ShiftEarlyOutBefore != null) ? item.ShiftEarlyOutBefore : "00:00";
                            model.ModifiedBy = userIdByUserName;
                            model.ModifiedDate = DateTime.Now;
                            model.Status = 1;
                            db.SaveChanges();
                        }
                    }
                }



                TempData.Add("Message", "Sucess");
            }
            catch
            {

                TempData.Add("Message", "Failed");
            }
            return base.RedirectToAction("Add", "SetupShiftTime", new { shiftId = SetupShiftId });
        }

        [HttpPost]
        public JsonResult CheckDuplicate(string ShiftName, int? Id = 0, int? OfficeId = 0)
        {
            IEnumerable<SetupShiftTime> enumerable;
            Func<SetupShiftTime, bool> predicate = null;
            Func<SetupShiftTime, bool> func2 = null;
            Func<SetupShiftTime, bool> func3 = null;
            Func<SetupShiftTime, bool> func4 = null;
            JsonResult result = new JsonResult();
            bool flag = true;
            if (Id == 0)
            {
                if (predicate == null)
                {
                    predicate = p => p.ShiftName == ShiftName.Trim();
                }
                enumerable = db.SetupShiftTime.ToList().Where<SetupShiftTime>(predicate);
                if (OfficeId > 0)
                {
                    if (func2 == null)
                    {
                        func2 = delegate(SetupShiftTime p)
                        {
                            int? nullable;
                            int? nullable2;
                            return (p.ShiftName == ShiftName.Trim()) && (((nullable = p.OfficeId).GetValueOrDefault() == (nullable2 = OfficeId).GetValueOrDefault()) && (nullable.HasValue == nullable2.HasValue));
                        };
                    }
                    enumerable = db.SetupShiftTime.ToList().Where<SetupShiftTime>(func2);
                }
                if (enumerable.Count<SetupShiftTime>() > 0)
                {
                    flag = false;
                }
            }
            else
            {
                if (func3 == null)
                {
                    func3 = delegate(SetupShiftTime p)
                    {
                        int? nullable;
                        return (p.ShiftName == ShiftName.Trim()) && ((p.ShiftId != (nullable = Id).GetValueOrDefault()) || !nullable.HasValue);
                    };
                }
                enumerable = db.SetupShiftTime.ToList().Where<SetupShiftTime>(func3);
                if (OfficeId > 0)
                {

                }
                if (enumerable.Count<SetupShiftTime>() > 0)
                {
                    flag = false;
                }
            }


            return result;
        }

        [HttpPost]
        public ActionResult Delete(SetupShiftTime model)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                model.ModifiedBy = userIdByUserName;
                model.ModifiedDate = DateTime.Now;
                model.Status = 2;
                db.SaveChanges();

                TempData.Add("Message", "Sucess");
            }
            catch
            {

                TempData.Add("Message", "Failed");
            }
            return base.RedirectToAction("Index", "SetupShiftTime");
        }


        // POST: /SetupShiftTime/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SetupShiftTime setupshifttime)
        {
            if (ModelState.IsValid)
            {
                setupshifttime.ModifiedBy = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                setupshifttime.ModifiedDate = DateTime.Now;
                db.Entry(setupshifttime).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfficeId = new SelectList(db.OfficeSetUp, "OfficeId", "OfficeName", setupshifttime.OfficeId);
            ViewBag.ShiftId = new SelectList(db.Shift, "ShiftId", "ShiftName", setupshifttime.ShiftId);
            return View(setupshifttime);
        }

        // GET: /SetupShiftTime/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SetupShiftTime setupshifttime = db.SetupShiftTime.Find(id);
            if (setupshifttime == null)
            {
                return HttpNotFound();
            }
            return View(setupshifttime);
        }

        // POST: /SetupShiftTime/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SetupShiftTime setupshifttime = db.SetupShiftTime.Find(id);
            db.SetupShiftTime.Remove(setupshifttime);
            db.SaveChanges();
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
