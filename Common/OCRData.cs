using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SQLite;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.IO;
//using Oracle.ManagedDataAccess.Client;

namespace STSH_OCR.Common
{
    class OCRData
    {
        public OCRData()
        {
            //_Cn = Cn;
        }

        #region エラー項目番号プロパティ
        //---------------------------------------------------
        //          エラー情報
        //---------------------------------------------------

        enum errCode
        {
            eNothing, eYearMonth, eMonth, eDay, eKinmuTaikeiCode
        }

        /// <summary>
        ///     エラーヘッダ行RowIndex</summary>
        public int _errHeaderIndex { get; set; }

        /// <summary>
        ///     エラー項目番号</summary>
        public int _errNumber { get; set; }

        /// <summary>
        ///     エラー明細行RowIndex </summary>
        public int _errRow { get; set; }

        /// <summary> 
        ///     エラーメッセージ </summary>
        public string _errMsg { get; set; }

        /// <summary> 
        ///     エラーなし </summary>
        public int eNothing = 0;

        /// <summary>
        ///     エラー項目 = 確認チェック </summary>
        public int eDataCheck = 35;

        /// <summary> 
        ///     エラー項目 = 対象年月日 </summary>
        public int eYearMonth = 1;

        /// <summary> 
        ///     エラー項目 = 納品希望月 </summary>
        public int eMonth = 2;

        /// <summary> 
        ///     エラー項目 = 納品希望日 </summary>
        public int eDay = 3;

        /// <summary> 
        ///     エラー項目 = パターンID </summary>
        public int ePattern = 4;

        /// <summary> 
        ///     エラー項目 = 得意先番号 </summary>
        public int eTdkNo = 5;

        /// <summary> 
        ///     エラー項目 = 商品コード </summary>
        public int eHinCode = 36;
        public int eHinCode2 = 37;

        /// <summary> 
        ///     エラー項目 = 発注数 </summary>
        public int eSuu = 6;
        public int eSuu2 = 11;
        
        /// <summary> 
        ///     エラー項目 = 追加注文コード </summary>
        public int eAddCode = 7;
        public int eAddCode2 = 8;

        /// <summary> 
        ///     エラー項目 = 追加注文数 </summary>
        public int eAddSuu = 9;
        public int eAddSuu2 = 10;

        /// <summary> 
        ///     エラー項目 = 店着日付 </summary>
        public int eTenDate1 = 11;
        public int eTenDate2 = 12;
        public int eTenDate3 = 13;
        public int eTenDate4 = 14;
        public int eTenDate5 = 15;
        public int eTenDate6 = 16;
        public int eTenDate7 = 17;


        /// <summary> 
        ///     エラー項目 = 残業時1 </summary>
        public int eZanH1 = 18;

        /// <summary> 
        ///     エラー項目 = 残業分1 </summary>
        public int eZanM1 = 19;

        /// <summary> 
        ///     エラー項目 = 残業理由2 </summary>
        public int eZanRe2 = 20;

        /// <summary> 
        ///     エラー項目 = 残業時2 </summary>
        public int eZanH2 = 21;

        /// <summary> 
        ///     エラー項目 = 残業分2 </summary>
        public int eZanM2 = 22;

        /// <summary> 
        ///     エラー項目 = ライン </summary>
        public int eLine = 23;
        public int eLine2 = 28;

        /// <summary> 
        ///     エラー項目 = 部門 </summary>
        public int eBmn = 24;
        public int eBmn2 = 29;

        /// <summary> 
        ///     エラー項目 = 製品群 </summary>
        public int eHin = 25;
        public int eHin2 = 30;

        /// <summary> 
        ///     エラー項目 = 応援分 </summary>
        public int eOuenM = 26;

        /// <summary> 
        ///     エラー項目 = 応援分 </summary>
        public int eOuenIP = 32;
        public int eOuenIP2 = 33;

        /// <summary> 
        ///     エラー項目 = 応援移動票と勤怠データＩ／Ｐ票 </summary>
        public int eIpOuen = 34;

        /// <summary> 
        ///     エラー項目 = 再FAX </summary>
        public int eReFax = 38;

        #endregion
        
        #region 警告項目
        ///     <!--警告項目配列 -->
        public int[] warArray = new int[6];

        /// <summary>
        ///     警告項目番号</summary>
        public int _warNumber { get; set; }

        /// <summary>
        ///     警告明細行RowIndex </summary>
        public int _warRow { get; set; }

        /// <summary> 
        ///     警告項目 = 勤怠記号1&2 </summary>
        public int wKintaiKigou = 0;

        /// <summary> 
        ///     警告項目 = 開始終了時分 </summary>
        public int wSEHM = 1;

        /// <summary> 
        ///     警告項目 = 時間外時分 </summary>
        public int wZHM = 2;

        /// <summary> 
        ///     警告項目 = 深夜勤務時分 </summary>
        public int wSIHM = 3;

        /// <summary> 
        ///     警告項目 = 休日出勤時分 </summary>
        public int wKSHM = 4;

        /// <summary> 
        ///     警告項目 = 出勤形態 </summary>
        public int wShukeitai = 5;

        #endregion
        
        // テーブルアダプターインスタンス
        //NHBRDataSetTableAdapters.TableAdapterManager adpMn = new NHBRDataSetTableAdapters.TableAdapterManager();
        
