using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using STSH_OCR.Common;
using STSH_OCR.OCR;
using System.Configuration;
//using Oracle.ManagedDataAccess.Client;
using Excel = Microsoft.Office.Interop.Excel;
using OpenCvSharp;

namespace STSH_OCR.OCR
{
    public partial class frmCorrect : Form
    {
        /// ------------------------------------------------------------
        /// <summary>
        ///     コンストラクタ </summary>
        /// <param name="myCode">
        ///     入力担当者コード</param>
        /// ------------------------------------------------------------
        public frmCorrect(string myCode)
        {
            InitializeComponent();

            //_dbName = dbName;       // データベース名
            //_comName = comName;     // 会社名
            //dID = sID;              // 処理モード
            //_eMode = eMode;         // 処理モード2

            _myCode = myCode;       // 担当者コード
        }

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // 発注書パターンマスター
        Table<Common.ClsOrderPattern> tblPtn = null;
        ClsOrderPattern ClsOrderPattern = null;

        // FAX発注書データ
        Table<Common.ClsFaxOrder>tblFax = null;
        ClsFaxOrder ClsFaxOrder = null;

        // FAX発注書保留データ
        Table<Common.ClsHoldFax> tblHold = null;
        ClsHoldFax ClsHoldFax = null;

        // セル値
        private string cellName = string.Empty;         // セル名
        private string cellBeforeValue = string.Empty;  // 編集前
        private string cellAfterValue = string.Empty;   // 編集後

        #region 編集ログ・項目名 2015/09/08
        private const string LOG_YEAR = "年";
        private const string LOG_MONTH = "月";
        private const string LOG_DAY = "日";
        private const string LOG_TAIKEICD = "体系コード";
        private const string CELL_TORIKESHI = "取消";
        private const string CELL_NUMBER = "社員番号";
        private const string CELL_KIGOU = "記号";
        private const string CELL_FUTSU = "普通残業・時";
        private const string CELL_FUTSU_M = "普通残業・分";
        private const string CELL_SHINYA = "深夜残業・時";
        private const string CELL_SHINYA_M = "深夜残業・分";
        private const string CELL_SHIGYO = "始業時刻・時";
        private const string CELL_SHIGYO_M = "始業時刻・分";
        private const string CELL_SHUUGYO = "終業時刻・時";
        private const string CELL_SHUUGYO_M = "終業時刻・分";
        #endregion 編集ログ・項目名

        // カレント社員情報
        //SCCSDataSet.社員所属Row cSR = null;
        
        // 社員マスターより取得した所属コード
        string mSzCode = string.Empty;

        #region 終了ステータス定数
        const string END_BUTTON = "btn";
        const string END_MAKEDATA = "data";
        const string END_CONTOROL = "close";
        const string END_NODATA = "non Data";
        #endregion

        string dID = string.Empty;                  // 表示する過去データのID
        string sDBNM = string.Empty;                // データベース名

        string _dbName = string.Empty;           // 会社領域データベース識別番号
        string _comNo = string.Empty;            // 会社番号
        string _comName = string.Empty;          // 会社名
        string _myCode = string.Empty;           // 担当者コード
        string _imgFile = string.Empty;         // 画像名

        bool _eMode = true;

        // dataGridView1_CellEnterステータス
        bool gridViewCellEnterStatus = true;

        // 編集ログ書き込み状態
        bool editLogStatus = false;
        
        // カレントデータRowsインデックス
        string [] cID = null;
        int cI = 0;

        // グローバルクラス
        global gl = new global();

        //OracleConnection Conn = new OracleConnection();

        // 画面表示時ステータス
        bool showStatus = false;

        int fCnt = 0;   // データ件数
        
        string _img = string.Empty;

        // openCvSharp 関連
        const float B_WIDTH = 0.45f;
        const float B_HEIGHT = 0.45f;
        float n_width = 0f;
        float n_height = 0f;

        Mat mMat = new Mat();

        // カラム定義
        private readonly string colHinCode = "c0";
        private readonly string colIrisu = "c1";
        private readonly string colKikaku = "c2";
        private readonly string colNouka = "c3";
        private readonly string colBaika = "c4";
        private readonly string colMaker = "c5";
        private readonly string colDay1 = "c6";
        private readonly string colDay2 = "c7";
        private readonly string colDay3 = "c8";
        private readonly string colDay4 = "c9";
        private readonly string colDay5 = "c10";
        private readonly string colDay6 = "c11";
        private readonly string colDay7 = "c12";
        private readonly string colSyubai = "c13";

        private void frmCorrect_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            //// Tabキーの既定のショートカットキーを解除する。
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow3.ShortcutKeyManager.Unregister(Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Unregister(Keys.Enter);
            //gcMultiRow2.ShortcutKeyManager.Unregister(Keys.Enter);
            //gcMultiRow3.ShortcutKeyManager.Unregister(Keys.Enter);

            //// Tabキーのショートカットキーにユーザー定義のショートカットキーを割り当てる。
            //gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            //gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            //gcMultiRow3.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Tab);
            //gcMultiRow1.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            //gcMultiRow2.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);
            //gcMultiRow3.ShortcutKeyManager.Register(new clsKeyTab.CustomMoveToNextContorol(), Keys.Enter);

            // ローカルマスター接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            tblFax = context.GetTable<Common.ClsFaxOrder>();        // ＦＡＸ発注書テーブル
            tblPtn = context.GetTable<Common.ClsOrderPattern>();    // 登録パターンテーブル

            // データ登録
            if (dID == string.Empty)
            {
                // CSVデータをローカルマスターへ読み込みます
                GetCsvDataToSQLite();

                // データテーブル件数カウント
                if (tblFax.Count() == 0)
                {
                    MessageBox.Show("ＦＡＸ発注書データがありません", "発注書登録", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    //終了処理
                    Environment.Exit(0);
                }

                // キー配列作成
                keyArrayCreate();
            }
            
            // キャプション
            this.Text = "ＦＡＸ発注書表示";

            // GCMultiRow初期化
            gcMrSetting();

            GridviewSet(dg1);


            // 編集作業、過去データ表示の判断
            if (dID == string.Empty) // パラメータのヘッダIDがないときは編集作業
            {
                // 最初のレコードを表示
                cI = 0;
                showOcrData(cI);
            }

            // tagを初期化
            this.Tag = string.Empty;

            // 現在の表示倍率を初期化
            gl.miMdlZoomRate = 0f;
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     キー配列作成 </summary>
        ///-------------------------------------------------------------
        private void keyArrayCreate()
        {
            int iX = 0;
            foreach (var t in tblFax.OrderBy(a => a.ID))
            {
                Array.Resize(ref cID, iX + 1);
                cID[iX] = t.ID;
                iX++;
            }
        }

        #region データグリッドビューカラム定義
        private static string cCheck = "col1";      // 取消
        private static string cShainNum = "col2";   // 社員番号
        private static string cName = "col3";       // 氏名
        private static string cKinmu = "col4";      // 勤務記号
        private static string cZH = "col5";         // 残業時
        private static string cZE = "col6";         // :
        private static string cZM = "col7";         // 残業分
        private static string cSIH = "col8";        // 深夜時
        private static string cSIE = "col9";        // :
        private static string cSIM = "col10";       // 深夜分
        private static string cSH = "col11";        // 開始時
        private static string cSE = "col12";        // :
        private static string cSM = "col13";        // 開始分
        private static string cEH = "col14";        // 終了時
        private static string cEE = "col15";        // :
        private static string cEM = "col16";        // 終了分
        //private static string cID = "colID";        // ID
        private static string cSzCode = "colSzCode";  // 所属コード
        private static string cSzName = "colSzName";  // 所属名

        #endregion

        private void gcMrSetting()
        {
            ////multirow編集モード
            //gcMultiRow1.EditMode = EditMode.EditProgrammatically;

            //this.gcMultiRow1.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            //this.gcMultiRow1.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            ////this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            //this.gcMultiRow1.RowCount = 1;                                  // 行数を設定
            //this.gcMultiRow1.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

            ////multirow編集モード
            //gcMultiRow2.EditMode = EditMode.EditProgrammatically;

            //this.gcMultiRow2.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            //this.gcMultiRow2.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            ////this.gcMultiRow2.Rows.Clear();                                  // 行数をクリア
            //this.gcMultiRow2.RowCount = global.MAX_GYO;                                  // 行数を設定
            //this.gcMultiRow2.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする
            
            ////multirow編集モード
            //gcMultiRow3.EditMode = EditMode.EditProgrammatically;

            //this.gcMultiRow3.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            //this.gcMultiRow3.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            ////this.gcMultiRow3.Rows.Clear();                                  // 行数をクリア
            //this.gcMultiRow3.RowCount = 5;                                  // 行数を設定
            //this.gcMultiRow3.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     発注書登録商品データグリッドビュー定義 </summary>
        ///------------------------------------------------------------------------
        private void GridviewSet(DataGridViewEx tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.Lavender;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", (float)(8.25), FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                //tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 618;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colMaker, "メーカー／商品名");
                tempDGV.Columns.Add(colKikaku, "規格");
                tempDGV.Columns.Add(colIrisu, "入数");
                tempDGV.Columns.Add(colHinCode, "商品CD");
                tempDGV.Columns.Add(colNouka, "納価");
                tempDGV.Columns.Add(colBaika, "売価");
                tempDGV.Columns.Add(colDay1, "月");
                tempDGV.Columns.Add(colDay2, "火");
                tempDGV.Columns.Add(colDay3, "水");
                tempDGV.Columns.Add(colDay4, "木");
                tempDGV.Columns.Add(colDay5, "金");
                tempDGV.Columns.Add(colDay6, "土");
                tempDGV.Columns.Add(colDay7, "日");
                tempDGV.Columns.Add(colSyubai, "終売");

                tempDGV.Columns[colMaker].Width = 210;
                tempDGV.Columns[colKikaku].Width = 70;
                tempDGV.Columns[colIrisu].Width = 40;
                tempDGV.Columns[colHinCode].Width = 70;
                tempDGV.Columns[colNouka].Width = 50;
                tempDGV.Columns[colBaika].Width = 50;
                tempDGV.Columns[colDay1].Width = 40;
                tempDGV.Columns[colDay2].Width = 40;
                tempDGV.Columns[colDay3].Width = 40;
                tempDGV.Columns[colDay4].Width = 40;
                tempDGV.Columns[colDay5].Width = 40;
                tempDGV.Columns[colDay6].Width = 40;
                tempDGV.Columns[colDay7].Width = 40;
                tempDGV.Columns[colSyubai].Width = 50;

                //tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colMaker].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                tempDGV.Columns[colKikaku].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colIrisu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colNouka].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colBaika].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colDay7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = false;

                // 列ごとの設定
                foreach (DataGridViewColumn c in tempDGV.Columns)
                {
                    // 編集可否
                    if (c.Name == colMaker || c.Name == colKikaku || c.Name == colIrisu )
                    {
                        c.ReadOnly = true;
                    }
                    else
                    {
                        c.ReadOnly = false;
                    }

                    // フォントサイズ
                    if (c.Name == colMaker)
                    {
                        c.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", (float)(9.5), FontStyle.Regular);
                    }
                    else if (c.Name == colDay1 || c.Name == colDay2 || c.Name == colDay3 || c.Name == colDay4 || 
                             c.Name == colDay5 || c.Name == colDay6 || c.Name == colDay7)
                    {
                        c.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 11, FontStyle.Regular);
                    }
                    else if (c.Name == colSyubai)
                    {
                        c.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);
                    }
                }

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

                // 編集モード
                tempDGV.EditMode = DataGridViewEditMode.EditOnEnter;

                // 追加行表示しない
                tempDGV.AllowUserToAddRows = false;

                // データグリッドビューから行削除を禁止する
                tempDGV.AllowUserToDeleteRows = false;

                // 手動による列移動の禁止
                tempDGV.AllowUserToOrderColumns = false;

                // 列サイズ変更禁止
                tempDGV.AllowUserToResizeColumns = true;

                // 行サイズ変更禁止
                tempDGV.AllowUserToResizeRows = false;

                //TAB動作
                tempDGV.StandardTab = false;

                // Enter次行移動先カラム
                global.NEXT_COLUMN = colHinCode;

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // ソート不可
                foreach (DataGridViewColumn c in dg1.Columns)
                {
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // 罫線
                //tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                //tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;

                // コンテキストメニュー
                //tempDGV.ContextMenuStrip = this.contextMenuStrip1;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///----------------------------------------------------------------------------
        /// <summary>
        ///     CSVデータをMDBへインサートする</summary>
        ///----------------------------------------------------------------------------
        private void GetCsvDataToSQLite()
        {
            // CSVファイル数をカウント
            string[] inCsv = System.IO.Directory.GetFiles(Properties.Settings.Default.MyDataPath, "*.csv");

            // CSVファイルがなければ終了
            if (inCsv.Length == 0)
            {
                return;
            }

            // オーナーフォームを無効にする
            this.Enabled = false;

            //プログレスバーを表示する
            frmPrg frmP = new frmPrg();
            frmP.Owner = this;
            frmP.Show();

            // OCRのCSVデータをSQLiteへ取り込む
            CsvToSQLite(Properties.Settings.Default.MyDataPath, frmP);

            // いったんオーナーをアクティブにする
            this.Activate();

            // 進行状況ダイアログを閉じる
            frmP.Close();

            // オーナーのフォームを有効に戻す
            this.Enabled = true;
        }

        private void txtYear_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //if (e.Control is DataGridViewTextBoxEditingControl)
            //{
            //    // 数字のみ入力可能とする
            //    if (dGV.CurrentCell.ColumnIndex != 0 && dGV.CurrentCell.ColumnIndex != 2)
            //    {
            //        //イベントハンドラが複数回追加されてしまうので最初に削除する
            //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress2);

            //        //イベントハンドラを追加する
            //        e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            //    }
            //}
        }

        void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        void Control_KeyPress2(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                e.KeyChar == '\b' || e.KeyChar == '\t')
                e.Handled = false;
            else e.Handled = true;
        }

        void Control_KeyPress3(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '0' && e.KeyChar != '5' && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void frmCorrect_Shown(object sender, EventArgs e)
        {
            //if (dID != string.Empty) lnkRtn.Focus();
        }

        private void dataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                //イベントハンドラを追加する
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            }
        }

        private void dataGridView4_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                //イベントハンドラを追加する
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
        }

