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
using System.Configuration;
//using Oracle.ManagedDataAccess.Client;
using Excel = Microsoft.Office.Interop.Excel;
using STSH_OCR.Common;

namespace STSH_OCR.Pattern
{
    public partial class frmPrnPtn : Form
    {
        public frmPrnPtn()
        {
            InitializeComponent();
        }
        // 2020/04/08
        ClsCsvData.ClsCsvSyohin_New csvSyohin = null;

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // 発注書パターンマスター
        Table<Common.ClsOrderPattern> dbPtn = null;
        ClsOrderPattern ClsOrderPattern = null;

        // カラム定義
        private string colNouCode = "c0";
        private string colNouName = "c1";
        private string colTel = "c2";
        private string colZip = "c3";
        private string colAddress = "c4";
        private string colPtnID = "c5";
        private string colDate = "c6";
        private string colID = "c7";
        private string colMemo = "c8";
        private string colChk = "c9";
        private string colSecoundNum = "c10";

        ///----------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義を行います </summary>
        ///----------------------------------------------------------------
        private void GridviewSet(DataGridView tempDGV)
        {
            Cursor = Cursors.WaitCursor;

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
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 10, FontStyle.Regular);
                //tempDGV.DefaultCellStyle.Font = new Font("游ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 582;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.Name = colChk;
                tempDGV.Columns.Add(chk);
                tempDGV.Columns[colChk].HeaderText = "";

                tempDGV.Columns.Add(colNouCode, "コード");
                tempDGV.Columns.Add(colNouName, "得意先名");
                tempDGV.Columns.Add(colPtnID, "PID");
                tempDGV.Columns.Add(colSecoundNum, "Page");
                tempDGV.Columns.Add(colMemo, "備考");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");
                tempDGV.Columns.Add(colDate, "登録日");
                tempDGV.Columns.Add(colID, "ID");

                tempDGV.Columns[colChk].Width = 30;
                tempDGV.Columns[colNouCode].Width = 80;
                tempDGV.Columns[colNouName].Width = 300;
                tempDGV.Columns[colPtnID].Width = 50;
                tempDGV.Columns[colSecoundNum].Width = 50;
                tempDGV.Columns[colMemo].Width = 160;
                tempDGV.Columns[colTel].Width = 100;
                tempDGV.Columns[colDate].Width = 150;
                //tempDGV.Columns[colAddress].Width = 200;

