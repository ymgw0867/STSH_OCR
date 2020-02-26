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

            // 勤務票ヘッダデータを取得
            var r = tblFax.Single(a => a.ID == cID[iX]);

            // フォーム初期化
            formInitialize(dID, iX);

            txtYear.Text = r.Year.ToString();
            txtMonth.Text = Utility.EmptytoZero(r.Month.ToString());
            txtTokuisakiCD.Text = r.TokuisakiCode.ToString();
            txtPID.Text = r.patternID.ToString();
            txtSeqNum.Text = r.SeqNumber.ToString();

            global.ChangeValueStatus = false;   // これ以下ChangeValueイベントを発生させない

            checkBox1.Checked = Convert.ToBoolean(r.Veri);
            txtMemo.Text = r.memo;

            global.ChangeValueStatus = true;    // これ以下ChangeValueイベントを発生させる

            // 登録パターン表示
            showItem(r.ID, dGV, r.週払い区分);

            //// 月間合計値表示
            //getMonthTotal();

            // エラー情報表示初期化
            lblErrMsg.Visible = false;
            lblErrMsg.Text = string.Empty;

            // 画像表示
            _img = Properties.Settings.Default.mydataPath + r.画像名.ToString();
            showImage_openCv(_img);
            trackBar1.Enabled = true;

            // ログ書き込み状態とする
            editLogStatus = true;

            showStatus = false;
            Cursor = Cursors.Default;
        }
        
        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     発注商品明細表示 </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row</param>
        /// <param name="mr">
        ///     GcMultiRow</param>
        /// <param name="ptnNum">
        ///     パターンID</param>
        ///------------------------------------------------------------------------------------
        private void showItem()
        {
            gl.ChangeValueStatus = false;

            //mr.SetValue(0, "txtSuu", r.注文数1);
            //mr.SetValue(1, "txtSuu", r.注文数2);
            //mr.SetValue(2, "txtSuu", r.注文数3);
            //mr.SetValue(3, "txtSuu", r.注文数4);
            //mr.SetValue(4, "txtSuu", r.注文数5);
            //mr.SetValue(5, "txtSuu", r.注文数6);
            //mr.SetValue(6, "txtSuu", r.注文数7);
            //mr.SetValue(7, "txtSuu", r.注文数8);
            //mr.SetValue(8, "txtSuu", r.注文数9);
            //mr.SetValue(9, "txtSuu", r.注文数10);
            //mr.SetValue(10, "txtSuu", r.注文数11);
            //mr.SetValue(11, "txtSuu", r.注文数12);
            //mr.SetValue(12, "txtSuu", r.注文数13);
            //mr.SetValue(13, "txtSuu", r.注文数14);
            //mr.SetValue(14, "txtSuu", r.注文数15);
            //mr.SetValue(0, "txtSuu2", r.注文数16);
            //mr.SetValue(1, "txtSuu2", r.注文数17);
            //mr.SetValue(2, "txtSuu2", r.注文数18);
            //mr.SetValue(3, "txtSuu2", r.注文数19);
            //mr.SetValue(4, "txtSuu2", r.注文数20);
            //mr.SetValue(5, "txtSuu2", r.注文数21);
            //mr.SetValue(6, "txtSuu2", r.注文数22);
            //mr.SetValue(7, "txtSuu2", r.注文数23);
            //mr.SetValue(8, "txtSuu2", r.注文数24);
            //mr.SetValue(9, "txtSuu2", r.注文数25);
            //mr.SetValue(10, "txtSuu2", r.注文数26);
            //mr.SetValue(11, "txtSuu2", r.注文数27);
            //mr.SetValue(12, "txtSuu2", r.注文数28);
            //mr.SetValue(13, "txtSuu2", r.注文数29);
            //mr.SetValue(14, "txtSuu2", r.注文数30);
                                    
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

            ////カレントセル選択状態としない
            //mr.CurrentCell = null;
        }

        private void ptnShow(GcMultiRow mr, int tdkCode, int ptnCode)
        {
            gl.ChangeValueStatus = true;

            if (dts.パターンID.Any(a => a.届先番号 == tdkCode && a.連番 == ptnCode))
            {
                var s = dts.パターンID.Single(a => a.届先番号 == tdkCode && a.連番 == ptnCode);

                mr.SetValue(0, "txtHinCode", Utility.ptnShohinStr(s.商品1));
                mr.SetValue(1, "txtHinCode", Utility.ptnShohinStr(s.商品2));
                mr.SetValue(2, "txtHinCode", Utility.ptnShohinStr(s.商品3));
                mr.SetValue(3, "txtHinCode", Utility.ptnShohinStr(s.商品4));
                mr.SetValue(4, "txtHinCode", Utility.ptnShohinStr(s.商品5));
                mr.SetValue(5, "txtHinCode", Utility.ptnShohinStr(s.商品6));
                mr.SetValue(6, "txtHinCode", Utility.ptnShohinStr(s.商品7));
                mr.SetValue(7, "txtHinCode", Utility.ptnShohinStr(s.商品8));
                mr.SetValue(8, "txtHinCode", Utility.ptnShohinStr(s.商品9));
                mr.SetValue(9, "txtHinCode", Utility.ptnShohinStr(s.商品10));
                mr.SetValue(10, "txtHinCode", Utility.ptnShohinStr(s.商品11));
                mr.SetValue(11, "txtHinCode", Utility.ptnShohinStr(s.商品12));
                mr.SetValue(12, "txtHinCode", Utility.ptnShohinStr(s.商品13));
                mr.SetValue(13, "txtHinCode", Utility.ptnShohinStr(s.商品14));
                mr.SetValue(14, "txtHinCode", Utility.ptnShohinStr(s.商品15));

                mr.SetValue(0, "txtHinCode2", Utility.ptnShohinStr(s.商品16));
                mr.SetValue(1, "txtHinCode2", Utility.ptnShohinStr(s.商品17));
                mr.SetValue(2, "txtHinCode2", Utility.ptnShohinStr(s.商品18));
                mr.SetValue(3, "txtHinCode2", Utility.ptnShohinStr(s.商品19));
                mr.SetValue(4, "txtHinCode2", Utility.ptnShohinStr(s.商品20));
                mr.SetValue(5, "txtHinCode2", Utility.ptnShohinStr(s.商品21));
                mr.SetValue(6, "txtHinCode2", Utility.ptnShohinStr(s.商品22));
                mr.SetValue(7, "txtHinCode2", Utility.ptnShohinStr(s.商品23));
                mr.SetValue(8, "txtHinCode2", Utility.ptnShohinStr(s.商品24));
                mr.SetValue(9, "txtHinCode2", Utility.ptnShohinStr(s.商品25));
                mr.SetValue(10, "txtHinCode2", Utility.ptnShohinStr(s.商品26));
                mr.SetValue(11, "txtHinCode2", Utility.ptnShohinStr(s.商品27));
                mr.SetValue(12, "txtHinCode2", Utility.ptnShohinStr(s.商品28));
                mr.SetValue(13, "txtHinCode2", Utility.ptnShohinStr(s.商品29));
                mr.SetValue(14, "txtHinCode2", Utility.ptnShohinStr(s.商品30));
            }
            else
            {
                mr.SetValue(0, "txtHinCode", "");
                mr.SetValue(1, "txtHinCode", "");
                mr.SetValue(2, "txtHinCode", "");
                mr.SetValue(3, "txtHinCode", "");
                mr.SetValue(4, "txtHinCode", "");
                mr.SetValue(5, "txtHinCode", "");
                mr.SetValue(6, "txtHinCode", "");
                mr.SetValue(7, "txtHinCode", "");
                mr.SetValue(8, "txtHinCode", "");
                mr.SetValue(9, "txtHinCode", "");
                mr.SetValue(10, "txtHinCode", "");
                mr.SetValue(11, "txtHinCode", "");
                mr.SetValue(12, "txtHinCode", "");
                mr.SetValue(13, "txtHinCode", "");
                mr.SetValue(14, "txtHinCode", "");

                mr.SetValue(0, "txtHinCode2", "");
                mr.SetValue(1, "txtHinCode2", "");
                mr.SetValue(2, "txtHinCode2", "");
                mr.SetValue(3, "txtHinCode2", "");
                mr.SetValue(4, "txtHinCode2", "");
                mr.SetValue(5, "txtHinCode2", "");
                mr.SetValue(6, "txtHinCode2", "");
                mr.SetValue(7, "txtHinCode2", "");
                mr.SetValue(8, "txtHinCode2", "");
                mr.SetValue(9, "txtHinCode2", "");
                mr.SetValue(10, "txtHinCode2", "");
                mr.SetValue(11, "txtHinCode2", "");
                mr.SetValue(12, "txtHinCode2", "");
                mr.SetValue(13, "txtHinCode2", "");
                mr.SetValue(14, "txtHinCode2", "");
            }
        }
        
        ///------------------------------------------------------------------------------------
        /// <summary>
        ///     追加商品明細表示 </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row</param>
        /// <param name="mr">
        ///     GcMultiRow</param>
        ///------------------------------------------------------------------------------------
        private void addShowItem(NHBR_CLIDataSet.FAX注文書Row r, GcMultiRow mr)
        {
            // 2017/08/23
            for (int i = 0; i < gcMultiRow3.RowCount; i++)
            {
                // 注文数欄背景色初期化
                gcMultiRow3[i, "txtHinCode"].Style.BackColor = Color.Empty;
                gcMultiRow3[i, "txtSuu"].Style.BackColor = Color.Empty;
                gcMultiRow3[i, "txtHinCode2"].Style.BackColor = Color.Empty;
                gcMultiRow3[i, "txtSuu2"].Style.BackColor = Color.Empty;
            }

            gl.ChangeValueStatus = true;
            
            mr.SetValue(0, "txtHinCode", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード1)));
            mr.SetValue(1, "txtHinCode", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード2)));
            mr.SetValue(2, "txtHinCode", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード3)));
            mr.SetValue(3, "txtHinCode", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード4)));
            mr.SetValue(4, "txtHinCode", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード5)));
            mr.SetValue(0, "txtHinCode2", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード6)));
            mr.SetValue(1, "txtHinCode2", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード7)));
            mr.SetValue(2, "txtHinCode2", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード8)));
            mr.SetValue(3, "txtHinCode2", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード9)));
            mr.SetValue(4, "txtHinCode2", Utility.ptnShohinStr(Utility.StrtoInt(r.追加注文商品コード10)));

            //gl.ChangeValueStatus = false;

            mr.SetValue(0, "txtSuu", r.追加注文数1);
            mr.SetValue(1, "txtSuu", r.追加注文数2);
            mr.SetValue(2, "txtSuu", r.追加注文数3);
            mr.SetValue(3, "txtSuu", r.追加注文数4);
            mr.SetValue(4, "txtSuu", r.追加注文数5);
            mr.SetValue(0, "txtSuu2", r.追加注文数6);
            mr.SetValue(1, "txtSuu2", r.追加注文数7);
            mr.SetValue(2, "txtSuu2", r.追加注文数8);
            mr.SetValue(3, "txtSuu2", r.追加注文数9);
            mr.SetValue(4, "txtSuu2", r.追加注文数10);

            gl.ChangeValueStatus = true;

            // 編集を可能とする
            mr.ReadOnly = false;

            //mr.EndEdit();

            //カレントセル選択状態としない
            mr.CurrentCell = null;

            //string sss = r.追加注文商品コード1 + " " + r.追加注文数1 + Environment.NewLine;
            //sss += r.追加注文商品コード2 + " " + r.追加注文数2 + Environment.NewLine;
            //sss += r.追加注文商品コード3 + " " + r.追加注文数3 + Environment.NewLine;
            //sss += r.追加注文商品コード4 + " " + r.追加注文数4 + Environment.NewLine;
            //sss += r.追加注文商品コード5 + " " + r.追加注文数5 + Environment.NewLine;
            //sss += r.追加注文商品コード6 + " " + r.追加注文数6 + Environment.NewLine;
            //sss += r.追加注文商品コード7 + " " + r.追加注文数7 + Environment.NewLine;
            //sss += r.追加注文商品コード8 + " " + r.追加注文数8 + Environment.NewLine;
            //sss += r.追加注文商品コード9 + " " + r.追加注文数9 + Environment.NewLine;
            //sss += r.追加注文商品コード10 + " " + r.追加注文数10 + Environment.NewLine;

            //MessageBox.Show(sss);
        }

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

            mZan = (Utility.StrtoInt(gcMultiRow1[m, "txtZanH1"].Value.ToString()) * 60) + (Utility.StrtoInt(gcMultiRow1[m, "txtZanM1"].Value.ToString()) * 60 / 10);

            // 記入時間と計算された残業時間が不一致のとき
            if (zan != mZan)
            {
                gcMultiRow1[m, "txtZanH1"].Style.BackColor = Color.LightPink;
                gcMultiRow1[m, "txtZanH1"].Style.BackColor = Color.LightPink;
            }
            else
            {
                gcMultiRow1[m, "txtZanM1"].Style.BackColor = Color.White;
                gcMultiRow1[m, "txtZanM1"].Style.BackColor = Color.White;
            }
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

            // 勤務票データ編集のとき
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
                lblPage.Text = " (" + (cI + 1).ToString() + "/" + cID.Length + ")";
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

                // 届先番号
                if (ocr._errNumber == ocr.eTdkNo)
                {
                    gcMultiRow1[0, "txtTdkNum"].Style.BackColor = Color.Yellow;
                    gcMultiRow1.Focus();
                    gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtTdkNum"];
                    gcMultiRow1.BeginEdit(true);
                    
                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // パターンID
                if (ocr._errNumber == ocr.ePattern)
                {
                    gcMultiRow1[0, "txtPtnNum"].Style.BackColor = Color.Yellow;
                    gcMultiRow1.Focus();
                    gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtPtnNum"];
                    gcMultiRow1.BeginEdit(true);
                    
                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }
                
                // 納品希望日
                if (ocr._errNumber == ocr.eMonth || ocr._errNumber == ocr.eDay)
                {
                    gcMultiRow1[0, "txtMonth"].Style.BackColor = Color.Yellow;
                    gcMultiRow1[0, "txtDay"].Style.BackColor = Color.Yellow;
                    gcMultiRow1.Focus();
                    gcMultiRow1.CurrentCell = gcMultiRow1[0, "txtMonth"];
                    gcMultiRow1.BeginEdit(true);
                    
                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 再ＦＡＸ：2018/08/03
                if (ocr._errNumber == ocr.eReFax)
                {
                    gcMultiRow1[0, "chkReFax"].Style.BackColor = Color.Yellow;
                    gcMultiRow1.Focus();
                    gcMultiRow1.CurrentCell = gcMultiRow1[0, "chkReFax"];
                    gcMultiRow1.BeginEdit(true);

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 商品コード
                if (ocr._errNumber == ocr.eHinCode)
                {
                    gcMultiRow2[ocr._errRow, "txtHinCode"].Style.BackColor = Color.Yellow;
                    gcMultiRow2.Focus();
                    gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtHinCode"];
                    gcMultiRow2.BeginEdit(true);

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eHinCode2)
                {
                    gcMultiRow2[ocr._errRow, "txtHinCode2"].Style.BackColor = Color.Yellow;
                    gcMultiRow2.Focus();
                    gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtHinCode2"];
                    gcMultiRow2.BeginEdit(true);

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 発注数
                if (ocr._errNumber == ocr.eSuu)
                {
                    gcMultiRow2[ocr._errRow, "txtSuu"].Style.BackColor = Color.Yellow;
                    gcMultiRow2.Focus();
                    gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtSuu"];
                    gcMultiRow2.BeginEdit(true);

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eSuu2)
                {
                    gcMultiRow2[ocr._errRow, "txtSuu2"].Style.BackColor = Color.Yellow;
                    gcMultiRow2.Focus();
                    gcMultiRow2.CurrentCell = gcMultiRow2[ocr._errRow, "txtSuu2"];
                    gcMultiRow2.BeginEdit(true);
                    
                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 追加注文・商品コード
                if (ocr._errNumber == ocr.eAddCode)
                {
                    gcMultiRow3[ocr._errRow, "txtHinCode"].Style.BackColor = Color.Yellow;
                    gcMultiRow3.Focus();
                    gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtHinCode"];
                    gcMultiRow3.BeginEdit(true);
                }

                if (ocr._errNumber == ocr.eAddCode2)
                {
                    gcMultiRow3[ocr._errRow, "txtHinCode2"].Style.BackColor = Color.Yellow;
                    gcMultiRow3.Focus();
                    gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtHinCode2"];
                    gcMultiRow3.BeginEdit(true);
                }
                
                // 追加注文・発注数
                if (ocr._errNumber == ocr.eAddSuu)
                {
                    gcMultiRow3[ocr._errRow, "txtSuu"].Style.BackColor = Color.Yellow;
                    gcMultiRow3.Focus();
                    gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtSuu"];
                    gcMultiRow3.BeginEdit(true);
                }

                if (ocr._errNumber == ocr.eAddSuu2)
                {
                    gcMultiRow3[ocr._errRow, "txtSuu2"].Style.BackColor = Color.Yellow;
                    gcMultiRow3.Focus();
                    gcMultiRow3.CurrentCell = gcMultiRow3[ocr._errRow, "txtSuu2"];
                    gcMultiRow3.BeginEdit(true);
                }
                
                // グリッドビューCellEnterイベントステータスを戻す
                gridViewCellEnterStatus = true;
            }
        }
    }
}
