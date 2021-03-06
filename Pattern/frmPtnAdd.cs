﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using STSH_OCR.Common;
using System.Data.SQLite;
using System.Data.Linq;
using ClosedXML.Excel;
using ClosedXML;
using Microsoft.Office.Interop.Excel;

namespace STSH_OCR.Pattern
{
    public partial class frmPtnAdd : Form
    {
        public frmPtnAdd()
        {
            InitializeComponent();
        }

        //string[] TkArray = null;        // 得意先マスター配列
        //string[] SyArray = null;        // 商品マスター配列
        //string[] SyZkArray = null;      // 商品在庫マスター配列
        //string[] ShiireArray = null;    // 仕入先マスター配列

        const int ADDMODE = 0;
        const int EDITMODE = 1;
        
        const int GET_NOUHIN_NAME = 0;
        const int GET_NOUHIN_TEL = 1;
        const int GET_NOUHIN_ADD = 2;

        const int CALL_GC = 0;
        const int CALL_DG = 1;

        int fMode;
        int fID;

        const int ROW_PLUS = 2;         // DataGridView後に行移動
        const int ROW_MINUS = -1;       // DataGridView前に行移動

        DateTime frDt;    // 商品履歴の表示開始日

        // valueChangeステータス
        bool valueChangeStatus = false;
        bool DataGridMouse = false;

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // 発注書パターンマスター
        Table<Common.ClsOrderPattern> dbPtn = null;
        ClsOrderPattern ClsOrderPattern = null;

        // 商品分類コンボボックス
        ClsMyComboBox[] ClsCombos_L = null;
        ClsMyComboBox[] ClsCombos_M = null;
        ClsMyComboBox[] ClsCombos_S = null;

        // 商品マスタークラス配列
        //ClsCsvData.ClsCsvSyohin[] global.syohin_News = null;
        //ClsCsvData.ClsCsvSyohin_New[] syohin_News = null;

        //string[] csvArray = null;

        string comm1 = "※機械で読み込みます" + Environment.NewLine + "※２重線での訂正はしないで下さい" + Environment.NewLine + "※FAXは曲がらないようご注意下さい";
        string comm2 = "佐藤食品株式会社";
        string comm3 = "担当事務　古城";
        string comm4 = "FAX:0930-23-3278";

        private void frmPtnAdd_Load(object sender, EventArgs e)
        {
            // 商品分類リスト読み込み
            valueChangeStatus = false;
            SetSyohinBunrui_L();
            valueChangeStatus = true;

            // フォーム最大値
            //Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            // グリッドビュー初期化
            tdkGridviewSet(dataGridView2);
            GridviewSet(dataGridView1);
            Gridview3Set(dataGridView3);

            // フォーム初期化
            dispInitial();

            // ローカルマスター接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            // 発注書パターンマスターテーブル読み込み
            dbPtn = context.GetTable<Common.ClsOrderPattern>();
        }


        ///-------------------------------------------------------
        /// <summary>
        ///     商品分類リスト読み込み </summary>
        ///-------------------------------------------------------
        private void SetSyohinBunrui_L()
        {
            // 商品分類リスト読み込み
            if (System.IO.File.Exists(Properties.Settings.Default.商品分類リスト))
            {
                using (IXLWorkbook bk = new XLWorkbook(Properties.Settings.Default.商品分類リスト, XLEventTracking.Disabled))
                {
                    var sheet1 = bk.Worksheet(1);
                    var tbl = sheet1.RangeUsed().AsTable();
                    int Cnt = 0;

                    string wCD = "";

                    foreach (var t in tbl.DataRange.Rows())
                    {
                        if (t.RowNumber() < 3)
                        {
                            continue;
                        }

                        string BunruiCD = Utility.NulltoStr(t.Cell(1).Value).PadLeft(2, '0');

                        if (wCD != BunruiCD)
                        {
                            Array.Resize(ref ClsCombos_L, Cnt + 1);
                            ClsCombos_L[Cnt] = new ClsMyComboBox();
                            ClsCombos_L[Cnt].MyID = BunruiCD;
                            ClsCombos_L[Cnt].MyName = Utility.NulltoStr(t.Cell(2).Value);
                            //cmbSyohin_L.Items.Add(BunruiCD + " " + Utility.NulltoStr(t.Cell(2).Value));
                            Cnt++;
                        }

                        wCD = BunruiCD;
                    }
                }

                //表示される値はNAMEプロパティ
                cmbSyohin_L.DisplayMember = "MyName";

                //対応する値はIDプロパティ
                cmbSyohin_L.ValueMember = "MyID";

                //DataSourceに上で作成した配列をセット
                cmbSyohin_L.DataSource = ClsCombos_L;

                // 非選択状態とする
                cmbSyohin_L.SelectedIndex = -1;
            }
        }


        private void dispInitial()
        {
            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;
            
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add(50);
            dataGridView2.CurrentCell = null;
            dataGridView2.ReadOnly = false;

            dataGridView1.Rows.Clear();
            dataGridView1.CurrentCell = null;

            button1.Enabled = true;
            button2.Enabled = false;
            comboBox1.SelectedIndex = 0;
            comboBox1.Enabled = false;
            //lblFrDate.Visible = false;
            dateTimePicker1.Enabled = false;

            txtMemo.Text = string.Empty;
            txtComment1.Text = comm1; 
            txtComment2.Text = comm2;
            txtComment3.Text = comm3;
            txtComment4.Text = comm4;

            fMode = ADDMODE;
            btnDel.Visible = false;
            button7.Enabled = true;
        }


        private void continueInitial()
        {
            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;

            // パターン番号インクリメント
            for (int i = 0; i < 50; i++)
            {
                int nn = Utility.StrtoInt(Utility.NulltoStr(dataGridView2[colPtnNum, i].Value));
                if (nn != 0)
                {
                    dataGridView2[colPtnNum, i].Value = nn + 1;
                }
            }

            valueChangeStatus = true;

            //dataGridView2.Rows.Clear();
            //dataGridView2.Rows.Add(50);
            //dataGridView2.CurrentCell = null;
            //dataGridView2.ReadOnly = false;

            //dataGridView1.Rows.Clear();
            //dataGridView1.CurrentCell = null;

            //button1.Enabled = true;
            //button2.Enabled = false;
            //comboBox1.SelectedIndex = 0;
            //comboBox1.Enabled = false;
            //dateTimePicker1.Enabled = false;

            //txtMemo.Text = string.Empty;

            fMode = ADDMODE;
            btnDel.Visible = false;
            button7.Enabled = true;
        }

        // カラム定義
        private string colTdkCode = "c0";
        private string colTdkName = "c1";
        private string colPtnNum = "c2";
        private string colTel = "c3";
        private string colAddress = "c4";
        private string colSecondNum = "c5";

        ///----------------------------------------------------------------------
        /// <summary>
        /// 得意先データグリッドビューの定義を行います </summary>
        ///----------------------------------------------------------------------
        private void tdkGridviewSet(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 22;

                // 全体の高さ
                tempDGV.Height = 196;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colTdkCode, "コード");
                tempDGV.Columns.Add(colTdkName, "得意先名");
                tempDGV.Columns.Add(colPtnNum, "PID");
                tempDGV.Columns.Add(colSecondNum, "Page");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");

                tempDGV.Columns[colTdkCode].Width = 70;
                tempDGV.Columns[colTdkName].Width = 220;
                tempDGV.Columns[colPtnNum].Width = 50;
                tempDGV.Columns[colSecondNum].Width = 50;
                tempDGV.Columns[colTel].Width = 100;
                tempDGV.Columns[colAddress].Width = 320;

                tempDGV.Columns[colTdkCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colPtnNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colSecondNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = false;

                foreach (DataGridViewColumn item in dataGridView2.Columns)
                {
                    if (item.Name == colTdkCode)
                    {
                        item.ReadOnly = false;
                    }
                    else
                    {
                        item.ReadOnly = true;
                    }
                }

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.CellSelect;
                tempDGV.MultiSelect = false;

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

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;

                dataGridView2.Rows.Add(50);
                dataGridView2.CurrentCell = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // カラム定義
        private string colHinCode = "c0";
        private string colHinName = "c1";
        private string colRyou = "c2";
        private string colIrisu = "c3";
        private string colShubetsu = "c4";
        private string colTani = "c5";
        private string colUriDate = "c6";
        private string colSuu = "c7";
        private string colKikaku = "c8";
        private string colNouka = "c9";
        private string colBaika = "c10";
        private string colJanCD = "c11";
        private string colReadDays = "c12";
        private string colMaker = "c13";
        private string colSeqNum = "c14";
        private string colKikanUri = "c15";
        private string colBunrui_L = "c16";
        private string colBunrui_M = "c17";

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品一覧データグリッドビューの定義を行います </summary>
        ///------------------------------------------------------------------------
        private void GridviewSet(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 22;

                // 全体の高さ
                tempDGV.Height = 638;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colMaker, "メーカー");
                tempDGV.Columns.Add(colHinCode, "商品コード");
                tempDGV.Columns.Add(colHinName, "商品名");
                tempDGV.Columns.Add(colKikaku, "規格");
                tempDGV.Columns.Add(colIrisu, "入数");

                tempDGV.Columns[colMaker].Width = 200;
                tempDGV.Columns[colHinCode].Width = 80;
                tempDGV.Columns[colIrisu].Width = 50;
                tempDGV.Columns[colKikaku].Width = 80;

                tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colIrisu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKikaku].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = true;

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

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        ///------------------------------------------------------------------------
        /// <summary>
        ///     発注書登録商品データグリッドビュー定義 </summary>
        ///------------------------------------------------------------------------
        private void Gridview3Set(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する
                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 22;

                // 全体の高さ
                tempDGV.Height = 892;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colSeqNum, "No.");
                tempDGV.Columns.Add(colMaker, "メーカー");
                tempDGV.Columns.Add(colHinCode, "商品コード");
                tempDGV.Columns.Add(colHinName, "商品名");
                tempDGV.Columns.Add(colKikaku, "規格");
                tempDGV.Columns.Add(colIrisu, "入数");

                //tempDGV.Columns.Add(colNouka, "納価");  // 2020/04/10
                //tempDGV.Columns.Add(colBaika, "売価");  // 2020/04/10

                tempDGV.Columns.Add(colJanCD, "JAN");
                tempDGV.Columns.Add(colReadDays, "リード");
                tempDGV.Columns.Add(colBunrui_L, "商品分類");
                //tempDGV.Columns.Add(colBunrui_M, "中分類");

                tempDGV.Columns[colSeqNum].Width = 40;
                tempDGV.Columns[colMaker].Width = 200;
                tempDGV.Columns[colHinCode].Width = 80;
                tempDGV.Columns[colKikaku].Width = 80;
                tempDGV.Columns[colIrisu].Width = 50;

                tempDGV.Columns[colBunrui_L].Width = 180;   // 2020/06/23
                //tempDGV.Columns[colBunrui_M].Width = 100;   // 2020/06/23