        ///---------------------------------------------------------------------------------
        /// <summary>
        ///     追加用FAX注文書Rowオブジェクトを作成する </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row </param>
        /// <param name="stCSV">
        ///     CSV配列</param>
        /// <returns>
        ///     追加するFAX注文書Rowオブジェクト</returns>
        ///---------------------------------------------------------------------------------
        private void setNewHeadRecRow(ref NHBR_CLIDataSet.FAX注文書Row r, string[] stCSV)
        {
            r.ID = Utility.GetStringSubMax(stCSV[1].Trim(), 17);
            r.画像名 = Utility.GetStringSubMax(stCSV[1].Trim(), 17) + ".tif";
            r.パターンID = Utility.StrtoInt(Utility.GetStringSubMax(stCSV[2].Trim(), 4).Replace("-", ""));
            r.得意先番号 = Utility.StrtoInt(Utility.GetStringSubMax(stCSV[3].Trim(), 6).Replace("-", ""));
            r.発注番号 = Utility.GetStringSubMax(stCSV[5].Trim(), 8).Replace("-", "").Replace(" ", "");
            
            //r.納品希望月 = Utility.GetStringSubMax(stCSV[6].Trim(), 2).Replace("-", "");   // 2018/09/14
            //r.納品希望日 = Utility.GetStringSubMax(stCSV[7].Trim(), 2).Replace("-", "");   // 2018/09/14
            
            // 2018/09/14 新書式対応
            r.納品希望月 = string.Empty;     // OCR認識するも表示せず 2018/09/18
            r.納品希望日 = string.Empty;     // OCR認識するも表示せず2018/09/18

            r.確認 = global.flgOff;
            r.更新年月日 = DateTime.Now;
            r.出荷基準A = string.Empty;
            r.出荷基準B = string.Empty;
            r.出荷基準C = string.Empty;
            r.出荷基準D = string.Empty;
            r.出荷基準E = string.Empty;
            r.出荷基準F = string.Empty;
            r.出荷基準G = string.Empty;

            // 2018/08/02 再FAXフラグ
            string rf = Utility.GetStringSubMax(stCSV[4].Trim(), 1).Replace("-", "");

            if (rf == global.FLGOFF)
            {
                r.メモ = global.REFAX;
            }
            else
            {
                r.メモ = string.Empty;
            }
        }
        
        ///---------------------------------------------------------------------------------
        /// <summary>
        ///     追加用FAX注文書Rowオブジェクトを作成する </summary>
        /// <param name="r">
        ///     NHBR_CLIDataSet.FAX注文書Row </param>
        /// <param name="stCSV">
        ///     CSV配列</param>
        /// <param name="iX">
        ///     インデックス</param>
        /// <param name="mCode">
        ///     担当者コード</param>
        /// <returns>
        ///     追加するFAX注文書Rowオブジェクト</returns>
        ///---------------------------------------------------------------------------------
        private void setNewItemRecRow(ref NHBR_CLIDataSet.FAX注文書Row r, string[] stCSV, int iX, string mCode)
        {
            if (iX == 1)
            {
                r.注文数1 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 2)
            {
                r.注文数2 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 3)
            {
                r.注文数3 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 4)
            {
                r.注文数4 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 5)
            {
                r.注文数5 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 6)
            {
                r.注文数6 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 7)
            {
                r.注文数7 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 8)
            {
                r.注文数8 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 9)
            {
                r.注文数9 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 10)
            {
                r.注文数10 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 11)
            {
                r.注文数11 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 12)
            {
                r.注文数12 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 13)
            {
                r.注文数13 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 14)
            {
                r.注文数14 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 15)
            {
                r.注文数15 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 16)
            {
                r.注文数16 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 17)
            {
                r.注文数17 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 18)
            {
                r.注文数18 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 19)
            {
                r.注文数19 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 20)
            {
                r.注文数20 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 21)
            {
                r.注文数21 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 22)
            {
                r.注文数22 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 23)
            {
                r.注文数23 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 24)
            {
                r.注文数24 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 25)
            {
                r.注文数25 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 26)
            {
                r.注文数26 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 27)
            {
                r.注文数27 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 28)
            {
                r.注文数28 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 29)
            {
                r.注文数29 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 30)
            {
                r.注文数30 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 31)
            {
                r.追加注文数1 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 32)
            {
                r.追加注文数2 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 33)
            {
                r.追加注文数3 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 34)
            {
                r.追加注文数4 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 35)
            {
                r.追加注文数5 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 36)
            {
                r.追加注文数6 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 37)
            {
                r.追加注文数7 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 38)
            {
                r.追加注文数8 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 39)
            {
                r.追加注文数9 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 40)
            {
                r.追加注文数10 = stCSV[0].Trim().Replace("-", "").Replace(" ", "");
            }

            if (iX == 41)
            {
                r.追加注文チェック = Utility.StrtoInt(stCSV[0].Trim());
                r.備考欄記入 = Utility.StrtoInt(stCSV[1].Trim());

                r.エラー有無 = global.flgOff;

                //r.メモ = string.Empty;
                
                r.更新年月日 = DateTime.Now;
                r.担当者コード = mCode;
                r.追加注文商品コード1 = string.Empty;
                r.追加注文商品コード2 = string.Empty;
                r.追加注文商品コード3 = string.Empty;
                r.追加注文商品コード4 = string.Empty;
                r.追加注文商品コード5 = string.Empty;
                r.追加注文商品コード6 = string.Empty;
                r.追加注文商品コード7 = string.Empty;
                r.追加注文商品コード8 = string.Empty;
                r.追加注文商品コード9 = string.Empty;
                r.追加注文商品コード10 = string.Empty;
            }
        }

        ///----------------------------------------------------------------------------------------
        /// <summary>
        ///     値1がemptyで値2がNot string.Empty のとき "0"を返す。そうではないとき値1をそのまま返す</summary>
        /// <param name="str1">
        ///     値1：文字列</param>
        /// <param name="str2">
        ///     値2：文字列</param>
        /// <returns>
        ///     文字列</returns>
        ///----------------------------------------------------------------------------------------
        private string hmStrToZero(string str1, string str2)
        {
            string rVal = str1;
            if (str1 == string.Empty && str2 != string.Empty)
                rVal = "0";

            return rVal;
        }


