using BioMetricConsole.AttendanceLogRefrence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using zkemkeeper;

namespace BioMetricConsole
{
    public class MachineHelper
    {
        // Fields
        private bool bIsConnected = false;
        private int iMachineNumber = 1;
        public ZkemClient zke;//= new ZkemClient(RaiseDeviceEvent);


        private string strdwEnrollNumber = "3";
        private int dwVerifyMode = 1;
        private int dwInOutMode = 0;
        private int dwYear = 0;
        private int dwMonth = 0;
        private int dwDay = 0;
        private int dwHour = 0;
        private int dwMinute = 0;
        private int dwSecond = 0;
        private int dwWorkcode = 0;

        private void RaiseDeviceEvent(object sender, string actionType)
        {
            switch (actionType)
            {
                case UniversalStatic.acx_Disconnect:
                    {
                        //ShowStatusBar("The device is switched off", true);
                        //DisplayEmpty();
                        //btnConnect.Text = "Connect";
                        //ToggleControls(false);
                        break;
                    }

                default:
                    break;
            }

        }

        // Methods
        public bool Connect_Net(string IPAdd, int Port, out string message)
        {

            int dwErrorCode = 0;
            try
            {
                zke = new ZkemClient(RaiseDeviceEvent);
                this.bIsConnected = zke.Connect_Net(IPAdd, Port);
            }
            catch (Exception ex)
            {

                throw;
            }

            if (this.bIsConnected)
            {
                message = "Connected successfully.\n";
                this.iMachineNumber = 1;
                zke.RegEvent(this.iMachineNumber, 0xffff);
            }
            else
            {
                zke.GetLastError(ref dwErrorCode);
                message = "Unable to connect the device " + IPAdd + ", ErrorCode=" + dwErrorCode.ToString();
            }
            return this.bIsConnected;
        }

        public bool Disconnect_Net(out string message)
        {
            if (zke == null) zke = new ZkemClient(RaiseDeviceEvent);
            zke.Disconnect();
            message = "Device Disconnected.\n";
            return true;
        }

