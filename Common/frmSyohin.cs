using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace STSH_OCR.Common
{
    public partial class frmSyohin : Form
    {
        public frmSyohin(bool mMode)
        {
            InitializeComponent();
            _mMode = mMode;
        }

        bool _mMode = false;

        // カラム定義
        private string colCode = "c0";
        private string colName = "c1";
        private string colShiireNM = "c2";
        private string colJan = "c3";
        private string colKikaku = "c4";
        private string colSyubai = "c5";

        /// <summary>
        /// データグリッドビューの定義を行います
        /// </summary>
        private void GridviewSet(DataGridView tempDGV)
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
                tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

                // データフォント指定
                tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", 10, FontStyle.Regular);

                // 行の高さ
                tempDGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                tempDGV.ColumnHeadersHeight = 20;
                tempDGV.RowTemplate.Height = 20;

                // 全体の高さ
                tempDGV.Height = 502;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colCode, "商品番号");
                tempDGV.Columns.Add(colShiireNM, "仕入先名");
                tempDGV.Columns.Add(colName, "商品名");
                tempDGV.Columns.Add(colJan, "JANコード");
                tempDGV.Columns.Add(colKikaku, "規格");
                tempDGV.Columns.Add(colSyubai, "終売日");

                tempDGV.Columns[colCode].Width = 80;
                tempDGV.Columns[colShiireNM].Width = 160;
                tempDGV.Columns[colName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                tempDGV.Columns[colJan].Width = 100;
                tempDGV.Columns[colKikaku].Width = 90;
                tempDGV.Columns[colSyubai].Width = 100;

                //tempDGV.Columns[colAddress].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colCode].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colKikaku].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colJan].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colSyubai].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = true;

                // 行ヘッダを表示しない
                tempDGV.RowHeadersVisible = false;

                // 選択モード
                tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                if (_mMode)
                {
                    tempDGV.MultiSelect = true;
                }
                else
                {
                    tempDGV.MultiSelect = false;
                }

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

        private void showNouhin(DataGridView g)
        {            
            this.Cursor = Cursors.WaitCursor;

            ClsCsvData.ClsCsvSyohin [] syohins = Utility.GetSyohinData(Properties.Settings.Default.商品マスター, Properties.Settings.Default.商品在庫マスター, Properties.Settings.Default.仕入先マスター);

            int cnt = 0;
            dataGridView1.Rows.Clear();

            foreach (var t in syohins.OrderBy(a => a.SIRESAKI_CD).ThenBy(a => a.SYOHIN_CD))
            {
                if (!checkBox1.Checked && t.SHUBAI)
                {
                    continue;
                }
                
                // 商品コード検索
                if (sCode.Text != string.Empty && !t.SYOHIN_CD.Contains(sCode.Text))
                {
                    continue;
                }

                // 商品名カナ検索
                if (sName.Text != string.Empty && !t.SYOHIN_KANA.Contains(sName.Text))
                {
                    continue;
                }

                // 仕入先コード検索
                if (sSCode.Text != string.Empty && !t.SIRESAKI_CD.Contains(sSCode.Text))
                {
                    continue;
                }

                // 仕入先カナ検索
                if (sSName.Text != string.Empty && !t.SIRESAKI_KANA_NM.Contains(sSName.Text))
                {
                    continue;
                }

                // JANコード検索
                if (sJanCode.Text != string.Empty && !t.JAN_CD.Contains(sJanCode.Text))
                {
                    continue;
                }

                // 商品表示
                g.Rows.Add();
                dataGridView1[colCode, cnt].Value = t.SYOHIN_CD;
                dataGridView1[colShiireNM, cnt].Value = t.SIRESAKI_NM;
                dataGridView1[colName, cnt].Value = t.SYOHIN_NM;
                dataGridView1[colJan, cnt].Value = t.JAN_CD;
                dataGridView1[colKikaku, cnt].Value = t.SYOHIN_KIKAKU;

                if (t.LAST_SALE_YMD.Length > 7)
                {
                    dataGridView1[colSyubai, cnt].Value = t.LAST_SALE_YMD.Substring(0, 4) + "/" +
                                                          t.LAST_SALE_YMD.Substring(4, 2) + "/" +
                                                          t.LAST_SALE_YMD.Substring(6, 2);
                }
                else
                {
                    dataGridView1[colSyubai, cnt].Value = ""; ;
                }

                if (t.SHUBAI)
                {
                    dataGridView1.Rows[cnt].DefaultCellStyle.ForeColor = Color.Red;
                }
                else
                {
                    dataGridView1.Rows[cnt].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                }

                cnt++;
            }            

            this.Cursor = Cursors.Default;

            if (cnt == 0)
            {
                MessageBox.Show("該当する商品はありませんでした", "検索結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            dataGridView1.CurrentCell = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void frmTodoke_Load(object sender, EventArgs e)
        {
            // フォーム最大値
            Utility.WindowsMaxSize(this, this.Width, this.Height);

            // フォーム最小値
            Utility.WindowsMinSize(this, this.Width, this.Height);

            GridviewSet(dataGridView1);

            _nouCode = null;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int iX = 0;

            if (_mMode)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        Array.Resize(ref _nouCode, iX + 1);
                        _nouCode[iX] = dataGridView1[colCode, i].Value.ToString();
                        iX++;
                    }
                }
            }
            else
            {
                Array.Resize(ref _nouCode, iX + 1);
                _nouCode[iX] = dataGridView1[colCode, dataGridView1.SelectedRows[0].Index].Value.ToString();
            }

            Close();
        }

        public string[] _nouCode;

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getSelectRows();
        }

        private void getSelectRows()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int iX = 0;

            if (_mMode)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Selected)
                    {
                        Array.Resize(ref _nouCode, iX + 1);
                        _nouCode[iX] = dataGridView1[colCode, i].Value.ToString();
                        iX++;
                    }
                }
            }
            else
            {
                Array.Resize(ref _nouCode, iX + 1);
                _nouCode[iX] = dataGridView1[colCode, dataGridView1.SelectedRows[0].Index].Value.ToString();
            }

            Close();
        }

        private void frmTodoke_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F10 && button3.Enabled)
            {
                getSelectRows();
            }

            if (e.KeyData == Keys.F12)
            {
                Close();
            }
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            showNouhin(dataGridView1);
        }
    }
}
