using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using eAttendance.Controllers;
using eAttendance.Models;
using Newtonsoft.Json.Linq;

namespace eAttendance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

            // Start ISUP listener in background thread
            Thread isupThread = new Thread(StartISUPListener);
            isupThread.IsBackground = true;
            isupThread.Start();
        }

        #region ISUP TCP Listener

        private void StartISUPListener()
        {
            int port = 7660;
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, port);
            listener.Start();
            LogToFile("ISUP_Log.txt", $"ISUP Listener started on port {port}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleISUPClient, client);
            }
        }

        private void HandleISUPClient(object obj)
        {
            var client = (TcpClient)obj;
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                int read = stream.Read(buffer, 0, buffer.Length);
                string data = Encoding.UTF8.GetString(buffer, 0, read);

                LogToFile("ISUP_Raw.txt", data);

                JObject json = null;
                try
                {
                    json = JObject.Parse(data);
                }
                catch
                {
                    LogToFile("ISUP_Error.txt", "Non-JSON message received: " + data);
                    return;
                }

                var infoList = json["AcsEvent"]?["InfoList"];
                if (infoList != null)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        foreach (var e in infoList)
                        {
                            string empNo = e["employeeNo"]?.ToString();
                            string timeStr = e["time"]?.ToString();
                            string verifyMode = e["verifyMode"]?.ToString();
                            string deviceIp = ((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                            if (string.IsNullOrEmpty(empNo) || string.IsNullOrEmpty(timeStr)) continue;

                            if (!DateTime.TryParse(timeStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime punchTime))
                                continue;

                            var emp = db.EmployeeInfo.FirstOrDefault(x => x.EmployeeNo == empNo);
                            if (emp == null) continue;

                            var office = db.EmployeeOfficeDetail.FirstOrDefault(x => x.EmployeeId == emp.EmployeeId);
                            if (office == null) continue;

                            bool exists = db.AttendanceLog.Any(x =>
                                x.EmployeeId == emp.EmployeeId && x.DateTime == punchTime);
                            if (exists) continue;

                            db.AttendanceLog.Add(new AttendanceLog
                            {
                                EmployeeId = emp.EmployeeId,
                                EnrollNumber = empNo,
                                OfficeId = (int)office.OfficeId,
                                OfficeDeviceId = 1,
                                IpAddress = deviceIp,
                                DateTime = punchTime,
                                InOutMode = "0",
                                VerifyMode = verifyMode,
                                Status = 1
                            });
                        }
                        db.SaveChanges();
                    }
                }

                // respond quickly to device
                byte[] respBytes = Encoding.UTF8.GetBytes("{\"status\":\"OK\"}");
                stream.Write(respBytes, 0, respBytes.Length);
            }
            catch (Exception ex)
            {
                LogToFile("ISUP_Error.txt", ex.ToString());
            }
            finally
            {
                client.Close();
            }
        }

        #endregion

        #region Logging Helper

        private void LogToFile(string fileName, string content)
        {
            try
            {
                string path;
                if (Server != null)
                    path = Server.MapPath("~/App_Data/" + fileName);
                else
                    path = @"C:\eAttendance\App_Data\" + fileName; // fallback for background thread

                System.IO.File.AppendAllText(
                    path,
                    DateTime.Now + Environment.NewLine + content + Environment.NewLine + "------------------" + Environment.NewLine
                );
            }
            catch (Exception ex)
            {
                // fallback log
                System.IO.File.AppendAllText(@"C:\Temp\ISUP_ErrorFallback.txt",
                    DateTime.Now + Environment.NewLine + ex.ToString() + Environment.NewLine + "------------------" + Environment.NewLine);
            }
        }

        #endregion
    }

    #region Optional HTTP Test Controller

    public class AttendanceController : Controller
    {
        [HttpPost]
        public ActionResult HikvisionPush()
        {
            try
            {
                using (var reader = new System.IO.StreamReader(Request.InputStream))
                {
                    string rawJson = reader.ReadToEnd();
                    // Just log and test
                    System.IO.File.AppendAllText(@"C:\eAttendance\App_Data\Hikvision_Test.txt",
                        DateTime.Now + Environment.NewLine + rawJson + Environment.NewLine + "------------------" + Environment.NewLine);
                }
                return Json(new { status = "ok" });
            }
            catch { return Json(new { status = "error" }); }
        }
    }

    #endregion
}
