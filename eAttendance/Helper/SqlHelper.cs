using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;

namespace eAttendance.Helper
{[DataContract]
    public class SqlHelper
    {
       [WebMethod]
        public  List<MachineBLL> GetAllMachine()
        {
            List<MachineBLL> list2;
            string connectionstring = GetConnectionString();
            SqlConnection connection = new SqlConnection(connectionstring);
            try
            {

                int OfficeId = Convert.ToInt32(ConfigurationManager.AppSettings["OfficeId"]);

                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                command.CommandText = "SpGetOfficeDeviceList";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("DeviceType", 2);

                command.Parameters.AddWithValue("OfficeId", OfficeId);
                SqlDataReader reader = command.ExecuteReader();
                List<MachineBLL> list = new List<MachineBLL>();
                while (reader.Read())
                {
                    MachineBLL item = new MachineBLL();
                    item.OfficeId = Convert.ToInt32(reader["OfficeId"]);
                    item.OfficeName = Convert.ToString(reader["OfficeName"]);
                    item.LicenseNo = Convert.ToInt32(reader["LicenseNo"]);
                    item.OfficeDeviceId = Convert.ToInt32(reader["OfficeDeviceId"]);
                    item.DeviceNo = Convert.ToInt32(reader["DeviceNo"]);
                    item.DeviceIp = Convert.ToString(reader["DeviceIp"]);
                    item.Port = Convert.ToInt32(reader["Port"]);
                    item.LastImportDate = (reader["LastImportDate"].ToString() != "00/00/0000 12:00:00 AM")
                        ? Convert.ToDateTime(reader["LastImportDate"])
                        : DateTime.MinValue;

                    list.Add(item);
                }
                connection.Close();
                list2 = list;
            }
            catch (SqlException exception)
            {
                Console.WriteLine("Commit Exception Type: {0}", exception.GetType());
                Console.WriteLine("  Message: {0}", exception.Message);
                list2 = null;
            }
            catch (Exception exception2)
            {
                Console.WriteLine("Commit Exception Type: {0}", exception2.GetType());
                Console.WriteLine("  Message: {0}", exception2.Message);
                list2 = null;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return list2;
        }
        [WebMethod]
        public static string GetConnectionString()
        {
            string str = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            return str;
        }
        [WebMethod]
        public static void SaveEnrollDataToDB(int MachineId, string IpAddress, int vEnrollNumber, int vBackupNumber, int vPrivilege, string vEnrollName)
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                try
                {
                    command.CommandText = "[dbo].[SpSaveMachineEnrollData]";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@OfficeDeviceId", MachineId);
                    command.Parameters.AddWithValue("@IpAddress", IpAddress);
                    command.Parameters.AddWithValue("@EnrollNumber", vEnrollNumber);
                    command.Parameters.AddWithValue("@BackupNumber", vBackupNumber);
                    command.Parameters.AddWithValue("@Privilege", vPrivilege);
                    command.Parameters.AddWithValue("@EnrollName", vEnrollName);
                    command.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Commit Exception Type: {0}", exception.GetType());
                    Console.WriteLine("  Message: {0}", exception.Message);
                    try
                    {
                    }
                    catch (Exception exception2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", exception2.GetType());
                        Console.WriteLine("  Message: {0}", exception2.Message);
                    }
                }
            }
        }
        [WebMethod]
        public static bool SaveGeneralLogData(int MachineId, string IpAddress, int EnrollNumber, int VerityMode, int InOutMode, DateTime Date)
        {
            bool flag;
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                try
                {
                    command.CommandText = "call SpSaveMachineData(@MachineId,@IpAddress,@EnrollNumber,@VerityMode,@InOutMode,@DateTime)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@MachineId", MachineId);
                    command.Parameters.AddWithValue("@IpAddress", IpAddress);
                    command.Parameters.AddWithValue("@EnrollNumber", EnrollNumber);
                    command.Parameters.AddWithValue("@VerityMode", VerityMode);
                    command.Parameters.AddWithValue("@InOutMode", InOutMode);
                    command.Parameters.AddWithValue("@DateTime", Date);
                    int num = command.ExecuteNonQuery();
                    flag = true;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Commit Exception Type: {0}", exception.GetType());
                    Console.WriteLine("  Message: {0}", exception.Message);
                    try
                    {
                    }
                    catch (Exception exception2)
                    {
                        Console.WriteLine("Rollback Exception Type: {0}", exception2.GetType());
                        Console.WriteLine("  Message: {0}", exception2.Message);
                    }
                    flag = false;
                }
            }
            return flag;
        }
        [WebMethod]
        public static bool SaveLogData(List<LogDataBLL> devicedata, out string message)
        {
            bool flag2;
            int num = 1;
            message = "";
            SqlConnection connection = new SqlConnection(GetConnectionString());
            try
            {
                connection.Open();
                foreach (LogDataBLL abll in devicedata)
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Connection = connection;
                    object[] objArray1 = new object[] { abll.dwYear, "-", abll.dwMonth, "-", abll.dwDay };
                    string str = string.Concat(objArray1);
                    TimeSpan span = new TimeSpan(abll.dwHour, abll.dwMinute, abll.dwSecond);
                    command.CommandText = "[SpSaveLogData]";
                    command.Parameters.AddWithValue("OfficeId", abll.OfficeId);
                    command.Parameters.AddWithValue("OfficeDeviceId", abll.OfficeDeviceId);
                    command.Parameters.AddWithValue("IpAddress", abll.IpAddress);
                    command.Parameters.AddWithValue("EnrollNumber", abll.EnrollNumber);
                    command.Parameters.AddWithValue("VerifyMode", abll.VerifyMode);
                    command.Parameters.AddWithValue("InOutMode", abll.InOutMode);

                    command.Parameters.AddWithValue("DateTime", new DateTime(abll.dwYear, abll.dwMonth, abll.dwDay, abll.dwHour, abll.dwMinute, abll.dwSecond));
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }
                if (num > 0)
                {
                    message = "Success";
                    return true;
                }
                message = "No data saved in database.";
                flag2 = true;
            }
            catch (SqlException exception)
            {
                message = "MySqlError " + exception.Message;
                flag2 = false;
            }
            catch (Exception exception2)
            {
                message = "Error " + exception2;
                Console.WriteLine("Commit Exception Type: {0}", exception2.GetType());
                Console.WriteLine("  Message: {0}", exception2.Message);
                try
                {
                }
                catch (Exception exception3)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", exception3.GetType());
                    Console.WriteLine("  Message: {0}", exception3.Message);
                }
                flag2 = false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return flag2;
        }
        [WebMethod]
        public static string TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(GetConnectionString()))
                {
                    connection.Open();
                }
                return "Success";
            }
            catch (SqlException exception)
            {
                return exception.Message;
            }
        }
        [WebMethod]
        public static bool UpdateLastImportDate(string DeviceIp, DateTime dateTime, out string message)
        {
            bool flag2;
            SqlConnection connection = new SqlConnection(GetConnectionString());
            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.Connection = connection;
                object[] objArray1 = new object[] { dateTime.Year, "-", dateTime.Month, "-", dateTime.Day, " ", dateTime.Hour, ":", dateTime.Minute, ":", dateTime.Second };
                string str = string.Concat(objArray1);
                string[] textArray1 = new string[] { "update OfficeDeviceSetUps set LastImportDate='", str, "' where DeviceIp='", DeviceIp, "'" };
                command.CommandText = string.Concat(textArray1);
                command.CommandType = CommandType.Text;
                if (command.ExecuteNonQuery() > 0)
                {
                    message = "Success";
                    return true;
                }
                message = "Error2";
                flag2 = false;
            }
            catch (SqlException exception)
            {
                message = "MySqlError " + exception.Message;
                flag2 = false;
            }
            catch (Exception exception2)
            {
                message = "Error " + exception2;
                Console.WriteLine("Commit Exception Type: {0}", exception2.GetType());
                Console.WriteLine("  Message: {0}", exception2.Message);
                try
                {
                }
                catch (Exception exception3)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", exception3.GetType());
                    Console.WriteLine("  Message: {0}", exception3.Message);
                }
                flag2 = false;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Dispose();
                }
            }
            return flag2;
        }
    }
}