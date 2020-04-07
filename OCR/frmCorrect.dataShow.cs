using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.Linq;
using System.Data.SQLite;
using STSH_OCR.Common;
//using GrapeCity.Win.MultiRow;

namespace STSH_OCR.OCR
{
    partial class frmCorrect
    {
        #region 単位時間フィールド
        /// <summary> 
        ///     ３０分単位 </summary>
        private int tanMin30 = 30;

        /// <summary> 
        ///     １５分単位 </summary> 
        private int tanMin15 = 15;

        /// <summary> 
        ///     １０分単位 </summary> 
        private int tanMin10 = 10;

        /// <summary> 
        ///     １分単位 </summary>
        private int tanMin1 = 1;
        #endregion
        
        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     勤務票ヘッダと勤務票明細のデータセットにデータを読み込む </summary>
        ///------------------------------------------------------------------------------------
        private void getDataSet()
        {
            //fAdp.Fill(dtsC.FAX注文書);
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     データを画面に表示します </summary>
        /// <param name="iX">
        ///     ヘッダデータインデックス</param>
        ///------------------------------------------------------------------------------------
        private void showOcrData(int iX)
        {
            System.Diagnostics.Debug.WriteLine("発注書表示");

            Cursor = Cursors.WaitCursor;
            showStatus = false;

            // 非ログ書き込み状態とする
            editLogStatus = false;

            // フォーム初期化
            formInitialize(dID, iX);

            // 発注データを取得
            ClsFaxOrder = tblFax.Single(a => a.ID == cID[iX]);

            global.ChangeValueStatus = true;   // これ以下ChangeValueイベントを発生させない

            string Sql = "select * from FAX_Order WHERE ID = '" + cID[iX] + "'";

            using (SQLiteCommand com = new SQLiteCommand(Sql, cn2))
            {
                SQLiteDataReader dataReader = com.ExecuteReader();

                while (dataReader.Read())
                {
                    string sID = dataReader["ID"].ToString();
                    int sYear = Utility.StrtoInt(dataReader["年"].ToString());
                    int sMonth = Utility.StrtoInt(dataReader["月"].ToString());
                    int TkCD = Utility.StrtoInt(dataReader["得意先コード"].ToString());
                    int PtID = Utility.StrtoInt(dataReader["patternID"].ToString());
                    int Seq = Utility.StrtoInt(dataReader["SeqNumber"].ToString());

                    string sDay = dataReader["ID"].ToString().Substring(0, 8);
                    int cc = 0;

                    // 同じ発注書が存在するとき：Fax発注書
                    foreach (var t in tblFaxCheck.Where(a => a.TokuisakiCode == TkCD && a.Year == sYear && a.Month == sMonth &&
                                                             a.patternID == PtID && a.SeqNumber == Seq && a.ID != sID))
                    {
                        if (t.ID.Contains(sDay))
                        {
                            cc++;
                        }
                    }

                    if (cc > 0)
                    {
                        lblWarning.Text = "同じ発注書が他に" + cc + "件あります。 得意先：" + TkCD + "　発注書番号：" + PtID.ToString("D3") + Seq.ToString("D2") +
                                         "　年月：" + sYear + sMonth.ToString("D2") + "　受信日：" + sDay.Substring(0, 4) + "/" + sDay.Substring(4, 2) + "/" + sDay.Substring(6, 2);

                        lblWarning.Visible = true;
                    }

                    cc = 0;

                    // 同じ発注書が存在するとき：発注書データ
                    foreach (var t in tblOrder.Where(a => a.TokuisakiCode == TkCD && a.Year == sYear && a.Month == sMonth &&
                                                             a.patternID == PtID && a.SeqNumber == Seq && a.ID != sID))
                    {
                        if (t.ID.Contains(sDay))
                        {
                            cc++;
                        }
                    }

                    if (cc > 0)
                    {
                        lblWarning.Text = "発注書データに" + cc + "件、登録済みです。 得意先：" + TkCD + "　発注書番号：" + PtID.ToString("D3") + Seq.ToString("D2") +
                                         "　年月：" + sYear + sMonth.ToString("D2") + "　受信日：" + sDay.Substring(0, 4) + "/" + sDay.Substring(4, 2) + "/" + sDay.Substring(6, 2);

                        lblWarning.Visible = true;
                    }

                    // 画像表示
                    _img = Properties.Settings.Default.MyDataPath + dataReader["画像名"].ToString();

                    if (System.IO.File.Exists(_img))
                    {
                        showImage_openCv(_img);
                        trackBar1.Enabled = true;
                        btnLeft.Enabled = true;
                    }
                    else
                    {
                        pictureBox1.Image = null;
                        trackBar1.Enabled = false;
                        btnLeft.Enabled = false;
                    }                    

                    // ヘッダ情報
                    txtYear.Text = dataReader["年"].ToString();
                    txtMonth.Text = dataReader["月"].ToString();
                    txtTokuisakiCD.Text = dataReader["得意先コード"].ToString().PadLeft(7, '0'); ;
                    txtPID.Text = dataReader["patternID"].ToString();
                    txtSeqNum.Text = dataReader["SeqNumber"].ToString();

                    // 店着日
                    txtTenDay1.Text = dataReader["Day1"].ToString();
                    txtTenDay2.Text = dataReader["Day2"].ToString();
                    txtTenDay3.Text = dataReader["Day3"].ToString();
                    txtTenDay4.Text = dataReader["Day4"].ToString();
                    txtTenDay5.Text = dataReader["Day5"].ToString();
                    txtTenDay6.Text = dataReader["Day6"].ToString();
                    txtTenDay7.Text = dataReader["Day7"].ToString();

                    checkBox1.Checked = Convert.ToBoolean(Utility.StrtoInt(Utility.NulltoStr(dataReader["確認"])));
                    txtMemo.Text = dataReader["メモ"].ToString();

                    // 店着日配列作成
                    SetShowTenDate(tenDates);

                    //global.ChangeValueStatus = true;    // これ以下ChangeValueイベントを発生させる

                    // FAX発注書データ表示
                    showItem(dataReader, dg1);

                    // エラー情報表示初期化
                    lblErrMsg.Visible = false;
                    lblErrMsg.Text = string.Empty;


                    label3.Text = "[" + dataReader["ID"].ToString() + "]";
                }

                dataReader.Close();
            }

            //// 店着日配列を更新
            //SetShowTenDate(tenDates);

            // 店着日ロック
            DayLock(tenDates);

            showStatus = true;

            // 発注済み商品数表示コントロール
            for (int i = 0; i < tenDates.Length; i++)
            {
                int col = i + 6;

                for (int r = 1; r < dg1.RowCount; r += 2)
                {
                    ShowPastOrder(i, col, r);
                }
            }

            // ログ書き込み状態とする
            editLogStatus = true;

            Cursor = Cursors.Default;
        }

        ///-----------------------------------------------------------
        /// <summary>
        ///     店着日配列を作成する </summary>
        /// <param name="tenDates">
        ///     ClstenDate</param>
        ///-----------------------------------------------------------
        private void SetShowTenDate(ClsTenDate[] tenDates)
        {
            if (!TenDateStatus)
            {
                return;
            }

            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Day = txtTenDay1.Text;

            if (txtTenDay1.Text != string.Empty)
            {
                tenDates[0].Year = txtYear.Text.ToString();
                tenDates[0].Month = txtMonth.Text.ToString();
            }
            else
            {
                // 日付が無記入のときは年月も空白
                tenDates[0].Year = string.Empty;
                tenDates[0].Month = string.Empty;
            }

            tenDates[1].Day = txtTenDay2.Text.Trim();
            tenDates[2].Day = txtTenDay3.Text.Trim();
            tenDates[3].Day = txtTenDay4.Text.Trim();
            tenDates[4].Day = txtTenDay5.Text.Trim();
            tenDates[5].Day = txtTenDay6.Text.Trim();
            tenDates[6].Day = txtTenDay7.Text.Trim();

            int sYear = Utility.StrtoInt(txtYear.Text);
            int sMonth = Utility.StrtoInt(txtMonth.Text);
            bool NextMonth = false;
            string wDay = "";

            // 店着日付（年月日）をセット
            for (int i = 0; i < tenDates.Length; i++)
            {
                // 空白はネグる
                if (tenDates[i].Day == string.Empty)
                {
                    tenDates[i].Year = string.Empty;
                    tenDates[i].Month = string.Empty;
                    continue;
                }

                // 日付が若くなったら翌月扱い
                if (!NextMonth && Utility.StrtoInt(wDay) > Utility.StrtoInt(tenDates[i].Day))
                {
                    // ここから翌月
                    sMonth++;

                    if (sMonth > 12)
                    {
                        // 翌年
                        sMonth -= 12;
                        sYear++;
                    }

                    NextMonth = true;
                }

                if (tenDates[i].Day != string.Empty)
                {
                    tenDates[i].Year = sYear.ToString();
                    tenDates[i].Month = sMonth.ToString();
                }
                else
                {
                    tenDates[i].Year = string.Empty;
                    tenDates[i].Month = string.Empty;
                }

                wDay = tenDates[i].Day;

                // 該当テキストボックス
                TextBox box;

                switch (i)
                {
                    case 0:
                        box = txtTenDay1;
                        break;
                    case 1:
                        box = txtTenDay2;
                        break;
                    case 2:
                        box = txtTenDay3;
                        break;
                    case 3:
                        box = txtTenDay4;
                        break;
                    case 4:
                        box = txtTenDay5;
                        break;
                    case 5:
                        box = txtTenDay6;
                        break;
                    case 6:
                        box = txtTenDay7;
                        break;
                    default:
                        box = txtTenDay1;
                        break;
                }

                // 店着日テキストボックス文字色
                box.ForeColor = global.defaultColor;

                if (tenDates[i].Month != string.Empty)
                {
                    if (txtMonth.Text != tenDates[i].Month)
                    {
                        box.ForeColor = Color.Green;
                    }
                }
            }

            for (int i = 0; i < tenDates.Length; i++)
            {
                System.Diagnostics.Debug.WriteLine(tenDates[i].Year + "/" + tenDates[i].Month + "/" + tenDates[i].Day);
            }
        }

        private void SetTenDate(ClsTenDate [] tenDates)
        {
            int week = 0;

            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Day = txtTenDay1.Text;

            if (txtTenDay1.Text != string.Empty)
            {
                tenDates[0].Year = txtYear.ToString();
                tenDates[0].Month = txtMonth.ToString();
            }
            else
            {
                // 日付が無記入のときは年月も空白
                tenDates[0].Year = string.Empty;
                tenDates[0].Month = string.Empty;
            }

            tenDates[1].Day = txtTenDay2.Text.Trim();
            tenDates[2].Day = txtTenDay3.Text.Trim();
            tenDates[3].Day = txtTenDay4.Text.Trim();
            tenDates[4].Day = txtTenDay5.Text.Trim();
            tenDates[5].Day = txtTenDay6.Text.Trim();
            tenDates[6].Day = txtTenDay7.Text.Trim();

            int sYear = Utility.StrtoInt(txtYear.Text);
            int sMonth = Utility.StrtoInt(txtMonth.Text);

            // 店着日付（年月日）をセット
            for (int i = 1; i < tenDates.Length; i++)
            {
                if (tenDates[i].Day == string.Empty)
                {
                    // 日付無記入は年月も空白にしてネグる
                    tenDates[i].Year = string.Empty;
                    tenDates[i].Month = string.Empty;
                    continue;
                }

                // 曜日をセット
                switch (i)
                {
                    case 0:
                        week = global.Mon;
                        break;
                    case 1:
                        week = global.Tue;
                        break;
                    case 2:
                        week = global.Wed;
                        break;
                    case 3:
                        week = global.Thu;
                        break;
                    case 4:
                        week = global.Fri;
                        break;
                    case 5:
                        week = global.Sat;
                        break;
                    case 6:
                        week = global.Sun;
                        break;
                    default:
                        break;
                }

                tenDates[i].Year = sYear.ToString();
                tenDates[i].Month = sMonth.ToString();

                DateTime dt;
                DayOfWeek wk;

                if (DateTime.TryParse(tenDates[i].Year + "/" + tenDates[i].Month + "/" + tenDates[i].Day, out dt))
                {
                    // 曜日を確認
                    wk = dt.DayOfWeek;

                    if ((Int32)wk != week)
                    {
                        // 曜日が一致しないので翌月で検証
                        int yy = Utility.StrtoInt(tenDates[i].Year);
                        int mm = Utility.StrtoInt(tenDates[i].Month) + 1;

                        if (mm > 12)
                        {
                            yy++;
                            mm = 1;
                        }

                        if (DateTime.TryParse(yy + "/" + mm + "/" + tenDates[i].Day, out dt))
                        {
                            // 曜日を確認
                            wk = dt.DayOfWeek;

                            if ((Int32)wk == week)
                            {
                                // 曜日が一致したので翌月とみなす
                                tenDates[i].Year = yy.ToString();
                                tenDates[i].Month = mm.ToString();
                            }
                        }
                    }
                }
            }
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     発注書データにＦＡＸ発注パターン構成を読み込む </summary>
        /// <param name="clsPtn">
        ///     ClsOrderPattern </param>
        /// <param name="clsFax">
        ///     ClsFaxOrder </param>
        ///--------------------------------------------------------------------------
        private void PatternLoad(ClsOrderPattern clsPtn, ClsFaxOrder clsFax)
        {
            clsFax.G_Code1 = clsPtn.G_Code1;
            clsFax.G_Code2 = clsPtn.G_Code2;
            clsFax.G_Code3 = clsPtn.G_Code3;
            clsFax.G_Code4 = clsPtn.G_Code4;
            clsFax.G_Code5 = clsPtn.G_Code5;
            clsFax.G_Code6 = clsPtn.G_Code6;
            clsFax.G_Code7 = clsPtn.G_Code7;
            clsFax.G_Code8 = clsPtn.G_Code8;
            clsFax.G_Code9 = clsPtn.G_Code9;
            clsFax.G_Code10 = clsPtn.G_Code10;
            clsFax.G_Code11 = clsPtn.G_Code11;
            clsFax.G_Code12 = clsPtn.G_Code12;
            clsFax.G_Code13 = clsPtn.G_Code13;
            clsFax.G_Code14 = clsPtn.G_Code14;
            clsFax.G_Code15 = clsPtn.G_Code15;
            clsFax.G_Code16 = clsPtn.G_Code16;
            clsFax.G_Code17 = clsPtn.G_Code17;
            clsFax.G_Code18 = clsPtn.G_Code18;
            clsFax.G_Code19 = clsPtn.G_Code19;
            clsFax.G_Code20 = clsPtn.G_Code20;

            clsFax.G_Read1 = clsPtn.G_Read1;
            clsFax.G_Read2 = clsPtn.G_Read2;
            clsFax.G_Read3 = clsPtn.G_Read3;
            clsFax.G_Read4 = clsPtn.G_Read4;
            clsFax.G_Read5 = clsPtn.G_Read5;
            clsFax.G_Read6 = clsPtn.G_Read6;
            clsFax.G_Read7 = clsPtn.G_Read7;
            clsFax.G_Read8 = clsPtn.G_Read8;
            clsFax.G_Read9 = clsPtn.G_Read9;
            clsFax.G_Read10 = clsPtn.G_Read10;
            clsFax.G_Read11 = clsPtn.G_Read11;
            clsFax.G_Read12 = clsPtn.G_Read12;
            clsFax.G_Read13 = clsPtn.G_Read13;
            clsFax.G_Read14 = clsPtn.G_Read14;
            clsFax.G_Read15 = clsPtn.G_Read15;
            clsFax.G_Read16 = clsPtn.G_Read16;
            clsFax.G_Read17 = clsPtn.G_Read17;
            clsFax.G_Read18 = clsPtn.G_Read18;
            clsFax.G_Read19 = clsPtn.G_Read19;
            clsFax.G_Read20 = clsPtn.G_Read20;

            clsFax.PatternLoad = global.flgOn;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     発注商品表示 </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row</param>
        /// <param name="mr">
        ///     GcMultiRow</param>
        /// <param name="ptnNum">
        ///     パターンID</param>
        ///------------------------------------------------------------------------------------
        private void showItem_org(ClsFaxOrder r, DataGridView dataGrid)
        {
            global.ChangeValueStatus = false;

            // １行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 1].Value = r.G_Code1;
            global.ChangeValueStatus = false;

            if (r.G_Nouka1 != 0)
            {
                dataGrid[colNouka, 1].Value = r.G_Nouka1;
            }

            if (r.G_Baika1 != 0)
            {
                dataGrid[colBaika, 1].Value = r.G_Baika1;
            }

            dataGrid[colDay1, 1].Value = r.Goods1_1;
            dataGrid[colDay2, 1].Value = r.Goods1_2;
            dataGrid[colDay3, 1].Value = r.Goods1_3;
            dataGrid[colDay4, 1].Value = r.Goods1_4;
            dataGrid[colDay5, 1].Value = r.Goods1_5;
            dataGrid[colDay6, 1].Value = r.Goods1_6;
            dataGrid[colDay7, 1].Value = r.Goods1_7;

            dg1.Rows[1].Cells[colSyubai].Value = global.SyubaiArray[r.G_Syubai1];

            // ２行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 3].Value = r.G_Code2;
            global.ChangeValueStatus = false;

            if (r.G_Nouka2 != 0)
            {
                dataGrid[colNouka, 3].Value = r.G_Nouka2;
            }

            if (r.G_Baika2 != 0)
            {
                dataGrid[colBaika, 3].Value = r.G_Baika2;
            }

            dataGrid[colDay1, 3].Value = r.Goods2_1;
            dataGrid[colDay2, 3].Value = r.Goods2_2;
            dataGrid[colDay3, 3].Value = r.Goods2_3;
            dataGrid[colDay4, 3].Value = r.Goods2_4;
            dataGrid[colDay5, 3].Value = r.Goods2_5;
            dataGrid[colDay6, 3].Value = r.Goods2_6;
            dataGrid[colDay7, 3].Value = r.Goods2_7;

            dataGrid[colSyubai, 3].Value = global.SyubaiArray[r.G_Syubai2];

            // ３行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 5].Value = r.G_Code3;
            global.ChangeValueStatus = false;

            if (r.G_Nouka3 != 0)
            {
                dataGrid[colNouka, 5].Value = r.G_Nouka3;
            }

            if (r.G_Baika3 != 0)
            {
                dataGrid[colBaika, 5].Value = r.G_Baika3;
            }

            dataGrid[colDay1, 5].Value = r.Goods3_1;
            dataGrid[colDay2, 5].Value = r.Goods3_2;
            dataGrid[colDay3, 5].Value = r.Goods3_3;
            dataGrid[colDay4, 5].Value = r.Goods3_4;
            dataGrid[colDay5, 5].Value = r.Goods3_5;
            dataGrid[colDay6, 5].Value = r.Goods3_6;
            dataGrid[colDay7, 5].Value = r.Goods3_7;

            dataGrid[colSyubai, 5].Value = global.SyubaiArray[r.G_Syubai3];

            // ４行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 7].Value = r.G_Code4;
            global.ChangeValueStatus = false;

            if (r.G_Nouka4 != 0)
            {
                dataGrid[colNouka, 7].Value = r.G_Nouka4;
            }

            if (r.G_Baika4 != 0)
            {
                dataGrid[colBaika, 7].Value = r.G_Baika4;
            }

            dataGrid[colDay1, 7].Value = r.Goods4_1;
            dataGrid[colDay2, 7].Value = r.Goods4_2;
            dataGrid[colDay3, 7].Value = r.Goods4_3;
            dataGrid[colDay4, 7].Value = r.Goods4_4;
            dataGrid[colDay5, 7].Value = r.Goods4_5;
            dataGrid[colDay6, 7].Value = r.Goods4_6;
            dataGrid[colDay7, 7].Value = r.Goods4_7;

            dataGrid[colSyubai, 7].Value = global.SyubaiArray[r.G_Syubai4];

            // ５行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 9].Value = r.G_Code5;
            global.ChangeValueStatus = false;

            if (r.G_Nouka5 != 0)
            {
                dataGrid[colNouka, 9].Value = r.G_Nouka5;
            }

            if (r.G_Baika5 != 0)
            {
                dataGrid[colBaika, 9].Value = r.G_Baika5;
            }

            dataGrid[colDay1, 9].Value = r.Goods5_1;
            dataGrid[colDay2, 9].Value = r.Goods5_2;
            dataGrid[colDay3, 9].Value = r.Goods5_3;
            dataGrid[colDay4, 9].Value = r.Goods5_4;
            dataGrid[colDay5, 9].Value = r.Goods5_5;
            dataGrid[colDay6, 9].Value = r.Goods5_6;
            dataGrid[colDay7, 9].Value = r.Goods5_7;

            dataGrid[colSyubai, 9].Value = global.SyubaiArray[r.G_Syubai5];

            // ６行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 11].Value = r.G_Code6;
            global.ChangeValueStatus = false;

            if (r.G_Nouka6 != 0)
            {
                dataGrid[colNouka, 11].Value = r.G_Nouka6;
            }

            if (r.G_Baika6 != 0)
            {
                dataGrid[colBaika, 11].Value = r.G_Baika6;
            }

            dataGrid[colDay1, 11].Value = r.Goods6_1;
            dataGrid[colDay2, 11].Value = r.Goods6_2;
            dataGrid[colDay3, 11].Value = r.Goods6_3;
            dataGrid[colDay4, 11].Value = r.Goods6_4;
            dataGrid[colDay5, 11].Value = r.Goods6_5;
            dataGrid[colDay6, 11].Value = r.Goods6_6;
            dataGrid[colDay7, 11].Value = r.Goods6_7;

            dataGrid[colSyubai, 11].Value = global.SyubaiArray[r.G_Syubai6];

            // ７行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 13].Value = r.G_Code7;
            global.ChangeValueStatus = false;

            if (r.G_Nouka7 != 0)
            {
                dataGrid[colNouka, 13].Value = r.G_Nouka7;
            }

            if (r.G_Baika7 != 0)
            {
                dataGrid[colBaika, 13].Value = r.G_Baika7;
            }

            dataGrid[colDay1, 13].Value = r.Goods7_1;
            dataGrid[colDay2, 13].Value = r.Goods7_2;
            dataGrid[colDay3, 13].Value = r.Goods7_3;
            dataGrid[colDay4, 13].Value = r.Goods7_4;
            dataGrid[colDay5, 13].Value = r.Goods7_5;
            dataGrid[colDay6, 13].Value = r.Goods7_6;
            dataGrid[colDay7, 13].Value = r.Goods7_7;

            dataGrid[colSyubai, 13].Value = global.SyubaiArray[r.G_Syubai7];

            // ８行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 15].Value = r.G_Code8;
            global.ChangeValueStatus = false;

            if (r.G_Nouka8 != 0)
            {
                dataGrid[colNouka, 15].Value = r.G_Nouka8;
            }

            if (r.G_Baika8 != 0)
            {
                dataGrid[colBaika, 15].Value = r.G_Baika8;
            }

            dataGrid[colDay1, 15].Value = r.Goods8_1;
            dataGrid[colDay2, 15].Value = r.Goods8_2;
            dataGrid[colDay3, 15].Value = r.Goods8_3;
            dataGrid[colDay4, 15].Value = r.Goods8_4;
            dataGrid[colDay5, 15].Value = r.Goods8_5;
            dataGrid[colDay6, 15].Value = r.Goods8_6;
            dataGrid[colDay7, 15].Value = r.Goods8_7;

            dataGrid[colSyubai, 15].Value = global.SyubaiArray[r.G_Syubai8];

            // ９行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 17].Value = r.G_Code9;
            global.ChangeValueStatus = false;

            if (r.G_Nouka9 != 0)
            {
                dataGrid[colNouka, 17].Value = r.G_Nouka9;
            }

            if (r.G_Baika9 != 0)
            {
                dataGrid[colBaika, 17].Value = r.G_Baika9;
            }

            dataGrid[colDay1, 17].Value = r.Goods9_1;
            dataGrid[colDay2, 17].Value = r.Goods9_2;
            dataGrid[colDay3, 17].Value = r.Goods9_3;
            dataGrid[colDay4, 17].Value = r.Goods9_4;
            dataGrid[colDay5, 17].Value = r.Goods9_5;
            dataGrid[colDay6, 17].Value = r.Goods9_6;
            dataGrid[colDay7, 17].Value = r.Goods9_7;

            dataGrid[colSyubai, 17].Value = global.SyubaiArray[r.G_Syubai9];

            // 10行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 19].Value = r.G_Code10;
            global.ChangeValueStatus = false;

            if (r.G_Nouka10 != 0)
            {
                dataGrid[colNouka, 19].Value = r.G_Nouka10;
            }

            if (r.G_Baika10 != 0)
            {
                dataGrid[colBaika, 19].Value = r.G_Baika10;
            }

            dataGrid[colDay1, 19].Value = r.Goods10_1;
            dataGrid[colDay2, 19].Value = r.Goods10_2;
            dataGrid[colDay3, 19].Value = r.Goods10_3;
            dataGrid[colDay4, 19].Value = r.Goods10_4;
            dataGrid[colDay5, 19].Value = r.Goods10_5;
            dataGrid[colDay6, 19].Value = r.Goods10_6;
            dataGrid[colDay7, 19].Value = r.Goods10_7;

            dataGrid[colSyubai, 19].Value = global.SyubaiArray[r.G_Syubai10];

            // 11行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 21].Value = r.G_Code11;
            global.ChangeValueStatus = false;

            if (r.G_Nouka11 != 0)
            {
                dataGrid[colNouka, 21].Value = r.G_Nouka11;
            }

            if (r.G_Baika11 != 0)
            {
                dataGrid[colBaika, 21].Value = r.G_Baika11;
            }

            dataGrid[colDay1, 21].Value = r.Goods11_1;
            dataGrid[colDay2, 21].Value = r.Goods11_2;
            dataGrid[colDay3, 21].Value = r.Goods11_3;
            dataGrid[colDay4, 21].Value = r.Goods11_4;
            dataGrid[colDay5, 21].Value = r.Goods11_5;
            dataGrid[colDay6, 21].Value = r.Goods11_6;
            dataGrid[colDay7, 21].Value = r.Goods11_7;

            dataGrid[colSyubai, 21].Value = global.SyubaiArray[r.G_Syubai11];

            // 12行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 23].Value = r.G_Code12;
            global.ChangeValueStatus = false;

            if (r.G_Nouka12 != 0)
            {
                dataGrid[colNouka, 23].Value = r.G_Nouka12;
            }

            if (r.G_Baika12 != 0)
            {
                dataGrid[colBaika, 23].Value = r.G_Baika12;
            }

            dataGrid[colDay1, 23].Value = r.Goods12_1;
            dataGrid[colDay2, 23].Value = r.Goods12_2;
            dataGrid[colDay3, 23].Value = r.Goods12_3;
            dataGrid[colDay4, 23].Value = r.Goods12_4;
            dataGrid[colDay5, 23].Value = r.Goods12_5;
            dataGrid[colDay6, 23].Value = r.Goods12_6;
            dataGrid[colDay7, 23].Value = r.Goods12_7;

            dataGrid[colSyubai, 23].Value = global.SyubaiArray[r.G_Syubai12];

            // 13行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 25].Value = r.G_Code13;
            global.ChangeValueStatus = false;

            if (r.G_Nouka13 != 0)
            {
                dataGrid[colNouka, 25].Value = r.G_Nouka13;
            }

            if (r.G_Baika13 != 0)
            {
                dataGrid[colBaika, 25].Value = r.G_Baika13;
            }

            dataGrid[colDay1, 25].Value = r.Goods13_1;
            dataGrid[colDay2, 25].Value = r.Goods13_2;
            dataGrid[colDay3, 25].Value = r.Goods13_3;
            dataGrid[colDay4, 25].Value = r.Goods13_4;
            dataGrid[colDay5, 25].Value = r.Goods13_5;
            dataGrid[colDay6, 25].Value = r.Goods13_6;
            dataGrid[colDay7, 25].Value = r.Goods13_7;

            dataGrid[colSyubai, 25].Value = global.SyubaiArray[r.G_Syubai13];

            // 14行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 27].Value = r.G_Code14;
            global.ChangeValueStatus = false;

            if (r.G_Nouka14 != 0)
            {
                dataGrid[colNouka, 27].Value = r.G_Nouka14;
            }

            if (r.G_Baika14 != 0)
            {
                dataGrid[colBaika, 27].Value = r.G_Baika14;
            }

            dataGrid[colDay1, 27].Value = r.Goods14_1;
            dataGrid[colDay2, 27].Value = r.Goods14_2;
            dataGrid[colDay3, 27].Value = r.Goods14_3;
            dataGrid[colDay4, 27].Value = r.Goods14_4;
            dataGrid[colDay5, 27].Value = r.Goods14_5;
            dataGrid[colDay6, 27].Value = r.Goods14_6;
            dataGrid[colDay7, 27].Value = r.Goods14_7;

            dataGrid[colSyubai, 27].Value = global.SyubaiArray[r.G_Syubai14];

            // 15行目
            global.ChangeValueStatus = true;
            dataGrid[colHinCode, 29].Value = r.G_Code15;
            global.ChangeValueStatus = false;

            if (r.G_Nouka15 != 0)
            {
                dataGrid[colNouka, 29].Value = r.G_Nouka15;
            }

            if (r.G_Baika15 != 0)
            {
                dataGrid[colBaika, 29].Value = r.G_Baika15;
            }

            dataGrid[colDay1, 29].Value = r.Goods15_1;
            dataGrid[colDay2, 29].Value = r.Goods15_2;
            dataGrid[colDay3, 29].Value = r.Goods15_3;
            dataGrid[colDay4, 29].Value = r.Goods15_4;
            dataGrid[colDay5, 29].Value = r.Goods15_5;
            dataGrid[colDay6, 29].Value = r.Goods15_6;
            dataGrid[colDay7, 29].Value = r.Goods15_7;

            dataGrid[colSyubai, 29].Value = global.SyubaiArray[r.G_Syubai15];

            //カレントセル選択状態としない
            dg1.CurrentCell = null;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     発注商品表示 </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row</param>
        /// <param name="mr">
        ///     GcMultiRow</param>
        /// <param name="ptnNum">
        ///     パターンID</param>
        ///------------------------------------------------------------------------------------
        private void showItem(SQLiteDataReader r, DataGridView dataGrid)
        {
            global.ChangeValueStatus = false;
            bool g_Ptn = false;

            // 発注書パターン未読み込みの発注データのとき
            if (Utility.StrtoInt(r["パターンロード"].ToString()) == global.flgOff)
            {
                // 該当するＦＡＸ発注書パターンが存在するとき
                if (tblPtn.Any(a => a.TokuisakiCode == Utility.StrtoInt(r["得意先コード"].ToString()) &&
                                    a.SeqNum == Utility.StrtoInt(r["patternID"].ToString()) &&
                                    a.SecondNum == Utility.StrtoInt(r["SeqNumber"].ToString())))
                {
                    ClsOrderPattern = tblPtn.Single(a => a.TokuisakiCode == Utility.StrtoInt(r["得意先コード"].ToString()) &&
                                    a.SeqNum == Utility.StrtoInt(r["patternID"].ToString()) &&
                                    a.SecondNum == Utility.StrtoInt(r["SeqNumber"].ToString()));

                    // ＦＡＸ発注書パターンの商品構成とする
                    g_Ptn = true;
                }
            }

            // 商品発注明細クラス
            ClsGoods[] goods = new ClsGoods[15];
            for (int i = 0; i < 15; i++)
            {
                goods[i] = new ClsGoods();
                goods[i].Suu = new string[7];

                switch (i)
                {
                    case 0:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code1;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code1"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka1"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika1"].ToString());
                        goods[i].Suu[0] = r["Goods1_1"].ToString();
                        goods[i].Suu[1] = r["Goods1_2"].ToString();
                        goods[i].Suu[2] = r["Goods1_3"].ToString();
                        goods[i].Suu[3] = r["Goods1_4"].ToString();
                        goods[i].Suu[4] = r["Goods1_5"].ToString();
                        goods[i].Suu[5] = r["Goods1_6"].ToString();
                        goods[i].Suu[6] = r["Goods1_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai1"].ToString());

                        break;

                    case 1:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code2;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code2"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka2"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika2"].ToString());
                        goods[i].Suu[0] = r["Goods2_1"].ToString();
                        goods[i].Suu[1] = r["Goods2_2"].ToString();
                        goods[i].Suu[2] = r["Goods2_3"].ToString();
                        goods[i].Suu[3] = r["Goods2_4"].ToString();
                        goods[i].Suu[4] = r["Goods2_5"].ToString();
                        goods[i].Suu[5] = r["Goods2_6"].ToString();
                        goods[i].Suu[6] = r["Goods2_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai2"].ToString());

                        break;


                    case 2:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code3;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code3"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka3"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika3"].ToString());
                        goods[i].Suu[0] = r["Goods3_1"].ToString();
                        goods[i].Suu[1] = r["Goods3_2"].ToString();
                        goods[i].Suu[2] = r["Goods3_3"].ToString();
                        goods[i].Suu[3] = r["Goods3_4"].ToString();
                        goods[i].Suu[4] = r["Goods3_5"].ToString();
                        goods[i].Suu[5] = r["Goods3_6"].ToString();
                        goods[i].Suu[6] = r["Goods3_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai3"].ToString());

                        break;


                    case 3:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code4;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code4"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka4"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika4"].ToString());
                        goods[i].Suu[0] = r["Goods4_1"].ToString();
                        goods[i].Suu[1] = r["Goods4_2"].ToString();
                        goods[i].Suu[2] = r["Goods4_3"].ToString();
                        goods[i].Suu[3] = r["Goods4_4"].ToString();
                        goods[i].Suu[4] = r["Goods4_5"].ToString();
                        goods[i].Suu[5] = r["Goods4_6"].ToString();
                        goods[i].Suu[6] = r["Goods4_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai4"].ToString());

                        break;

                    case 4:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code5;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code5"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka5"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika5"].ToString());
                        goods[i].Suu[0] = r["Goods5_1"].ToString();
                        goods[i].Suu[1] = r["Goods5_2"].ToString();
                        goods[i].Suu[2] = r["Goods5_3"].ToString();
                        goods[i].Suu[3] = r["Goods5_4"].ToString();
                        goods[i].Suu[4] = r["Goods5_5"].ToString();
                        goods[i].Suu[5] = r["Goods5_6"].ToString();
                        goods[i].Suu[6] = r["Goods5_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai5"].ToString());

                        break;

                    case 5:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code6;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code6"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka6"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika6"].ToString());
                        goods[i].Suu[0] = r["Goods6_1"].ToString();
                        goods[i].Suu[1] = r["Goods6_2"].ToString();
                        goods[i].Suu[2] = r["Goods6_3"].ToString();
                        goods[i].Suu[3] = r["Goods6_4"].ToString();
                        goods[i].Suu[4] = r["Goods6_5"].ToString();
                        goods[i].Suu[5] = r["Goods6_6"].ToString();
                        goods[i].Suu[6] = r["Goods6_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai6"].ToString());

                        break;

                    case 6:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code7;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code7"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka7"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika7"].ToString());
                        goods[i].Suu[0] = r["Goods7_1"].ToString();
                        goods[i].Suu[1] = r["Goods7_2"].ToString();
                        goods[i].Suu[2] = r["Goods7_3"].ToString();
                        goods[i].Suu[3] = r["Goods7_4"].ToString();
                        goods[i].Suu[4] = r["Goods7_5"].ToString();
                        goods[i].Suu[5] = r["Goods7_6"].ToString();
                        goods[i].Suu[6] = r["Goods7_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai7"].ToString());

