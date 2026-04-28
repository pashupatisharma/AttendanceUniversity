using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eAttendance.Models
{
    public class DeviceModel
    {
        public string DeviceIp { get; set; }
        public int Port { get; set; }
        public string officeName { get; set; }
        public bool ischecked { get; set; }
        public List<DeviceModel> DeviceModelList { get; set; }
    }
}