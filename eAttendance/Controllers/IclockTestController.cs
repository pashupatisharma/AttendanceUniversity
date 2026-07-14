using eAttendance.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{

    
    public class IClockControllerTest : Controller
    {
        private void WriteLog(string message)
        {
            string logPath = @"D:\ZKLogs\SudhurPaximUniversity_log.txt";

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(logPath));

                using (var writer = new StreamWriter(logPath, true))
                {
                    writer.WriteLine("====================================================");
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine(message);
                    writer.WriteLine("====================================================");
                }
            }
            catch
            {

            }
        }

        private string ReadBody()
        {
            Request.InputStream.Position = 0;

            using (var reader = new StreamReader(Request.InputStream))
            {
                return reader.ReadToEnd();
            }
        }

        //-------------------------------------------------------
        // Device Initialization
        //-------------------------------------------------------

        [HttpGet]
        public ActionResult CData(string SN)
        {
            WriteLog("GET /iclock/cdata");
            WriteLog("SN : " + SN);

            return Content("OK", "text/plain");
        }

        //-------------------------------------------------------
        // Device Registration
        //-------------------------------------------------------

        [HttpGet]
        public ActionResult Registry(string SN)
        {
            WriteLog("Registry : " + SN);

            // 10 digit registration code
            return Content("1234567890", "text/plain");
        }

        //-------------------------------------------------------
        // Push Configuration
        //-------------------------------------------------------

        [HttpGet]
        public ActionResult Push(string SN)
        {
            WriteLog("Push Request : " + SN);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("ServerVersion=3.0.1");
            sb.AppendLine("ServerName=ADMS");
            sb.AppendLine("PushVersion=3.0.1");
            sb.AppendLine("ErrorDelay=30");
            sb.AppendLine("RequestDelay=2");
            sb.AppendLine("Realtime=1");
            sb.AppendLine("TimeoutSec=10");
            sb.AppendLine("TransInterval=1");
            sb.AppendLine("TransTables=User Transaction");

            return Content(sb.ToString(), "text/plain");
        }


        [HttpPost]
        public ActionResult CData()
        {
            try
            {
                string sn = Request["SN"];

                string rawBody = ReadBody();

                WriteLog("POST /iclock/cdata");
                WriteLog("SN : " + sn);
                WriteLog(rawBody);

                if (string.IsNullOrWhiteSpace(sn))
                    return Content("OK");

                if (string.IsNullOrWhiteSpace(rawBody))
                    return Content("OK");

                using (var db = new ApplicationDbContext())
                {
                    //var device = db.OfficeDeviceSetUp
                    //               .FirstOrDefault(x => x.DeviceNo == sn);

                    //if (device == null)
                    //{
                    //    WriteLog("Device Not Registered : " + sn);
                    //    return Content("OK");
                    //}

                    string table = "";

                    foreach (string line in rawBody.Split('\n'))
                    {
                        if (line.StartsWith("table=", StringComparison.OrdinalIgnoreCase))
                        {
                            table = line.Substring(6).Trim();
                            break;
                        }
                    }

                    WriteLog("Table = " + table);

                    string[] records = rawBody.Split(
                        new[] { '\r', '\n' },
                        StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in records)
                    {
                        try
                        {
                            if (!line.Contains("PIN="))
                                continue;

                            Dictionary<string, string> data =
                                new Dictionary<string, string>();

                            foreach (string item in line.Split('\t'))
                            {
                                if (!item.Contains("="))
                                    continue;

                                string[] kv = item.Split('=');

                                if (kv.Length == 2)
                                    data[kv[0]] = kv[1];
                            }

                            string enrollNo =
                                data.ContainsKey("PIN")
                                ? data["PIN"]
                                : "";

                            if (String.IsNullOrWhiteSpace(enrollNo))
                                continue;

                            DateTime punchTime = DateTime.Now;

                            if (data.ContainsKey("DateTime"))
                            {
                                DateTime.TryParseExact(
                                    data["DateTime"],
                                    "yyyy-MM-dd HH:mm:ss",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out punchTime);
                            }

                            string verifyMode =
                                data.ContainsKey("Verify")
                                ? data["Verify"]
                                : "0";

                            string status =
                                data.ContainsKey("Status")
                                ? data["Status"]
                                : "0";

                            var emp =
                                (from e in db.EmployeeInfo
                                 join o in db.EmployeeOfficeDetail
                                 on e.EmployeeId equals o.EmployeeId
                                 where e.EmployeeNo == enrollNo
                              
                                 && e.Status != 2
                                 select new EmployeeInfoReport
                                 {
                                     EmployeeId = e.EmployeeId,
                                     OfficeId = o.OfficeId
                                 }).FirstOrDefault();

                            if (emp == null)
                            {
                                WriteLog("Employee Not Found : " + enrollNo);
                                continue;
                            }

                            bool exists =
                                db.AttendanceLog.Any(x =>
                                    x.EmployeeId == emp.EmployeeId &&
                                    x.DateTime == punchTime);

                            if (exists)
                            {
                                WriteLog("Duplicate Punch : " + enrollNo);
                                continue;
                            }

                            AttendanceLog log = new AttendanceLog();

                            log.OfficeId = (int)emp.OfficeId;

                            // Replace DeviceId with your actual PK if different
                            log.OfficeDeviceId = 1;

                            log.EmployeeId = emp.EmployeeId;

                            log.EnrollNumber = enrollNo;

                            log.DateTime = punchTime;

                            log.VerifyMode = verifyMode;

                            log.InOutMode = status;

                            log.Status = 1;

                            log.IpAddress = Request.UserHostAddress;

                            db.AttendanceLog.Add(log);

                            WriteLog("Attendance Saved : " +
                                     enrollNo +
                                     " " +
                                     punchTime.ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        catch (Exception ex)
                        {
                            WriteLog("Record Error : " + ex);
                        }
                    }

                    db.SaveChanges();
                }

                return Content("OK", "text/plain");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());

                return Content("OK", "text/plain");
            }
        }


        [HttpGet]
        public ActionResult GetRequest(string SN)
        {
            try
            {
                WriteLog("==================================");
                WriteLog("GETREQUEST");
                WriteLog("SN : " + SN);

                if (string.IsNullOrWhiteSpace(SN))
                    return Content("OK", "text/plain");

                using (var db = new ApplicationDbContext())
                {
                    // Verify device
                    var device = db.OfficeDeviceSetUp
                                   .FirstOrDefault(x => x.DeviceNo == SN);

                    if (device == null)
                    {
                        WriteLog("Device  is  not set");
                        return Content("OK", "text/plain");
                    }


                    WriteLog("Heartbeat OK");

                    return Content("OK", "text/plain");
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());

                return Content("OK", "text/plain");
            }
        }


        [HttpPost]
        public ActionResult DeviceCmd(string SN)
        {
            try
            {
                string body = "";

                using (var reader = new StreamReader(Request.InputStream))
                {
                    body = reader.ReadToEnd();
                }

                WriteLog("==================================");
                WriteLog("DEVICE COMMAND RESULT");
                WriteLog("SN : " + SN);
                WriteLog(body);

                return Content("OK", "text/plain");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());

                return Content("OK", "text/plain");
            }
        }

        [HttpGet]
        public ActionResult Ping(string SN)
        {
            WriteLog("PING : " + SN);

            return Content("OK", "text/plain");
        }

        public ActionResult RebootDevice(string sn)
        {
            // Later this will be stored in DB command queue

            string command = "C:223:CONTROL DEVICE 03000000";

            WriteLog("Reboot Command");
            WriteLog(command);

            return Content(command);
        }
    }
}