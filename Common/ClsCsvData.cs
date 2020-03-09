using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSH_OCR.Common
{
    class ClsCsvData
    {
        ///--------------------------------------------------
        /// <summary>
        ///     商品情報クラス </summary>
        ///--------------------------------------------------
        public class ClsCsvSyohin
        {
            // 商品コード
            public string SYOHIN_CD { get; set; }

            // 商品名
            public string SYOHIN_NM { get; set; }

            // 商品略称
            public string SYOHIN_SNM { get; set; }

            // 商品カナ
            public string SYOHIN_KANA { get; set; }

            // 仕入先コード
            public string SIRESAKI_CD { get; set; }

            // 仕入先名
            public string SIRESAKI_NM { get; set; }
            
            // 仕入先名カナ
            public string SIRESAKI_KANA_NM { get; set; }

            // JANコード
            public string JAN_CD { get; set; }

            // 規格
            public string SYOHIN_KIKAKU { get; set; }

            // ケース入数（バラ換算）
            public double CASE_IRISU { get; set; }

            // 納品単価（仮単価）
            public double NOUHIN_KARI_TANKA { get; set; }

            // 標準小売単価（希望小売価格）
            public double RETAIL_TANKA { get; set; }

            // 発注可能日数
            public double HATYU_LIMIT_DAY_CNT { get; set; }

            // 販売開始日付
            public string START_SALE_YMD { get; set; }
            
            // 商品販売開始日付
            public string LAST_SALE_YMD { get; set; }

            // 終売フラグ
            public bool SHUBAI { get; set; }

            public static ClsCsvSyohin[] Load(string[] Sy_Array, string[] SySz_Array, string [] Shiire_Array, int sDate)
            {
                ClsCsvSyohin[] syohins = null;
                int x = 0;

                foreach (var item in Sy_Array)
                {
                    string cStart_Sale_YMD = "";    // 商品販売開始日付
                    string cLast_Sale_YMD = "";     // 商品販売終了日付
                    string[] t = item.Split(',');

                    // 削除フラグ
                    string DelFlg = t[63].Replace("\"", "");

                    // 1行目見出し行は読み飛ばす
                    if (DelFlg == "DELFLG")
                    {
                        continue;
                    }

                    if (DelFlg == global.FLGON)
                    {
                        continue;
                    }

                    // 商品在庫マスターで終売を調べる
                    bool Shubai = false;
                    foreach (var sz in SySz_Array)
                    {
                        string[] z = sz.Split(',');

                        // 削除フラグ
                        string zDelFlg = z[10].Replace("\"", "");

                        // 1行目見出し行は読み飛ばす
                        if (zDelFlg == "DELFLG")
                        {
                            continue;
                        }

                        if (zDelFlg == global.FLGON)
                        {
                            continue;
                        }

                        if (t[1].Replace("\"", "") != z[2].Replace("\"", ""))
                        {
                            continue;
                        }

                        // 有効開始日、有効終了日を検証する
                        cStart_Sale_YMD = z[3].Replace("\"", "");    // 商品販売開始日付
                        cLast_Sale_YMD = z[4].Replace("\"", "");     // 商品販売終了日付（終売日）

                        if (Utility.StrtoInt(cStart_Sale_YMD) > sDate)
                        {
                            continue;
                        }

                        if (sDate > Utility.StrtoInt(cLast_Sale_YMD))
                        {
                            continue;
                        }

                        Shubai = true;
                        break;
                    }

                    // 終売（商品販売期間に該当しないとき）
                    if (Shubai)
                    {
                        continue;
                    }

                    // メーカー名（仕入先）取得
                    string ShiiresakiName = string.Empty;
                    string ShiiresakiKana = string.Empty;

                    foreach (var sr in Shiire_Array)
                    {
                        string[] z = sr.Split(',');

                        // 削除フラグ
                        string zDelFlg = z[80].Replace("\"", "");

                        // 1行目見出し行は読み飛ばす
                        if (zDelFlg == "DELFLG")
                        {
                            continue;
                        }

                        if (zDelFlg == global.FLGON)
                        {
                            continue;
                        }

                        // 仕入先コード
                        if (t[13].Replace("\"", "") != z[1].Replace("\"", ""))
                        {
                            continue;
                        }

                        ShiiresakiName = z[4].Replace("\"", "");
                        ShiiresakiKana = z[7].Replace("\"", "");
                        break;
                    }

                    //// 仕入先名検索のとき
                    //if (txtMaker.Text.Trim() != string.Empty)
                    //{
                    //    if (!ShiiresakiName.Contains(txtMaker.Text))
                    //    {
                    //        continue;
                    //    }
                    //}

                    //// 商品名検索のとき
                    //if (txtSyohinName.Text.Trim() != string.Empty)
                    //{
                    //    if (!t[2].Replace("\"", "").Contains(txtSyohinName.Text))
                    //    {
                    //        continue;
                    //    }
                    //}

                    // 小売り単価：新単価適用日で判断
                    double _RETAIL_TANKA = 0;
                    if (sDate < Utility.StrtoInt(t[34].Replace("\"", "")))
                    {
                        _RETAIL_TANKA = Utility.StrtoDouble(t[32].Replace("\"", ""));
                    }
                    else
                    {
                        _RETAIL_TANKA = Utility.StrtoDouble(t[35].Replace("\"", ""));
                    }

                    Array.Resize(ref syohins, x + 1);

                    syohins[x] = new ClsCsvSyohin
                    {
                        SYOHIN_CD = t[1].Replace("\"", ""),
                        SYOHIN_NM = t[2].Replace("\"", ""),
                        SYOHIN_KANA = t[4].Replace("\"", ""),
                        SYOHIN_KIKAKU = t[19].Replace("\"", ""),
                        CASE_IRISU = Utility.StrtoDouble(Utility.NulltoStr(t[24].Replace("\"", ""))),
                        SIRESAKI_CD = t[13].Replace("\"", ""),
                        SIRESAKI_NM = ShiiresakiName,
                        SIRESAKI_KANA_NM = ShiiresakiKana,
                        JAN_CD = t[16].Replace("\"", ""),
                        NOUHIN_KARI_TANKA = Utility.StrtoDouble(Utility.NulltoStr(t[31].Replace("\"", ""))),
                        RETAIL_TANKA = _RETAIL_TANKA,
                        HATYU_LIMIT_DAY_CNT = Utility.StrtoDouble(t[39].Replace("\"", "")),
                        START_SALE_YMD = cStart_Sale_YMD,
                        LAST_SALE_YMD = cLast_Sale_YMD
                    };

                    x++;
                }

                return syohins;
            }
            ///--------------------------------------------------
            ///--------------------------------------------------
        }
        
        ///--------------------------------------------------
        /// <summary>
        ///     得意先情報クラス </summary>
        ///--------------------------------------------------
        public class ClsCsvTokuisaki
        {
            // 得意先コード
            public string TOKUISAKI_CD { get; set; }

            // 有効開始日付
            public string YUKO_START_YMD { get; set; }

            // 有効終了日付
            public string YUKO_END_YMD { get; set; }

            // 得意先名称
            public string TOKUISAKI_NM { get; set; }

            // 郵便番号
            public string TOKUISAKI_YUBIN_NO { get; set; }

            // 住所１
            public string TOKUISAKI_ZYUSYO1 { get; set; }

            // 住所２
            public string TOKUISAKI_ZYUSYO2 { get; set; }

            // 電話番号
            public string TOKUISAKI_TEL { get; set; }

            // ＦＡＸ番号
            public string TOKUISAKI_FAX { get; set; }

            // 削除フラグ
            public string DELFLG { get; set; }


            public static ClsCsvTokuisaki[] Load(string[] Tk_Array, int sDate)
            {
                ClsCsvTokuisaki[] tokuisakis = null;
                int x = 0;

                foreach (var item in Tk_Array)
                {
                    string[] t = item.Split(',');

                    // 削除フラグ
                    string DelFlg = t[119].Replace("\"", "");

                    // 1行目見出し行は読み飛ばす
                    if (DelFlg == "DELFLG")
                    {
                        continue;
                    }

                    if (DelFlg == global.FLGON)
                    {
                        continue;
                    }

                    // 有効開始日、有効終了日を検証する
                    string cYuko_Start_Date = t[2].Replace("\"", "");   // 有効開始日付
                    string cYuko_End_Date = t[3].Replace("\"", "");   // 有効終了日付

                    //int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cYuko_Start_Date) > sDate)
                    {
                        continue;
                    }

                    if (sDate > Utility.StrtoInt(cYuko_End_Date))
                    {
                        continue;
                    }

                    Array.Resize(ref tokuisakis, x + 1);

                    tokuisakis[x] = new ClsCsvTokuisaki
                    {
                        TOKUISAKI_CD = t[1].Replace("\"", ""),        // 得意先コード                    
                        TOKUISAKI_NM = t[4].Replace("\"", ""),        // 得意先名称                    
                        TOKUISAKI_YUBIN_NO = t[24].Replace("\"", ""), // 得意先郵便番号                    
                        TOKUISAKI_ZYUSYO1 = t[25].Replace("\"", ""),  // 得意先住所
                        TOKUISAKI_ZYUSYO2 = t[26].Replace("\"", ""),  // 得意先住所                    
                        TOKUISAKI_TEL = t[27].Replace("\"", ""),      // 得意先TEL                    
                        TOKUISAKI_FAX = t[28].Replace("\"", ""),      // 得意先FAX
                        YUKO_START_YMD = t[2].Replace("\"", ""),      // 有効開始日付
                        YUKO_END_YMD = t[3].Replace("\"", ""),        // 有効終了日付      
                        DELFLG = t[119].Replace("\"", "")            // 削除フラグ
                    };

                    x++;
                }

                return tokuisakis;
            }
        }
        
        ///--------------------------------------------------
        /// <summary>
        ///     CSVデータクラス </summary>
        ///--------------------------------------------------
        public class ClsCsvCSV
        {
            // 得意先コード
            public string TOKUISAKI_CD { get; set; }

            // 得意先名称
            public string TOKUISAKI_NM { get; set; }

            // 納品日
            public string NOUHIN_DATE { get; set; }

            // 商品コード
            public string SYOHIN_CD { get; set; }

            // 商品名
            public string SYOHIN_NM { get; set; }

            // 数量
            public string SUU { get; set; }

            // 納価
            public string NOUKA { get; set; }

            // 売価
            public string BAIKA { get; set; }

            // ＤＴ区分
            public string DT_KBN { get; set; }
        }
    }
}
