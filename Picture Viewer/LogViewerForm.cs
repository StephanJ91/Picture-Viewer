using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Picture_Viewer
{
    public partial class LogViewerForm : Form
    {

        public LogViewerForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LogViewerForm_Load(object sender, EventArgs e)
        {
            try
            {
                System.IO.StreamReader objFile = new System.IO.StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + @"\PictureLog.txt");
                txtLog.Text = objFile.ReadToEnd();
                objFile.Close();
                objFile.Dispose();
            }
            catch
            {
                txtLog.Text = "The log file could not be found";
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            System.IO.File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + @"\PictureLog.txt");
            txtLog.Text = "Log file has been deleted.";
        }
    }
}
