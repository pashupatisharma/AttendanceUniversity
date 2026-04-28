using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using eAttendance.Models;
using Newtonsoft.Json.Linq;


namespace eAttendance.Controllers
{

  
    public class AttendanceController : Controller
    {
        // =========================================================
        // HIKVISION EVENT PUSH (POST)
        // =========================================================
        [HttpPost]
        [AllowAnonymous]
        public ActionResult HikvisionPush()
        {
            try
            {
                // Read RAW POST body
                string rawJson;
                using (var reader = new StreamReader(Request.InputStream, Encoding.UTF8))
                {
                    rawJson = reader.ReadToEnd();
                }

                // Log raw data (VERY IMPORTANT)
                LogToFile("hikvision_raw.txt", rawJson);

                JObject json = JObject.Parse(rawJson);
                var infoList = json["AcsEvent"]?["InfoList"];

                if (infoList == null || !infoList.Any())
                    return Json(new { status = "no_data" });

                using (var db = new ApplicationDbContext())
                {
                    foreach (var e in infoList)
                    {
                        string empNo = e["employeeNo"]?.ToString();
                        string timeStr = e["time"]?.ToString();
                        string verifyMode = e["verifyMode"]?.ToString();
                        string deviceIp = Request.UserHostAddress;

                        if (string.IsNullOrEmpty(empNo) || string.IsNullOrEmpty(timeStr))
                            continue;

                        DateTime punchTime;
                        if (!DateTime.TryParse(
                                timeStr,
                                null,
                                System.Globalization.DateTimeStyles.RoundtripKind,
                                out punchTime))
                            continue;

                        var emp = db.EmployeeInfo
                            .FirstOrDefault(x => x.EmployeeNo == empNo);

                        if (emp == null) continue;

                        var office = db.EmployeeOfficeDetail
                            .FirstOrDefault(x => x.EmployeeId == emp.EmployeeId);

                        if (office == null) continue;

                        bool exists = db.AttendanceLog.Any(x =>
                            x.EmployeeId == emp.EmployeeId &&
                            x.DateTime == punchTime);

                        if (exists) continue;

                        db.AttendanceLog.Add(new AttendanceLog
                        {
                            EmployeeId = emp.EmployeeId,
                            EnrollNumber = empNo,
                            OfficeId = (int)office.OfficeId,
                            OfficeDeviceId = 1, // CHANGE if needed
                            IpAddress = deviceIp,
                            DateTime = punchTime,
                            InOutMode = "0",
                            VerifyMode = verifyMode,
                            Status = 1
                        });
                    }

                    db.SaveChanges();
                }

                return Json(new { status = "ok" });
            }
            catch (Exception ex)
            {
                LogToFile("hikvision_error.txt", ex.ToString());
                return Json(new { status = "error" });
            }
        }

        // =========================================================
        // SIMPLE TEST METHOD (OPTIONAL)
        // =========================================================
        [HttpGet]
        public ActionResult Test()
        {
            return Content("Hikvision Push API is working");
        }

        // =========================================================
        // LOGGING HELPER
        // =========================================================
        private void LogToFile(string fileName, string content)
        {
            string path = Server.MapPath("~/App_Data/" + fileName);
            System.IO.File.AppendAllText(
                path,
                DateTime.Now + Environment.NewLine + content + Environment.NewLine + "------------------" + Environment.NewLine
            );
        }
    }
}
