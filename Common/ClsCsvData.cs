using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STSH_OCR.Common
{
    public class ClsCsvData
    {
        ///--------------------------------------------------------------
        /// <summary>
        ///     商品情報クラス Ver.2 </summary>        
        ///--------------------------------------------------------------
        public class ClsCsvSyohin_New
        {
            // 商品コード
            public string SYOHIN_CD { get; set; }

            // 商品名
            public string SYOHIN_NM { get; set; }

            // 商品略称
            public string SYOHIN_SNM { get; set; }

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

            // 販売開始日付
            public string START_SALE_YMD { get; set; }

            // 販売終了日付
            public string LAST_SALE_YMD { get; set; }

            // 終売フラグ
            public bool SHUBAI { get; set; }

            // 大分類
            public string SYOHIN_KIND_L_CD { get; set; }

            // 中分類
            public string SYOHIN_KIND_M_CD { get; set; }

            // 小分類
            public string SYOHIN_KIND_S_CD { get; set; }

            // 商品分類
            public string SYOHIN_KIND_CD { get; set; }
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

            // 得意先カナ名称
            public string TOKUISAKI_KANA_NM { get; set; }

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


            ///-----------------------------------------------------------------
            /// <summary>
            ///     得意先クラス配列作成 </summary>
            /// <param name="Tk_Array">
            ///     得意先マスターcsvデータ配列</param>
            /// <param name="sDate">
            ///     基準日</param>
            /// <returns>
            ///     ClsCsvTokuisakiクラス配列</returns>
            ///-----------------------------------------------------------------
            public static ClsCsvTokuisaki[] Load_20200408 (string[] Tk_Array, int sDate)
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


        ///--------------------------------------------------
        /// <summary>
        ///     納価売価クラス </summary>
        ///--------------------------------------------------
        public class ClsCsvNoukaBaika
        {
            // 商品コード
            public string SYOHIN_CD { get; set; }

            // 得意先コード
            public string TOKUISAKI_CD { get; set; }

            // 納価
            //public int NOUKA { get; set; }    // 2020/08/04 コメント化
            public double NOUKA { get; set; }   // 202020/08/04 浮動小数点

            // 売価
            //public int BAIKA { get; set; }    // 2020/08/04 コメント化
            public double BAIKA { get; set; }   // 202020/08/04 浮動小数点
        }
    }
}