                //tempDGV.Columns[colNouka].Width = 60;  // 2020/04/10
                //tempDGV.Columns[colBaika].Width = 60;  // 2020/04/10

                tempDGV.Columns[colJanCD].Width = 110;
                tempDGV.Columns[colReadDays].Width = 70;

                tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colSeqNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colIrisu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKikaku].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //tempDGV.Columns[colNouka].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  // 2020/04/10
                //tempDGV.Columns[colBaika].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;  // 2020/04/10

                tempDGV.Columns[colJanCD].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colReadDays].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = false;

                foreach (DataGridViewColumn c in tempDGV.Columns)
                {
                    // 編集可否 : 大分類、中分類を追加 2020/06/23
                    if (c.Name == colSeqNum || c.Name == colMaker || c.Name == colKikaku || c.Name == colIrisu || c.Name == colJanCD || 
                        c.Name == colBunrui_L || c.Name == colBunrui_M)
                    {
                        c.ReadOnly = true;
                    }
                    else
                    {
                        c.ReadOnly = false;
                    }
                }

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = true;

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

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;

                // コンテキストメニュー
                tempDGV.ContextMenuStrip = this.contextMenuStrip1;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void gcMrSetting()
        {
            dataGridView3.Rows.Clear();

            //multirow編集モード
            //gcMultiRow1.EditMode = EditMode.EditProgrammatically;

            //this.gcMultiRow1.AllowUserToAddRows = false;                    // 手動による行追加を禁止する
            //this.gcMultiRow1.AllowUserToDeleteRows = false;                 // 手動による行削除を禁止する
            //this.gcMultiRow1.Rows.Clear();                                  // 行数をクリア
            //this.gcMultiRow1.RowCount = 15;                     // 行数を設定
            //this.gcMultiRow1.HideSelection = true;                          // GcMultiRow コントロールがフォーカスを失ったとき、セルの選択状態を非表示にする

            //gcMultiRow1[0, "lblNum"].Value = "1";
            //gcMultiRow1[0, "lblNum2"].Value = "16";
            //gcMultiRow1[1, "lblNum"].Value = "2";
            //gcMultiRow1[1, "lblNum2"].Value = "17";
            //gcMultiRow1[2, "lblNum"].Value = "3";
            //gcMultiRow1[2, "lblNum2"].Value = "18";
            //gcMultiRow1[3, "lblNum"].Value = "4";
            //gcMultiRow1[3, "lblNum2"].Value = "19";
            //gcMultiRow1[4, "lblNum"].Value = "5";
            //gcMultiRow1[4, "lblNum2"].Value = "20";
            //gcMultiRow1[5, "lblNum"].Value = "6";
            //gcMultiRow1[5, "lblNum2"].Value = "21";
            //gcMultiRow1[6, "lblNum"].Value = "7";
            //gcMultiRow1[6, "lblNum2"].Value = "22";
            //gcMultiRow1[7, "lblNum"].Value = "8";
            //gcMultiRow1[7, "lblNum2"].Value = "23";
            //gcMultiRow1[8, "lblNum"].Value = "9";
            //gcMultiRow1[8, "lblNum2"].Value = "24";
            //gcMultiRow1[9, "lblNum"].Value = "10";
            //gcMultiRow1[9, "lblNum2"].Value = "25";
            //gcMultiRow1[10, "lblNum"].Value = "11";
            //gcMultiRow1[10, "lblNum2"].Value = "26";
            //gcMultiRow1[11, "lblNum"].Value = "12";
            //gcMultiRow1[11, "lblNum2"].Value = "27";
            //gcMultiRow1[12, "lblNum"].Value = "13";
            //gcMultiRow1[12, "lblNum2"].Value = "28";
            //gcMultiRow1[13, "lblNum"].Value = "14";
            //gcMultiRow1[13, "lblNum2"].Value = "29";
            //gcMultiRow1[14, "lblNum"].Value = "15";
            //gcMultiRow1[14, "lblNum2"].Value = "30";
        }



        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品履歴一覧データグリッドビューの定義を行います </summary>
        ///------------------------------------------------------------------------
        private void rirekiGridviewSet(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する

                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                tempDGV.EnableHeadersVisualStyles = false;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new System.Drawing.Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 22;

                // 全体の高さ
                tempDGV.Height = 638;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;


                // 各列幅指定
                tempDGV.Columns.Add(colMaker, "メーカー");
                tempDGV.Columns.Add(colHinCode, "商品コード");
                tempDGV.Columns.Add(colHinName, "商品名");
                tempDGV.Columns.Add(colKikaku, "規格");
                tempDGV.Columns.Add(colKikanUri, "期間発注");

                tempDGV.Columns[colMaker].Width = 140;
                tempDGV.Columns[colHinCode].Width = 80;
                tempDGV.Columns[colKikaku].Width = 80;
                tempDGV.Columns[colKikanUri].Width = 80;
                tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKikaku].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKikanUri].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                               
                //// 各列幅指定
                //tempDGV.Columns.Add(colHinCode, "商品コード");
                //tempDGV.Columns.Add(colHinName, "商品名");
                //tempDGV.Columns.Add(colUriDate, "最終売上日");
                //tempDGV.Columns.Add(colSuu, "販売数");

                //tempDGV.Columns[colHinCode].Width = 80;
                //tempDGV.Columns[colUriDate].Width = 90;
                //tempDGV.Columns[colSuu].Width = 70;

                //tempDGV.Columns[colHinName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                //tempDGV.Columns[colHinCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colUriDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colSuu].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                tempDGV.MultiSelect = true;

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

                // 行ヘッダーの自動調節
                //tempDGV.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                // 罫線
                tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
                tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            GridviewSet(dataGridView1);

            ShowShohin(dataGridView1, global.dtSyohin); // 2020/04/09
        }

        ///--------------------------------------------------------
        /// <summary>
        ///     データグリッドに商品一覧を表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------
        private void ShowShohin(DataGridView g, System.Data.DataTable dtSyohin)
        {
            this.Cursor = Cursors.WaitCursor;

            g.Rows.Clear();

            int cnt = 0;
            this.txtMemo.Text = "";

            DataRow[] row = dtSyohin.AsEnumerable().OrderBy(a => a["SYOHIN_CD"].ToString().PadLeft(8, '0')).ToArray();

            foreach (var t in row)
            {
                // 仕入先名検索のとき
                if (txtMaker.Text.Trim() != string.Empty)
                {
                    if (!t["SIRESAKI_NM"].ToString().Contains(txtMaker.Text))
                    {
                        continue;
                    }
                }

                // 商品名検索のとき
                if (txtSyohinName.Text.Trim() != string.Empty)
                {
                    if (!t["SYOHIN_NM"].ToString().Contains(txtSyohinName.Text))
                    {
                        continue;
                    }
                }

                // 商品分類検索
                string Bunrui_L = Utility.NulltoStr(cmbSyohin_L.SelectedValue);
                string Bunrui_M = Utility.NulltoStr(cmbSyohin_M.SelectedValue);
                string Bunrui_S = Utility.NulltoStr(cmbSyohin_S.SelectedValue);

                // 大分類
                if (Bunrui_L != string.Empty)
                {                    
                    if (t["SYOHIN_KIND_L_CD"].ToString().PadLeft(2, '0') != Bunrui_L)
                    {
                        continue;
                    }

                    // 中分類
                    if (Bunrui_M != string.Empty)
                    {
                        if (t["SYOHIN_KIND_M_CD"].ToString().PadLeft(2, '0') != Bunrui_M)
                        {
                            continue;
                        }

                        // 小分類
                        if (Bunrui_S != string.Empty)
                        {
                            if (t["SYOHIN_KIND_S_CD"].ToString().PadLeft(2, '0') != Bunrui_S)
                            {
                                continue;
                            }
                        }
                    }
                }
                
                // 商品表示
                g.Rows.Add();
                g[colMaker, cnt].Value = t["SIRESAKI_NM"].ToString();
                g[colHinCode, cnt].Value = t["SYOHIN_CD"].ToString().PadLeft(8, '0');
                g[colHinName, cnt].Value = t["SYOHIN_NM"].ToString();
                g[colIrisu, cnt].Value = t["CASE_IRISU"].ToString();
                g[colKikaku, cnt].Value = t["SYOHIN_KIKAKU"].ToString();

                // 終売判断：2020/04/15
                if (Utility.IsShubai(t["LAST_SALE_YMD"].ToString()))
                {
                    g.Rows[cnt].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    g.Rows[cnt].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                }

                cnt++;
            }

            g.CurrentCell = null;

            this.Cursor = Cursors.Default;
        }


        ///--------------------------------------------------------
        /// <summary>
        ///     データグリッドに商品一覧を表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        ///--------------------------------------------------------
        //private void ShowShohin(DataGridView g, string [] syohin)
        //{
        //    this.Cursor = Cursors.WaitCursor;

        //    g.Rows.Clear();

        //    int cnt = 0;
        //    this.txtMemo.Text = "";

        //    foreach (var t in global.syohin_News.OrderBy(a => a.SYOHIN_CD))
        //    {
        //        // 仕入先名検索のとき
        //        if (txtMaker.Text.Trim() != string.Empty)
        //        {
        //            if (!t.SIRESAKI_NM.Contains(txtMaker.Text))
        //            {
        //                continue;
        //            }
        //        }

        //        // 商品名検索のとき
        //        if (txtSyohinName.Text.Trim() != string.Empty)
        //        {
        //            if (!t.SYOHIN_NM.Contains(txtSyohinName.Text))
        //            {
        //                continue;
        //            }
        //        }

        //        // 商品分類検索
        //        string Bunrui_L = Utility.NulltoStr(cmbSyohin_L.SelectedValue);
        //        string Bunrui_M = Utility.NulltoStr(cmbSyohin_M.SelectedValue);
        //        string Bunrui_S = Utility.NulltoStr(cmbSyohin_S.SelectedValue);

        //        // 大分類
        //        if (Bunrui_L != string.Empty)
        //        {
        //            if (t.SYOHIN_KIND_L_CD != Bunrui_L)
        //            {
        //                continue;
        //            }

        //            // 中分類
        //            if (Bunrui_M != string.Empty)
        //            {
        //                if (t.SYOHIN_KIND_M_CD != Bunrui_M)
        //                {
        //                    continue;
        //                }

        //                // 小分類
        //                if (Bunrui_S != string.Empty)
        //                {
        //                    if (t.SYOHIN_KIND_S_CD != Bunrui_S)
        //                    {
        //                        continue;
        //                    }
        //                }
        //            }
        //        }

        //        // 商品表示
        //        g.Rows.Add();
        //        g[colMaker, cnt].Value = t.SIRESAKI_NM;
        //        g[colHinCode, cnt].Value = t.SYOHIN_CD.PadLeft(8, '0');
        //        g[colHinName, cnt].Value = t.SYOHIN_NM;
        //        g[colIrisu, cnt].Value = t.CASE_IRISU;
        //        g[colKikaku, cnt].Value = t.SYOHIN_KIKAKU;

        //        cnt++;
        //    }

        //    g.CurrentCell = null;

        //    this.Cursor = Cursors.Default;
        //}


        ///--------------------------------------------------------
        /// <summary>
        ///     データグリッドに商品履歴を表示する </summary>
        /// <param name="g">
        ///     データグリッドビューオブジェクト</param>
        /// <param name="S_YYMMDD">
        ///     期間開始日</param>
        /// <param name="E_YYMMDD">
        ///     期間終了日</param>
        ///--------------------------------------------------------
        private void ShowShohinRireki(DataGridView g, int TokuisakiCD, string S_YYMMDD, string E_YYMMDD)
        {
            this.Cursor = Cursors.WaitCursor;

            // 商品明細クラス    
            ClsSyohinRireki[] rireki = new ClsSyohinRireki[global.dtSyohin.Rows.Count];     // 2020/04/09 
            //ClsSyohinRireki[] rireki = null;     // 2020/04/09

            cn.Open();

            try
            {
                //// 商品分類検索
                //string Bunrui_L = Utility.NulltoStr(cmbSyohin_L.SelectedValue);
                //string Bunrui_M = Utility.NulltoStr(cmbSyohin_M.SelectedValue);
                //string Bunrui_S = Utility.NulltoStr(cmbSyohin_S.SelectedValue);

                // 商品明細クラスに商品情報をセットする
                int i = 0;
                foreach (var t in global.dtSyohin.AsEnumerable())
                {
                    //// 大分類 : 2020/06/22
                    //if (Bunrui_L != string.Empty)
                    //{
                    //    if (t["SYOHIN_KIND_L_CD"].ToString().PadLeft(2, '0') != Bunrui_L)
                    //    {
                    //        continue;
                    //    }

                    //    // 中分類 : 2020/06/22
                    //    if (Bunrui_M != string.Empty)
                    //    {
                    //        if (t["SYOHIN_KIND_M_CD"].ToString().PadLeft(2, '0') != Bunrui_M)
                    //        {
                    //            continue;
                    //        }

                    //        // 小分類 : 2020/06/22
                    //        if (Bunrui_S != string.Empty)
                    //        {
                    //            if (t["SYOHIN_KIND_S_CD"].ToString().PadLeft(2, '0') != Bunrui_S)
                    //            {
                    //                continue;
                    //            }
                    //        }
                    //    }
                    //}

                    Array.Resize(ref rireki, i + 1);

                    rireki[i] = new ClsSyohinRireki();
                    rireki[i].SYOHIN_CD = t["SYOHIN_CD"].ToString();
                    rireki[i].SYOHIN_NM = t["SYOHIN_NM"].ToString();
                    rireki[i].SIRESAKI_NM = t["SIRESAKI_NM"].ToString();
                    rireki[i].SYOHIN_KIKAKU = t["SYOHIN_KIKAKU"].ToString();
                    rireki[i].CASE_IRISU = Utility.StrtoDouble(t["CASE_IRISU"].ToString());
                    rireki[i].Suu = 0;

                    // 終売：2020/04/15
                    rireki[i].Shubai = Utility.IsShubai(t["LAST_SALE_YMD"].ToString());

                    //// 大分類・中分類名称取得 : 2020/06/22
                    //string B_Name = GetSyohin_LM_Name(t["SYOHIN_KIND_L_CD"].ToString().PadLeft(2, '0'), t["SYOHIN_KIND_M_CD"].ToString().PadLeft(2, '0'));

                    //string [] bb_Name = B_Name.Split(',');

                    //// 大分類・中分類・小分類 : 2020/06/22
                    //if (bb_Name.Length > 1)
                    //{
                    //    rireki[i].SYOHIN_KIND_L_CD = bb_Name[0];
                    //    rireki[i].SYOHIN_KIND_M_CD = bb_Name[1];
                    //}
                    //else
                    //{
                    //    rireki[i].SYOHIN_KIND_L_CD = "";
                    //    rireki[i].SYOHIN_KIND_M_CD = "";
                    //}

                    rireki[i].SYOHIN_KIND_L_CD = "";
                    rireki[i].SYOHIN_KIND_M_CD = "";
                    rireki[i].SYOHIN_KIND_S_CD = "";

                    i++;
                }

                this.txtMemo.Text = "";

                // 該当得意先の商品別の発注数実績を抽出
                string sql = "select 得意先コード, 商品コード, sum(数量) as suu from orderhistory ";
                sql += "where 得意先コード = ? and (発注年月日 between ? and ?) ";
                sql += "group by 得意先コード, 商品コード order by suu desc";

                SQLiteDataReader reader = null;

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.CommandText = sql;
                    com.Parameters.AddWithValue("@TokCD", TokuisakiCD); // 得意先コード
                    com.Parameters.AddWithValue("@SYMD", S_YYMMDD);     // 期間開始日
                    com.Parameters.AddWithValue("@EYMD", E_YYMMDD);     // 期間終了日

                    reader = com.ExecuteReader();

                    // 商品明細クラスに発注実績数をセット
                    while (reader.Read())
                    {
                        for (int iX = 0; iX < rireki.Length; iX++)
                        {
                            // 商品コードが一致している
                            if (rireki[iX].SYOHIN_CD.PadLeft(8, '0') == reader["商品コード"].ToString().PadLeft(8, '0'))
                            {
                                rireki[iX].Suu = Utility.StrtoInt(reader["suu"].ToString());
                                break;
                            }
                        }
                    }

                    reader.Close();
                }

                g.Rows.Clear();

                int cnt = 0;

                // グリッドビューに表示
                foreach (var t in rireki.OrderByDescending(a => a.Suu).ThenBy(a => a.SYOHIN_CD))
                {
                    // 商品表示
                    g.Rows.Add();
                    g[colMaker, cnt].Value = t.SIRESAKI_NM;
                    g[colHinCode, cnt].Value = t.SYOHIN_CD;
                    g[colHinName, cnt].Value = t.SYOHIN_NM;
                    g[colKikaku, cnt].Value = t.SYOHIN_KIKAKU;
                    g[colKikanUri, cnt].Value = t.Suu;

                    // 終売判断：2020/04/15
                    if (t.Shubai)
                    {
                        g.Rows[cnt].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else
                    {
                        g.Rows[cnt].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                    }

                    cnt++;
                }

                g.CurrentCell = null;

                // 該当なしメッセージ
                if (cnt == 0)
                {
                    MessageBox.Show("該当する商品はありませんでした", "結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.Cursor = Cursors.Default;
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

        //private void gcMultiRow2_CellValueChanged(object sender, CellEventArgs e)
        //{
        //    if (!valueChangeStatus)
        //    {
        //        return;
        //    }

        //    if (e.CellName == "txtTdkNum")
        //    {
        //        valueChangeStatus = false;

        //        // 届先名表示
        //        string gTel = string.Empty;
        //        string gJyu = string.Empty; 

        //        valueChangeStatus = true;
        //    }
        //}

        ///-------------------------------------------------------------------
        /// <summary>
        ///     お届け先情報取得 </summary>
        /// <param name="tID">
        ///     届先番号</param>
        /// <param name="sTel">
        ///     電話番号</param>
        /// <param name="sJyu">
        ///     住所</param>
        /// <returns>
        ///     届先名</returns>
        ///-------------------------------------------------------------------
        //private string getNouhinName(string tID, out string sTel, out string sJyu)
        //{
        //    string val = string.Empty;
        //    sTel = string.Empty;
        //    sJyu = string.Empty;

        //    using (var Conn = new OracleConnection())
        //    {
        //        Conn.ConnectionString = ConfigurationManager.ConnectionStrings["OracleDbContext"].ConnectionString;
        //        Conn.Open();

        //        string strSQL = "SELECT KOK_ID, NOU_NAME, NOU_JYU1, NOU_JYU2, NOU_TEL from RAKUSYO_FAXOCR.V_NOUHINSAKI WHERE KOK_ID = '" + tID + "'";
        //        OracleCommand Cmd = new OracleCommand(strSQL, Conn);
        //        OracleDataReader dR = Cmd.ExecuteReader();
        //        while (dR.Read())
        //        {
        //            val = dR["NOU_NAME"].ToString().Trim();
        //            sTel = dR["NOU_TEL"].ToString().Trim();
        //            sJyu = dR["NOU_JYU1"].ToString().Trim() + " " + dR["NOU_JYU2"].ToString().Trim();
        //        }

        //        dR.Dispose();
        //        Cmd.Dispose();
        //    }

        //    return val;
        //}



        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
                e.Handled = true;
        }

        private void Control_KeyDown2(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.F12)
            //{
            //    frmTodoke frm = new frmTodoke(true);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        int r =  dataGridView2.CurrentCell.RowIndex;

            //        for (int i = 0; i < frm._nouCode.Length; i++)
            //        {
            //            if ((r + i) < 50)
            //            {
            //                dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
            //            }

            //            //dataGridView2.Rows.Add();
            //        }
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //    dataGridView2.CurrentCell = null;
            //}
        }

        private void dataGridView2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                //イベントハンドラが複数回追加されてしまうので最初に削除する
                //e.Control.KeyDown -= new KeyEventHandler(Control_KeyDown2);
                e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);

                //イベントハンドラを追加する
                if (dataGridView2.CurrentCell.ColumnIndex == 0)
                {
                    // お届け先検索画面表示
                    //e.Control.KeyDown += new KeyEventHandler(Control_KeyDown2);

                    // 数字のみ入力可能とする
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!valueChangeStatus)
            {
                return;
            }

            // 選択した得意先件数を取得する
            int cnt = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) != string.Empty)
                {
                    cnt++;
                }
            }

            // 得意先単独指定のときのみ「商品履歴」ボタンは使用可
            if (cnt == 1)
            {
                button2.Enabled = true;
                comboBox1.Enabled = true;
                dateTimePicker1.Enabled = false;
            }
            else
            {
                // 得意先1件以外は「商品履歴」ボタンは使用不可
                button2.Enabled = false;
                comboBox1.Enabled = false;
                dateTimePicker1.Enabled = false; ;
            }

            valueChangeStatus = false;

            if (e.ColumnIndex == 0)
            {
                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, e.RowIndex].Value).PadLeft(7, '0');

                if (tdkCode != "000000")
                {
                    dataGridView2[colTdkCode, e.RowIndex].Value = tdkCode;
                    dataGridView2.RefreshEdit();
                }

                dataGridView2[colTdkName, e.RowIndex].Value = string.Empty;

                // 2020/04/08 コメント化
                // 得意先名、電話番号、住所表示
                //string gName = string.Empty;
                //string gTel = string.Empty;
                //string gJyu = string.Empty;

                ClsCsvData.ClsCsvTokuisaki tokuisaki = Utility.GetTokuisakiFromDataTable(tdkCode, global.dtTokuisaki);  // 2020/04/08

                // 2020/04/08
                if (tdkCode != "000000" && tokuisaki.TOKUISAKI_NM == "")
                {
                    MessageBox.Show("未登録の得意先コードです", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    dataGridView2[colTdkName, e.RowIndex].Value = tokuisaki.TOKUISAKI_NM;   // 2020/04/08
                    dataGridView2[colTel, e.RowIndex].Value = tokuisaki.TOKUISAKI_TEL;      // 2020/04/08
                    dataGridView2[colAddress, e.RowIndex].Value = tokuisaki.TOKUISAKI_ZYUSYO1 + tokuisaki.TOKUISAKI_ZYUSYO2;    // 2020/04/08
                }

                // パターンID取得
                if (dataGridView2[colTdkName, e.RowIndex].Value.ToString() != string.Empty)
                {
                    int seqNum = 0;
                    int tdNum = Utility.StrtoInt(tdkCode);

                    if (dbPtn.Any(a => a.TokuisakiCode == tdNum))
                    {
                        // 現在の連番に「１」加算
                        seqNum = dbPtn.Where(a => a.TokuisakiCode == tdNum).Max(a => a.SeqNum);
                        seqNum++;
                        dataGridView2[colPtnNum, e.RowIndex].Value = seqNum.ToString();
                    }
                    else
                    {
                        // 初期値の「１」
                        dataGridView2[colPtnNum, e.RowIndex].Value = (seqNum + 1).ToString();
                    }
                }
                else
                {
                    dataGridView2[colPtnNum, e.RowIndex].Value = string.Empty;
                }
                
                dataGridView2.CurrentCell = null;
                valueChangeStatus = true;
            }
        }

        private void dataGridView2_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //string colName = dataGridView2.Columns[dataGridView2.CurrentCell.ColumnIndex].Name;
            //if (colName == colTdkCode)
            //{
            //    if (dataGridView2.IsCurrentCellDirty)
            //    {
            //        //コミットする
            //        dataGridView2.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //        dataGridView2.RefreshEdit();
            //    }
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataUpdate();
        }

        private void dataUpdate()
        {
            // 得意先確認
            if (getTdksaki() == 0)
            {
                MessageBox.Show("得意先をひとつ以上選択してください", "得意先未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                return;
            }

            int eCodeRow = sameTdkFind();
            if (eCodeRow != -1)
            {
                MessageBox.Show("同じ得意先が複数選択されています", "得意先選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                dataGridView2.CurrentCell = dataGridView2[colTdkCode, eCodeRow];
                return;
            }

            eCodeRow = getErrTdksaki();
            if (eCodeRow != -1)
            {
                MessageBox.Show("マスター未登録の得意先番号が選択されています", "得意先選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView2.Focus();
                dataGridView2.CurrentCell = dataGridView2[colTdkCode, eCodeRow];
                return;
            }

            // 商品確認
            if (dataGridView3.Rows.Count == 0)
            {
                MessageBox.Show("商品パターンを登録してください", "商品選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                dataGridView3.Focus();
                //gcMultiRow1.CurrentCell = gcMultiRow1[0, "lblNum"];
                return;
            }

            // リード日数確認
            for (int i = 0; i < dataGridView3.Rows.Count; i++)
            {
                // 商品登録されてリード日数がゼロのとき
                if (Utility.NulltoStr(dataGridView3.Rows[i].Cells[colHinCode].Value) != string.Empty && 
                    Utility.NulltoStr(dataGridView3.Rows[i].Cells[colReadDays].Value) == global.FLGOFF)
                {
                    if (MessageBox.Show("リード日数がゼロの商品があります。続行しますか？", "リード日数", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        dataGridView3.Focus();
                        return;
                    }
                }
            }

            // 登録確認
            int dCnt = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) == string.Empty)
                {
                    continue;
                }
                dCnt++;
            }

            if (MessageBox.Show(dCnt + "件の得意先の発注書パターンを登録します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (fMode == ADDMODE)
            {
                // 登録処理
                dataAdd(dataGridView2, dataGridView3);

                // 続けて登録確認：2017/08/21
                if (MessageBox.Show("現在の得意先を続けて登録しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
                {
                    // 画面初期化
                    dispInitial();
                }
                else
                {
                    // 一部初期化
                    continueInitial();
                }
            }
            else if (fMode == EDITMODE)
            {
                if (dataGridView3.Rows.Count > global.MAX_GYO)
                {
                    if (MessageBox.Show("商品数が16行以上登録されています。１枚の発注書に登録される商品は15行までです。続行しますか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                // 更新処理
                dataUpdate(dataGridView3);

                // 画面初期化
                dispInitial();
            }
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     発注書パターン登録 </summary>
        /// <param name="g">
        ///     dataGridViewオブジェクト：商品選択ビュー</param>
        /// <param name="m">
        ///     dataGridViewオブジェクト：商品登録欄ビュー</param>
        ///----------------------------------------------------------------------
        private void dataAdd(DataGridView g, DataGridView m)
        {
            cn.Open();

            try
            {
                string sqlSy = "";

                for (int i = 0; i < g.Rows.Count; i++)
                {
                    if (Utility.NulltoStr(g[colTdkCode, i].Value) == string.Empty)
                    {
                        continue;
                    }

                    string sql = "insert into orderpattern ";
                    sql += "(得意先コード, 連番, 枝番, ";
                    sql += "商品1, 商品名1, 商品1リード日数, 商品2, 商品名2, 商品2リード日数, 商品3, 商品名3, 商品3リード日数, 商品4, 商品名4, 商品4リード日数, 商品5, 商品名5, 商品5リード日数, ";
                    sql += "商品6, 商品名6, 商品6リード日数, 商品7, 商品名7, 商品7リード日数, 商品8, 商品名8, 商品8リード日数, 商品9, 商品名9, 商品9リード日数, 商品10, 商品名10, 商品10リード日数, ";
                    sql += "商品11, 商品名11, 商品11リード日数, 商品12, 商品名12, 商品12リード日数, 商品13, 商品名13, 商品13リード日数, 商品14, 商品名14, 商品14リード日数, 商品15, 商品名15, 商品15リード日数, ";
                    sql += "商品16, 商品名16, 商品16リード日数, 商品17, 商品名17, 商品17リード日数, 商品18, 商品名18, 商品18リード日数, 商品19, 商品名19, 商品19リード日数, 商品20, 商品名20, 商品20リード日数, ";
                    //sql += "備考, 更新年月日) ";
                    sql += "備考, 更新年月日, Comment1, Comment2, Comment3, Comment4) ";
                    sql += "values (";
                    sql += g[colTdkCode, i].Value.ToString() + "," + g[colPtnNum, i].Value.ToString();

                    int pCnt = global.flgOn;
                    int sCnt = 0;

                    sqlSy = "," + pCnt;

                    for (int iX = 0; iX < m.Rows.Count; iX++)
                    {
                        sCnt++;
                        sqlSy += ",'" + Utility.NulltoStr(m[colHinCode, iX].Value) + "','" + Utility.NulltoStr(m[colHinName, iX].Value) + "'," + Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, iX].Value));

                        if (sCnt == global.MAX_GYO)
                        {
                            sqlSy += ",'','',0,'','',0,'','',0,'','',0,'','',0,'" + txtMemo.Text + "','" + DateTime.Now.ToString() + "',";
                            sqlSy += "'" + txtComment1.Text + "','" + txtComment2.Text + "','" + txtComment3.Text + "','" + txtComment4.Text + "')";

                            //MessageBox.Show(sql + sqlSy);

                            using (SQLiteCommand com = new SQLiteCommand(sql + sqlSy, cn))
                            {
                                com.ExecuteNonQuery();
                            }

                            sCnt = 0;

                            if ((iX + 1) < m.Rows.Count)
                            {
                                pCnt++;
                                sqlSy = "," + pCnt;
                            }
                        }

                        //sCnt++;
                    }

                    if (sCnt > 0)
                    {
                        if (sCnt < global.MAX_GYO)
                        {
                            for (int iX = sCnt; iX < global.MAX_GYO; iX++)
                            {
                                sqlSy += ",'','',0";
                            }
                        }

                        //sqlSy += ",'','',0,'','',0,'','',0,'','',0,'','',0,'" + txtMemo.Text + "','" + DateTime.Now.ToString() + "');";

                        sqlSy += ",'','',0,'','',0,'','',0,'','',0,'','',0,'" + txtMemo.Text + "','" + DateTime.Now.ToString() + "',";
                        sqlSy += "'" + txtComment1.Text + "','" + txtComment2.Text + "','" + txtComment3.Text + "','" + txtComment4.Text + "')";

                        //MessageBox.Show(sql + sqlSy);

                        using (SQLiteCommand com = new SQLiteCommand(sql + sqlSy, cn))
                        {
                            com.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("注文書パターンが登録されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                cn.Close();
            }
        }

        ///-------------------------------------------------------------------------------
        ///
        /// <summary>
        ///     発注書パターン更新処理 </summary>
        /// <param name="m">
        ///     DataGridViewオブジェクト</param>
        ///-------------------------------------------------------------------------------
        private void dataUpdate(DataGridView m)
        {
            try
            {
                ClsOrderPattern.G_Code1 = Utility.NulltoStr(m[colHinCode, 0].Value);
                ClsOrderPattern.G_Code2 = Utility.NulltoStr(m[colHinCode, 1].Value);
                ClsOrderPattern.G_Code3 = Utility.NulltoStr(m[colHinCode, 2].Value);
                ClsOrderPattern.G_Code4 = Utility.NulltoStr(m[colHinCode, 3].Value);
                ClsOrderPattern.G_Code5 = Utility.NulltoStr(m[colHinCode, 4].Value);
                ClsOrderPattern.G_Code6 = Utility.NulltoStr(m[colHinCode, 5].Value);
                ClsOrderPattern.G_Code7 = Utility.NulltoStr(m[colHinCode, 6].Value);
                ClsOrderPattern.G_Code8 = Utility.NulltoStr(m[colHinCode, 7].Value);
                ClsOrderPattern.G_Code9 = Utility.NulltoStr(m[colHinCode, 8].Value);
                ClsOrderPattern.G_Code10 = Utility.NulltoStr(m[colHinCode, 9].Value);
                ClsOrderPattern.G_Code11 = Utility.NulltoStr(m[colHinCode, 10].Value);
                ClsOrderPattern.G_Code12 = Utility.NulltoStr(m[colHinCode, 11].Value);
                ClsOrderPattern.G_Code13 = Utility.NulltoStr(m[colHinCode, 12].Value);
                ClsOrderPattern.G_Code14 = Utility.NulltoStr(m[colHinCode, 13].Value);
                ClsOrderPattern.G_Code15 = Utility.NulltoStr(m[colHinCode, 14].Value);

                ClsOrderPattern.G_Name1 = Utility.NulltoStr(m[colHinName, 0].Value);
                ClsOrderPattern.G_Name2 = Utility.NulltoStr(m[colHinName, 1].Value);
                ClsOrderPattern.G_Name3 = Utility.NulltoStr(m[colHinName, 2].Value);
                ClsOrderPattern.G_Name4 = Utility.NulltoStr(m[colHinName, 3].Value);
                ClsOrderPattern.G_Name5 = Utility.NulltoStr(m[colHinName, 4].Value);
                ClsOrderPattern.G_Name6 = Utility.NulltoStr(m[colHinName, 5].Value);
                ClsOrderPattern.G_Name7 = Utility.NulltoStr(m[colHinName, 6].Value);
                ClsOrderPattern.G_Name8 = Utility.NulltoStr(m[colHinName, 7].Value);
                ClsOrderPattern.G_Name9 = Utility.NulltoStr(m[colHinName, 8].Value);
                ClsOrderPattern.G_Name10 = Utility.NulltoStr(m[colHinName, 9].Value);
                ClsOrderPattern.G_Name11 = Utility.NulltoStr(m[colHinName, 10].Value);
                ClsOrderPattern.G_Name12 = Utility.NulltoStr(m[colHinName, 11].Value);
                ClsOrderPattern.G_Name13 = Utility.NulltoStr(m[colHinName, 12].Value);
                ClsOrderPattern.G_Name14 = Utility.NulltoStr(m[colHinName, 13].Value);
                ClsOrderPattern.G_Name15 = Utility.NulltoStr(m[colHinName, 14].Value);

                ClsOrderPattern.G_Read1 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 0].Value));
                ClsOrderPattern.G_Read2 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 1].Value));
                ClsOrderPattern.G_Read3 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 2].Value));
                ClsOrderPattern.G_Read4 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 3].Value));
                ClsOrderPattern.G_Read5 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 4].Value));
                ClsOrderPattern.G_Read6 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 5].Value));
                ClsOrderPattern.G_Read7 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 6].Value));
                ClsOrderPattern.G_Read8 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 7].Value));
                ClsOrderPattern.G_Read9 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 8].Value));
                ClsOrderPattern.G_Read10 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 9].Value));
                ClsOrderPattern.G_Read11 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 10].Value));
                ClsOrderPattern.G_Read12 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 11].Value));
                ClsOrderPattern.G_Read13 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 12].Value));
                ClsOrderPattern.G_Read14 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 13].Value));
                ClsOrderPattern.G_Read15 = Utility.StrtoInt(Utility.NulltoStr(m[colReadDays, 14].Value));

                ClsOrderPattern.Memo = txtMemo.Text;
                ClsOrderPattern.YyMmDd = DateTime.Now.ToString();

                ClsOrderPattern.comment1 = txtComment1.Text;    // 2020/04/01
                ClsOrderPattern.comment2 = txtComment2.Text;    // 2020/04/01
                ClsOrderPattern.comment3 = txtComment3.Text;    // 2020/04/01
                ClsOrderPattern.comment4 = txtComment4.Text;    // 2020/04/01

                // データベース更新
                context.SubmitChanges();

                MessageBox.Show("発注書パターンが更新されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void dataDelete(int sID)
        {
            cn.Open();

            try
            {
                string sql = "delete from orderpattern where ID = " + sID;

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                MessageBox.Show("発注書パターンが削除されました", "処理終了", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPtnAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        ///---------------------------------------------------------
        /// <summary>
        ///     登録届先数取得  </summary>
        /// <returns>
        ///     件数</returns>
        ///---------------------------------------------------------
        private int getTdksaki()
        {
            int cnt = 0;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) == string.Empty)
                {
                    continue;
                }

                cnt++;
            }

            return cnt;
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     マスター未登録の届先番号の行番号を取得する </summary>
        /// <returns>
        ///     行番号</returns>
        ///----------------------------------------------------------------------
        private int getErrTdksaki()
        {
            int cnt = -1;

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if ((Utility.NulltoStr(dataGridView2[colTdkCode, i].Value) != string.Empty) && 
                    (Utility.NulltoStr(dataGridView2[colTdkName, i].Value) == string.Empty))
                {
                    cnt = i;
                    break;
                }
            }

            return cnt;
        }

        private string getTdksakiCode(DataGridView g)
        {
            string nou_Code = string.Empty;

            for (int i = 0; i < g.Rows.Count; i++)
            {
                nou_Code = Utility.NulltoStr(g[colTdkCode, i].Value);

                if (nou_Code != string.Empty)
                {
                    break;
                }
            }

            return nou_Code;
        }

        private int getPtnShohin()
        {
            int cnt = 0;

            //for (int i = 0; i < gcMultiRow1.RowCount; i++)
            //{
            //    if (Utility.NulltoStr(gcMultiRow1[i, "txtHinCode"].Value) != string.Empty)
            //    {
            //        cnt++;
            //    }

            //    if (Utility.NulltoStr(gcMultiRow1[i, "txtHinCode2"].Value) != string.Empty)
            //    {
            //        cnt++;
            //    }
            //}

            return cnt;
        }

        private int sameTdkFind()
        {
            int val = -1;

            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                if (val != -1)
                {
                    break;
                }

                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, i].Value);

                if (tdkCode == string.Empty)
                {
                    continue;
                }

                if (i < dataGridView2.RowCount - 1)
                {
                    for (int j = i + 1; j < dataGridView2.RowCount; j++)
                    {
                        if (Utility.NulltoStr(dataGridView2[colTdkCode, j].Value) == tdkCode)
                        {
                            val = j;
                            break;
                        }
                    }
                }
            }

            return val;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            rirekiGridviewSet(dataGridView1);

            int tokCD = 0; 

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                string val = Utility.NulltoStr(dataGridView2[colTdkCode, i].Value);
                if (val == string.Empty)
                {
                    continue;
                }

                // 得意先コード
                tokCD = Utility.StrtoInt(val);
                break;
            }
            
            string SYYMMDD = "19000101";

            if (dateTimePicker1.Checked)
            {
                SYYMMDD = dateTimePicker1.Value.Year + dateTimePicker1.Value.Month.ToString("D2") + dateTimePicker1.Value.Day.ToString("D2");
            }

            string EYYMMDD = DateTime.Today.Year + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2");

            // 商品履歴表示
            ShowShohinRireki(dataGridView1, tokCD, SYYMMDD, EYYMMDD);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //lblFrDate.Text = "";
                dateTimePicker1.Checked = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                // １ヶ月以内
                frDt = DateTime.Today.AddMonths(-1);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                // ３ヶ月以内
                frDt = DateTime.Today.AddMonths(-3);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                // ６ヶ月以内
                frDt = DateTime.Today.AddMonths(-6);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                // １年以内
                frDt = DateTime.Today.AddYears(-1);
                dateTimePicker1.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                // 期間を指定
                frDt = DateTime.Today;
                dateTimePicker1.Enabled = true;
            }

            if (comboBox1.SelectedIndex != 0)
            {
                //lblFrDate.Text = frDt.ToShortDateString() + "～";
                dateTimePicker1.Checked = true;
                dateTimePicker1.Value = frDt;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // パターン呼出
            callPattern();
        }

        private void callPattern()
        {
            // 画面初期化
            dispInitial();

            // パターン呼出
            //adp.Fill(dts.パターンID);

            frmPtnCall frm = new frmPtnCall();
            frm.ShowDialog();

            if (frm.ptnID != string.Empty)
            {
                fID = Utility.StrtoInt(frm.ptnID);
                getPatterIDData(fID);

                fMode = EDITMODE;
                dataGridView2.ReadOnly = true;
                btnDel.Visible = true;
                button7.Enabled = false;
            }
            else
            {
                fID = 0;
            }

            // 後片付け
            frm.Dispose();
        }


        ///--------------------------------------------------------------------
        /// <summary>
        ///     登録済み注文書パターンを取得して表示する </summary>
        /// <param name="sID">
        ///     ID</param>
        ///--------------------------------------------------------------------
        private void getPatterIDData(int sID)
        {
            if (!dbPtn.Any(a => a.ID == sID))
            {
                MessageBox.Show("発注書パターンの取得に失敗しました。:" + sID.ToString());
                return;
            }

            ClsOrderPattern = dbPtn.Single(a => a.ID == sID);

            // 得意先グリッドは1行とする
            dataGridView2.Rows.Clear();
            dataGridView2.Rows.Add();
            dataGridView2[colTdkCode, 0].Value =ClsOrderPattern.TokuisakiCode.ToString().PadLeft(7, '0');

            valueChangeStatus = false;
            dataGridView2[colPtnNum, 0].Value =ClsOrderPattern.SeqNum.ToString();
            dataGridView2[colSecondNum, 0].Value =ClsOrderPattern.SecondNum.ToString();

            // 発注書グリッド
            valueChangeStatus = true;

            dataGridView3.Rows.Add(global.MAX_GYO);

            dataGridView3[colSeqNum, 0].Value = 1;
            dataGridView3[colHinCode, 0].Value =ClsOrderPattern.G_Code1;
            dataGridView3[colHinName, 0].Value =ClsOrderPattern.G_Name1;
            dataGridView3[colReadDays, 0].Value = ClsOrderPattern.G_Read1;

            dataGridView3[colSeqNum, 1].Value = 2;
            dataGridView3[colHinCode, 1].Value =ClsOrderPattern.G_Code2;
            dataGridView3[colHinName, 1].Value =ClsOrderPattern.G_Name2;
            dataGridView3[colReadDays, 1].Value = ClsOrderPattern.G_Read2;

            dataGridView3[colSeqNum, 2].Value = 3;
            dataGridView3[colHinCode, 2].Value =ClsOrderPattern.G_Code3;
            dataGridView3[colHinName, 2].Value =ClsOrderPattern.G_Name3;
            dataGridView3[colReadDays, 2].Value = ClsOrderPattern.G_Read3;

            dataGridView3[colSeqNum, 3].Value = 4;
            dataGridView3[colHinCode, 3].Value =ClsOrderPattern.G_Code4;
            dataGridView3[colHinName, 3].Value =ClsOrderPattern.G_Name4;
            dataGridView3[colReadDays, 3].Value = ClsOrderPattern.G_Read4;

            dataGridView3[colSeqNum, 4].Value = 5;
            dataGridView3[colHinCode, 4].Value =ClsOrderPattern.G_Code5;
            dataGridView3[colHinName, 4].Value =ClsOrderPattern.G_Name5;
            dataGridView3[colReadDays, 4].Value = ClsOrderPattern.G_Read5;

            dataGridView3[colSeqNum, 5].Value = 6;
            dataGridView3[colHinCode, 5].Value =ClsOrderPattern.G_Code6;
            dataGridView3[colHinName, 5].Value =ClsOrderPattern.G_Name6;
            dataGridView3[colReadDays, 5].Value = ClsOrderPattern.G_Read6;

            dataGridView3[colSeqNum, 6].Value = 7;
            dataGridView3[colHinCode, 6].Value =ClsOrderPattern.G_Code7;
            dataGridView3[colHinName, 6].Value =ClsOrderPattern.G_Name7;
            dataGridView3[colReadDays, 6].Value = ClsOrderPattern.G_Read7;

            dataGridView3[colSeqNum, 7].Value = 8;
            dataGridView3[colHinCode, 7].Value =ClsOrderPattern.G_Code8;
            dataGridView3[colHinName, 7].Value =ClsOrderPattern.G_Name8;
            dataGridView3[colReadDays, 7].Value = ClsOrderPattern.G_Read8;

            dataGridView3[colSeqNum, 8].Value = 9;
            dataGridView3[colHinCode, 8].Value =ClsOrderPattern.G_Code9;
            dataGridView3[colHinName, 8].Value =ClsOrderPattern.G_Name9;
            dataGridView3[colReadDays, 8].Value = ClsOrderPattern.G_Read9;

            dataGridView3[colSeqNum, 9].Value = 10;
            dataGridView3[colHinCode, 9].Value =ClsOrderPattern.G_Code10;
            dataGridView3[colHinName, 9].Value =ClsOrderPattern.G_Name10;
            dataGridView3[colReadDays, 9].Value = ClsOrderPattern.G_Read10;

            dataGridView3[colSeqNum, 10].Value = 11;
            dataGridView3[colHinCode, 10].Value =ClsOrderPattern.G_Code11;
            dataGridView3[colHinName, 10].Value =ClsOrderPattern.G_Name11;
            dataGridView3[colReadDays, 10].Value = ClsOrderPattern.G_Read11;

            dataGridView3[colSeqNum, 11].Value = 12;
            dataGridView3[colHinCode, 11].Value =ClsOrderPattern.G_Code12;
            dataGridView3[colHinName, 11].Value =ClsOrderPattern.G_Name12;
            dataGridView3[colReadDays, 11].Value = ClsOrderPattern.G_Read12;

            dataGridView3[colSeqNum, 12].Value = 13;
            dataGridView3[colHinCode, 12].Value =ClsOrderPattern.G_Code13;
            dataGridView3[colHinName, 12].Value =ClsOrderPattern.G_Name13;
            dataGridView3[colReadDays, 12].Value = ClsOrderPattern.G_Read13;

            dataGridView3[colSeqNum, 13].Value = 14;
            dataGridView3[colHinCode, 13].Value =ClsOrderPattern.G_Code14;
            dataGridView3[colHinName, 13].Value =ClsOrderPattern.G_Name14;
            dataGridView3[colReadDays, 13].Value = ClsOrderPattern.G_Read14;

            dataGridView3[colSeqNum, 14].Value = 15;
            dataGridView3[colHinCode, 14].Value =ClsOrderPattern.G_Code15;
            dataGridView3[colHinName, 14].Value =ClsOrderPattern.G_Name15;
            dataGridView3[colReadDays, 14].Value = ClsOrderPattern.G_Read15;

            txtMemo.Text = ClsOrderPattern.Memo;

            txtComment1.Text = ClsOrderPattern.comment1;
            txtComment2.Text = ClsOrderPattern.comment2;
            txtComment3.Text = ClsOrderPattern.comment3;
            txtComment4.Text = ClsOrderPattern.comment4;

            dataGridView3.CurrentCell = null;
        }

        //private string ptnShohinStr(int s)
        //{
        //    string val = string.Empty;

        //    if (s == global.flgOff)
        //    {
        //        val = string.Empty;
        //    }
        //    else
        //    {
        //        val = s.ToString().PadLeft(8, '0');
        //    }

        //    return val;
        //}

        private void button6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画面を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            dispInitial();
        }

        private void dataGridView2_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                valueChangeStatus = false;

                string tdkCode = Utility.NulltoStr(dataGridView2[colTdkCode, e.RowIndex].Value).PadLeft(6, '0');

                if (tdkCode != "000000")
                {
                    dataGridView2[colTdkCode, e.RowIndex].Value = tdkCode;
                }

                valueChangeStatus = true;
            }
        }

        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyData == Keys.F12)
            //{
            //    frmTodoke frm = new frmTodoke(true);
            //    frm.ShowDialog();

            //    if (frm._nouCode != null)
            //    {
            //        int r = dataGridView2.CurrentCell.RowIndex;

            //        for (int i = 0; i < frm._nouCode.Length; i++)
            //        {
            //            if ((r + i) < 50)
            //            {
            //                dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
            //            }

            //            //dataGridView2.Rows.Add();
            //        }
            //    }

            //    // 後片付け
            //    frm.Dispose();
            //    dataGridView2.CurrentCell = null;
            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmTodoke frm = new frmTodoke(true);
            frm.ShowDialog();

            if (frm._nouCode != null)
            {
                //int r = dataGridView2.CurrentCell.RowIndex;
                bool edt;
                int r = 0;

                // 上書きセル指定か？
                if (dataGridView2.CurrentCell == null)
                {
                    edt = false;
                }
                else
                {
                    edt = true;
                    r = dataGridView2.CurrentCell.RowIndex;
                }

                for (int i = 0; i < frm._nouCode.Length; i++)
                {
                    // 新規追加登録
                    if (!edt)
                    {
                        for (int iX = 0; iX < 50; iX++)
                        {
                            if (dataGridView2[colTdkCode, iX].Value == null ||
                                dataGridView2[colTdkCode, iX].Value.ToString() == string.Empty)
                            {
                                dataGridView2[colTdkCode, iX].Value = frm._nouCode[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 上書き
                        if ((r + i) < 50)
                        {
                            dataGridView2[colTdkCode, r + i].Value = frm._nouCode[i];
                        }
                    }
                }
            }

            // 後片付け
            frm.Dispose();
            dataGridView2.CurrentCell = null;
        }

        private void frmPtnAdd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F8)
            {
                // パターン呼出
                callPattern();
            }

            if (e.KeyData == Keys.F9)
            {
                if (MessageBox.Show("画面を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                dispInitial();
            }

            if (e.KeyData == Keys.F10)
            {
                if (MessageBox.Show("注文商品を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                valueChangeStatus = false;
                //gcMrSetting();
                valueChangeStatus = true;
            }

            if (e.KeyData == Keys.F11)
            {
                // 更新
                dataUpdate();
            }

            if (e.KeyData == Keys.F12)
            {
                // 終了
                Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("注文商品を初期化します。よろしいですか", "取消確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            valueChangeStatus = false;
            gcMrSetting();
            valueChangeStatus = true;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の注文書パターンを削除します。よろしいですか", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            // 削除処理
            dataDelete(fID);

            // 画面初期化
            dispInitial();
        }

        private void dataGridView1_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == colSuu || e.Column.Name == colIrisu)
            {
                e.SortResult = Utility.StrtoInt(Utility.NulltoStr(e.CellValue1)) - Utility.StrtoInt(Utility.NulltoStr(e.CellValue2));
                e.Handled = true;
            }
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            int s = dataGridView1.SelectedRows.Count;

            if (s > 0)
            {
                //dataGridView3.ContextMenuStrip.Items[0].Enabled = true;
                DataGridMouse = false;
            }

            //if (s > 15)
            //{
            //    MessageBox.Show(s + "件選択されています。16件以上は選択できません。", "選択制限数オーバー",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    dataGridView1.ClearSelection();
            //}
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 商品コード
            if (e.ColumnIndex == 2)
            {
                string syCD = Utility.NulltoStr(dataGridView3[colHinCode, e.RowIndex].Value).ToString().PadLeft(0, '8');

                // 2020/04/09
                ClsCsvData.ClsCsvSyohin_New cls = Utility.GetSyohinsFromDataTable(global.dtSyohin, syCD);

                dataGridView3[colMaker, e.RowIndex].Value = cls.SIRESAKI_NM;
                dataGridView3[colHinName, e.RowIndex].Value = cls.SYOHIN_NM;
                dataGridView3[colKikaku, e.RowIndex].Value = cls.SYOHIN_KIKAKU;
                dataGridView3[colIrisu, e.RowIndex].Value = cls.CASE_IRISU;

                dataGridView3[colJanCD, e.RowIndex].Value = cls.JAN_CD;
                dataGridView3[colReadDays, e.RowIndex].Value = global.FLGOFF;   // 初期値ゼロ表示：2020/04/10

                // 大分類、中分類表示：2020/06/23
                //string [] bArray = GetSyohin_LM_Name(cls.SYOHIN_KIND_L_CD, cls.SYOHIN_KIND_M_CD).Split(',');
                //if (bArray.Length > 1)
                //{
                //    dataGridView3[colBunrui_L, e.RowIndex].Value = bArray[0];
                //    dataGridView3[colBunrui_M, e.RowIndex].Value = bArray[1];
                //}
                //else
                //{
                //    dataGridView3[colBunrui_L, e.RowIndex].Value = "";
                //    dataGridView3[colBunrui_M, e.RowIndex].Value = "";
                //}

                // 大分類、中分類表示：2020/06/23
                dataGridView3[colBunrui_L, e.RowIndex].Value = GetSyohin_LM_Name(cls.SYOHIN_KIND_L_CD, cls.SYOHIN_KIND_M_CD);
            }

            // リード日数
            if (e.ColumnIndex == 7)
            {
                if (Utility.NulltoStr(dataGridView3.Rows[e.RowIndex].Cells[colHinCode].Value) != string.Empty &&
                    Utility.NulltoStr(dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == global.FLGOFF)
                {
                    dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = Color.Red;
                }
                else
                {
                    dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private void 行挿入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                int r = dataGridView3.CurrentRow.Index;
                dataGridView3.Rows.Insert(r);
                //dataGridView3[colHinCode, r].Value = "";
            }

            // ナンバーリング
            SetGridSeqNumber(dataGridView3);
        }

        private void 行削除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView3.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow  item in dataGridView3.SelectedRows)
                {
                    dataGridView3.Rows.Remove(item);
                }

                //dataGridView3.Rows.Remove(dataGridView3.CurrentRow);
                dataGridView3.CurrentCell = null;
            }

            // ナンバーリング
            SetGridSeqNumber(dataGridView3);
        }

        private void 最後に追加AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // 商品追加
                for (int i = dataGridView1.SelectedRows.Count - 1; i >= 0; i--)
                {
                    dataGridView3.Rows.Add();

                    //int gRow = r + dataGridView1.SelectedRows.Count - 1 - i;

                    dataGridView3[colHinCode, dataGridView3.Rows.Count - 1].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[i].Index].Value.ToString().PadLeft(8, '0');
                    dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.LightPink;
                }

                dataGridView1.CurrentCell = null;
                dataGridView3.CurrentCell = null;

                dataGridView3.ContextMenuStrip.Items[0].Enabled = false;

                DataGridMouse = true;

                // ナンバーリング
                SetGridSeqNumber(dataGridView3);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView3.ContextMenuStrip.Items[0].Enabled = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dataGridView1.SelectAll();
            dataGridView3.ContextMenuStrip.Items[0].Enabled = true;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // 登録商品が選択されているか
            if (dataGridView1.SelectedRows.Count > 0)
            {
                toolStripMenuItem_SyohinAdd.Enabled = true;
            }
            else
            {
                toolStripMenuItem_SyohinAdd.Enabled = false;
            }

            // 登録済み商品の選択状況
            if (dataGridView3.SelectedRows.Count > 0)
            {
                ToolStripMenuItem_Delete.Enabled = true;    // 行削除
                ToolStripMenuItem_ReadDays.Enabled = true;  // リード日数設定

                if (dataGridView3.SelectedRows.Count == 1)
                {
                    ToolStripMenuItem_SyohinInsert.Enabled = true;  // 選択商品の挿入登録
                    ToolStripMenuItem_Insert.Enabled = true;        // 行挿入
                    ToolStripMenuItem_Before.Enabled = true;        // 前へ移動
                    ToolStripMenuItem_After.Enabled = true;         // 後へ移動
                }
                else
                {
                    ToolStripMenuItem_Insert.Enabled = false;   // 行挿入
                    ToolStripMenuItem_Before.Enabled = false;   // 前へ移動
                    ToolStripMenuItem_After.Enabled = false;    // 後へ移動
                }

                ToolStripMenuItem_Sort.Enabled = true;      // 商品分類、商品コードで並び替え：2020/06/23
            }
            else
            {
                ToolStripMenuItem_SyohinInsert.Enabled = false;    // 選択商品の挿入登録
                ToolStripMenuItem_Insert.Enabled = false;   // 行挿入
                ToolStripMenuItem_Delete.Enabled = false;   // 行削除
                ToolStripMenuItem_Before.Enabled = false;   // 前へ移動
                ToolStripMenuItem_After.Enabled = false;    // 後へ移動
                ToolStripMenuItem_ReadDays.Enabled = false; // リード日数

                // 登録商品があるか：2020/06/23
                if (dataGridView3.Rows.Count < 1)
                {
                    ToolStripMenuItem_Sort.Enabled = false;      // 商品分類、商品コードで並び替え：2020/06/23
                }
                else
                {
                    ToolStripMenuItem_Sort.Enabled = true;      // 商品分類、商品コードで並び替え：2020/06/23
                }
            }
        }

        private void 現在の行に挿入IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int r = dataGridView3.SelectedRows[0].Index;

            // 商品の数だけ選択行の前に行挿入
            dataGridView3.Rows.Insert(dataGridView3.CurrentRow.Index, dataGridView1.SelectedRows.Count);

            int iX = 0;

            // 商品登録
            for (int i = dataGridView1.SelectedRows.Count - 1; i >= 0; i--)
            {
                dataGridView3[colHinCode, r + iX].Value = dataGridView1[colHinCode, dataGridView1.SelectedRows[i].Index].Value.ToString().PadLeft(8, '0');
                dataGridView1.SelectedRows[i].DefaultCellStyle.BackColor = Color.LightPink;

                iX++;
            }

            dataGridView1.CurrentCell = null;
            dataGridView3.CurrentCell = null;

            dataGridView3.ContextMenuStrip.Items[0].Enabled = false;
            toolStripMenuItem_SyohinAdd.DropDownItems[1].Enabled = false;
            dataGridView3.ContextMenuStrip.Items[1].Enabled = false;
            dataGridView3.ContextMenuStrip.Items[2].Enabled = false;
            dataGridView3.ContextMenuStrip.Items[3].Enabled = false;
            dataGridView3.ContextMenuStrip.Items[4].Enabled = false;

            DataGridMouse = true;

            // ナンバーリング
            SetGridSeqNumber(dataGridView3);
        }

        private void dataGridView3_MouseMove(object sender, MouseEventArgs e)
        {
            //if (!DataGridMouse)
            //{
            //    return;
            //}

            //// マウス座標のDataGridViewの位置情報を取得します。
            //var hitTest = dataGridView3.HitTest(e.X, e.Y);

            //// 念のため、有効なセル上でのみ許可する
            //int newRowIndex = dataGridView3.NewRowIndex;
            //int rowIndex = hitTest.RowIndex;
            //if ((hitTest.Type == DataGridViewHitTestType.Cell)
            //    && ((newRowIndex == -1) || (newRowIndex != rowIndex)))
            //{
            //    // ドラッグアンドドロップ動作を開始します。
            //    var row = dataGridView3.Rows[rowIndex];
            //    dataGridView3.DoDragDrop(row, DragDropEffects.Copy);
            //}
        }

        private void dataGridView3_DragEnter(object sender, DragEventArgs e)
        {
            //if (!DataGridMouse)
            //{
            //    return;
            //}

            //// ドラッグアンドドロップのドロップ効果をコピーに設定します。
            //e.Effect = DragDropEffects.Copy;
        }

        private void dataGridView3_DragDrop(object sender, DragEventArgs e)
        {
            //if (!DataGridMouse)
            //{
            //    return;
            //}

            //// ドロップ元(dataGridView1)のデータを取得します。
            //var row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
            //var cellDataNum = row.Cells.Count;
            //var cellData = new object[cellDataNum];

            //for (int column = 0; column < cellDataNum; ++column)
            //{
            //    cellData[column] = row.Cells[column].Value;
            //}

            //// ドロップ先(dataGridView2)のクライアント位置からDataGridViewの位置情報を取得します。
            //var point = dataGridView3.PointToClient(new Point(e.X, e.Y));
            //var hitTest = dataGridView3.HitTest(point.X, point.Y);

            //// ドロップ先(dataGridView2)の行位置を取得します。
            //int rowIndex = hitTest.RowIndex;

            //// ドロップ先(dataGridView2)の行位置がヘッダー行では無い場合
            //if (rowIndex != -1)
            //{
            //    // 該当行に挿入します。
            //    dataGridView3.Rows.Insert(rowIndex, cellData);
            //}

            //// ドロップ先(dataGridView2)の行位置がヘッダー行の場合
            //else
            //{
            //    // 末尾に行を追加します。
            //    dataGridView3.Rows.Add(cellData);
            //}

            //// 移動する場合、元のdataGridView1から該当行を削除します。
            //// コピーする場合、以下の行はコメントアウトしてください。
            //row.DataGridView.Rows.Remove(row);
        }

        private void 前へ移動BToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow_Move(dataGridView3, ROW_MINUS);

            // ナンバーリング
            SetGridSeqNumber(dataGridView3);
        }

        private void ToolStripMenuItem_After_Click(object sender, EventArgs e)
        {
            DataGridViewRow_Move(dataGridView3, ROW_PLUS);

            // ナンバーリング
            SetGridSeqNumber(dataGridView3);
        }

        ///-----------------------------------------------------------------------
        /// <summary>
        ///     DataGridView行を移動する　</summary>
        /// <param name="dg">
        ///     DataGridView オブジェクト　</param>
        /// <param name="n">
        ///     移動方向</param>
        ///-----------------------------------------------------------------------
        private void DataGridViewRow_Move(DataGridView dg, int n)
        {
            // 複数選択は不可
            if (dg.SelectedRows.Count > 1)
            {
                return;
            }

            int r = dg.SelectedRows[0].Index;

            if (n == ROW_MINUS)
            {
                // 上に移動のときドロップ先の行位置がヘッダー行では無い場合
                if (r == 0)
                {
                    return;
                }
            }
            else if (n == ROW_PLUS)
            {
                // 後へ移動のとき最下行は不可
                if (r == dg.RowCount - 1)
                {
                    return;
                }
            }

            //string HinCode = dg[colHinCode, r].Value.ToString();

            DataGridViewRow RowlData = (DataGridViewRow)dg.SelectedRows[0];

            // 行をコピーし元の行のセル値をコピーする
            DataGridViewRow copyRow = (DataGridViewRow)RowlData.Clone();

            for (int i = 0; i < RowlData.Cells.Count; i++)
            {
                copyRow.Cells[i].Value = RowlData.Cells[i].Value;
            }

            // 移動先の行に挿入
            dg.Rows.Insert(r + n, copyRow);

            // 元の該当行を削除
            dg.Rows.Remove(RowlData);

            // 移動先をカレントセルとする
            switch (n)
            {
                case ROW_MINUS:
                    dg.CurrentCell = dg[colHinCode, r + n];
                    break;
                case ROW_PLUS:
                    dg.CurrentCell = dg[colHinCode, r + 1];
                    break;
            }
        }

        private void dataGridView3_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                // 納価、売価、リード日数：数字のみ入力可能とする
                if (dataGridView3.CurrentCell.ColumnIndex == 6 || dataGridView3.CurrentCell.ColumnIndex == 7 ||
                    dataGridView3.CurrentCell.ColumnIndex == 9)
                {
                    //イベントハンドラが複数回追加されてしまうので最初に削除する
                    e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);

                    //イベントハンドラを追加する
                    e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);
                }
            }
        }

        ///-----------------------------------------------------------
        /// <summary>
        ///     発注書商品のナンバーを振る </summary>
        /// <param name="view">
        ///     DataGridViewオブジェクト</param>
        ///-----------------------------------------------------------
        private void SetGridSeqNumber(DataGridView view)
        {
            for (int i = 0; i < view.Rows.Count; i++)
            {
                view[colSeqNum, i].Value = i + 1;

                dataGridView3.Rows[i].DefaultCellStyle.ForeColor = SystemColors.ControlText;

                if (fMode == EDITMODE)
                {
                    // 16行目以降
                    if ((i + 1) > global.MAX_GYO)
                    {
                        dataGridView3.Rows[i].DefaultCellStyle.ForeColor = Color.LightGray;
                    }
                }
            }
        }

        private void ToolStripMenuItem_ReadDays_Click(object sender, EventArgs e)
        {
            frmReadDays frm = new frmReadDays();
            frm.ShowDialog();

            if (frm.MyStatus)
            {
                if (dataGridView3.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow item in dataGridView3.SelectedRows)
                    {
                        item.Cells[colReadDays].Value = frm.MyProperty;
                    }

                    dataGridView3.CurrentCell = null;
                }
            }

            // 後片付け
            frm.Dispose();
        }

        private void dataGridView3_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView3_Leave(object sender, EventArgs e)
        {
            dataGridView3.CurrentCell = null;
        }

        private void frmPtnAdd_Shown(object sender, EventArgs e)
        {
        }

        private void cmbSyohin_L_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbSyohin_L.SelectedIndex < 0)
            {
                return;
            }

            if (!valueChangeStatus)
            {
                return;
            }

            valueChangeStatus = false;

            // 中分類コンボ初期化
            ClsCombos_M = null;
            cmbSyohin_M.DataSource = null;
            cmbSyohin_M.SelectedIndex = -1;
            cmbSyohin_M.Text = "";

            // 小分類コンボ初期化
            ClsCombos_S = null;
            cmbSyohin_S.DataSource = null;
            cmbSyohin_S.SelectedIndex = -1;
            cmbSyohin_S.Text = "";

            string Cmb_L = cmbSyohin_L.SelectedValue.ToString();

            // 商品中分類リスト読み込み
            if (System.IO.File.Exists(Properties.Settings.Default.商品分類リスト))
            {
                using (IXLWorkbook bk = new XLWorkbook(Properties.Settings.Default.商品分類リスト, XLEventTracking.Disabled))
                {
                    var sheet1 = bk.Worksheet(1);
                    var tbl = sheet1.RangeUsed().AsTable();

                    int Cnt = 0;
                    string wCD = "";

                    foreach (var t in tbl.DataRange.Rows())
                    {
                        if (t.RowNumber() < 3)
                        {
                            continue;
                        }

                        if (Utility.NulltoStr(t.Cell(3).Value) == string.Empty)
                        {
                            // 中分類コード空白はネグる
                            continue;
                        }
                                               
                        string BunruiCD_L = Utility.NulltoStr(t.Cell(1).Value).PadLeft(2, '0'); // 大分類

                        if (Cmb_L == BunruiCD_L)
                        {
                            string BunruiCD_M = Utility.NulltoStr(t.Cell(3).Value).PadLeft(2, '0');

                            if (wCD != BunruiCD_M)
                            {
                                Array.Resize(ref ClsCombos_M, Cnt + 1);
                                ClsCombos_M[Cnt] = new ClsMyComboBox();
                                ClsCombos_M[Cnt].MyID = BunruiCD_M;
                                ClsCombos_M[Cnt].MyName = Utility.NulltoStr(t.Cell(4).Value);

                                wCD = BunruiCD_M;
                                Cnt++;
                            }
                        }
                    }
                }

                //表示される値はNAMEプロパティ
                cmbSyohin_M.DisplayMember = "MyName";

                //対応する値はIDプロパティ
                cmbSyohin_M.ValueMember = "MyID";

                //DataSourceに上で作成した配列をセット
                cmbSyohin_M.DataSource = ClsCombos_M;

                // 非選択状態とする
                cmbSyohin_M.SelectedIndex = -1;
            }

            valueChangeStatus = true;
        }

        private void cmbSyohin_M_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbSyohin_M.SelectedIndex < 0)
            {
                return;
            }

            if (!valueChangeStatus)
            {
                return;
            }

            //valueChangeStatus = false;

            // 小分類初期化
            ClsCombos_S = null;
            cmbSyohin_S.DataSource = null;
            cmbSyohin_S.SelectedIndex = -1;
            cmbSyohin_S.Text = "";

            string Cmb_L = cmbSyohin_L.SelectedValue.ToString();     // 大分類コンボ選択値
            string Cmb_M = cmbSyohin_M.SelectedValue.ToString();     // 中分類コンボ選択値

            // 商品小分類リスト読み込み
            if (System.IO.File.Exists(Properties.Settings.Default.商品分類リスト))
            {
                using (IXLWorkbook bk = new XLWorkbook(Properties.Settings.Default.商品分類リスト, XLEventTracking.Disabled))
                {
                    var sheet1 = bk.Worksheet(1);
                    var tbl = sheet1.RangeUsed().AsTable();

                    int Cnt = 0;
                    string wCD = "";

                    foreach (var t in tbl.DataRange.Rows())
                    {
                        if (t.RowNumber() < 3)
                        {
                            continue;
                        }
                        
                        if (Utility.NulltoStr(t.Cell(5).Value) == string.Empty)
                        {
                            // 小分類コード空白はネグる
                            continue;
                        }

                        string BunruiCD_L = Utility.NulltoStr(t.Cell(1).Value).PadLeft(2, '0'); // 大分類
                        string BunruiCD_M = Utility.NulltoStr(t.Cell(3).Value).PadLeft(2, '0'); // 中分類

                        if (Cmb_L == BunruiCD_L && Cmb_M == BunruiCD_M)
                        {
                            string BunruiCD_S = Utility.NulltoStr(t.Cell(5).Value).PadLeft(2, '0');

                            Array.Resize(ref ClsCombos_S, Cnt + 1);
                            ClsCombos_S[Cnt] = new ClsMyComboBox();
                            ClsCombos_S[Cnt].MyID = BunruiCD_S;
                            ClsCombos_S[Cnt].MyName = Utility.NulltoStr(t.Cell(6).Value);

                            Cnt++;
                        }
                    }
                }

                //表示される値はNAMEプロパティ
                cmbSyohin_S.DisplayMember = "MyName";

                //対応する値はIDプロパティ
                cmbSyohin_S.ValueMember = "MyID";

                //DataSourceに上で作成した配列をセット
                cmbSyohin_S.DataSource = ClsCombos_S;

                // 非選択状態とする
                cmbSyohin_S.SelectedIndex = -1;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //ArrayToCSV();
        }

        //private void ArrayToCSV()
        //{
        //    csvArray = new string[global.syohin_News.Length + 1];
        //    StringBuilder sb = new StringBuilder();
        //    csvArray[0] = "SYOHIN_CD,SYOHIN_NM,SYOHIN_SNM,SIRESAKI_CD,SIRESAKI_NM,SIRESAKI_KANA_NM,JAN_CD,SYOHIN_KIKAKU,CASE_IRISU,NOUHIN_KARI_TANKA,RETAIL_TANKA,HATYU_LIMIT_DAY_CNT,START_SALE_YMD,LAST_SALE_YMD,SHUBAI,SYOHIN_KIND_L_CD,SYOHIN_KIND_M_CD,SYOHIN_KIND_S_CD,SYOHIN_KIND_CD";

        //    for (int i = 0; i < global.syohin_News.Length; i++)
        //    {
        //        sb.Clear();
        //        sb.Append(global.syohin_News[i].SYOHIN_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_NM).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_SNM).Append(",");
        //        sb.Append(global.syohin_News[i].SIRESAKI_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SIRESAKI_NM).Append(",");
        //        sb.Append(global.syohin_News[i].SIRESAKI_KANA_NM).Append(",");
        //        sb.Append(global.syohin_News[i].JAN_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_KIKAKU).Append(",");
        //        sb.Append(global.syohin_News[i].CASE_IRISU).Append(",");
        //        sb.Append(global.syohin_News[i].NOUHIN_KARI_TANKA).Append(",");
        //        sb.Append(global.syohin_News[i].RETAIL_TANKA).Append(",");
        //        sb.Append(global.syohin_News[i].HATYU_LIMIT_DAY_CNT).Append(",");
        //        sb.Append(global.syohin_News[i].START_SALE_YMD).Append(",");
        //        sb.Append(global.syohin_News[i].LAST_SALE_YMD).Append(",");

        //        if (global.syohin_News[i].SHUBAI)
        //        {
        //            sb.Append("1,");
        //        }
        //        else
        //        {
        //            sb.Append("0,");
        //        }

        //        sb.Append(global.syohin_News[i].SYOHIN_KIND_L_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_KIND_M_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_KIND_S_CD).Append(",");
        //        sb.Append(global.syohin_News[i].SYOHIN_KIND_CD);

        //        csvArray[i + 1] = sb.ToString();
        //    }

        //    // 上書き
        //    System.IO.File.WriteAllLines(@"C:\STSH_OCR\TESTMST\syohinmst_New.csv", csvArray, System.Text.Encoding.GetEncoding(932));

        //    MessageBox.Show("Finish!");
        //}

        ///----------------------------------------------------------------------------------
        /// <summary>
        ///     商品の大分類、中分類名称を取得する </summary>
        /// <param name="L_Code">
        ///     大分類コード </param>
        /// <param name="M_Code">
        ///     中分類コード</param>
        /// <returns>
        ///     文字列 "コード+大分類名,コード＋中分類名"</returns>
        ///----------------------------------------------------------------------------------
        private string GetSyohin_LM_Name(string L_Code, string M_Code)
        {
            string RtnName = "";

            L_Code = L_Code.PadLeft(2, '0');
            M_Code = M_Code.PadLeft(2, '0');

            // 商品分類リスト読み込み
            if (System.IO.File.Exists(Properties.Settings.Default.商品分類リスト))
            {
                using (IXLWorkbook bk = new XLWorkbook(Properties.Settings.Default.商品分類リスト, XLEventTracking.Disabled))
                {
                    var sheet1 = bk.Worksheet(1);
                    var tbl = sheet1.RangeUsed().AsTable();

                    int Cnt = 0;
                    foreach (var t in tbl.DataRange.Rows())
                    {
                        if (t.RowNumber() < 3)
                        {
                            continue;
                        }

                        if (Utility.NulltoStr(t.Cell(3).Value) == string.Empty)
                        {
                            // 中分類コード空白はネグる
                            continue;
                        }

                        string BunruiCD_L = Utility.NulltoStr(t.Cell(1).Value).PadLeft(2, '0'); // 大分類

                        if (L_Code == BunruiCD_L)
                        {
                            string BunruiCD_M = Utility.NulltoStr(t.Cell(3).Value).PadLeft(2, '0');

                            if (M_Code == BunruiCD_M)
                            {
                                RtnName = BunruiCD_L + BunruiCD_M +" " + Utility.NulltoStr(t.Cell(2).Value) + "・" + Utility.NulltoStr(t.Cell(4).Value);
                                break;
                            }
                        }
                    }
                }
            }

            return RtnName;
        }

        public class CustomComparer : System.Collections.IComparer
        {
            private int sortOrder;
            private System.Collections.Comparer comparer;

            public CustomComparer(SortOrder order)
            {
                this.sortOrder = (order == SortOrder.Descending ? -1 : 1);
                this.comparer = new System.Collections.Comparer(System.Globalization.CultureInfo.CurrentCulture);
            }

            //並び替え方を定義する
            public int Compare(object x, object y)
            {
                int result = 0;

                DataGridViewRow rowx = (DataGridViewRow)x;
                DataGridViewRow rowy = (DataGridViewRow)y;

                //はじめの列のセルの値を比較し、同じならば次の列を比較する
                //for (int i = 0; i < rowx.Cells.Count; i++)
                //{
                //    result = this.comparer.Compare(rowx.Cells[i].Value, rowy.Cells[i].Value);

                //    if (result != 0)
                //    {
                //        break;
                //    }
                //}

                result = this.comparer.Compare(rowx.Cells[8].Value, rowy.Cells[8].Value);

                if (result == 0)
                {
                    result = this.comparer.Compare(rowx.Cells[2].Value, rowy.Cells[2].Value);
                }

                //結果を返す
                return result * this.sortOrder;
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            dataGridView3.Sort(new CustomComparer(SortOrder.Ascending));
        }

        private void 分類商品コードで並び替えToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 並び替え：2020/06/23
            dataGridView3.Sort(new CustomComparer(SortOrder.Ascending));

            // ナンバーリング：2020/06/23
            SetGridSeqNumber(dataGridView3);
        }
    }
}
