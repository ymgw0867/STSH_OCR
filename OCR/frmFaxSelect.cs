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
using STSH_OCR.Common;

namespace STSH_OCR.OCR
{
    public partial class frmFaxSelect : Form
    {
        public frmFaxSelect()
        {
            InitializeComponent();
        }

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;

        // FAX発注書データ
        Table<Common.ClsFaxOrder> dbFax = null;
        ClsFaxOrder ClsFaxOrder= null;

        // カラム定義
        private string colDirName = "c0";
        private string colCount = "c1";
        private string colTkName = "c2";
        private string colZip = "c3";
        private string colAddress = "c4";
        private string colPtnID = "c5";
        private string colDate = "c6";
        private string colID = "c7";
        private string colMemo = "c8";
        private string colChk = "c9";
        private string colSecoundNum = "c10";

        string[] vs = null;

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
            {
                e.Handled = true;
            }
        }

        private void frmFaxSelect_Load(object sender, EventArgs e)
        {
            // ローカルマスター接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            dbFax = context.GetTable<Common.ClsFaxOrder>();

            GridviewSet(dataGridView1);
            ShowDirectoryCount(dataGridView1);

            textBox1.Text = "";


            //foreach (var t in dtsC.保留注文書.OrderBy(a => a.更新年月日))
            //{
            //    checkedListBox1.Items.Add(t.更新年月日.ToShortDateString() + " " + t.更新年月日.Hour + ":" + t.更新年月日.Minute + ":" + t.更新年月日.Second + ", " + t.ID);                
            //}

            lblDataCnt.Text = dbFax.Count().ToString();
        }


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
                tempDGV.Height = 407;

                // 奇数行の色
                tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

                // 各列幅指定
                tempDGV.Columns.Add(colID, "No.");
                tempDGV.Columns.Add(colDirName, "フォルダ名");
                tempDGV.Columns.Add(colCount, "受信件数");
                tempDGV.Columns.Add(colChk, "");
                tempDGV.Columns[colChk].Visible = false;

                tempDGV.Columns[colID].Width = 40;
                tempDGV.Columns[colDirName].Width = 200;
                tempDGV.Columns[colCount].Width = 110;

