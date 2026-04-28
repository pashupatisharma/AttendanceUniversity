using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    using global::eAttendance.Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using System.Text;
    using System.Globalization;

    namespace eAttendance.Controllers
    {

        public class IClockController : Controller
        {
            private void WriteLog(string message)
            {
                string logPath = @"D:\ZKLogs\Universal_ZK_ADMS_Log.txt";
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(logPath));
                    using (var writer = new StreamWriter(logPath, true))
                    {
                        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} >> {message}");
                    }
                }
                catch { /* Skip logging error */ }
            }


            [HttpPost]
            public ActionResult CData()
            {
                try
                {
                    string sn = Request.QueryString["SN"]; // For ZKTeco ADMS
                    string rawBody;
                    using (var reader = new StreamReader(Request.InputStream))
                    {
                        rawBody = reader.ReadToEnd();
                    }

                    if (string.IsNullOrWhiteSpace(rawBody))
                        return new HttpStatusCodeResult(400, "No content received");

                    using (var db = new ApplicationDbContext())
                    {
                        string deviceType = "UNKNOWN";
                        string deviceSerial = string.Empty;

                        // Detect device type
                        if (!string.IsNullOrEmpty(sn))
                        {
                            deviceType = "ZKTECO_ADMS";
                        }
                        else if (rawBody.TrimStart().StartsWith("{")) // JSON -> Hikvision
                        {
                            deviceType = "HIKVISION";
                        }

                        if (deviceType == "ZKTECO_ADMS")
                        {
                            var device = db.OfficeDeviceSetUp.FirstOrDefault(x => x.DeviceNo == sn);
                            if (device == null)
                                return new HttpStatusCodeResult(404, "Device not registered");

                            string[] records = rawBody.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                            foreach (var line in records)
                            {
                                try
                                {
                                    WriteLog($"ZKTeco Raw: {line}");
                                    string enrollNo = string.Empty;
                                    DateTime punchTime = DateTime.Now;
                                    string inoutmode = "0";
                                    string verifyMode = "0";

                                    if (line.Contains("=")) // Key=Value format
                                    {
                                        var recordData = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)
                                                             .Select(pair => pair.Split('='))
                                                             .Where(kv => kv.Length == 2)
                                                             .ToDictionary(kv => kv[0], kv => kv[1]);

                                        enrollNo = recordData.ContainsKey("PIN") ? recordData["PIN"] : string.Empty;
                                        string timeStr = recordData.ContainsKey("DateTime") ? recordData["DateTime"] : "";
                                        DateTime.TryParseExact(timeStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out punchTime);
                                        inoutmode = recordData.ContainsKey("Status") ? recordData["Status"] : "0";
                                        verifyMode = recordData.ContainsKey("Verify") ? recordData["Verify"] : "0";
                                    }
                                    else // Tab format
                                    {
                                        var parts = line.Split('\t');
                                        if (parts.Length < 3)
                                        {
                                            WriteLog("⚠ Invalid record: " + line);
                                            continue;
                                        }

                                        enrollNo = parts[0].Trim();
                                        string timeStr = parts[1];
                                        DateTime.TryParseExact(timeStr, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out punchTime);
                                        inoutmode = parts[2];
                                        verifyMode = parts.Length > 3 ? parts[3] : "0";
                                    }

                                    if (string.IsNullOrEmpty(enrollNo))
                                        continue;

                                    var emp = db.EmployeeInfo.FirstOrDefault(x => x.EmployeeNo == enrollNo);
                                    if (emp == null) continue;

                                    var office = db.EmployeeOfficeDetail.FirstOrDefault(x => x.EmployeeId == emp.EmployeeId);
                                    if (office == null) continue;

                                    db.AttendanceLog.Add(new AttendanceLog
                                    {
                                        OfficeId = (int)office.OfficeId,
                                        OfficeDeviceId = device.OfficeDeviceId,
                                        IpAddress = device.DeviceIp,
                                        EnrollNumber = enrollNo,
                                        EmployeeId = emp.EmployeeId,
                                        InOutMode = inoutmode,
                                        VerifyMode = verifyMode,
                                        DateTime = punchTime,
                                        Status = 1
                                    });
                                }
                                catch (Exception ex)
                                {
                                    WriteLog($"❌ ZKTeco Record Error: {ex.Message}");
                                }
                            }
                        }
                        else if (deviceType == "HIKVISION")
                        {
                            WriteLog("Hikvision JSON Detected");
                            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(rawBody);
                            deviceSerial = json?.DeviceSerialNo?.ToString();

                            if (string.IsNullOrEmpty(deviceSerial))
                                return new HttpStatusCodeResult(400, "DeviceSerialNo missing");

                            var device = db.OfficeDeviceSetUp.FirstOrDefault(x => x.DeviceNo == deviceSerial);
                            if (device == null)
                                return new HttpStatusCodeResult(404, "Device not registered");

                            string employeeNo = json?.EmployeeNo?.ToString() ?? json?.employeeNoString?.ToString();
                            string passTime = json?.PassTime?.ToString() ?? json?.time?.ToString();
                            DateTime punchTime = DateTime.Now;
                            DateTime.TryParse(passTime, out punchTime);

                            if (!string.IsNullOrEmpty(employeeNo))
                            {
                                var emp = db.EmployeeInfo.FirstOrDefault(x => x.EmployeeNo == employeeNo);
                                if (emp != null)
                                {
                                    var office = db.EmployeeOfficeDetail.FirstOrDefault(x => x.EmployeeId == emp.EmployeeId);
                                    if (office != null)
                                    {
                                        db.AttendanceLog.Add(new AttendanceLog
                                        {
                                            OfficeId = (int)office.OfficeId,
                                            OfficeDeviceId = device.OfficeDeviceId,
                                            IpAddress = device.DeviceIp,
                                            EnrollNumber = employeeNo,
                                            EmployeeId = emp.EmployeeId,
                                            InOutMode = "0",
                                            VerifyMode = "0",
                                            DateTime = punchTime,
                                            Status = 1
                                        });
                                    }
                                }
                            }
                        }

                        db.SaveChanges();
                    }

                    return Content("OK:1"); // Required for ZKTeco, works for both
                }
                catch (Exception ex)
                {
                    WriteLog("❌ Exception: " + ex.Message);
                    return new HttpStatusCodeResult(500, "Server Error");
                }
            }




            [HttpGet]
            public ActionResult GetRequest(string SN)
            {
                if (string.IsNullOrEmpty(SN))
                    return new HttpStatusCodeResult(400, "Bad Request: SN is required.");
                return Content("OK", "text/plain");
            }

            [HttpPost]
            public ActionResult DeviceMD(string SN)
            {
                if (string.IsNullOrEmpty(SN))
                    return new HttpStatusCodeResult(400, "Bad Request: SN is required.");

                string content;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    content = reader.ReadToEnd();
                }
                WriteLog($"DeviceMD reply from {SN}: {content}");
                return Content("OK", "text/plain");
            }
        }

    }
}