        ///-----------------------------------------------------------------------------------
        /// <summary>
        ///     カレントデータを更新する</summary>
        /// <param name="iX">
        ///     カレントレコードのインデックス</param>
        ///-----------------------------------------------------------------------------------
        private void CurDataUpDate(string iX)
        {
            // エラーメッセージ
            string errMsg = "ＦＡＸ発注書テーブル更新";

            try
            {
                // ＦＡＸ発注書テーブルセット更新
                ClsFaxOrder.TokuisakiCode = Utility.StrtoInt(Utility.NulltoStr(txtTokuisakiCD.Text));
                ClsFaxOrder.patternID = Utility.StrtoInt(Utility.NulltoStr(txtPID.Text));
                ClsFaxOrder.SeqNumber = Utility.StrtoInt(Utility.NulltoStr(txtSeqNum.Text));
                ClsFaxOrder.Year = Utility.StrtoInt(Utility.NulltoStr(txtYear.Text));
                ClsFaxOrder.Month = Utility.StrtoInt(Utility.NulltoStr(txtMonth.Text));

                ClsFaxOrder.Day1 = Utility.NulltoStr(txtTenDay1.Text);
                ClsFaxOrder.Day2 = Utility.NulltoStr(txtTenDay2.Text);
                ClsFaxOrder.Day3 = Utility.NulltoStr(txtTenDay3.Text);
                ClsFaxOrder.Day4 = Utility.NulltoStr(txtTenDay4.Text);
                ClsFaxOrder.Day5 = Utility.NulltoStr(txtTenDay5.Text);
                ClsFaxOrder.Day6 = Utility.NulltoStr(txtTenDay6.Text);
                ClsFaxOrder.Day7 = Utility.NulltoStr(txtTenDay7.Text);

                ClsFaxOrder.memo = txtMemo.Text;
                ClsFaxOrder.Veri = Convert.ToInt32(checkBox1.Checked);

                // 商品１
                ClsFaxOrder.G_Code1 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 1].Value));
                ClsFaxOrder.G_Nouka1 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 1].Value));
                ClsFaxOrder.G_Baika1 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 1].Value));

                ClsFaxOrder.Goods1_1 = Utility.NulltoStr(dg1[colDay1, 1].Value);
                ClsFaxOrder.Goods1_2 = Utility.NulltoStr(dg1[colDay2, 1].Value);
                ClsFaxOrder.Goods1_3 = Utility.NulltoStr(dg1[colDay3, 1].Value);
                ClsFaxOrder.Goods1_4 = Utility.NulltoStr(dg1[colDay4, 1].Value);
                ClsFaxOrder.Goods1_5 = Utility.NulltoStr(dg1[colDay5, 1].Value);
                ClsFaxOrder.Goods1_6 = Utility.NulltoStr(dg1[colDay6, 1].Value);
                ClsFaxOrder.Goods1_7 = Utility.NulltoStr(dg1[colDay7, 1].Value);

                ClsFaxOrder.G_Syubai1 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 1].Value));
                
                //DataGridViewComboBoxCell boxCell = (DataGridViewComboBoxCell)dg1[colSyubai, 1];
                
                //if (boxCell != null)
                //{
                //    ClsFaxOrder.G_Syubai1 =  boxCell.Items.IndexOf(Utility.NulltoStr(boxCell.Value));
                //}
                //else
                //{
                //    ClsFaxOrder.G_Syubai1 = global.flgOff;
                //}

                // 商品2
                ClsFaxOrder.G_Code2 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 3].Value));
                ClsFaxOrder.G_Nouka2 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 3].Value));
                ClsFaxOrder.G_Baika2 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 3].Value));

                ClsFaxOrder.Goods2_1 = Utility.NulltoStr(dg1[colDay1, 3].Value);
                ClsFaxOrder.Goods2_2 = Utility.NulltoStr(dg1[colDay2, 3].Value);
                ClsFaxOrder.Goods2_3 = Utility.NulltoStr(dg1[colDay3, 3].Value);
                ClsFaxOrder.Goods2_4 = Utility.NulltoStr(dg1[colDay4, 3].Value);
                ClsFaxOrder.Goods2_5 = Utility.NulltoStr(dg1[colDay5, 3].Value);
                ClsFaxOrder.Goods2_6 = Utility.NulltoStr(dg1[colDay6, 3].Value);
                ClsFaxOrder.Goods2_7 = Utility.NulltoStr(dg1[colDay7, 3].Value);

                ClsFaxOrder.G_Syubai2 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 3].Value));

                // 商品3
                ClsFaxOrder.G_Code3 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 5].Value));
                ClsFaxOrder.G_Nouka3 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 5].Value));
                ClsFaxOrder.G_Baika3 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 5].Value));

                ClsFaxOrder.Goods3_1 = Utility.NulltoStr(dg1[colDay1, 5].Value);
                ClsFaxOrder.Goods3_2 = Utility.NulltoStr(dg1[colDay2, 5].Value);
                ClsFaxOrder.Goods3_3 = Utility.NulltoStr(dg1[colDay3, 5].Value);
                ClsFaxOrder.Goods3_4 = Utility.NulltoStr(dg1[colDay4, 5].Value);
                ClsFaxOrder.Goods3_5 = Utility.NulltoStr(dg1[colDay5, 5].Value);
                ClsFaxOrder.Goods3_6 = Utility.NulltoStr(dg1[colDay6, 5].Value);
                ClsFaxOrder.Goods3_7 = Utility.NulltoStr(dg1[colDay7, 5].Value);

                ClsFaxOrder.G_Syubai3 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 5].Value));

                // 商品４
                ClsFaxOrder.G_Code4 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 7].Value));
                ClsFaxOrder.G_Nouka4 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 7].Value));
                ClsFaxOrder.G_Baika4 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 7].Value));

                ClsFaxOrder.Goods4_1 = Utility.NulltoStr(dg1[colDay1, 7].Value);
                ClsFaxOrder.Goods4_2 = Utility.NulltoStr(dg1[colDay2, 7].Value);
                ClsFaxOrder.Goods4_3 = Utility.NulltoStr(dg1[colDay3, 7].Value);
                ClsFaxOrder.Goods4_4 = Utility.NulltoStr(dg1[colDay4, 7].Value);
                ClsFaxOrder.Goods4_5 = Utility.NulltoStr(dg1[colDay5, 7].Value);
                ClsFaxOrder.Goods4_6 = Utility.NulltoStr(dg1[colDay6, 7].Value);
                ClsFaxOrder.Goods4_7 = Utility.NulltoStr(dg1[colDay7, 7].Value);

                ClsFaxOrder.G_Syubai4 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 7].Value));


                // 商品５
                ClsFaxOrder.G_Code5 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 9].Value));
                ClsFaxOrder.G_Nouka5 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 9].Value));
                ClsFaxOrder.G_Baika5 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 9].Value));

                ClsFaxOrder.Goods5_1 = Utility.NulltoStr(dg1[colDay1, 9].Value);
                ClsFaxOrder.Goods5_2 = Utility.NulltoStr(dg1[colDay2, 9].Value);
                ClsFaxOrder.Goods5_3 = Utility.NulltoStr(dg1[colDay3, 9].Value);
                ClsFaxOrder.Goods5_4 = Utility.NulltoStr(dg1[colDay4, 9].Value);
                ClsFaxOrder.Goods5_5 = Utility.NulltoStr(dg1[colDay5, 9].Value);
                ClsFaxOrder.Goods5_6 = Utility.NulltoStr(dg1[colDay6, 9].Value);
                ClsFaxOrder.Goods5_7 = Utility.NulltoStr(dg1[colDay7, 9].Value);

                ClsFaxOrder.G_Syubai5 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 9].Value));


                // 商品6
                ClsFaxOrder.G_Code6 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 11].Value));
                ClsFaxOrder.G_Nouka6 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 11].Value));
                ClsFaxOrder.G_Baika6 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 11].Value));

                ClsFaxOrder.Goods6_1 = Utility.NulltoStr(dg1[colDay1, 11].Value);
                ClsFaxOrder.Goods6_2 = Utility.NulltoStr(dg1[colDay2, 11].Value);
                ClsFaxOrder.Goods6_3 = Utility.NulltoStr(dg1[colDay3, 11].Value);
                ClsFaxOrder.Goods6_4 = Utility.NulltoStr(dg1[colDay4, 11].Value);
                ClsFaxOrder.Goods6_5 = Utility.NulltoStr(dg1[colDay5, 11].Value);
                ClsFaxOrder.Goods6_6 = Utility.NulltoStr(dg1[colDay6, 11].Value);
                ClsFaxOrder.Goods6_7 = Utility.NulltoStr(dg1[colDay7, 11].Value);

                ClsFaxOrder.G_Syubai6 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 11].Value));


                // 商品7
                ClsFaxOrder.G_Code7  = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 13].Value));
                ClsFaxOrder.G_Nouka7  = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 13].Value));
                ClsFaxOrder.G_Baika7  = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 13].Value));

                ClsFaxOrder.Goods7_1 = Utility.NulltoStr(dg1[colDay1, 13].Value);
                ClsFaxOrder.Goods7_2 = Utility.NulltoStr(dg1[colDay2, 13].Value);
                ClsFaxOrder.Goods7_3 = Utility.NulltoStr(dg1[colDay3, 13].Value);
                ClsFaxOrder.Goods7_4 = Utility.NulltoStr(dg1[colDay4, 13].Value);
                ClsFaxOrder.Goods7_5 = Utility.NulltoStr(dg1[colDay5, 13].Value);
                ClsFaxOrder.Goods7_6 = Utility.NulltoStr(dg1[colDay6, 13].Value);
                ClsFaxOrder.Goods7_7 = Utility.NulltoStr(dg1[colDay7, 13].Value);

                ClsFaxOrder.G_Syubai7 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 13].Value));


                // 商品8
                ClsFaxOrder.G_Code8  = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 15].Value));
                ClsFaxOrder.G_Nouka8  = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 15].Value));
                ClsFaxOrder.G_Baika8  = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 15].Value));

                ClsFaxOrder.Goods8_1 = Utility.NulltoStr(dg1[colDay1, 15].Value);
                ClsFaxOrder.Goods8_2 = Utility.NulltoStr(dg1[colDay2, 15].Value);
                ClsFaxOrder.Goods8_3 = Utility.NulltoStr(dg1[colDay3, 15].Value);
                ClsFaxOrder.Goods8_4 = Utility.NulltoStr(dg1[colDay4, 15].Value);
                ClsFaxOrder.Goods8_5 = Utility.NulltoStr(dg1[colDay5, 15].Value);
                ClsFaxOrder.Goods8_6 = Utility.NulltoStr(dg1[colDay6, 15].Value);
                ClsFaxOrder.Goods8_7 = Utility.NulltoStr(dg1[colDay7, 15].Value);

                ClsFaxOrder.G_Syubai8 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 15].Value));


                // 商品9
                ClsFaxOrder.G_Code9 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 17].Value));
                ClsFaxOrder.G_Nouka9 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 17].Value));
                ClsFaxOrder.G_Baika9 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 17].Value));

                ClsFaxOrder.Goods9_1 = Utility.NulltoStr(dg1[colDay1, 17].Value);
                ClsFaxOrder.Goods9_2 = Utility.NulltoStr(dg1[colDay2, 17].Value);
                ClsFaxOrder.Goods9_3 = Utility.NulltoStr(dg1[colDay3, 17].Value);
                ClsFaxOrder.Goods9_4 = Utility.NulltoStr(dg1[colDay4, 17].Value);
                ClsFaxOrder.Goods9_5 = Utility.NulltoStr(dg1[colDay5, 17].Value);
                ClsFaxOrder.Goods9_6 = Utility.NulltoStr(dg1[colDay6, 17].Value);
                ClsFaxOrder.Goods9_7 = Utility.NulltoStr(dg1[colDay7, 17].Value);

                ClsFaxOrder.G_Syubai9 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 17].Value));


                // 商品10
                ClsFaxOrder.G_Code10 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 19].Value));
                ClsFaxOrder.G_Nouka10 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 19].Value));
                ClsFaxOrder.G_Baika10 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 19].Value));

                ClsFaxOrder.Goods10_1 = Utility.NulltoStr(dg1[colDay1, 19].Value);
                ClsFaxOrder.Goods10_2 = Utility.NulltoStr(dg1[colDay2, 19].Value);
                ClsFaxOrder.Goods10_3 = Utility.NulltoStr(dg1[colDay3, 19].Value);
                ClsFaxOrder.Goods10_4 = Utility.NulltoStr(dg1[colDay4, 19].Value);
                ClsFaxOrder.Goods10_5 = Utility.NulltoStr(dg1[colDay5, 19].Value);
                ClsFaxOrder.Goods10_6 = Utility.NulltoStr(dg1[colDay6, 19].Value);
                ClsFaxOrder.Goods10_7 = Utility.NulltoStr(dg1[colDay7, 19].Value);

                ClsFaxOrder.G_Syubai10 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 19].Value));


                // 商品11
                ClsFaxOrder.G_Code11 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 21].Value));
                ClsFaxOrder.G_Nouka11 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 21].Value));
                ClsFaxOrder.G_Baika11 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 21].Value));

                ClsFaxOrder.Goods11_1 = Utility.NulltoStr(dg1[colDay1, 21].Value);
                ClsFaxOrder.Goods11_2 = Utility.NulltoStr(dg1[colDay2, 21].Value);
                ClsFaxOrder.Goods11_3 = Utility.NulltoStr(dg1[colDay3, 21].Value);
                ClsFaxOrder.Goods11_4 = Utility.NulltoStr(dg1[colDay4, 21].Value);
                ClsFaxOrder.Goods11_5 = Utility.NulltoStr(dg1[colDay5, 21].Value);
                ClsFaxOrder.Goods11_6 = Utility.NulltoStr(dg1[colDay6, 21].Value);
                ClsFaxOrder.Goods11_7 = Utility.NulltoStr(dg1[colDay7, 21].Value);

                ClsFaxOrder.G_Syubai11 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 21].Value));


                // 商品12
                ClsFaxOrder.G_Code12 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 23].Value));
                ClsFaxOrder.G_Nouka12 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 23].Value));
                ClsFaxOrder.G_Baika12 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 23].Value));

                ClsFaxOrder.Goods12_1 = Utility.NulltoStr(dg1[colDay1, 23].Value);
                ClsFaxOrder.Goods12_2 = Utility.NulltoStr(dg1[colDay2, 23].Value);
                ClsFaxOrder.Goods12_3 = Utility.NulltoStr(dg1[colDay3, 23].Value);
                ClsFaxOrder.Goods12_4 = Utility.NulltoStr(dg1[colDay4, 23].Value);
                ClsFaxOrder.Goods12_5 = Utility.NulltoStr(dg1[colDay5, 23].Value);
                ClsFaxOrder.Goods12_6 = Utility.NulltoStr(dg1[colDay6, 23].Value);
                ClsFaxOrder.Goods12_7 = Utility.NulltoStr(dg1[colDay7, 23].Value);

                ClsFaxOrder.G_Syubai12 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 23].Value));


                // 商品13
                ClsFaxOrder.G_Code13 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 25].Value));
                ClsFaxOrder.G_Nouka13 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 25].Value));
                ClsFaxOrder.G_Baika13 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 25].Value));

                ClsFaxOrder.Goods13_1 = Utility.NulltoStr(dg1[colDay1, 25].Value);
                ClsFaxOrder.Goods13_2 = Utility.NulltoStr(dg1[colDay2, 25].Value);
                ClsFaxOrder.Goods13_3 = Utility.NulltoStr(dg1[colDay3, 25].Value);
                ClsFaxOrder.Goods13_4 = Utility.NulltoStr(dg1[colDay4, 25].Value);
                ClsFaxOrder.Goods13_5 = Utility.NulltoStr(dg1[colDay5, 25].Value);
                ClsFaxOrder.Goods13_6 = Utility.NulltoStr(dg1[colDay6, 25].Value);
                ClsFaxOrder.Goods13_7 = Utility.NulltoStr(dg1[colDay7, 25].Value);

                ClsFaxOrder.G_Syubai13 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 25].Value));


                // 商品14
                ClsFaxOrder.G_Code14 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 27].Value));
                ClsFaxOrder.G_Nouka14 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 27].Value));
                ClsFaxOrder.G_Baika14 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 27].Value));

                ClsFaxOrder.Goods14_1 = Utility.NulltoStr(dg1[colDay1, 27].Value);
                ClsFaxOrder.Goods14_2 = Utility.NulltoStr(dg1[colDay2, 27].Value);
                ClsFaxOrder.Goods14_3 = Utility.NulltoStr(dg1[colDay3, 27].Value);
                ClsFaxOrder.Goods14_4 = Utility.NulltoStr(dg1[colDay4, 27].Value);
                ClsFaxOrder.Goods14_5 = Utility.NulltoStr(dg1[colDay5, 27].Value);
                ClsFaxOrder.Goods14_6 = Utility.NulltoStr(dg1[colDay6, 27].Value);
                ClsFaxOrder.Goods14_7 = Utility.NulltoStr(dg1[colDay7, 27].Value);

                ClsFaxOrder.G_Syubai14 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 27].Value));


                // 商品15
                ClsFaxOrder.G_Code15 = SetSyohinCode(Utility.NulltoStr(dg1[colHinCode, 29].Value));
                ClsFaxOrder.G_Nouka15 = Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 29].Value));
                ClsFaxOrder.G_Baika15 = Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 29].Value));

                ClsFaxOrder.Goods15_1 = Utility.NulltoStr(dg1[colDay1, 29].Value);
                ClsFaxOrder.Goods15_2 = Utility.NulltoStr(dg1[colDay2, 29].Value);
                ClsFaxOrder.Goods15_3 = Utility.NulltoStr(dg1[colDay3, 29].Value);
                ClsFaxOrder.Goods15_4 = Utility.NulltoStr(dg1[colDay4, 29].Value);
                ClsFaxOrder.Goods15_5 = Utility.NulltoStr(dg1[colDay5, 29].Value);
                ClsFaxOrder.Goods15_6 = Utility.NulltoStr(dg1[colDay6, 29].Value);
                ClsFaxOrder.Goods15_7 = Utility.NulltoStr(dg1[colDay7, 29].Value);

                ClsFaxOrder.G_Syubai15 = GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 29].Value));


                ClsFaxOrder.YyMmDd = DateTime.Now.ToString();

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, errMsg, MessageBoxButtons.OK);
            }
            finally
            {
            }
        }

        private string SetSyohinCode(string gCode)
        {
            if (gCode != string.Empty)
            {
                return gCode.PadLeft(8, '0');
            }
            else
            {
                return string.Empty;
            }
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     終売処理区分を取得する </summary>
        /// <param name="val">
        ///     コンボボックス選択した値</param>
        /// <returns>
        ///     区分　0:なし, 1:取消, 2:有効</returns>
        ///-------------------------------------------------------------
        private int GetSyubaiStatus(string val)
        {
            int rtn = 0;

            for (int i = 0; i < global.SyubaiArray.Length; i++)
            {
                if (global.SyubaiArray[i] == val)
                {
                    rtn = i;
                    break;
                }
            }

            return rtn;
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     空白以外のとき、指定された文字数になるまで左側に０を埋めこみ、右寄せした文字列を返す
        /// </summary>
        /// <param name="tm">
        ///     文字列</param>
        /// <param name="len">
        ///     文字列の長さ</param>
        /// <returns>
        ///     文字列</returns>
        /// ----------------------------------------------------------------------------------------------------
        private string timeVal(object tm, int len)
        {
            string t = Utility.NulltoStr(tm);
            if (t != string.Empty) return t.PadLeft(len, '0');
            else return t;
        }

        /// ----------------------------------------------------------------------------------------------------
        /// <summary>
        ///     空白以外のとき、先頭文字が０のとき先頭文字を削除した文字列を返す　
        ///     先頭文字が０以外のときはそのまま返す
        /// </summary>
        /// <param name="tm">
        ///     文字列</param>
        /// <returns>
        ///     文字列</returns>
        /// ----------------------------------------------------------------------------------------------------
        private string timeValH(object tm)
        {
            string t = Utility.NulltoStr(tm);

            if (t != string.Empty)
            {
                t = t.PadLeft(2, '0');
                if (t.Substring(0, 1) == "0")
                {
                    t = t.Substring(1, 1);
                }
            }

            return t;
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        ///     Bool値を数値に変換する </summary>
        /// <param name="b">
        ///     True or False</param>
        /// <returns>
        ///     true:1, false:0</returns>
        /// ------------------------------------------------------------------------------------
        private int booltoFlg(string b)
        {
            if (b == "True") return global.flgOn;
            else return global.flgOff;
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
        }

        private void btnBefore_Click(object sender, EventArgs e)
        {
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
        }

        ///-----------------------------------------------------------------
        /// <summary>
        ///     エラーチェックボタン </summary>
        /// <param name="sender">
        ///     </param>
        /// <param name="e">
        ///     </param>
        ///-----------------------------------------------------------------
        private void btnErrCheck_Click(object sender, EventArgs e)
        {
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = hScrollBar1.Value;
            showOcrData(cI);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------------------------
        /// <summary>
        ///     １．指定した勤務票ヘッダデータと勤務票明細データを削除する　
        ///     ２．該当する画像データを削除する</summary>
        /// <param name="i">
        ///     勤務票ヘッダRow インデックス</param>
        ///-------------------------------------------------------------------------------
        private void DataDelete(int i)
        {
            //string sImgNm = string.Empty;
            //string errMsg = string.Empty;

            //// 勤務票データ削除
            //try
            //{
            //    // IDを取得します
            //    STSH_CLIDataSet.FAX注文書Row r = dtsC.FAX注文書.Single(a => a.ID == cID[i]);

            //    // 画像ファイル名を取得します
            //    sImgNm = r.画像名;

            //    // データテーブルから勤務票ヘッダデータを削除します
            //    errMsg = "FAX注文書データ";
            //    r.Delete();

            //    // データベース更新
            //    fAdp.Update(dtsC.FAX注文書);

            //    // 画像ファイルを削除します
            //    errMsg = "FAX発注書画像";
            //    if (sImgNm != string.Empty)
            //    {
            //        if (System.IO.File.Exists(Properties.Settings.Default.mydataPath + sImgNm))
            //        {
            //            System.IO.File.Delete(Properties.Settings.Default.mydataPath + sImgNm);
            //        }
            //    }

            //    // 配列キー再構築
            //    keyArrayCreate();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(errMsg + "の削除に失敗しました" + Environment.NewLine + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //finally
            //{
            //}

        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
        }

        private void frmCorrect_FormClosing(object sender, FormClosingEventArgs e)
        {
            //「受入データ作成終了」「勤務票データなし」以外での終了のとき
            if (this.Tag.ToString() != END_MAKEDATA && this.Tag.ToString() != END_NODATA)
            {
                //if (MessageBox.Show("終了します。よろしいですか", "終了確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                //{
                //    e.Cancel = true;
                //    return;
                //}

                // カレントデータ更新
                if (dID == string.Empty)
                {
                    //CurDataUpDate(cID[cI]);
                }

                //// 勤務表データのない帰宅後勤務データを削除する
                //kitakuClean();
            }

            // データベース更新
            //fAdp.Update(dtsC.FAX注文書);

            //// 楽商データベース接続解除
            //Conn.Close();
            //Conn.Dispose();

            // 解放する
            this.Dispose();
        }

        private void btnDataMake_Click(object sender, EventArgs e)
        {
        }

        /// -----------------------------------------------------------------------
        /// <summary>
        ///     楽商受入CSVデータ出力 </summary>
        /// -----------------------------------------------------------------------
        private void textDataMake()
        {
            //if (MessageBox.Show("楽商データを作成します。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            //// OCRDataクラス生成
            //OCRData ocr = new OCRData(Conn);

            //// エラーチェックを実行
            //if (getErrData(cI, ocr))
            //{
            //    // 社内伝票番号日付を入力
            //    frmDenNumDate frmDen = new frmDenNumDate();
            //    frmDen.ShowDialog();

            //    // OCROutputクラス インスタンス生成
            //    OCROutput kd = new OCROutput(this, dtsC, dts, Conn, _myCode);

            //    // 楽商発注データ作成
            //    kd.SaveData();          
            //}
            //else
            //{
            //    // カレントインデックスをエラーありインデックスで更新
            //    cI = ocr._errHeaderIndex;

            //    // データ表示
            //    showOcrData(cI);

            //    // エラー表示
            //    ErrShow(ocr);

            //    return;
            //}

            //// 画像ファイル退避
            //tifFileMove();
            
            //// FAX注文書データ削除
            //deleteDataAll();

            //// MDBファイル最適化
            //mdbCompact();

            ////終了
            //MessageBox.Show("終了しました。楽商で発注データ受け入れを行ってください。", "楽商受入データ作成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //this.Tag = END_MAKEDATA;
            //this.Close();
        }

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     エラーチェックを実行する</summary>
        /// <param name="cIdx">
        ///     現在表示中の勤務票ヘッダデータインデックス</param>
        /// <param name="ocr">
        ///     OCRDATAクラスインスタンス</param>
        /// <returns>
        ///     エラーなし：true, エラーあり：false</returns>
        /// -----------------------------------------------------------------------------------
        private bool getErrData(int cIdx, OCRData ocr)
        {
            // カレントレコード更新
            CurDataUpDate(cID[cIdx]);

            // エラー番号初期化
            ocr._errNumber = ocr.eNothing;

            // エラーメッセージクリーン
            ocr._errMsg = string.Empty;

            // エラーチェック実行①:カレントレコードから最終レコードまで
            if (!ocr.errCheckMain(cIdx, cID.Length - 1, this, tblFax, tblPtn, cID))
            {
                return false;
            }

            // エラーチェック実行②:最初のレコードからカレントレコードの前のレコードまで
            if (cIdx > 0)
            {
                if (!ocr.errCheckMain(0, (cIdx - 1), this, tblFax, tblPtn, cID))
                {
                    return false;
                }
            }

            // エラーなし
            lblErrMsg.Text = string.Empty;

            return true;
        }

        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     画像ファイル退避処理 </summary>
        ///----------------------------------------------------------------------------------
        private void tifFileMove()
        {
            string sTel = string.Empty;
            string sJyu = string.Empty;

            //var s = dts.環境設定.Single(a => a.ID == global.flgOn);

            //// 移動先フォルダがあるか？なければ作成する（TIFフォルダ）
            //if (!System.IO.Directory.Exists(s.画像保存先パス))
            //{
            //    System.IO.Directory.CreateDirectory(s.画像保存先パス);
            //}

            string fromImg = string.Empty;
            string toImg = string.Empty;

            // 発注書データを取得する
            foreach (var t in tblFax.OrderBy(a => a.ID))
            {
                // 発注書画像ファイルパスを取得する
                fromImg = Properties.Settings.Default.MyDataPath + t.ImageFileName;

                // 移動先フォルダ
                string sName = Utility.getNouhinName(t.TokuisakiCode.ToString().PadLeft(7, '0'), out sTel, out sJyu);   // 届先名

                //// 発注書移動先ファイルパス
                //string userFolder = s.画像保存先パス + t.TokuisakiCode.ToString().PadLeft(7, '0') + "_" + sName;
                
                //// お客様フォルダがあるか？なければ作成する
                //if (!System.IO.Directory.Exists(userFolder))
                //{
                //    System.IO.Directory.CreateDirectory(userFolder);
                //}

                //// 同名ファイルが既に登録済みのときは削除する
                //toImg = userFolder + @"\" + t.ImageFileName;
                //if (System.IO.File.Exists(toImg)) 
                //{
                //    System.IO.File.Delete(toImg);
                //}

                //// ファイルを移動する
                //if (System.IO.File.Exists(fromImg)) 
                //{
                //    System.IO.File.Move(fromImg, toImg);
                //}
            }
        }

        /// ---------------------------------------------------------------------
        /// <summary>
        ///     MDBファイルを最適化する </summary>
        /// ---------------------------------------------------------------------
        private void mdbCompact()
        {
            //try
            //{
            //    JRO.JetEngine jro = new JRO.JetEngine();
            //    string OldDb = Properties.Settings.Default.mdbOlePath;
            //    string NewDb = Properties.Settings.Default.mdbPathTemp;

            //    jro.CompactDatabase(OldDb, NewDb);

            //    //今までのバックアップファイルを削除する
            //    System.IO.File.Delete(Properties.Settings.Default.mdbPath + global.MDBBACK);

            //    //今までのファイルをバックアップとする
            //    System.IO.File.Move(Properties.Settings.Default.mdbPath + global.MDBFILE, Properties.Settings.Default.mdbPath + global.MDBBACK);

            //    //一時ファイルをMDBファイルとする
            //    System.IO.File.Move(Properties.Settings.Default.mdbPath + global.MDBTEMP, Properties.Settings.Default.mdbPath + global.MDBFILE);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("MDB最適化中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            //}
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {

            //if (dGV.RowCount == global.NIPPOU_TATE)
            //{
            //    global.miMdlZoomRate_TATE = (float)leadImg.ScaleFactor;
            //}
            //else if (dGV.RowCount == global.NIPPOU_YOKO)
            //{
            //    global.miMdlZoomRate_YOKO = (float)leadImg.ScaleFactor;
            //}
        }

        /// ---------------------------------------------------------------------------------
        /// <summary>
        ///     設定月数分経過した過去画像と過去勤務データ、過去応援移動票データを削除する </summary> 
        /// ---------------------------------------------------------------------------------
        private void deleteArchived()
        {
            //// 削除月設定が0のとき、「過去画像削除しない」とみなし終了する
            //if (Properties.Settings.Default.dataDelSpan == global.flgOff) return;

            //try
            //{
            //    // 削除年月の取得
            //    DateTime dt = DateTime.Parse(DateTime.Today.Year.ToString() + "/" + DateTime.Today.Month.ToString() + "/01");
            //    DateTime delDate = dt.AddMonths(Properties.Settings.Default.dataDelSpan * (-1));
            //    int _dYY = delDate.Year;            //基準年
            //    int _dMM = delDate.Month;           //基準月
            //    int _dYYMM = _dYY * 100 + _dMM;     //基準年月
            //    int _waYYMM = (delDate.Year - Properties.Settings.Default.rekiHosei) * 100 + _dMM;   //基準年月(和暦）

            //    // 設定月数分経過した過去画像・過去勤務票データを削除する
            //    deleteLastDataArchived(_dYYMM);

            //    // 設定月数分経過した過去画像・過去応援移動票データを削除する
            //    deleteLastOuenDataArchived(_dYYMM);
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("過去画像・過去勤務票データ削除中" + Environment.NewLine + e.Message, "エラー", MessageBoxButtons.OK);
            //    return;
            //}
            //finally
            //{
            //    //if (ocr.sCom.Connection.State == ConnectionState.Open) ocr.sCom.Connection.Close();
            //}
        }

        /// ---------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票データ削除～登録 </summary>
        /// ---------------------------------------------------------------------------
        private void saveLastData()
        {
            //try
            //{
            //    // データベース更新
            //    adpMn.UpdateAll(dts);
            //    pAdpMn.UpdateAll(dts);

            //    //  過去勤務票ヘッダデータとその明細データを削除します
            //    //deleteLastData();
            //    delPastData();

            //    // データセットへデータを再読み込みします
            //    getDataSet();

            //    // 過去勤務票ヘッダデータと過去勤務票明細データを作成します
            //    addLastdata();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "過去勤務票データ作成エラー", MessageBoxButtons.OK);
            //}
            //finally
            //{
            //}
        }


        ///------------------------------------------------------
        /// <summary>
        ///     過去勤務票データ削除 </summary>
        ///------------------------------------------------------
        private void delPastData()
        {
            //// 過去勤務票ヘッダデータ削除
            //foreach (var t in dts.勤務票ヘッダ)
            //{
            //    string sBusho = t.部署コード;
            //    int sYY = t.年;
            //    int sMM = t.月;
            //    int sDD = t.日;

            //    // 過去勤務票ヘッダ削除
            //    delPastHeader(sBusho, sYY, sMM, sDD);
            //}

            //// 過去勤務票明細データ削除
            //delPastItem();
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータ削除 </summary>
        /// <param name="bCode">
        ///     部署コード</param>
        /// <param name="syy">
        ///     対象年</param>
        /// <param name="smm">
        ///     対象月</param>
        /// <param name="sdd">
        ///     対象日</param>
        ///----------------------------------------------------------------
        private void delPastHeader(string bCode, int syy, int smm, int sdd)
        {
            //OleDbCommand sCom = new OleDbCommand();
            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);

            //try
            //{
            //    StringBuilder sb = new StringBuilder();

            //    sb.Clear();
            //    sb.Append("delete from 過去勤務票ヘッダ ");
            //    sb.Append("where 部署コード = ? and 年 = ? and 月 = ? and 日 = ?");

            //    sCom.CommandText = sb.ToString();
            //    sCom.Parameters.Clear();
            //    sCom.Parameters.AddWithValue("@b", bCode);
            //    sCom.Parameters.AddWithValue("@y", syy);
            //    sCom.Parameters.AddWithValue("@m", smm);
            //    sCom.Parameters.AddWithValue("@d", sdd);

            //    sCom.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }
            //}
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     過去勤務票明細データ削除 </summary>
        ///--------------------------------------------------------
        private void delPastItem()
        {
            //OleDbCommand sCom = new OleDbCommand();
            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);

            //try
            //{
            //    StringBuilder sb = new StringBuilder();

            //    sb.Clear();
            //    sb.Append("delete a.ヘッダID from  過去勤務票明細 as a ");
            //    sb.Append("where not EXISTS (select * from 過去勤務票ヘッダ ");
            //    sb.Append("WHERE 過去勤務票ヘッダ.ID = a.ヘッダID)");
                
            //    sCom.CommandText = sb.ToString();
            //    sCom.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //    throw;
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }
            //}
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータとその明細データを削除します</summary>    
        ///     
        /// -------------------------------------------------------------------------
        private void deleteLastData()
        {
            //OleDbCommand sCom = new OleDbCommand();
            //OleDbCommand sCom2 = new OleDbCommand();
            //OleDbCommand sCom3 = new OleDbCommand();

            //mdbControl mdb = new mdbControl();
            //mdb.dbConnect(sCom);
            //mdb.dbConnect(sCom2);
            //mdb.dbConnect(sCom3);

            //OleDbDataReader dR = null;
            //OleDbDataReader dR2 = null;

            //StringBuilder sb = new StringBuilder();
            //StringBuilder sbd = new StringBuilder();

            //try
            //{
            //    // 対象データ : 取消は対象外とする
            //    sb.Clear();
            //    sb.Append("Select 勤務票明細.ヘッダID, 勤務票明細.ID,");
            //    sb.Append("勤務票ヘッダ.年, 勤務票ヘッダ.月, 勤務票ヘッダ.日,");
            //    sb.Append("勤務票明細.社員番号 from 勤務票ヘッダ inner join 勤務票明細 ");
            //    sb.Append("on 勤務票ヘッダ.ID = 勤務票明細.ヘッダID ");
            //    sb.Append("where 勤務票明細.取消 = '").Append(global.FLGOFF).Append("'");
            //    sb.Append("order by 勤務票明細.ヘッダID, 勤務票明細.ID");

            //    sCom.CommandText = sb.ToString();
            //    dR = sCom.ExecuteReader();

            //    while (dR.Read())
            //    {
            //        // ヘッダID
            //        string hdID = string.Empty;

            //        // 日付と社員番号で過去データを抽出（該当するのは1件）
            //        sb.Clear();
            //        sb.Append("Select 過去勤務票明細.ヘッダID,過去勤務票明細.ID,");
            //        sb.Append("過去勤務票ヘッダ.年, 過去勤務票ヘッダ.月, 過去勤務票ヘッダ.日,");
            //        sb.Append("過去勤務票明細.社員番号 from 過去勤務票ヘッダ inner join 過去勤務票明細 ");
            //        sb.Append("on 過去勤務票ヘッダ.ID = 過去勤務票明細.ヘッダID ");
            //        sb.Append("where ");
            //        sb.Append("過去勤務票ヘッダ.年 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.月 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.日 = ? and ");
            //        sb.Append("過去勤務票ヘッダ.データ領域名 = ? and ");
            //        sb.Append("過去勤務票明細.社員番号 = ?");

            //        sCom2.CommandText = sb.ToString();
            //        sCom2.Parameters.Clear();
            //        sCom2.Parameters.AddWithValue("@yy", dR["年"].ToString());
            //        sCom2.Parameters.AddWithValue("@mm", dR["月"].ToString());
            //        sCom2.Parameters.AddWithValue("@dd", dR["日"].ToString());
            //        sCom2.Parameters.AddWithValue("@db", _dbName);
            //        sCom2.Parameters.AddWithValue("@n", dR["社員番号"].ToString());

            //        dR2 = sCom2.ExecuteReader();

            //        while (dR2.Read())
            //        {
            //            //// ヘッダIDを取得
            //            //if (hdID == string.Empty)
            //            //{
            //            //    hdID = dR2["ヘッダID"].ToString();
            //            //}

            //            // 過去勤務票明細レコード削除
            //            sbd.Clear();
            //            sbd.Append("delete from 過去勤務票明細 ");
            //            sbd.Append("where ID = ?");

            //            sCom3.CommandText = sbd.ToString();
            //            sCom3.Parameters.Clear();
            //            sCom3.Parameters.AddWithValue("@id", dR2["ID"].ToString());

            //            sCom3.ExecuteNonQuery();
            //        }

            //        dR2.Close();
            //    }

            //    dR.Close();

            //    // データベース接続解除
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }

            //    if (sCom2.Connection.State == ConnectionState.Open)
            //    {
            //        sCom2.Connection.Close();
            //    }

            //    if (sCom3.Connection.State == ConnectionState.Open)
            //    {
            //        sCom3.Connection.Close();
            //    }

            //    // データベース再接続
            //    mdb.dbConnect(sCom);
            //    mdb.dbConnect(sCom2);

            //    // 明細データのない過去勤務票ヘッダデータを抽出
            //    sb.Clear();
            //    sb.Append("Select 過去勤務票ヘッダ.ID,過去勤務票明細.ヘッダID ");
            //    sb.Append("from 過去勤務票ヘッダ left join 過去勤務票明細 ");
            //    sb.Append("on 過去勤務票ヘッダ.ID = 過去勤務票明細.ヘッダID ");
            //    sb.Append("where ");
            //    sb.Append("過去勤務票明細.ヘッダID is null");
            //    sCom.CommandText = sb.ToString();
            //    dR = sCom.ExecuteReader();

            //    while (dR.Read())
            //    {
            //        // 過去勤務票ヘッダレコード削除
            //        sbd.Clear();

            //        sbd.Append("delete from 過去勤務票ヘッダ ");
            //        sbd.Append("where ID = ?");

            //        sCom2.CommandText = sbd.ToString();
            //        sCom2.Parameters.Clear();
            //        sCom2.Parameters.AddWithValue("@id", dR["ID"].ToString());

            //        sCom2.ExecuteNonQuery();
            //    }

            //    dR.Close();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
            //finally
            //{
            //    if (sCom.Connection.State == ConnectionState.Open)
            //    {
            //        sCom.Connection.Close();
            //    }

            //    if (sCom2.Connection.State == ConnectionState.Open)
            //    {
            //        sCom2.Connection.Close();
            //    }

            //    if (sCom3.Connection.State == ConnectionState.Open)
            //    {
            //        sCom3.Connection.Close();
            //    }
            //}
        }


        /// -------------------------------------------------------------------------
        /// <summary>
        ///     過去勤務票ヘッダデータと過去勤務票明細データを作成します</summary>
        ///     
        /// -------------------------------------------------------------------------
        private void addLastdata()
        {
            //for (int i = 0; i < dts.勤務票ヘッダ.Rows.Count; i++)
            //{
            //    // -------------------------------------------------------------------------
            //    //      過去勤務票ヘッダレコードを作成します
            //    // -------------------------------------------------------------------------
            //    DataSet1.勤務票ヘッダRow hr = (DataSet1.勤務票ヘッダRow)dts.勤務票ヘッダ.Rows[i];
            //    DataSet1.過去勤務票ヘッダRow nr = dts.過去勤務票ヘッダ.New過去勤務票ヘッダRow();

            //    #region テーブルカラム名比較～データコピー

            //    // 勤務票ヘッダのカラムを順番に読む
            //    for (int j = 0; j < dts.勤務票ヘッダ.Columns.Count; j++)
            //    {
            //        // 過去勤務票ヘッダのカラムを順番に読む
            //        for (int k = 0; k < dts.過去勤務票ヘッダ.Columns.Count; k++)
            //        {
            //            // フィールド名が同じであること
            //            if (dts.勤務票ヘッダ.Columns[j].ColumnName == dts.過去勤務票ヘッダ.Columns[k].ColumnName)
            //            {
            //                if (dts.過去勤務票ヘッダ.Columns[k].ColumnName == "更新年月日")
            //                {
            //                    nr[k] = DateTime.Now;   // 更新年月日はこの時点のタイムスタンプを登録
            //                }
            //                else
            //                {
            //                    nr[k] = hr[j];          // データをコピー
            //                }
            //                break;
            //            }
            //        }
            //    }
            //    #endregion

            //    // 過去勤務票ヘッダデータテーブルに追加
            //    dts.過去勤務票ヘッダ.Add過去勤務票ヘッダRow(nr);

            //    // -------------------------------------------------------------------------
            //    //      過去勤務票明細レコードを作成します
            //    // -------------------------------------------------------------------------
            //    var mm = dts.勤務票明細
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //               a.ヘッダID == hr.ID)
            //        .OrderBy(a => a.ID);

            //    foreach (var item in mm)
            //    {
            //        DataSet1.勤務票明細Row m = (DataSet1.勤務票明細Row)dts.勤務票明細.Rows.Find(item.ID);
            //        DataSet1.過去勤務票明細Row nm = dts.過去勤務票明細.New過去勤務票明細Row();

            //        // 取消は対象外：2015/10/01
            //        if (m.取消 == global.FLGON) continue;

            //        // 社員番号が空白のレコードは対象外とします
            //        if (m.社員番号 == string.Empty) continue;

            //        #region  テーブルカラム名比較～データコピー

            //        // 勤務票明細のカラムを順番に読む
            //        for (int j = 0; j < dts.勤務票明細.Columns.Count; j++)
            //        {
            //            // IDはオートナンバーのため値はコピーしない
            //            if (dts.勤務票明細.Columns[j].ColumnName != "ID")
            //            {
            //                // 過去勤務票ヘッダのカラムを順番に読む
            //                for (int k = 0; k < dts.過去勤務票明細.Columns.Count; k++)
            //                {
            //                    // フィールド名が同じであること
            //                    if (dts.勤務票明細.Columns[j].ColumnName == dts.過去勤務票明細.Columns[k].ColumnName)
            //                    {
            //                        if (dts.過去勤務票明細.Columns[k].ColumnName == "更新年月日")
            //                        {
            //                            nm[k] = DateTime.Now;   // 更新年月日はこの時点のタイムスタンプを登録
            //                        }
            //                        else
            //                        {
            //                            nm[k] = m[j];          // データをコピー
            //                        }
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        #endregion

            //        // 過去勤務票明細データテーブルに追加
            //        dts.過去勤務票明細.Add過去勤務票明細Row(nm);
            //    }
            //}

            //// データベース更新
            //pAdpMn.UpdateAll(dts);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        //    //if (e.RowIndex < 0) return;

        //    string colName = dGV.Columns[e.ColumnIndex].Name;

        //    if (colName == cSH || colName == cSE || colName == cEH || colName == cEE ||
        //        colName == cZH || colName == cZE || colName == cSIH || colName == cSIE)
        //    {
        //        e.AdvancedBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
        //    }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //string colName = dGV.Columns[dGV.CurrentCell.ColumnIndex].Name;
            ////if (colName == cKyuka || colName == cCheck)
            ////{
            ////    if (dGV.IsCurrentCellDirty)
            ////    {
            ////        dGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ////        dGV.RefreshEdit();
            ////    }
            ////}
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellEnter_1(object sender, DataGridViewCellEventArgs e)
        {
            //// 時が入力済みで分が未入力のとき分に"00"を表示します
            //if (dGV[ColH, dGV.CurrentRow.Index].Value != null)
            //{
            //    if (dGV[ColH, dGV.CurrentRow.Index].Value.ToString().Trim() != string.Empty)
            //    {
            //        if (dGV[ColM, dGV.CurrentRow.Index].Value == null)
            //        {
            //            dGV[ColM, dGV.CurrentRow.Index].Value = "00";
            //        }
            //        else if (dGV[ColM, dGV.CurrentRow.Index].Value.ToString().Trim() == string.Empty)
            //        {
            //            dGV[ColM, dGV.CurrentRow.Index].Value = "00";
            //        }
            //    }
            //}
        }

        /// ------------------------------------------------------------------------------
        /// <summary>
        ///     伝票画像表示 </summary>
        /// <param name="iX">
        ///     現在の伝票</param>
        /// <param name="tempImgName">
        ///     画像名</param>
        /// ------------------------------------------------------------------------------
        public void ShowImage(string tempImgName)
        {
            ////修正画面へ組み入れた画像フォームの表示    
            ////画像の出力が無い場合は、画像表示をしない。
            //if (tempImgName == string.Empty)
            //{
            //    leadImg.Visible = false;
            //    lblNoImage.Visible = false;
            //    //global.pblImagePath = string.Empty;
            //    return;
            //}

            ////画像ファイルがあるとき表示
            //if (File.Exists(tempImgName))
            //{
            //    lblNoImage.Visible = false;
            //    leadImg.Visible = true;

            //    // 画像操作ボタン
            //    btnPlus.Enabled = true;
            //    btnMinus.Enabled = true;

            //    // 画像回転ボタン
            //    btnLeft.Enabled = true;
            //    btnRight.Enabled = true;

            //    //画像ロード
            //    Leadtools.Codecs.RasterCodecs.Startup();
            //    Leadtools.Codecs.RasterCodecs cs = new Leadtools.Codecs.RasterCodecs();

            //    // 描画時に使用される速度、品質、およびスタイルを制御します。 
            //    Leadtools.RasterPaintProperties prop = new Leadtools.RasterPaintProperties();
            //    prop = Leadtools.RasterPaintProperties.Default;
            //    prop.PaintDisplayMode = Leadtools.RasterPaintDisplayModeFlags.Resample;
            //    leadImg.PaintProperties = prop;

            //    leadImg.Image = cs.Load(tempImgName, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, 1, 1);

            //    //画像表示倍率設定
            //    if (gl.miMdlZoomRate == 0f)
            //    {
            //        leadImg.ScaleFactor *= gl.ZOOM_RATE;
            //    }
            //    else
            //    {
            //        leadImg.ScaleFactor *= gl.miMdlZoomRate;
            //    }

            //    //画像のマウスによる移動を可能とする
            //    leadImg.InteractiveMode = Leadtools.WinForms.RasterViewerInteractiveMode.Pan;

            //    // グレースケールに変換
            //    Leadtools.ImageProcessing.GrayscaleCommand grayScaleCommand = new Leadtools.ImageProcessing.GrayscaleCommand();
            //    grayScaleCommand.BitsPerPixel = 8;
            //    grayScaleCommand.Run(leadImg.Image);
            //    leadImg.Refresh();

            //    cs.Dispose();
            //    Leadtools.Codecs.RasterCodecs.Shutdown();
            //    //global.pblImagePath = tempImgName;
            //}
            //else
            //{
            //    //画像ファイルがないとき
            //    lblNoImage.Visible = true;

            //    // 画像操作ボタン
            //    btnPlus.Enabled = false;
            //    btnMinus.Enabled = false;

            //    leadImg.Visible = false;
            //    //global.pblImagePath = string.Empty;

            //    // 画像回転ボタン
            //    btnLeft.Enabled = false;
            //    btnRight.Enabled = false;
            //}
        }

        private void leadImg_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void leadImg_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        /// -------------------------------------------------------------------------
        /// <summary>
        ///     基準年月以前の過去勤務票ヘッダデータとその明細データを削除します</summary>
        /// <param name="sYYMM">
        ///     基準年月</param>     
        /// -------------------------------------------------------------------------
        private void deleteLastDataArchived(int sYYMM)
        {
            //// データ読み込み
            //getDataSet();

            //// 基準年月以前の過去勤務票ヘッダデータを取得します
            //var h = dts.過去勤務票ヘッダ
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //                    a.年 * 100 + a.月 < sYYMM);

            //// foreach用の配列を作成
            //var hLst = h.ToList();

            //foreach (var lh in hLst)
            //{
            //    // ヘッダIDが一致する過去勤務票明細を取得します
            //    var m = dts.過去勤務票明細
            //        .Where(a => a.RowState != DataRowState.Deleted && a.RowState != DataRowState.Detached &&
            //                    a.ヘッダID == lh.ID);

            //    // foreach用の配列を作成
            //    var list = m.ToList();

            //    // 該当過去勤務票明細を削除します
            //    foreach (var lm in list)
            //    {
            //        DataSet1.過去勤務票明細Row lRow = (DataSet1.過去勤務票明細Row)dts.過去勤務票明細.Rows.Find(lm.ID);
            //        lRow.Delete();
            //    }

            //    // 画像ファイルを削除します
            //    string imgPath = Properties.Settings.Default.tifPath + lh.画像名;
            //    File.Delete(imgPath);

            //    // 過去勤務票ヘッダを削除します
            //    lh.Delete();
            //}

            //// データベース更新
            //pAdpMn.UpdateAll(dts);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///     設定月数分経過した過去画像を削除する</summary>
        /// <param name="_dYYMM">
        ///     基準年月 (例：201401)</param>
        /// -----------------------------------------------------------------------------
        private void deleteImageArchived(int _dYYMM)
        {
            //int _DataYYMM;
            //string fileYYMM;

            //// 設定月数分経過した過去画像を削除する            
            //foreach (string files in System.IO.Directory.GetFiles(Properties.Settings.Default.tifPath, "*.tif"))
            //{
            //    // ファイル名が規定外のファイルは読み飛ばします
            //    if (System.IO.Path.GetFileName(files).Length < 21) continue;

            //    //ファイル名より年月を取得する
            //    fileYYMM = System.IO.Path.GetFileName(files).Substring(0, 6);

            //    if (Utility.NumericCheck(fileYYMM))
            //    {
            //        _DataYYMM = int.Parse(fileYYMM);

            //        //基準年月以前なら削除する
            //        if (_DataYYMM <= _dYYMM) File.Delete(files);
            //    }
            //}
        }

        /// -------------------------------------------------------------------
        /// <summary>
        ///     FAX注文書データを全件削除します</summary>
        /// -------------------------------------------------------------------
        private void deleteDataAll()
        {
            // FAX注文書データ読み込み
            //getDataSet();

            //// FAX注文書削除
            //var m = dtsC.FAX注文書.Where(a => a.RowState != DataRowState.Deleted);
            //foreach (var t in m)
            //{
            //    t.Delete();
            //}

            //// データベース更新
            //fAdp.Update(dtsC.FAX注文書);

            //// 後片付け
            //dtsC.FAX注文書.Dispose();
        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void txtYear_TextChanged(object sender, EventArgs e)
        {
            //// 曜日
            //DateTime eDate;
            //int tYY = Utility.StrtoInt(txtYear.Text);
            //string sDate = tYY.ToString() + "/" + Utility.EmptytoZero(txtMonth.Text) + "/" +
            //        Utility.EmptytoZero(txtDay.Text);

            //// 存在する日付と認識された場合、曜日を表示する
            //if (DateTime.TryParse(sDate, out eDate))
            //{
            //    txtWeekDay.Text = ("日月火水木金土").Substring(int.Parse(eDate.DayOfWeek.ToString("d")), 1);
            //}
            //else
            //{
            //    txtWeekDay.Text = string.Empty;
            //}
        }

        private void dGV_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //if (editLogStatus)
            //{
            //    if (e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 4 ||
            //        e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 9 || e.ColumnIndex == 10 ||
            //        e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 15)
            //    {
            //        dGV.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //        cellAfterValue = Utility.NulltoStr(dGV[e.ColumnIndex, e.RowIndex].Value);

            //        //// 変更のとき編集ログデータを書き込み
            //        //if (cellBeforeValue != cellAfterValue)
            //        //{
            //        //    logDataUpdate(e.RowIndex, cI, global.flgOn);
            //        //}
            //    }
            //}
        }

        private void txtYear_Enter(object sender, EventArgs e)
        {
            //if (editLogStatus)
            //{
            //    if (sender == txtYear) cellName = LOG_YEAR;
            //    if (sender == txtMonth) cellName = LOG_MONTH;
            //    if (sender == txtDay) cellName = LOG_DAY;
            //    //if (sender == txtSftCode) cellName = LOG_TAIKEICD;

            //    TextBox tb = (TextBox)sender;

            //    // 値を保持
            //    cellBeforeValue = Utility.NulltoStr(tb.Text);
            //}
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            if (editLogStatus)
            {
                TextBox tb = (TextBox)sender;
                cellAfterValue = Utility.NulltoStr(tb.Text);

                //// 変更のとき編集ログデータを書き込み
                //if (cellBeforeValue != cellAfterValue)
                //{
                //    logDataUpdate(0, cI, global.flgOff);
                //}
            }
        }

        //private void gcMultiRow1_CellValueChanged(object sender, CellEventArgs e)
        //{
        //    if (!gl.ChangeValueStatus) return;

        //    if (e.RowIndex < 0) return;

        //    // 過去データ表示のときは終了
        //    if (dID != string.Empty) return;

        //    // パターンコードのとき発注書パターンを更新
        //    if (e.CellName == "txtPtnNum")
        //    {
        //        // 発注パターン表示
        //        ptnShow(gcMultiRow2,
        //            Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtTdkNum"].Value)),
        //            Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName].Value)));

        //        // パターンID「０」でフリー入力のとき
        //        if (Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName])) == global.flgOff)
        //        {
        //            // 商品コード、数量入力可能とする
        //            for (int i = 0; i < gcMultiRow2.RowCount; i++)
        //            {
        //                gcMultiRow2[i, "txtHinCode"].ReadOnly = false;
        //                gcMultiRow2[i, "txtHinCode"].Selectable = true;

        //                gcMultiRow2[i, "txtSuu"].ReadOnly = false;
        //                gcMultiRow2[i, "txtSuu"].Selectable = true;

        //                gcMultiRow2[i, "txtHinCode2"].ReadOnly = false;
        //                gcMultiRow2[i, "txtHinCode2"].Selectable = true;

        //                gcMultiRow2[i, "txtSuu2"].ReadOnly = false;
        //                gcMultiRow2[i, "txtSuu2"].Selectable = true;
        //            }
        //        }
        //        else
        //        {
        //            // 商品コード、数量入力可能とする
        //            for (int i = 0; i < gcMultiRow2.RowCount; i++)
        //            {
        //                gcMultiRow2[i, "txtHinCode"].ReadOnly = true;
        //                gcMultiRow2[i, "txtHinCode"].Selectable = false;

        //                gcMultiRow2[i, "txtSuu"].ReadOnly = false;
        //                gcMultiRow2[i, "txtSuu"].Selectable = true;

        //                gcMultiRow2[i, "txtHinCode2"].ReadOnly = true;
        //                gcMultiRow2[i, "txtHinCode2"].Selectable = false;

        //                gcMultiRow2[i, "txtSuu2"].ReadOnly = false;
        //                gcMultiRow2[i, "txtSuu2"].Selectable = true;
        //            }
        //        }
        //    }
        //    else if (e.CellName == "txtTdkNum")
        //    {
        //        // お客様番号のときお客様名を表示します

        //        // ChangeValueイベントを発生させない
        //        gl.ChangeValueStatus = false;

        //        // 氏名と電話番号を初期化
        //        gcMultiRow1[e.RowIndex, "lblName"].Value = string.Empty;
        //        gcMultiRow1[e.RowIndex, "lblTel"].Value = string.Empty;
                
        //        // 楽商データベースよりお客様名を取得して表示します
        //        if (Utility.NulltoStr(gcMultiRow1[0, "txtTdkNum"].Value) != string.Empty)
        //        {
        //            // 届先名、電話番号、住所表示
        //            string gName = string.Empty;
        //            string gTel = string.Empty;
        //            string gJyu = string.Empty;

        //            string bCode = gcMultiRow1[e.RowIndex, "txtTdkNum"].Value.ToString().PadLeft(6, '0');
        //            gName = getUserName(bCode, out gTel, out gJyu);

        //            gcMultiRow1[e.RowIndex, "lblName"].Value = gName;
        //            gcMultiRow1[e.RowIndex, "lblTel"].Value = gTel;

        //            // ChangeValueイベントステータスをtrueに戻す
        //            gl.ChangeValueStatus = true;
        //        }

        //        // 発注パターン表示
        //        ptnShow(gcMultiRow2,
        //            Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, e.CellName].Value)),
        //            Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[e.RowIndex, "txtPtnNum"].Value)));
        //    }
        //}
        
        //private void gcMultiRow1_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        //{
        //    if (e.Control is TextBoxEditingControl)
        //    {
        //        //イベントハンドラが複数回追加されてしまうので最初に削除する
        //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
        //        e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown2);

        //        // 数字のみ入力可能とする
        //        if (gcMultiRow1.CurrentCell.Name == "txtPtnNum" || gcMultiRow1.CurrentCell.Name == "txtTdkNum" ||
        //            gcMultiRow1.CurrentCell.Name == "txtOrderNum" || gcMultiRow1.CurrentCell.Name == "txtMonth" ||
        //            gcMultiRow1.CurrentCell.Name == "txtDay")
        //        {
        //            //イベントハンドラを追加する
        //            e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        //        }

        //        // お客様検索画面
        //        if (gcMultiRow1.CurrentCell.Name == "txtTdkNum")
        //        {
        //            //イベントハンドラを追加する
        //            e.Control.KeyDown += new KeyEventHandler(Control_KeyDown2);
        //        }
        //    }
        //}

        private void Control_KeyDown2(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.Space)
            //{
            //    gcMultiRow1.EndEdit();

            //    frmTodoke frm = new frmTodoke(false);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        gcMultiRow1.SetValue(0, "txtTdkNum", frm._nouCode[0]);
            //        gcMultiRow1.CurrentCellPosition = new CellPosition(0, "txtOrderNum");
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //}
        }

        private void Control_KeyDownHinM2(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.Space)
            //{
            //    //gcMultiRow2.EndEdit();

            //    frmSyohin frm = new frmSyohin(false);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        gcMultiRow2.SetValue(gcMultiRow2.CurrentCell.RowIndex, gcMultiRow2.CurrentCellPosition.CellName, frm._nouCode[0]);

            //        if (gcMultiRow2.CurrentCellPosition.CellName == "txtHinCode")
            //        {
            //            gcMultiRow2.CurrentCellPosition = new CellPosition(gcMultiRow2.CurrentCell.RowIndex, "txtSuu");
            //            //gcMultiRow2.CurrentCell = null;
            //        }
            //        else if (gcMultiRow2.CurrentCellPosition.CellName == "txtHinCode2")
            //        {
            //            gcMultiRow2.CurrentCellPosition = new CellPosition(gcMultiRow2.CurrentCell.RowIndex, "txtSuu2");
            //            //gcMultiRow2.CurrentCell = null;
            //        }
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //}
        }

        private void Control_KeyDownHin(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.Space)
            //{
            //    gcMultiRow3.EndEdit();

            //    frmSyohin frm = new frmSyohin(false);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        gcMultiRow3.SetValue(gcMultiRow3.CurrentCell.RowIndex, gcMultiRow3.CurrentCellPosition.CellName, frm._nouCode[0]);

            //        if (gcMultiRow3.CurrentCellPosition.CellName == "txtHinCode")
            //        {
            //            gcMultiRow3.CurrentCellPosition = new CellPosition(gcMultiRow3.CurrentCell.RowIndex, "txtSuu");
            //            //gcMultiRow3.CurrentCell = null;
            //        }
            //        else if (gcMultiRow3.CurrentCellPosition.CellName == "txtHinCode2")
            //        {
            //            gcMultiRow3.CurrentCellPosition = new CellPosition(gcMultiRow3.CurrentCell.RowIndex, "txtSuu2");
            //            //gcMultiRow3.CurrentCell = null;
            //        }
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //}
        }

        //private void gcMultiRow2_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        //{
            //if (e.Control is TextBoxEditingControl)
            //{
            //    //イベントハンドラが複数回追加されてしまうので最初に削除する
            //    e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            //    //e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress2);
            //    e.Control.KeyDown -= new KeyEventHandler(Control_KeyDownHinM2);

            //    // 数字のみ入力可能とする
            //    if (gcMultiRow2.CurrentCell.Name == "txtHinCode" || gcMultiRow2.CurrentCell.Name == "txtHinCode2" || 
            //        gcMultiRow2.CurrentCell.Name == "txtSuu" || gcMultiRow2.CurrentCell.Name == "txtSuu2")
            //    {
            //        //イベントハンドラを追加する
            //        e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
            //    }
                
            //    // 商品検索画面呼出
            //    if (gcMultiRow2.CurrentCell.Name == "txtHinCode" || gcMultiRow2.CurrentCell.Name == "txtHinCode2")
            //    {
            //        //イベントハンドラを追加する
            //        e.Control.KeyDown += new KeyEventHandler(Control_KeyDownHinM2);
            //    }
            //}
        //}

        //private void gcMultiRow2_CellValueChanged(object sender, CellEventArgs e)
        //{
        //    if (!gl.ChangeValueStatus)
        //    {
        //        return;
        //    }

        //    if (e.RowIndex < 0)
        //    {
        //        return;
        //    }
            
        //    // 商品名表示
        //    if (e.CellName == "txtHinCode" || e.CellName == "txtHinCode2")
        //    {
        //        gl.ChangeValueStatus = false;

        //        gcHinCodeChange(gcMultiRow2, e.CellName, e.RowIndex, true);

        //        //if (!showStatus)
        //        //{
        //        //    // 出荷基準判定
        //        //    kijunCheckMain();
        //        //}

        //        // パターンIDが０のときフリー入力可能とする：2017/08/21
        //        int ptnCode = Utility.StrtoInt(Utility.NulltoStr(gcMultiRow1[0, "txtPtnNum"].Value));

        //        if (ptnCode == global.flgOff)
        //        {
        //            gcMultiRow2[e.RowIndex, e.CellName].ReadOnly = false;
        //            gcMultiRow2[e.RowIndex, e.CellName].Selectable = true;
        //        }
        //        else
        //        {
        //            gcMultiRow2[e.RowIndex, e.CellName].ReadOnly = true;
        //            gcMultiRow2[e.RowIndex, e.CellName].Selectable = false;
        //        }

        //        gl.ChangeValueStatus = true;
        //    }

        //    // 発注数
        //    if (e.CellName == "txtSuu" || e.CellName == "txtSuu2")
        //    {
        //        gl.ChangeValueStatus = false;

        //        if (!showStatus)
        //        {
        //            // 出荷基準判定
        //            kijunCheckMain();
        //        }

        //        gl.ChangeValueStatus = true;
        //    }


        //}

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     奉行シリーズ部署名取得 </summary>
        /// <param name="dName">
        ///     取得する部署名</param>
        /// <param name="dCode">
        ///     部署コード</param>
        /// <param name="r">
        ///     MultiRowRowIndex</param>
        /// <returns>
        ///     true:該当あり, false:該当なし</returns>
        ///-------------------------------------------------------------------------
        private bool getDepartMentName(out string dName, string dCode, int r)
        {
            bool rtn = false;
            //int c = 0;

            //// 部署名を初期化
            dName = string.Empty;

            //// 奉行データベースより部署名を取得して表示します
            //if (Utility.NulltoStr(gcMultiRow2[r, "txtBushoCode"].Value) != string.Empty)
            //{
            //    string b = string.Empty;

            //    // 検索用部署コード
            //    if (Utility.StrtoInt(gcMultiRow2[r, "txtBushoCode"].Value.ToString()) != global.flgOff)
            //    {
            //        b = gcMultiRow2[r, "txtBushoCode"].Value.ToString().Trim().PadLeft(15, '0');
            //    }
            //    else
            //    {
            //        b = gcMultiRow2[r, "txtBushoCode"].Value.ToString().Trim().PadRight(15, ' ');
            //    }

            //    // 接続文字列取得
            //    string sc = sqlControl.obcConnectSting.get(_dbName);
            //    sqlControl.DataControl sdCon = new Common.sqlControl.DataControl(sc);

            //    string dt = DateTime.Today.ToShortDateString();
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("SELECT DepartmentID, DepartmentCode, DepartmentName ");
            //    sb.Append("FROM tbDepartment ");
            //    sb.Append("where EstablishDate <= '").Append(dt).Append("'");
            //    sb.Append(" and AbolitionDate >= '").Append(dt).Append("'");
            //    sb.Append(" and ValidDate <= '").Append(dt).Append("'");
            //    sb.Append(" and InValidDate >= '").Append(dt).Append("'");
            //    sb.Append(" and DepartmentCode = '").Append(b).Append("'");

            //    SqlDataReader dR = sdCon.free_dsReader(sb.ToString());

            //    while (dR.Read())
            //    {
            //        dName = dR["DepartmentName"].ToString().Trim();
            //        c++;
            //    }

            //    dR.Close();
            //    sdCon.Close();

            //    if (c > 0)
            //    {
            //        rtn = true;
            //    }
            //}
            
            return rtn;
        }

        ///-------------------------------------------------------------------
        /// <summary>
        ///     ライン・部門・製品群コード配列取得   </summary>
        /// <returns>
        ///     ID,コード配列</returns>
        ///-------------------------------------------------------------------
        private string[] getCategoryArray()
        {
            //// 接続文字列取得
            //string sc = sqlControl.obcConnectSting.get(_dbName);
            //sqlControl.DataControl sdCon = new sqlControl.DataControl(sc);

            //StringBuilder sb = new StringBuilder();
            //sb.Append("select CategoryID, CategoryCode from tbHistoryDivisionCategory");
            //SqlDataReader dr = sdCon.free_dsReader(sb.ToString());

            //int iX = 0;
            string[] hArray = new string[1];

            //while (dr.Read())
            //{
            //    if (iX > 0)
            //    {
            //        Array.Resize(ref hArray, iX + 1);
            //    }

            //    hArray[iX] = dr["CategoryID"].ToString() + "," + dr["CategoryCode"].ToString();
            //    iX++;
            //}

            //dr.Close();
            //sdCon.Close();

            return hArray;
        }

        //private void gcMultiRow2_CellEnter(object sender, CellEventArgs e)
        //{
        //    if (gcMultiRow2.EditMode == EditMode.EditProgrammatically)
        //    {
        //        gcMultiRow2.BeginEdit(true);
        //    }
        //}

        //private void gcMultiRow1_CellEnter(object sender, CellEventArgs e)
        //{
        //    if (gcMultiRow1.EditMode == EditMode.EditProgrammatically)
        //    {
        //        gcMultiRow1.BeginEdit(true);
        //    }
        //}

        //private void gcMultiRow1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        //{
        //    string colName = gcMultiRow1.CurrentCell.Name;

        //    if (colName == "chkReFax")
        //    {
        //        if (gcMultiRow1.IsCurrentCellDirty)
        //        {
        //            gcMultiRow1.CommitEdit(DataErrorContexts.Commit);
        //            gcMultiRow1.Refresh();
        //        }
        //    }
        //}

        //private void gcMultiRow1_CellLeave(object sender, CellEventArgs e)
        //{
           
        //}

        //private void gcMultiRow1_CellContentClick(object sender, CellEventArgs e)
        //{
        //    // 2018/08/02
        //    if (e.CellName == "chkReFax")
        //    {
        //        if (Convert.ToInt32(gcMultiRow1[0, "chkReFax"].Value) == global.flgOn)
        //        {
        //            gcMultiRow1[0, "labelCell2"].Style.BackColor = Color.Red;
        //        }
        //        else
        //        {
        //            gcMultiRow1[0, "labelCell2"].Style.BackColor = Color.FromArgb(225, 243, 190);
        //        }
        //    }

        //    // 2018/08/02
        //    if (e.CellName == "buttonCell1")
        //    {
        //        if (Convert.ToInt32(gcMultiRow1[0, "chkReFax"].Value) == global.flgOff)
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            if (MessageBox.Show("表示中の発注書を再FAXフォルダへ移動しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
        //            {
        //                return;
        //            }
        //            else
        //            {
        //                // 画像を再FAXフォルダへ移動
        //                moveReFax(cI);
        //                MessageBox.Show("発注書データを再FAXフォルダへ移動しました", "発注書移動", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
        //                // 件数カウント
        //                if (dtsC.FAX注文書.Count() > 0)
        //                {
        //                    // カレントレコードインデックスを再設定
        //                    if (dtsC.FAX注文書.Count() - 1 < cI) cI = dtsC.FAX注文書.Count() - 1;

        //                    // データ画面表示
        //                    showOcrData(cI);
        //                }
        //                else
        //                {
        //                    // ゼロならばプログラム終了
        //                    MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        //                    //終了処理
        //                    this.Tag = END_NODATA;
        //                    this.Close();
        //                }
        //            }                        
        //        }               
        //    }

            //if (e.CellName == "btnCell")
            //{
            //    //カレントデータの更新
            //    CurDataUpDate(cID[cI]);
                
            //    int sMID = Utility.StrtoInt(gcMultiRow1[e.RowIndex, "txtID"].Value.ToString());

            //    if (dts.勤務票明細.Any(a => a.ID == sMID))
            //    {
            //        var s = dts.勤務票明細.Single(a => a.ID == sMID);
            //        string kID = s.帰宅後勤務ID;
            //        frmKitakugo frm = new frmKitakugo(_dbName, sMID, kID, hArray, bs, true);
            //        frm.ShowDialog();

            //        // 帰宅後勤務データ再読み込み
            //        tAdp.Fill(dts.帰宅後勤務);

            //        //// 勤務票明細再読み込み
            //        //adpMn.勤務票明細TableAdapter.Fill(dts.勤務票明細);

            //        // データ再表示
            //        showOcrData(cI);
            //    }
            //}
        //}

        ///------------------------------------------------------------------------
        /// <summary>
        ///     画像ファイルを再FAXフォルダへ移動して勤務データを削除 </summary>
        /// <param name="i">
        ///     IDインデックス</param>
        ///------------------------------------------------------------------------
        private void moveReFax(int i)
        {
            //string sImgNm = string.Empty;
            //string _fromImgFile = string.Empty;
            //string _toImgFile = string.Empty;
            //string errMsg = string.Empty;

            //// 勤務票データ再FAXフォルダへ移動
            //try
            //{
            //    // IDを取得します
            //    STSH_CLIDataSet.FAX注文書Row r = dtsC.FAX注文書.Single(a => a.ID == cID[i]);
                
            //    // 画像ファイルを再FAXフォルダへ移動
            //    _fromImgFile = Properties.Settings.Default.mydataPath + r.画像名.ToString();
            //    _toImgFile = Properties.Settings.Default.reFaxPath + r.画像名.ToString();
                
            //    System.IO.File.Move(_fromImgFile, _toImgFile);

            //    // データテーブルから勤務票データを削除します
            //    errMsg = "FAX注文書データ";
            //    r.Delete();

            //    // データベース更新
            //    fAdp.Update(dtsC.FAX注文書);
                
            //    // 配列キー再構築
            //    keyArrayCreate();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(errMsg + "の削除に失敗しました" + Environment.NewLine + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //finally
            //{
            //}


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //frmOCRIndex frm = new frmOCRIndex(_dbName, dts, hAdp, iAdp);
            //frm.ShowDialog();
            //string hID = frm.hdID;
            //frm.Dispose();

            //if (hID != string.Empty)
            //{
            //    //カレントデータの更新
            //    CurDataUpDate(cID[cI]);

            //    // レコード検索
            //    for (int i = 0; i < cID.Length; i++)
            //    {
            //        if (cID[i] == hID)
            //        {
            //            cI = i;
            //            showOcrData(cI);
            //            break;
            //        }
            //    }
            //}
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void lnkLblClr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void lnkLblDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        //private void gcMultiRow3_CellEnter(object sender, CellEventArgs e)
        //{
        //    if (gcMultiRow3.EditMode == EditMode.EditProgrammatically)
        //    {
        //        gcMultiRow3.BeginEdit(true);
        //    }
        //}

        //private void gcMultiRow3_CellValueChanged(object sender, CellEventArgs e)
        //{
        //    if (!gl.ChangeValueStatus)
        //    {
        //        return;
        //    }

        //    if (e.RowIndex < 0)
        //    {
        //        return;
        //    }

        //    gl.ChangeValueStatus = false;

        //    // 商品名表示
        //    if (e.CellName == "txtHinCode" || e.CellName == "txtHinCode2")
        //    {
        //        gcHinCodeChange(gcMultiRow3, e.CellName, e.RowIndex, false);

        //        if (!showStatus)
        //        {
        //            // 出荷基準判定
        //            kijunCheckMain();
        //        }
        //    }

        //    gl.ChangeValueStatus = true;

        //    // 追加記入があるとき行を赤表示します
        //    if (e.CellName == "txtSuu")
        //    {
        //        if (Utility.NulltoStr(gcMultiRow3[e.RowIndex, "txtSuu"].Value) != string.Empty)
        //        {
        //            gcMultiRow3[e.RowIndex, "txtHinCode"].Style.BackColor = Color.MistyRose;
        //            gcMultiRow3[e.RowIndex, "lblHinName"].Style.BackColor = Color.MistyRose;
        //            gcMultiRow3[e.RowIndex, "txtSuu"].Style.BackColor = Color.MistyRose;
        //        }
        //        else
        //        {
        //            gcMultiRow3[e.RowIndex, "txtHinCode"].Style.BackColor = Color.White;
        //            gcMultiRow3[e.RowIndex, "lblHinName"].Style.BackColor = Color.White;
        //            gcMultiRow3[e.RowIndex, "txtSuu"].Style.BackColor = Color.White;
        //        }

        //        if (!showStatus)
        //        {
        //            // 出荷基準判定
        //            kijunCheckMain();
        //        }
        //    }

        //    if (e.CellName == "txtSuu2")
        //    {
        //        if (Utility.NulltoStr(gcMultiRow3[e.RowIndex, "txtSuu2"].Value) != string.Empty)
        //        {
        //            gcMultiRow3[e.RowIndex, "txtHinCode2"].Style.BackColor = Color.MistyRose;
        //            gcMultiRow3[e.RowIndex, "lblHinName2"].Style.BackColor = Color.MistyRose;
        //            gcMultiRow3[e.RowIndex, "txtSuu2"].Style.BackColor = Color.MistyRose;
        //        }
        //        else
        //        {
        //            gcMultiRow3[e.RowIndex, "txtHinCode2"].Style.BackColor = Color.White;
        //            gcMultiRow3[e.RowIndex, "lblHinName2"].Style.BackColor = Color.White;
        //            gcMultiRow3[e.RowIndex, "txtSuu2"].Style.BackColor = Color.White;
        //        }
                                
        //        if (!showStatus)
        //        {
        //            // 出荷基準判定
        //            kijunCheckMain();
        //        }
        //    }
        //}

        /////------------------------------------------------------------------------
        ///// <summary>
        /////     商品コードから商品名を表示する </summary>
        ///// <param name="gc">
        /////     GcMultiRowオブジェクト</param>
        ///// <param name="cCellName">
        /////     該当セルの名前</param>
        ///// <param name="rIndex">
        /////     該当セルのrowIndex</param>
        ///// <param name="iriTani">
        /////     true:入数、単位も表示する、false:入数、単位は表示しない</param>
        /////------------------------------------------------------------------------
        //private void gcHinCodeChange(GcMultiRow gc, string cCellName, int rIndex, bool iriTani)
        //{
        //    string hinCode = string.Empty;

        //    if (cCellName == "txtHinCode")
        //    {
        //        hinCode = Utility.NulltoStr(gc[rIndex, "txtHinCode"].Value).PadLeft(8, '0');

        //        if (hinCode != "00000000")
        //        {
        //            gc[rIndex, "txtHinCode"].Value = hinCode;
        //        }

        //        gc[rIndex, "lblHinName"].Value = string.Empty;

        //        if (iriTani)
        //        {
        //            gc[rIndex, "lblIrisu"].Value = string.Empty;
        //            gc[rIndex, "lblTani"].Value = string.Empty;
        //        }
        //    }
        //    else if (cCellName == "txtHinCode2")
        //    {
        //        hinCode = Utility.NulltoStr(gc[rIndex, "txtHinCode2"].Value).PadLeft(8, '0');

        //        if (hinCode != "00000000")
        //        {
        //            gc[rIndex, "txtHinCode2"].Value = hinCode;
        //        }

        //        gc[rIndex, "lblHinName2"].Value = string.Empty;

        //        if (iriTani)
        //        {
        //            gc[rIndex, "lblIrisu2"].Value = string.Empty;
        //            gc[rIndex, "lblTani2"].Value = string.Empty;
        //        }
        //    }

        //    string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
        //    OracleCommand Cmd = new OracleCommand(strSQL, Conn);
        //    OracleDataReader dR = Cmd.ExecuteReader();

        //    while (dR.Read())
        //    {
        //        if (cCellName == "txtHinCode")
        //        {
        //            gc[rIndex, "lblHinName"].Value = dR["SYO_NAME"].ToString().Trim();

        //            if (iriTani)
        //            {
        //                gc[rIndex, "lblIrisu"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
        //                gc[rIndex, "lblTani"].Value = dR["SYO_TANI"].ToString().Trim();
        //            }
        //        }
        //        else if (cCellName == "txtHinCode2")
        //        {
        //            gc[rIndex, "lblHinName2"].Value = dR["SYO_NAME"].ToString().Trim();

        //            if (iriTani)
        //            {
        //                gc[rIndex, "lblIrisu2"].Value = dR["SYO_IRI_KESU"].ToString().Trim();
        //                gc[rIndex, "lblTani2"].Value = dR["SYO_TANI"].ToString().Trim();
        //            }
        //        }
        //    }

        //    dR.Dispose();
        //    Cmd.Dispose();
        //}



        //private void gcMultiRow3_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        //{
        //    if (e.Control is TextBoxEditingControl)
        //    {
        //        //イベントハンドラが複数回追加されてしまうので最初に削除する
        //        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
        //        e.Control.KeyDown -= new KeyEventHandler(Control_KeyDownHin);

        //        // 数字のみ入力可能とする
        //        if (gcMultiRow3.CurrentCell.Name == "txtHinCode" || gcMultiRow3.CurrentCell.Name == "txtHinCode2" ||
        //            gcMultiRow3.CurrentCell.Name == "txtSuu" || gcMultiRow3.CurrentCell.Name == "txtSuu2")
        //        {
        //            //イベントハンドラを追加する
        //            e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
        //        }

        //        // 商品検索画面呼出
        //        if (gcMultiRow3.CurrentCell.Name == "txtHinCode" || gcMultiRow3.CurrentCell.Name == "txtHinCode2")
        //        {
        //            //イベントハンドラを追加する
        //            e.Control.KeyDown += new KeyEventHandler(Control_KeyDownHin);
        //        }
        //    }
        //}

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // エラーチェック
            errCheckClick();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     エラーチェック実行 </summary>
        ///---------------------------------------------------------
        private void errCheckClick()
        {
            // 非ログ書き込み状態とする：2015/09/25
            editLogStatus = false;

            // OCRDataクラス生成
            OCRData ocr = new OCRData();

            // エラーチェックを実行
            if (getErrData(cI, ocr))
            {
                MessageBox.Show("エラーはありませんでした", "エラーチェック", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //gcMultiRow1.CurrentCell = null;
                //gcMultiRow2.CurrentCell = null;
                //gcMultiRow3.CurrentCell = null;

                // データ表示
                showOcrData(cI);
            }
            else
            {
                // カレントインデックスをエラーありインデックスで更新
                cI = ocr._errHeaderIndex;

                // データ表示
                showOcrData(cI);

                // エラー表示
                ErrShow(ocr);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            // 非ログ書き込み状態とする
            editLogStatus = false;

            // 楽商TXTデータ出力
            textDataMake();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // FAX発注書削除
            faxDelete();
        }

        ///------------------------------------------------------------------
        /// <summary>
        ///     FAX発注書削除  </summary>
        ///------------------------------------------------------------------
        private void faxDelete()
        {
            //if (MessageBox.Show("表示中のＦＡＸ発注書を削除します。よろしいですか", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            //    return;

            //// 非ログ書き込み状態とする
            //editLogStatus = false;

            //// レコードと画像ファイルを削除する
            //DataDelete(cI);

            //// 件数カウント
            //if (dtsC.FAX注文書.Count() > 0)
            //{
            //    // カレントレコードインデックスを再設定
            //    if (dtsC.FAX注文書.Count() - 1 < cI) cI = dtsC.FAX注文書.Count() - 1;

            //    // データ画面表示
            //    showOcrData(cI);
            //}
            //else
            //{
            //    // ゼロならばプログラム終了
            //    MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //    //終了処理
            //    this.Tag = END_NODATA;
            //    this.Close();
            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 非ログ書き込み状態とする
            editLogStatus = false;

            // フォームを閉じる
            this.Tag = END_BUTTON;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //if (leadImg.ScaleFactor < gl.ZOOM_MAX)
            //{
            //    leadImg.ScaleFactor += gl.ZOOM_STEP;
            //}
            //gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //if (leadImg.ScaleFactor > gl.ZOOM_MIN)
            //{
            //    leadImg.ScaleFactor -= gl.ZOOM_STEP;
            //}
            //gl.miMdlZoomRate = (float)leadImg.ScaleFactor;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = 0;
            showOcrData(cI);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            if (cI > 0)
            {
                cI--;
                showOcrData(cI);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            if (cI + 1 < cID.Length)
            {
                cI++;
                showOcrData(cI);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //カレントデータの更新
            CurDataUpDate(cID[cI]);

            //レコードの移動
            cI = cID.Length - 1;
            showOcrData(cI);
        }

        private void gcMultiRow3_Leave(object sender, EventArgs e)
        {
            //gcMultiRow3.EndEdit();  
        }

        private void gcMultiRow1_Leave(object sender, EventArgs e)
        {
            //gcMultiRow1.EndEdit();
        }

        private void gcMultiRow2_Leave(object sender, EventArgs e)
        {
            //gcMultiRow2.EndEdit();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(_imgFile);
            //e.Graphics.DrawImage(img, 0, 0);

            // 2017/12/12 縮小
            //e.Graphics.DrawImage(img, 0, 0, img.Width * 49 / 100, img.Height * 49 / 100);

            // 2018/06/21 元画像のピクセル調整を行わないことによる縮小調整
            e.Graphics.DrawImage(img, 0, 0, img.Width * 47 / 100, img.Height * 47 / 100);
            e.HasMorePages = false;

            img.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            // 印刷実行
            printDocument1.Print();
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     保留処理 </summary>
        /// <param name="iX">
        ///     データインデックス</param>
        ///----------------------------------------------------------
        private void setHoldData(string iX)
        {
            //try
            //{
            //    STSH_CLIDataSetTableAdapters.保留注文書TableAdapter hAdp = new STSH_CLIDataSetTableAdapters.保留注文書TableAdapter();
            //    hAdp.Fill(dtsC.保留注文書);

            //    var t = dtsC.FAX注文書.Single(a => a.ID == iX);

            //    STSH_CLIDataSet.保留注文書Row hr = dtsC.保留注文書.New保留注文書Row();
            //    hr.ID = t.ID;
            //    hr.画像名 = t.画像名;
            //    hr.届先番号 = t.届先番号;
            //    hr.パターンID = t.パターンID;
            //    hr.発注番号 = t.発注番号;
            //    hr.納品希望月 = t.納品希望月;
            //    hr.納品希望日 = t.納品希望日;
            //    hr.注文数1 = t.注文数1;
            //    hr.注文数2 = t.注文数2;
            //    hr.注文数3 = t.注文数3;
            //    hr.注文数4 = t.注文数4;
            //    hr.注文数5 = t.注文数5;
            //    hr.注文数6 = t.注文数6;
            //    hr.注文数7 = t.注文数7;
            //    hr.注文数8 = t.注文数8;
            //    hr.注文数9 = t.注文数9;
            //    hr.注文数10 = t.注文数10;
            //    hr.注文数11 = t.注文数11;
            //    hr.注文数12 = t.注文数12;
            //    hr.注文数13 = t.注文数13;
            //    hr.注文数14 = t.注文数14;
            //    hr.注文数15 = t.注文数15;
            //    hr.注文数16 = t.注文数16;
            //    hr.注文数17 = t.注文数17;
            //    hr.注文数18 = t.注文数18;
            //    hr.注文数19 = t.注文数19;
            //    hr.注文数20 = t.注文数20;
            //    hr.注文数21 = t.注文数21;
            //    hr.注文数22 = t.注文数22;
            //    hr.注文数23 = t.注文数23;
            //    hr.注文数24 = t.注文数24;
            //    hr.注文数25 = t.注文数25;
            //    hr.注文数26 = t.注文数26;
            //    hr.注文数27 = t.注文数27;
            //    hr.注文数28 = t.注文数28;
            //    hr.注文数29 = t.注文数29;
            //    hr.注文数30 = t.注文数30;
            //    hr.追加注文チェック = t.追加注文チェック;
            //    hr.追加注文数1 = t.追加注文数1;
            //    hr.追加注文数2 = t.追加注文数2;
            //    hr.追加注文数3 = t.追加注文数3;
            //    hr.追加注文数4 = t.追加注文数4;
            //    hr.追加注文数5 = t.追加注文数5;
            //    hr.追加注文数6 = t.追加注文数6;
            //    hr.追加注文数7 = t.追加注文数7;
            //    hr.追加注文数8 = t.追加注文数8;
            //    hr.追加注文数9 = t.追加注文数9;
            //    hr.追加注文数10 = t.追加注文数10;

            //    hr.追加注文商品コード1 = t.追加注文商品コード1;
            //    hr.追加注文商品コード2 = t.追加注文商品コード2;
            //    hr.追加注文商品コード3 = t.追加注文商品コード3;
            //    hr.追加注文商品コード4 = t.追加注文商品コード4;
            //    hr.追加注文商品コード5 = t.追加注文商品コード5;
            //    hr.追加注文商品コード6 = t.追加注文商品コード6;
            //    hr.追加注文商品コード7 = t.追加注文商品コード7;
            //    hr.追加注文商品コード8 = t.追加注文商品コード8;
            //    hr.追加注文商品コード9 = t.追加注文商品コード9;
            //    hr.追加注文商品コード10 = t.追加注文商品コード10;

            //    hr.担当者コード = t.担当者コード;
            //    hr.備考欄記入 = t.備考欄記入;
            //    hr.メモ = t.メモ;
            //    hr.エラー有無 = t.エラー有無;
            //    hr.更新年月日 = DateTime.Now;
            //    hr.確認 = t.確認;

            //    hr.出荷基準A = t.出荷基準A;
            //    hr.出荷基準B = t.出荷基準B;
            //    hr.出荷基準C = t.出荷基準C;
            //    hr.出荷基準D = t.出荷基準D;
            //    hr.出荷基準E = t.出荷基準E;
            //    hr.出荷基準F = t.出荷基準F;
            //    hr.出荷基準G = t.出荷基準G;

            //    // 2017/08/23
            //    hr.商品コード1 = t.商品コード1;
            //    hr.商品コード2 = t.商品コード2;
            //    hr.商品コード3 = t.商品コード3;
            //    hr.商品コード4 = t.商品コード4;
            //    hr.商品コード5 = t.商品コード5;
            //    hr.商品コード6 = t.商品コード6;
            //    hr.商品コード7 = t.商品コード7;
            //    hr.商品コード8 = t.商品コード8;
            //    hr.商品コード9 = t.商品コード9;
            //    hr.商品コード10 = t.商品コード10;

            //    hr.商品コード11 = t.商品コード11;
            //    hr.商品コード12 = t.商品コード12;
            //    hr.商品コード13 = t.商品コード13;
            //    hr.商品コード14 = t.商品コード14;
            //    hr.商品コード15 = t.商品コード15;
            //    hr.商品コード16 = t.商品コード16;
            //    hr.商品コード17 = t.商品コード17;
            //    hr.商品コード18 = t.商品コード18;
            //    hr.商品コード19 = t.商品コード19;
            //    hr.商品コード20 = t.商品コード20;

            //    hr.商品コード21 = t.商品コード21;
            //    hr.商品コード22 = t.商品コード22;
            //    hr.商品コード23 = t.商品コード23;
            //    hr.商品コード24 = t.商品コード24;
            //    hr.商品コード25 = t.商品コード25;
            //    hr.商品コード26 = t.商品コード26;
            //    hr.商品コード27 = t.商品コード27;
            //    hr.商品コード28 = t.商品コード28;
            //    hr.商品コード29 = t.商品コード29;
            //    hr.商品コード30 = t.商品コード30;

            //    // 保留データ追加処理
            //    dtsC.保留注文書.Add保留注文書Row(hr);
            //    hAdp.Update(dtsC.保留注文書);

            //    // ＦＡＸ発注書データ削除
            //    t.Delete();
            //    fAdp.Update(dtsC.FAX注文書);

            //    // 配列キー再構築
            //    keyArrayCreate();

            //    // 終了メッセージ
            //    MessageBox.Show("注文書が保留されました", "ＦＡＸ発注書保留", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    // 件数カウント
            //    if (dtsC.FAX注文書.Count() > 0)
            //    {
            //        // カレントレコードインデックスを再設定
            //        if (dtsC.FAX注文書.Count() - 1 < cI)
            //        {
            //            cI = dtsC.FAX注文書.Count() - 1;
            //        }

            //        // データ画面表示
            //        showOcrData(cI);
            //    }
            //    else
            //    {
            //        // ゼロならばプログラム終了
            //        MessageBox.Show("全ての発注書データが削除されました。処理を終了します。", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        //終了処理
            //        this.Tag = END_NODATA;
            //        this.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中のＦＡＸ発注書を保留にします。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            //カレントデータの更新 : 2017/07/14
            CurDataUpDate(cID[cI]);

            // 保留処理
            setHoldData(cID[cI]);
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            //kijunCheckMain();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            //// 右へ90°回転させる
            //Leadtools.ImageProcessing.RotateCommand rc = new Leadtools.ImageProcessing.RotateCommand();
            //rc.Angle = 90 * 100;
            //rc.FillColor = new Leadtools.RasterColor(255, 255, 255);
            ////rc.Flags = RotateCommandFlags.Bicubic;
            //rc.Flags = Leadtools.ImageProcessing.RotateCommandFlags.Resize;
            //rc.Run(leadImg.Image);
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            //// 左へ90°回転させる
            //Leadtools.ImageProcessing.RotateCommand rc = new Leadtools.ImageProcessing.RotateCommand();
            //rc.Angle = -90 * 100;
            //rc.FillColor = new Leadtools.RasterColor(255, 255, 255);
            ////rc.Flags = RotateCommandFlags.Bicubic;
            //rc.Flags = Leadtools.ImageProcessing.RotateCommandFlags.Resize;
            //rc.Run(leadImg.Image);
        }

        ///-----------------------------------------------------------------------
        /// <summary>
        ///     CSVデータをSQLiteに登録する </summary>
        /// <param name="_inPath">
        ///     CSVデータパス</param>
        /// <param name="frmP">
        ///     プログレスバーフォームオブジェクト</param>
        ///-----------------------------------------------------------------------
        private void CsvToSQLite(string _inPath, frmPrg frmP)
        {
            ClsFaxOrder order = null;

            try
            {
                // 対象CSVファイル数を取得
                int cLen = System.IO.Directory.GetFiles(_inPath, "*.csv").Count();

                //CSVデータをSQLiteへ取込
                int cCnt = 0;
                foreach (string files in System.IO.Directory.GetFiles(_inPath, "*.csv"))
                {
                    //件数カウント
                    cCnt++;

                    //プログレスバー表示
                    frmP.Text = "OCR変換CSVデータロード中　" + cCnt.ToString() + "/" + cLen.ToString();
                    frmP.progressValue = cCnt * 100 / cLen;
                    frmP.ProgressStep();

                    int mCnt = 1;

                    // CSVファイルインポート
                    foreach (var stBuffer in System.IO.File.ReadAllLines(files, Encoding.Default))
                    {
                        // カンマ区切りで分割して配列に格納する
                        string[] stCSV = stBuffer.Split(',');

                        // ヘッダ
                        if (stCSV[0] == "*")
                        {
                            order = new ClsFaxOrder
                            {
                                ID = Utility.GetStringSubMax(stCSV[1].Trim(), 17),
                                ImageFileName = Utility.GetStringSubMax(stCSV[1].Trim(), 21),
                                TokuisakiCode = Utility.StrtoInt(stCSV[5].Trim()),
                                patternID = Utility.StrtoInt(stCSV[4].Trim()),
                                SeqNumber = Utility.StrtoInt(stCSV[6].Trim()),
                                Year = Utility.StrtoInt(stCSV[2].Trim()),
                                Month = Utility.StrtoInt(stCSV[3].Trim()),
                                Day1 = stCSV[7].Trim(),
                                Day2 = stCSV[8].Trim(),
                                Day3 = stCSV[9].Trim(),
                                Day4 = stCSV[10].Trim(),
                                Day5 = stCSV[11].Trim(),
                                Day6 = stCSV[12].Trim(),
                                Day7 = stCSV[13].Trim()
                            };
                        }
                        else
                        {
                            switch (mCnt)
                            {
                                case 1:
                                    order.Goods1_1 = stCSV[0].Trim();
                                    order.Goods1_2 = stCSV[1].Trim();
                                    order.Goods1_3 = stCSV[2].Trim();
                                    order.Goods1_4 = stCSV[3].Trim();
                                    order.Goods1_5 = stCSV[4].Trim();
                                    order.Goods1_6 = stCSV[5].Trim();
                                    order.Goods1_7 = stCSV[6].Trim();
                                    break;

                                case 2:
                                    order.Goods2_1 = stCSV[0].Trim();
                                    order.Goods2_2 = stCSV[1].Trim();
                                    order.Goods2_3 = stCSV[2].Trim();
                                    order.Goods2_4 = stCSV[3].Trim();
                                    order.Goods2_5 = stCSV[4].Trim();
                                    order.Goods2_6 = stCSV[5].Trim();
                                    order.Goods2_7 = stCSV[6].Trim();
                                    break;

                                case 3:
                                    order.Goods3_1 = stCSV[0].Trim();
                                    order.Goods3_2 = stCSV[1].Trim();
                                    order.Goods3_3 = stCSV[2].Trim();
                                    order.Goods3_4 = stCSV[3].Trim();
                                    order.Goods3_5 = stCSV[4].Trim();
                                    order.Goods3_6 = stCSV[5].Trim();
                                    order.Goods3_7 = stCSV[6].Trim();
                                    break;

                                case 4:
                                    order.Goods4_1 = stCSV[0].Trim();
                                    order.Goods4_2 = stCSV[1].Trim();
                                    order.Goods4_3 = stCSV[2].Trim();
                                    order.Goods4_4 = stCSV[3].Trim();
                                    order.Goods4_5 = stCSV[4].Trim();
                                    order.Goods4_6 = stCSV[5].Trim();
                                    order.Goods4_7 = stCSV[6].Trim();
                                    break;

                                case 5:
                                    order.Goods5_1 = stCSV[0].Trim();
                                    order.Goods5_2 = stCSV[1].Trim();
                                    order.Goods5_3 = stCSV[2].Trim();
                                    order.Goods5_4 = stCSV[3].Trim();
                                    order.Goods5_5 = stCSV[4].Trim();
                                    order.Goods5_6 = stCSV[5].Trim();
                                    order.Goods5_7 = stCSV[6].Trim();
                                    break;

                                case 6:
                                    order.Goods6_1 = stCSV[0].Trim();
                                    order.Goods6_2 = stCSV[1].Trim();
                                    order.Goods6_3 = stCSV[2].Trim();
                                    order.Goods6_4 = stCSV[3].Trim();
                                    order.Goods6_5 = stCSV[4].Trim();
                                    order.Goods6_6 = stCSV[5].Trim();
                                    order.Goods6_7 = stCSV[6].Trim();
                                    break;

                                case 7:
                                    order.Goods7_1 = stCSV[0].Trim();
                                    order.Goods7_2 = stCSV[1].Trim();
                                    order.Goods7_3 = stCSV[2].Trim();
                                    order.Goods7_4 = stCSV[3].Trim();
                                    order.Goods7_5 = stCSV[4].Trim();
                                    order.Goods7_6 = stCSV[5].Trim();
                                    order.Goods7_7 = stCSV[6].Trim();
                                    break;

                                case 8:
                                    order.Goods8_1 = stCSV[0].Trim();
                                    order.Goods8_2 = stCSV[1].Trim();
                                    order.Goods8_3 = stCSV[2].Trim();
                                    order.Goods8_4 = stCSV[3].Trim();
                                    order.Goods8_5 = stCSV[4].Trim();
                                    order.Goods8_6 = stCSV[5].Trim();
                                    order.Goods8_7 = stCSV[6].Trim();
                                    break;

                                case 9:
                                    order.Goods9_1 = stCSV[0].Trim();
                                    order.Goods9_2 = stCSV[1].Trim();
                                    order.Goods9_3 = stCSV[2].Trim();
                                    order.Goods9_4 = stCSV[3].Trim();
                                    order.Goods9_5 = stCSV[4].Trim();
                                    order.Goods9_6 = stCSV[5].Trim();
                                    order.Goods9_7 = stCSV[6].Trim();
                                    break;

                                case 10:
                                    order.Goods10_1 = stCSV[0].Trim();
                                    order.Goods10_2 = stCSV[1].Trim();
                                    order.Goods10_3 = stCSV[2].Trim();
                                    order.Goods10_4 = stCSV[3].Trim();
                                    order.Goods10_5 = stCSV[4].Trim();
                                    order.Goods10_6 = stCSV[5].Trim();
                                    order.Goods10_7 = stCSV[6].Trim();
                                    break;

                                case 11:
                                    order.Goods11_1 = stCSV[0].Trim();
                                    order.Goods11_2 = stCSV[1].Trim();
                                    order.Goods11_3 = stCSV[2].Trim();
                                    order.Goods11_4 = stCSV[3].Trim();
                                    order.Goods11_5 = stCSV[4].Trim();
                                    order.Goods11_6 = stCSV[5].Trim();
                                    order.Goods11_7 = stCSV[6].Trim();
                                    break;

                                case 12:
                                    order.Goods12_1 = stCSV[0].Trim();
                                    order.Goods12_2 = stCSV[1].Trim();
                                    order.Goods12_3 = stCSV[2].Trim();
                                    order.Goods12_4 = stCSV[3].Trim();
                                    order.Goods12_5 = stCSV[4].Trim();
                                    order.Goods12_6 = stCSV[5].Trim();
                                    order.Goods12_7 = stCSV[6].Trim();
                                    break;

                                case 13:
                                    order.Goods13_1 = stCSV[0].Trim();
                                    order.Goods13_2 = stCSV[1].Trim();
                                    order.Goods13_3 = stCSV[2].Trim();
                                    order.Goods13_4 = stCSV[3].Trim();
                                    order.Goods13_5 = stCSV[4].Trim();
                                    order.Goods13_6 = stCSV[5].Trim();
                                    order.Goods13_7 = stCSV[6].Trim();
                                    break;

                                case 14:
                                    order.Goods14_1 = stCSV[0].Trim();
                                    order.Goods14_2 = stCSV[1].Trim();
                                    order.Goods14_3 = stCSV[2].Trim();
                                    order.Goods14_4 = stCSV[3].Trim();
                                    order.Goods14_5 = stCSV[4].Trim();
                                    order.Goods14_6 = stCSV[5].Trim();
                                    order.Goods14_7 = stCSV[6].Trim();
                                    break;

                                case 15:
                                    order.Goods15_1 = stCSV[0].Trim();
                                    order.Goods15_2 = stCSV[1].Trim();
                                    order.Goods15_3 = stCSV[2].Trim();
                                    order.Goods15_4 = stCSV[3].Trim();
                                    order.Goods15_5 = stCSV[4].Trim();
                                    order.Goods15_6 = stCSV[5].Trim();
                                    order.Goods15_7 = stCSV[6].Trim();
                                    break;

                                default:
                                    break;
                            }

                            mCnt++;
                        }
                    }

                    order.Goods16_1 = string.Empty;
                    order.Goods16_2 = string.Empty;
                    order.Goods16_3 = string.Empty;
                    order.Goods16_4 = string.Empty;
                    order.Goods16_5 = string.Empty;
                    order.Goods16_6 = string.Empty;
                    order.Goods16_7 = string.Empty;

                    order.Goods17_1 = string.Empty;
                    order.Goods17_2 = string.Empty;
                    order.Goods17_3 = string.Empty;
                    order.Goods17_4 = string.Empty;
                    order.Goods17_5 = string.Empty;
                    order.Goods17_6 = string.Empty;
                    order.Goods17_7 = string.Empty;

                    order.Goods18_1 = string.Empty;
                    order.Goods18_2 = string.Empty;
                    order.Goods18_3 = string.Empty;
                    order.Goods18_4 = string.Empty;
                    order.Goods18_5 = string.Empty;
                    order.Goods18_6 = string.Empty;
                    order.Goods18_7 = string.Empty;

                    order.Goods19_1 = string.Empty;
                    order.Goods19_2 = string.Empty;
                    order.Goods19_3 = string.Empty;
                    order.Goods19_4 = string.Empty;
                    order.Goods19_5 = string.Empty;
                    order.Goods19_6 = string.Empty;
                    order.Goods19_7 = string.Empty;

                    order.Goods20_1 = string.Empty;
                    order.Goods20_2 = string.Empty;
                    order.Goods20_3 = string.Empty;
                    order.Goods20_4 = string.Empty;
                    order.Goods20_5 = string.Empty;
                    order.Goods20_6 = string.Empty;
                    order.Goods20_7 = string.Empty;

                    order.G_Code1 = string.Empty;
                    order.G_Code2 = string.Empty;
                    order.G_Code3 = string.Empty;
                    order.G_Code4 = string.Empty;
                    order.G_Code5 = string.Empty;
                    order.G_Code6 = string.Empty;
                    order.G_Code7 = string.Empty;
                    order.G_Code8 = string.Empty;
                    order.G_Code9 = string.Empty;
                    order.G_Code10 = string.Empty;
                    order.G_Code11 = string.Empty;
                    order.G_Code12 = string.Empty;
                    order.G_Code13 = string.Empty;
                    order.G_Code14 = string.Empty;
                    order.G_Code15 = string.Empty;
                    order.G_Code16 = string.Empty;
                    order.G_Code17 = string.Empty;
                    order.G_Code18 = string.Empty;
                    order.G_Code19 = string.Empty;
                    order.G_Code20 = string.Empty;

                    order.G_Syubai1 = global.flgOff;
                    order.G_Syubai2 = global.flgOff;
                    order.G_Syubai3 = global.flgOff;
                    order.G_Syubai4 = global.flgOff;
                    order.G_Syubai5 = global.flgOff;
                    order.G_Syubai6 = global.flgOff;
                    order.G_Syubai7 = global.flgOff;
                    order.G_Syubai8 = global.flgOff;
                    order.G_Syubai9 = global.flgOff;
                    order.G_Syubai10 = global.flgOff;
                    order.G_Syubai11 = global.flgOff;
                    order.G_Syubai12 = global.flgOff;
                    order.G_Syubai13 = global.flgOff;
                    order.G_Syubai14 = global.flgOff;
                    order.G_Syubai15 = global.flgOff;

                    order.memo = string.Empty;
                    order.Veri = global.flgOff;
                    order.PatternLoad = global.flgOff;
                    order.YyMmDd = DateTime.Now.ToString();

                    // ＦＡＸ発注書データを追加登録する
                    tblFax.InsertOnSubmit(order);
                }

                // ローカルのデータベースを更新
                context.SubmitChanges();

                //CSVファイルを削除する
                foreach (string files in System.IO.Directory.GetFiles(_inPath, "*.csv"))
                {
                    System.IO.File.Delete(files);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ＦＡＸ注文書CSVインポート処理", MessageBoxButtons.OK);
            }
            finally
            {
            }
        }

        private void dataGridViewEx1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dv = (DataGridView)sender;

            // 行・列共にヘッダは処理しない
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            // セルの上側の境界線を「境界線なし」に設定
            e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

            //if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            //{
            //    // セルの上側の境界線を「境界線なし」に設定
            //    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            //}
            //else
            //{
            //    // セルの上側の境界線を既定の境界線に設定
            //    e.AdvancedBorderStyle.Top = dg1.AdvancedCellBorderStyle.Top;
            //}

            // 奇数行を対象とする（インデックスは偶数）
            if ((e.RowIndex % 2) == 0 && (e.ColumnIndex == 0 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || 
                e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8 || e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11 || 
                e.ColumnIndex == 12))
            {
                // セルの下側の境界線を「境界線なし」に設定
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                // セルの下側の境界線を既定の境界線に設定
                e.AdvancedBorderStyle.Bottom = dg1.AdvancedCellBorderStyle.Bottom;
            }

            Rectangle rect;
            DataGridViewCell cell;

            // ３列目および６列目以降の処理（入数、発注数）
            if (e.ColumnIndex == 2)
            {
                //rect = e.CellBounds;

                //// 奇数行(1,3,5..行目 = RowIndexは0,2,4..)
                //if (e.RowIndex % 2 == 0)
                //{
                //    cell = dg1[e.ColumnIndex, e.RowIndex + 1];
                //    //一つ下の次のセルの高さを足す
                //    rect.Height += cell.Size.Height;
                //}
                //// 偶数行の処理
                //else
                //{
                //    cell = dg1[e.ColumnIndex, e.RowIndex - 1];
                //    // 一つ上のセルの高さを足し、矩形の座標も一つ上のセルに合わす
                //    rect.Height += cell.Size.Height;
                //    rect.Y -= cell.Size.Height;
                //}
                //// セルボーダーライン分矩形の位置を補正
                //rect.X -= 1;
                //rect.Y -= 1;
                //// 背景、セルボーダーライン、セルの値を描画
                //e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                //e.Graphics.DrawRectangle(new Pen(dv.GridColor), rect);
                //e.CellStyle.WrapMode = DataGridViewTriState.True;
                //TextRenderer.DrawText(e.Graphics,
                //             cell.FormattedValue.ToString(),
                //             e.CellStyle.Font, rect, e.CellStyle.ForeColor,
                //             TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.GlyphOverhangPadding);


                //// イベント　ハンドラ内で処理を行ったことを通知
                //e.Handled = true;
            }

            // 列の結合処理（商品名）
            else if (e.ColumnIndex == 0 )
            {
                // 偶数行のみ列結合（インデックスは奇数 1,3,5,...）
                if (e.RowIndex % 2 != 0)
                {
                    rect = e.CellBounds;
                    cell = dg1[e.ColumnIndex + 1, e.RowIndex];

                    // 一つ右のセルの幅を足す
                    rect.Width += cell.Size.Width;
                    rect.X -= 1;
                    rect.Y -= 1;
                    e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                    e.Graphics.DrawRectangle(new Pen(dv.GridColor), rect);

                    TextRenderer.DrawText(e.Graphics,
                                 e.FormattedValue.ToString(),
                                 e.CellStyle.Font, rect, e.CellStyle.ForeColor,
                                 TextFormatFlags.Left
                                 | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
                else
                {
                    // 奇数行は、結合を行わないので、通常の描画処理に任せる
                    e.Paint(e.ClipBounds, e.PaintParts);
                }
            }
            // 列の結合処理（ＪＡＮ）
            else if (e.ColumnIndex == 4)
            {
                //// 偶数行のみ列結合（インデックスは奇数 1,3,5,...）
                //if (e.RowIndex % 2 != 0)
                //{
                //    rect = e.CellBounds;
                //    cell = dg1[e.ColumnIndex + 1, e.RowIndex];

                //    // 一つ右のセルの幅を足す
                //    rect.Width += cell.Size.Width;
                //    rect.X -= 1;
                //    rect.Y -= 1;
                //    e.Graphics.FillRectangle(new SolidBrush(e.CellStyle.BackColor), rect);
                //    e.Graphics.DrawRectangle(new Pen(dv.GridColor), rect);
                //    TextRenderer.DrawText(e.Graphics,
                //                 e.FormattedValue.ToString(),
                //                 e.CellStyle.Font, rect, e.CellStyle.ForeColor,
                //                 TextFormatFlags.HorizontalCenter
                //                 | TextFormatFlags.VerticalCenter);
                //    e.Handled = true;
                //}
                //else
                //{
                //    // 奇数行は、結合を行わないので、通常の描画処理に任せる
                //    e.Paint(e.ClipBounds, e.PaintParts);
                //}
            }
            else
            {
                // 2列目の偶数行は描画処理をせずに、
                // イベントハンドラ内で処理を完了したこと通知
                if (e.RowIndex % 2 != 0 && e.ColumnIndex == 1)
                {
                    e.Handled = true;
                }

                //// 6列目の偶数行は描画処理をせずに、
                //// イベントハンドラ内で処理を完了したこと通知
                //if (e.RowIndex % 2 != 0 && e.ColumnIndex == 5)
                //{
                //    e.Handled = true;
                //}
            }
        }

        private void dg1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = dg1.Columns[dg1.CurrentCell.ColumnIndex].Name;
            if (colName == colDay1 || colName == colDay2 || colName == colDay3 || colName == colDay4 || colName == colDay5 || colName == colDay6 || colName == colDay7)
            {
                if (dg1.IsCurrentCellDirty)
                {
                    dg1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void frmCorrect_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }

        private void txtSeqNum_TextChanged(object sender, EventArgs e)
        {
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }

        private void ShowFaxPattern(TextBox TokuisakiCD, TextBox PID, TextBox SeqNum)
        {
            string _TokuisakiCD = Utility.NulltoStr(TokuisakiCD.Text);
            string _PID = Utility.NulltoStr(PID.Text);
            string _SeqNum = Utility.NulltoStr(SeqNum.Text);

            if (_TokuisakiCD == string.Empty || _PID == string.Empty || _SeqNum == string.Empty)
            {
                return;
            }
            
            foreach (var t in tblPtn.Where(a => a.TokuisakiCode ==  Utility.StrtoInt(_TokuisakiCD) && 
                            a.SeqNum == Utility.StrtoInt(_PID) && a.SecondNum == Utility.StrtoInt(_SeqNum)))
            {
                if (t.G_Code1 != string.Empty)
                {
                    dg1[colHinCode, 1].Value = t.G_Code1.PadLeft(8, '0');
                    dg1[colMaker, 1].Value = t.G_Name1;
                    //dg1[colHinCode, 1].Value = t.G_Read1;
                }

                if (t.G_Code2 != string.Empty)
                {
                    dg1[colHinCode, 3].Value = t.G_Code2.PadLeft(8, '0');
                    dg1[colMaker, 3].Value = t.G_Name2;
                    //dg1[colHinCode, 3].Value = t.G_Read2;
                }

                if (t.G_Code3 != string.Empty)
                {
                    dg1[colHinCode, 5].Value = t.G_Code3.PadLeft(8, '0');
                    dg1[colMaker, 5].Value = t.G_Name3;
                    //dg1[colHinCode, 5].Value = t.G_Read3;
                }


                if (t.G_Code4 != string.Empty)
                {
                    dg1[colHinCode, 7].Value = t.G_Code4.PadLeft(8, '0');
                    dg1[colMaker, 7].Value = t.G_Name4;
                    //dg1[colHinCode, 7].Value = t.G_Read4;
                }


                if (t.G_Code5 != string.Empty)
                {
                    dg1[colHinCode, 9].Value = t.G_Code5.PadLeft(8, '0');
                    dg1[colMaker, 9].Value = t.G_Name5;
                    //dg1[colHinCode, 9].Value = t.G_Read5;
                }


                if (t.G_Code6 != string.Empty)
                {
                    dg1[colHinCode, 11].Value = t.G_Code6.PadLeft(8, '0');
                    dg1[colMaker, 11].Value = t.G_Name6;
                    //dg1[colHinCode, 11].Value = t.G_Read6;
                }


                if (t.G_Code7 != string.Empty)
                {
                    dg1[colHinCode, 13].Value = t.G_Code7.PadLeft(8, '0');
                    dg1[colMaker, 13].Value = t.G_Name7;
                    //dg1[colHinCode, 13].Value = t.G_Read7;
                }


                if (t.G_Code8 != string.Empty)
                {
                    dg1[colHinCode, 15].Value = t.G_Code8.PadLeft(8, '0');
                    dg1[colMaker, 15].Value = t.G_Name8;
                    //dg1[colHinCode, 15].Value = t.G_Read8;
                }


                if (t.G_Code9 != string.Empty)
                {
                    dg1[colHinCode, 17].Value = t.G_Code9.PadLeft(8, '0');
                    dg1[colMaker, 17].Value = t.G_Name9;
                    //dg1[colHinCode, 17].Value = t.G_Read9;
                }


                if (t.G_Code10 != string.Empty)
                {
                    dg1[colHinCode, 19].Value = t.G_Code10.PadLeft(8, '0');
                    dg1[colMaker, 19].Value = t.G_Name10;
                    //dg1[colHinCode, 19].Value = t.G_Read10;
                }


                if (t.G_Code11 != string.Empty)
                {
                    dg1[colHinCode, 21].Value = t.G_Code11.PadLeft(8, '0');
                    dg1[colMaker, 21].Value = t.G_Name11;
                    //dg1[colHinCode, 21].Value = t.G_Read11;
                }


                if (t.G_Code12 != string.Empty)
                {
                    dg1[colHinCode, 23].Value = t.G_Code12.PadLeft(8, '0');
                    dg1[colMaker, 23].Value = t.G_Name12;
                    //dg1[colHinCode, 23].Value = t.G_Read12;
                }


                if (t.G_Code13 != string.Empty)
                {
                    dg1[colHinCode, 25].Value = t.G_Code13.PadLeft(8, '0');
                    dg1[colMaker, 25].Value = t.G_Name13;
                    //dg1[colHinCode, 25].Value = t.G_Read13;
                }


                if (t.G_Code14 != string.Empty)
                {
                    dg1[colHinCode, 27].Value = t.G_Code14.PadLeft(8, '0');
                    dg1[colMaker, 27].Value = t.G_Name14;
                    //dg1[colHinCode, 27].Value = t.G_Read14;
                }

                if (t.G_Code15 != string.Empty)
                {
                    dg1[colHinCode, 29].Value = t.G_Code15.PadLeft(8, '0');
                    dg1[colMaker, 29].Value = t.G_Name15;
                    //dg1[colHinCode, 29].Value = t.G_Read15;
                }
            }
        }

        private void txtTokuisakiCD_TextChanged(object sender, EventArgs e)
        {
            string _tel = string.Empty;
            string _Jyu = string.Empty;

            // 得意先名表示
            lblTokuisakiName.Text = Utility.getNouhinName(txtTokuisakiCD.Text, out _tel, out _Jyu);

            // 発注書パターン表示
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }



        ///-----------------------------------------------------------
        /// <summary>
        ///     画像表示 openCV：2018/10/24 </summary>
        /// <param name="img">
        ///     表示画像ファイル名</param>
        ///-----------------------------------------------------------
        private void showImage_openCv(string img)
        {
            n_width = B_WIDTH;
            n_height = B_HEIGHT;

            imgShow(img, n_width, n_height);

            trackBar1.Value = 0;
        }


        // GUI上に画像を表示するには、OpenCV上で扱うMat形式をBitmap形式に変換する必要がある
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }


        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン openCV : 2018/10/24 </summary>
        /// <param name="mImg">
        ///     Mat形式イメージ</param>
        /// <param name="w">
        ///     width</param>
        /// <param name="h">
        ///     height</param>
        ///---------------------------------------------------------
        private void imgShow(Mat mImg, float w, float h)
        {
            int cWidth = 0;
            int cHeight = 0;

            Bitmap bt = MatToBitmap(mImg);

            // Bitmapサイズ
            if (panel1.Width < (bt.Width * w) || panel1.Height < (bt.Height * h))
            {
                cWidth = (int)(bt.Width * w);
                cHeight = (int)(bt.Height * h);
            }
            else
            {
                cWidth = panel1.Width;
                cHeight = panel1.Height;
            }

            // Bitmap を生成
            Bitmap canvas = new Bitmap(cWidth, cHeight);

            // ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            // 画像をcanvasの座標(0, 0)の位置に指定のサイズで描画する
            g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

            //メモリクリア
            bt.Dispose();
            g.Dispose();

            // PictureBox1に表示する
            pictureBox1.Image = canvas;
        }


        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン openCV : 2018/10/24 </summary>
        /// <param name="mImg">
        ///     Mat形式イメージ</param>
        /// <param name="w">
        ///     width</param>
        /// <param name="h">
        ///     height</param>
        ///---------------------------------------------------------
        private void imgShow(string filePath, float w, float h)
        {
            mMat = new Mat(filePath, ImreadModes.Grayscale);
            Bitmap bt = MatToBitmap(mMat);

            // Bitmap を生成
            Bitmap canvas = new Bitmap((int)(bt.Width * w), (int)(bt.Height * h));

            Graphics g = Graphics.FromImage(canvas);

            g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

            //メモリクリア
            bt.Dispose();
            g.Dispose();

            pictureBox1.Image = canvas;
        }

        private void dg1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                if ((e.RowIndex % 2) != 0)
                {
                    ClsCsvData.ClsCsvSyohin syohin = Utility.GetSyohinData(Properties.Settings.Default.商品マスター, Properties.Settings.Default.商品在庫マスター, 
                        Properties.Settings.Default.仕入先マスター, Utility.NulltoStr(dg1[e.ColumnIndex, e.RowIndex].Value).PadLeft(8, '0'));

                    dg1[colMaker, e.RowIndex - 1].Value = syohin.SIRESAKI_NM;       // 仕入先名
                    dg1[colMaker, e.RowIndex].Value = syohin.SYOHIN_NM;             // 商品名
                    dg1[colKikaku, e.RowIndex - 1].Value = syohin.SYOHIN_KIKAKU;    // 規格
                    dg1[colIrisu, e.RowIndex].Value = syohin.CASE_IRISU;            // 入数
                    dg1[colNouka, e.RowIndex].Value = syohin.NOUHIN_KARI_TANKA;     // 納価
                    dg1[colBaika, e.RowIndex].Value = syohin.RETAIL_TANKA;          // 売価

                    // 終売のとき
                    if (syohin.SHUBAI)
                    {
                        if (syohin.LAST_SALE_YMD.Length > 7)
                        {
                            dg1[colHinCode, e.RowIndex - 1].Value = syohin.LAST_SALE_YMD.Substring(0, 4) + "/" + 
                                                                    syohin.LAST_SALE_YMD.Substring(4, 2) + "/" + 
                                                                    syohin.LAST_SALE_YMD.Substring(6, 2);

                            dg1[colMaker, e.RowIndex].Style.ForeColor = Color.Red;
                            dg1[colHinCode, e.RowIndex - 1].Style.ForeColor = Color.Red;
                            dg1[colHinCode, e.RowIndex - 1].Style.Font = new Font("MS UI Gothic", 8, FontStyle.Regular);
                        }
                        else
                        {
                            dg1[colHinCode, e.RowIndex - 1].Value = "";
                        }

                        // 終売処理コンボボックスを編集可能とする
                        dg1[colSyubai, e.RowIndex].ReadOnly = false;
                    }
                    else
                    {
                        dg1[colHinCode, e.RowIndex - 1].Value = string.Empty;
                        dg1[colMaker, e.RowIndex].Style.ForeColor = SystemColors.ControlText;
                        dg1[colHinCode, e.RowIndex - 1].Style.ForeColor = SystemColors.ControlText;

                        // 終売処理コンボボックスを編集不可とする
                        dg1[colSyubai, e.RowIndex].ReadOnly = true;
                    }
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            n_width = B_WIDTH + (float)trackBar1.Value * 0.05f;
            n_height = B_HEIGHT + (float)trackBar1.Value * 0.05f;

            imgShow(mMat, n_width, n_height);
        }
        private DataGridViewComboBoxEditingControl dataGridViewComboBox = null;

        private void dg1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //表示されているコントロールがDataGridViewComboBoxEditingControlか調べる
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
                DataGridView dgv = (DataGridView)sender;

                //該当する列か調べる
                //if (dgv.CurrentCell.OwningColumn.Name == "ComboBox")
                //{
                //    //編集のために表示されているコントロールを取得
                //    this.dataGridViewComboBox =
                //        (DataGridViewComboBoxEditingControl)e.Control;
                //    //SelectedIndexChangedイベントハンドラを追加
                //    this.dataGridViewComboBox.SelectedIndexChanged +=
                //        new EventHandler(dataGridViewComboBox_SelectedIndexChanged);
                //}

                //該当する列か調べる
                if (dgv.CurrentCell.OwningColumn.Name == colSyubai && 
                    (dgv.CurrentCell.OwningRow.Index % 2  != 0))
                {
                    //編集のために表示されているコントロールを取得
                    this.dataGridViewComboBox =
                        (DataGridViewComboBoxEditingControl)e.Control;
                    //SelectedIndexChangedイベントハンドラを追加
                    this.dataGridViewComboBox.SelectedIndexChanged +=
                        new EventHandler(dataGridViewComboBox_SelectedIndexChanged);
                }
            }
        }


        //CellEndEditイベントハンドラ
        private void DataGridView1_CellEndEdit(object sender,
            DataGridViewCellEventArgs e)
        {
            //SelectedIndexChangedイベントハンドラを削除
            if (this.dataGridViewComboBox != null)
            {
                this.dataGridViewComboBox.SelectedIndexChanged -=
                    new EventHandler(dataGridViewComboBox_SelectedIndexChanged);
                this.dataGridViewComboBox = null;
            }
        }

        //DataGridViewに表示されているコンボボックスの
        //SelectedIndexChangedイベントハンドラ
        private void dataGridViewComboBox_SelectedIndexChanged(object sender,
            EventArgs e)
        {
            //選択されたアイテムを表示
            //DataGridViewComboBoxEditingControl cb = (DataGridViewComboBoxEditingControl)sender;
            //Console.WriteLine(cb.SelectedItem);
            //MessageBox.Show(cb.SelectedIndex.ToString());
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            frmTodoke frm = new frmTodoke(false);
            frm.ShowDialog();

            if (frm._nouCode != null)
            {
                txtTokuisakiCD.Text = frm._nouCode[0];
            }

            // 後片付け
            frm.Dispose();
        }

        private void dg1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1) && (e.RowIndex % 2 != 0))
            {
                frmSyohin frmSyohin = new frmSyohin(false);
                frmSyohin.ShowDialog();

                if (frmSyohin._nouCode != null)
                {
                    dg1[colHinCode, e.RowIndex].Value = frmSyohin._nouCode[0];
                }

                // 後片付け
                frmSyohin.Dispose();
            }
        }
    }
}