        ///--------------------------------------------------------------------------------------------------
        /// <summary>
        ///     エラーチェックメイン処理。
        ///     エラーのときOCRDataクラスのヘッダ行インデックス、フィールド番号、明細行インデックス、
        ///     エラーメッセージが記録される </summary>
        /// <param name="sIx">
        ///     開始ヘッダ行インデックス</param>
        /// <param name="eIx">
        ///     終了ヘッダ行インデックス</param>
        /// <param name="frm">
        ///     親フォーム</param>
        /// <param name="dtsC">
        ///     NHBR_CLIDataSet </param>
        /// <param name="dts">
        ///     NHBRDataSet </param>
        /// <param name="cID">
        ///     FAX注文書@ID配列</param>
        /// <returns>
        ///     True:エラーなし、false:エラーあり</returns>
        ///-----------------------------------------------------------------------------------------------
        public Boolean errCheckMain(int sIx, int eIx, Form frm, Table<ClsFaxOrder> tblFax, Table<ClsOrderPattern> tblPtn, string[] cID)
        {
            int rCnt = 0;

            // オーナーフォームを無効にする
            frm.Enabled = false;

            // プログレスバーを表示する
            frmPrg frmP = new frmPrg();
            frmP.Owner = frm;
            frmP.Show();

            // レコード件数取得
            int cTotal = cID.Length;

            // 出勤簿データ読み出し
            Boolean eCheck = true;

            for (int i = 0; i < cTotal; i++)
            {
                //データ件数加算
                rCnt++;

                //プログレスバー表示
                frmP.Text = "エラーチェック実行中　" + rCnt.ToString() + "/" + cTotal.ToString();
                frmP.progressValue = rCnt * 100 / cTotal;
                frmP.ProgressStep();

                //指定範囲ならエラーチェックを実施する：（i:行index）
                if (i >= sIx && i <= eIx)
                {
                    // FAX注文書データのコレクションを取得します
                    ClsFaxOrder r = tblFax.Single(a => a.ID == cID[i]);

                    // エラーチェック実施
                    eCheck = errCheckData(dtsC, r, tblPtn);

                    if (!eCheck)　//エラーがあったとき
                    {
                        _errHeaderIndex = i;     // エラーとなったヘッダRowIndex
                        break;
                    }
                }
            }

            // いったんオーナーをアクティブにする
            frm.Activate();

            // 進行状況ダイアログを閉じる
            frmP.Close();

            // オーナーのフォームを有効に戻す
            frm.Enabled = true;

            return eCheck;
        }

        ///---------------------------------------------------------------------------------
        /// <summary>
        ///     エラー情報を取得します </summary>
        /// <param name="eID">
        ///     エラーデータのID</param>
        /// <param name="eNo">
        ///     エラー項目番号</param>
        /// <param name="eRow">
        ///     エラー明細行</param>
        /// <param name="eMsg">
        ///     表示メッセージ</param>
        ///---------------------------------------------------------------------------------
        private void setErrStatus(int eNo, int eRow, string eMsg)
        {
            //errHeaderIndex = eHRow;
            _errNumber = eNo;
            _errRow = eRow;
            _errMsg = eMsg;
        }


