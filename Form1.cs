using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using STSH_OCR.Pattern;

namespace STSH_OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 環境設定
            Hide();
            Form frm = new Config.frmConfig();
            frm.ShowDialog();
            Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // メニューを閉じる
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Hide();
            Form frm = new Master.frmComment();
            frm.ShowDialog();
            Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            using (var Conn = new OracleConnection())
            {
                Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
                Conn.Open();

                // 商品マスターを読み込み
                string strSQL = "SELECT SYOHIN_CD, SYOHIN_NM FROM M_SYOHIN ";

                // using句を使用しないパターン
                //OracleCommand Cmd = new OracleCommand(strSQL, Conn);
                //OracleDataReader dR = Cmd.ExecuteReader();
                //while (dR.Read())
                //{
                //    MessageBox.Show(dR["SYOHIN_CD"].ToString() + ":" + dR["SYOHIN_NM"].ToString());
                //}

                //dR.Dispose();
                //Cmd.Dispose();

                // using句を使用
                using (OracleCommand Cmd = new OracleCommand(strSQL, Conn))
                {
                    using (OracleDataReader dR = Cmd.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            MessageBox.Show(dR["SYOHIN_CD"].ToString() + ":" + dR["SYOHIN_NM"].ToString());
                        }
                    }
                }


                // 得意先マスターを読み込み
                strSQL = "SELECT TOKUISAKI_CD, TOKUISAKI_NM FROM M_TOKUISAKI ";

                // using句を使用
                using (OracleCommand Cmd = new OracleCommand(strSQL, Conn))
                {
                    using (OracleDataReader dR = Cmd.ExecuteReader())
                    {
                        while (dR.Read())
                        {
                            MessageBox.Show(dR["TOKUISAKI_CD"].ToString() + ":" + dR["TOKUISAKI_NM"].ToString());
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPtnAdd frm = new frmPtnAdd();
            frm.ShowDialog();
            Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmPrnPtn frm = new frmPrnPtn();
            frm.ShowDialog();
            Show();
        }
    }
}
