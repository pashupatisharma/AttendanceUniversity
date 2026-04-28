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
using System.Web.Helpers;
using System.IO;
using System.Net.Mime;

namespace eAttendance.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrator,Admin")]
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Document/
        public ActionResult IndexPage()
        {
            //Document model = new Document();
            //model.DocumentList = new List<Document>();
            //model.DocumentList = db.Documents.ToList();
            return View("Index");
        }

        // GET: /Document/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // GET: /Document/Create
        public ActionResult Add(int employeeId)
        {
            Document model = new Document();
            model.EmployeeId = employeeId;
            return View(model);
        }

        // POST: /Document/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Add(Document model, HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    string str3;
                    string[] strArray = file.FileName.Split(new char[] { '.' });
                    model.DocumentName = strArray[0];
                    model.DocumentFullName = file.FileName;
                    string[] strArray2 = file.ContentType.Split(new char[] { '/' });
                    string str = strArray2[0];

                    string employeeNameByEmployeeId = db.EmployeeInfo.Where(x => x.EmployeeId == model.EmployeeId).FirstOrDefault().EmployeeNameNp;
                    if (str == "image")
                    {
                        WebImage image = new WebImage(file.InputStream);
                        str3 = Server.MapPath(string.Concat(new object[] { "~/Document/Uploads/", model.EmployeeId, '/', model.DocumentFullName }));
                        if (!System.IO.File.Exists(str3))
                        {
                            Directory.CreateDirectory(base.Server.MapPath("Uploads/" + model.EmployeeId));
                            str3 = Server.MapPath(string.Concat(new object[] { "~/Document/Uploads/", model.EmployeeId, '/', model.DocumentFullName }));
                            model.ContentType = "Images";
                        }
                        else
                        {
                            TempData.Add("HasMessage", true);
                            TempData.Add("MessageType", "success");
                            TempData.Add("MessageHeader", "Dear User");
                            TempData.Add("Message", "Duplicate name! please select another name.");
                        }
                        image.Save(str3, null, true);
                    }
                    else
                    {


                        str3 = Server.MapPath(string.Concat(new object[] { "~/Document/Uploads/", model.EmployeeId, '/', model.DocumentFullName }));
                        if (!System.IO.File.Exists(str3))
                        {
                            Directory.CreateDirectory(Server.MapPath("Uploads/" + model.EmployeeId));
                            str3 = Server.MapPath(string.Concat(new object[] { "~/Document/Uploads/", model.EmployeeId, '/', model.DocumentFullName }));
                        }
                        else
                        {

                            TempData.Add("Message", "Duplicate name! please select another name.");
                        }
                        file.SaveAs(str3);
                    }
                }
                db.Documents.Add(model);
                db.SaveChanges();

                TempData.Add("Message", "Sucess");
            }
            catch
            {

                TempData.Add("Message", "Failed");
            }
            return RedirectToAction("IndexPage");
        }



        public FileResult Download(string DocumentFullName)
        {
            byte[] buffer = System.IO.File.ReadAllBytes(@"c:\Downloads" + DocumentFullName);
            string str = "DocumentFullName";
            return this.File(buffer, "application/octet-stream", str);
        }

        public ActionResult DownloadFile(string DocumentFullName,int EmployeeId)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Document" + "\\" + "Uploads\\" +EmployeeId+ "\\" + DocumentFullName;
            byte[] buffer = System.IO.File.ReadAllBytes(path);
            string mimeMapping = MimeMapping.GetMimeMapping(path);
            ContentDisposition disposition = new ContentDisposition
            {
                FileName = DocumentFullName,
                Inline = true
            };
            Response.AppendHeader("Content-Disposition", disposition.ToString());
            return base.File(buffer, mimeMapping);
        }

        public ActionResult Downloads()
        {
            FileInfo[] files = new DirectoryInfo(Server.MapPath("~/Document/Uploads/")).GetFiles("*.*");
            List<string> list = new List<string>();
            foreach (FileInfo info2 in files)
            {
                list.Add(info2.Name);
            }
            return base.View(list);
        }


        [HttpGet]
        public PartialViewResult EmployeeDocument(int employeeId = 0)
        {

            List<Document> list = db.Documents.Where(x => x.EmployeeId == employeeId).ToList();
            ViewBag.employeeId = employeeId;
            return this.PartialView("_EmployeeDocument", list);
        }





        // GET: /Document/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: /Document/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DocumentId,EmployeeId,DocumentName,DocumentFullName,CategoryId,ContentType,Extension,Size,IsActive")] Document document)
        {
            if (ModelState.IsValid)
            {
                db.Entry(document).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("IndexPage");
            }
            return View(document);
        }

        // GET: /Document/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Document document = await db.Documents.FindAsync(id);
            if (document == null)
            {
                return HttpNotFound();
            }
            return View(document);
        }

        // POST: /Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Document document = await db.Documents.FindAsync(id);
            db.Documents.Remove(document);
            await db.SaveChangesAsync();
            return RedirectToAction("IndexPage");
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
