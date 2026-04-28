using BioMetricConsole.AttendanceLogRefrence;
using System;
using System.Collections.Generic;

namespace BioMetricConsole
{
    public class Program
    {

        static void Main(string[] args)
        {
            NewProgram obj = new NewProgram();
            obj.SaveDeviceLogDataNew();
        }
    }


    public class NewProgram
    {
        readonly LogPushSoapClient obj = new LogPushSoapClient();
        public void SaveDeviceLogDataNew()
        {
            try
            {

                List<MachineBLL> allMachine = obj.GetAllMachine();
                if (allMachine.Count > 0)
                {
                    foreach (MachineBLL mac in allMachine)
                    {

                        MachineHelper helper = new MachineHelper();
                        string outmessage = "";
                        if (helper.Connect_Net(mac.DeviceIp, mac.Port, out outmessage))
                        {

                            int totalDeviceData = 0;
                            DateTime lastimportdate = new DateTime();
                            List<LogDataBLL> devicedata = helper.GetLogData(mac, out outmessage, out totalDeviceData, out lastimportdate);
                            string message = "";
                            helper.Disconnect_Net(out message);
                            if (outmessage.Length > 0)
                            {


                            }
                            else
                            {





                                string dbmessage = obj.PushLogRefrence(devicedata, mac.DeviceIp, DateTime.Now, mac, "");



                            }

                        }
                        else
                        {

                            helper.Disconnect_Net(out outmessage);

                        }
                    }
                }
                else
                {

                }

            }
            catch (Exception exception1)
            {
                Exception ex = exception1;

            }
        }
    }




}

