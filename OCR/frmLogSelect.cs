using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STSH_OCR.Common;

namespace STSH_OCR.OCR
{
    public partial class frmLogSelect : Form
    {
        public frmLogSelect()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 閉じる
            Close();
        }

        private void frmLogSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片づけ
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            frmEditLogRep logRep = new frmEditLogRep();
            logRep.ShowDialog();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            frmCsvOutLog outLog = new frmCsvOutLog();
            outLog.ShowDialog();
            Show();
        }

        private void frmLogSelect_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, Width, Height);
            Utility.WindowsMinSize(this, Width, Height);
        }
    }
}
