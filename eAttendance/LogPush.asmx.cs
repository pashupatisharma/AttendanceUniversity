using eAttendance.Helper;
using System;
using System.Collections.Generic;
using System.Web.Services;

namespace eAttendance
{
    /// <summary>
    /// Summary description for LogPush
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class LogPush : System.Web.Services.WebService
    {

        [WebMethod]
        public string PushLogRefrence(List<LogDataBLL> devicedata, string DeviceIp, DateTime lastimportdate, MachineBLL machineobj, string dbmessage)
        {

            string rtbFullMessage = string.Empty;
            if (LogDataBLL.MySqlSaveLogData(devicedata, out dbmessage))
            {

                rtbFullMessage = "Saving data to database completed";
                string lastImportDateMessage = "";
                if (SqlHelper.UpdateLastImportDate(DeviceIp, lastimportdate, out lastImportDateMessage))
                    if (MachineBLL.UpdateLastImportDate(DeviceIp, lastimportdate, out lastImportDateMessage))
                    {

                        object[] objArray1 = new object[] { rtbFullMessage, "Lastest Import date (", lastimportdate, ") updated.\n" };
                        rtbFullMessage = string.Concat(objArray1);

                    }
                    else
                    {
                        rtbFullMessage = rtbFullMessage + lastImportDateMessage;
                    }
                rtbFullMessage = "Importing data completed successfully for device : " + DeviceIp;
            }

            else
            {
                rtbFullMessage = rtbFullMessage +
                                                          "Error while saving data to database. Error:" + dbmessage;
                rtbFullMessage =
                    rtbFullMessage + "Importing data failed for device:" + DeviceIp;

            }
            return rtbFullMessage;

        }


        [WebMethod]
        public List<MachineBLL> GetAllMachine()
        {
            SqlHelper obj = new SqlHelper();
            var list = obj.GetAllMachine();
            return list;
        }

    }
}
