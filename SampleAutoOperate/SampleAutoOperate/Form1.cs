using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AutoOperate32;

namespace SampleAutoOperate
{
    public partial class KeySendTool : Form
    {
        AutoOperate32.Operate ope;

        public KeySendTool()
        {
            InitializeComponent();

            ope = new AutoOperate32.Operate();
        }

        private void PrepareSend(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("Sample.txt");
            AutoOperate32.Operate.WaitPWShow("Sample.txt - sakura 2.1.0.0 ", 100, 10);
        }

        private void UpClick(object sender, EventArgs e)
        {
            char[] cmd = new char[1];
            cmd[0] = (char)(0x26);
            string key = new string(cmd);
            ope.SendKeyboardEvent(ope.getPWHandle(), key);
        }

        private void LeftClick(object sender, EventArgs e)
        {
            char[] cmd = new char[1];
            cmd[0] = (char)(0x25);
            string key = new string(cmd);
            ope.SendKeyboardEvent(ope.getPWHandle(), key);
        }

        private void RightClick(object sender, EventArgs e)
        {
            char[] cmd = new char[1];
            cmd[0] = (char)(0x27);
            string key = new string(cmd);
            ope.SendKeyboardEvent(ope.getPWHandle(), key);
        }

        private void DownClick(object sender, EventArgs e)
        {
            char[] cmd = new char[1];
            cmd[0] = (char)(0x28);
            string key = new string(cmd);
            ope.SendKeyboardEvent(ope.getPWHandle(), key);
        }
    }
}
