using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using STSH_OCR.Common;

namespace STSH_OCR.OCR
{
    public partial class frmEditLogRep : Form
    {
        public frmEditLogRep()
        {
            InitializeComponent();
        }

        private void frmEditLogRep_Load(object sender, EventArgs e)
        {
            //ウィンドウズ最小サイズ
            Utility.WindowsMinSize(this, this.Size.Width, this.Size.Height);

            //ウィンドウズ最大サイズ
            //Utility.WindowsMaxSize(this, this.Size.Width, this.Size.Height);

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            // 共有DB接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            tblEditLog = context.GetTable<Common.ClsDataEditLog>(); // 編集ログテーブル

            // PC名コンボボックスアイテム追加
            foreach (var t in tblEditLog.Select(a => a.ComputerName).Distinct())
            {
                comboBox1.Items.Add(t);
            }

            GridViewSetting(dataGridView1);

            button1.Enabled = false;
        }

        string colStaffCode = "c1";
        string colPcName = "c2";
        string colDate = "c3";
        string colEditDate = "c4";
        string colField = "c5";
        string colBefore = "c6";
        string colAfter = "c7";
        string ColSyohinNM = "c8";
        string colTokuisakiCD = "c9";
        string colTokuisakiNM = "c10";
        string colPatternID = "c11";
        string colYear = "c12";
        string colMonth = "c13";
        string colDay = "c14";
        string colID = "c15";

        // データベース：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        string db_file = Properties.Settings.Default.DB_File;
        
        // 編集ログデータ
        Table<Common.ClsDataEditLog> tblEditLog = null;
        ClsDataEditLog ClsDataEditLog = null;

        public void GridViewSetting(DataGridView tempDGV)
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
            tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", (float)10.5, FontStyle.Regular);

            // 行の高さ
            tempDGV.ColumnHeadersHeight = 20;
            tempDGV.RowTemplate.Height = 20;
                       
            // 全体の高さ
            tempDGV.Height = 722;            

            // 奇数行の色
            tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

            // 各列幅指定
            tempDGV.Columns.Add(colEditDate, "編集日時");
            tempDGV.Columns.Add(colPcName, "ＰＣ名");
            //tempDGV.Columns.Add(colTokuisakiCD, "得意先コード");
            tempDGV.Columns.Add(colTokuisakiNM, "得意先名");
            tempDGV.Columns.Add(colPatternID, "PID");
            tempDGV.Columns.Add(colYear, "発注年月");
            tempDGV.Columns.Add(ColSyohinNM, "商品名");
            //tempDGV.Columns.Add(colDay, "着日");
            tempDGV.Columns.Add(colField, "編集項目");
            tempDGV.Columns.Add(colBefore, "編集前");
            tempDGV.Columns.Add(colAfter, "編集後");
            tempDGV.Columns.Add(colID, "発注書ID");

            tempDGV.Columns[colEditDate].Width = 160;
            tempDGV.Columns[colPcName].Width = 140;
            //tempDGV.Columns[colTokuisakiCD].Width = 120;
            tempDGV.Columns[colTokuisakiNM].Width = 300;
            tempDGV.Columns[colPatternID].Width = 50;
            tempDGV.Columns[colYear].Width = 76;
            //tempDGV.Columns[colMonth].Width = 40;
            tempDGV.Columns[ColSyohinNM].Width = 360;
            //tempDGV.Columns[colDay].Width = 56;
            tempDGV.Columns[colField].Width = 120;
            tempDGV.Columns[colBefore].Width = 100;
            tempDGV.Columns[colAfter].Width = 100;
            tempDGV.Columns[colID].Width = 100;

            //tempDGV.Columns[colField].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            tempDGV.Columns[colTokuisakiNM].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //tempDGV.Columns[colEditDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //tempDGV.Columns[colTokuisakiCD].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colYear].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //tempDGV.Columns[colMonth].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //tempDGV.Columns[colDay].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colBefore].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colAfter].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;         
            tempDGV.Columns[colID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 編集可否
            tempDGV.ReadOnly = true;

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
            //tempDGV.AllowUserToResizeColumns = false;

            // 行サイズ変更禁止
            tempDGV.AllowUserToResizeRows = false;

            // 罫線
            tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }

        /// ----------------------------------------------------------------------
        /// <summary>
        ///     グリッドビュー表示 </summary>
        /// <param name="tempDGV">
        ///     DataGridViewオブジェクト名</param>
        /// <param name="sCode">
        ///     指定所属コード</param>
        /// ----------------------------------------------------------------------
        private void GridViewShowData(DataGridView g)
        {
            // カーソル待機中
            this.Cursor = Cursors.WaitCursor;

            // データグリッド行クリア
            g.Rows.Clear();

            try
            {
                // 開始日付
                DateTime sdt = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day, 0, 0, 0);
                string _sdt = sdt.Year + "/" + sdt.Month.ToString("D2") + "/" + sdt.Day.ToString("D2") + " " +
                              sdt.Hour.ToString("D2") + ":" + sdt.Minute.ToString("D2") + ":" + sdt.Second.ToString("D2") + "','";

                // 終了日付
                DateTime edt = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, 23, 59, 59);
                string _edt = edt.Year + "/" + edt.Month.ToString("D2") + "/" + edt.Day.ToString("D2") + " " +
                              edt.Hour.ToString("D2") + ":" + edt.Minute.ToString("D2") + ":" + edt.Second.ToString("D2") + "','";

