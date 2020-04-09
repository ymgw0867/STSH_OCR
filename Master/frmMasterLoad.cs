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

namespace STSH_OCR.Master
{
    public partial class frmMasterLoad : Form
    {
        public frmMasterLoad()
        {
            InitializeComponent();
        }

        public int MyProperty { get; set; }

        // 商品マスタークラス配列
        public ClsCsvData.ClsCsvSyohin_New[] syohin_News = null;

        // 得意先マスタークラス配列
        public ClsCsvData.ClsCsvTokuisaki[] tokuisakis = null;

        private void frmMasterLoad_Load(object sender, EventArgs e)
        {
        }
        private DataTable readCSV()
        {
            //パスの設定
            string path = "CSVファイルのパス";

            //StreamReaderクラスのインスタンスの作成
            System.IO.StreamReader sr = new System.IO.StreamReader(path);

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

            return dt;

        }

        ///-------------------------------------------------------------------
        /// <summary>
        ///     商品情報配列取得 </summary>
        /// <returns>
        ///     clsCsvSyohinクラス配列</returns>
        ///-------------------------------------------------------------------
        private ClsCsvData.ClsCsvSyohin_New[] GetSyohinData()
        {
            button1.Text = "商品マスターを読み込み中です・・・";
            System.Threading.Thread.Sleep(10);
            Application.DoEvents();

            Cursor = Cursors.WaitCursor;

            ClsCsvData.ClsCsvSyohin_New[] syohins = null;

            try
            {
                // 商品CSVデータ配列読み込み
                string[] Sy_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.商品マスター, Encoding.Default);

                int toDate = 0;
                int x = 0;
                int cnt = 0;
                bool Syubai = false;
                int totalCnt = Sy_Array.Length;

                // プログレスバー初期化
                progressBar1.Minimum = 0;
                progressBar1.Maximum = totalCnt;
                progressBar1.Value = 0;

                foreach (var item in Sy_Array)
                {
                    cnt++;
                    progressBar1.Value = cnt;

                    string[] t = item.Split(',');
                    //string cStart_Sale_YMD = "";      // 商品販売開始日付
                    string cLast_Sale_YMD = "";         // 商品販売終了日付

                    // 削除フラグ
                    string Header = t[0].Replace("\"", "");

                    // 1行目見出し行は読み飛ばす
                    if (Header == "SYOHIN_CD")
                    {
                        continue;
                    }

                    //// リストビューに表示
                    //listBox1.Items.Add(t[0].Replace("\"", "").PadLeft(8, '0') + " " + t[1].Replace("\"", ""));
                    //listBox1.TopIndex = listBox1.Items.Count - 1;

                    //// プログレスバー
                    ////System.Threading.Thread.Sleep(10);
                    //Application.DoEvents();

                    // 有効開始日、有効終了日で終売を調べる
                    //cStart_Sale_YMD = t[9].Replace("\"", "");   // 商品販売開始日付
                    cLast_Sale_YMD = t[10].Replace("\"", "");   // 商品販売終了日付（終売日）

                    toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cLast_Sale_YMD) > 0)
                    {
                        if (toDate > Utility.StrtoInt(cLast_Sale_YMD))
                        {
                            // 終売商品
                            Syubai = true;
                        }
                        else
                        {
                            // 終売ではない
                            Syubai = false;
                        }
                    }

                    Array.Resize(ref syohins, x + 1);

                    // 返り値
                    syohins[x] = new ClsCsvData.ClsCsvSyohin_New
                    {
                        SYOHIN_CD = t[0].Replace("\"", ""),
                        SYOHIN_NM = t[1].Replace("\"", ""),
                        SYOHIN_SNM = t[2].Replace("\"", ""),
                        SIRESAKI_CD = t[3].Replace("\"", ""),
                        SIRESAKI_NM = t[4].Replace("\"", ""),
                        SIRESAKI_KANA_NM = t[5].Replace("\"", ""),
                        JAN_CD = t[6].Replace("\"", ""),
                        SYOHIN_KIKAKU = t[7].Replace("\"", ""),
                        CASE_IRISU = Utility.StrtoDouble(t[8].Replace("\"", "")),
                        START_SALE_YMD = t[9].Replace("\"", ""),
                        LAST_SALE_YMD = t[10].Replace("\"", ""),
                        SHUBAI = Syubai,
                        SYOHIN_KIND_L_CD = t[12].Replace("\"", ""),
                        SYOHIN_KIND_M_CD = t[13].Replace("\"", ""),
                        SYOHIN_KIND_S_CD = t[14].Replace("\"", ""),
                        SYOHIN_KIND_CD = t[15].Replace("\"", "")
                    };

