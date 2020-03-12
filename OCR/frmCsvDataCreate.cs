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
    public partial class frmCsvDataCreate : Form
    {
        public frmCsvDataCreate()
        {
            InitializeComponent();
        }

        // ローカルマスター：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        string db_file = Properties.Settings.Default.DB_File;

        // FAX発注書データ
        Table<Common.ClsOrder> tblOrder = null;
        ClsOrder order = null;

        // CSVデータ出力先
        string _sPath = "";

        // CSVデータ出力方法
        int _FileAppend = 0;
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("CSVデータを出力します。よろしいですか？","確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            button1.Enabled = false;

            // CSVデータ出力処理
            int Cnt = CsvDataOutput();

            if (Cnt > 0)
            {
                // CSVデータ出力ログデータ書き込み
                CsvDataLogWrite(Cnt);

                // 発注データ削除
                DeleteOrderData();
            }

            // 終了
            //Close();
        }

        ///------------------------------------------------------
        /// <summary>
        ///     発注データ削除 </summary>
        ///------------------------------------------------------
        private void DeleteOrderData()
        {
            cn.Open();

            try
            {
                // 発注データバックアップテーブルに追加
                string sql = "INSERT INTO OrderData_Backup ";
                sql += "SELECT * FROM OrderData ";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                listBox1.Items.Add("発注データのバックアップを行いました"); 
                listBox1.TopIndex = listBox1.Items.Count - 1;

                // 発注データテーブル全件削除
                sql = "DELETE FROM OrderData ";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }

                listBox1.Items.Add("発注データを削除しました");
                listBox1.TopIndex = listBox1.Items.Count - 1;
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

        ///----------------------------------------------------------------
        /// <summary>
        ///     基幹システムCSVデータ作成 </summary>
        /// <returns>
        ///     作成件数</returns>
        ///----------------------------------------------------------------
        private int CsvDataOutput()
        {
            int orderCnt = tblOrder.Count();
            if (orderCnt == 0)
            {
                MessageBox.Show("発注データはありません","対象データなし",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }

            ClsCsvData.ClsCsvCSV[] csvDatas = null;

            progressBar1.Visible = true;
            int rCnt = 0;
            int dCnt = 0;

            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (var r in tblOrder.OrderBy(a => a.ID))
                {
                    string cTokuisakiCD = r.TokuisakiCode.ToString("D7");
                    ClsCsvData.ClsCsvTokuisaki tokuisaki = Utility.GetTokuisaki(r.TokuisakiCode.ToString("D7"));
                    string cTokuisakiNM = tokuisaki.TOKUISAKI_NM;
                    string cNouhinDT = string.Empty;
                    string cDT = global.DTKBN;

                    // 商品発注明細クラス
                    ClsGoods[] goods = new ClsGoods[15];
                    for (int i = 0; i < global.MAX_GYO; i++)
                    {
                        goods[i] = new ClsGoods();
                        goods[i].Suu = new string[7];

                        switch (i)
                        {
                            case 0:
                                goods[i].Code = r.G_Code1;
                                goods[i].Suu[0] = r.Goods1_1;
                                goods[i].Suu[1] = r.Goods1_2;
                                goods[i].Suu[2] = r.Goods1_3;
                                goods[i].Suu[3] = r.Goods1_4;
                                goods[i].Suu[4] = r.Goods1_5;
                                goods[i].Suu[5] = r.Goods1_6;
                                goods[i].Suu[6] = r.Goods1_7;
                                goods[i].Nouka = r.G_Nouka1;
                                goods[i].Baika = r.G_Baika1;
                                goods[i].Syubai = r.G_Syubai1;
                                break;

                            case 1:
                                goods[i].Code = r.G_Code2;
                                goods[i].Suu[0] = r.Goods2_1;
                                goods[i].Suu[1] = r.Goods2_2;
                                goods[i].Suu[2] = r.Goods2_3;
                                goods[i].Suu[3] = r.Goods2_4;
                                goods[i].Suu[4] = r.Goods2_5;
                                goods[i].Suu[5] = r.Goods2_6;
                                goods[i].Suu[6] = r.Goods2_7;
                                goods[i].Nouka = r.G_Nouka2;
                                goods[i].Baika = r.G_Baika2;
                                goods[i].Syubai = r.G_Syubai2;
                                break;

                            case 2:
                                goods[i].Code = r.G_Code3;
                                goods[i].Suu[0] = r.Goods3_1;
                                goods[i].Suu[1] = r.Goods3_2;
                                goods[i].Suu[2] = r.Goods3_3;
                                goods[i].Suu[3] = r.Goods3_4;
                                goods[i].Suu[4] = r.Goods3_5;
                                goods[i].Suu[5] = r.Goods3_6;
                                goods[i].Suu[6] = r.Goods3_7;
                                goods[i].Nouka = r.G_Nouka3;
                                goods[i].Baika = r.G_Baika3;
                                goods[i].Syubai = r.G_Syubai3;
                                break;

                            case 3:
                                goods[i].Code = r.G_Code4;
                                goods[i].Suu[0] = r.Goods4_1;
                                goods[i].Suu[1] = r.Goods4_2;
                                goods[i].Suu[2] = r.Goods4_3;
                                goods[i].Suu[3] = r.Goods4_4;
                                goods[i].Suu[4] = r.Goods4_5;
                                goods[i].Suu[5] = r.Goods4_6;
                                goods[i].Suu[6] = r.Goods4_7;
                                goods[i].Nouka = r.G_Nouka4;
                                goods[i].Baika = r.G_Baika4;
                                goods[i].Syubai = r.G_Syubai4;
                                break;

                            case 4:
                                goods[i].Code = r.G_Code5;
                                goods[i].Suu[0] = r.Goods5_1;
                                goods[i].Suu[1] = r.Goods5_2;
                                goods[i].Suu[2] = r.Goods5_3;
                                goods[i].Suu[3] = r.Goods5_4;
                                goods[i].Suu[4] = r.Goods5_5;
                                goods[i].Suu[5] = r.Goods5_6;
                                goods[i].Suu[6] = r.Goods5_7;
                                goods[i].Nouka = r.G_Nouka5;
                                goods[i].Baika = r.G_Baika5;
                                goods[i].Syubai = r.G_Syubai5;
                                break;

                            case 5:
                                goods[i].Code = r.G_Code6;
                                goods[i].Suu[0] = r.Goods6_1;
                                goods[i].Suu[1] = r.Goods6_2;
                                goods[i].Suu[2] = r.Goods6_3;
                                goods[i].Suu[3] = r.Goods6_4;
                                goods[i].Suu[4] = r.Goods6_5;
                                goods[i].Suu[5] = r.Goods6_6;
                                goods[i].Suu[6] = r.Goods6_7;
                                goods[i].Nouka = r.G_Nouka6;
                                goods[i].Baika = r.G_Baika6;
                                goods[i].Syubai = r.G_Syubai6;
                                break;

                            case 6:
                                goods[i].Code = r.G_Code7;
                                goods[i].Suu[0] = r.Goods7_1;
                                goods[i].Suu[1] = r.Goods7_2;
                                goods[i].Suu[2] = r.Goods7_3;
                                goods[i].Suu[3] = r.Goods7_4;
                                goods[i].Suu[4] = r.Goods7_5;
                                goods[i].Suu[5] = r.Goods7_6;
                                goods[i].Suu[6] = r.Goods7_7;
                                goods[i].Nouka = r.G_Nouka7;
                                goods[i].Baika = r.G_Baika7;
                                goods[i].Syubai = r.G_Syubai7;
                                break;

                            case 7:
                                goods[i].Code = r.G_Code8;
                                goods[i].Suu[0] = r.Goods8_1;
                                goods[i].Suu[1] = r.Goods8_2;
                                goods[i].Suu[2] = r.Goods8_3;
                                goods[i].Suu[3] = r.Goods8_4;
                                goods[i].Suu[4] = r.Goods8_5;
                                goods[i].Suu[5] = r.Goods8_6;
                                goods[i].Suu[6] = r.Goods8_7;
                                goods[i].Nouka = r.G_Nouka8;
                                goods[i].Baika = r.G_Baika8;
                                goods[i].Syubai = r.G_Syubai8;
                                break;

                            case 8:
                                goods[i].Code = r.G_Code9;
                                goods[i].Suu[0] = r.Goods9_1;
                                goods[i].Suu[1] = r.Goods9_2;
                                goods[i].Suu[2] = r.Goods9_3;
                                goods[i].Suu[3] = r.Goods9_4;
                                goods[i].Suu[4] = r.Goods9_5;
                                goods[i].Suu[5] = r.Goods9_6;
                                goods[i].Suu[6] = r.Goods9_7;
                                goods[i].Nouka = r.G_Nouka9;
                                goods[i].Baika = r.G_Baika9;
                                goods[i].Syubai = r.G_Syubai9;
                                break;

                            case 9:
                                goods[i].Code = r.G_Code10;
                                goods[i].Suu[0] = r.Goods10_1;
                                goods[i].Suu[1] = r.Goods10_2;
                                goods[i].Suu[2] = r.Goods10_3;
                                goods[i].Suu[3] = r.Goods10_4;
                                goods[i].Suu[4] = r.Goods10_5;
                                goods[i].Suu[5] = r.Goods10_6;
                                goods[i].Suu[6] = r.Goods10_7;
                                goods[i].Nouka = r.G_Nouka10;
                                goods[i].Baika = r.G_Baika10;
                                goods[i].Syubai = r.G_Syubai10;
                                break;

                            case 10:
                                goods[i].Code = r.G_Code11;
                                goods[i].Suu[0] = r.Goods11_1;
                                goods[i].Suu[1] = r.Goods11_2;
                                goods[i].Suu[2] = r.Goods11_3;
                                goods[i].Suu[3] = r.Goods11_4;
                                goods[i].Suu[4] = r.Goods11_5;
                                goods[i].Suu[5] = r.Goods11_6;
                                goods[i].Suu[6] = r.Goods11_7;
                                goods[i].Nouka = r.G_Nouka11;
                                goods[i].Baika = r.G_Baika11;
                                goods[i].Syubai = r.G_Syubai11;
                                break;

                            case 11:
                                goods[i].Code = r.G_Code12;
                                goods[i].Suu[0] = r.Goods12_1;
                                goods[i].Suu[1] = r.Goods12_2;
                                goods[i].Suu[2] = r.Goods12_3;
                                goods[i].Suu[3] = r.Goods12_4;
                                goods[i].Suu[4] = r.Goods12_5;
                                goods[i].Suu[5] = r.Goods12_6;
                                goods[i].Suu[6] = r.Goods12_7;
                                goods[i].Nouka = r.G_Nouka12;
                                goods[i].Baika = r.G_Baika12;
                                goods[i].Syubai = r.G_Syubai12;
                                break;

                            case 12:
                                goods[i].Code = r.G_Code13;
                                goods[i].Suu[0] = r.Goods13_1;
                                goods[i].Suu[1] = r.Goods13_2;
                                goods[i].Suu[2] = r.Goods13_3;
                                goods[i].Suu[3] = r.Goods13_4;
                                goods[i].Suu[4] = r.Goods13_5;
                                goods[i].Suu[5] = r.Goods13_6;
                                goods[i].Suu[6] = r.Goods13_7;
                                goods[i].Nouka = r.G_Nouka13;
                                goods[i].Baika = r.G_Baika13;
                                goods[i].Syubai = r.G_Syubai13;
                                break;

                            case 13:
                                goods[i].Code = r.G_Code14;
                                goods[i].Suu[0] = r.Goods14_1;
                                goods[i].Suu[1] = r.Goods14_2;
                                goods[i].Suu[2] = r.Goods14_3;
                                goods[i].Suu[3] = r.Goods14_4;
                                goods[i].Suu[4] = r.Goods14_5;
                                goods[i].Suu[5] = r.Goods14_6;
                                goods[i].Suu[6] = r.Goods14_7;
                                goods[i].Nouka = r.G_Nouka14;
                                goods[i].Baika = r.G_Baika14;
                                goods[i].Syubai = r.G_Syubai14;
                                break;

                            case 14:
                                goods[i].Code = r.G_Code15;
                                goods[i].Suu[0] = r.Goods15_1;
                                goods[i].Suu[1] = r.Goods15_2;
                                goods[i].Suu[2] = r.Goods15_3;
                                goods[i].Suu[3] = r.Goods15_4;
                                goods[i].Suu[4] = r.Goods15_5;
                                goods[i].Suu[5] = r.Goods15_6;
                                goods[i].Suu[6] = r.Goods15_7;
                                goods[i].Nouka = r.G_Nouka15;
                                goods[i].Baika = r.G_Baika15;
                                goods[i].Syubai = r.G_Syubai15;
                                break;

                            default:
                                break;
                        }
                    }

                    // 商品明細
                    for (int i = 0; i < global.MAX_GYO; i++)
                    {
                        if (goods[i].Code == string.Empty)
                        {
                            continue;
                        }

                        // 終売取消はネグる
                        if (goods[i].Syubai == global.SYUBAI_TORIKESHI)
                        {
                            continue;
                        }

                        int hCnt = 0;

                        // 発注数があるか？
                        for (int iX = 0; iX < 7; iX++)
                        {
                            hCnt += Utility.StrtoInt(goods[i].Suu[iX]);
                        }

                        // 発注数がなければネグる
                        if (hCnt == global.flgOff)
                        {
                            continue;
                        }

                        // 商品情報取得
                        ClsCsvData.ClsCsvSyohin syohin = Utility.GetSyohinData(goods[i].Code.PadLeft(8, '0'));

                        // 店着日別発注数
                        for (int iX = 0; iX < 7; iX++)
                        {
                            if (goods[i].Suu[iX] == string.Empty)
                            {
                                continue;
                            }

                            // 納品日取得
                            switch (iX)
                            {
                                case 0:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day1.PadLeft(2, '0');
                                    break;
                                case 1:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day2.PadLeft(2, '0');
                                    break;
                                case 2:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day3.PadLeft(2, '0');
                                    break;
                                case 3:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day4.PadLeft(2, '0');
                                    break;
                                case 4:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day5.PadLeft(2, '0');
                                    break;
                                case 5:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day6.PadLeft(2, '0');
                                    break;
                                case 6:
                                    cNouhinDT = r.Year + r.Month.ToString("D2") + r.Day7.PadLeft(2, '0');
                                    break;
                                default:
                                    break;
                            }

                            // ＣＳＶクラス配列追加
                            Array.Resize(ref csvDatas, dCnt + 1);
                            csvDatas[dCnt] = new ClsCsvData.ClsCsvCSV()
                            {
                                TOKUISAKI_CD = cTokuisakiCD,
                                TOKUISAKI_NM = cTokuisakiNM,
                                NOUHIN_DATE = cNouhinDT,
                                SYOHIN_CD = goods[i].Code.PadLeft(8, '0'),
                                SYOHIN_NM = syohin.SYOHIN_NM,
                                SUU = goods[i].Suu[iX],
                                NOUKA = goods[i].Nouka.ToString(),
                                BAIKA = goods[i].Baika.ToString(),
                                DT_KBN = global.DTKBN
                            };

                            // リストビューに表示
                            listBox1.Items.Add(cTokuisakiCD + " " + cTokuisakiNM + " " + cNouhinDT.Substring(0, 4) + "/" + cNouhinDT.Substring(4, 2) + "/" + cNouhinDT.Substring(6, 2) + " " +
                                goods[i].Code.PadLeft(8, '0') + " " + syohin.SYOHIN_NM + "(" + goods[i].Suu[iX] + ")");

                            listBox1.TopIndex = listBox1.Items.Count - 1;

                            // プログレスバー
                            progressBar1.Value = (rCnt + 1) * 100 / orderCnt;
                            //System.Threading.Thread.Sleep(10);
                            Application.DoEvents();

                            dCnt++;
                        }
                    }

                    rCnt++;
                }


                listBox1.Items.Add("終了しました..... 出力件数 " + dCnt.ToString("#,##0") + "件");

                listBox1.TopIndex = listBox1.Items.Count - 1;
                System.Threading.Thread.Sleep(500);
                Application.DoEvents();

                Cursor = Cursors.Default;

                if (csvDatas != null)
                {
                    // ファイルへ書き出し
                    CsvDataWrite(csvDatas);

                    // 終了メッセージ
                    MessageBox.Show(csvDatas.Length + "件のCSVデータ出力が終了しました", "処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return csvDatas.Length;
                }
                else
                {
                    return global.flgOff;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                if (csvDatas != null)
                {
                    return csvDatas.Length;
                }
                else
                {
                    return global.flgOff;
                }
            }
        }



        ///----------------------------------------------------------------------------
        /// <summary>
        ///     テキストファイルを出力する</summary>
        /// <param name="outFilePath">
        ///     出力するフォルダ</param>
        /// <param name="arrayData">
        ///     書き込む配列データ</param>
        ///----------------------------------------------------------------------------
        private void CsvDataWrite(ClsCsvData.ClsCsvCSV [] clsCsvs)
        {
            string[] arrayData = null;

            try
            {
                // ＣＳＶデータ配列作成
                for (int i = 0; i < clsCsvs.Length; i++)
                {
                    string str = clsCsvs[i].TOKUISAKI_CD + ",";
                    str += clsCsvs[i].TOKUISAKI_NM + ",";
                    str += clsCsvs[i].NOUHIN_DATE + ",";
                    str += clsCsvs[i].SYOHIN_CD + ",";
                    str += clsCsvs[i].SYOHIN_NM + ",";
                    str += clsCsvs[i].SUU + ",";
                    str += clsCsvs[i].NOUKA + ",";
                    str += clsCsvs[i].BAIKA + ",";
                    str += clsCsvs[i].DT_KBN;

                    Array.Resize(ref arrayData, i + 1);
                    arrayData[i] = str;
                }

                if (_FileAppend == global.flgOff)
                {
                    // 追加書き込み
                    System.IO.File.AppendAllLines(lblFileName.Text, arrayData, System.Text.Encoding.GetEncoding(932));
                }
                else
                {
                    // 上書き
                    System.IO.File.WriteAllLines(lblFileName.Text, arrayData, System.Text.Encoding.GetEncoding(932));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     CSVデータ作成履歴データ作成 </summary> 
        /// <param name="Cnt">
        ///     CSVデータ作成件数</param>
        ///----------------------------------------------------------
        private void CsvDataLogWrite(int Cnt)
        {
            // CSVデータ作成履歴
            cn.Open();

            DateTime nDate = DateTime.Now;
            string CreateDateTime = nDate.Year + "/" + nDate.Month.ToString("D2") + "/" + nDate.Day.ToString("D2") + " " +
                                    nDate.Hour.ToString("D2") + ":" + nDate.Minute.ToString("D2") + ":" + nDate.Second.ToString("D2");

            try
            {
                string sql = "INSERT INTO CsvOutHistory (";
                sql += "作成年月日時刻, コンピュータ名, 書き込みモード, 出力件数) ";
                sql += "VALUES ('";
                sql += CreateDateTime + "','";
                sql += System.Net.Dns.GetHostName() + "',";
                sql += _FileAppend + ",";
                sql +=  Cnt + ")";

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
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
            }
        }

        private void frmCsvDataCreate_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, Width, Height);
            Utility.WindowsMinSize(this, Width, Height);

            // データベース接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            // FAX発注書データ
            tblOrder = context.GetTable<Common.ClsOrder>();

            // 環境設定データ
            Table<Common.ClsSystemConfig> tblCnf = context.GetTable<Common.ClsSystemConfig>();

            var cnf = tblCnf.Single(a => a.ID == global.configKEY);
            //_sPath = cnf.DataPath;
            _FileAppend = cnf.FileWriteStatus;

            // プログレスバー初期設定
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Visible = false;

            // 付加文字列（タイムスタンプ）
            string newFileName = DateTime.Now.Year.ToString() +
                                 DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                 DateTime.Now.Day.ToString().PadLeft(2, '0');

            // ファイル名
            lblFileName.Text = cnf.DataPath + "FAX受注" + newFileName + ".csv";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 閉じる
            Close();
        }

        private void frmCsvDataCreate_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }
    }
}