                var s = tblEditLog.Where(a => a.Date_Time.CompareTo(_sdt) >= 0 && a.Date_Time.CompareTo(_edt) <= 0).OrderByDescending(a => a.Date_Time);

                // PC指定
                if (comboBox1.SelectedIndex != -1)
                {
                    s = s.Where(a => a.ComputerName.Contains(comboBox1.Text)).OrderByDescending(a => a.Date_Time);
                }

                // 得意先名指定
                if (sTokuisakiNM.Text.Trim() != string.Empty)
                {
                    s = s.Where(a => a.TokuisakiName.Contains(sTokuisakiNM.Text)).OrderByDescending(a => a.Date_Time);
                }

                foreach (var t in s)
                {
                    g.Rows.Add();

                    g[colEditDate, g.Rows.Count - 1].Value = t.Date_Time;
                    g[colPcName, g.Rows.Count - 1].Value = t.ComputerName;
                    //g[colTokuisakiCD, g.Rows.Count - 1].Value = t.TokuisakiCode;
                    g[colTokuisakiNM, g.Rows.Count - 1].Value = t.TokuisakiCode + " " + t.TokuisakiName;
                    g[colPatternID, g.Rows.Count - 1].Value = t.patternID.PadLeft(3, '0') + t.patternIDSeq.PadLeft(2, '0');
                    g[colYear, g.Rows.Count - 1].Value = t.Year + "/" + t.Month.PadLeft(2, '0');
                    //g[colMonth, g.Rows.Count - 1].Value = t.Month;
                    //g[colDay, g.Rows.Count - 1].Value = t.TenchakuDate;
                    g[ColSyohinNM, g.Rows.Count - 1].Value = t.ShohinName;
                    //g[colField, g.Rows.Count - 1].Value = t.FieldName;

                    if (t.TenchakuDate != string.Empty)
                    {
                        g[colField, g.Rows.Count - 1].Value = t.TenchakuDate + t.FieldName;
                    }
                    else
                    {
                        g[colField, g.Rows.Count - 1].Value = t.FieldName;
                    }


                    //if (t.ShohinName != string.Empty)
                    //{
                    //    string str = t.ShohinName + "・";

                    //    if (t.TenchakuDate != string.Empty)
                    //    {
                    //        str += t.TenchakuDate + "日";
                    //    }

                    //    g[colField, g.Rows.Count - 1].Value = str + t.FieldName;
                    //}
                    //else
                    //{
                    //    g[colField, g.Rows.Count - 1].Value = t.FieldName;
                    //}

                    g[colBefore, g.Rows.Count - 1].Value = t.BeforeValue;
                    g[colAfter, g.Rows.Count - 1].Value = t.AfterValue;
                    g[colID, g.Rows.Count - 1].Value = t.ID;
                }