                    x++;
                }

                System.Threading.Thread.Sleep(10);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor = Cursors.Default;
            return syohins;
        }


        ///-----------------------------------------------------------------
        /// <summary>
        ///     得意先クラス配列作成 : 2020/04/08</summary>
        /// <returns>
        ///     ClsCsvTokuisakiクラス配列</returns>
        ///-----------------------------------------------------------------
        private ClsCsvData.ClsCsvTokuisaki[] GetTokuisakis()
        {
            button1.Text = "得意先マスターを読み込み中です・・・";
            System.Threading.Thread.Sleep(10);
            Application.DoEvents();

            Cursor = Cursors.WaitCursor;

            ClsCsvData.ClsCsvTokuisaki[] tokuisakis = null;

            try
            {
                // 得意先CSVデータ配列読み込み
                string[] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);

                int x = 0;
                int cnt = 0;
                int totalCnt = Tk_Array.Length;

                // プログレスバー初期化
                progressBar2.Minimum = 0;
                progressBar2.Maximum = totalCnt;
                progressBar2.Value = 0;

                foreach (var item in Tk_Array)
                {
                    cnt++;
                    progressBar2.Value = cnt;

                    string[] t = item.Split(',');

                    // 削除フラグ
                    string DelFlg = t[10].Replace("\"", "");

                    // 1行目見出し行は読み飛ばす
                    if (DelFlg == "DELFLG")
                    {
                        continue;
                    }

                    if (DelFlg == global.FLGON)
                    {
                        continue;
                    }

                    // 有効開始日、有効終了日を検証する
                    string cYuko_Start_Date = t[1].Replace("\"", "");   // 有効開始日付
                    string cYuko_End_Date = t[2].Replace("\"", "");   // 有効終了日付

                    int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cYuko_Start_Date) > toDate)
                    {
                        continue;
                    }

                    if (toDate > Utility.StrtoInt(cYuko_End_Date))
                    {
                        continue;
                    }

                    Array.Resize(ref tokuisakis, x + 1);

                    tokuisakis[x] = new ClsCsvData.ClsCsvTokuisaki
                    {
                        TOKUISAKI_CD = t[0].Replace("\"", ""),          // 得意先コード   
                        YUKO_START_YMD = t[1].Replace("\"", ""),        // 有効開始日付
                        YUKO_END_YMD = t[2].Replace("\"", ""),          // 有効終了日付                   
                        TOKUISAKI_NM = t[3].Replace("\"", ""),          // 得意先名称                    
                        TOKUISAKI_KANA_NM = t[4].Replace("\"", ""),     // 得意先名称                    
                        TOKUISAKI_YUBIN_NO = t[5].Replace("\"", ""),    // 得意先郵便番号                    
                        TOKUISAKI_ZYUSYO1 = t[6].Replace("\"", ""),     // 得意先住所
                        TOKUISAKI_ZYUSYO2 = t[7].Replace("\"", ""),     // 得意先住所                    
                        TOKUISAKI_TEL = t[8].Replace("\"", ""),         // 得意先TEL                    
                        TOKUISAKI_FAX = t[9].Replace("\"", ""),         // 得意先FAX    
                        DELFLG = t[10].Replace("\"", "")                // 削除フラグ
                    };

                    x++;
                }

                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Cursor = Cursors.Default;
            return tokuisakis;
        }



        private void frmMasterLoad_Shown(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());

            syohin_News = GetSyohinData();  // 商品マスター読み込み

            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());

            tokuisakis = GetTokuisakis();  // 得意先マスター読み込み

            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString());

            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Close();
        }

        private void frmMasterLoad_FormClosing(object sender, FormClosingEventArgs e)
        {
            //// 後片付け
            //Dispose();
        }
    }
}