        ///-----------------------------------------------------------------------------------------------
        /// <summary>
        ///     項目別エラーチェック。
        ///     エラーのときヘッダ行インデックス、フィールド番号、明細行インデックス、エラーメッセージが記録される </summary>
        /// <param name="dts">
        ///     データセット</param>
        /// <param name="r">
        ///     勤務票ヘッダ行コレクション</param>
        /// <returns>
        ///     エラーなし：true, エラー有り：false</returns>
        ///-----------------------------------------------------------------------------------------------
        /// 
        public Boolean errCheckData(NHBR_CLIDataSet dtsC,  ClsFaxOrder r, Table<ClsOrderPattern> ptn)
        {
            string sDate;
            DateTime eDate;

            // 確認チェック
            if (r.Veri == global.flgOff)
            {
                setErrStatus(eDataCheck, 0, "未確認の発注書です");
                return false;
            }

            // 年月
            int rDate = r.Year * 100 + r.Month;
            int toDate = (DateTime.Today.Year * 100) + DateTime.Today.Day;
            if (rDate < toDate)
            {
                setErrStatus(eYearMonth, 0, "年月が正しくありません");
                return false;
            }

            // パターンID : 「０」はフリー入力可能とする 2017/08/22
            if (r.patternID != global.flgOff)
            {
                if (!ptn.Any(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber))
                {
                    setErrStatus(ePattern, 0, "登録されていない発注書番号です");
                    return false;
                }
            }

            // 得意先コード
            if (!getTdkStatus(r.TokuisakiCode.ToString().PadLeft(7, '0')))
            {
                setErrStatus(eTdkNo, 0, "不明な得意先コードです");
                return false;
            }

            // 店着日付
            DateTime dt;
            string rdt = r.Year + "/" + r.Month + "/" + r.Day1;
            if (!DateTime.TryParse(rdt, out dt))
            {
                setErrStatus(eTenDate1, 0, "店着日付が正しくありません");
                return false;
            }

            DayOfWeek wk = dt.DayOfWeek;
            if ((Int32)wk != 1)
            {
                setErrStatus(eTenDate1, 0, "店着日付の曜日が正しくありません");
                return false;
            }


            // 商品未登録の発注
            if (r.G_Code1 == string.Empty)
            {
                if  (r.Goods1_1 != string.Empty || r.Goods1_2 != string.Empty || r.Goods1_3 != string.Empty || r.Goods1_4 != string.Empty ||
                     r.Goods1_5 != string.Empty || r.Goods1_6 != string.Empty || r.Goods1_7 != string.Empty)
                {
                    setErrStatus(eSH, 0, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code2 == string.Empty)
            {
                if (r.Goods2_1 != string.Empty || r.Goods2_2 != string.Empty || r.Goods2_3 != string.Empty || r.Goods2_4 != string.Empty ||
                     r.Goods2_5 != string.Empty || r.Goods2_6 != string.Empty || r.Goods2_7 != string.Empty)
                {
                    setErrStatus(eSH, 1, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code3 == string.Empty)
            {
                if (r.Goods3_1 != string.Empty || r.Goods3_2 != string.Empty || r.Goods3_3 != string.Empty || r.Goods3_4 != string.Empty ||
                     r.Goods3_5 != string.Empty || r.Goods3_6 != string.Empty || r.Goods3_7 != string.Empty)
                {
                    setErrStatus(eSH,2, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code4 == string.Empty)
            {
                if (r.Goods4_1 != string.Empty || r.Goods4_2 != string.Empty || r.Goods4_3 != string.Empty || r.Goods4_4 != string.Empty ||
                     r.Goods4_5 != string.Empty || r.Goods4_6 != string.Empty || r.Goods4_7 != string.Empty)
                {
                    setErrStatus(eSH, 3, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code5 == string.Empty)
            {
                if (r.Goods5_1 != string.Empty || r.Goods5_2 != string.Empty || r.Goods5_3 != string.Empty || r.Goods5_4 != string.Empty ||
                     r.Goods5_5 != string.Empty || r.Goods5_6 != string.Empty || r.Goods5_7 != string.Empty)
                {
                    setErrStatus(eSH, 4, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code6 == string.Empty)
            {
                if (r.Goods6_1 != string.Empty || r.Goods6_2 != string.Empty || r.Goods6_3 != string.Empty || r.Goods6_4 != string.Empty ||
                     r.Goods6_5 != string.Empty || r.Goods6_6 != string.Empty || r.Goods6_7 != string.Empty)
                {
                    setErrStatus(eSH, 5, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code7 == string.Empty)
            {
                if (r.Goods8_1 != string.Empty || r.Goods7_2 != string.Empty || r.Goods7_3 != string.Empty || r.Goods7_4 != string.Empty ||
                     r.Goods7_5 != string.Empty || r.Goods7_6 != string.Empty || r.Goods7_7 != string.Empty)
                {
                    setErrStatus(eSH, 6, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code8 == string.Empty)
            {
                if (r.Goods8_1 != string.Empty || r.Goods8_2 != string.Empty || r.Goods8_3 != string.Empty || r.Goods8_4 != string.Empty ||
                     r.Goods8_5 != string.Empty || r.Goods8_6 != string.Empty || r.Goods8_7 != string.Empty)
                {
                    setErrStatus(eSH, 7, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code9 == string.Empty)
            {
                if (r.Goods9_1 != string.Empty || r.Goods9_2 != string.Empty || r.Goods9_3 != string.Empty || r.Goods9_4 != string.Empty ||
                     r.Goods9_5 != string.Empty || r.Goods9_6 != string.Empty || r.Goods9_7 != string.Empty)
                {
                    setErrStatus(eSH, 8, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code10 == string.Empty)
            {
                if (r.Goods10_1 != string.Empty || r.Goods10_2 != string.Empty || r.Goods10_3 != string.Empty || r.Goods10_4 != string.Empty ||
                     r.Goods10_5 != string.Empty || r.Goods10_6 != string.Empty || r.Goods10_7 != string.Empty)
                {
                    setErrStatus(eSH, 9, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code11 == string.Empty)
            {
                if (r.Goods11_1 != string.Empty || r.Goods11_2 != string.Empty || r.Goods11_3 != string.Empty || r.Goods11_4 != string.Empty ||
                     r.Goods11_5 != string.Empty || r.Goods11_6 != string.Empty || r.Goods11_7 != string.Empty)
                {
                    setErrStatus(eSH, 10, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code12 == string.Empty)
            {
                if (r.Goods13_1 != string.Empty || r.Goods12_2 != string.Empty || r.Goods12_3 != string.Empty || r.Goods12_4 != string.Empty ||
                     r.Goods12_5 != string.Empty || r.Goods12_6 != string.Empty || r.Goods12_7 != string.Empty)
                {
                    setErrStatus(eSH, 11, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code13 == string.Empty)
            {
                if (r.Goods13_1 != string.Empty || r.Goods13_2 != string.Empty || r.Goods13_3 != string.Empty || r.Goods13_4 != string.Empty ||
                     r.Goods13_5 != string.Empty || r.Goods13_6 != string.Empty || r.Goods13_7 != string.Empty)
                {
                    setErrStatus(eSH, 12, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code14 == string.Empty)
            {
                if (r.Goods14_1 != string.Empty || r.Goods14_2 != string.Empty || r.Goods14_3 != string.Empty || r.Goods14_4 != string.Empty ||
                     r.Goods14_5 != string.Empty || r.Goods14_6 != string.Empty || r.Goods14_7 != string.Empty)
                {
                    setErrStatus(eSH, 13, "商品が登録されていません");
                    return false;
                }
            }

            if (r.G_Code15 == string.Empty)
            {
                if (r.Goods15_1 != string.Empty || r.Goods15_2 != string.Empty || r.Goods15_3 != string.Empty || r.Goods15_4 != string.Empty ||
                     r.Goods15_5 != string.Empty || r.Goods15_6 != string.Empty || r.Goods15_7 != string.Empty)
                {
                    setErrStatus(eSH, 14, "商品が登録されていません");
                    return false;
                }
            }










            // パターン登録商品 : パターンID「０」以外を対象　2017/08/22
            if (r.patternID != global.flgOff)
            {
                //NHBR_OCR.NHBRDataSet.パターンIDRow t = dts.パターンID.Single(a => a.得意先番号 == r.得意先番号 && a.連番 == r.パターンID);

                ClsOrderPattern t = ptn.Single(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber);

                if (t.G_Code1 == string.Empty && r. != string.Empty)
                {
                    setErrStatus(eSuu, 0, "商品登録されていません");
                    return false;
                }

                if (t.商品2 == global.flgOff && r.注文数2 != string.Empty)
                {
                    setErrStatus(eSuu, 1, "商品登録されていません");
                    return false;
                }

                if (t.商品3 == global.flgOff && r.注文数3 != string.Empty)
                {
                    setErrStatus(eSuu, 2, "商品登録されていません");
                    return false;
                }

                if (t.商品4 == global.flgOff && r.注文数4 != string.Empty)
                {
                    setErrStatus(eSuu, 3, "商品登録されていません");
                    return false;
                }

                if (t.商品5 == global.flgOff && r.注文数5 != string.Empty)
                {
                    setErrStatus(eSuu, 4, "商品登録されていません");
                    return false;
                }

                if (t.商品6 == global.flgOff && r.注文数6 != string.Empty)
                {
                    setErrStatus(eSuu, 5, "商品登録されていません");
                    return false;
                }

                if (t.商品7 == global.flgOff && r.注文数7 != string.Empty)
                {
                    setErrStatus(eSuu, 6, "商品登録されていません");
                    return false;
                }

                if (t.商品8 == global.flgOff && r.注文数8 != string.Empty)
                {
                    setErrStatus(eSuu, 7, "商品登録されていません");
                    return false;
                }

                if (t.商品9 == global.flgOff && r.注文数9 != string.Empty)
                {
                    setErrStatus(eSuu, 8, "商品登録されていません");
                    return false;
                }

                if (t.商品10 == global.flgOff && r.注文数10 != string.Empty)
                {
                    setErrStatus(eSuu, 9, "商品登録されていません");
                    return false;
                }

                if (t.商品11 == global.flgOff && r.注文数11 != string.Empty)
                {
                    setErrStatus(eSuu, 10, "商品登録されていません");
                    return false;
                }

                if (t.商品12 == global.flgOff && r.注文数12 != string.Empty)
                {
                    setErrStatus(eSuu, 11, "商品登録されていません");
                    return false;
                }

                if (t.商品13 == global.flgOff && r.注文数13 != string.Empty)
                {
                    setErrStatus(eSuu, 12, "商品登録されていません");
                    return false;
                }

                if (t.商品14 == global.flgOff && r.注文数14 != string.Empty)
                {
                    setErrStatus(eSuu, 13, "商品登録されていません");
                    return false;
                }

                if (t.商品15 == global.flgOff && r.注文数15 != string.Empty)
                {
                    setErrStatus(eSuu, 14, "商品登録されていません");
                    return false;
                }

                if (t.商品16 == global.flgOff && r.注文数16 != string.Empty)
                {
                    setErrStatus(eSuu2, 0, "商品登録されていません");
                    return false;
                }

                if (t.商品17 == global.flgOff && r.注文数17 != string.Empty)
                {
                    setErrStatus(eSuu2, 1, "商品登録されていません");
                    return false;
                }

                if (t.商品18 == global.flgOff && r.注文数18 != string.Empty)
                {
                    setErrStatus(eSuu2, 2, "商品登録されていません");
                    return false;
                }

                if (t.商品19 == global.flgOff && r.注文数19 != string.Empty)
                {
                    setErrStatus(eSuu2, 3, "商品登録されていません");
                    return false;
                }

                if (t.商品20 == global.flgOff && r.注文数20 != string.Empty)
                {
                    setErrStatus(eSuu2, 4, "商品登録されていません");
                    return false;
                }

                if (t.商品21 == global.flgOff && r.注文数21 != string.Empty)
                {
                    setErrStatus(eSuu2, 5, "商品登録されていません");
                    return false;
                }

                if (t.商品22 == global.flgOff && r.注文数22 != string.Empty)
                {
                    setErrStatus(eSuu2, 6, "商品登録されていません");
                    return false;
                }

                if (t.商品23 == global.flgOff && r.注文数23 != string.Empty)
                {
                    setErrStatus(eSuu2, 7, "商品登録されていません");
                    return false;
                }

                if (t.商品24 == global.flgOff && r.注文数24 != string.Empty)
                {
                    setErrStatus(eSuu2, 8, "商品登録されていません");
                    return false;
                }

                if (t.商品25 == global.flgOff && r.注文数25 != string.Empty)
                {
                    setErrStatus(eSuu2, 9, "商品登録されていません");
                    return false;
                }

                if (t.商品26 == global.flgOff && r.注文数26 != string.Empty)
                {
                    setErrStatus(eSuu2, 10, "商品登録されていません");
                    return false;
                }

                if (t.商品27 == global.flgOff && r.注文数27 != string.Empty)
                {
                    setErrStatus(eSuu2, 11, "商品登録されていません");
                    return false;
                }

                if (t.商品28 == global.flgOff && r.注文数28 != string.Empty)
                {
                    setErrStatus(eSuu2, 12, "商品登録されていません");
                    return false;
                }

                if (t.商品29 == global.flgOff && r.注文数29 != string.Empty)
                {
                    setErrStatus(eSuu2, 13, "商品登録されていません");
                    return false;
                }

                if (t.商品30 == global.flgOff && r.注文数30 != string.Empty)
                {
                    setErrStatus(eSuu2, 14, "商品登録されていません");
                    return false;
                }
            }
            else
            {
                // フリー入力の場合：商品マスター登録チェック
                if (!getHinStatus(r.商品コード1))
                {
                    setErrStatus(eHinCode, 0, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード2))
                {
                    setErrStatus(eHinCode, 1, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード3))
                {
                    setErrStatus(eHinCode, 2, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード4))
                {
                    setErrStatus(eHinCode, 3, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード5))
                {
                    setErrStatus(eHinCode, 4, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード6))
                {
                    setErrStatus(eHinCode, 5, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード7))
                {
                    setErrStatus(eHinCode, 6, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード8))
                {
                    setErrStatus(eHinCode, 7, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード9))
                {
                    setErrStatus(eHinCode, 8, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード10))
                {
                    setErrStatus(eHinCode, 9, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード11))
                {
                    setErrStatus(eHinCode, 10, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード12))
                {
                    setErrStatus(eHinCode, 11, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード13))
                {
                    setErrStatus(eHinCode, 12, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード14))
                {
                    setErrStatus(eHinCode, 13, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード15))
                {
                    setErrStatus(eHinCode, 14, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード16))
                {
                    setErrStatus(eHinCode2, 0, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード17))
                {
                    setErrStatus(eHinCode2, 1, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード18))
                {
                    setErrStatus(eHinCode2, 2, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード19))
                {
                    setErrStatus(eHinCode2, 3, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード20))
                {
                    setErrStatus(eHinCode2, 4, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード21))
                {
                    setErrStatus(eHinCode2, 5, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード22))
                {
                    setErrStatus(eHinCode2, 6, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード23))
                {
                    setErrStatus(eHinCode2, 7, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード24))
                {
                    setErrStatus(eHinCode2, 8, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード25))
                {
                    setErrStatus(eHinCode2, 9, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード26))
                {
                    setErrStatus(eHinCode2, 10, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード27))
                {
                    setErrStatus(eHinCode2, 11, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード28))
                {
                    setErrStatus(eHinCode2, 12, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード29))
                {
                    setErrStatus(eHinCode2, 13, "登録されていない商品コードです");
                    return false;
                }

                if (!getHinStatus(r.商品コード30))
                {
                    setErrStatus(eHinCode2, 14, "登録されていない商品コードです");
                    return false;
                }
                
                // フリー入力の場合：商品未入力のとき
                if (r.商品コード1 == string.Empty && r.注文数1 != string.Empty)
                {
                    setErrStatus(eSuu, 0, "商品登録されていません");
                    return false;
                }

                if (r.商品コード2 == string.Empty && r.注文数2 != string.Empty)
                {
                    setErrStatus(eSuu, 1, "商品登録されていません");
                    return false;
                }

                if (r.商品コード3 == string.Empty && r.注文数3 != string.Empty)
                {
                    setErrStatus(eSuu, 2, "商品登録されていません");
                    return false;
                }

                if (r.商品コード4 == string.Empty && r.注文数4 != string.Empty)
                {
                    setErrStatus(eSuu, 3, "商品登録されていません");
                    return false;
                }

                if (r.商品コード5 == string.Empty && r.注文数5 != string.Empty)
                {
                    setErrStatus(eSuu, 4, "商品登録されていません");
                    return false;
                }

                if (r.商品コード6 == string.Empty && r.注文数6 != string.Empty)
                {
                    setErrStatus(eSuu, 5, "商品登録されていません");
                    return false;
                }

                if (r.商品コード7 == string.Empty && r.注文数7 != string.Empty)
                {
                    setErrStatus(eSuu, 6, "商品登録されていません");
                    return false;
                }

                if (r.商品コード8 == string.Empty && r.注文数8 != string.Empty)
                {
                    setErrStatus(eSuu, 7, "商品登録されていません");
                    return false;
                }

                if (r.商品コード9 == string.Empty && r.注文数9 != string.Empty)
                {
                    setErrStatus(eSuu, 8, "商品登録されていません");
                    return false;
                }

                if (r.商品コード10 == string.Empty && r.注文数10 != string.Empty)
                {
                    setErrStatus(eSuu, 9, "商品登録されていません");
                    return false;
                }

                if (r.商品コード11 == string.Empty && r.注文数11 != string.Empty)
                {
                    setErrStatus(eSuu, 10, "商品登録されていません");
                    return false;
                }

                if (r.商品コード12 == string.Empty && r.注文数12 != string.Empty)
                {
                    setErrStatus(eSuu, 11, "商品登録されていません");
                    return false;
                }

                if (r.商品コード13 == string.Empty && r.注文数13 != string.Empty)
                {
                    setErrStatus(eSuu, 12, "商品登録されていません");
                    return false;
                }

                if (r.商品コード14 == string.Empty && r.注文数14 != string.Empty)
                {
                    setErrStatus(eSuu, 13, "商品登録されていません");
                    return false;
                }

                if (r.商品コード15 == string.Empty && r.注文数15 != string.Empty)
                {
                    setErrStatus(eSuu, 14, "商品登録されていません");
                    return false;
                }

                if (r.商品コード16 == string.Empty && r.注文数16 != string.Empty)
                {
                    setErrStatus(eSuu2, 0, "商品登録されていません");
                    return false;
                }

                if (r.商品コード17 == string.Empty && r.注文数17 != string.Empty)
                {
                    setErrStatus(eSuu2, 1, "商品登録されていません");
                    return false;
                }

                if (r.商品コード18 == string.Empty && r.注文数18 != string.Empty)
                {
                    setErrStatus(eSuu2, 2, "商品登録されていません");
                    return false;
                }

                if (r.商品コード19 == string.Empty && r.注文数19 != string.Empty)
                {
                    setErrStatus(eSuu2, 3, "商品登録されていません");
                    return false;
                }

                if (r.商品コード20 == string.Empty && r.注文数20 != string.Empty)
                {
                    setErrStatus(eSuu2, 4, "商品登録されていません");
                    return false;
                }

                if (r.商品コード21 == string.Empty && r.注文数21 != string.Empty)
                {
                    setErrStatus(eSuu2, 5, "商品登録されていません");
                    return false;
                }

                if (r.商品コード22 == string.Empty && r.注文数22 != string.Empty)
                {
                    setErrStatus(eSuu2, 6, "商品登録されていません");
                    return false;
                }

                if (r.商品コード23 == string.Empty && r.注文数23 != string.Empty)
                {
                    setErrStatus(eSuu2, 7, "商品登録されていません");
                    return false;
                }

                if (r.商品コード24 == string.Empty && r.注文数24 != string.Empty)
                {
                    setErrStatus(eSuu2, 8, "商品登録されていません");
                    return false;
                }

                if (r.商品コード25 == string.Empty && r.注文数25 != string.Empty)
                {
                    setErrStatus(eSuu2, 9, "商品登録されていません");
                    return false;
                }

                if (r.商品コード26 == string.Empty && r.注文数26 != string.Empty)
                {
                    setErrStatus(eSuu2, 10, "商品登録されていません");
                    return false;
                }

                if (r.商品コード27 == string.Empty && r.注文数27 != string.Empty)
                {
                    setErrStatus(eSuu2, 11, "商品登録されていません");
                    return false;
                }

                if (r.商品コード28 == string.Empty && r.注文数28 != string.Empty)
                {
                    setErrStatus(eSuu2, 12, "商品登録されていません");
                    return false;
                }

                if (r.商品コード29 == string.Empty && r.注文数29 != string.Empty)
                {
                    setErrStatus(eSuu2, 13, "商品登録されていません");
                    return false;
                }

                if (r.商品コード30 == string.Empty && r.注文数30 != string.Empty)
                {
                    setErrStatus(eSuu2, 14, "商品登録されていません");
                    return false;
                }

                // フリー入力の場合：注文数未入力のとき
                if (r.商品コード1 != string.Empty && r.注文数1 == string.Empty)
                {
                    setErrStatus(eSuu, 0, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード2 != string.Empty && r.注文数2 == string.Empty)
                {
                    setErrStatus(eSuu, 1, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード3 != string.Empty && r.注文数3 == string.Empty)
                {
                    setErrStatus(eSuu, 2, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード4 != string.Empty && r.注文数4 == string.Empty)
                {
                    setErrStatus(eSuu, 3, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード5 != string.Empty && r.注文数5 == string.Empty)
                {
                    setErrStatus(eSuu, 4, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード6 != string.Empty && r.注文数6 == string.Empty)
                {
                    setErrStatus(eSuu, 5, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード7 != string.Empty && r.注文数7 == string.Empty)
                {
                    setErrStatus(eSuu, 6, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード8 != string.Empty && r.注文数8 == string.Empty)
                {
                    setErrStatus(eSuu, 7, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード9 != string.Empty && r.注文数9 == string.Empty)
                {
                    setErrStatus(eSuu, 8, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード10 != string.Empty && r.注文数10 == string.Empty)
                {
                    setErrStatus(eSuu, 9, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード11 != string.Empty && r.注文数11 == string.Empty)
                {
                    setErrStatus(eSuu, 10, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード12 != string.Empty && r.注文数12 == string.Empty)
                {
                    setErrStatus(eSuu, 11, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード13 != string.Empty && r.注文数13 == string.Empty)
                {
                    setErrStatus(eSuu, 12, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード14 != string.Empty && r.注文数14 == string.Empty)
                {
                    setErrStatus(eSuu, 13, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード15 != string.Empty && r.注文数15 == string.Empty)
                {
                    setErrStatus(eSuu, 14, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード16 != string.Empty && r.注文数16 == string.Empty)
                {
                    setErrStatus(eSuu2, 0, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード17 != string.Empty && r.注文数17 == string.Empty)
                {
                    setErrStatus(eSuu2, 1, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード18 != string.Empty && r.注文数18 == string.Empty)
                {
                    setErrStatus(eSuu2, 2, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード19 != string.Empty && r.注文数19 == string.Empty)
                {
                    setErrStatus(eSuu2, 3, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード20 != string.Empty && r.注文数20 == string.Empty)
                {
                    setErrStatus(eSuu2, 4, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード21 != string.Empty && r.注文数21 == string.Empty)
                {
                    setErrStatus(eSuu2, 5, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード22 != string.Empty && r.注文数22 == string.Empty)
                {
                    setErrStatus(eSuu2, 6, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード23 != string.Empty && r.注文数23 == string.Empty)
                {
                    setErrStatus(eSuu2, 7, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード24 != string.Empty && r.注文数24 == string.Empty)
                {
                    setErrStatus(eSuu2, 8, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード25 != string.Empty && r.注文数25 == string.Empty)
                {
                    setErrStatus(eSuu2, 9, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード26 != string.Empty && r.注文数26 == string.Empty)
                {
                    setErrStatus(eSuu2, 10, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード27 != string.Empty && r.注文数27 == string.Empty)
                {
                    setErrStatus(eSuu2, 11, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード28 != string.Empty && r.注文数28 == string.Empty)
                {
                    setErrStatus(eSuu2, 12, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード29 != string.Empty && r.注文数29 == string.Empty)
                {
                    setErrStatus(eSuu2, 13, "商品数が入力されていません");
                    return false;
                }

                if (r.商品コード30 != string.Empty && r.注文数30 == string.Empty)
                {
                    setErrStatus(eSuu2, 14, "商品数が入力されていません");
                    return false;
                }
            }

            // 追加注文商品コード：2017/08/23
            if (!getHinStatus(r.追加注文商品コード1))
            {
                setErrStatus(eAddCode, 0, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード2))
            {
                setErrStatus(eAddCode, 1, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード3))
            {
                setErrStatus(eAddCode, 2, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード4))
            {
                setErrStatus(eAddCode, 3, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード5))
            {
                setErrStatus(eAddCode, 4, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード6))
            {
                setErrStatus(eAddCode2, 0, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード7))
            {
                setErrStatus(eAddCode2, 1, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード8))
            {
                setErrStatus(eAddCode2, 2, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード9))
            {
                setErrStatus(eAddCode2, 3, "登録されていない商品コードです");
                return false;
            }

            if (!getHinStatus(r.追加注文商品コード10))
            {
                setErrStatus(eAddCode2, 4, "登録されていない商品コードです");
                return false;
            }

            // 追加注文
            if (r.追加注文商品コード1 == string.Empty && r.追加注文数1 != string.Empty)
            {
                setErrStatus(eAddCode, 0, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード1 != string.Empty && r.追加注文数1 == string.Empty)
            {
                setErrStatus(eAddSuu, 0, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード2 == string.Empty && r.追加注文数2 != string.Empty)
            {
                setErrStatus(eAddCode, 1, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード2 != string.Empty && r.追加注文数2 == string.Empty)
            {
                setErrStatus(eAddSuu, 1, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード3 == string.Empty && r.追加注文数3 != string.Empty)
            {
                setErrStatus(eAddCode, 2, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード3 != string.Empty && r.追加注文数3 == string.Empty)
            {
                setErrStatus(eAddSuu, 2, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード4 == string.Empty && r.追加注文数4 != string.Empty)
            {
                setErrStatus(eAddCode, 3, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード4 != string.Empty && r.追加注文数4 == string.Empty)
            {
                setErrStatus(eAddSuu, 3, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード5 == string.Empty && r.追加注文数5 != string.Empty)
            {
                setErrStatus(eAddCode, 4, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード5 != string.Empty && r.追加注文数5 == string.Empty)
            {
                setErrStatus(eAddSuu, 4, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード6 == string.Empty && r.追加注文数6 != string.Empty)
            {
                setErrStatus(eAddCode2, 0, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード6 != string.Empty && r.追加注文数6 == string.Empty)
            {
                setErrStatus(eAddSuu2, 0, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード7 == string.Empty && r.追加注文数7 != string.Empty)
            {
                setErrStatus(eAddCode2, 1, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード7 != string.Empty && r.追加注文数7 == string.Empty)
            {
                setErrStatus(eAddSuu2, 1, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード8 == string.Empty && r.追加注文数8 != string.Empty)
            {
                setErrStatus(eAddCode2, 2, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード8 != string.Empty && r.追加注文数8 == string.Empty)
            {
                setErrStatus(eAddSuu2, 2, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード9 == string.Empty && r.追加注文数9 != string.Empty)
            {
                setErrStatus(eAddCode2, 3, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード9 != string.Empty && r.追加注文数9 == string.Empty)
            {
                setErrStatus(eAddSuu2, 3, "追加商品数が入力されていません");
                return false;
            }

            if (r.追加注文商品コード10 == string.Empty && r.追加注文数10 != string.Empty)
            {
                setErrStatus(eAddCode2, 4, "追加商品コードが入力されていません");
                return false;
            }

            if (r.追加注文商品コード10 != string.Empty && r.追加注文数10 == string.Empty)
            {
                setErrStatus(eAddSuu2, 4, "追加商品数が入力されていません");
                return false;
            }
            
            return true;
        }


        private bool ChkTenDate(string rDate)
        {
            // 店着日付
            DateTime dt;
            if (!DateTime.TryParse(rDate, out dt))
            {
                setErrStatus(eTenDate1, 0, "店着日付が正しくありません");
                return false;
            }

            DayOfWeek wk = dt.DayOfWeek;
            if ((Int32)wk != 1)
            {
                setErrStatus(eTenDate1, 0, "店着日付の曜日が正しくありません");
                return false;
            }
        }







        ///--------------------------------------------------------------
        /// <summary>
        ///     得意先番号が登録済みか調べる </summary>
        /// <param name="tCode">
        ///     得意先番号</param>
        /// <returns>
        ///     true:登録済み、false:未登録</returns>
        ///--------------------------------------------------------------
        private bool getTdkStatus(string tCode)
        {
            bool rtn = false;
            string _Tel = "";
            string _Jyu = "";

            // 得意先番号
            if (Utility.getNouhinName(tCode, out _Tel, out _Jyu) != string.Empty)
            {
                rtn = true;
            }

            return rtn;
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     商品コードが登録済みか調べる </summary>
        /// <param name="hinCode">
        ///     商品コード</param>
        /// <returns>
        ///     true:登録済み、false:未登録</returns>
        ///--------------------------------------------------------------
        private bool getHinStatus(string hinCode)
        {
            if (hinCode == string.Empty)
            {
                return true;
            }
          
            bool rtn = false;

            hinCode = hinCode.PadLeft(8, '0');

            string strSQL = "select SYO_ID, SYO_NAME, SYO_IRI_KESU, SYO_TANI from RAKUSYO_FAXOCR.V_SYOHIN WHERE SYO_ID = '" + hinCode + "'";
            OracleCommand Cmd = new OracleCommand(strSQL, _Conn);
            OracleDataReader dR = Cmd.ExecuteReader();
            if (dR.HasRows)
            {
                rtn = true;
            }

            dR.Dispose();
            Cmd.Dispose();

            return rtn;
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     検索用DepartmentCodeを取得する </summary>
        /// <returns>
        ///     DepartmentCode</returns>
        ///----------------------------------------------------------
        private string getDepartmentCode(string bCode)
        {
            string strCode = "";

            // DepartmentCode（部署コード）
            if (Utility.NumericCheck(bCode))
            {
                strCode = bCode.PadLeft(15, '0');
            }
            else
            {
                strCode = bCode.PadRight(15, ' ');
            }

            return strCode;
        }

    }
}