                tempDGV.Columns[colAddress].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colNouCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colPtnID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colSecoundNum].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colAddress].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                tempDGV.Columns[colID].Visible = false;

                // 編集可否
                tempDGV.ReadOnly = false;

                foreach (DataGridViewColumn item in tempDGV.Columns)
                {
                    if (item.Name == colChk)
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
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        ///------------------------------------------------------------
        /// <summary>
        ///     発注書パターン一覧表示 </summary>
        /// <param name="g">
        ///     DataGridView</param>
        ///------------------------------------------------------------
        private void showPattern(DataGridView g)
        {
            this.Cursor = Cursors.WaitCursor;

            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            dbPtn = context.GetTable<Common.ClsOrderPattern>();

            g.Rows.Clear();

            int cnt = 0;

            foreach (var s in dbPtn.OrderBy(a => a.TokuisakiCode).ThenBy(a => a.SeqNum).ThenBy(a => a.SecondNum))
            {
                // 得意先情報を取得：2020/04/09
                ClsCsvData.ClsCsvTokuisaki tokuisaki = Utility.GetTokuisakiFromDataTable(s.TokuisakiCode.ToString().PadLeft(7, '0'), global.dtTokuisaki);

                if (tokuisaki.TOKUISAKI_CD == string.Empty)
                {
                    continue;
                }

                // 検索得意先コード
                if (sCode.Text != string.Empty)
                {
                    if (!tokuisaki.TOKUISAKI_CD.Contains(sCode.Text))
                    {
                        continue;
                    }
                }

                // 検索電話番号
                if (sTel.Text != string.Empty)
                {
                    if (!tokuisaki.TOKUISAKI_TEL.Contains(sTel.Text))
                    {
                        continue;
                    }
                }

                // 検索得意先名称
                if (sName.Text != string.Empty)
                {
                    if (!tokuisaki.TOKUISAKI_NM.Contains(sName.Text))
                    {
                        continue;
                    }
                }

                // 検索住所
                if (sAddress.Text != string.Empty)
                {
                    if (!tokuisaki.TOKUISAKI_ZYUSYO1.Contains(sAddress.Text) &&
                        !tokuisaki.TOKUISAKI_ZYUSYO2.Contains(sAddress.Text))
                    {
                        continue;
                    }
                }
                
                g.Rows.Add();
                g[colChk, cnt].Value = true;
                g[colNouCode, cnt].Value = s.TokuisakiCode.ToString().Trim().PadLeft(7, '0');
                g[colNouName, cnt].Value = tokuisaki.TOKUISAKI_NM;
                g[colPtnID, cnt].Value = s.SeqNum.ToString().PadLeft(3, '0');
                g[colSecoundNum, cnt].Value = s.SecondNum.ToString();
                //g[colMemo, cnt].Value = t.備考;
                g[colTel, cnt].Value = tokuisaki.TOKUISAKI_TEL;
                g[colAddress, cnt].Value = tokuisaki.TOKUISAKI_ZYUSYO1 + " " + tokuisaki.TOKUISAKI_ZYUSYO2;
                g[colDate, cnt].Value = s.YyMmDd;
                g[colID, cnt].Value = s.ID.ToString();

                cnt++;
                g.CurrentCell = null;
            }

            this.Cursor = Cursors.Default;

            if (cnt == 0)
            {
                MessageBox.Show("該当する得意先はありませんでした", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);

                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
            else 
            { 
                button3.Enabled = true; 
                button4.Enabled = true;
                button5.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showPattern(dataGridView1);
        }

        private void frmTodoke_Load(object sender, EventArgs e)
        {
            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            GridviewSet(dataGridView1);

            ptnID = string.Empty;

            // 2020/04/08 コメント化
            //// 納品先マスター読み込み
            //getVNouhin();

            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;

            radioButton1.Checked = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.SelectedRows.Count == 0)
            //{
            //    return;
            //}

            //int r = dataGridView1.SelectedRows[0].Index;

            //_nouCode = dataGridView1[colNouCode, r].Value.ToString();

            //Close();
        }

        public string ptnID;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int pCnt = 0;

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                // チェックされている注文書を対象とする
                if (dataGridView1[colChk, r.Index].Value.ToString() == "True")
                {
                    pCnt++;
                }
            }

            if (pCnt == 0)
            {
                MessageBox.Show("印刷する発注書を選択してください", "印刷対象", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtYear.Text == string.Empty)
            {
                MessageBox.Show("対象年を入力してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtYear.Focus();
                return;
            }

            if (Utility.StrtoInt(txtYear.Text) < 2020)
            {
                MessageBox.Show("適切な対象年を入力してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtYear.Focus();
                return;
            }

            if (txtMonth.Text == string.Empty)
            {
                MessageBox.Show("対象月を入力してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMonth.Focus();
                return;
            }

            if (Utility.StrtoInt(txtMonth.Text) < 1 || Utility.StrtoInt(txtMonth.Text) > 12)
            {
                MessageBox.Show("対象月を正しく入力してください", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtMonth.Focus();
                return;
            }

            // 印刷確認 : 2020/04/14
            string msg = "";
            if (radioButton1.Checked)
            {
                msg = "印刷";
            }
            else
            {
                msg = "Excel出力";
            }

            if (MessageBox.Show(pCnt + "件の発注書を" + msg + "します。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // 印刷時のプリンター設定 : 2020/04/14
            if (radioButton1.Checked)
            {
                PrintDialog pd = new PrintDialog();
                pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string printerName = pd.PrinterSettings.PrinterName; // プリンター名
                    int copies = pd.PrinterSettings.Copies; // 印刷部数
                    bool ptof = pd.PrinterSettings.PrintToFile; // printToFile

                    // ＦＡＸ注文書印刷
                    prnSheet(printerName, copies, ptof);
                }
            }
            else
            {
                // ＦＡＸ注文書Excel出力
                prnSheet("", 0, false);
            }

        }
                
        private class clsVNouhin
        {
            public string KOK_ID { get; set; }
            public string NOU_NAME { get; set; }
            public string NOU_TEL { get; set; }
            public string NOU_JYU { get; set; }
        }

        private class clsVSYOHIN
        {
            public string SYO_ID { get; set; }
            public string SYO_NAME { get; set; }
            public string SYO_IRI_KESU { get; set; }
            public string SYO_TANI { get; set; }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての発注書を印刷対象とします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[colChk, i].Value = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("全ての発注書を印刷対象外とします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[colChk, i].Value = false;
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            string colName = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex].Name;
            if (colName == colChk)
            {
                if (dataGridView1.IsCurrentCellDirty)
                {
                    //コミットする
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                    dataGridView1.RefreshEdit();
                }
            }
        }


        ///----------------------------------------------------------------------
        /// <summary>
        ///     ＦＡＸ注文書印刷処理 </summary>
        /// <param name="prnName">
        ///     プリンタ名</param>
        /// <param name="copies">
        ///     印刷部数</param>
        /// <param name="ptof">
        ///     ファイルに出力</param>
        ///----------------------------------------------------------------------
        private void prnSheet(string prnName, int copies, bool ptof)
        {
            //マウスポインタを待機にする
            this.Cursor = Cursors.WaitCursor;

            // Excel起動
            string sAppPath = System.AppDomain.CurrentDomain.BaseDirectory;

            Excel.Application oXls = new Excel.Application();

            Excel.Workbook oXlsBook = (Excel.Workbook)(oXls.Workbooks.Open(Properties.Settings.Default.FAX注文書, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                               Type.Missing, Type.Missing));

            Excel.Worksheet oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[1];
            Excel.Worksheet oxlsMsSheet = (Excel.Worksheet)oXlsBook.Sheets[1]; // テンプレートシート
            oxlsSheet.Select(Type.Missing);

            Excel.Range rng = null;
            Excel.Range rngFormura = null;

            int pCnt = 1;   // ページカウント
            object[,] rtnArray = null;

            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    // チェックされている行を対象とする
                    if (dataGridView1[colChk, i].Value.ToString() == "False")
                    {
                        continue;
                    }

                    // テンプレートシートを追加する
                    pCnt++;
                    oxlsMsSheet.Copy(Type.Missing, oXlsBook.Sheets[pCnt - 1]);
                    oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[pCnt];

                    // シートのセルを一括して配列に取得します
                    rng = oxlsMsSheet.Range[oxlsMsSheet.Cells[1, 1], oxlsMsSheet.Cells[oxlsMsSheet.UsedRange.Rows.Count, oxlsMsSheet.UsedRange.Columns.Count]];
                    //rng.Value2 = "";
                    //rtnArray = (object[,])rng.Value2;
                    rtnArray = rng.Value2;
                    rtnArray = rng.Formula;

                    // 年月
                    rtnArray[1, 1] = txtYear.Text.Substring(0, 1);
                    rtnArray[1, 7] = txtYear.Text.Substring(1, 1);
                    rtnArray[1, 13] = txtYear.Text.Substring(2, 1);
                    rtnArray[1, 19] = txtYear.Text.Substring(3, 1);

                    rtnArray[1, 31] = txtMonth.Text.PadLeft(2, '0').Substring(0, 1);
                    rtnArray[1, 37] = txtMonth.Text.PadLeft(2, '0').Substring(1, 1);

                    // パターンＩＤ
                    string pPID = dataGridView1[colPtnID, i].Value.ToString().PadLeft(3, '0');
                    rtnArray[1, 79] = pPID.Substring(0, 1);
                    rtnArray[1, 85] = pPID.Substring(1, 1);
                    rtnArray[1, 91] = pPID.Substring(2, 1);

                    // 枝番
                    pPID = dataGridView1[colSecoundNum, i].Value.ToString().PadLeft(2, '0');
                    rtnArray[3, 121] = pPID.Substring(0, 1);
                    rtnArray[3, 127] = pPID.Substring(1, 1);

                    // 得意先コード
                    pPID = dataGridView1[colNouCode, i].Value.ToString().PadLeft(7, '0');
                    rtnArray[1, 121] = pPID.Substring(0, 1);
                    rtnArray[1, 127] = pPID.Substring(1, 1);
                    rtnArray[1, 133] = pPID.Substring(2, 1);
                    rtnArray[1, 139] = pPID.Substring(3, 1);
                    rtnArray[1, 145] = pPID.Substring(4, 1);
                    rtnArray[1, 151] = pPID.Substring(5, 1);
                    rtnArray[1, 157] = pPID.Substring(6, 1);

                    // 得意先名
                    rtnArray[3, 31] = Utility.NulltoStr(dataGridView1[colNouName, i].Value);

                    //// 更新日：2018/11/01
                    //DateTime uDt;
                    //if (DateTime.TryParse(dataGridView1[colDate, i].Value.ToString(), out uDt))
                    //{
                    //    rtnArray[15, 2] = uDt.ToShortDateString() + " 更新";
                    //}
                    //else
                    //{
                    //    rtnArray[15, 2] = string.Empty;
                    //}


                    ClsOrderPattern = dbPtn.Single(a => a.ID == Utility.StrtoInt(dataGridView1[colID, i].Value.ToString()));

                    // コメント１：2020/04/01
                    if (Utility.NulltoStr(ClsOrderPattern.comment1) != string.Empty)
                    {
                        rtnArray[5, 1] = Utility.NulltoStr(ClsOrderPattern.comment1);
                    }

                    // 会社名：2020/04/01
                    if (Utility.NulltoStr(ClsOrderPattern.comment2) != string.Empty)
                    {
                        rtnArray[3, 133] = Utility.NulltoStr(ClsOrderPattern.comment2);
                    }

                    // 担当者：2020/04/01
                    if (Utility.NulltoStr(ClsOrderPattern.comment3) != string.Empty)
                    {
                        rtnArray[4, 133] = Utility.NulltoStr(ClsOrderPattern.comment3);
                    }
                    
                    // FAX：2020/04/01
                    if (Utility.NulltoStr(ClsOrderPattern.comment4) != string.Empty)
                    {
                        rtnArray[5, 133] = Utility.NulltoStr(ClsOrderPattern.comment4);
                    }

                    string sIrisu = string.Empty;
                    string sTani = string.Empty;

                    string _G_Code = "";
                    int _R_Days = 0;
                    int xR = 9;

                    for (int r = 0; r < global.MAX_GYO; r++)
                    {
                        switch (r)
                        {
                            case 0:
                                _G_Code = ClsOrderPattern.G_Code1;
                                _R_Days = ClsOrderPattern.G_Read1;
                                break;

                            case 1:
                                _G_Code = ClsOrderPattern.G_Code2;
                                _R_Days = ClsOrderPattern.G_Read2;
                                break;

                            case 2:
                                _G_Code = ClsOrderPattern.G_Code3;
                                _R_Days = ClsOrderPattern.G_Read3;
                                break;
                                
                            case 3:
                                _G_Code = ClsOrderPattern.G_Code4;
                                _R_Days = ClsOrderPattern.G_Read4;
                                break;

                            case 4:
                                _G_Code = ClsOrderPattern.G_Code5;
                                _R_Days = ClsOrderPattern.G_Read5;
                                break;

                            case 5:
                                _G_Code = ClsOrderPattern.G_Code6;
                                _R_Days = ClsOrderPattern.G_Read6;
                                break;

                            case 6:
                                _G_Code = ClsOrderPattern.G_Code7;
                                _R_Days = ClsOrderPattern.G_Read7;
                                break;

                            case 7:
                                _G_Code = ClsOrderPattern.G_Code8;
                                _R_Days = ClsOrderPattern.G_Read8;
                                break;

                            case 8:
                                _G_Code = ClsOrderPattern.G_Code9;
                                _R_Days = ClsOrderPattern.G_Read9;
                                break;

                            case 9:
                                _G_Code = ClsOrderPattern.G_Code10;
                                _R_Days = ClsOrderPattern.G_Read10;
                                break;

                            case 10:
                                _G_Code = ClsOrderPattern.G_Code11;
                                _R_Days = ClsOrderPattern.G_Read11;
                                break;

                            case 11:
                                _G_Code = ClsOrderPattern.G_Code12;
                                _R_Days = ClsOrderPattern.G_Read12;
                                break;

                            case 12:
                                _G_Code = ClsOrderPattern.G_Code13;
                                _R_Days = ClsOrderPattern.G_Read13;
                                break;

                            case 13:
                                _G_Code = ClsOrderPattern.G_Code14;
                                _R_Days = ClsOrderPattern.G_Read14;
                                break;

                            case 14:
                                _G_Code = ClsOrderPattern.G_Code15;
                                _R_Days = ClsOrderPattern.G_Read15;
                                break;

                            default:
                                break;
                        }

                        // 2020/04/08 コメント化
                        //csvSyohin = Utility.GetSyohinData(SyoArray, SySzArray, ShiireArray, _G_Code);

                        // 商品情報取得 2020/04/09
                        csvSyohin = Utility.GetSyohinsFromDataTable(global.dtSyohin, _G_Code);

                        int xRow = r * 2 + xR;
                        int xRow2 = r * 2 + xR + 1;

                        if (csvSyohin.SYOHIN_CD == "")
                        {
                            rtnArray[xRow, 1] = string.Empty;
                            rtnArray[xRow2, 1] = string.Empty;
                            rtnArray[xRow, 23] = string.Empty;
                            rtnArray[xRow, 44] = string.Empty;
                            rtnArray[xRow, 48] = string.Empty;
                            rtnArray[xRow2, 48] = string.Empty;
                            rtnArray[xRow, 60] = string.Empty;
                            rtnArray[xRow, 74] = string.Empty;
                            rtnArray[xRow2, 60] = string.Empty;
                        }
                        else
                        {
                            rtnArray[xRow, 1] = csvSyohin.SIRESAKI_NM;
                            rtnArray[xRow2, 1] = csvSyohin.SYOHIN_NM;
                            rtnArray[xRow, 23] = csvSyohin.SYOHIN_KIKAKU;
                            rtnArray[xRow, 44] = csvSyohin.CASE_IRISU;
                            rtnArray[xRow, 48] = csvSyohin.SYOHIN_CD.PadLeft(8, '0');
                            rtnArray[xRow2, 48] = _R_Days + "日前";

                            // 納価売価取得：2020/04/10
                            ClsCsvData.ClsCsvNoukaBaika noukaBaika = Utility.GetNoukaBaikaFromDataTable(pPID, csvSyohin.SYOHIN_CD.PadLeft(8, '0'), global.dtNoukaBaika);
                            rtnArray[xRow, 60] = noukaBaika.NOUKA;  // 2020/04/10
                            rtnArray[xRow, 74] = noukaBaika.BAIKA;  // 2020/04/10

                            rtnArray[xRow2, 60] = csvSyohin.JAN_CD;
                        }
                    }

                    // 備考
                    //rtnArray[62, 2] = s.備考; 
                    
                    // 配列からシートセルに一括してデータをセットします
                    rng = oxlsSheet.Range[oxlsSheet.Cells[1, 1], oxlsSheet.Cells[oxlsSheet.UsedRange.Rows.Count, oxlsSheet.UsedRange.Columns.Count]];
                    rng.Value2 = rtnArray;
                }

                // 確認のためExcelのウィンドウを表示する
                oXls.Visible = true;

                // 1枚目はテンプレートシートなので印刷時には削除する
                oXls.DisplayAlerts = false;
                oXlsBook.Sheets[1].Delete();

                // 印刷：2020/04/14
                if (radioButton1.Checked)
                {
                    // 印刷
                    oXlsBook.PrintOutEx(Type.Missing, Type.Missing, copies, true, prnName, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                // 確認のためExcelのウィンドウを非表示にする
                oXls.Visible = false;

                // Excel出力：2020/04/14
                if (radioButton2.Checked)
                {
                    //ダイアログボックスの初期設定
                    DialogResult ret;
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Title = "FAX発注書";
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = "FAX発注書";
                    saveFileDialog1.Filter = "Microsoft Office Excelファイル(*.xlsx)|*.xlsx|全てのファイル(*.*)|*.*";

                    //ダイアログボックスを表示し「保存」ボタンが選択されたらファイル名を表示
                    string fileName;
                    ret = saveFileDialog1.ShowDialog();

                    if (ret == System.Windows.Forms.DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                        oXlsBook.SaveAs(fileName, Type.Missing, Type.Missing,
                                        Type.Missing, Type.Missing, Type.Missing,
                                        Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing,
                                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    }
                }

                // 終了メッセージ 
                MessageBox.Show("終了しました");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "出力処理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            finally
            {
                // ウィンドウを非表示にする
                oXls.Visible = false;

                // 保存処理
                oXls.DisplayAlerts = false;

                // Bookをクローズ
                oXlsBook.Close(Type.Missing, Type.Missing, Type.Missing);

                // Excelを終了
                oXls.Quit();

                // COM オブジェクトの参照カウントを解放する 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsMsSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXlsBook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oXls);

                oXls = null;
                oXlsBook = null;
                oxlsSheet = null;
                oxlsMsSheet = null;

                GC.Collect();

                //マウスポインタを元に戻す
                this.Cursor = Cursors.Default;
            }
        }
        
        private void sCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
            {
                e.Handled = true;
            }
        }
    }
}
