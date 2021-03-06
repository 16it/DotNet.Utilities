﻿using System;
using System.Data;
using System.Windows.Forms;
using YanZhiwei.DotNet.MyXls.Utilities;
using YanZhiwei.DotNet2.Utilities.Common;
using YanZhiwei.DotNet2.Utilities.WinForm;
using YanZhiwei.DotNet2.Utilities.WinForm.Core;

namespace YanZhiwei.DotNet2.Utilities.WinFromExamples
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //winGridExamle _winExample = new winGridExamle();
            //_winExample.ShowDialog();
            FormHelper.ShowDialogForm<FormGridExamle>();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ApplicationHelper.CapturedExit<FormMain>(this, () =>
            {
                if (MessageBox.Show("确认退出？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    return true;

                return false;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormCheckedListBox>();
            //winCheckedListBox _winExample = new winCheckedListBox();
            //_winExample.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormTreeView>();
            //winTreeView _winExample = new winTreeView();
            //_winExample.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            throw new System.Exception("测试:" + DateTime.Now.FormatDate(1));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ApplicationHelper.GetExecuteDirectory());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ApplicationHelper.ToFullScreen(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormSerialPortExamle>();
            //winSerialPortExamle _winExample = new winSerialPortExamle();
            //_winExample.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormProgressBarExample>();
            //winProgressBarExample _winExample = new winProgressBarExample();
            //_winExample.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormLanguageExample>();
            //winLanguageExample _winExample = new winLanguageExample();
            //_winExample.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            MessageBoxTimeOut _messageBox = new MessageBoxTimeOut();
            _messageBox.Show("测试", "警告");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DataTable _table = new DataTable();
            _table.Columns.Add(new DataColumn("名称", typeof(string)));
            _table.Columns.Add(new DataColumn("年龄", typeof(int)));
            _table.Columns.Add(new DataColumn("出生", typeof(DateTime)));

            for (int i = 0; i < 10; i++)
            {
                DataRow _row = _table.NewRow();
                _row["名称"] = "churenyouzi" + i;
                _row["年龄"] = i;
                _row["出生"] = DateTime.Now.AddYears(-20);
                _table.Rows.Add(_row);
            }

            MyxlsExcel.ToExecel(_table, @"D:\myxlsTest.xls", "信息一览");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormUnPackageData>();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormXmlSectionWrapper>();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            FormHelper.ShowDialogForm<FormXmlNodeWrapper>();
        }
    }
}