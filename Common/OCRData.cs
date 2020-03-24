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

namespace STSH_OCR.Common
{
    class OCRData
    {
        public OCRData()
        {

        }
        
        // 得意先クラス
        ClsCsvData.ClsCsvTokuisaki[] tokuisaki = null;

        // 商品クラス
        ClsCsvData.ClsCsvSyohin[] syohins = null;


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
        public int [] eSuu = new int[7];
        private const int ESUU_CODE = 100; 
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
        ///     エラー項目 = 終売 </summary>
        public int eShubai = 18;

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

        const int Sun = 0;
        const int Mon = 1;
        const int Tue = 2;
        const int Wed = 3;
        const int Thu = 4;
        const int Fri = 5;
        const int Sat = 6;

        // 発注書パターン
        ClsOrderPattern OrderPattern = null;

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
        /// <param name="tblFax">
        ///     ClsFaxOrderクラス</param>
        /// <param name="tblPtn">
        ///     ClsOrderPatternクラス</param>
        /// <param name="cID">
        ///     FAX注文書@ID配列</param>
        /// <returns>
        ///     True:エラーなし、false:エラーあり</returns>
        ///-----------------------------------------------------------------------------------------------
        public Boolean errCheckMain(int sIx, int eIx, Form frm, Table<ClsFaxOrder> tblFax, Table<ClsOrderPattern> tblPtn, string[] cID)
        {
            string[] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);
            int sDate = DateTime.Today.Year * 10000 + DateTime.Today.Month * 100 + DateTime.Today.Day;

            // 得意先マスタークラス配列取得
            tokuisaki = ClsCsvData.ClsCsvTokuisaki.Load(Tk_Array, sDate);

            // 商品マスタークラス配列取得
            syohins = Utility.GetSyohinData(Properties.Settings.Default.商品マスター, Properties.Settings.Default.商品在庫マスター, Properties.Settings.Default.仕入先マスター);

            int rCnt = 0;

            // オーナーフォームを無効にする
            frm.Enabled = false;

            // プログレスバーを表示する
            frmPrg frmP = new frmPrg();
            frmP.Owner = frm;
            frmP.Show();

            // レコード件数取得
            int cTotal = cID.Length;

            Boolean eCheck = true;

