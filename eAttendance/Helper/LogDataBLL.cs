using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Services;

namespace eAttendance.Helper
{[DataContract]
    public class LogDataBLL
    {
        [WebMethod]
        public static bool MySqlSaveLogData(List<LogDataBLL> devicedata, out string message)
        {
            return SqlHelper.SaveLogData(devicedata, out message);
        }

       [DataMember]
        public int OfficeId { get; set; }
        [DataMember]
        public int OfficeDeviceId { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
        [DataMember]
        public int EnrollNumber { get; set; }
        [DataMember]
        public string VerifyMode { get; set; }
        [DataMember]
        public string InOutMode { get; set; }
        [DataMember]
        public int dwYear { get; set; }
        [DataMember]
        public int dwMonth { get; set; }
        [DataMember]
        public int dwDay { get; set; }
        [DataMember]
        public int dwHour { get; set; }
        [DataMember]
        public int dwMinute { get; set; }
        [DataMember]
        public int dwSecond { get; set; }
        [DataMember]
        public int Workcode { get; set; }
        [DataMember]
        public List<LogDataBLL> LogDateList { get; set; }

    }
}