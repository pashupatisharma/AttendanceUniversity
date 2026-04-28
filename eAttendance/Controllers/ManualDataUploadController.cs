using eAttendance.Models;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace eAttendance.Controllers
{
    public class ManualDataUploadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ManualDataUpload
        public static string GetConnectionString()
        {
            string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            return str;
        }
        public ActionResult Index()
        {
            return View();
        }


     


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file, int OfficeId)
        {
            SqlConnection connection = new SqlConnection(GetConnectionString());

            var DeviceId = OfficeId;
            string ip = OfficeId.ToString();
            if (db.OfficeDeviceSetUp.Where(x => x.OfficeId == OfficeId).FirstOrDefault() != null)
            {
                DeviceId = db.OfficeDeviceSetUp.Where(x => x.OfficeId == OfficeId).FirstOrDefault().OfficeDeviceId;
                ip = db.OfficeDeviceSetUp.Where(x => x.OfficeId == OfficeId).FirstOrDefault().DeviceIp;
            }
            connection.Open();
            string filePath = string.Empty;
            var fileName = Path.GetFileName(file.FileName);
            if (file != null)
            {
                try
                {

                    using (var csvReader = new System.IO.StreamReader(file.InputStream))
                    {
                        string inputLine = "";

                        //read each line
                        while ((inputLine = csvReader.ReadLine()) != null)
                        {
                            string[] dr = inputLine.Trim().Split('\t');
                            SqlCommand command = connection.CreateCommand();
                            command.Connection = connection;

                            command.CommandText = "[SpSaveLogData]";
                            command.Parameters.AddWithValue("OfficeId", OfficeId);

                            command.Parameters.AddWithValue("OfficeDeviceId", DeviceId);
                            command.Parameters.AddWithValue("IpAddress", ip);
                            command.Parameters.AddWithValue("EnrollNumber", Convert.ToInt32(dr[0].ToString()));
                            command.Parameters.AddWithValue("VerifyMode", dr[3].ToString());
                            command.Parameters.AddWithValue("InOutMode", dr[2].ToString());

                            command.Parameters.AddWithValue("DateTime", Convert.ToDateTime(dr[1].ToString()));
                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();
                        }
                    }
                    TempData["Message"] = "Data Upload Sucessfully";
                }
                catch(Exception ex)
                {
                    TempData["Message"] = "Error on Data Upload";
                }












            }
            return View();
        }





    }
}