            // 発注書データ読み出し
            for (int i = 0; i < cTotal; i++)
            {
                // データ件数加算
                rCnt++;

                // プログレスバー表示
                frmP.Text = "エラーチェック実行中　" + rCnt.ToString() + "/" + cTotal.ToString();
                frmP.progressValue = rCnt * 100 / cTotal;
                frmP.ProgressStep();

                // 指定範囲ならエラーチェックを実施する
                if (i >= sIx && i <= eIx)
                {
                    // FAX注文書データのコレクションを取得します
                    ClsFaxOrder r = tblFax.Single(a => a.ID == cID[i]);

                    // エラーチェック実施
                    eCheck = errCheckData(r, tblPtn);

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
        public Boolean errCheckMain(string sID, Table<ClsOrder> tblOrder, Table<ClsOrderPattern> tblPtn)
        {
            string[] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);
            int sDate = DateTime.Today.Year * 10000 + DateTime.Today.Month * 100 + DateTime.Today.Day;

            // 得意先マスタークラス配列取得
            tokuisaki = ClsCsvData.ClsCsvTokuisaki.Load(Tk_Array, sDate);

            // 商品マスタークラス配列取得
            syohins = Utility.GetSyohinData(Properties.Settings.Default.商品マスター, Properties.Settings.Default.商品在庫マスター, Properties.Settings.Default.仕入先マスター);

            // 発注書データ読み出し
            Boolean eCheck = true;

            // FAX注文書データのコレクションを取得します
            ClsOrder r = tblOrder.Single(a => a.ID == sID);

            // エラーチェック実施
            eCheck = errCheckData(r, tblPtn);

            //if (!eCheck) //エラーがあったとき
            //{
            //    _errHeaderIndex = i;     // エラーとなったヘッダRowIndex
            //}

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
        /// <param name="r">
        ///     発注書行コレクション</param>
        /// <param name="ptn">
        ///     ClsOrderPatternクラス</param>
        /// <returns>
        ///     エラーなし：true, エラー有り：false</returns>
        ///-----------------------------------------------------------------------------------------------
        /// 
        public Boolean errCheckData(ClsFaxOrder r, Table<ClsOrderPattern> ptn)
        {
            int eNum = 0;

            // 発注数エラーコードセット
            for (int i = 0; i < 7; i++)
            {
                eSuu[i] = ESUU_CODE + i;
            }

            // 確認チェック
            if (r.Veri == global.flgOff)
            {
                setErrStatus(eDataCheck, 0, "未確認の発注書です");
                return false;
            }

            // 年月
            int rDate = r.Year * 100 + r.Month;
            int toDate = (DateTime.Today.Year * 100) + DateTime.Today.Month;
            if (rDate < toDate)
            {
                setErrStatus(eYearMonth, 0, "年月が正しくありません");
                return false;
            }

            if (r.Month < 1 || r.Month > 12)
            {
                setErrStatus(eMonth, 0, "月が正しくありません");
                return false;
            }

            // 得意先コード
            if (!getTdkStatus(r.TokuisakiCode.ToString().PadLeft(7, '0')))
            {
                setErrStatus(eTdkNo, 0, "不明な得意先コードです");
                return false;
            }

            // パターンID : 「０」はフリー入力可能とする 2017/08/22
            //if (r.patternID != global.flgOff)
            //{
            //    if (!ptn.Any(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber))
            //    {
            //        setErrStatus(ePattern, 0, "登録されていない発注書番号です");
            //        return false;
            //    }
            //}

            if (!ptn.Any(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber))
            {
                setErrStatus(ePattern, 0, "登録されていない発注書番号です");
                return false;
            }
            else
            {
                // パターンID取得
                OrderPattern = ptn.Single(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber);
            }

            ClsTenDate[] tenDates = new ClsTenDate[7];

            Utility.SetTenDate(tenDates, r);

            string eMsg = "";
            string strDate = "";
            int dYear = r.Year;
            int dMonth = r.Month;

            for (int i = 0; i < 7; i++)
            {
                strDate = tenDates[i].Year + "/" + tenDates[i].Month.ToString("D2") + "/" + tenDates[i].Day.ToString("D2");

                switch (i)
                {
                    case 0:
                        if (!ChkTenDate(strDate, out eMsg, Mon))
                        {
                            setErrStatus(eTenDate1, 0, eMsg);
                            return false;
                        }

                        break;

                    case 1:
                        if (!ChkTenDate(strDate, out eMsg, Tue))
                        {
                            setErrStatus(eTenDate2, 0, eMsg);
                            return false;
                        }
                        break;

                    case 2:
                        if (!ChkTenDate(strDate, out eMsg, Wed))
                        {
                            setErrStatus(eTenDate3, 0, eMsg);
                            return false;
                        }
                        break;

                    case 3:
                        if (!ChkTenDate(strDate, out eMsg, Thu))
                        {
                            setErrStatus(eTenDate4, 0, eMsg);
                            return false;
                        }
                        break;

                    case 4:
                        if (!ChkTenDate(strDate, out eMsg, Fri))
                        {
                            setErrStatus(eTenDate5, 0, eMsg);
                            return false;
                        }
                        break;

                    case 5:
                        if (!ChkTenDate(strDate, out eMsg, Sat))
                        {
                            setErrStatus(eTenDate6, 0, eMsg);
                            return false;
                        }
                        break;

                    case 6:
                        if (!ChkTenDate(strDate, out eMsg, Sun))
                        {
                            setErrStatus(eTenDate7, 0, eMsg);
                            return false;
                        }
                        break;

                    default:
                        break;
                }
            }

            //--------------------------------------------------------------------
            //
            //      商品発注明細クラス配列を作成
            //
            //--------------------------------------------------------------------
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
                        goods[i].Syubai = r.G_Syubai15;
                        break;

                    default:
                        break;
                }
            }

            bool ha = false;

            //--------------------------------------------------------------------
            //
            //      商品エラーチェック
            //
            //--------------------------------------------------------------------
            for (int i = 0; i < 15; i++)
            {
                if (!ChkShohin_NEW(goods, i, tenDates, out eMsg, out eNum))
                {
                    setErrStatus(eNum, i * 2 + 1, eMsg);
                    return false;
                }
            }


            return true;
        }

