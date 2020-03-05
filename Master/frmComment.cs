using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.Linq;
using STSH_OCR.Common;

namespace STSH_OCR.Master
{
    public partial class frmComment : Form
    {
        public frmComment()
        {
            InitializeComponent();

            Utility.WindowsMinSize(this, this.Width, this.Height);
        }

        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;
        Table<Common.ClsReFaxComment> conf = null;
        ClsReFaxComment ClsConf = null;

        private string ColID = "c0";
        private string ColComment = "c1";
        private string ColDate = "c2";

        const int ADD_MODE = 0;
        const int EDIT_MODE = 1;

        int eMode = 0; 

        private void frmComment_Load(object sender, EventArgs e)
        {
            GridviewSet(dataGridView1);

            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            conf = context.GetTable<Common.ClsReFaxComment>();

            // DataGridViewにデータ表示
            ConfigDataShow(dataGridView1);
        }

        ///---------------------------------------------------------
        /// <summary>
        /// データグリッドビューの定義を行います </summary>
        ///---------------------------------------------------------
        private void GridviewSet(DataGridView tempDGV)
        {
            try
            {
                //フォームサイズ定義

                // 列スタイルを変更する
                tempDGV.EnableHeadersVisualStyles = false;
                tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
                tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                // 列ヘッダー表示位置指定
                tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 列ヘッダーフォント指定
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                //tempDGV.Height = 618;

                // 奇数行の色
                //tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                // 各列幅指定
                tempDGV.Columns.Add(ColID, "ID");
                tempDGV.Columns.Add(ColComment, "定型コメント");
                tempDGV.Columns.Add(ColDate, "更新年月日");

                tempDGV.Columns[ColID].Width = 60;
                tempDGV.Columns[ColComment].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tempDGV.Columns[ColDate].Width = 140;

                tempDGV.Columns[ColID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[ColComment].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                tempDGV.Columns[ColDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

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
                //tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラーメッセージ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///---------------------------------------------------------------
        /// <summary>
        ///     DataGridViewにデータ表示 </summary>
        /// <param name="dataGrid">
        ///     DataGridViewオブジェクト</param>
        ///---------------------------------------------------------------
        private void ConfigDataShow(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();

            foreach (var item in conf.OrderBy(a => a.ID))
            {
                dataGrid.Rows.Add();
                dataGrid[ColID, dataGrid.Rows.Count - 1].Value = item.ID.ToString();
                dataGrid[ColComment, dataGrid.Rows.Count - 1].Value = item.Comment.ToString();
                dataGrid[ColDate, dataGrid.Rows.Count - 1].Value = item.YyMmDd;
            }

            dataGrid.CurrentCell = null;

            // 画面初期化
            DataClear();
        }

        ///------------------------------------------------------
        /// <summary>
        ///     画面初期化 </summary>
        ///------------------------------------------------------
        private void DataClear()
        {
            textBox1.Text = string.Empty;

            ClsConf = null;

            //button2.Enabled = false;
            button1.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;

            dataGridView1.CurrentCell = null;

            eMode = ADD_MODE;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // 指定データを表示
                ClsConf = conf.Single(a => a.ID == Utility.StrtoInt(dataGridView1[ColID, e.RowIndex].Value.ToString()));

                textBox1.Text = ClsConf.Comment;

                //button2.Enabled = true;
                button1.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;

                eMode = EDIT_MODE;
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("定型コメントを入力してください", "コメント未入力", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (eMode == ADD_MODE)
            {
                // データ追加
                DataAdd();
            }
            else if (eMode == EDIT_MODE)
            {
                // データ更新
                DataEdit();
            }

            // データ再表示
            ConfigDataShow(dataGridView1);

            // 画面初期化
            DataClear();
        }

        private void DataAdd()
        {
            cn.Open();

            try
            {
                // データ追加
                string sql = "insert into ReFaxComment ";
                sql += "(定型コメント, 更新年月日) ";
                sql += "values ('";
                sql += textBox1.Text + "','";
                sql += DateTime.Now.ToString() + "');";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

            }
            catch (SQLiteException exc)
            {
                MessageBox.Show(exc.Message);
                //System.Diagnostics.Debug.WriteLine(exc.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void DataEdit()
        {
            try
            {
                // データ更新
                ClsConf.Comment = textBox1.Text;
                ClsConf.YyMmDd = DateTime.Now.ToString();

                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の定型コメントを削除します。よろしいですか？", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            // データ削除
            DataDelete();

            // データ再表示
            ConfigDataShow(dataGridView1);

            // 画面初期化
            DataClear();
        }

        private void DataDelete()
        {
            // データ削除
            conf.DeleteOnSubmit(ClsConf);
            context.SubmitChanges();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MyComment = string.Empty;
            Close();
        }

        private void frmComment_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            //Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataClear();
        }

        private void frmComment_Shown(object sender, EventArgs e)
        {
            dataGridView1.CurrentCell = null;
        }

        public string MyComment { get; set; }

        private void button5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("表示中の定型コメントを返信ファクスへ引用します。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            MyComment = textBox1.Text;

            // 終了
            Close();
        }
    }
}
