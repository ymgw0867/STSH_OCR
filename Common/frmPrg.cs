using System;
using System.Windows.Forms;

namespace STSH_OCR.Common
{
    public partial class frmPrg : Form
    {
        public frmPrg()
        {
            InitializeComponent();
        }

        private void frmPrg_Load(object sender, EventArgs e)
        {
            prgBar.Maximum = 100;
            prgBar.Minimum = 0;
        }

        public int progressValue { get; set; }

        public void ProgressStep()
        {
            prgBar.Value = progressValue;
        }

        private void frmPrg_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