                tempDGV.Columns[colDirName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                tempDGV.Columns[colID].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                tempDGV.Columns[colCount].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // 編集可否
                tempDGV.ReadOnly = true;

                //foreach (DataGridViewColumn item in tempDGV.Columns)
                //{
                //    if (item.Name == colChk)
                //    {
                //        item.ReadOnly = false;
                //    }
                //    else
                //    {
                //        item.ReadOnly = true;
                //    }
                //}

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

        private void ShowDirectoryCount(DataGridView view)
        {
            string[] subFolders = System.IO.Directory.GetDirectories(Properties.Settings.Default.DataPath);

            foreach (var dir in subFolders)
            {
                view.Rows.Add();
                view[colID, view.Rows.Count - 1].Value = view.Rows.Count;
                view[colDirName, view.Rows.Count - 1].Value = System.IO.Path.GetFileName(dir);
                view[colCount, view.Rows.Count - 1].Value = System.IO.Directory.GetFiles(dir, "*.csv").Count();
                view[colChk, view.Rows.Count - 1].Value = dir;
            }

            view.CurrentCell = null;
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            //if (Utility.StrtoInt(textBox1.Text) > ALLCnt)
            //{
            //    textBox1.Text = ALLCnt.ToString();
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myCnt = 0;
            myBool = false;
            Close();
        }

        public int myCnt { get; set; }
        public bool myBool { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = getDataCount();

            if (n == 0)
            {
                MessageBox.Show("処理するデータがありません", "確認", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(n + "件の発注書を処理します。よろしいですか。", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            
            myBool = true;

            myCnt = Utility.StrtoInt(textBox1.Text);

            // 受信したＦＡＸ発注書を取り込む
            if (myCnt > 0)
            {
                getFaxData();
            }

            // 保留データをＦＡＸ発注書データに戻す
            holdToData();

            Close();
        }

        ///-------------------------------------------------------
        /// <summary>
        ///     保留データをＦＡＸ発注書データに戻す </summary>
        ///-------------------------------------------------------
        private void holdToData()
        {
            if (checkedListBox1.SelectedItems.Count == 0)
            {
                return;
            }

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string s = checkedListBox1.CheckedItems[i].ToString();
                string[] st = s.Split(',');

                setHoldToData(st[1].Trim());
            }
        }
        
        ///----------------------------------------------------------
        /// <summary>
        ///     保留処理 </summary>
        /// <param name="iX">
        ///     データインデックス</param>
        ///----------------------------------------------------------
        private void setHoldToData(string iX)
        {
            try
            {
                //var t = dtsC.保留注文書.Single(a => a.ID == iX);

                //NHBR_CLIDataSet.FAX注文書Row hr = dtsC.FAX注文書.NewFAX注文書Row();

                //hr.ID = t.ID;
                //hr.画像名 = t.画像名;
                //hr.届先番号 = t.届先番号;
                //hr.パターンID = t.パターンID;
                //hr.発注番号 = t.発注番号;
                //hr.納品希望月 = t.納品希望月;
                //hr.納品希望日 = t.納品希望日;
                //hr.注文数1 = t.注文数1;
                //hr.注文数2 = t.注文数2;
                //hr.注文数3 = t.注文数3;
                //hr.注文数4 = t.注文数4;
                //hr.注文数5 = t.注文数5;
                //hr.注文数6 = t.注文数6;
                //hr.注文数7 = t.注文数7;
                //hr.注文数8 = t.注文数8;
                //hr.注文数9 = t.注文数9;
                //hr.注文数10 = t.注文数10;
                //hr.注文数11 = t.注文数11;
                //hr.注文数12 = t.注文数12;
                //hr.注文数13 = t.注文数13;
                //hr.注文数14 = t.注文数14;
                //hr.注文数15 = t.注文数15;
                //hr.注文数16 = t.注文数16;
                //hr.注文数17 = t.注文数17;
                //hr.注文数18 = t.注文数18;
                //hr.注文数19 = t.注文数19;
                //hr.注文数20 = t.注文数20;
                //hr.注文数21 = t.注文数21;
                //hr.注文数22 = t.注文数22;
                //hr.注文数23 = t.注文数23;
                //hr.注文数24 = t.注文数24;
                //hr.注文数25 = t.注文数25;
                //hr.注文数26 = t.注文数26;
                //hr.注文数27 = t.注文数27;
                //hr.注文数28 = t.注文数28;
                //hr.注文数29 = t.注文数29;
                //hr.注文数30 = t.注文数30;

                //hr.追加注文チェック = t.追加注文チェック;
                //hr.追加注文数1 = t.追加注文数1;
                //hr.追加注文数2 = t.追加注文数2;
                //hr.追加注文数3 = t.追加注文数3;
                //hr.追加注文数4 = t.追加注文数4;
                //hr.追加注文数5 = t.追加注文数5;
                //hr.追加注文数6 = t.追加注文数6;
                //hr.追加注文数7 = t.追加注文数7;
                //hr.追加注文数8 = t.追加注文数8;
                //hr.追加注文数9 = t.追加注文数9;
                //hr.追加注文数10 = t.追加注文数10;

                //hr.追加注文商品コード1 = t.追加注文商品コード1;
                //hr.追加注文商品コード2 = t.追加注文商品コード2;
                //hr.追加注文商品コード3 = t.追加注文商品コード3;
                //hr.追加注文商品コード4 = t.追加注文商品コード4;
                //hr.追加注文商品コード5 = t.追加注文商品コード5;
                //hr.追加注文商品コード6 = t.追加注文商品コード6;
                //hr.追加注文商品コード7 = t.追加注文商品コード7;
                //hr.追加注文商品コード8 = t.追加注文商品コード8;
                //hr.追加注文商品コード9 = t.追加注文商品コード9;
                //hr.追加注文商品コード10 = t.追加注文商品コード10;

                //hr.担当者コード = t.担当者コード;
                //hr.備考欄記入 = t.備考欄記入;
                //hr.メモ = t.メモ;
                //hr.エラー有無 = t.エラー有無;
                //hr.更新年月日 = DateTime.Now;
                //hr.確認 = t.確認;

                //// 2017/08/23
                //hr.商品コード1 = t.商品コード1;
                //hr.商品コード2 = t.商品コード2;
                //hr.商品コード3 = t.商品コード3;
                //hr.商品コード4 = t.商品コード4;
                //hr.商品コード5 = t.商品コード5;
                //hr.商品コード6 = t.商品コード6;
                //hr.商品コード7 = t.商品コード7;
                //hr.商品コード8 = t.商品コード8;
                //hr.商品コード9 = t.商品コード9;
                //hr.商品コード10 = t.商品コード10;

                //hr.商品コード11 = t.商品コード11;
                //hr.商品コード12 = t.商品コード12;
                //hr.商品コード13 = t.商品コード13;
                //hr.商品コード14 = t.商品コード14;
                //hr.商品コード15 = t.商品コード15;
                //hr.商品コード16 = t.商品コード16;
                //hr.商品コード17 = t.商品コード17;
                //hr.商品コード18 = t.商品コード18;
                //hr.商品コード19 = t.商品コード19;
                //hr.商品コード20 = t.商品コード20;

                //hr.商品コード21 = t.商品コード21;
                //hr.商品コード22 = t.商品コード22;
                //hr.商品コード23 = t.商品コード23;
                //hr.商品コード24 = t.商品コード24;
                //hr.商品コード25 = t.商品コード25;
                //hr.商品コード26 = t.商品コード26;
                //hr.商品コード27 = t.商品コード27;
                //hr.商品コード28 = t.商品コード28;
                //hr.商品コード29 = t.商品コード29;
                //hr.商品コード30 = t.商品コード30;

                //// ＦＡＸ発注書追加処理
                //dtsC.FAX注文書.AddFAX注文書Row(hr);
                //fAdp.Update(dtsC.FAX注文書);

                //// 保留データ削除
                //t.Delete();
                //hAdp.Update(dtsC.保留注文書);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        ///-----------------------------------------------------------
        /// <summary>
        ///     処理データ件数を取得する </summary>
        /// <returns>
        ///     データ件数 </returns>
        ///-----------------------------------------------------------
        private int getDataCount()
        {
            int dCnt = 0;

            dCnt += Utility.StrtoInt(textBox1.Text);
            dCnt += checkedListBox1.CheckedItems.Count;
            dCnt += Utility.StrtoInt(lblDataCnt.Text);

            return dCnt;
        }
        
        ///-------------------------------------------------------
        /// <summary>
        ///     ＦＡＸ発注書を自分のフォルダへ取り込む </summary>
        /// <param name="mCnt">
        ///     取り込む枚数</param>
        ///-------------------------------------------------------
        private void getFaxData()
        {
            int MoveFileCnt = 0;

            Boolean fMoveFlg = false;

            //if (MessageBox.Show(textBox1.Text + "枚の受信ＦＡＸ発注書を取り込みます。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            //{
            //    return;
            //}

            try
            {
                foreach (var item in listBox1.SelectedIndices)
                {
                    fMoveFlg = false;

                    int i = Utility.StrtoInt(item.ToString());

                    //*****CSV******
                    //移動先に同じ名前のファイルが存在する場合、既にあるファイルを削除する
                    string csvFname = Properties.Settings.Default.MyDataPath + System.IO.Path.GetFileName(vs[i]);

                    if (System.IO.File.Exists(csvFname))
                    {
                        System.IO.File.Delete(csvFname);
                    }

                    System.IO.File.Move(vs[i], csvFname);

                    fMoveFlg = true;

                    if (fMoveFlg)
                    {
                        //*****TIF******
                        //移動先に同じ名前のファイルが存在する場合、既にあるファイルを削除する
                        string tifName = Properties.Settings.Default.MyDataPath + System.IO.Path.GetFileName(vs[i].Replace("csv", "tif"));

                        if (System.IO.File.Exists(tifName))
                        {
                            System.IO.File.Delete(tifName);
                        }

                        System.IO.File.Move(vs[i].Replace("csv", "tif"), tifName);

                        MoveFileCnt++;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ///-----------------------------------------------------------------
        /// <summary>
        ///     派遣先毎の件数をグリッドビューに表示 </summary>
        /// <param name="_inPath">
        ///     Dataフォルダ</param>
        /// <param name="dg">
        ///     データグリッドビュー</param>
        ///-----------------------------------------------------------------
        private void getCsvData(string _inPath)
        {
            listBox1.Items.Clear();

            // 対象CSVファイル数を取得
            string[] t = System.IO.Directory.GetFiles(_inPath, "*.csv");

            // CSVファイルがなければ終了
            if (t.Length == 0)
            {
                return;
            }

            // 得意先名、電話番号、住所表示
            string gTel = string.Empty;
            string gJyu = string.Empty;

            int iV = 0;

            foreach (string files in System.IO.Directory.GetFiles(_inPath, "*.csv"))
            {
                // CSVファイルインポート
                var s = System.IO.File.ReadAllLines(files, System.Text.Encoding.Default);

                foreach (var stBuffer in s)
                {
                    // カンマ区切りで分割して配列に格納する
                    string[] stCSV = stBuffer.Split(',');

                    if (stCSV.Length < 7)
                    {
                        continue;
                    }

                    // ヘッダ行
                    if (stCSV[0] == "*")
                    {
                        string tkCode = Utility.GetStringSubMax(stCSV[5].Trim(), 8);
                        string datetime = System.IO.Path.GetFileNameWithoutExtension(files);
                        string ListItem = datetime.Substring(0, 4) + "/" + datetime.Substring(4, 2) + "/" + datetime.Substring(6, 2) + " " + 
                                          datetime.Substring(8, 2) + ":" + datetime.Substring(10, 2) + ":" + datetime.Substring(12, 2) + " ";
                        string _tkName = Utility.getNouhinName(tkCode, out gTel, out gJyu);
                        if (_tkName == string.Empty)
                        {
                            ListItem += "--- 不明 ---";
                        }
                        else
                        {
                            ListItem += Utility.getNouhinName(tkCode, out gTel, out gJyu);
                        }

                        listBox1.Items.Add(ListItem);

                        iV++;
                        Array.Resize(ref vs, iV);
                        vs[iV - 1] = files;

                        break;
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            vs = null;
            string dir = dataGridView1.SelectedRows[0].Cells[colChk].Value.ToString();
            getCsvData(dir);

            textBox1.Text = "";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBox1.SelectedItems.Count.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
