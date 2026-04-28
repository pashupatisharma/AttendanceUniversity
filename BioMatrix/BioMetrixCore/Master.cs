
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

using System.ComponentModel;
using Syncfusion.WinForms.Core.Utils;
using BioMetrixCore.LogPushService;

namespace BioMetrixCore
{
    public partial class Master : Form
    {
        LogPushSoapClient obj = new LogPushSoapClient();
        private bool isDeviceConnected = false;
        private ProgressBar pbImport;
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        BusyIndicator busyIndicator = new BusyIndicator();




        public Master()
        {
            InitializeComponent();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += DoWork;
            backgroundWorker.RunWorkerCompleted += RunWorkerCompleted;
            LoadOficeInfo();
          

        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            this.btnUploadUserInfo.Invoke(new Action(() => this.btnUploadUserInfo.Text = string.Empty));
            busyIndicator.Show(this.btnUploadUserInfo);
            SaveDeviceLogData();


        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.btnUploadUserInfo.Text = "Import Data";
            if (!backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
            busyIndicator.Hide();
           
        }

        private void btnUploadUserInfo_Click(object sender, EventArgs e)
        {

            backgroundWorker.RunWorkerAsync();
           
        }


        public void LoadOficeInfo()
        {

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnCount = 6;


            //Add Columns
            dataGridView1.Columns[0].Name = "OfficeName"; // name
            dataGridView1.Columns[0].HeaderText = "Office Name"; // header text
            dataGridView1.Columns[0].DataPropertyName = "OfficeName"; // field name


            dataGridView1.Columns[1].Name = "OfficeId"; // name
            dataGridView1.Columns[1].HeaderText = "Office Id"; // header text
            dataGridView1.Columns[1].DataPropertyName = "OfficeId"; // field name


            dataGridView1.Columns[2].Name = "OfficeDeviceId"; // name
            dataGridView1.Columns[2].HeaderText = "Device Id"; // header text
            dataGridView1.Columns[2].DataPropertyName = "OfficeDeviceId"; // field name


            dataGridView1.Columns[3].Name = "DeviceIp"; // name
            dataGridView1.Columns[3].HeaderText = "Device Ip"; // header text
            dataGridView1.Columns[3].DataPropertyName = "DeviceIp"; // field name


            dataGridView1.Columns[4].Name = "Port"; // name
            dataGridView1.Columns[4].HeaderText = "Port"; // header text
            dataGridView1.Columns[4].DataPropertyName = "Port"; // field name


            dataGridView1.Columns[5].Name = "LastImportDate"; // name
            dataGridView1.Columns[5].HeaderText = "Last Import Date"; // header text
            dataGridView1.Columns[5].DataPropertyName = "LastImportDate"; // field name

            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();


            checkBoxColumn.HeaderText = "";
            checkBoxColumn.Width = 30;
            checkBoxColumn.Name = "checkBoxColumn";
            dataGridView1.Columns.Insert(0, checkBoxColumn);

            List<MachineBLL> allMachine = obj.GetAllMachine();
            dataGridView1.DataSource = allMachine;
           
        }

        public void SaveDeviceLogData()
        {
            int count = 0;
            try
            {

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    bool isSelected = Convert.ToBoolean(row.Cells["checkBoxColumn"].Value);
                    if (isSelected)
                    {
                        count++;
                        var  officeId= row.Cells["OfficeId"].Value.ToString();
                        var DeviceId = row.Cells["OfficeDeviceId"].Value.ToString();
                        var ip = row.Cells["DeviceIp"].Value.ToString();
                        int port = Convert.ToInt32(row.Cells["Port"].Value.ToString());
                        var LastImportDate= row.Cells["LastImportDate"].Value.ToString();



                        MachineHelper helper = new MachineHelper();
                        this.rtbFullMessage.Text = this.rtbFullMessage.Text + "Connecting to device : " + ip;
                        string outmessage = "";
                        if (helper.Connect_Net(ip, port, out outmessage))
                        {
                            MachineBLL mac = new MachineBLL();

                            mac.OfficeDeviceId =int.Parse(DeviceId);

                            mac.OfficeId = int.Parse(officeId);
                            mac.DeviceIp = ip;
                            mac.Port = port;
                            mac.LastImportDate = Convert.ToDateTime(LastImportDate);
                            rtbFullMessage.Text = "Connected Successfully";

                            this.rtbFullMessage.Text = "Importing data from device";
                            int totalDeviceData = 0;
                            DateTime lastimportdate = new DateTime();
                            List<LogDataBLL> devicedata = helper.GetLogData(mac, out outmessage, out totalDeviceData, out lastimportdate);
                            string message = "";
                            helper.Disconnect_Net(out message);
                            if (outmessage.Length > 0)
                            {

                                this.rtbFullMessage.Text = this.rtbFullMessage.Text + outmessage;
                                this.rtbFullMessage.Text = this.rtbFullMessage.Text + "Import failed";

                            }
                            else
                            {


                                object[] objArrayfirst = new object[] { rtbFullMessage.Text, "Total device data count: ", totalDeviceData, " \n" };
                                rtbFullMessage.Text = string.Concat(objArrayfirst);


                                object[] objArraysecond = new object[] { rtbFullMessage.Text, "Total data to save in DB:", devicedata.Count, " \n" };
                                rtbFullMessage.Text = string.Concat(objArraysecond);


                                this.rtbFullMessage.Text = "Importing data from device completed";
                                this.rtbFullMessage.Text = "Saving data to database.";


                                string dbmessage = obj.PushLogRefrence(devicedata, mac.DeviceIp, DateTime.Now, mac, "");


                                rtbFullMessage.Text = this.rtbFullMessage.Text + dbmessage;
                            }

                        }
                        else
                        {

                            rtbFullMessage.Text = this.rtbFullMessage.Text + outmessage;
                            helper.Disconnect_Net(out outmessage);

                        }

                    }
                   
                }
                if (count == 0)
                {
                   
                        MessageBox.Show("Selected at least one Rows");
                    
                }
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                this.rtbFullMessage.Text = ex.Message;
            }
        }

        private void Master_Load(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }

        private void Master_FormClosed(object sender, FormClosedEventArgs e)
        {
           Application.Exit();
        }








    }
}
