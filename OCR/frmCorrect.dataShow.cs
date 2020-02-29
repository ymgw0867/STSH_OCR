using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.OleDb;
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
            Cursor = Cursors.WaitCursor;
            showStatus = true;

            // 非ログ書き込み状態とする
            editLogStatus = false;

            // 発注データを取得
            ClsFaxOrder = tblFax.Single(a => a.ID == cID[iX]);

            // フォーム初期化
            formInitialize(dID, iX);

            global.ChangeValueStatus = false;   // これ以下ChangeValueイベントを発生させない

            txtYear.Text = ClsFaxOrder.Year.ToString();
            txtMonth.Text = Utility.EmptytoZero(ClsFaxOrder.Month.ToString());
            txtTokuisakiCD.Text = ClsFaxOrder.TokuisakiCode.ToString("D7");
            txtPID.Text = ClsFaxOrder.patternID.ToString();
            txtSeqNum.Text = ClsFaxOrder.SeqNumber.ToString();

            // 店着日
            txtTenDay1.Text = ClsFaxOrder.Day1.ToString();
            txtTenDay2.Text = ClsFaxOrder.Day2.ToString();
            txtTenDay3.Text = ClsFaxOrder.Day3.ToString();
            txtTenDay4.Text = ClsFaxOrder.Day4.ToString();
            txtTenDay5.Text = ClsFaxOrder.Day5.ToString();
            txtTenDay6.Text = ClsFaxOrder.Day6.ToString();
            txtTenDay7.Text = ClsFaxOrder.Day7.ToString();

            checkBox1.Checked = Convert.ToBoolean(ClsFaxOrder.Veri);
            txtMemo.Text = ClsFaxOrder.memo;

            global.ChangeValueStatus = true;    // これ以下ChangeValueイベントを発生させる

            // 発注書パターン未読み込みの発注データのとき
            if (ClsFaxOrder.PatternLoad == global.flgOff)
            {
                // 該当するＦＡＸ発注書パターンが存在するとき
                if (tblPtn.Any(a => a.TokuisakiCode == ClsFaxOrder.TokuisakiCode &&
                                a.SeqNum == ClsFaxOrder.patternID && a.SecondNum == ClsFaxOrder.SeqNumber))
                {
                    ClsOrderPattern = tblPtn.Single(a => a.TokuisakiCode == ClsFaxOrder.TokuisakiCode &&
                                    a.SeqNum == ClsFaxOrder.patternID && a.SecondNum == ClsFaxOrder.SeqNumber);

                    // ＦＡＸ発注書パターンの商品構成とする
                    PatternLoad(ClsOrderPattern, ClsFaxOrder);
                }
            }

            // FAX発注書データ表示
            showItem(ClsFaxOrder, dg1);

            //// 月間合計値表示
            //getMonthTotal();

            // エラー情報表示初期化
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;

            // 画像表示
            _img = Properties.Settings.Default.MyDataPath + ClsFaxOrder.ImageFileName.ToString();
            showImage_openCv(_img);
            trackBar1.Enabled = true;

            // ログ書き込み状態とする
            editLogStatus = true;

            showStatus = false;
            Cursor = Cursors.Default;
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
        private void showItem(ClsFaxOrder r, DataGridView dataGrid)
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


            //// 編集を可能とする
            //mr.ReadOnly = false;

            //// パターン登録のとき
            //if (ptnNum != global.flgOff)
            //{
            //    /* 商品パターンが登録されていない欄の発注数
            //       有効数字あり：編集可（要訂正） 
            //       有効数字なし：編集不可 */
            //    for (int i = 0; i < gcMultiRow2.Rows.Count; i++)
            //    {
            //        if (Utility.NulltoStr(gcMultiRow2[i, "txtHinCode"].Value) == string.Empty &&
            //            Utility.NulltoStr(gcMultiRow2[i, "txtSuu"].Value) == string.Empty)
            //        {
            //            gcMultiRow2[i, "txtSuu"].ReadOnly = true;
            //            //gcMultiRow2[i, "txtSuu"].Selectable = false;
            //        }
            //        else
            //        {
            //            gcMultiRow2[i, "txtSuu"].ReadOnly = false;
            //            //gcMultiRow2[i, "txtSuu"].Selectable = true;
            //        }

            //        if (Utility.NulltoStr(gcMultiRow2[i, "txtHinCode2"].Value) == string.Empty &&
            //            Utility.NulltoStr(gcMultiRow2[i, "txtSuu2"].Value) == string.Empty)
            //        {
            //            gcMultiRow2[i, "txtSuu2"].ReadOnly = true;
            //            //gcMultiRow2[i, "txtSuu2"].Selectable = false;
            //        }
            //        else
            //        {
            //            gcMultiRow2[i, "txtSuu2"].ReadOnly = false;
            //            //gcMultiRow2[i, "txtSuu2"].Selectable = true;
            //        }

            //        // 2017/08/23
            //        gcMultiRow2[i, "txtHinCode"].ReadOnly = true;
            //        gcMultiRow2[i, "txtSuu"].ReadOnly = false;
            //        gcMultiRow2[i, "txtHinCode2"].ReadOnly = true;
            //        gcMultiRow2[i, "txtSuu2"].ReadOnly = false;

            //        // 注文数欄背景色初期化
            //        gcMultiRow2[i, "txtHinCode"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtSuu"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtHinCode2"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtSuu2"].Style.BackColor = Color.Empty;
            //    }
            //}
            //else
            //{
            //    // フリー入力のとき
            //    gl.ChangeValueStatus = true;

            //    if (r.Is商品コード1Null())
            //    {
            //        mr.SetValue(0, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(0, "txtHinCode", r.商品コード1);
            //    }

            //    if (r.Is商品コード2Null())
            //    {
            //        mr.SetValue(1, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(1, "txtHinCode", r.商品コード2);
            //    }

            //    if (r.Is商品コード3Null())
            //    {
            //        mr.SetValue(2, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(2, "txtHinCode", r.商品コード3);
            //    }

            //    if (r.Is商品コード4Null())
            //    {
            //        mr.SetValue(3, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(3, "txtHinCode", r.商品コード4);
            //    }

            //    if (r.Is商品コード5Null())
            //    {
            //        mr.SetValue(4, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(4, "txtHinCode", r.商品コード5);
            //    }

            //    if (r.Is商品コード6Null())
            //    {
            //        mr.SetValue(5, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(5, "txtHinCode", r.商品コード6);
            //    }

            //    if (r.Is商品コード7Null())
            //    {
            //        mr.SetValue(6, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(6, "txtHinCode", r.商品コード7);
            //    }

            //    if (r.Is商品コード8Null())
            //    {
            //        mr.SetValue(7, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(7, "txtHinCode", r.商品コード8);
            //    }

            //    if (r.Is商品コード9Null())
            //    {
            //        mr.SetValue(8, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(8, "txtHinCode", r.商品コード9);
            //    }

            //    if (r.Is商品コード10Null())
            //    {
            //        mr.SetValue(9, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(9, "txtHinCode", r.商品コード10);
            //    }

            //    if (r.Is商品コード11Null())
            //    {
            //        mr.SetValue(10, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(10, "txtHinCode", r.商品コード11);
            //    }

            //    if (r.Is商品コード12Null())
            //    {
            //        mr.SetValue(11, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(11, "txtHinCode", r.商品コード12);
            //    }

            //    if (r.Is商品コード13Null())
            //    {
            //        mr.SetValue(12, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(12, "txtHinCode", r.商品コード13);
            //    }

            //    if (r.Is商品コード14Null())
            //    {
            //        mr.SetValue(13, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(13, "txtHinCode", r.商品コード14);
            //    }

            //    if (r.Is商品コード15Null())
            //    {
            //        mr.SetValue(14, "txtHinCode", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(14, "txtHinCode", r.商品コード15);
            //    }

            //    if (r.Is商品コード16Null())
            //    {
            //        mr.SetValue(0, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(0, "txtHinCode2", r.商品コード16);
            //    }

            //    if (r.Is商品コード17Null())
            //    {
            //        mr.SetValue(1, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(1, "txtHinCode2", r.商品コード17);
            //    }

            //    if (r.Is商品コード18Null())
            //    {
            //        mr.SetValue(2, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(2, "txtHinCode2", r.商品コード18);
            //    }

            //    if (r.Is商品コード19Null())
            //    {
            //        mr.SetValue(3, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(3, "txtHinCode2", r.商品コード19);
            //    }

            //    if (r.Is商品コード20Null())
            //    {
            //        mr.SetValue(4, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(4, "txtHinCode2", r.商品コード20);
            //    }

            //    if (r.Is商品コード21Null())
            //    {
            //        mr.SetValue(5, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(5, "txtHinCode2", r.商品コード21);
            //    }

            //    if (r.Is商品コード22Null())
            //    {
            //        mr.SetValue(6, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(6, "txtHinCode2", r.商品コード22);
            //    }

            //    if (r.Is商品コード23Null())
            //    {
            //        mr.SetValue(7, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(7, "txtHinCode2", r.商品コード23);
            //    }

            //    if (r.Is商品コード24Null())
            //    {
            //        mr.SetValue(8, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(8, "txtHinCode2", r.商品コード24);
            //    }

            //    if (r.Is商品コード25Null())
            //    {
            //        mr.SetValue(9, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(9, "txtHinCode2", r.商品コード25);
            //    }

            //    if (r.Is商品コード26Null())
            //    {
            //        mr.SetValue(10, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(10, "txtHinCode2", r.商品コード26);
            //    }

            //    if (r.Is商品コード27Null())
            //    {
            //        mr.SetValue(11, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(11, "txtHinCode2", r.商品コード27);
            //    }

            //    if (r.Is商品コード28Null())
            //    {
            //        mr.SetValue(12, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(12, "txtHinCode2", r.商品コード28);
            //    }

            //    if (r.Is商品コード29Null())
            //    {
            //        mr.SetValue(13, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(13, "txtHinCode2", r.商品コード29);
            //    }

            //    if (r.Is商品コード30Null())
            //    {
            //        mr.SetValue(14, "txtHinCode2", "");
            //    }
            //    else
            //    {
            //        mr.SetValue(14, "txtHinCode2", r.商品コード30);
            //    }

            //    gl.ChangeValueStatus = false;

            //    // 2017/08/23
            //    for (int i = 0; i < gcMultiRow2.Rows.Count; i++)
            //    {
            //        gcMultiRow2[i, "txtHinCode"].ReadOnly = false;
            //        gcMultiRow2[i, "txtSuu"].ReadOnly = false;
            //        gcMultiRow2[i, "txtHinCode2"].ReadOnly = false;
            //        gcMultiRow2[i, "txtSuu2"].ReadOnly = false;

            //        // 注文数欄背景色初期化
            //        gcMultiRow2[i, "txtHinCode"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtSuu"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtHinCode2"].Style.BackColor = Color.Empty;
            //        gcMultiRow2[i, "txtSuu2"].Style.BackColor = Color.Empty;
            //    }
            //}

            ////mr.EndEdit();

            //カレントセル選択状態としない
            dg1.CurrentCell = null;
        }

        //private void ptnShow(GcMultiRow mr, int tdkCode, int ptnCode)
        //{
        //    gl.ChangeValueStatus = true;

        //    if (dts.パターンID.Any(a => a.届先番号 == tdkCode && a.連番 == ptnCode))
        //    {
        //        var s = dts.パターンID.Single(a => a.届先番号 == tdkCode && a.連番 == ptnCode);

        //        mr.SetValue(0, "txtHinCode", Utility.ptnShohinStr(s.商品1));
        //        mr.SetValue(1, "txtHinCode", Utility.ptnShohinStr(s.商品2));
        //        mr.SetValue(2, "txtHinCode", Utility.ptnShohinStr(s.商品3));
        //        mr.SetValue(3, "txtHinCode", Utility.ptnShohinStr(s.商品4));
        //        mr.SetValue(4, "txtHinCode", Utility.ptnShohinStr(s.商品5));
        //        mr.SetValue(5, "txtHinCode", Utility.ptnShohinStr(s.商品6));
        //        mr.SetValue(6, "txtHinCode", Utility.ptnShohinStr(s.商品7));
        //        mr.SetValue(7, "txtHinCode", Utility.ptnShohinStr(s.商品8));
        //        mr.SetValue(8, "txtHinCode", Utility.ptnShohinStr(s.商品9));
        //        mr.SetValue(9, "txtHinCode", Utility.ptnShohinStr(s.商品10));
        //        mr.SetValue(10, "txtHinCode", Utility.ptnShohinStr(s.商品11));
        //        mr.SetValue(11, "txtHinCode", Utility.ptnShohinStr(s.商品12));
        //        mr.SetValue(12, "txtHinCode", Utility.ptnShohinStr(s.商品13));
        //        mr.SetValue(13, "txtHinCode", Utility.ptnShohinStr(s.商品14));
        //        mr.SetValue(14, "txtHinCode", Utility.ptnShohinStr(s.商品15));

        //        mr.SetValue(0, "txtHinCode2", Utility.ptnShohinStr(s.商品16));
        //        mr.SetValue(1, "txtHinCode2", Utility.ptnShohinStr(s.商品17));
        //        mr.SetValue(2, "txtHinCode2", Utility.ptnShohinStr(s.商品18));
        //        mr.SetValue(3, "txtHinCode2", Utility.ptnShohinStr(s.商品19));
        //        mr.SetValue(4, "txtHinCode2", Utility.ptnShohinStr(s.商品20));
        //        mr.SetValue(5, "txtHinCode2", Utility.ptnShohinStr(s.商品21));
        //        mr.SetValue(6, "txtHinCode2", Utility.ptnShohinStr(s.商品22));
        //        mr.SetValue(7, "txtHinCode2", Utility.ptnShohinStr(s.商品23));
        //        mr.SetValue(8, "txtHinCode2", Utility.ptnShohinStr(s.商品24));
        //        mr.SetValue(9, "txtHinCode2", Utility.ptnShohinStr(s.商品25));
        //        mr.SetValue(10, "txtHinCode2", Utility.ptnShohinStr(s.商品26));
        //        mr.SetValue(11, "txtHinCode2", Utility.ptnShohinStr(s.商品27));
        //        mr.SetValue(12, "txtHinCode2", Utility.ptnShohinStr(s.商品28));
        //        mr.SetValue(13, "txtHinCode2", Utility.ptnShohinStr(s.商品29));
        //        mr.SetValue(14, "txtHinCode2", Utility.ptnShohinStr(s.商品30));
        //    }
        //    else
        //    {
        //        mr.SetValue(0, "txtHinCode", "");
        //        mr.SetValue(1, "txtHinCode", "");
        //        mr.SetValue(2, "txtHinCode", "");
        //        mr.SetValue(3, "txtHinCode", "");
        //        mr.SetValue(4, "txtHinCode", "");
        //        mr.SetValue(5, "txtHinCode", "");
        //        mr.SetValue(6, "txtHinCode", "");
        //        mr.SetValue(7, "txtHinCode", "");
        //        mr.SetValue(8, "txtHinCode", "");
        //        mr.SetValue(9, "txtHinCode", "");
        //        mr.SetValue(10, "txtHinCode", "");
        //        mr.SetValue(11, "txtHinCode", "");
        //        mr.SetValue(12, "txtHinCode", "");
        //        mr.SetValue(13, "txtHinCode", "");
        //        mr.SetValue(14, "txtHinCode", "");

        //        mr.SetValue(0, "txtHinCode2", "");
        //        mr.SetValue(1, "txtHinCode2", "");
        //        mr.SetValue(2, "txtHinCode2", "");
        //        mr.SetValue(3, "txtHinCode2", "");
        //        mr.SetValue(4, "txtHinCode2", "");
        //        mr.SetValue(5, "txtHinCode2", "");
        //        mr.SetValue(6, "txtHinCode2", "");
        //        mr.SetValue(7, "txtHinCode2", "");
        //        mr.SetValue(8, "txtHinCode2", "");
        //        mr.SetValue(9, "txtHinCode2", "");
        //        mr.SetValue(10, "txtHinCode2", "");
        //        mr.SetValue(11, "txtHinCode2", "");
        //        mr.SetValue(12, "txtHinCode2", "");
        //        mr.SetValue(13, "txtHinCode2", "");
        //        mr.SetValue(14, "txtHinCode2", "");
        //    }
        //}


        /// --------------------------------------------------------------------------------
        /// <summary>
        ///     時間外記入チェック </summary>
        /// <param name="wkSpan">
        ///     所定労働時間 </param>
        /// <param name="wkSpanName">
        ///     勤務体系名称 </param>
        /// <param name="mRow">
        ///     グリッド行インデックス </param>
        /// <param name="TaikeiCode">
        ///     勤務体系コード </param>
        /// --------------------------------------------------------------------------------
        private void zanCheckShow(long wkSpan, string wkSpanName, int mRow, int TaikeiCode)
        {
            //Int64 s10 = 0;  // 深夜勤務時間中の10分または15分休憩時間

            //// 所定勤務時間が取得されていないとき戻る
            //if (wkSpan == 0)
            //{
            //    return;
            //}
            
            //// 所定勤務時間が取得されているとき残業時間計算チェックを行う
            //Int64 restTm = 0;

            //// 所定時間ごとの休憩時間
            ////if (wkSpanName == WKSPAN0750)
            ////{
            ////    restTm = RESTTIME0750;
            ////}
            ////else if (wkSpanName == WKSPAN0755)
            ////{
            ////    restTm = RESTTIME0755;
            ////}
            ////else if (wkSpanName == WKSPAN0800)
            ////{
            ////    restTm = RESTTIME0800;
            ////}
                
            //// 時間外勤務時間取得 2015/09/30
            //Int64 zan = getZangyoTime(mRow, (Int64)tanMin30, wkSpan, restTm, out s10, TaikeiCode);

            //// 時間外記入時間チェック 2015/09/30
            //errCheckZanTm(mRow, zan);

            //OCRData ocr = new OCRData(_dbName, bs);

            //string sh = Utility.NulltoStr(dGV[cSH, mRow].Value.ToString());
            //string sm = Utility.NulltoStr(dGV[cSM, mRow].Value.ToString());
            //string eh = Utility.NulltoStr(dGV[cEH, mRow].Value.ToString());
            //string em = Utility.NulltoStr(dGV[cEM, mRow].Value.ToString());

            //// 深夜勤務時間を取得
            //double shinyaTm = ocr.getShinyaWorkTime(sh, sm, eh, em, tanMin10, s10);

            //// 深夜勤務時間チェック
            //errCheckShinyaTm(mRow, (Int64)shinyaTm);
        }

        /// -----------------------------------------------------------------------------------
        /// <summary>
        ///     時間外勤務時間取得 </summary>
        /// <param name="m">
        ///     グリッド行インデックス</param>
        /// <param name="Tani">
        ///     丸め単位</param>
        /// <param name="ws">
        ///     所定労働時間</param>
        /// <param name="restTime">
        ///     勤務体系別の所定労働時間内の休憩時間</param>
        /// <param name="s10Rest">
        ///     勤務体系別の所定労働時間以降の休憩時間単位</param>
        /// <param name="taikeiCode">
        ///     勤務体系コード</param>
        /// <returns>
        ///     時間外勤務時間</returns>
        /// -----------------------------------------------------------------------------------
        private Int64 getZangyoTime(int m, Int64 Tani, Int64 ws, Int64 restTime, out Int64 s10Rest, int taikeiCode)
        {
            Int64 zan = 0;  // 計算後時間外勤務時間
            s10Rest = 0;    // 深夜勤務時間帯の10分休憩時間

            //DateTime cTm;
            //DateTime sTm;
            //DateTime eTm;
            //DateTime zsTm;
            //DateTime pTm;

            //if (dGV[cSH, m].Value != null && dGV[cSM, m].Value != null && dGV[cEH, m].Value != null && dGV[cEM, m].Value != null)
            //{
            //    int ss = Utility.StrtoInt(dGV[cSH, m].Value.ToString()) * 100 + Utility.StrtoInt(dGV[cSM, m].Value.ToString());
            //    int ee = Utility.StrtoInt(dGV[cEH, m].Value.ToString()) * 100 + Utility.StrtoInt(dGV[cEM, m].Value.ToString());
            //    DateTime dt = DateTime.Today;
            //    string sToday = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();

            //    // 始業時刻
            //    if (DateTime.TryParse(sToday + " " + dGV[cSH, m].Value.ToString() + ":" + dGV[cSM, m].Value.ToString(), out cTm))
            //    {
            //        sTm = cTm;
            //    }
            //    else return 0;

            //    // 終業時刻
            //    if (ss > ee)
            //    {
            //        // 翌日
            //        dt = DateTime.Today.AddDays(1);
            //        sToday = dt.Year.ToString() + "/" + dt.Month.ToString() + "/" + dt.Day.ToString();
            //        if (DateTime.TryParse(sToday + " " + dGV[cEH, m].Value.ToString() + ":" + dGV[cEM, m].Value.ToString(), out cTm))
            //        {
            //            eTm = cTm;
            //        }
            //        else return 0;
            //    }
            //    else
            //    {
            //        // 同日
            //        if (DateTime.TryParse(sToday + " " + dGV[cEH, m].Value.ToString() + ":" + dGV[cEM, m].Value.ToString(), out cTm))
            //        {
            //            eTm = cTm;
            //        }
            //        else return 0;
            //    }

            //    // 作業日報に記入されている始業から就業までの就業時間取得
            //    double w = Utility.GetTimeSpan(sTm, eTm).TotalMinutes - restTime;

            //    // 所定労働時間内なら時間外なし
            //    if (w <= ws)
            //    {
            //        return 0;
            //    }

            //    // 所定労働時間＋休憩時間＋10分または15分経過後の時刻を取得（時間外開始時刻）
            //    zsTm = sTm.AddMinutes(ws);          // 所定労働時間
            //    zsTm = zsTm.AddMinutes(restTime);   // 休憩時間
            //    int zSpan = 0;

            //    if (taikeiCode == 100)
            //    {
            //        zsTm = zsTm.AddMinutes(10);         // 体系コード：100 所定労働時間後の10分休憩
            //        zSpan = 130;
            //    }
            //    else if (taikeiCode == 200 || taikeiCode == 300)
            //    {
            //        zsTm = zsTm.AddMinutes(15);         // 体系コード：200,300 所定労働時間後の15分休憩
            //        zSpan = 135;
            //    }

            //    pTm = zsTm;                         // 時間外開始時刻

            //    // 該当時刻から終業時刻まで130分または135分以上あればループさせる
            //    while (Utility.GetTimeSpan(pTm, eTm).TotalMinutes > zSpan)
            //    {
            //        // 終業時刻まで2時間につき10分休憩として時間外を算出
            //        // 時間外として2時間加算
            //        zan += 120;

            //        // 130分、または135分後の時刻を取得（2時間＋10分、または15分）
            //        pTm = pTm.AddMinutes(zSpan);

            //        // 深夜勤務時間中の10分または15分休憩時間を取得する
            //        s10Rest += getShinya10Rest(pTm, eTm, zSpan - 120);
            //    }

            //    // 130分（135分）以下の時間外を加算
            //    zan += (Int64)Utility.GetTimeSpan(pTm, eTm).TotalMinutes;

            //    // 単位で丸める
            //    zan -= (zan % Tani);
            //}

            return zan;
        }


        /// --------------------------------------------------------------------
        /// <summary>
        ///     深夜勤務時間中の10分または15分休憩時間を取得する </summary>
        /// <param name="pTm">
        ///     時刻</param>
        /// <param name="eTm">
        ///     終業時刻</param>
        /// <param name="taikeiRest">
        ///     勤務体系別の休憩時間(10分または15分）</param>
        /// <returns>
        ///     休憩時間</returns>
        /// --------------------------------------------------------------------
        private int getShinya10Rest(DateTime pTm, DateTime eTm, int taikeiRest)
        {
            int restTime = 0;

            // 130(135)分後の時刻が終業時刻以内か
            TimeSpan ts = eTm.TimeOfDay;

            if (pTm <= eTm)
            {
                // 時刻が深夜時間帯か？
                if (pTm.Hour >= 22 || pTm.Hour <= 5)
                {
                    if (pTm.Hour == 22)
                    {
                        // 22時帯は22時以降の経過分を対象とします。
                        // 例）21:57～22:07のとき22時台の7分が休憩時間
                        if (pTm.Minute >= taikeiRest)
                        {
                            restTime = taikeiRest;
                        }
                        else
                        {
                            restTime = pTm.Minute;
                        }
                    }
                    else if (pTm.Hour == 5)
                    {
                        // 4時帯の経過分を対象とするので5時帯は減算します。
                        // 例）4:57～5:07のとき5時台の7分は差し引いて3分が休憩時間
                        if (pTm.Minute < taikeiRest)
                        {
                            restTime = (taikeiRest - pTm.Minute);
                        }
                    }
                    else
                    {
                        restTime = taikeiRest;
                    }
                }
            }

            return restTime;
        }


        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     時間外記入チェック </summary>
        /// <param name="m">
        ///     勤務票明細Rowコレクション</param>
        /// <param name="tittle">
        ///     チェック項目名称</param>
        /// <param name="iX">
        ///     日付を表すインデックス</param>
        /// <param name="zan">
        ///     算出残業時間</param>
        /// <returns>
        ///     エラーなし：true, エラーあり：false</returns>
        ///------------------------------------------------------------------------------------
        private void errCheckZanTm(int m, Int64 zan)
        {
            Int64 mZan = 0;

            //mZan = (Utility.StrtoInt(gcMultiRow1[m, "txtZanH1"].Value.ToString()) * 60) + (Utility.StrtoInt(gcMultiRow1[m, "txtZanM1"].Value.ToString()) * 60 / 10);

            //// 記入時間と計算された残業時間が不一致のとき
            //if (zan != mZan)
            //{
            //    gcMultiRow1[m, "txtZanH1"].Style.BackColor = Color.LightPink;
            //    gcMultiRow1[m, "txtZanH1"].Style.BackColor = Color.LightPink;
            //}
            //else
            //{
            //    gcMultiRow1[m, "txtZanM1"].Style.BackColor = Color.White;
            //    gcMultiRow1[m, "txtZanM1"].Style.BackColor = Color.White;
            //}
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

            // テキストボックス表示色設定
            txtYear.BackColor = Color.White;
            txtMonth.BackColor = Color.White;
            txtPID.BackColor = Color.White;
            txtSeqNum.BackColor = Color.White;
            txtTokuisakiCD.BackColor = Color.White;
            checkBox1.BackColor = SystemColors.Control;

            txtYear.ForeColor = global.defaultColor;
            txtMonth.ForeColor = global.defaultColor;
            txtPID.ForeColor = global.defaultColor;
            txtSeqNum.ForeColor = global.defaultColor;
            txtTokuisakiCD.ForeColor = global.defaultColor;
            txtMemo.ForeColor = global.defaultColor;

            // ヘッダ情報表示欄
            txtYear.Text = string.Empty;
            txtMonth.Text = string.Empty;
            txtPID.Text = string.Empty;
            txtSeqNum.Text = string.Empty;
            txtTokuisakiCD.Text = string.Empty;
            lblNoImage.Visible = false;

            dg1.Rows.Clear();   // 行数をクリア
            dg1.Rows.Add(30);   // 行数を設定

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
                lblPage.Text = string.Empty;
            }
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     フォーム表示初期化 </summary>
        /// <param name="sID">
        ///     過去データ表示時のヘッダID</param>
        /// <param name="cIx">
        ///     勤務票ヘッダカレントレコードインデックス</param>
        ///------------------------------------------------------------------------------------
        private void formInitialize(string sID, int cIx, int dCnt)
        {
            // 表示色設定
            //gcMultiRow1[0, "txtPtnNum"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "txtTdkNum"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "lblName"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "lblTel"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "txtOrderNum"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "txtMonth"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "txtDay"].Style.BackColor = SystemColors.Window;
            //gcMultiRow1[0, "lblPage"].Style.BackColor = SystemColors.Control;
            //gcMultiRow1[0, "chkReFax"].Style.BackColor = SystemColors.Window;   // 2018/08/03
            
            lblNoImage.Visible = false;

            // 編集可否
            //gcMultiRow1.ReadOnly = false;
            //gcMultiRow2.ReadOnly = false;
            //gcMultiRow3.ReadOnly = false;
                
            // スクロールバー設定
            hScrollBar1.Enabled = true;
            hScrollBar1.Minimum = 0;
            hScrollBar1.Maximum = dCnt;
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
            if ((cIx + 1) == dCnt)
            {
                btnNext.Enabled = false;
                btnEnd.Enabled = false;
            }
            
            //データ数表示
            //gcMultiRow1[0, "lblPage"].Value = " (" + (cI + 1).ToString() + "/" + dCnt + ")";
            
            // メモ欄
            txtMemo.Text = string.Empty;

            // 確認チェック欄
            checkBox1.BackColor = SystemColors.Control;
            checkBox1.Checked = false;
        }

        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     エラー表示 </summary>
        /// <param name="ocr">
        ///     OCRDATAクラス</param>
        ///------------------------------------------------------------------------------------
        //private void ErrShow(OCRData ocr)
        //{
        //    if (ocr._errNumber != ocr.eNothing)
        //    {
        //        // グリッドビューCellEnterイベント処理は実行しない
        //        gridViewCellEnterStatus = false;

        //        lblErrMsg.Visible = true;
        //        lblErrMsg.Text = ocr._errMsg;

        //        // 確認
        //        if (ocr._errNumber == ocr.eDataCheck)
        //        {
        //            checkBox1.BackColor = Color.Yellow;
        //            checkBox1.Focus();
        //        }

        //        // 届先番号
        //        if (ocr._errNumber == ocr.eTdkNo)
        //        {
        //            gcMultiRow1[0, "txtTdkNum"].Style.BackColor = Color.Yellow;
        //            gcMultiRow1.Focus();
        //            gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtTdkNum"];
        //            gcMultiRow1.BeginEdit(true);
                    
        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        // パターンID
        //        if (ocr._errNumber == ocr.ePattern)
        //        {
        //            gcMultiRow1[0, "txtPtnNum"].Style.BackColor = Color.Yellow;
        //            gcMultiRow1.Focus();
        //            gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtPtnNum"];
        //            gcMultiRow1.BeginEdit(true);
                    
        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }
                
        //        // 納品希望日
        //        if (ocr._errNumber == ocr.eMonth || ocr._errNumber == ocr.eDay)
        //        {
        //            gcMultiRow1[0, "txtMonth"].Style.BackColor = Color.Yellow;
        //            gcMultiRow1[0, "txtDay"].Style.BackColor = Color.Yellow;
        //            gcMultiRow1.Focus();
        //            gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtMonth"];
        //            gcMultiRow1.BeginEdit(true);
                    
        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        // 再ＦＡＸ：2018/08/03
        //        if (ocr._errNumber == ocr.eReFax)
        //        {
        //            gcMultiRow1[0, "chkReFax"].Style.BackColor = Color.Yellow;
        //            gcMultiRow1.Focus();
        //            gcMultiRow1.CurrentCell = gcMultiRow1[0, "chkReFax"];
        //            gcMultiRow1.BeginEdit(true);

        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        // 商品コード
        //        if (ocr._errNumber == ocr.eHinCode)
        //        {
        //            gcMultiRow2[ocr._errRow, "txtHinCode"].Style.BackColor = Color.Yellow;
        //            gcMultiRow2.Focus();
        //            gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtHinCode"];
        //            gcMultiRow2.BeginEdit(true);

        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        if (ocr._errNumber == ocr.eHinCode2)
        //        {
        //            gcMultiRow2[ocr._errRow, "txtHinCode2"].Style.BackColor = Color.Yellow;
        //            gcMultiRow2.Focus();
        //            gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtHinCode2"];
        //            gcMultiRow2.BeginEdit(true);

        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        // 発注数
        //        if (ocr._errNumber == ocr.eSuu)
        //        {
        //            gcMultiRow2[ocr._errRow, "txtSuu"].Style.BackColor = Color.Yellow;
        //            gcMultiRow2.Focus();
        //            gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtSuu"];
        //            gcMultiRow2.BeginEdit(true);

        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        if (ocr._errNumber == ocr.eSuu2)
        //        {
        //            gcMultiRow2[ocr._errRow, "txtSuu2"].Style.BackColor = Color.Yellow;
        //            gcMultiRow2.Focus();
        //            gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtSuu2"];
        //            gcMultiRow2.BeginEdit(true);
                    
        //            // エラー有りフラグ
        //            txtErrStatus.Text = global.FLGON;
        //        }

        //        // 追加注文・商品コード
        //        if (ocr._errNumber == ocr.eAddCode)
        //        {
        //            gcMultiRow3[ocr._errRow, "txtHinCode"].Style.BackColor = Color.Yellow;
        //            gcMultiRow3.Focus();
        //            gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtHinCode"];
        //            gcMultiRow3.BeginEdit(true);
        //        }

        //        if (ocr._errNumber == ocr.eAddCode2)
        //        {
        //            gcMultiRow3[ocr._errRow, "txtHinCode2"].Style.BackColor = Color.Yellow;
        //            gcMultiRow3.Focus();
        //            gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtHinCode2"];
        //            gcMultiRow3.BeginEdit(true);
        //        }
                
        //        // 追加注文・発注数
        //        if (ocr._errNumber == ocr.eAddSuu)
        //        {
        //            gcMultiRow3[ocr._errRow, "txtSuu"].Style.BackColor = Color.Yellow;
        //            gcMultiRow3.Focus();
        //            gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtSuu"];
        //            gcMultiRow3.BeginEdit(true);
        //        }

        //        if (ocr._errNumber == ocr.eAddSuu2)
        //        {
        //            gcMultiRow3[ocr._errRow, "txtSuu2"].Style.BackColor = Color.Yellow;
        //            gcMultiRow3.Focus();
        //            gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtSuu2"];
        //            gcMultiRow3.BeginEdit(true);
        //        }
                
        //        // グリッドビューCellEnterイベントステータスを戻す
        //        gridViewCellEnterStatus = true;
        //    }
        //}

    }
}
