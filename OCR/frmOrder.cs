﻿using System;
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
using Excel = Microsoft.Office.Interop.Excel;
//using OpenCvSharp;

namespace STSH_OCR.OCR
{
    public partial class frmOrder : Form
    {
        /// ------------------------------------------------------------
        /// <summary>
        ///     コンストラクタ </summary>
        /// <param name="myCode">
        ///     入力担当者コード</param>
        /// ------------------------------------------------------------
        public frmOrder(string myCode)
        {
            InitializeComponent();

            dID = myCode;
        }

        // データベース：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        SQLiteConnection cn2 = null;
        DataContext context2 = null;

        string db_file = Properties.Settings.Default.DB_File;
        string Local_DB = Properties.Settings.Default.Local_DB;

        // 発注書パターンマスター
        Table<Common.ClsOrderPattern> tblPtn = null;
        ClsOrderPattern ClsOrderPattern = null;

        // 環境設定
        Table<Common.ClsSystemConfig> tblConfig = null;
        ClsSystemConfig Config = null;

        // 編集ログデータ
        Table<Common.ClsDataEditLog> tblEditLog = null;
        ClsDataEditLog ClsDataEditLog = null;

        // 発注書データ
        Table<Common.ClsOrder> tblOrder = null;
        ClsOrder Order = null;

        // 発注書データ ※重複チェック用
        Table<Common.ClsOrder> tblOrderCheck = null;
        ClsOrder OrderCheck = null;

        // 得意先別発注履歴
        Table<Common.ClsOrderHistory> tblOrderHistories = null;
        ClsOrderHistory clsOrderHistory = null;

        // セル値
        private string cellName = string.Empty;         // セル名
        private string cellBeforeValue = string.Empty;  // 編集前
        private string cellAfterValue = string.Empty;   // 編集後

        #region 編集ログ・項目名
        private const string LOG_YEAR = "年";
        private const string LOG_MONTH = "月";
        private const string LOG_TOKUISAKICD = "得意先コード";
        private const string LOG_PID = "発注書ID";
        private const string LOG_PSEQNUM = "発注書連番";
        private const string LOG_DAY_1 = "月曜日付";
        private const string LOG_DAY_2 = "火曜日付";
        private const string LOG_DAY_3 = "水曜日付";
        private const string LOG_DAY_4 = "木曜日付";
        private const string LOG_DAY_5 = "金曜日付";
        private const string LOG_DAY_6 = "土曜日付";
        private const string LOG_DAY_7 = "日曜日付";
        private const string CELL_SYOHINCD = "商品コード";
        private const string CELL_NOUKA = "納価";
        private const string CELL_BAIKA = "売価";
        private const string CELL_MON = "(月)発注数";
        private const string CELL_TUE = "(火)発注数";
        private const string CELL_WED = "(水)発注数";
        private const string CELL_THU = "(木)発注数";
        private const string CELL_FRI = "(金)発注数";
        private const string CELL_SAT = "(土)発注数";
        private const string CELL_SUN = "(日)発注数";
        private const string CELL_SHUBAI = "終売処理";
        private const string LOG_DELETE = "登録済み発注書削除";
        #endregion 編集ログ・項目名

        #region 終了ステータス定数
        const string END_BUTTON = "btn";
        const string END_MAKEDATA = "data";
        const string END_CONTOROL = "close";
        const string END_NODATA = "non Data";
        #endregion

        string dID = string.Empty;              // 表示する発注データのID
        string _img = string.Empty;             // 画像名

        bool _eMode = true;

        // dataGridView1_CellEnterステータス
        bool gridViewCellEnterStatus = true;

        // 編集ログ書き込み状態
        bool editLogStatus = false;

        // グローバルクラス
        global gl = new global();

        // 画面表示時ステータス
        bool showStatus = false;
        bool TenDateStatus = false;
        bool PtnShowStatus = false;     // 2020/04/12

        // openCvSharp 関連
        const float B_WIDTH = 0.45f;
        const float B_HEIGHT = 0.45f;
        float n_width = 0f;
        float n_height = 0f;

        //Mat mMat = new Mat(); 

        // 2020/04/14
        Image FaxImg = null;

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

        // 店着日配列
        ClsTenDate[] tenDates = new ClsTenDate[7];

        private void frmCorrect_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // 共有DB接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            tblPtn = context.GetTable<Common.ClsOrderPattern>();                // 登録パターンテーブル
            tblOrder = context.GetTable<Common.ClsOrder>();                     // 発注書テーブル
            tblConfig = context.GetTable<Common.ClsSystemConfig>();             // 環境設定
            tblOrderHistories = context.GetTable<Common.ClsOrderHistory>();     // 得意先別発注履歴
            tblOrderCheck = context.GetTable<Common.ClsOrder>();                // 発注書データ ※チェック用

            // ローカルDB接続
            cn2 = new SQLiteConnection("DataSource=" + Local_DB);
            context2 = new DataContext(cn2);

            tblEditLog = context2.GetTable<Common.ClsDataEditLog>(); // 編集ログテーブル

            // 環境設定読み出し
            Config = (ClsSystemConfig)tblConfig.Single(a => a.ID == global.configKEY);

            // DBオープン
            cn.Open();
                        
            // キャプション
            this.Text = "発注書データ編集";

            GridviewSet(dg1);

            // レコードを表示
            showOcrData(dID);

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
            //int iX = 0;
            //foreach (var t in tblOrder.OrderBy(a => a.ID))
            //{
            //    Array.Resize(ref cID, iX + 1);
            //    cID[iX] = t.ID;
            //    iX++;
            //}
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
                tempDGV.Columns[colSyubai].Width = 52;

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
        
