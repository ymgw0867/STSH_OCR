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

        // 発注書データ
        Table<Common.ClsOrder> tblOrder = null;
        ClsOrder order = null;

        // 得意先別発注履歴データ：2020/04/12
        Table<ClsOrderHistory> tblOrderHistories = null;
        //ClsOrder order = null;

        // CSVデータ出力先
        string _sPath = "";

        // CSVデータ出力方法
        int _FileAppend = 0;

        // 納品単価マスタパス：2020/08/06
        string _NouhintankaMst = "";

        // 店着日付クラス
        ClsTenDate[] tenDates = new ClsTenDate[7];

        // 発注書ＣＳＶデータクラス
        ClsCsvData.ClsCsvCSV[] csvDatas = null;

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("CSVデータを出力します。よろしいですか？","確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (!CheckSameData())
            {
                if (MessageBox.Show("発注元、発注期間が同じと思われる発注書が確認されました。CSVデータを出力処理をすすめてよろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return;
                }
            }

            button1.Enabled = false;
            button2.Enabled = false;

            // CSVデータ出力処理
            int Cnt = CsvDataOutput();

            if (Cnt > 0)
            {
                // CSVデータ出力ログデータ書き込み
                CsvDataLogWrite(Cnt);

                // 発注データ削除
                DeleteOrderData();
            }

            // 終了メッセージ
            MessageBox.Show("処理が終了しました", "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            progressBar1.Visible = false;

            button2.Enabled = true;

            // 終了
            //Close();
        }

        ///-------------------------------------------------------
        /// <summary>
        ///     得意先別受注履歴データ登録 </summary>
        ///-------------------------------------------------------
        private void PutOrderHistory()
        {
            for (int i = 0; i < csvDatas.Length; i++)
            {
                string sql = "INSERT INTO OrderHistory (";
                sql += "得意先コード, 発注年月日, 商品コード, 数量, 更新年月日) ";
                sql += "VALUES (";
                sql += csvDatas[i].TOKUISAKI_CD + ",'";
                sql += csvDatas[i].NOUHIN_DATE + "','";
                sql += csvDatas[i].SYOHIN_CD + "',";
                sql += csvDatas[i].SUU + ",'";
                sql += DateTime.Now.ToString() + "')";

                using (SQLiteCommand com = new SQLiteCommand(sql, cn))
                {
                    com.ExecuteNonQuery();
                }
            }
        }


        ///------------------------------------------------------
        /// <summary>
        ///     発注データ削除 </summary>
        ///------------------------------------------------------
        private void DeleteOrderData()
        {
            Cursor = Cursors.WaitCursor;

            cn.Open();

            try
            {
                // 得意先別受注履歴データ登録
                PutOrderHistory();

                listBox1.Items.Add("得意先別受注履歴データを登録しました");
                listBox1.TopIndex = listBox1.Items.Count - 1;

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

                Cursor = Cursors.Default;
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

            progressBar1.Visible = true;
            int rCnt = 0;
            int dCnt = 0;
            
            try
            {
                Cursor = Cursors.WaitCursor;

                foreach (var r in tblOrder.OrderBy(a => a.ID))
                {
                    // 店着日付クラス配列作成
                    Utility.SetTenDate(tenDates, r);

                    string cTokuisakiCD = r.TokuisakiCode.ToString("D7");
                    string cTokuisakiNM = "";

                    // 2020/04/09
                    cTokuisakiNM = Utility.GetTokuisakiFromDataTable(cTokuisakiCD, global.dtTokuisaki).TOKUISAKI_NM;

                    string cNouhinDT = string.Empty;
                    string cDT = global.DTKBN;

                    // 商品発注明細クラス : 2020/04/12
                    ClsGoods[] goods = Utility.SetGoodsTabla(r, tenDates, tblOrderHistories);

                    // 商品明細
                    for (int i = 0; i < goods.Length; i++)
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

                        // コメント化 2020/04/12
                        //// 発注数があるか？
                        //for (int iX = 0; iX < 7; iX++)
                        //{
                        //    hCnt += Utility.StrtoInt(goods[i].Suu[iX]);
                        //}

                        //// 発注数がなければネグる
                        //if (hCnt == global.flgOff)
                        //{
                        //    continue;
                        //}

                        // 商品情報取得
                        ClsCsvData.ClsCsvSyohin_New syohin = Utility.GetSyohinsFromDataTable(global.dtSyohin, goods[i].Code.PadLeft(8, '0'));  // 2020/04/09

                        // 店着日別発注数
                        for (int iX = 0; iX < goods[i].Target.Length; iX++)
                        {
                            // 対象ステータスで判断：2020/04/13
                            if (!goods[i].Target[iX])
                            {
                                continue;
                            }

                            if (Utility.StrtoInt(goods[i].Suu[iX]) == global.flgOff)
                            {
                                // 発注数空白、０はネグる
                                continue;
                            }

                            // 納品日（店着日）
                            cNouhinDT = tenDates[iX].Year + tenDates[iX].Month.PadLeft(2, '0') + tenDates[iX].Day.PadLeft(2, '0');

                            // 納品単価取得
                            ClsCsvData.ClsCsvNouhinTanka clsTanka = GetNouhinTanka(goods[i].Code, cTokuisakiCD, Utility.StrtoInt(cNouhinDT), global.dtSyohinNoukaBaika);

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

                                // 納品単価マスタから取得 2020/08/06
                                NOUKA = clsTanka.NOUHIN_TANKA.ToString(),   // 2020/08/06
                                BAIKA = clsTanka.KOURI_TANKA.ToString(),    // 2020/08/06

                                // コメント化 2020/08/06
                                //NOUKA = goods[i].Nouka.ToString(),
                                //BAIKA = goods[i].Baika.ToString(),

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
            Cursor = Cursors.WaitCursor;

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

                Cursor = Cursors.Default;
            }
        }

        private void frmCsvDataCreate_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, Width, Height);
            Utility.WindowsMinSize(this, Width, Height);

            // データベース接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            // 発注書データ
            tblOrder = context.GetTable<Common.ClsOrder>();
            int Maisu = tblOrder.Count();
            lblMaisu.Text = Maisu.ToString("N0");

            // 得意先別発注履歴：2020/04/12
            tblOrderHistories = context.GetTable<Common.ClsOrderHistory>();

            if (Maisu > 0)
            {
                button1.Enabled = true;
                lblMsg.Visible = false;
            }
            else
            {
                button1.Enabled = false;
                lblMsg.Visible = true;
            }

            // 環境設定データ
            Table<Common.ClsSystemConfig> tblCnf = context.GetTable<Common.ClsSystemConfig>();

            var cnf = tblCnf.Single(a => a.ID == global.configKEY);
            //_sPath = cnf.DataPath;
            _FileAppend = cnf.FileWriteStatus;
            _NouhintankaMst = cnf.NouhinTankaPath;  // 納品単価マスタパス 2020/08/06

            // 納品単価マスタパスをデータセットに読み込む：2020/08/06
            global.dtSyohinNoukaBaika = Utility.readCSV(_NouhintankaMst);

            // 2020/04/08 コメント化
            //// 商品マスタークラス配列作成
            //syohins = Utility.GetSyohinData(Properties.Settings.Default.商品マスター, Properties.Settings.Default.商品在庫マスター, Properties.Settings.Default.仕入先マスター);

            // 2020/04/08 コメント化
            //// 得意先マスタークラス配列作成
            //string[] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);
            //int sDate = DateTime.Today.Year * 10000 + DateTime.Today.Month * 100 + DateTime.Today.Day;
            //tokuisakis = ClsCsvData.ClsCsvTokuisaki.Load(Tk_Array, sDate);

            // プログレスバー初期設定
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Visible = false;

            // 付加文字列（タイムスタンプ）
            string newFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') +
                                 DateTime.Now.Day.ToString().PadLeft(2, '0');

            // ファイル名
            lblFileName.Text = cnf.DataPath + "FAX受注" + newFileName + ".csv";
        }

        private bool CheckSameData()
        {
            faxData[] idArray = new faxData[tblOrder.Count()];
            int cnt = 0;

            foreach (var t in tblOrder.OrderBy(a => a.ID))
            {
                idArray[cnt] = new faxData();
                idArray[cnt].ID = t.ID;
                idArray[cnt].Year = t.Year;
                idArray[cnt].Month = t.Month;
                idArray[cnt].patternID = t.patternID;
                idArray[cnt].SeqNumber = t.SeqNumber;
                idArray[cnt].Day1 = t.Day1;

                cnt++;
            }

            bool rtn = true;

            for (int i = 0; i < idArray.Length; i++)
            {
                var s = tblOrder.Where(a => a.Year == idArray[i].Year && a.Month == idArray[i].Month && a.patternID == idArray[i].patternID && a.SeqNumber == idArray[i].SeqNumber &&
                                            a.Day1 == idArray[i].Day1 && a.ID != idArray[i].ID);

                foreach (var it in s)
                {
                    // リストビューに表示
                    listBox1.Items.Add("重複発注書 : " + it.Year + "年" + it.Month + "月 得意先コード : " + it.TokuisakiCode + " 発注書番号 : " + it.patternID.ToString("D3") +
                                       it.SeqNumber.ToString("D2") + " 店着日 : " + it.Day1 + "～");

                    listBox1.TopIndex = listBox1.Items.Count - 1;
                    //System.Threading.Thread.Sleep(10);
                    Application.DoEvents();

                    rtn = false;
                }
            }

            return rtn;

        }

        private class faxData
        {
            public string ID { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public int TokuisakiCode { get; set; }
            public int patternID { get; set; }
            public int SeqNumber { get; set; }
            public string Day1 { get; set; }
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

        ///--------------------------------------------------------------------
        /// <summary>
        ///     納品単価マスタを取得する </summary>
        /// <param name="SyohinCD">
        ///     商品コード</param>
        /// <param name="TokuisakiCD">
        ///     得意先コード</param>
        /// <param name="TenDate">
        ///     店着日付</param>
        /// <param name="data">
        ///     納品単価マスタデータセット</param>
        /// <returns>
        ///     納品単価マスタクラス</returns>
        ///--------------------------------------------------------------------
        private ClsCsvData.ClsCsvNouhinTanka GetNouhinTanka(string SyohinCD, string TokuisakiCD, int TenDate, System.Data.DataTable data)
        {
            ClsCsvData.ClsCsvNouhinTanka clsCsvNouhin = new ClsCsvData.ClsCsvNouhinTanka
            {
                SYOHIN_CD = "",
                SPECIAL_VAL_KBN = "",
                YUKO_START_YMD = "",
                YUKO_END_YMD = "",
                TOKUISAKI_CD = "",
                NOUHIN_TANKA = 0,
                KOURI_TANKA = 0,
                DELFLG = 0
            };

            DataRow[] rows = null;

            //// 定番
            //if (SpecialVal == global.TANKA_TEIBAN)
            //{
            //    rows = data.AsEnumerable().Where(a => a["SPECIAL_VAL_KBN"].ToString() == SpecialVal && 
            //                                          a["TOKUISAKI_CD"].ToString().PadLeft(7, '0') == TokuisakiCD &&
            //                                          a["SYOHIN_CD"].ToString().PadLeft(8, '0') == SyohinCD &&
            //                                          a["DELFLG"].ToString() == global.FLGOFF).ToArray();
            //}

            // 特売
            rows = data.AsEnumerable().Where(a => a["SPECIAL_VAL_KBN"].ToString() == global.TANKA_TOKUBAI &&
                                                  a["TOKUISAKI_CD"].ToString().PadLeft(7, '0') == TokuisakiCD &&
                                                  a["SYOHIN_CD"].ToString().PadLeft(8, '0') == SyohinCD &&
                                                  Utility.StrtoInt(a["YUKO_START_YMD"].ToString()) <= TenDate &&
                                                  Utility.StrtoInt(a["YUKO_END_YMD"].ToString()) >= TenDate &&
                                                  a["DELFLG"].ToString() == global.FLGOFF)
                                      .OrderByDescending(a => Utility.StrtoInt(a["YUKO_START_YMD"].ToString()))
                                      .ToArray();

            foreach (var t in rows)
            {
                clsCsvNouhin.SYOHIN_CD = t["SYOHIN_CD"].ToString();
                clsCsvNouhin.SPECIAL_VAL_KBN = t["SPECIAL_VAL_KBN"].ToString();
                clsCsvNouhin.YUKO_START_YMD = t["YUKO_START_YMD"].ToString();
                clsCsvNouhin.YUKO_END_YMD = t["YUKO_END_YMD"].ToString();
                clsCsvNouhin.TOKUISAKI_CD = t["TOKUISAKI_CD"].ToString();
                clsCsvNouhin.NOUHIN_TANKA = Utility.StrtoDouble(t["NOUHIN_TANKA"].ToString());
                clsCsvNouhin.KOURI_TANKA = Utility.StrtoDouble(t["KOURI_TANKA"].ToString());
                break;
            }

            // 該当する特売データがなかったとき
            if (clsCsvNouhin.SYOHIN_CD == "")
            {
                // 定番
                rows = data.AsEnumerable().Where(a => a["SPECIAL_VAL_KBN"].ToString() == global.TANKA_TEIBAN &&
                                                      a["TOKUISAKI_CD"].ToString().PadLeft(7, '0') == TokuisakiCD &&
                                                      a["SYOHIN_CD"].ToString().PadLeft(8, '0') == SyohinCD &&
                                                      a["DELFLG"].ToString() == global.FLGOFF).ToArray();

                foreach (var t in rows)
                {
                    clsCsvNouhin.SYOHIN_CD = t["SYOHIN_CD"].ToString();
                    clsCsvNouhin.SPECIAL_VAL_KBN = t["SPECIAL_VAL_KBN"].ToString();
                    clsCsvNouhin.YUKO_START_YMD = t["YUKO_START_YMD"].ToString();
                    clsCsvNouhin.YUKO_END_YMD = t["YUKO_END_YMD"].ToString();
                    clsCsvNouhin.TOKUISAKI_CD = t["TOKUISAKI_CD"].ToString();
                    clsCsvNouhin.NOUHIN_TANKA = Utility.StrtoDouble(t["NOUHIN_TANKA"].ToString());
                    clsCsvNouhin.KOURI_TANKA = Utility.StrtoDouble(t["KOURI_TANKA"].ToString());
                    break;
                }
            }

            return clsCsvNouhin;
        }
    }
}
