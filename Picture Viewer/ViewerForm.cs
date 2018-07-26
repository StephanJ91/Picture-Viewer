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
    public partial class ViewerForm : Form
    {
        const bool c_defPromptOnExit = false;
        string m_strUserName = "";
        bool m_blnPromptOnExit = c_defPromptOnExit;
        Color m_objPictureBackColour;

        private void OpenPicture()
        {
            try
            {
                //Show the Open File Dialog box.
                if (ofdSelectPicture.ShowDialog() == DialogResult.OK)
                {
                    //Load the picture into the picture box.
                    picShowPicture.Image = Image.FromFile(ofdSelectPicture.FileName);

                    //Show the name of the file in the form's caption.
                    this.Text = string.Concat("Picture Viewer(" + ofdSelectPicture.FileName + ")");
                    sbrMyStatusStrip.Items[0].Text = ofdSelectPicture.FileName;
                }

                UpdateLog(ofdSelectPicture.FileName);
            }
            catch(System.OutOfMemoryException)
            {
                MessageBox.Show("The file you have chosen is not an image file.", "Invalid file", MessageBoxButtons.OK);
            }
        }

        private void UpdateLog(string strFileName)
        {
            System.IO.StreamWriter objFile = new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + @"\PictureLog.txt", true);

            objFile.WriteLine(DateTime.Now + " " + strFileName);
            objFile.Close();
            objFile.Dispose();
        }

        private void DrawBorder(PictureBox objPicturebox)
        {
            Graphics objGraphics = null;
            objGraphics = this.CreateGraphics();
            objGraphics.Clear(SystemColors.ControlDark);
            objGraphics.DrawRectangle(Pens.Blue, picShowPicture.Left - 1, picShowPicture.Top - 1, picShowPicture.Width + 1, picShowPicture.Height + 1);
            objGraphics.Dispose();
        }

        private void GetAttributes()
        {
            //Make sure a file is open.
            if ((ofdSelectPicture.FileName) == "")
            {
                MessageBox.Show("There is no file open");
                return;
            }

            //create the string builder object to concatenate strings.
            System.Text.StringBuilder stbProperties = new StringBuilder("");
            System.IO.FileAttributes fileAttributes;

            //Get the dates
            stbProperties.Append("Created: ");
            stbProperties.Append(System.IO.File.GetCreationTime(ofdSelectPicture.FileName));
            stbProperties.Append("\r\n");

            stbProperties.Append("Accessed: ");
            stbProperties.Append(System.IO.File.GetLastAccessTime(ofdSelectPicture.FileName));
            stbProperties.Append("\r\n");

            stbProperties.Append("Modified: ");
            stbProperties.Append(System.IO.File.GetLastWriteTime(ofdSelectPicture.FileName));

            //Get File Attributes
            fileAttributes = System.IO.File.GetAttributes(ofdSelectPicture.FileName);
            stbProperties.Append("\r\n");

            //Use a binary AND to extract specific attributes.
            stbProperties.Append("Normal: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.Normal) == System.IO.FileAttributes.Normal));
            stbProperties.Append("\r\n");

            stbProperties.Append("Hidden: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden));
            stbProperties.Append("\r\n");

            stbProperties.Append("ReadOnly: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly));
            stbProperties.Append("\r\n");

            stbProperties.Append("System: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.System) == System.IO.FileAttributes.System));
            stbProperties.Append("\r\n");

            stbProperties.Append("Temporary File: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.Temporary) == System.IO.FileAttributes.Temporary));
            stbProperties.Append("\r\n");

            stbProperties.Append("Archive: ");
            stbProperties.Append(Convert.ToBoolean((fileAttributes & System.IO.FileAttributes.Archive) == System.IO.FileAttributes.Archive));
            MessageBox.Show(stbProperties.ToString());
        }

        private void ShowLog()
        {
            LogViewerForm objLog = new LogViewerForm();
            objLog.ShowDialog();
        }
        public ViewerForm()
        {
            InitializeComponent();
        }

        private void btnEnlarge_Click(object sender, EventArgs e)
        {
            this.Width = Width + 20;
            this.Height = Height + 20;
        }

        private void btnShrink_Click(object sender, EventArgs e)
        {

            this.Width = Width - 20;
            this.Height = Height - 20;
        }

        private void picShowPicture_MouseMove(object sender, MouseEventArgs e)
        {
            lblX.Text = "X: " + e.X.ToString();
            lblY.Text = "Y: " + e.Y.ToString();
        }

        private void picShowPicture_MouseLeave(object sender, EventArgs e)
        {
            lblX.Text = "";
            lblY.Text = "";
        }

        private void ViewerForm_Load(object sender, EventArgs e)
        {
            lblX.Text = "";
            lblY.Text = "";

            LoadDefaults();
        }

        private void LoadDefaults()
        {
            m_blnPromptOnExit = Convert.ToBoolean(Registry.GetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "PromptOnExit", "false"));

            if (Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\CleverSoftware\PictureViewer\", "BackColour", "Gray")) == "Gray")
                m_objPictureBackColour = SystemColors.ControlDark;
            else
                m_objPictureBackColour = Color.White;

            picShowPicture.BackColor = m_objPictureBackColour;

            mnuConfirmOnExit.Checked = m_blnPromptOnExit;
        }

        private void mnuOpenPicture_Click(object sender, EventArgs e)
        {
            this.OpenPicture();
        }

        private void mnuConfirmOnExit_Click(object sender, EventArgs e)
        {
            mnuConfirmOnExit.Checked = !(mnuConfirmOnExit.Checked);
            m_blnPromptOnExit = mnuConfirmOnExit.Checked;
        }

        private void mnuQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuDrawBorder_Click(object sender, EventArgs e)
        {
            this.DrawBorder(picShowPicture);
        }

        private void mnuOptions_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptionsDialog = new OptionsForm();
            frmOptionsDialog.ShowDialog();

            LoadDefaults();
        }

        private void tbbOpenPicture_Click(object sender, EventArgs e)
        {
            this.OpenPicture();
        }

        private void tbbDrawBorder_Click(object sender, EventArgs e)
        {
            this.DrawBorder(picShowPicture);
        }

        private void tbbOptions_Click(object sender, EventArgs e)
        {
            OptionsForm frmOptionsDialog = new OptionsForm();
            frmOptionsDialog.ShowDialog();

            LoadDefaults();
        }

        private void ViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_blnPromptOnExit)
            {
                if(MessageBox.Show("Close the Picture Viewer program?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void tbbGetFileAttributes_Click(object sender, EventArgs e)
        {
            GetAttributes();
        }

        private void ttbShowLog_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void getFileAttributesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAttributes();
        }

        private void mnuShowLog_Click(object sender, EventArgs e)
        {
            ShowLog();
        }
    }
}
