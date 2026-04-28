using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Services;

namespace eAttendance.Helper
{
    [DataContract]
    public class MachineBLL
    {

       
        [WebMethod]
        public static bool UpdateLastImportDate(String DeviceIp, DateTime dateTime, out string message)
        {
            return SqlHelper.UpdateLastImportDate(DeviceIp, dateTime, out message);
        }

        [DataMember]
        public int OfficeId { get; set; }
        [DataMember]

        public string OfficeName { get; set; }
        [DataMember]
        public int OfficeDeviceId { get; set; }
        [DataMember]
        public int MachineId { get; set; }
        [DataMember]
        public string DeviceIp { get; set; }
        [DataMember]
        public int DeviceNo { get; set; }
        [DataMember]
        public int Port { get; set; }
        [DataMember]
        public int LicenseNo { get; set; }
        [DataMember]
        public DateTime LastImportDate { get; set; }
        [DataMember]
        public List<MachineBLL> MachineList { get; set; }
        [DataMember]
        public bool IsChecked { get; set; }
    }

}