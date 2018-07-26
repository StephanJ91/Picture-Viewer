using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Picture_Viewer
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Registry.SetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "UserName", txtUserName.Text);

            Registry.SetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "PromptOnExit", chkPromptOnExit.Checked);

            if (optBackgroundDefault.Checked)
                Registry.SetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "BackColour", "Gray");
            else
                Registry.SetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "BackColour", "White");

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "UserName", ""));

            chkPromptOnExit.Checked = Convert.ToBoolean(Registry.GetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "PromptOnExit", "false"));

            if (Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "BackColour", "Gray")) == "Gray")
                optBackgroundDefault.Checked = true;
            else
                optBackgroundWhite.Checked = true;
        }
    }
}
