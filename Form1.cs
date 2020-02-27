using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
//using Oracle.ManagedDataAccess.Client;
using STSH_OCR.Pattern;
using STSH_OCR.OCR;
using STSH_OCR.Common;

namespace STSH_OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // FAX発注書データ
        Table<Common.ClsFaxOrder> tblFax = null;
        ClsFaxOrder ClsFaxOrder = null;

        // FAX発注書保留データ
        Table<Common.ClsHoldFax> tblHold = null;
        ClsHoldFax ClsHoldFax = null;

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
            //using (var Conn = new OracleConnection())
            //{
            //    Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
            //    Conn.Open();

            //    // 商品マスターを読み込み
            //    string strSQL = "SELECT SYOHIN_CD, SYOHIN_NM FROM M_SYOHIN ";

            //    // using句を使用しないパターン
            //    //OracleCommand Cmd = new OracleCommand(strSQL, Conn);
            //    //OracleDataReader dR = Cmd.ExecuteReader();
            //    //while (dR.Read())
            //    //{
            //    //    MessageBox.Show(dR["SYOHIN_CD"].ToString() + ":" + dR["SYOHIN_NM"].ToString());
            //    //}

            //    //dR.Dispose();
            //    //Cmd.Dispose();

            //    // using句を使用
            //    using (OracleCommand Cmd = new OracleCommand(strSQL, Conn))
            //    {
            //        using (OracleDataReader dR = Cmd.ExecuteReader())
            //        {
            //            while (dR.Read())
            //            {
            //                MessageBox.Show(dR["SYOHIN_CD"].ToString() + ":" + dR["SYOHIN_NM"].ToString());
            //            }
            //        }
            //    }


            //    // 得意先マスターを読み込み
            //    strSQL = "SELECT TOKUISAKI_CD, TOKUISAKI_NM FROM M_TOKUISAKI ";

            //    // using句を使用
            //    using (OracleCommand Cmd = new OracleCommand(strSQL, Conn))
            //    {
            //        using (OracleDataReader dR = Cmd.ExecuteReader())
            //        {
            //            while (dR.Read())
            //            {
            //                MessageBox.Show(dR["TOKUISAKI_CD"].ToString() + ":" + dR["TOKUISAKI_NM"].ToString());
            //            }
            //        }
            //    }
            //}
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

        private void button3_Click(object sender, EventArgs e)
        {
            // データ作成処理へ
            Hide();
            frmCorrect frm = new frmCorrect(string.Empty);
            frm.ShowDialog();
            Show();
            //KintaiData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ローカルマスター接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            tblFax = context.GetTable<Common.ClsFaxOrder>();
            tblHold = context.GetTable<Common.ClsHoldFax>();
        }

        private void KintaiData()
        {
            // 自らのロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.DataPath, Properties.Settings.Default.lockFileName);

            //他のPCで処理中の場合、続行不可
            if (Utility.existsLockFile(Properties.Settings.Default.DataPath))
            {
                MessageBox.Show("他のPCで処理中です。しばらくおまちください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // タイマー監視受信した出勤簿件数
            int s = System.IO.Directory.GetFiles(Properties.Settings.Default.DataPath, "*.tif").Count();

            // 処理可能なデータが存在するか？
            if (tblFax.Count() == 0 && tblHold.Count() == 0)
            {
                MessageBox.Show("現在、処理可能なＦＡＸ発注書データはありません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //LOCKファイル作成
            Utility.makeLockFile(Properties.Settings.Default.DataPath, Properties.Settings.Default.lockFileName);

            this.Hide();

            // 処理するデータを取得
            frmFaxSelect frmFax = new frmFaxSelect();
            frmFax.ShowDialog();

            int _myCnt = frmFax.myCnt;
            bool _myBool = frmFax.myBool;
            frmFax.Dispose();

            // ロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.DataPath, Properties.Settings.Default.lockFileName);

            if (!_myBool)
            {
                Show();
            }
            else
            {
                // データ作成処理へ
                frmCorrect frm = new frmCorrect(string.Empty);
                frm.ShowDialog();
                Show();
            }
        }

    }
}