                g.CurrentCell = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButtons.OK);
            }
            finally
            {
                // カーソルを戻す
                this.Cursor = Cursors.Default;
            }

            // 該当するデータがないとき
            if (g.RowCount == 0)
            {
                MessageBox.Show("該当するデータはありませんでした", "発注データ編集ログ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                button1.Enabled = false;
                //lblCnt.Visible = false;
            }
            else
            {
                button1.Enabled = true;
                //lblCnt.Visible = true;
                //lblCnt.Text = g.RowCount.ToString("#,##0") + "件";
            }
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            GridViewShowData(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 閉じる
            Close();
        }

        private void frmEditLogRep_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の編集ログをExcel形式で出力します。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ExcelOutput(dataGridView1);
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     在庫集計表Excel出力 </summary>
        /// <param name="g">
        ///     DataGridViewオブジェクト</param>
        ///----------------------------------------------------------------
        private void ExcelOutput(DataGridView g)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                using (var bk = new XLWorkbook(XLEventTracking.Disabled))
                {
                    // ワークシートを作成
                    bk.Style.Font.FontName = "ＭＳ ゴシック";
                    bk.Style.Font.FontSize = 11;

                    var sheet1 = bk.AddWorksheet("発注書編集ログ");


                    //sheet1.Range("A1:C1").Merge();

                    //string kijun = "出庫基準日：";

                    //if (dateTimePicker1.Checked)
                    //{
                    //    kijun += dateTimePicker1.Value.ToShortDateString() + "～";
                    //}
                    //else
                    //{
                    //    kijun += "全期間";
                    //}

                    //sheet1.Cell("A1").SetValue(kijun);


                    //g.Columns.Add(colEditDate, "編集日時");
                    //g.Columns.Add(colPcName, "ＰＣ名");
                    ////tempDGV.Columns.Add(colTokuisakiCD, "得意先コード");
                    //g.Columns.Add(colTokuisakiNM, "得意先名");
                    //g.Columns.Add(colPatternID, "PID");
                    //g.Columns.Add(colYear, "発注年月");
                    //g.Columns.Add(ColSyohinNM, "商品名");
                    ////tempDGV.Columns.Add(colDay, "着日");
                    //g.Columns.Add(colField, "編集項目");
                    //g.Columns.Add(colBefore, "編集前");
                    //g.Columns.Add(colAfter, "編集後");
                    //g.Columns.Add(colID, "発注書ID");

                    sheet1.Cell("A1").SetValue("編集日時");
                    sheet1.Cell("B1").SetValue("ＰＣ名");
                    sheet1.Cell("C1").SetValue("得意先名");
                    sheet1.Cell("D1").SetValue("PID");
                    sheet1.Cell("E1").SetValue("発注年月");
                    sheet1.Cell("F1").SetValue("商品名");
                    sheet1.Cell("G1").SetValue("編集項目");
                    sheet1.Cell("H1").SetValue("編集前");
                    sheet1.Cell("I1").SetValue("編集後");

                    for (int i = 2; i < g.Rows.Count; i++)
                    {
                        sheet1.Cell(i, 1).Value = g[colEditDate, i].Value.ToString();
                        sheet1.Cell(i, 2).Value = g[colPcName, i].Value.ToString();
                        sheet1.Cell(i, 3).Value = g[colTokuisakiNM, i].Value.ToString();
                        sheet1.Cell(i, 4).Value = g[colPatternID, i].Value.ToString();
                        sheet1.Cell(i, 5).Value = g[colYear, i].Value.ToString();
                        sheet1.Cell(i, 6).Value = g[ColSyohinNM, i].Value.ToString();
                        sheet1.Cell(i, 7).Value = g[colField, i].Value.ToString();
                        sheet1.Cell(i, 8).Value = g[colBefore, i].Value.ToString();
                        sheet1.Cell(i, 9).Value = g[colAfter, i].Value.ToString();
                    }

                    // 表示位置
                    sheet1.Column("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("C").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    sheet1.Column("D").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("F").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    sheet1.Column("G").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    sheet1.Column("H").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("I").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // 書式設定
                    sheet1.Column("A").Style.NumberFormat.SetFormat("yyyy/mm/dd HH:MM:SS");
                    sheet1.Column("D").Style.NumberFormat.SetFormat("00000");
                    sheet1.Column("E").Style.NumberFormat.SetFormat("yyyy年m月");
                    //sheet1.Column("H").Style.NumberFormat.SetFormat("#,##0");

                    // セル表示幅
                    sheet1.Column("A").Width = 26;
                    sheet1.Column("B").Width = 22;
                    sheet1.Column("C").Width = 62;
                    sheet1.Column("D").Width = 10;
                    sheet1.Column("E").Width = 12;
                    sheet1.Column("F").Width = 50;
                    sheet1.Column("G").Width = 24;
                    sheet1.Column("H").Width = 10;
                    sheet1.Column("I").Width = 10;

                    // 全体を縮小して表示
                    sheet1.Column("C").Style.Alignment.ShrinkToFit = true;
                    sheet1.Column("F").Style.Alignment.ShrinkToFit = true;

                    // 上下罫線
                    sheet1.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet1.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Range("A1:I1").Style.Fill.BackgroundColor = XLColor.LightGray;

                    // 全体の罫線
                    sheet1.Range("A1:I1").Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    sheet1.Range("A1:I1").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    sheet1.Range(sheet1.Cell("A1"), sheet1.LastCellUsed()).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        .Border.SetRightBorder(XLBorderStyleValues.Thin);
                    sheet1.Range("A" + (sheet1.RowsUsed().Count()) + ":I" + (sheet1.RowsUsed().Count())).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);

                    // 行の固定
                    sheet1.SheetView.FreezeRows(1);

                    DialogResult ret;

                    string fName = "発注書編集ログ";

                    //ダイアログボックスの初期設定
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Title = "発注書編集ログ";
                    saveFileDialog1.OverwritePrompt = true;
                    saveFileDialog1.RestoreDirectory = true;
                    saveFileDialog1.FileName = fName + "_" + DateTime.Today.Year + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2");
                    saveFileDialog1.Filter = "Microsoft Office Excelファイル(*.xlsx)|*.xlsx|全てのファイル(*.*)|*.*";

                    //ダイアログボックスを表示し「保存」ボタンが選択されたらファイル名を表示
                    string fileName;
                    ret = saveFileDialog1.ShowDialog();

                    if (ret == System.Windows.Forms.DialogResult.OK)
                    {
                        // エクセル保存
                        fileName = saveFileDialog1.FileName;
                        bk.SaveAs(fileName);

                        // メッセージ
                        MessageBox.Show("Excel出力が終了しました", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