        ///-----------------------------------------------------------------------------------
        /// <summary>
        ///     カレントデータを更新する</summary>
        /// <param name="iX">
        ///     カレントレコードのインデックス</param>
        ///-----------------------------------------------------------------------------------
        private void CurDataUpDate(string sID)
        {
            // エラーメッセージ
            string errMsg = "発注書テーブル更新";

            try
            {
                string Sql = "UPDATE OrderData set ";
                Sql += "得意先コード = " + Utility.NulltoStr(txtTokuisakiCD.Text) + ",";
                Sql += "patternID = " + Utility.NulltoStr(txtPID.Text) + ",";
                Sql += "SeqNumber = " + Utility.NulltoStr(txtSeqNum.Text) + ",";
                Sql += "年 = " + Utility.NulltoStr(txtYear.Text) + ",";
                Sql += "月 = " + Utility.NulltoStr(txtMonth.Text) + ",";
                Sql += "Day1 = '" + Utility.NulltoStr(txtTenDay1.Text) + "',";
                Sql += "Day2 = '" + Utility.NulltoStr(txtTenDay2.Text) + "',";
                Sql += "Day3 = '" + Utility.NulltoStr(txtTenDay3.Text) + "',";
                Sql += "Day4 = '" + Utility.NulltoStr(txtTenDay4.Text) + "',";
                Sql += "Day5 = '" + Utility.NulltoStr(txtTenDay5.Text) + "',";
                Sql += "Day6 = '" + Utility.NulltoStr(txtTenDay6.Text) + "',";
                Sql += "Day7 = '" + Utility.NulltoStr(txtTenDay7.Text) + "',";

                Sql += "Goods1_1 = '" + Utility.NulltoStr(dg1[colDay1, 1].Value) + "',";
                Sql += "Goods1_2 = '" + Utility.NulltoStr(dg1[colDay2, 1].Value) + "',";
                Sql += "Goods1_3 = '" + Utility.NulltoStr(dg1[colDay3, 1].Value) + "',";
                Sql += "Goods1_4 = '" + Utility.NulltoStr(dg1[colDay4, 1].Value) + "',";
                Sql += "Goods1_5 = '" + Utility.NulltoStr(dg1[colDay5, 1].Value) + "',";
                Sql += "Goods1_6 = '" + Utility.NulltoStr(dg1[colDay6, 1].Value) + "',";
                Sql += "Goods1_7 = '" + Utility.NulltoStr(dg1[colDay7, 1].Value) + "',";

                Sql += "Goods2_1 = '" + Utility.NulltoStr(dg1[colDay1, 3].Value) + "',";
                Sql += "Goods2_2 = '" + Utility.NulltoStr(dg1[colDay2, 3].Value) + "',";
                Sql += "Goods2_3 = '" + Utility.NulltoStr(dg1[colDay3, 3].Value) + "',";
                Sql += "Goods2_4 = '" + Utility.NulltoStr(dg1[colDay4, 3].Value) + "',";
                Sql += "Goods2_5 = '" + Utility.NulltoStr(dg1[colDay5, 3].Value) + "',";
                Sql += "Goods2_6 = '" + Utility.NulltoStr(dg1[colDay6, 3].Value) + "',";
                Sql += "Goods2_7 = '" + Utility.NulltoStr(dg1[colDay7, 3].Value) + "',";

                Sql += "Goods3_1 = '" + Utility.NulltoStr(dg1[colDay1, 5].Value) + "',";
                Sql += "Goods3_2 = '" + Utility.NulltoStr(dg1[colDay2, 5].Value) + "',";
                Sql += "Goods3_3 = '" + Utility.NulltoStr(dg1[colDay3, 5].Value) + "',";
                Sql += "Goods3_4 = '" + Utility.NulltoStr(dg1[colDay4, 5].Value) + "',";
                Sql += "Goods3_5 = '" + Utility.NulltoStr(dg1[colDay5, 5].Value) + "',";
                Sql += "Goods3_6 = '" + Utility.NulltoStr(dg1[colDay6, 5].Value) + "',";
                Sql += "Goods3_7 = '" + Utility.NulltoStr(dg1[colDay7, 5].Value) + "',";

                Sql += "Goods4_1 = '" + Utility.NulltoStr(dg1[colDay1, 7].Value) + "',";
                Sql += "Goods4_2 = '" + Utility.NulltoStr(dg1[colDay2, 7].Value) + "',";
                Sql += "Goods4_3 = '" + Utility.NulltoStr(dg1[colDay3, 7].Value) + "',";
                Sql += "Goods4_4 = '" + Utility.NulltoStr(dg1[colDay4, 7].Value) + "',";
                Sql += "Goods4_5 = '" + Utility.NulltoStr(dg1[colDay5, 7].Value) + "',";
                Sql += "Goods4_6 = '" + Utility.NulltoStr(dg1[colDay6, 7].Value) + "',";
                Sql += "Goods4_7 = '" + Utility.NulltoStr(dg1[colDay7, 7].Value) + "',";

                Sql += "Goods5_1 = '" + Utility.NulltoStr(dg1[colDay1, 9].Value) + "',";
                Sql += "Goods5_2 = '" + Utility.NulltoStr(dg1[colDay2, 9].Value) + "',";
                Sql += "Goods5_3 = '" + Utility.NulltoStr(dg1[colDay3, 9].Value) + "',";
                Sql += "Goods5_4 = '" + Utility.NulltoStr(dg1[colDay4, 9].Value) + "',";
                Sql += "Goods5_5 = '" + Utility.NulltoStr(dg1[colDay5, 9].Value) + "',";
                Sql += "Goods5_6 = '" + Utility.NulltoStr(dg1[colDay6, 9].Value) + "',";
                Sql += "Goods5_7 = '" + Utility.NulltoStr(dg1[colDay7, 9].Value) + "',";

                Sql += "Goods6_1 = '" + Utility.NulltoStr(dg1[colDay1, 11].Value) + "',";
                Sql += "Goods6_2 = '" + Utility.NulltoStr(dg1[colDay2, 11].Value) + "',";
                Sql += "Goods6_3 = '" + Utility.NulltoStr(dg1[colDay3, 11].Value) + "',";
                Sql += "Goods6_4 = '" + Utility.NulltoStr(dg1[colDay4, 11].Value) + "',";
                Sql += "Goods6_5 = '" + Utility.NulltoStr(dg1[colDay5, 11].Value) + "',";
                Sql += "Goods6_6 = '" + Utility.NulltoStr(dg1[colDay6, 11].Value) + "',";
                Sql += "Goods6_7 = '" + Utility.NulltoStr(dg1[colDay7, 11].Value) + "',";

                Sql += "Goods7_1 = '" + Utility.NulltoStr(dg1[colDay1, 13].Value) + "',";
                Sql += "Goods7_2 = '" + Utility.NulltoStr(dg1[colDay2, 13].Value) + "',";
                Sql += "Goods7_3 = '" + Utility.NulltoStr(dg1[colDay3, 13].Value) + "',";
                Sql += "Goods7_4 = '" + Utility.NulltoStr(dg1[colDay4, 13].Value) + "',";
                Sql += "Goods7_5 = '" + Utility.NulltoStr(dg1[colDay5, 13].Value) + "',";
                Sql += "Goods7_6 = '" + Utility.NulltoStr(dg1[colDay6, 13].Value) + "',";
                Sql += "Goods7_7 = '" + Utility.NulltoStr(dg1[colDay7, 13].Value) + "',";

                Sql += "Goods8_1 = '" + Utility.NulltoStr(dg1[colDay1, 15].Value) + "',";
                Sql += "Goods8_2 = '" + Utility.NulltoStr(dg1[colDay2, 15].Value) + "',";
                Sql += "Goods8_3 = '" + Utility.NulltoStr(dg1[colDay3, 15].Value) + "',";
                Sql += "Goods8_4 = '" + Utility.NulltoStr(dg1[colDay4, 15].Value) + "',";
                Sql += "Goods8_5 = '" + Utility.NulltoStr(dg1[colDay5, 15].Value) + "',";
                Sql += "Goods8_6 = '" + Utility.NulltoStr(dg1[colDay6, 15].Value) + "',";
                Sql += "Goods8_7 = '" + Utility.NulltoStr(dg1[colDay7, 15].Value) + "',";

                Sql += "Goods9_1 = '" + Utility.NulltoStr(dg1[colDay1, 17].Value) + "',";
                Sql += "Goods9_2 = '" + Utility.NulltoStr(dg1[colDay2, 17].Value) + "',";
                Sql += "Goods9_3 = '" + Utility.NulltoStr(dg1[colDay3, 17].Value) + "',";
                Sql += "Goods9_4 = '" + Utility.NulltoStr(dg1[colDay4, 17].Value) + "',";
                Sql += "Goods9_5 = '" + Utility.NulltoStr(dg1[colDay5, 17].Value) + "',";
                Sql += "Goods9_6 = '" + Utility.NulltoStr(dg1[colDay6, 17].Value) + "',";
                Sql += "Goods9_7 = '" + Utility.NulltoStr(dg1[colDay7, 17].Value) + "',";

                Sql += "Goods10_1 = '" + Utility.NulltoStr(dg1[colDay1, 19].Value) + "',";
                Sql += "Goods10_2 = '" + Utility.NulltoStr(dg1[colDay2, 19].Value) + "',";
                Sql += "Goods10_3 = '" + Utility.NulltoStr(dg1[colDay3, 19].Value) + "',";
                Sql += "Goods10_4 = '" + Utility.NulltoStr(dg1[colDay4, 19].Value) + "',";
                Sql += "Goods10_5 = '" + Utility.NulltoStr(dg1[colDay5, 19].Value) + "',";
                Sql += "Goods10_6 = '" + Utility.NulltoStr(dg1[colDay6, 19].Value) + "',";
                Sql += "Goods10_7 = '" + Utility.NulltoStr(dg1[colDay7, 19].Value) + "',";

                Sql += "Goods11_1 = '" + Utility.NulltoStr(dg1[colDay1, 21].Value) + "',";
                Sql += "Goods11_2 = '" + Utility.NulltoStr(dg1[colDay2, 21].Value) + "',";
                Sql += "Goods11_3 = '" + Utility.NulltoStr(dg1[colDay3, 21].Value) + "',";
                Sql += "Goods11_4 = '" + Utility.NulltoStr(dg1[colDay4, 21].Value) + "',";
                Sql += "Goods11_5 = '" + Utility.NulltoStr(dg1[colDay5, 21].Value) + "',";
                Sql += "Goods11_6 = '" + Utility.NulltoStr(dg1[colDay6, 21].Value) + "',";
                Sql += "Goods11_7 = '" + Utility.NulltoStr(dg1[colDay7, 21].Value) + "',";

                Sql += "Goods12_1 = '" + Utility.NulltoStr(dg1[colDay1, 23].Value) + "',";
                Sql += "Goods12_2 = '" + Utility.NulltoStr(dg1[colDay2, 23].Value) + "',";
                Sql += "Goods12_3 = '" + Utility.NulltoStr(dg1[colDay3, 23].Value) + "',";
                Sql += "Goods12_4 = '" + Utility.NulltoStr(dg1[colDay4, 23].Value) + "',";
                Sql += "Goods12_5 = '" + Utility.NulltoStr(dg1[colDay5, 23].Value) + "',";
                Sql += "Goods12_6 = '" + Utility.NulltoStr(dg1[colDay6, 23].Value) + "',";
                Sql += "Goods12_7 = '" + Utility.NulltoStr(dg1[colDay7, 23].Value) + "',";

                Sql += "Goods13_1 = '" + Utility.NulltoStr(dg1[colDay1, 25].Value) + "',";
                Sql += "Goods13_2 = '" + Utility.NulltoStr(dg1[colDay2, 25].Value) + "',";
                Sql += "Goods13_3 = '" + Utility.NulltoStr(dg1[colDay3, 25].Value) + "',";
                Sql += "Goods13_4 = '" + Utility.NulltoStr(dg1[colDay4, 25].Value) + "',";
                Sql += "Goods13_5 = '" + Utility.NulltoStr(dg1[colDay5, 25].Value) + "',";
                Sql += "Goods13_6 = '" + Utility.NulltoStr(dg1[colDay6, 25].Value) + "',";
                Sql += "Goods13_7 = '" + Utility.NulltoStr(dg1[colDay7, 25].Value) + "',";

                Sql += "Goods14_1 = '" + Utility.NulltoStr(dg1[colDay1, 27].Value) + "',";
                Sql += "Goods14_2 = '" + Utility.NulltoStr(dg1[colDay2, 27].Value) + "',";
                Sql += "Goods14_3 = '" + Utility.NulltoStr(dg1[colDay3, 27].Value) + "',";
                Sql += "Goods14_4 = '" + Utility.NulltoStr(dg1[colDay4, 27].Value) + "',";
                Sql += "Goods14_5 = '" + Utility.NulltoStr(dg1[colDay5, 27].Value) + "',";
                Sql += "Goods14_6 = '" + Utility.NulltoStr(dg1[colDay6, 27].Value) + "',";
                Sql += "Goods14_7 = '" + Utility.NulltoStr(dg1[colDay7, 27].Value) + "',";

                Sql += "Goods15_1 = '" + Utility.NulltoStr(dg1[colDay1, 29].Value) + "',";
                Sql += "Goods15_2 = '" + Utility.NulltoStr(dg1[colDay2, 29].Value) + "',";
                Sql += "Goods15_3 = '" + Utility.NulltoStr(dg1[colDay3, 29].Value) + "',";
                Sql += "Goods15_4 = '" + Utility.NulltoStr(dg1[colDay4, 29].Value) + "',";
                Sql += "Goods15_5 = '" + Utility.NulltoStr(dg1[colDay5, 29].Value) + "',";
                Sql += "Goods15_6 = '" + Utility.NulltoStr(dg1[colDay6, 29].Value) + "',";
                Sql += "Goods15_7 = '" + Utility.NulltoStr(dg1[colDay7, 29].Value) + "',";

                Sql += "G_Code1 = '" + timeVal(dg1[colHinCode, 1].Value, 8) + "',";
                Sql += "G_Code2 = '" + timeVal(dg1[colHinCode, 3].Value, 8) + "',";
                Sql += "G_Code3 = '" + timeVal(dg1[colHinCode, 5].Value, 8) + "',";
                Sql += "G_Code4 = '" + timeVal(dg1[colHinCode, 7].Value, 8) + "',";
                Sql += "G_Code5 = '" + timeVal(dg1[colHinCode, 9].Value, 8) + "',";
                Sql += "G_Code6 = '" + timeVal(dg1[colHinCode, 11].Value, 8) + "',";
                Sql += "G_Code7 = '" + timeVal(dg1[colHinCode, 13].Value, 8) + "',";
                Sql += "G_Code8 = '" + timeVal(dg1[colHinCode, 15].Value, 8) + "',";
                Sql += "G_Code9 = '" + timeVal(dg1[colHinCode, 17].Value, 8) + "',";
                Sql += "G_Code10 = '" + timeVal(dg1[colHinCode, 19].Value, 8) + "',";
                Sql += "G_Code11 = '" + timeVal(dg1[colHinCode, 21].Value, 8) + "',";
                Sql += "G_Code12 = '" + timeVal(dg1[colHinCode, 23].Value, 8) + "',";
                Sql += "G_Code13 = '" + timeVal(dg1[colHinCode, 25].Value, 8) + "',";
                Sql += "G_Code14 = '" + timeVal(dg1[colHinCode, 27].Value, 8) + "',";
                Sql += "G_Code15 = '" + timeVal(dg1[colHinCode, 29].Value, 8) + "',";

                Sql += "G_Nouka1 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 1].Value)) + ",";
                Sql += "G_Nouka2 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 3].Value)) + ",";
                Sql += "G_Nouka3 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 5].Value)) + ",";
                Sql += "G_Nouka4 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 7].Value)) + ",";
                Sql += "G_Nouka5 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 9].Value)) + ",";
                Sql += "G_Nouka6 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 11].Value)) + ",";
                Sql += "G_Nouka7 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 13].Value)) + ",";
                Sql += "G_Nouka8 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 15].Value)) + ",";
                Sql += "G_Nouka9 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 17].Value)) + ",";
                Sql += "G_Nouka10 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 19].Value)) + ",";
                Sql += "G_Nouka11 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 21].Value)) + ",";
                Sql += "G_Nouka12 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 23].Value)) + ",";
                Sql += "G_Nouka13 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 25].Value)) + ",";
                Sql += "G_Nouka14 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 27].Value)) + ",";
                Sql += "G_Nouka15 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colNouka, 29].Value)) + ",";

                Sql += "G_Baika1 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 1].Value)) + ",";
                Sql += "G_Baika2 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 3].Value)) + ",";
                Sql += "G_Baika3 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 5].Value)) + ",";
                Sql += "G_Baika4 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 7].Value)) + ",";
                Sql += "G_Baika5 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 9].Value)) + ",";
                Sql += "G_Baika6 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 11].Value)) + ",";
                Sql += "G_Baika7 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 13].Value)) + ",";
                Sql += "G_Baika8 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 15].Value)) + ",";
                Sql += "G_Baika9 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 17].Value)) + ",";
                Sql += "G_Baika10 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 19].Value)) + ",";
                Sql += "G_Baika11 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 21].Value)) + ",";
                Sql += "G_Baika12 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 23].Value)) + ",";
                Sql += "G_Baika13 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 25].Value)) + ",";
                Sql += "G_Baika14 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 27].Value)) + ",";
                Sql += "G_Baika15 = " + Utility.StrtoInt(Utility.NulltoStr(dg1[colBaika, 29].Value)) + ",";

                Sql += "G_Syubai1 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 1].Value)) + ",";
                Sql += "G_Syubai2 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 3].Value)) + ",";
                Sql += "G_Syubai3 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 5].Value)) + ",";
                Sql += "G_Syubai4 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 7].Value)) + ",";
                Sql += "G_Syubai5 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 9].Value)) + ",";
                Sql += "G_Syubai6 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 11].Value)) + ",";
                Sql += "G_Syubai7 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 13].Value)) + ",";
                Sql += "G_Syubai8 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 15].Value)) + ",";
                Sql += "G_Syubai9 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 17].Value)) + ",";
                Sql += "G_Syubai10 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 19].Value)) + ",";
                Sql += "G_Syubai11 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 21].Value)) + ",";
                Sql += "G_Syubai12 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 23].Value)) + ",";
                Sql += "G_Syubai13 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 25].Value)) + ",";
                Sql += "G_Syubai14 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 27].Value)) + ",";
                Sql += "G_Syubai15 = " + GetSyubaiStatus(Utility.NulltoStr(dg1[colSyubai, 29].Value)) + ",";

                Sql += "メモ = '" + txtMemo.Text + "',";
                Sql += "確認 = " + Convert.ToInt32(checkBox1.Checked) + ",";
                Sql += "更新年月日 = '" + DateTime.Now.ToString() + "' ";
                Sql += "WHERE ID = '" + sID + "'";

                using (SQLiteCommand com = new SQLiteCommand(Sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                // 発注書テーブル読み込む
                context = new DataContext(cn);
                tblOrder = context.GetTable<Common.ClsOrder>();
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
            if (t != string.Empty)
            {
                return t.PadLeft(len, '0');
            }
            else
            {
                return t;
            }
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
            ////カレントデータの更新
            //CurDataUpDate(cI);

            ////レコードの移動
            //cI = hScrollBar1.Value;
            //showOcrData(cI);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------------------------
        /// <summary>
        ///     １．指定した勤務票ヘッダデータと勤務票明細データを削除する　
        ///     ２．該当する画像データを削除する</summary>
        ///-------------------------------------------------------------------------------
        private void DataDelete(string sID)
        {
            string errMsg = string.Empty;

            // 発注書データ削除
            try
            {
                // 発注書データを削除します
                errMsg = "発注書データ削除";
                                             
                // 発注書データを削除します
                string sql = "Delete from OrderData ";
                sql += "WHERE ID = '" + sID + "'";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                // 画像ファイルを削除します
                errMsg = "発注書画像";

                if (System.IO.File.Exists(_img))
                {
                    System.IO.File.Delete(_img);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(errMsg + "の削除に失敗しました" + Environment.NewLine + ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
            }
        }

        private void btnRtn_Click(object sender, EventArgs e)
        {
        }

        private void frmCorrect_FormClosing(object sender, FormClosingEventArgs e)
        {
            // カレントデータ更新
            CurDataUpDate(dID);

            // 編集ログデータアップロード
            EditDataUpload();

            // データベース接続解除
            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }

            if (cn2.State == ConnectionState.Open)
            {
                cn2.Close();
            }

            // 解放する
            this.Dispose();
        }

        private void btnDataMake_Click(object sender, EventArgs e)
        {
        }

        /// -----------------------------------------------------------------------
        /// <summary>
        ///     発注データ登録 </summary>
        /// -----------------------------------------------------------------------
        private void textDataMake()
        {
            //if (MessageBox.Show("発注データを登録します。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}

            //// OCRDataクラス生成
            //OCRData ocr = new OCRData();

            //// エラーチェックを実行
            //if (getErrData(cI, ocr))
            //{
            //    // 発注データ作成
            //    OrderDataUpload();
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

            ////終了
            //MessageBox.Show("発注書データが登録されました", "発注書登録", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //this.Tag = END_MAKEDATA;
            //this.Close();
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     発注データ作成 </summary>
        ///----------------------------------------------------------------
        private void OrderDataUpload()
        {
            //string errMsg = "";

            ////cn2.Open();

            //try
            //{
            //    Cursor = Cursors.WaitCursor;

            //    //カレントデータの更新
            //    CurDataUpDate(cI);

            //    // STSH_OCR.db3をAttachする
            //    string sql = "ATTACH [";
            //    sql += Properties.Settings.Default.DB_File + "] AS db;";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }
            //    }

            //    sql = "INSERT INTO db.OrderData ";
            //    sql += "SELECT * FROM main.FAX_Order ";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // 発注書画像移動処理
            //    //foreach (var file in System.IO.Directory.GetFiles(Properties.Settings.Default.MyDataPath, "*.tif"))
            //    //{
            //    //    // 画像ファイル名を取得します
            //    //    string sImgNm = System.IO.Path.GetFileName(file);

            //    //    // 移動先に同じ名前のファイルが存在するとき削除する
            //    //    string tifName = Properties.Settings.Default.TifPath + sImgNm;

            //    //    if (System.IO.File.Exists(tifName))
            //    //    {
            //    //        System.IO.File.Delete(tifName);
            //    //    }

            //    //    // 画像ファイルをTIFフォルダに移動する
            //    //    System.IO.File.Move(file, tifName);
            //    //}

            //    MoveFaxImage();


            //    // 発注書データを削除します
            //    errMsg = "FAX発注書データ削除";
            //    sql = "delete from FAX_Order ";

            //    //cn2.Open();
            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // 編集ログデータアップロード
            //    sql = "INSERT INTO db.DataEditLog (";
            //    sql += "年月日時刻, 得意先コード, 得意先名, 年, 月, 発注書ID, 発注書ID連番, 商品コード, 商品名, 店着日付, 行番号, 列番号, 項目名, ";
            //    sql += "変更前値, 変更後値, 画像名, 編集アカウントID, コンピュータ名, 更新年月日, 発注データID) ";
            //    sql += "SELECT 年月日時刻, 得意先コード, 得意先名, 年, 月, 発注書ID, 発注書ID連番, 商品コード, 商品名, 店着日付, 行番号, 列番号, 項目名,";
            //    sql += "変更前値, 変更後値, 画像名, 編集アカウントID, コンピュータ名, 更新年月日, 発注データID FROM main.DataEditLog ";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // ローカルの編集ログデータを削除します
            //    errMsg = "ローカル編集ログデータ削除";
            //    sql = "delete from DataEditLog ";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //finally
            //{
            //    //if (cn2.State == ConnectionState.Open)
            //    //{
            //    //    cn2.Close();
            //    //}
            //}
        }

        ///-----------------------------------------------------------------
        /// <summary>
        ///     FAX発注書画像保存 </summary>
        ///-----------------------------------------------------------------
        //private void MoveFaxImage()
        //{
        //    try
        //    {
        //        Cursor = Cursors.WaitCursor;

        //        Table<Common.ClsSystemConfig> systemConfigs = context.GetTable<Common.ClsSystemConfig>();

        //        var s = systemConfigs.Single(a => a.ID == global.configKEY);

        //        // 画像保存先パス
        //        string _ImgPath = s.ImgPath;

        //        string sql = "select 得意先コード, 画像名 from Fax_Order order by ID";

        //        using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
        //        {
        //            SQLiteDataReader sqlData = com.ExecuteReader();

        //            while(sqlData.Read())
        //            {
        //                // 得意先名取得
        //                string TokuiNM = string.Empty;

        //                // 2020/04/08 コメント化
        //                //for (int i = 0; i < tokuisaki.Length; i++)
        //                //{
        //                //    if (tokuisaki[i].TOKUISAKI_CD == sqlData["得意先コード"].ToString().PadLeft(7, '0'))
        //                //    {
        //                //        TokuiNM = tokuisaki[i].TOKUISAKI_NM;
        //                //        break;
        //                //    }
        //                //}

        //                // 2020/04/08
        //                for (int i = 0; i < global.tokuisakis.Length; i++)
        //                {
        //                    if (global.tokuisakis[i].TOKUISAKI_CD == sqlData["得意先コード"].ToString().PadLeft(7, '0'))
        //                    {
        //                        TokuiNM = global.tokuisakis[i].TOKUISAKI_NM;
        //                        break;
        //                    }
        //                }

        //                // フォルダ名
        //                string DirNM = sqlData["得意先コード"].ToString().PadLeft(7, '0') + "_" + TokuiNM;

        //                if (!System.IO.Directory.Exists(_ImgPath + DirNM))
        //                {
        //                    // 保存先フォルダ未作成の場合は作成する
        //                    System.IO.Directory.CreateDirectory(_ImgPath + DirNM);
        //                }

        //                // 画像名
        //                string imgFile = Properties.Settings.Default.MyDataPath + sqlData["画像名"];

        //                // 保存先画像名パス
        //                string NewFile = _ImgPath + DirNM + @"\" + sqlData["画像名"];

        //                // 移動先に同名ファイルが登録済みのとき削除する
        //                if (System.IO.File.Exists(NewFile))
        //                {
        //                    System.IO.File.Delete(NewFile);
        //                }

        //                // 画像移動
        //                System.IO.File.Move(imgFile, NewFile);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        Cursor = Cursors.Default;
        //    }
        //}

        ///------------------------------------------------------------------------
        /// <summary>
        ///     編集ログデータアップロード </summary>
        ///------------------------------------------------------------------------

        private void EditDataUpload()
        {
            string errMsg = "";

            cn2.Open();

            try
            {
                Cursor = Cursors.WaitCursor;

                // STSH_OCR.db3をAttachする
                string sql = "ATTACH [";
                sql += Properties.Settings.Default.DB_File.Replace(@"\\\", @"\\") + "] AS db;";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
                {
                    com.ExecuteNonQuery();
                }

                sql = "INSERT INTO db.DataEditLog (";
                sql += "年月日時刻, 得意先コード, 得意先名, 年, 月, 発注書ID, 発注書ID連番, 商品コード, 商品名, 店着日付, 行番号, 列番号, 項目名, ";
                sql += "変更前値, 変更後値, 画像名, 編集アカウントID, コンピュータ名, 更新年月日, 発注データID) ";
                sql += "SELECT 年月日時刻, 得意先コード, 得意先名, 年, 月, 発注書ID, 発注書ID連番, 商品コード, 商品名, 店着日付, 行番号, 列番号, 項目名,";
                sql += "変更前値, 変更後値, 画像名, 編集アカウントID, コンピュータ名, 更新年月日, 発注データID FROM main.DataEditLog ";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
                {
                    com.ExecuteNonQuery();
                }

                // ローカルの編集ログデータを削除します
                errMsg = "ローカル編集ログデータ削除";
                sql = "delete from DataEditLog ";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
                {
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, errMsg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (cn2.State == ConnectionState.Open)
                {
                    cn2.Close();
                }
            }
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
        private bool getErrData(string sID, OCRData ocr)
        {
            // カレントレコード更新
            CurDataUpDate(sID);

            // エラー番号初期化
            ocr._errNumber = ocr.eNothing;

            // エラーメッセージクリーン
            ocr._errMsg = string.Empty;

            // エラーチェック実行①:カレントレコードから最終レコードまで
            if (!ocr.errCheckMain(sID, tblOrder, tblPtn, tblOrderHistories))
            {
                return false;
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
            foreach (var t in tblOrder.OrderBy(a => a.ID))
            {
                // 発注書画像ファイルパスを取得する
                fromImg = Properties.Settings.Default.MyDataPath + t.ImageFileName;

                // 移動先フォルダ
                //string sName = Utility.getNouhinName(t.TokuisakiCode.ToString().PadLeft(7, '0'), out sTel, out sJyu);   // 2020/04/09コメント化
                string sName = Utility.GetTokuisakiFromDataTable(t.TokuisakiCode.ToString().PadLeft(7, '0'), global.dtTokuisaki).TOKUISAKI_NM;   // 2020/04/09コメント化

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


        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            // ログ書き込み状態のとき、値を保持する
            if (editLogStatus)
            {
                // 商品コード
                if (e.ColumnIndex == 3)
                {
                    cellName = CELL_SYOHINCD;
                }

                // 納価
                if (e.ColumnIndex == 4)
                {
                    cellName = CELL_NOUKA;
                }

                // 売価
                if (e.ColumnIndex == 5)
                {
                    cellName = CELL_BAIKA;
                }

                // 月曜発注数
                if (e.ColumnIndex == 6)
                {
                    cellName = CELL_MON;
                }

                // 火曜発注数
                if (e.ColumnIndex == 7)
                {
                    cellName = CELL_TUE;
                }

                // 水曜発注数
                if (e.ColumnIndex == 8)
                {
                    cellName = CELL_WED;
                }

                // 木曜発注数
                if (e.ColumnIndex == 9)
                {
                    cellName = CELL_THU;
                }

                // 金曜発注数
                if (e.ColumnIndex == 10)
                {
                    cellName = CELL_FRI;
                }

                // 土曜発注数
                if (e.ColumnIndex == 11)
                {
                    cellName = CELL_SAT;
                }

                // 日曜発注数
                if (e.ColumnIndex == 12)
                {
                    cellName = CELL_SUN;
                }

                // 終売処理
                if (e.ColumnIndex == 13)
                {
                    cellName = CELL_SHUBAI;
                }

                cellBeforeValue = Utility.NulltoStr(dg1[e.ColumnIndex, e.RowIndex].Value);
            }
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
            if (editLogStatus)
            {
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || 
                    e.ColumnIndex == 7 || e.ColumnIndex == 9 || e.ColumnIndex == 10 || e.ColumnIndex == 11 || 
                    e.ColumnIndex == 12 || e.ColumnIndex == 13)
                {
                    dg1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    cellAfterValue = Utility.NulltoStr(dg1[e.ColumnIndex, e.RowIndex].Value);

                    // 変更のとき編集ログデータを書き込み
                    if (cellBeforeValue != cellAfterValue)
                    {
                        // 店着日付
                        string TenDay = string.Empty;

                        if (e.ColumnIndex == 6)
                        {
                            TenDay = txtTenDay1.Text;
                        }

                        if (e.ColumnIndex == 7)
                        {
                            TenDay = txtTenDay2.Text;
                        }

                        if (e.ColumnIndex == 8)
                        {
                            TenDay = txtTenDay3.Text;
                        }

                        if (e.ColumnIndex == 9)
                        {
                            TenDay = txtTenDay4.Text;
                        }

                        if (e.ColumnIndex == 10)
                        {
                            TenDay = txtTenDay5.Text;
                        }

                        if (e.ColumnIndex == 11)
                        {
                            TenDay = txtTenDay6.Text;
                        }

                        if (e.ColumnIndex == 12)
                        {
                            TenDay = txtTenDay7.Text;
                        }

                        LogDataUpdate(e.RowIndex, e.ColumnIndex, global.flgOn, cellName, lblTokuisakiName.Text, 
                                      Utility.NulltoStr(dg1[colHinCode, e.RowIndex].Value).PadLeft(8, '0'), Utility.NulltoStr(dg1[colMaker, e.RowIndex].Value), TenDay);
                    }
                }
            }
        }

        private void txtYear_Enter(object sender, EventArgs e)
        {
            if (editLogStatus)
            {
                // 年
                if (sender == txtYear)
                {
                    cellName = LOG_YEAR;
                }

                // 月
                if (sender == txtMonth)
                {
                    cellName = LOG_MONTH;
                }

                // 発注書ID
                if (sender == txtPID)
                {
                    cellName = LOG_PID;
                }

                // 発注書ID連番
                if (sender == txtSeqNum)
                {
                    cellName = LOG_PSEQNUM;
                }

                // 得意先コード
                if (sender == txtTokuisakiCD)
                {
                    cellName = LOG_TOKUISAKICD;
                }

                // 店着日月曜
                if (sender == txtTenDay1)
                {
                    cellName = LOG_DAY_1;
                }

                // 店着日火曜
                if (sender == txtTenDay2)
                {
                    cellName = LOG_DAY_2;
                }

                // 店着日水曜
                if (sender == txtTenDay3)
                {
                    cellName = LOG_DAY_3;
                }

                // 店着日木曜
                if (sender == txtTenDay4)
                {
                    cellName = LOG_DAY_4;
                }

                // 店着日金曜
                if (sender == txtTenDay5)
                {
                    cellName = LOG_DAY_5;
                }

                // 店着日土曜
                if (sender == txtTenDay6)
                {
                    cellName = LOG_DAY_6;
                }

                // 店着日日曜
                if (sender == txtTenDay7)
                {
                    cellName = LOG_DAY_7;
                }

                TextBox tb = (TextBox)sender;

                // 値を保持
                cellBeforeValue = Utility.NulltoStr(tb.Text);
            }
        }

        private void txtYear_Leave(object sender, EventArgs e)
        {
            if (editLogStatus)
            {
                TextBox tb = (TextBox)sender;
                cellAfterValue = Utility.NulltoStr(tb.Text);

                // 変更のとき編集ログデータを書き込み
                if (cellBeforeValue != cellAfterValue)
                {
                    LogDataUpdate(0, 0, global.flgOff, cellName, lblTokuisakiName.Text, string.Empty, string.Empty, string.Empty);
                }
            }
        }

        /// ----------------------------------------------------------------------
        /// <summary>
        ///     編集ログデータ書き込み </summary>
        /// <param name="rIndex">
        ///     データグリッドビュー行インデックス</param>
        /// <param name="iX">
        ///     列番号</param>
        /// <param name="dType">
        ///     データタイプ　0:ヘッダーデータ, 1:発注明細データ</param>
        /// <param name="colName">
        ///     カラム名</param>
        /// ----------------------------------------------------------------------
        private void LogDataUpdate(int rIndex, int iX, int dType, string colName, string TokuisakiName, string SyohinCD, string SyohinName, string TenDay)
        {
            //cn.Open();

            try
            {
                DateTime NowDate = DateTime.Now;

                // データ追加
                string sql = "insert into DataEditLog ";
                sql += "(年月日時刻, 得意先コード, 得意先名, 年, 月, 発注書ID, 発注書ID連番, 商品コード, 商品名, 店着日付, 行番号, 列番号, " +
                        "項目名, 変更前値, 変更後値, 画像名, 編集アカウントID, コンピュータ名, 更新年月日, 発注データID) ";
                sql += "values ('";
                sql += NowDate.Year + "/" + NowDate.Month.ToString("D2") + "/" + NowDate.Day.ToString("D2") + " " + 
                       NowDate.Hour.ToString("D2") + ":" + NowDate.Minute.ToString("D2") + ":" + NowDate.Second.ToString("D2") + "','";    // 年月日時刻
                sql += Order.TokuisakiCode.ToString("D7") + "','";    // 得意先コード
                sql += TokuisakiName + "','";                   // 得意先名
                sql += Order.Year + "','";                // 年
                sql += Order.Month + "','";               // 月
                sql += Order.patternID + "','";           // 発注書ID
                sql += Order.SeqNumber + "','";           // 発注書ID連番
                sql += SyohinCD + "','";                        // 商品コード
                sql += SyohinName + "','";                      // 商品名
                sql += TenDay + "','";                          // 店着日付
                sql += rIndex + "','";                          // 行番号
                sql += iX + "','";                              // 列番号
                sql += colName + "','";                         // カラム名
                sql += cellBeforeValue + "','";                 // 変更前値
                sql += cellAfterValue + "','";                  // 変更後値
                sql += Order.ImageFileName + "','";       // 画像名
                sql += "','";                                   // 編集アカウントID
                sql += System.Net.Dns.GetHostName() + "','";    // コンピュータ名
                sql += DateTime.Now.ToString() + "','";         // 更新年月日
                sql += Order.ID;                          // 発注データID
                sql += "');";

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
                //cn.Close();
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            // エラーチェック
            ErrCheckClick();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     エラーチェック実行 </summary>
        ///---------------------------------------------------------
        private void ErrCheckClick()
        {
            // 非ログ書き込み状態とする：2015/09/25
            editLogStatus = false;

            // OCRDataクラス生成
            OCRData ocr = new OCRData();

            // エラーチェックを実行
            if (getErrData(dID, ocr))
            {
                MessageBox.Show("エラーはありませんでした", "エラーチェック", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // データ表示
                showOcrData(dID);
            }
            else
            {
                // データ表示
                showOcrData(dID);

                // エラー表示
                ErrShow(ocr);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // 発注書削除
            OrderDelete();
        }

        ///------------------------------------------------------------------
        /// <summary>
        ///     FAX発注書削除  </summary>
        ///------------------------------------------------------------------
        private void OrderDelete()
        {
            if (MessageBox.Show("表示中の発注書データを削除します。よろしいですか", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            //// 非ログ書き込み状態とする
            //editLogStatus = false;

            // レコードと画像ファイルを削除する
            DataDelete(dID);

            // ログ書き込み
            LogDataUpdate(0, 0, global.flgOff, LOG_DELETE, lblTokuisakiName.Text, string.Empty, string.Empty, string.Empty);


            // ゼロならばプログラム終了
            MessageBox.Show("発注書データが削除されました", "発注書削除", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //終了処理
            this.Tag = END_NODATA;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 非ログ書き込み状態とする
            editLogStatus = false;

            // フォームを閉じる
            this.Tag = END_BUTTON;
            this.Close();
        }
        
        private void button3_Click_1(object sender, EventArgs e)
        {
            ////カレントデータの更新
            //CurDataUpDate(cI);

            ////レコードの移動
            //cI = 0;
            //showOcrData(cI);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            ////カレントデータの更新
            //CurDataUpDate(cI);

            ////レコードの移動
            //if (cI > 0)
            //{
            //    cI--;
            //    showOcrData(cI);
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ////カレントデータの更新
            //CurDataUpDate(cI);

            ////レコードの移動
            //if (cI + 1 < cID.Length)
            //{
            //    cI++;
            //    showOcrData(cI);
            //}
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ////カレントデータの更新
            //CurDataUpDate(cI);

            ////レコードの移動
            //cI = cID.Length - 1;
            //showOcrData(cI);
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(_img);

            // 2018/06/21 元画像のピクセル調整を行わないことによる縮小調整
            e.Graphics.DrawImage(img, 0, 0, img.Width * 47 / 100, img.Height * 47 / 100);
            e.HasMorePages = false;

            MessageBox.Show("印刷が終了しました");
            img.Dispose();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

            // 印刷実行
            printDocument1.DefaultPageSettings.Landscape = true;
            printDocument1.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;    // デフォルトプリンタを設定
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
            ////cn2.Open();

            //try
            //{
            //    // STSH_OCR.db3をAttachする
            //    string sql = "ATTACH [";
            //    sql += Properties.Settings.Default.DB_File + "] AS db;";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // 保留テーブルに発注書データを移動する
            //    sql = "INSERT INTO db.Hold_Fax ";
            //    sql += "SELECT * FROM main.FAX_Order ";
            //    sql += "WHERE ID = '" + Order.ID + "'";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // 発注書データを削除します
            //    sql = "Delete from FAX_Order ";
            //    sql += "WHERE ID= '" + Order.ID + "'";

            //    using (SQLiteCommand com = new SQLiteCommand(sql, cn2))
            //    {
            //        com.ExecuteNonQuery();
            //    }

            //    // 画像ファイル名を取得します
            //    string sImgNm = System.IO.Path.GetFileName(_img);

            //    // 移動先に同じ名前のファイルが存在する場合、既にあるファイルを削除する
            //    string tifName = Properties.Settings.Default.HoldTifPath + sImgNm;

            //    if (System.IO.File.Exists(tifName))
            //    {
            //        System.IO.File.Delete(tifName);
            //    }

            //    // 画像ファイルを保留フォルダに移動する
            //    System.IO.File.Move(_img, tifName);

            //    //// 発注書データを削除します
            //    //string errMsg = "FAX発注書データ";
            //    //tblOrder.DeleteOnSubmit(ClsFaxOrder);
            //    //context2.SubmitChanges();

            //    // 終了メッセージ
            //    MessageBox.Show("注文書が保留されました", "ＦＡＸ発注書保留", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    // 件数カウント
            //    if (tblOrder.Count() > 0)
            //    {
            //        // 配列キー再構築
            //        keyArrayCreate();

            //        // カレントレコードインデックスを再設定
            //        if (cID.Length - 1 < cI)
            //        {
            //            cI = cID.Length - 1;
            //        }

            //        // データ画面表示
            //        showOcrData(cI);
            //    }
            //    else
            //    {
            //        // ゼロならばプログラム終了
            //        MessageBox.Show("全ての発注書データが保留されました。処理を終了します。", "発注書保留", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            //        //終了処理
            //        this.Tag = END_NODATA;
            //        this.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    if (cn.State == ConnectionState.Open)
            //    {
            //        cn.Close();
            //    }

            //    if (cn2.State == ConnectionState.Open)
            //    {
            //        // いったん閉じて又開く
            //        cn2.Close();
            //        cn2.Open();
            //    }
            //}
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("表示中のＦＡＸ発注書を保留にします。よろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //{
            //    return;
            //}

            ////カレントデータの更新
            //CurDataUpDate(cI);

            //// 保留処理
            //setHoldData(cID[cI]);
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
            if (colName == colDay1 || colName == colDay2 || colName == colDay3 || colName == colDay4 || colName == colDay5 || colName == colDay6 || colName == colDay7 || colName == colSyubai)
            {
                if (dg1.IsCurrentCellDirty)
                {
                    dg1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void frmCorrect_KeyDown(object sender, KeyEventArgs e)
        {
            // 発注書削除
            if (e.KeyData == Keys.F8 && btnDelete.Enabled)
            {
                OrderDelete();
            }

            // 画像印刷
            if (e.KeyData == Keys.F9 && btnPrint.Enabled)
            {
                if (MessageBox.Show("画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }

                // 印刷実行
                printDocument1.DefaultPageSettings.Landscape = true;
                printDocument1.Print();
            }

            // エラーチェック実行
            if (e.KeyData == Keys.F10 && btnErrCheck.Enabled)
            {
                ErrCheckClick();
            }
        }

        private void txtPID_TextChanged(object sender, EventArgs e)
        {
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }

        private void txtSeqNum_TextChanged(object sender, EventArgs e)
        {
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }

        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     発注パターンを表示する </summary>
        /// <param name="TokuisakiCD">
        ///     得意先コード</param>
        /// <param name="PID">
        ///     発注書ID</param>
        /// <param name="SeqNum">
        ///     発注書ID連番</param>
        ///----------------------------------------------------------------------------------
        private void ShowFaxPattern(TextBox TokuisakiCD, TextBox PID, TextBox SeqNum)
        {
            // 2020/04/12
            if (!PtnShowStatus)
            {
                return;
            }

            string _TokuisakiCD = Utility.NulltoStr(TokuisakiCD.Text);
            string _PID = Utility.NulltoStr(PID.Text);
            string _SeqNum = Utility.NulltoStr(SeqNum.Text);

            if (_TokuisakiCD == string.Empty || _PID == string.Empty || _SeqNum == string.Empty)
            {
                return;
            }

            // 商品欄初期化
            for (int i = 1; i < 30; i += 2)
            {
                dg1[colHinCode, i].Value = string.Empty;
                dg1[colMaker, i].Value = string.Empty;
            }

            // 商品パターン表示
            foreach (var t in tblPtn.Where(a => a.TokuisakiCode ==  Utility.StrtoInt(_TokuisakiCD) && 
                            a.SeqNum == Utility.StrtoInt(_PID) && a.SecondNum == Utility.StrtoInt(_SeqNum)))
            {
                if (t.G_Code1 != string.Empty)
                {
                    dg1[colHinCode, 1].Value = t.G_Code1.PadLeft(8, '0');
                    dg1[colMaker, 1].Value = t.G_Name1;
                }

                if (t.G_Code2 != string.Empty)
                {
                    dg1[colHinCode, 3].Value = t.G_Code2.PadLeft(8, '0');
                    dg1[colMaker, 3].Value = t.G_Name2;
                }

                if (t.G_Code3 != string.Empty)
                {
                    dg1[colHinCode, 5].Value = t.G_Code3.PadLeft(8, '0');
                    dg1[colMaker, 5].Value = t.G_Name3;
                }


                if (t.G_Code4 != string.Empty)
                {
                    dg1[colHinCode, 7].Value = t.G_Code4.PadLeft(8, '0');
                    dg1[colMaker, 7].Value = t.G_Name4;
                }


                if (t.G_Code5 != string.Empty)
                {
                    dg1[colHinCode, 9].Value = t.G_Code5.PadLeft(8, '0');
                    dg1[colMaker, 9].Value = t.G_Name5;
                }


                if (t.G_Code6 != string.Empty)
                {
                    dg1[colHinCode, 11].Value = t.G_Code6.PadLeft(8, '0');
                    dg1[colMaker, 11].Value = t.G_Name6;
                }


                if (t.G_Code7 != string.Empty)
                {
                    dg1[colHinCode, 13].Value = t.G_Code7.PadLeft(8, '0');
                    dg1[colMaker, 13].Value = t.G_Name7;
                }


                if (t.G_Code8 != string.Empty)
                {
                    dg1[colHinCode, 15].Value = t.G_Code8.PadLeft(8, '0');
                    dg1[colMaker, 15].Value = t.G_Name8;
                }


                if (t.G_Code9 != string.Empty)
                {
                    dg1[colHinCode, 17].Value = t.G_Code9.PadLeft(8, '0');
                    dg1[colMaker, 17].Value = t.G_Name9;
                }


                if (t.G_Code10 != string.Empty)
                {
                    dg1[colHinCode, 19].Value = t.G_Code10.PadLeft(8, '0');
                    dg1[colMaker, 19].Value = t.G_Name10;
                }


                if (t.G_Code11 != string.Empty)
                {
                    dg1[colHinCode, 21].Value = t.G_Code11.PadLeft(8, '0');
                    dg1[colMaker, 21].Value = t.G_Name11;
                }


                if (t.G_Code12 != string.Empty)
                {
                    dg1[colHinCode, 23].Value = t.G_Code12.PadLeft(8, '0');
                    dg1[colMaker, 23].Value = t.G_Name12;
                }


                if (t.G_Code13 != string.Empty)
                {
                    dg1[colHinCode, 25].Value = t.G_Code13.PadLeft(8, '0');
                    dg1[colMaker, 25].Value = t.G_Name13;
                }


                if (t.G_Code14 != string.Empty)
                {
                    dg1[colHinCode, 27].Value = t.G_Code14.PadLeft(8, '0');
                    dg1[colMaker, 27].Value = t.G_Name14;
                }

                if (t.G_Code15 != string.Empty)
                {
                    dg1[colHinCode, 29].Value = t.G_Code15.PadLeft(8, '0');
                    dg1[colMaker, 29].Value = t.G_Name15;
                }
            }

            System.Diagnostics.Debug.WriteLine("商品パターン表示");
        }

        private void txtTokuisakiCD_TextChanged(object sender, EventArgs e)
        {
            // 得意先名表示

            // 2020/04/08 コメント化
            //for (int i = 0; i < tokuisaki.Length; i++)
            //{
            //    if (tokuisaki[i].TOKUISAKI_CD == txtTokuisakiCD.Text.PadLeft(7, '0'))
            //    {
            //        TokuiNM = tokuisaki[i].TOKUISAKI_NM;
            //        break;
            //    }
            //}

            // 2020/04/09
            lblTokuisakiName.Text = Utility.GetTokuisakiFromDataTable(txtTokuisakiCD.Text.PadLeft(7, '0'), global.dtTokuisaki).TOKUISAKI_NM;

            // 発注書パターン表示
            ShowFaxPattern(txtTokuisakiCD, txtPID, txtSeqNum);
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン : 2020/04/14 </summary>
        /// <param name="filePath">
        ///     画像ファイルパス</param>
        ///---------------------------------------------------------
        private void imgShow(string filePath)
        {
            try
            {
                // System.Drawing.Imageを作成する
                FaxImg = Utility.CreateImage(filePath);

                // PictureBoxの大きさにあわせて画像を拡大または縮小して表示する
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                // 画像を表示する
                pictureBox1.Image = FaxImg;
            }
            catch (Exception ex)
            {
                pictureBox1.Image = null;
                MessageBox.Show(ex.Message);
            }
        }

        /////-----------------------------------------------------------
        ///// <summary>
        /////     画像表示 openCV：2018/10/24 </summary>
        ///// <param name="img">
        /////     表示画像ファイル名</param>
        /////-----------------------------------------------------------
        //private void showImage_openCv(string img)
        //{
        //    n_width = B_WIDTH;
        //    n_height = B_HEIGHT;

        //    imgShow(img, n_width, n_height);

        //    trackBar1.Value = 0;
        //}

        //// GUI上に画像を表示するには、OpenCV上で扱うMat形式をBitmap形式に変換する必要がある
        //public static Bitmap MatToBitmap(Mat image)
        //{
        //    return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        //}

        /////---------------------------------------------------------
        ///// <summary>
        /////     画像表示メイン openCV : 2018/10/24 </summary>
        ///// <param name="mImg">
        /////     Mat形式イメージ</param>
        ///// <param name="w">
        /////     width</param>
        ///// <param name="h">
        /////     height</param>
        /////---------------------------------------------------------
        //private void imgShow(Mat mImg, float w, float h)
        //{
        //    int cWidth = 0;
        //    int cHeight = 0;

        //    Bitmap bt = MatToBitmap(mImg);

        //    // Bitmapサイズ
        //    if (panel1.Width < (bt.Width * w) || panel1.Height < (bt.Height * h))
        //    {
        //        cWidth = (int)(bt.Width * w);
        //        cHeight = (int)(bt.Height * h);
        //    }
        //    else
        //    {
        //        cWidth = panel1.Width;
        //        cHeight = panel1.Height;
        //    }

        //    // Bitmap を生成
        //    Bitmap canvas = new Bitmap(cWidth, cHeight);

        //    // ImageオブジェクトのGraphicsオブジェクトを作成する
        //    Graphics g = Graphics.FromImage(canvas);

        //    // 画像をcanvasの座標(0, 0)の位置に指定のサイズで描画する
        //    g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

        //    //メモリクリア
        //    bt.Dispose();
        //    g.Dispose();

        //    // PictureBox1に表示する
        //    pictureBox1.Image = canvas;
        //}

        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン openCV : 2018/10/24 </summary>
        /// <param name="mImg">
        ///     Image形式イメージ</param>
        /// <param name="w">
        ///     width</param>
        /// <param name="h">
        ///     height</param>
        ///---------------------------------------------------------
        private void imgShow(Image mImg, float w, float h)
        {
            try
            {
                Bitmap bt = new Bitmap(mImg);

                // Bitmap を生成
                Bitmap canvas = new Bitmap((int)(bt.Width * w), (int)(bt.Height * h));

                Graphics g = Graphics.FromImage(canvas);

                // 画像をcanvasの座標(0, 0)の位置に指定のサイズで描画する
                g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

                //メモリクリア
                bt.Dispose();
                g.Dispose();

                // PictureBox1に表示する
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = canvas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dg1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!global.ChangeValueStatus)
            {
                return;
            }

            // 終売取消
            if (e.ColumnIndex == 13)
            {
                if ((e.RowIndex % 2) != 0)
                {
                    if (e.RowIndex % 4 == 1)
                    {
                        //dg1.Rows[e.RowIndex - 1].DefaultCellStyle.BackColor = Color.White;
                        //dg1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                        for (int i = 0; i < dg1.ColumnCount; i++)
                        {
                            if (dg1.Rows[e.RowIndex].Cells[i].Style.BackColor != Color.LightGray)
                            {
                                dg1.Rows[e.RowIndex - 1].Cells[i].Style.BackColor = Color.White;
                                dg1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.White;
                            }
                        }
                    }
                    else if (e.RowIndex % 4 == 3)
                    {
                        //dg1.Rows[e.RowIndex - 1].DefaultCellStyle.BackColor = Color.Lavender;
                        //dg1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Lavender;

                        for (int i = 0; i < dg1.ColumnCount; i++)
                        {
                            if (dg1.Rows[e.RowIndex].Cells[i].Style.BackColor != Color.LightGray)
                            {
                                dg1.Rows[e.RowIndex - 1].Cells[i].Style.BackColor = Color.Lavender;
                                dg1.Rows[e.RowIndex].Cells[i].Style.BackColor = Color.Lavender;
                            }
                        }
                    }

                    // 終売取消
                    if (Utility.NulltoStr(dg1[e.ColumnIndex, e.RowIndex].Value) == global.SyubaiArray[1])
                    {
                        //dg1.Rows[e.RowIndex - 1].DefaultCellStyle.BackColor = Color.LightGray;
                        //dg1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;

                        for (int i = 4; i < dg1.ColumnCount; i++)
                        {
                            dg1.Rows[e.RowIndex - 1].Cells[i].Style.ForeColor = Color.LightGray;
                            dg1.Rows[e.RowIndex].Cells[i].Style.ForeColor = Color.LightGray;
                        }
                    }
                    else
                    {
                        //dg1.Rows[e.RowIndex - 1].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                        //dg1.Rows[e.RowIndex].DefaultCellStyle.ForeColor = SystemColors.ControlText;

                        for (int i = 4; i < dg1.ColumnCount; i++)
                        {
                            dg1.Rows[e.RowIndex - 1].Cells[i].Style.ForeColor = SystemColors.ControlText;
                            dg1.Rows[e.RowIndex].Cells[i].Style.ForeColor = SystemColors.ControlText;
                        }

                        // 注文済み商品発注数表示
                        if (showStatus)
                        {
                            for (int i = 6; i <= 12; i++)
                            {
                                // 2020/04/13
                                Utility.ShowPastOrder(i - 6, i, e.RowIndex, tenDates, dg1, colHinCode, colSyubai,  txtTokuisakiCD.Text, tblOrderHistories);
                            }
                        }
                    }

                    // 注文済み商品メッセージコントロール : 2020/04/13
                    label6.Text = Utility.ShowPastOrderMessage(dg1);
                    if (label6.Text != string.Empty)
                    {
                        label1.Text = "注文済み商品があります";
                    }
                    else
                    {
                        label1.Text = "";
                    }
                }
            }

            // 商品コード
            if (e.ColumnIndex == 3)
            {
                if ((e.RowIndex % 2) != 0)
                {
                    string syCd = Utility.NulltoStr(dg1[e.ColumnIndex, e.RowIndex].Value).PadLeft(8, '0');
                    ClsCsvData.ClsCsvSyohin_New syohin = Utility.GetSyohinsFromDataTable(global.dtSyohin, syCd);    // 2020/04/09

                    dg1[colMaker, e.RowIndex - 1].Value = syohin.SIRESAKI_NM;       // 仕入先名
                    dg1[colMaker, e.RowIndex].Value = syohin.SYOHIN_NM;             // 商品名
                    dg1[colKikaku, e.RowIndex - 1].Value = syohin.SYOHIN_KIKAKU;    // 規格
                    dg1[colIrisu, e.RowIndex].Value = syohin.CASE_IRISU;            // 入数

                    // 納価売価取得：2020/04/10
                    ClsCsvData.ClsCsvNoukaBaika noukaBaika = Utility.GetNoukaBaikaFromDataTable(txtTokuisakiCD.Text.PadLeft(7, '0'), syCd, global.dtNoukaBaika);
                    dg1[colNouka, e.RowIndex].Value = noukaBaika.NOUKA;     // 納価
                    dg1[colBaika, e.RowIndex].Value = noukaBaika.BAIKA;     // 売価

                    // 終売のとき : 2020/04/15
                    //if (syohin.SHUBAI)
                    if (Utility.IsShubai(syohin.LAST_SALE_YMD))
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
                        dg1[colSyubai, e.RowIndex].Value = "";
                        dg1[colSyubai, e.RowIndex].ReadOnly = true;
                    }

                    // 注文済み商品発注数表示
                    if (showStatus)
                    {
                        for (int i = 6; i <= 12; i++)
                        {
                            //ShowPastOrder(i - 6, i, e.RowIndex); // 2020/04/13 コメント化

                            // 2020/04/13
                            Utility.ShowPastOrder(i - 6, i, e.RowIndex, tenDates, dg1, colHinCode, colSyubai, txtTokuisakiCD.Text, tblOrderHistories);
                        }

                        // 2020/04/14
                        label6.Text = Utility.ShowPastOrderMessage(dg1);
                        if (label6.Text != string.Empty)
                        {
                            label1.Text = "注文済み商品があります";
                        }
                        else
                        {
                            label1.Text = "";
                        }
                    }
                }

                return;
            }

            // 発注数
            if (e.ColumnIndex >= 6 && e.ColumnIndex <= 12)
            {
                if ((e.RowIndex % 2) != 0)
                {
                    if (showStatus)
                    {
                        int iX = e.ColumnIndex - 6;
                        //ShowPastOrder(iX, e.ColumnIndex, e.RowIndex);  2020/04/13 コメント化

                        // 2020/04/13
                        Utility.ShowPastOrder(iX, e.ColumnIndex, e.RowIndex, tenDates, dg1, colHinCode, colSyubai, txtTokuisakiCD.Text, tblOrderHistories);
                    }
                }

                // 注文済み商品メッセージコントロール : 2020/04/13
                label6.Text = Utility.ShowPastOrderMessage(dg1);
                if (label6.Text != string.Empty)
                {
                    label1.Text = "注文済み商品があります";
                }
                else
                {
                    label1.Text = "";
                }
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            n_width = B_WIDTH + (float)trackBar1.Value * 0.05f;
            n_height = B_HEIGHT + (float)trackBar1.Value * 0.05f;

            //imgShow(mMat, n_width, n_height);
            imgShow(FaxImg, n_width, n_height);
        }
        private DataGridViewComboBoxEditingControl dataGridViewComboBox = null;

        private void dg1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            //表示されているコントロールがDataGridViewComboBoxEditingControlか調べる
            if (e.Control is DataGridViewComboBoxEditingControl)
            {
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
            }

            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                // 数字のみ入力可能とする
                if (dgv.CurrentCell.ColumnIndex == 3 || dgv.CurrentCell.ColumnIndex == 4 || dgv.CurrentCell.ColumnIndex == 5 || dgv.CurrentCell.ColumnIndex == 6 ||
                    dgv.CurrentCell.ColumnIndex == 7 || dgv.CurrentCell.ColumnIndex == 8 || dgv.CurrentCell.ColumnIndex == 9 || dgv.CurrentCell.ColumnIndex == 10 ||
                    dgv.CurrentCell.ColumnIndex == 11 || dgv.CurrentCell.ColumnIndex == 12)
                {
                    if (dgv.CurrentCell.ColumnIndex != 2)
                    {
                        //イベントハンドラが複数回追加されてしまうので最初に削除する
                        e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                        e.Control.KeyPress -= new KeyPressEventHandler(dataGridViewComboBox_SelectedIndexChanged);

                        //イベントハンドラを追加する
                        e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                    }
                }
            }

            // 終売処理の列か調べる
            if (dgv.CurrentCell.OwningColumn.Name == colSyubai && dgv.CurrentCell.OwningRow.Index % 2 != 0)
            {
                //編集のために表示されているコントロールを取得
                this.dataGridViewComboBox = (DataGridViewComboBoxEditingControl)e.Control;

                //イベントハンドラが複数回追加されてしまうので最初に削除する
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
                e.Control.KeyPress -= new KeyPressEventHandler(dataGridViewComboBox_SelectedIndexChanged);

                //SelectedIndexChangedイベントハンドラを追加
                this.dataGridViewComboBox.SelectedIndexChanged += new EventHandler(dataGridViewComboBox_SelectedIndexChanged);
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 得意先名・FAX番号取得
            string TokuiNM = string.Empty;
            string TokuiFax = string.Empty;

            // 2020/04/08 コメント化
            //for (int i = 0; i < tokuisaki.Length; i++)
            //{
            //    if (tokuisaki[i].TOKUISAKI_CD == txtTokuisakiCD.Text.PadLeft(7, '0'))
            //    {
            //        TokuiNM = tokuisaki[i].TOKUISAKI_NM;
            //        TokuiFax = tokuisaki[i].TOKUISAKI_FAX;
            //        break;
            //    }
            //}

            // 2020/04/09
            ClsCsvData.ClsCsvTokuisaki tokuisaki = Utility.GetTokuisakiFromDataTable(txtTokuisakiCD.Text.PadLeft(7, '0'), global.dtTokuisaki);

            TokuiNM = tokuisaki.TOKUISAKI_NM;
            TokuiFax = tokuisaki.TOKUISAKI_FAX;

            Hide();
            frmReFax reFax = new frmReFax(_img, TokuiNM, TokuiFax);
            reFax.ShowDialog();
            Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click_1(object sender, EventArgs e)
        {
            ImageRotate(pictureBox1.Image);
        }

        ///-------------------------------------------------------
        /// <summary>
        ///     画像回転 </summary>
        /// <param name="img">
        ///     Image</param>
        ///-------------------------------------------------------
        private void ImageRotate(Image img)
        {
            Bitmap bmp = (Bitmap)img;

            // 反転せず時計回りに90度回転する
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

            //表示
            pictureBox1.Image = img;
        }

        private void txtTenDay1_TextChanged(object sender, EventArgs e)
        {
            if (!showStatus)
            {
                return;
            }

            // 店着日配列を更新
            SetShowTenDate(tenDates);

            // 店着日ロック
            DayLock(tenDates);

            // 発注済み商品数表示コントロール
            for (int i = 0; i < tenDates.Length; i++)
            {
                int col = i + 6;

                for (int r = 1; r < dg1.RowCount; r += 2)
                {
                    //ShowPastOrder(i, col, r);

                    // 2020/04/13
                    Utility.ShowPastOrder(i, col, r, tenDates, dg1, colHinCode, colSyubai, txtTokuisakiCD.Text, tblOrderHistories);
                }
            }

            // 注文済み商品メッセージコントロール : 2020/04/13
            // 2020/04/13
            label6.Text = Utility.ShowPastOrderMessage(dg1);
            if (label6.Text != string.Empty)
            {
                label1.Text = "注文済み商品があります";
            }
            else
            {
                label1.Text = "";
            }
        }
    }
}