        ///-----------------------------------------------------------------------------------------------
        /// <summary>
        ///     項目別エラーチェック。
        ///     エラーのときヘッダ行インデックス、フィールド番号、明細行インデックス、エラーメッセージが記録される </summary>
        /// <param name="dts">
        ///     データセット</param>
        /// <param name="r">
        ///     発注書行コレクション</param>
        /// <returns>
        ///     エラーなし：true, エラー有り：false</returns>
        ///-----------------------------------------------------------------------------------------------
        /// 
        public Boolean errCheckData(ClsOrder r, Table<ClsOrderPattern> ptn)
        {
            int eNum = 0;

            // 発注数エラーコードセット
            for (int i = 0; i < 7; i++)
            {
                eSuu[i] = ESUU_CODE + i;
            }

            // 確認チェック
            if (r.Veri == global.flgOff)
            {
                setErrStatus(eDataCheck, 0, "未確認の発注書です");
                return false;
            }

            // 年月
            int rDate = r.Year * 100 + r.Month;
            int toDate = (DateTime.Today.Year * 100) + DateTime.Today.Month;
            if (rDate < toDate)
            {
                setErrStatus(eYearMonth, 0, "年月が正しくありません");
                return false;
            }

            if (r.Month < 1 || r.Month > 12)
            {
                setErrStatus(eMonth, 0, "月が正しくありません");
                return false;
            }

            // 得意先コード
            if (!getTdkStatus(r.TokuisakiCode.ToString().PadLeft(7, '0')))
            {
                setErrStatus(eTdkNo, 0, "不明な得意先コードです");
                return false;
            }

            //// パターンID : 「０」はフリー入力可能とする 2017/08/22
            //if (r.patternID != global.flgOff)
            //{
            //    if (!ptn.Any(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber))
            //    {
            //        setErrStatus(ePattern, 0, "登録されていない発注書番号です");
            //        return false;
            //    }
            //}

            // パターンID
            if (!ptn.Any(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber))
            {
                setErrStatus(ePattern, 0, "登録されていない発注書番号です");
                OrderPattern = null;
                return false;
            }
            else
            {
                // パターンID取得
                OrderPattern = ptn.Single(a => a.TokuisakiCode == r.TokuisakiCode && a.SeqNum == r.patternID && a.SecondNum == r.SeqNumber);
            }

            ClsTenDate[] tenDates = new ClsTenDate[7];

            Utility.SetTenDate(tenDates, r);

            string eMsg = "";
            string strDate = "";
            int dYear = r.Year;
            int dMonth = r.Month;

            for (int i = 0; i < 7; i++)
            {

                strDate = tenDates[i].Year + "/" + tenDates[i].Month.ToString("D2") + "/" + tenDates[i].Day.ToString("D2");

                switch (i)
                {
                    case 0:
                        if (!ChkTenDate(strDate, out eMsg, Mon))
                        {
                            setErrStatus(eTenDate1, 0, eMsg);
                            return false;
                        }

                        break;

                    case 1:
                        if (!ChkTenDate(strDate, out eMsg, Tue))
                        {
                            setErrStatus(eTenDate2, 0, eMsg);
                            return false;
                        }
                        break;

                    case 2:
                        if (!ChkTenDate(strDate, out eMsg, Wed))
                        {
                            setErrStatus(eTenDate3, 0, eMsg);
                            return false;
                        }
                        break;

                    case 3:
                        if (!ChkTenDate(strDate, out eMsg, Thu))
                        {
                            setErrStatus(eTenDate4, 0, eMsg);
                            return false;
                        }
                        break;

                    case 4:
                        if (!ChkTenDate(strDate, out eMsg, Fri))
                        {
                            setErrStatus(eTenDate5, 0, eMsg);
                            return false;
                        }
                        break;

                    case 5:
                        if (!ChkTenDate(strDate, out eMsg, Sat))
                        {
                            setErrStatus(eTenDate6, 0, eMsg);
                            return false;
                        }
                        break;

                    case 6:
                        if (!ChkTenDate(strDate, out eMsg, Sun))
                        {
                            setErrStatus(eTenDate7, 0, eMsg);
                            return false;
                        }
                        break;

                    default:
                        break;
                }
            }

            //--------------------------------------------------------------------
            //
            //      商品発注明細クラス配列を作成
            //
            //--------------------------------------------------------------------
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
                        goods[i].Syubai = r.G_Syubai15;
                        break;

                    default:
                        break;
                }
            }

