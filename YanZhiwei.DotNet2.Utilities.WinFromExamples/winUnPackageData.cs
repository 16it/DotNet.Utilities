using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YanZhiwei.DotNet2.Utilities.WinFromExamples
{
    public partial class winUnPackageData : Form
    {
        public winUnPackageData()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.Open();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
        }
        
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
        }
    }
}
