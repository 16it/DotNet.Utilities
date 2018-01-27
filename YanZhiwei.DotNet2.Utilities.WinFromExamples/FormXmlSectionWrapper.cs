using System;
using System.Windows.Forms;
using YanZhiwei.DotNet2.Utilities.WinForm;

namespace YanZhiwei.DotNet2.Utilities.WinFromExamples
{
    public partial class FormXmlSectionWrapper : Form
    {
        public FormXmlSectionWrapper()
        {
            InitializeComponent();
        }
        private string ConfigFilename = string.Empty;
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = "Configuration Files (*.config)| *.config";
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ConfigFilename = _openFileDialog.FileName;
                propertyGrid1.LoadSectionConfigure(ConfigFilename);
                txtConfigurationFile.Text = ConfigFilename;
                this.propertyGrid1.Focus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            propertyGrid1.SaveConfigure(ConfigFilename);
        }
    }
}