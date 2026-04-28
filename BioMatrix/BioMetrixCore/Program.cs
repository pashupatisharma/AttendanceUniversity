using System;
using System.Drawing;
using System.Windows.Forms;

namespace BioMetrixCore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Master form = new Master();
            //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);


            string[] s = { "\\bin" };
            string path = Application.StartupPath.Split(s, StringSplitOptions.None)[0] + "\\nepal-logo.ico";

            form.Icon = new Icon(path);
        
            form.Text = "Attendace Data Push";

            form.FormBorderStyle = FormBorderStyle.FixedDialog;

            // Set the MaximizeBox to false to remove the maximize box.
            form.MaximizeBox = false;

            form.MinimumSize = new Size(500, 400);
            form.MaximumSize = new Size(500, 400);

            // Set the start position of the form to the center of the screen.
            form.StartPosition = FormStartPosition.CenterScreen;

            // Display the form as a modal dialog box.
            form.ShowDialog();

            Application.Run(form);
        }
    }
}
