using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;
using System.Data.Linq;
using STSH_OCR.Common;

namespace STSH_OCR.Common
{
    public partial class frmPtnCall : Form
    {
        public frmPtnCall()
        {
            InitializeComponent();
        }

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // 発注書パターンマスター
        Table<Common.ClsOrderPattern> dbPtn = null;
        ClsOrderPattern ClsOrderPattern = null;

        //ClsCsvData.ClsCsvTokuisaki[] csvTokuisakis = null;

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
        private string colSecondNum = "c9";

        ///-------------------------------------------------------------------
        /// <summary>
        ///     データグリッドビューの定義 </summary>
        /// <param name="tempDGV">
        ///     DataGridViewオブジェクト</param>
        ///-------------------------------------------------------------------
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
                //tempDGV.DefaultCellStyle.Font = new Font("游ゴシック", 9, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 582;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colNouCode, "コード");
                tempDGV.Columns.Add(colNouName, "得意先名");
                tempDGV.Columns.Add(colPtnID, "発注書ID");
                tempDGV.Columns.Add(colSecondNum, "枝番");
                tempDGV.Columns.Add(colMemo, "備考");
                tempDGV.Columns.Add(colTel, "TEL");
                tempDGV.Columns.Add(colAddress, "住所");
                tempDGV.Columns.Add(colDate, "登録日");
                tempDGV.Columns.Add(colID, "ID");

                tempDGV.Columns[colNouCode].Width = 80;
                tempDGV.Columns[colNouName].Width = 300;
                tempDGV.Columns[colPtnID].Width = 70;
                tempDGV.Columns[colSecondNum].Width = 60;
                tempDGV.Columns[colMemo].Width = 160;
                tempDGV.Columns[colTel].Width = 100;
                tempDGV.Columns[colDate].Width = 140;
                //tempDGV.Columns[colAddress].Width = 200;

                tempDGV.Columns[colAddress].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colNouCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colPtnID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colTel].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //tempDGV.Columns[colAddress].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                tempDGV.Columns[colID].Visible = false;

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

        private void getVNouhin()
        {
            // 得意先CSVデータ配列読み込み
            //string [] MstArray = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);  // 2020/04/08 コメント化
            //int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

            //csvTokuisakis = ClsCsvData.ClsCsvTokuisaki.Load(MstArray, toDate);　// 2020/04/08 コメント化
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     発注書パターンデータ表示 </summary>
        /// <param name="g">
        ///     DataGridViewオブジェクト</param>
        ///----------------------------------------------------------------
        private void ShowPattern(DataGridView g)
        {
            this.Cursor = Cursors.WaitCursor;

            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            dbPtn = context.GetTable<Common.ClsOrderPattern>();

            // 発注書パターンを表示
            g.Rows.Clear();

            int cnt = 0;
            //int i = 0;
            foreach (var s in dbPtn.OrderBy(a => a.TokuisakiCode).ThenBy(a => a.SeqNum).ThenBy(a => a.SecondNum))
            {
                int vI = 0;
                bool bl = false;

                ClsCsvData.ClsCsvTokuisaki tokuisaki = Utility.GetTokuisakiFromDataTable(s.TokuisakiCode.ToString("D2"), global.dtTokuisaki);

                if (tokuisaki.TOKUISAKI_CD == "")
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
                g[colNouCode, cnt].Value = s.TokuisakiCode;
                g[colNouName, cnt].Value = tokuisaki.TOKUISAKI_NM;
                g[colPtnID, cnt].Value = s.SeqNum.ToString().PadLeft(3, '0');
                g[colSecondNum, cnt].Value = s.SecondNum.ToString().PadLeft(3, '0');
                g[colMemo, cnt].Value = s.Memo;
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
                MessageBox.Show("該当する発注書パターンはありませんでした", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowPattern(dataGridView1);
        }

        private void frmTodoke_Load(object sender, EventArgs e)
        {
            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            GridviewSet(dataGridView1);

            ptnID = string.Empty;

            getVNouhin();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            ptnID = dataGridView1[colID, dataGridView1.SelectedRows[0].Index].Value.ToString();

            Close();
        }

        public string ptnID;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            ptnID = dataGridView1[colID, dataGridView1.SelectedRows[0].Index].Value.ToString();

            Close();
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
