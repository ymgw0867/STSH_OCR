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

            GridViewSetting(dataGridView1);

        }

        string colStaffCode = "c1";
        string colPcName = "c2";
        string colDate = "c3";
        string colEditDate = "c4";
        string colField = "c5";
        string colBefore = "c6";
        string colAfter = "c7";
        string colAccount = "c8";
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
            tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 10, FontStyle.Regular);

            // データフォント指定
            tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", (float)11, FontStyle.Regular);

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
            tempDGV.Columns.Add(colTokuisakiCD, "得意先コード");
            tempDGV.Columns.Add(colTokuisakiNM, "得意先名");
            tempDGV.Columns.Add(colPatternID, "FAX発注書");
            tempDGV.Columns.Add(colYear, "年");
            tempDGV.Columns.Add(colMonth, "月");
            tempDGV.Columns.Add(colDay, "日");
            tempDGV.Columns.Add(colField, "編集項目");
            tempDGV.Columns.Add(colBefore, "編集前");
            tempDGV.Columns.Add(colAfter, "編集後");
            tempDGV.Columns.Add(colID, "発注書ID");

            tempDGV.Columns[colEditDate].Width = 160;
            tempDGV.Columns[colPcName].Width = 100;
            tempDGV.Columns[colTokuisakiCD].Width = 200;
            tempDGV.Columns[colTokuisakiNM].Width = 110;
            tempDGV.Columns[colPatternID].Width = 130;
            tempDGV.Columns[colBefore].Width = 100;
            tempDGV.Columns[colAfter].Width = 100;
            tempDGV.Columns[colYear].Width = 100;
            tempDGV.Columns[colMonth].Width = 100;
            tempDGV.Columns[colDay].Width = 100;
            tempDGV.Columns[colField].Width = 300;
            tempDGV.Columns[colID].Width = 100;

            //tempDGV.Columns[colAccount].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //tempDGV.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tempDGV.Columns[colEditDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colTokuisakiCD].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colYear].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colMonth].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tempDGV.Columns[colDay].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
            tempDGV.AllowUserToResizeColumns = false;

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
                DateTime edt = new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, dateTimePicker2.Value.Day, 23, 59, 59);
                var s = tblEditLog.Where(a => a.Date_Time.CompareTo(dateTimePicker1.Value.ToString()) >= 0 && 
                    a.Date_Time.CompareTo(edt.ToString()) <= 0)
                    .OrderByDescending(a => a.Date_Time);

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
                    g[colTokuisakiCD, g.Rows.Count - 1].Value = t.TokuisakiCode;
                    g[colTokuisakiNM, g.Rows.Count - 1].Value = t.TokuisakiName;
                    g[colPatternID, g.Rows.Count - 1].Value = t.patternID.PadLeft(2, '0') + t.patternIDSeq.PadLeft(2, '0');
                    g[colYear, g.Rows.Count - 1].Value = t.Year;
                    g[colMonth, g.Rows.Count - 1].Value = t.Month;
                    g[colDay, g.Rows.Count - 1].Value = t.TenchakuDate;
                    g[colField, g.Rows.Count - 1].Value = t.ShohinName + " " + t.FieldName;
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
    }
}
