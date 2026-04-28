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
    [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
    public class TransferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Transfer/
        public ViewResult Index(string sortOrder, string officeid, string currentFilter, string searchString, int? page)
        {
            TransferModel model = new TransferModel();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            IEnumerable<TransferModel> source = db.TransferModel.Where(x => x.ToOfficeId != 0);
            if (!string.IsNullOrEmpty(searchString))
            {

                source = source.Where(x => x.ToOfficeName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    source = from s in source
                             orderby s.ToOfficeName descending
                             select s;
                    break;

                case "Date":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                case "date_desc":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                default:
                    source = from s in source
                             orderby s.ToOfficeName
                             select s;
                    break;
            }
            int num = 10;
            int? nullable = page;
            int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            ViewBag.OnePageOfBranches = PagedListExtensions.ToPagedList<TransferModel>(source, num2, num);
            return base.View("Index");
        }




        public ViewResult IndexExternalTransfer(string sortOrder, string officeid, string currentFilter, string searchString, int? page)
        {
            TransferModel model = new TransferModel();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = (sortOrder == "Date") ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            IEnumerable<TransferModel> source = db.TransferModel.Where(x => x.ToOfficeId == 0);

            if (!string.IsNullOrEmpty(searchString))
            {

                source = source.Where(x => x.ToOfficeName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    source = from s in source
                             orderby s.ToOfficeName descending
                             select s;
                    break;

                case "Date":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                case "date_desc":
                    source = from s in source
                             orderby s.CreatedDate descending
                             select s;
                    break;

                default:
                    source = from s in source
                             orderby s.ToOfficeName
                             select s;
                    break;
            }
            int num = 10;
            int? nullable = page;
            int num2 = nullable.HasValue ? nullable.GetValueOrDefault() : 1;
            ViewBag.OnePageOfBranches = PagedListExtensions.ToPagedList<TransferModel>(source, num2, num);
            return base.View("IndexExternalTransfer");
        }


        // GET: /Transfer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferModel transfermodel = await db.TransferModel.FindAsync(id);
            if (transfermodel == null)
            {
                return HttpNotFound();
            }
            return View(transfermodel);
        }

        // GET: /Transfer/Create
        public ActionResult Add()
        {
            return View();
        }

        public ActionResult ExternalTransfer()
        {
            TransferModel model = new TransferModel();
            return base.View();
        }

        public ActionResult InternalTransfer()
        {
            TransferModel model = new TransferModel();
            return base.View(model);
        }
        [HttpPost]
        public ActionResult InternalTransfer(int EmployeeId, TransferModel model, EmployeeInfo objInfo, EmployeeOfficeDetail obj)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NTransferDateTime));
                if (time > DateTime.Now)
                {

                    return base.RedirectToAction("Index");
                }
                model.TransferDateTime = time;
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = new DateTime?(DateTime.Now);
                model.ModifiedDate = new DateTime?(DateTime.Now);
                model.Status = 1;
                model.EmployeeNo = model.EmployeeNo;
                db.TransferModel.Add(model);
                db.SaveChanges();

                if (objInfo.EmployeeId != 0)
                {

                    EmployeeInfo info = db.EmployeeInfo.Where(x => x.EmployeeId == objInfo.EmployeeId).FirstOrDefault();
                    if (info == null)
                    {

                        return base.RedirectToAction("Index");
                    }

                    info.ModifiedBy = userIdByUserName;

                    info.ModifiedDate = DateTime.Now;

                    info.Status = 4;

                    db.Entry(info).State = EntityState.Modified;
                    db.SaveChanges();

                    EmployeeOfficeDetail model2 = new EmployeeOfficeDetail
                    {
                        EmployeeId = objInfo.EmployeeId,
                        EffectiveDate = time,
                        OfficeId = new int?(model.ToOfficeId),
                        BranchId = new int?(model.ToBranchId),
                        LevelId = new int?(model.ToLevelId),
                        DesignationId = new int?(model.ToDesignationId),
                        ServiceId = new int?(model.ToServiceId),
                        Remarks = model.Remarks,
                        CreatedBy = userIdByUserName,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = 1
                    };
                    db.Entry(model2).State = EntityState.Modified;
                    db.SaveChanges();


                    EmployeeShiftTime time2 = db.EmployeeShiftTime.Where(x => x.EmployeeId == objInfo.EmployeeId).FirstOrDefault();
                    if (time2 != null)
                    {
                        time2.Stauts = 4;
                        db.Entry(time2).State = EntityState.Modified;
                        db.SaveChanges();
                        SetupShiftTime time3 = db.SetupShiftTime.Where(x => x.ShiftId == time2.ShiftId).FirstOrDefault();
                        if (time3 != null)
                        {
                            EmployeeShiftTime model3 = new EmployeeShiftTime
                            {
                                EmployeeId = obj.EmployeeId,
                                EffectiveDate = time,
                                ShiftId = time2.ShiftId,
                                Remarks = model.Remarks,
                                CreatedBy = userIdByUserName,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                                Stauts = 1
                            };
                            db.Entry(model3).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }

            }
            catch
            {

            }
            return base.RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult ExternalTransfer(int EmployeeId, TransferModel model, EmployeeInfo obj)
        {
            try
            {
                string userIdByUserName = EmployeeProvider.GetUserIdByUserName(User.Identity.Name);
                DateTime time = NepaliDateConverter.ConvertToEnglish(NepaliDateConverter.Format(model.NTransferDateTime));
                if (time > DateTime.Now)
                {

                    return base.RedirectToAction("Index");
                }

                model.TransferDateTime = time;
                model.CreatedBy = userIdByUserName;
                model.CreatedDate = new DateTime?(DateTime.Now);
                model.ModifiedDate = new DateTime?(DateTime.Now);
                model.Status = 1;
                model.EmployeeNo = model.EmployeeNo;
                db.TransferModel.Add(model);
                db.SaveChanges();

                if (obj.EmployeeId != 0)
                {

                    EmployeeInfo info = db.EmployeeInfo.Where(x => x.EmployeeId == obj.EmployeeId).FirstOrDefault();
                    if (info != null)
                    {
                        info.Status = 4;
                        info.ModifiedBy = userIdByUserName;
                        info.ModifiedDate = DateTime.Now;

                        db.Entry(info).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    EmployeeOfficeDetail info2 = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == obj.EmployeeId).FirstOrDefault();
                    if (info2 != null)
                    {
                        info2.Status = 4;
                        info2.ModifiedBy = userIdByUserName;
                        info2.ModifiedDate = DateTime.Now;
                        db.Entry(info2).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    EmployeeShiftTime time2 = db.EmployeeShiftTime.Where(x => x.EmployeeId == obj.EmployeeId).FirstOrDefault();
                    if (time2 != null)
                    {
                        time2.Stauts = 4;

                        db.Entry(time2).State = EntityState.Modified;

                        db.SaveChanges();
                    }
                }

            }
            catch
            {

            }
            return base.RedirectToAction("IndexExternalTransfer");
        }



        // POST: /Transfer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(TransferModel transfermodel)
        {
            if (ModelState.IsValid)
            {
                db.TransferModel.Add(transfermodel);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(transfermodel);
        }

        // GET: /Transfer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferModel transfermodel = await db.TransferModel.FindAsync(id);
            if (transfermodel == null)
            {
                return HttpNotFound();
            }
            return View(transfermodel);
        }

        // POST: /Transfer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TransferId,TransferDateTime,NTransferDateTime,EmployeeId,EmployeeNo,EmployeeName,EmployeeNameNp,FromOfficeId,FromMainBranchId,FromBranchId,FromServiceId,FromLevelId,FromDesignationId,ToOfficeName,ToMainBranchName,ToBranchName,ToServiceName,ToLevelName,ToDesignationName,TransferType,Remarks,ToOfficeId,ToMainBranchId,ToBranchId,ToServiceId,ToLevelId,ToDesignationId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Status")] TransferModel transfermodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transfermodel).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(transfermodel);
        }

        // GET: /Transfer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TransferModel transfermodel = await db.TransferModel.FindAsync(id);
            if (transfermodel == null)
            {
                return HttpNotFound();
            }
            return View(transfermodel);
        }

        // POST: /Transfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TransferModel transfermodel = await db.TransferModel.FindAsync(id);
            db.TransferModel.Remove(transfermodel);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult LoadDefaultValue(int EmployeeId)
        {
            List<TransferModel> list2 = new List<TransferModel>();
            TransferModel item = new TransferModel
            {
                FromOfficeId = Utility.GetOfficeIdByEmployeeId(EmployeeId),
                FromBranchId = Utility.GetBranchIdByEmployeeId(EmployeeId),
                FromServiceId = Utility.GetServiceIdByEmployeeId(EmployeeId),
                FromLevelId = Utility.GetLevelIdNameByEmployeeId(EmployeeId),
                FromDesignationId = Utility.GetDesignationIdByEmployeeId(EmployeeId),
                EmployeeId = EmployeeId
            };
            list2.Add(item);
            List<TransferModel> list = list2;
            return base.Json(list, 0);



        }

        public JsonResult loadSelectedEmployeeOfficeInfo(int EmployeeId)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            EmployeeOfficeDetail empoff = new EmployeeOfficeDetail();
            empoff = db.EmployeeOfficeDetail.Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
            return Json(empoff, JsonRequestBehavior.AllowGet);

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
