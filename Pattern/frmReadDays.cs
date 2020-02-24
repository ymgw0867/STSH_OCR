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

namespace STSH_OCR.Pattern
{
    public partial class frmReadDays : Form
    {
        public frmReadDays()
        {
            InitializeComponent();
        }

        public bool MyStatus { get; set; }
        public int MyProperty { get; set; }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("選択中の登録商品のリード日数を一律に入力した日数に変更します。" + Environment.NewLine + "よろしいですか？", "確認", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            MyStatus = true;
            MyProperty = Utility.StrtoInt(textBox1.Text);

            // フォームを閉じる
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // フォームを閉じる
            MyStatus = false;
            MyProperty = 0;

            Close();
        }
    }
}