        public void GetAllUserInfo()
        {
            string sdwEnrollNumber = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex;
            bool bEnabled = false;

            List<UserInfo> lstFPTemplates = new List<UserInfo>();

            zke.ReadAllUserID(iMachineNumber);
            zke.ReadAllTemplate(iMachineNumber);

            while (zke.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (zke.GetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))
                    {
                        UserInfo fpInfo = new UserInfo();
                        fpInfo.MachineNumber = iMachineNumber;
                        fpInfo.EnrollNumber = sdwEnrollNumber;
                        fpInfo.Name = sName;
                        fpInfo.FingerIndex = idwFingerIndex;
                        fpInfo.TmpData = sTmpData;
                        fpInfo.Privelage = iPrivilege;
                        fpInfo.Password = sPassword;
                        fpInfo.Enabled = bEnabled;
                        fpInfo.iFlag = iFlag.ToString();

                        lstFPTemplates.Add(fpInfo);
                    }
                }

            }
            //return lstFPTemplates;
        }

        public List<LogDataBLL> GetLogData(MachineBLL mac, out string message, out int totaldevicedata, out DateTime lastimportDate)
        {

            lastimportDate = new DateTime();
            totaldevicedata = 0;
            List<LogDataBLL> list = new List<LogDataBLL>();
            ZkemClient objZkeeper = zke;
            int machineNumber = iMachineNumber;
            string dwEnrollNumber1 = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<MachineInfo> lstEnrollData = new List<MachineInfo>();

            objZkeeper.ReadAllGLogData(machineNumber);

            while (objZkeeper.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))

            {
                DateTime dt = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);



                if (dt > mac.LastImportDate)
                {
                    LogDataBLL item = new LogDataBLL
                    {
                        OfficeId = mac.OfficeId,
                        OfficeDeviceId = mac.OfficeDeviceId,
                        IpAddress = mac.DeviceIp,
                        EnrollNumber = dwEnrollNumber1,
                        VerifyMode = dwVerifyMode,
                        InOutMode = dwInOutMode,
                        dwYear = dwYear,
                        dwMonth = dwMonth,
                        dwDay = dwDay,
                        dwHour = dwHour,
                        dwMinute = dwMinute,
                        dwSecond = dwSecond,
                        Workcode = dwWorkcode
                    };
                    list.Add(item);
                }
                if (lastimportDate < dt)
                {
                    lastimportDate = dt;
                }
                totaldevicedata++;

            }

            message = "";
            zke.RefreshData(this.iMachineNumber);
            zke.EnableDevice(this.iMachineNumber, true);
            zke.Disconnect();
            return list;

            //return lstEnrollData;
        }

        public List<LogDataBLL> GetAllGeneralLogData(MachineBLL mac, out string message, out int totaldevicedata, out DateTime lastimportDate)
        {


            totaldevicedata = 0;
            int dwErrorCode = 0;
            lastimportDate = new DateTime();
            List<LogDataBLL> list = new List<LogDataBLL>();
            zke.EnableDevice(mac.DeviceNo, false);




            if (zke.ReadGeneralLogData(this.iMachineNumber))
            {
                while (zke.SSR_GetGeneralLogData(this.iMachineNumber, out this.strdwEnrollNumber, out this.dwVerifyMode, out this.dwInOutMode, out this.dwYear, out this.dwMonth, out this.dwDay, out this.dwHour, out this.dwMinute, out this.dwSecond, ref this.dwWorkcode))
                {
                    if (new DateTime(this.dwYear, this.dwMonth, this.dwDay, this.dwHour, this.dwMinute, this.dwSecond) > mac.LastImportDate)
                    {
                        LogDataBLL item = new LogDataBLL
                        {
                            OfficeId = mac.OfficeId,
                            OfficeDeviceId = mac.OfficeDeviceId,
                            IpAddress = mac.DeviceIp,
                            EnrollNumber = this.strdwEnrollNumber,
                            VerifyMode = this.dwVerifyMode,
                            InOutMode = this.dwInOutMode,
                            dwYear = this.dwYear,
                            dwMonth = this.dwMonth,
                            dwDay = this.dwDay,
                            dwHour = this.dwHour,
                            dwMinute = this.dwMinute,
                            dwSecond = this.dwSecond,
                            Workcode = this.dwWorkcode
                        };
                        list.Add(item);
                    }
                    if (lastimportDate < new DateTime(this.dwYear, this.dwMonth, this.dwDay, this.dwHour, this.dwMinute, this.dwSecond))
                    {
                        lastimportDate = new DateTime(this.dwYear, this.dwMonth, this.dwDay, this.dwHour, this.dwMinute, this.dwSecond);
                    }
                    totaldevicedata++;
                }
                message = "";
                zke.RefreshData(this.iMachineNumber);
                zke.EnableDevice(this.iMachineNumber, true);
                zke.Disconnect();
                return list;
            }
            zke.GetLastError(ref dwErrorCode);
            message = "Reading data failed. Error Code : " + dwErrorCode + " \n";
            zke.EnableDevice(this.iMachineNumber, true);
            zke.Disconnect();
            return null;
        }

        public object ClearData(int machineNumber, ClearFlag clearFlag)
        {

            if (zke == null)
            {
                zke = new ZkemClient(RaiseDeviceEvent);
            }
            ZkemClient objZkeeper = zke;
            int iDataFlag = (int)clearFlag;

            if (objZkeeper.ClearData(machineNumber, iDataFlag))
                return objZkeeper.RefreshData(machineNumber);
            else
            {
                int idwErrorCode = 0;
                objZkeeper.GetLastError(ref idwErrorCode);
                return idwErrorCode;
            }
        }




    }





}
