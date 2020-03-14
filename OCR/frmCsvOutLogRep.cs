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
    public partial class frmCsvOutLog : Form
    {
        public frmCsvOutLog()
        {
            InitializeComponent();
        }

        private void frmEditLogRep_Load(object sender, EventArgs e)
        {
            // ウィンドウズ最小サイズ
            Utility.WindowsMinSize(this, this.Size.Width, this.Size.Height);

            // ウィンドウズ最大サイズ
            Utility.WindowsMaxSize(this, this.Size.Width, this.Size.Height);

            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker2.Value = DateTime.Today;

            // 共有DB接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            tblCsvOut = context.GetTable<Common.ClsCsvOutHistory>(); // 編集ログテーブル

            // PC名コンボボックスアイテム追加
            foreach (var t in tblCsvOut.Select(a => a.PcName).Distinct())
            {
                comboBox1.Items.Add(t);
            }

            GridViewSetting(dataGridView1);

            button1.Enabled = false;
        }

        string colPcName = "c2";
        string colEditDate = "c4";
        string colCount = "c5";

        // データベース：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        string db_file = Properties.Settings.Default.DB_File;
        
        // CSVデータ作成ログ
        Table<Common.ClsCsvOutHistory> tblCsvOut = null;
        ClsCsvOutHistory OutHistory = null;

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
            tempDGV.Columns.Add(colEditDate, "作成日時");
            tempDGV.Columns.Add(colPcName, "ＰＣ名");
            //tempDGV.Columns.Add(colTokuisakiCD, "得意先コード");
            tempDGV.Columns.Add(colCount, "出力件数");

            tempDGV.Columns[colEditDate].Width = 180;
            tempDGV.Columns[colPcName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tempDGV.Columns[colCount].Width = 100;

            tempDGV.Columns[colEditDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colPcName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            tempDGV.Columns[colCount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                              sdt.Hour.ToString("D2") + ":" + sdt.Minute.ToString("D2") + ":" + sdt.Second.ToString("D2");

                // 終了日付
                DateTime edt = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, 23, 59, 59);
                string _edt = edt.Year + "/" + edt.Month.ToString("D2") + "/" + edt.Day.ToString("D2") + " " +
                              edt.Hour.ToString("D2") + ":" + edt.Minute.ToString("D2") + ":" + edt.Second.ToString("D2");

                var s = tblCsvOut.Where(a => a.WriteDateTime.CompareTo(_sdt) >= 0 && a.WriteDateTime.CompareTo(_edt) <= 0).OrderByDescending(a => a.WriteDateTime);

                // PC指定
                if (comboBox1.SelectedIndex != -1)
                {
                    s = s.Where(a => a.PcName.Contains(comboBox1.Text)).OrderByDescending(a => a.WriteDateTime);
                }

                foreach (var t in s)
                {
                    g.Rows.Add();

                    g[colEditDate, g.Rows.Count - 1].Value = t.WriteDateTime;
                    g[colPcName, g.Rows.Count - 1].Value = t.PcName;
                    g[colCount, g.Rows.Count - 1].Value = t.OutPutCount.ToString("#,##0");
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
                MessageBox.Show("該当するデータはありませんでした", "CSVデータ作成ログ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
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
            if (MessageBox.Show("表示中の作成ログをExcel形式で出力します。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            ExcelOutput(dataGridView1);
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     Excel出力 </summary>
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

                    var sheet1 = bk.AddWorksheet("CSVデータ作成ログ");
                    
                    sheet1.Cell("A1").SetValue("編集日時");
                    sheet1.Cell("B1").SetValue("ＰＣ名");
                    sheet1.Cell("C1").SetValue("出力件数");

                    for (int i = 0; i < g.Rows.Count; i++)
                    {
                        sheet1.Cell(i + 2, 1).Value = g[colEditDate, i].Value.ToString();
                        sheet1.Cell(i + 2, 2).Value = g[colPcName, i].Value.ToString();
                        sheet1.Cell(i + 2, 3).Value = g[colCount, i].Value.ToString();
                    }

                    // 表示位置
                    sheet1.Column("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("B").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Column("C").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // 書式設定
                    sheet1.Column("A").Style.NumberFormat.SetFormat("yyyy/mm/dd HH:MM:SS");
                    sheet1.Column("C").Style.NumberFormat.SetFormat("#,##0");

                    // セル表示幅
                    sheet1.Column("A").Width = 26;
                    sheet1.Column("B").Width = 33;
                    sheet1.Column("C").Width = 15;

                    // 全体を縮小して表示
                    //sheet1.Column("C").Style.Alignment.ShrinkToFit = true;
                    //sheet1.Column("F").Style.Alignment.ShrinkToFit = true;

                    // 上下罫線
                    sheet1.Row(1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    //sheet1.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    sheet1.Range("A1:C1").Style.Fill.BackgroundColor = XLColor.LightGray;

                    // 全体の罫線
                    sheet1.Range("A1:c1").Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                    sheet1.Range("A1:c1").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                    sheet1.Range(sheet1.Cell("A1"), sheet1.LastCellUsed()).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin)
                        .Border.SetRightBorder(XLBorderStyleValues.Thin);
                    sheet1.Range("A" + (sheet1.RowsUsed().Count()) + ":C" + (sheet1.RowsUsed().Count())).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);

                    // 行の固定
                    sheet1.SheetView.FreezeRows(1);

                    DialogResult ret;

                    string fName = "ＣＳＶデータ作成ログ";

                    //ダイアログボックスの初期設定
                    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Title = "ＣＳＶデータ作成ログ";
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