            bool ha = false;

            //--------------------------------------------------------------------
            //
            //      商品エラーチェック
            //
            //--------------------------------------------------------------------
            for (int i = 0; i < 15; i++)
            {
                //ha = false;

                //// 発注の有無を調べる
                //for (int iX = 0; iX < 7; iX++)
                //{
                //    if (Utility.StrtoInt(goods[i].Suu[iX]) != global.flgOff)
                //    {
                //        // 発注あり
                //        ha = true;
                //    }
                //}

                //if (goods[i].Code == string.Empty)
                //{
                //    // 発注あり
                //    if (ha)
                //    {
                //        setErrStatus(eHinCode, i * 2 + 1, "商品が登録されていません");
                //        return false;
                //    }
                //}
                //else if (!ChkShohin(goods[i].Code, goods[i].Syubai, out eMsg, out eNum, ha))
                //{
                //    setErrStatus(eNum, i * 2 + 1, eMsg);
                //    return false;
                //}
                

                if (!ChkShohin_NEW(goods, i, tenDates, out eMsg, out eNum))
                {
                    setErrStatus(eNum, i * 2 + 1, eMsg);
                    return false;
                }
            }

            return true;
        }


        ///------------------------------------------------------------
        /// <summary>
        ///     商品コード検証 </summary>
        /// <param name="G_Code">
        ///     商品コード</param>
        /// <param name="G_Syubai">
        ///     終売処理</param>
        /// <param name="eMsg">
        ///     エラーメッセージ</param>
        /// <param name="eNum">
        ///     エラー箇所</param>
        /// <param name="ha">
        ///     発注の有無　true:発注あり, false:発注なし</param>
        /// <returns>
        ///     エラーなし：true, エラー有り：false</returns>
        ///------------------------------------------------------------

        private bool ChkShohin(string G_Code, int G_Syubai, out string eMsg, out int eNum, bool ha)
        {
            // 商品コードマスター登録チェック
            ClsCsvData.ClsCsvSyohin syohin = Utility.GetSyohinData(G_Code);

            // 商品マスター未登録
            if (syohin.SYOHIN_CD == string.Empty)
            {
                eNum = eHinCode;
                eMsg = "マスター未登録または削除済みの商品です";
                return false;
            }

            // 終売で発注ありのとき
            if (syohin.SHUBAI && ha)
            {
                if (G_Syubai == global.flgOff)
                {
                    eNum = eShubai;
                    eMsg = "該当商品は終売です。終売処理を選択してください";
                    return false;
                }
            }

            eNum = global.flgOff;
            eMsg = "";
            return true;
        }

        ///-----------------------------------------------------------------------------------
        /// <summary>
        ///     商品チェック </summary>
        /// <param name="Goods">
        ///     商品明細クラス</param>
        /// <param name="iX">
        ///     商品明細インデックス</param>
        /// <param name="tenDates">
        ///     店着日クラス配列</param>
        /// <param name="eMsg">
        ///     エラーメッセージ</param>
        /// <param name="eNum">
        ///     エラー箇所番号</param>
        /// <returns>
        ///     検証結果 true:エラーなし, false:エラーあり</returns>
        ///-----------------------------------------------------------------------------------
        private bool ChkShohin_NEW(ClsGoods[] Goods, int iX, ClsTenDate [] tenDates, out string eMsg, out int eNum)
        {
            bool ha = false;

            // 発注の有無を調べる
            for (int i = 0; i < 7; i++)
            {
                if (Utility.StrtoInt(Goods[iX].Suu[i]) != global.flgOff)
                {
                    // 発注あり
                    ha = true;
                    break;
                }
            }

            // 商品登録なしで発注なしのときはネグる
            if (Goods[iX].Code == string.Empty && !ha)
            {
                eNum = global.flgOff;
                eMsg = "";
                return true;
            }


            if (Goods[iX].Code == string.Empty)
            {
                // 発注あり
                if (ha)
                {
                    eNum = eHinCode;
                    eMsg = "商品が登録されていません";
                    return false;
                }
            }

            // 商品コードマスター登録チェック
            ClsCsvData.ClsCsvSyohin syohin = Utility.GetSyohins(syohins, Utility.NulltoStr(Goods[iX].Code));

            // 商品マスター未登録
            if (syohin.SYOHIN_CD == string.Empty)
            {
                eNum = eHinCode;
                eMsg = "マスター未登録または削除済みの商品です";
                return false;
            }

            // 店着日付とリード日数
            int Read = 0;
            for (int i = 0; i < 7; i++)
            {
                if (Utility.StrtoInt(Goods[iX].Suu[i]) != global.flgOff)
                {
                    // 店着日
                    DateTime tDate = new DateTime(tenDates[i].Year, tenDates[i].Month, tenDates[i].Day);

                    // リード日数
                    switch (iX)
                    {
                        case 0:
                            Read = OrderPattern.G_Read1;
                            break;

                        case 1:
                            Read = OrderPattern.G_Read2;
                            break;

                        case 2:
                            Read = OrderPattern.G_Read3;
                            break;

                        case 3:
                            Read = OrderPattern.G_Read4;
                            break;

                        case 4:
                            Read = OrderPattern.G_Read5;
                            break;

                        case 5:
                            Read = OrderPattern.G_Read6;
                            break;

                        case 6:
                            Read = OrderPattern.G_Read7;
                            break;

                        case 7:
                            Read = OrderPattern.G_Read8;
                            break;

                        case 8:
                            Read = OrderPattern.G_Read9;
                            break;

                        case 9:
                            Read = OrderPattern.G_Read10;
                            break;

                        case 10:
                            Read = OrderPattern.G_Read11;
                            break;

                        case 11:
                            Read = OrderPattern.G_Read12;
                            break;

                        case 12:
                            Read = OrderPattern.G_Read13;
                            break;

                        case 13:
                            Read = OrderPattern.G_Read14;
                            break;

                        case 14:
                            Read = OrderPattern.G_Read15;
                            break;

                        default:
                            break;
                    }

                    if (tDate.AddDays(-1 * Read) < DateTime.Today)
                    {
                        eNum = eSuu[i];
                        eMsg = "店着日：" + tDate.ToShortDateString() + "、リード日数：" + Read;
                        return false;
                    }
                }
            }


            // 終売で発注ありのとき
            if (syohin.SHUBAI && ha)
            {
                if (Goods[iX].Syubai == global.flgOff)
                {
                    eNum = eShubai;
                    eMsg = "該当商品は終売です。終売処理を選択してください";
                    return false;
                }
            }

            eNum = global.flgOff;
            eMsg = "";
            return true;
        }



        ///-------------------------------------------------------------------
        /// <summary>
        ///     店着日付の検証 </summary>
        /// <param name="rDate">
        ///     店着日付文字列(yyyy/mm/dd) </param>
        /// <param name="eMsg"><
        ///     エラーメッセージ/param>
        /// <param name="week">
        ///     曜日(Sun:0, Mon:1, ... Sat:6)</param>
        /// <returns>
        ///     エラーなし：true, エラー：false</returns>
        ///-------------------------------------------------------------------
        private bool ChkTenDate(string rDate, out string eMsg, int week)
        {
            DateTime dt;

            if (!DateTime.TryParse(rDate, out dt))
            {
                eMsg = "店着日付が正しくありません";
                return false;
            }

            DayOfWeek wk = dt.DayOfWeek;

            if ((Int32)wk != week)
            {
                eMsg = "店着日付の曜日が正しくありません";
                return false;
            }

            eMsg = "";
            return true;
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

            //string _Tel = "";
            //string _Jyu = "";

            // 得意先番号
            //if (Utility.getNouhinName(tCode, out _Tel, out _Jyu) != string.Empty)
            //{
            //    rtn = true;
            //}

            for (int i = 0; i < tokuisaki.Length; i++)
            {
                if (tokuisaki[i].TOKUISAKI_CD == tCode.PadLeft(7, '0'))
                {
                    rtn = true;
                    break;
                }
            }

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
