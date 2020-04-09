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
using STSH_OCR.Master;

namespace STSH_OCR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            timer1.Tick += new EventHandler(timer1_Tick);
        }

        Timer timer1 = new Timer();

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        SQLiteConnection cn2 = null;
        DataContext context2 = null;

        string db_file = Properties.Settings.Default.DB_File;
        string local_DB = Properties.Settings.Default.Local_DB;

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
            // 環境設定データ
            Table<Common.ClsSystemConfig> tblCnf = context.GetTable<Common.ClsSystemConfig>();

            var cnf = tblCnf.Single(a => a.ID == global.configKEY);

            Cursor = Cursors.WaitCursor;

            // 保存期間経過編集ログ、ＣＳＶデータ作成履歴削除
            DeleteDateSpan(cnf.LogSpan);

            // 保存期間経過FAX画像削除
            ImgDeleteSpan(cnf.ImgPath, cnf.DataSpan);

            Cursor = Cursors.Default;

            // メニューを閉じる
            Close();
        }

        ///---------------------------------------------------------------
        /// <summary>
        ///     保存期間経過編集ログ、ＣＳＶデータ作成履歴削除 </summary>
        /// <param name="dM">
        ///     発注書データ保存月数</param>
        /// <param name="dM2">
        ///     編集ログ保存月数</param>
        ///---------------------------------------------------------------
        private void DeleteDateSpan(int dM2)
        {
            cn.Open();

            try
            {
                DateTime sdt = DateTime.Now.AddMonths(-1 * dM2);
                //DateTime sdt = DateTime.Now.AddDays(-1 * dM2); // デバッグ用

                string _sdt = sdt.Year + "/" + sdt.Month.ToString("D2") + "/" + sdt.Day.ToString("D2") + " " +
                              sdt.Hour.ToString("D2") + ":" + sdt.Minute.ToString("D2") + ":" + sdt.Second.ToString("D2");

                // 編集ログ
                string sql = "delete from DataEditLog  ";
                sql += "where 年月日時刻 < '" + _sdt + "'";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                // ＣＳＶデータ作成履歴
                sql = "delete from CsvOutHistory ";
                sql += "where 作成年月日時刻 < '" + _sdt + "'";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                // OrderData_Backup
                sdt = DateTime.Now.AddDays(-1 * Properties.Settings.Default.orderDataBackupDay);
                _sdt = sdt.Year + sdt.Month.ToString("D2") + sdt.Day.ToString("D2") + "235959999";
                sql = "delete from OrderData_Backup ";
                sql += "where ID < '" + _sdt + "'";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        ///-------------------------------------------------------------------
        /// <summary>
        ///     保存期間経過FAX画像削除 </summary>
        /// <param name="dM">
        ///     保存月数</param>
        ///-------------------------------------------------------------------
        private void ImgDeleteSpan(string imgPath, int dM)
        {
            try
            {
                // 削除基準日付
                DateTime sdt = DateTime.Today.AddMonths(-1 * dM);

                double _sdt = Utility.StrtoDouble(sdt.Year + sdt.Month.ToString("D2") + sdt.Day.ToString("D2") + "235959999");

                // フォルダ毎に中身のファイルを見る
                foreach (var dir in System.IO.Directory.GetDirectories(imgPath))
                {
                    foreach (var file in System.IO.Directory.GetFiles(dir, "*.tif"))
                    {
                        double fnm = Utility.StrtoDouble(System.IO.Path.GetFileNameWithoutExtension(file));

                        if (fnm < _sdt)
                        {
                            // 画像削除
                            System.IO.File.Delete(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Hide();
            frmMasterLoad frm = new frmMasterLoad();
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
            //Hide();
            //frmCorrect frm = new frmCorrect(string.Empty);
            //frm.ShowDialog();
            //Show();

            KintaiData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // キャプションにバージョンを追加
            this.Text += "   ver " + Application.ProductVersion;

            timer1.Enabled = false;
            dCountShow();   // 件数表示
            timer1.Enabled = true;

            //// 得意先CSVデータ配列読み込み
            //global.gl_Tokuisaki = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);

            // CSVデータをDataSetに読み込む : 2020/04/09
            global.dtSyohin = readCSV(Properties.Settings.Default.商品マスター);
            global.dtTokuisaki = readCSV(Properties.Settings.Default.得意先マスター);

            // データセットにデータテーブルを追加 : 2020/04/09
            global.DataSet = new DataSet();
            global.DataSet.Tables.Add(global.dtSyohin);
            global.DataSet.Tables.Add(global.dtTokuisaki);

            // データベース接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            tblHold = context.GetTable<Common.ClsHoldFax>();

            cn2 = new SQLiteConnection("DataSource=" + local_DB);
            context2 = new DataContext(cn2);
            tblFax = context2.GetTable<Common.ClsFaxOrder>();

            // orderpattern にコメントフィールド追加：2020/04/01
            DbAlterTable();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     ＣＳＶデータからデータテーブルを生成する </summary>
        /// <param name="sPath">
        ///     CSVデータファイルパス</param>
        /// <returns>
        ///     データテーブル</returns>
        ///---------------------------------------------------------
        private DataTable readCSV(string sPath)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());

            ////パスの設定
            //string path = "CSVファイルのパス";

            //StreamReaderクラスのインスタンスの作成
            System.IO.StreamReader sr = new System.IO.StreamReader(sPath, Encoding.Default);

            //DataTableクラスのインスタンスの作成
            DataTable dt = new DataTable();

            //1行目を区切り文字(カンマ)で分割し列名を取得
            string[] items = sr.ReadLine().Split(',');

            //列の作成
            foreach (string item in items)
            {
                dt.Columns.Add(item, typeof(string));
            }

            //各行を読込み、テーブルを作成
            while (sr.Peek() != -1)
            {
                string[] values = sr.ReadLine().Split(',');

                DataRow dr = dt.NewRow();

                for (int ii = 0; ii < items.Length; ii++)
                {
                    dr[items[ii]] = values[ii];
                }

                dt.Rows.Add(dr);
            }

            //StreamReaderクラスのインスタンスの破棄
            sr.Close();

            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());

            return dt;
        }

        private void DbAlterTable()
        {
            cn.Open();
            string sql = "";

            try
            {
                sql = "ALTER TABLE orderpattern ADD COLUMN Comment1 TEXT";
                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                sql = "ALTER TABLE orderpattern ADD COLUMN Comment2 TEXT";
                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                sql = "ALTER TABLE orderpattern ADD COLUMN Comment3 TEXT";
                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                sql = "ALTER TABLE orderpattern ADD COLUMN Comment4 TEXT";
                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // 何もしない
            }

            cn.Close();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            dCountShow();   // 件数表示
            timer1.Enabled = true;
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
            // OCR認識データ
            bool _ocrFile = false;
            foreach (var dir in System.IO.Directory.GetDirectories(Properties.Settings.Default.DataPath))
            {
                if (System.IO.Directory.GetFiles(dir, "*.csv").Any())
                {
                    _ocrFile = true;
                    break;
                }
            } 

            // ローカルデータベース
            if (!_ocrFile && tblFax.Count() == 0 && tblHold.Count() == 0)
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

        private void button2_Click(object sender, EventArgs e)
        {
            // 自らのロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.NgPath, Properties.Settings.Default.lockFileName);

            //他のPCで処理中の場合、続行不可
            if (Utility.existsLockFile(Properties.Settings.Default.NgPath))
            {
                MessageBox.Show("他のPCで処理中です。しばらくおまちください。", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //LOCKファイル作成
            Utility.makeLockFile(Properties.Settings.Default.NgPath, Properties.Settings.Default.lockFileName);

            Hide();
            frmNgRecovery frmNg = new frmNgRecovery();
            frmNg.ShowDialog();

            // ロックファイルを削除する
            Utility.deleteLockFile(Properties.Settings.Default.NgPath, Properties.Settings.Default.lockFileName);

            // NG件数更新 : 2017/11/18
            dCountShow();

            Show();
        }


        ///--------------------------------------------------
        /// <summary>
        ///     NG件数表示 </summary>
        ///--------------------------------------------------
        private void dCountShow()
        {
            try
            {
                // NG件数取得
                int ngCnt = System.IO.Directory.GetFiles(Properties.Settings.Default.NgPath, "*.tif").Count();

                if (ngCnt > 0)
                {
                    button2.Enabled = true;
                    button2.Text = "ＮＧ画像確認 " + "(" + ngCnt + ") (&N)";
                }
                else
                {

                    button2.Enabled = false;
                    button2.Text = "ＮＧ画像なし";
                }
            }
            catch (Exception)
            {
                // 何もしない
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Hide();
            frmLogSelect logRep = new frmLogSelect();
            logRep.ShowDialog();
            Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Hide();
            frmCsvDataCreate frmCsv = new frmCsvDataCreate();
            frmCsv.ShowDialog();
            Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Hide();
            frmOrderIndex frmOrderIndex = new frmOrderIndex();
            frmOrderIndex.ShowDialog();
            Show();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Hide();
            //frmMasterLoad frm = new frmMasterLoad();
            //frm.ShowDialog();

            //if (frm.syohin_News == null)
            //{
            //    Close();
            //}

            //global.syohin_News = frm.syohin_News;
            //global.tokuisakis = frm.tokuisakis;

            //frm.Dispose();
            //Show();
        }
    }
}