                        break;

                    case 7:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code8;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code8"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka8"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika8"].ToString());
                        goods[i].Suu[0] = r["Goods8_1"].ToString();
                        goods[i].Suu[1] = r["Goods8_2"].ToString();
                        goods[i].Suu[2] = r["Goods8_3"].ToString();
                        goods[i].Suu[3] = r["Goods8_4"].ToString();
                        goods[i].Suu[4] = r["Goods8_5"].ToString();
                        goods[i].Suu[5] = r["Goods8_6"].ToString();
                        goods[i].Suu[6] = r["Goods8_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai8"].ToString());

                        break;


                    case 8:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code9;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code9"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka9"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika9"].ToString());
                        goods[i].Suu[0] = r["Goods9_1"].ToString();
                        goods[i].Suu[1] = r["Goods9_2"].ToString();
                        goods[i].Suu[2] = r["Goods9_3"].ToString();
                        goods[i].Suu[3] = r["Goods9_4"].ToString();
                        goods[i].Suu[4] = r["Goods9_5"].ToString();
                        goods[i].Suu[5] = r["Goods9_6"].ToString();
                        goods[i].Suu[6] = r["Goods9_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai9"].ToString());

                        break;


                    case 9:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code10;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code10"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka10"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika10"].ToString());
                        goods[i].Suu[0] = r["Goods10_1"].ToString();
                        goods[i].Suu[1] = r["Goods10_2"].ToString();
                        goods[i].Suu[2] = r["Goods10_3"].ToString();
                        goods[i].Suu[3] = r["Goods10_4"].ToString();
                        goods[i].Suu[4] = r["Goods10_5"].ToString();
                        goods[i].Suu[5] = r["Goods10_6"].ToString();
                        goods[i].Suu[6] = r["Goods10_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai10"].ToString());

                        break;


                    case 10:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code11;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code11"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka11"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika11"].ToString());
                        goods[i].Suu[0] = r["Goods11_1"].ToString();
                        goods[i].Suu[1] = r["Goods11_2"].ToString();
                        goods[i].Suu[2] = r["Goods11_3"].ToString();
                        goods[i].Suu[3] = r["Goods11_4"].ToString();
                        goods[i].Suu[4] = r["Goods11_5"].ToString();
                        goods[i].Suu[5] = r["Goods11_6"].ToString();
                        goods[i].Suu[6] = r["Goods11_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai11"].ToString());

                        break;


                    case 11:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code12;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code12"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka12"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika12"].ToString());
                        goods[i].Suu[0] = r["Goods12_1"].ToString();
                        goods[i].Suu[1] = r["Goods12_2"].ToString();
                        goods[i].Suu[2] = r["Goods12_3"].ToString();
                        goods[i].Suu[3] = r["Goods12_4"].ToString();
                        goods[i].Suu[4] = r["Goods12_5"].ToString();
                        goods[i].Suu[5] = r["Goods12_6"].ToString();
                        goods[i].Suu[6] = r["Goods12_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai12"].ToString());

                        break;


                    case 12:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code13;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code13"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka13"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika13"].ToString());
                        goods[i].Suu[0] = r["Goods13_1"].ToString();
                        goods[i].Suu[1] = r["Goods13_2"].ToString();
                        goods[i].Suu[2] = r["Goods13_3"].ToString();
                        goods[i].Suu[3] = r["Goods13_4"].ToString();
                        goods[i].Suu[4] = r["Goods13_5"].ToString();
                        goods[i].Suu[5] = r["Goods13_6"].ToString();
                        goods[i].Suu[6] = r["Goods13_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai13"].ToString());

                        break;


                    case 13:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code14;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code14"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka14"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika14"].ToString());
                        goods[i].Suu[0] = r["Goods14_1"].ToString();
                        goods[i].Suu[1] = r["Goods14_2"].ToString();
                        goods[i].Suu[2] = r["Goods14_3"].ToString();
                        goods[i].Suu[3] = r["Goods14_4"].ToString();
                        goods[i].Suu[4] = r["Goods14_5"].ToString();
                        goods[i].Suu[5] = r["Goods14_6"].ToString();
                        goods[i].Suu[6] = r["Goods14_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai14"].ToString());

                        break;

                    case 14:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code15;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code15"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka15"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika15"].ToString());
                        goods[i].Suu[0] = r["Goods15_1"].ToString();
                        goods[i].Suu[1] = r["Goods15_2"].ToString();
                        goods[i].Suu[2] = r["Goods15_3"].ToString();
                        goods[i].Suu[3] = r["Goods15_4"].ToString();
                        goods[i].Suu[4] = r["Goods15_5"].ToString();
                        goods[i].Suu[5] = r["Goods15_6"].ToString();
                        goods[i].Suu[6] = r["Goods15_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai15"].ToString());

                        break;

                    default:
                        break;
                }

            }

            for (int i = 0; i < 15; i++)
            {
                global.ChangeValueStatus = true;
                dataGrid[colHinCode, i * 2 + 1].Value = goods[i].Code;
                global.ChangeValueStatus = false;

                if (goods[i].Nouka != 0)
                {
                    dataGrid[colNouka, i * 2 + 1].Value = goods[i].Nouka;
                }

                if (goods[i].Baika != 0)
                {
                    dataGrid[colBaika, i * 2 + 1].Value = goods[i].Baika;
                }

                global.ChangeValueStatus = true;

                dg1.Rows[i * 2 + 1].Cells[colSyubai].Value = global.SyubaiArray[goods[i].Syubai];

                //// 発注実績表示コントロールオン
                //showStatus = true;

                dataGrid[colDay1, i * 2 + 1].Value = goods[i].Suu[0];
                dataGrid[colDay2, i * 2 + 1].Value = goods[i].Suu[1];
                dataGrid[colDay3, i * 2 + 1].Value = goods[i].Suu[2];
                dataGrid[colDay4, i * 2 + 1].Value = goods[i].Suu[3];
                dataGrid[colDay5, i * 2 + 1].Value = goods[i].Suu[4];
                dataGrid[colDay6, i * 2 + 1].Value = goods[i].Suu[5];
                dataGrid[colDay7, i * 2 + 1].Value = goods[i].Suu[6];
                //global.ChangeValueStatus = false;
            }


            //カレントセル選択状態としない
            dg1.CurrentCell = null;

            //カレントセル選択状態としない
            dg1.CurrentCell = null;
        }



        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     画像を表示する </summary>
        /// <param name="pic">
        ///     pictureBoxオブジェクト</param>
        /// <param name="imgName">
        ///     イメージファイルパス</param>
        /// <param name="fX">
        ///     X方向のスケールファクター</param>
        /// <param name="fY">
        ///     Y方向のスケールファクター</param>
        ///------------------------------------------------------------------------------------
        private void ImageGraphicsPaint(PictureBox pic, string imgName, float fX, float fY, int RectDest, int RectSrc)
        {
            Image _img = Image.FromFile(imgName);
            Graphics g = Graphics.FromImage(pic.Image);

            // 各変換設定値のリセット
            g.ResetTransform();

            // X軸とY軸の拡大率の設定
            g.ScaleTransform(fX, fY);

            // 画像を表示する
            g.DrawImage(_img, RectDest, RectSrc);

            // 現在の倍率,座標を保持する
            gl.ZOOM_NOW = fX;
            gl.RECTD_NOW = RectDest;
            gl.RECTS_NOW = RectSrc;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     フォーム表示初期化 </summary>
        /// <param name="sID">
        ///     過去データ表示時のヘッダID</param>
        /// <param name="cIx">
        ///     勤務票ヘッダカレントレコードインデックス</param>
        ///------------------------------------------------------------------------------------
        private void formInitialize(string sID, int cIx)
        {

            global.ChangeValueStatus = false;   // これ以下ChangeValueイベントを発生させない
            TenDateStatus = false;

            // テキストボックス表示色設定
            txtYear.BackColor = Color.White;
            txtMonth.BackColor = Color.White;
            txtPID.BackColor = Color.White;
            txtSeqNum.BackColor = Color.White;
            txtTokuisakiCD.BackColor = Color.White;
            txtTenDay1.BackColor = Color.White;
            txtTenDay2.BackColor = Color.White;
            txtTenDay3.BackColor = Color.White;
            txtTenDay4.BackColor = Color.White;
            txtTenDay5.BackColor = Color.White;
            txtTenDay6.BackColor = Color.White;
            txtTenDay7.BackColor = Color.White;
            checkBox1.BackColor = SystemColors.Control;

            label1.Text = string.Empty;

            txtYear.ForeColor = global.defaultColor;
            txtMonth.ForeColor = global.defaultColor;
            txtPID.ForeColor = global.defaultColor;
            txtSeqNum.ForeColor = global.defaultColor;
            txtTokuisakiCD.ForeColor = global.defaultColor;
            txtTenDay1.ForeColor = global.defaultColor;
            txtTenDay2.ForeColor = global.defaultColor;
            txtTenDay3.ForeColor = global.defaultColor;
            txtTenDay4.ForeColor = global.defaultColor;
            txtTenDay5.ForeColor = global.defaultColor;
            txtTenDay6.ForeColor = global.defaultColor;
            txtTenDay7.ForeColor = global.defaultColor;
            txtMemo.ForeColor = global.defaultColor;

            // ヘッダ情報表示欄
            txtYear.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtPID.Text = string.Empty;
            txtSeqNum.Text = string.Empty;
            txtTokuisakiCD.Text = string.Empty;
            lblNoImage.Visible = false;

            txtTenDay1.Text = " ";
            txtTenDay2.Text = " ";
            txtTenDay3.Text = " ";
            txtTenDay4.Text = " ";
            txtTenDay5.Text = " ";
            txtTenDay6.Text = " ";
            txtTenDay7.Text = " ";

            dg1.Rows.Clear();   // 行数をクリア
            dg1.Rows.Add(30);   // 行数を設定

            // 編集不可行
            for (int i = 0; i < dg1.RowCount; i+= 2)
            {
                dg1[colNouka, i].ReadOnly = true;
                dg1[colBaika, i].ReadOnly = true;
                dg1[colDay1, i].ReadOnly = true;
                dg1[colDay2, i].ReadOnly = true;
                dg1[colDay3, i].ReadOnly = true;
                dg1[colDay4, i].ReadOnly = true;
                dg1[colDay5, i].ReadOnly = true;
                dg1[colDay6, i].ReadOnly = true;
                dg1[colDay7, i].ReadOnly = true;
                dg1[colSyubai, i].ReadOnly = true;
            }

            for (int i = 1; i < dg1.RowCount; i += 2)
            {
                if (i % 2 == 0)
                {
                    dg1[colNouka, i].ReadOnly = true;
                    dg1[colBaika, i].ReadOnly = true;
                    dg1[colDay1, i].ReadOnly = true;
                    dg1[colDay2, i].ReadOnly = true;
                    dg1[colDay3, i].ReadOnly = true;
                    dg1[colDay4, i].ReadOnly = true;
                    dg1[colDay5, i].ReadOnly = true;
                    dg1[colDay6, i].ReadOnly = true;
                    dg1[colDay7, i].ReadOnly = true;
                    dg1[colSyubai, i].ReadOnly = true;
                }

                dg1[colSyubai, i] = new DataGridViewComboBoxCell();
                dg1[colSyubai, i].ReadOnly = false; // 編集可能に設定

                // コンボボックスにアイテムを追加する
                for (int iX = 0; iX < global.SyubaiArray.Length; iX++)
                {
                    ((DataGridViewComboBoxCell)dg1[colSyubai, i]).Items.Add(global.SyubaiArray[iX]);
                }
            }

            for (int i = 0; i < dg1.Rows.Count; i += 4)
            {
                dg1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                dg1.Rows[i + 1].DefaultCellStyle.BackColor = Color.White;
            }

            for (int i = 2; i < dg1.Rows.Count; i+=4)
            {
                dg1.Rows[i].DefaultCellStyle.BackColor = Color.Lavender;
                dg1.Rows[i + 1].DefaultCellStyle.BackColor = Color.Lavender;
            }

            // メモ欄
            txtMemo.Text = string.Empty;

            // 確認チェック欄
            checkBox1.BackColor = SystemColors.Control;
            checkBox1.Checked = false;

            lblWarning.Visible = false;

            // データ編集のとき
            if (sID == string.Empty)
            {
                // ヘッダ情報
                txtYear.ReadOnly = false;
                txtMonth.ReadOnly = false;

                // スクロールバー設定
                hScrollBar1.Enabled = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = cID.Length - 1;
                hScrollBar1.Value = cIx;
                hScrollBar1.LargeChange = 1;
                hScrollBar1.SmallChange = 1;

                //移動ボタン制御
                btnFirst.Enabled = true;
                btnNext.Enabled = true;
                btnBefore.Enabled = true;
                btnEnd.Enabled = true;

                //最初のレコード
                if (cIx == 0)
                {
                    btnBefore.Enabled = false;
                    btnFirst.Enabled = false;
                }

                //最終レコード
                if ((cIx + 1) == cID.Length)
                {
                    btnNext.Enabled = false;
                    btnEnd.Enabled = false;
                }

                // その他のボタンを有効とする
                btnErrCheck.Visible = true;
                btnHold.Visible = true;
                btnDelete.Visible = true;
                btnPrint.Visible = true;
                btnData.Visible = true;

                ////エラー情報表示
                //ErrShow();

                //データ数表示
                lblPage.Text = " (" + (cIx + 1).ToString() + "/" + cID.Length + ")";
            }
            else
            {
                // ヘッダ情報
                txtYear.ReadOnly = true;
                txtMonth.ReadOnly = true;

                // スクロールバー設定
                hScrollBar1.Enabled = true;
                hScrollBar1.Minimum = 0;
                hScrollBar1.Maximum = 0;
                hScrollBar1.Value = 0;
                hScrollBar1.LargeChange = 1;
                hScrollBar1.SmallChange = 1;

                //移動ボタン制御
                btnFirst.Enabled = false;
                btnNext.Enabled = false;
                btnBefore.Enabled = false;
                btnEnd.Enabled = false;

                // その他のボタンを無効とする
                btnErrCheck.Visible = true;
                btnHold.Visible = true;
                btnDelete.Visible = true;
                btnPrint.Visible = true;
                btnData.Visible = true;

                //データ数表示
                lblPages.Text = string.Empty;
            }

            TenDateStatus = true;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     エラー表示 </summary>
        /// <param name="ocr">
        ///     OCRDATAクラス</param>
        ///------------------------------------------------------------------------------------
        private void ErrShow(OCRData ocr)
        {
            if (ocr._errNumber != ocr.eNothing)
            {
                // グリッドビューCellEnterイベント処理は実行しない
                gridViewCellEnterStatus = false;

                lblErrMsg.Visible = true;
                lblErrMsg.Text = ocr._errMsg;

                // 確認
                if (ocr._errNumber == ocr.eDataCheck)
                {
                    checkBox1.BackColor = Color.Yellow;
                    checkBox1.Focus();
                }

                // 年月
                if (ocr._errNumber == ocr.eYearMonth)
                {
                    txtYear.BackColor = Color.Yellow;
                    txtYear.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eMonth)
                {
                    txtMonth.BackColor = Color.Yellow;
                    txtMonth.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 得意先コード
                if (ocr._errNumber == ocr.eTdkNo)
                {
                    txtTokuisakiCD.BackColor = Color.Yellow;
                    txtTokuisakiCD.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // パターンID
                if (ocr._errNumber == ocr.ePattern)
                {
                    txtPID.BackColor = Color.Yellow;
                    txtPID.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 店着日付
                if (ocr._errNumber == ocr.eTenDate1)
                {
                    txtTenDay1.BackColor = Color.Yellow;
                    txtTenDay1.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate2)
                {
                    txtTenDay2.BackColor = Color.Yellow;
                    txtTenDay2.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate3)
                {
                    txtTenDay3.BackColor = Color.Yellow;
                    txtTenDay3.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate4)
                {
                    txtTenDay4.BackColor = Color.Yellow;
                    txtTenDay4.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate5)
                {
                    txtTenDay5.BackColor = Color.Yellow;
                    txtTenDay5.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate6)
                {
                    txtTenDay6.BackColor = Color.Yellow;
                    txtTenDay6.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate7)
                {
                    txtTenDay7.BackColor = Color.Yellow;
                    txtTenDay7.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }
                
                // 商品コード
                if (ocr._errNumber == ocr.eHinCode)
                {
                    dg1[colHinCode,  ocr._errRow - 1].Style.BackColor = Color.Yellow;
                    dg1[colHinCode,  ocr._errRow].Style.BackColor = Color.Yellow;
                    dg1.Focus();
                    dg1.CurrentCell = dg1[colHinCode, ocr._errRow];

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 発注数
                string col = "";
                for (int i = 0; i < 7; i++)
                {
                    if (ocr._errNumber == ocr.eSuu[i])
                    {
                        switch (i)
                        {
                            case 0:
                                col = colDay1;
                                break;
                            case 1:
                                col = colDay2;
                                break;
                            case 2:
                                col = colDay3;
                                break;
                            case 3:
                                col = colDay4;
                                break;
                            case 4:
                                col = colDay5;
                                break;
                            case 5:
                                col = colDay6;
                                break;
                            case 6:
                                col = colDay7;
                                break;
                            default:
                                break;
                        }
                        
                        dg1[col, ocr._errRow - 1].Style.BackColor = Color.Yellow;
                        dg1[col, ocr._errRow].Style.BackColor = Color.Yellow;
                        dg1.Focus();
                        dg1.CurrentCell = dg1[col, ocr._errRow];

                        // エラー有りフラグ
                        txtErrStatus.Text = global.FLGON;

                        break;
                    }
                }

                // 終売コンボボックス
                if (ocr._errNumber == ocr.eShubai)
                {
                    dg1[colSyubai, ocr._errRow - 1].Style.BackColor = Color.Yellow;
                    dg1.Focus();
                    dg1.CurrentCell = dg1[colSyubai, ocr._errRow];

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }


                // グリッドビューCellEnterイベントステータスを戻す
                gridViewCellEnterStatus = true;
            }
        }
    }
}
