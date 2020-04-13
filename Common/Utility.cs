using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Data;
using System.Data.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing;
//using Excel = Microsoft.Office.Interop.Excel;

namespace STSH_OCR.Common
{
    class Utility
    {
        ///----------------------------------------------------------------------
        /// <summary>
        ///     ウィンドウ最小サイズの設定 </summary>
        /// <param name="tempFrm">
        ///     対象とするウィンドウオブジェクト</param>
        /// <param name="wSize">
        ///     width</param>
        /// <param name="hSize">
        ///     Height</param>
        ///----------------------------------------------------------------------
        public static void WindowsMinSize(Form tempFrm, int wSize, int hSize)
        {
            tempFrm.MinimumSize = new System.Drawing.Size(wSize, hSize);
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     ウィンドウ最小サイズの設定 </summary>
        /// <param name="tempFrm">
        ///     対象とするウィンドウオブジェクト</param>
        /// <param name="wSize">
        ///     width</param>
        /// <param name="hSize">
        ///     height</param>
        ///----------------------------------------------------------------------
        public static void WindowsMaxSize(Form tempFrm, int wSize, int hSize)
        {
            tempFrm.MaximumSize = new System.Drawing.Size(wSize, hSize);
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     文字列の値が数字かチェックする </summary>
        /// <param name="tempStr">
        ///     検証する文字列</param>
        /// <returns>
        ///     数字:true,数字でない:false</returns>
        ///------------------------------------------------------------------------
        public static bool NumericCheck(string tempStr)
        {
            double d;

            if (tempStr == null) return false;

            if (double.TryParse(tempStr, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out d) == false)
                return false;

            return true;
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     emptyを"0"に置き換える </summary>
        /// <param name="tempStr">
        ///     stringオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのときそのまま値を返す</returns>
        ///------------------------------------------------------------------------
        public static string EmptytoZero(string tempStr)
        {
            if (tempStr == string.Empty)
            {
                return "0";
            }
            else
            {
                return tempStr;
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     Nullをstring.Empty("")に置き換える </summary>
        /// <param name="tempStr">
        ///     stringオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのとき文字型値を返す</returns>
        ///------------------------------------------------------------------------
        public static string NulltoStr(string tempStr)
        {
            if (tempStr == null)
            {
                return string.Empty;
            }
            else
            {
                return tempStr;
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     Nullをstring.Empty("")に置き換える </summary>
        /// <param name="tempStr">
        ///     stringオブジェクト</param>
        /// <returns>
        ///     nullのときstring.Empty、not nullのときそのまま値を返す</returns>
        ///------------------------------------------------------------------------
        public static string NulltoStr(object tempStr)
        {
            if (tempStr == null)
            {
                return string.Empty;
            }
            else
            {
                if (tempStr == DBNull.Value)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)tempStr.ToString();
                }
            }
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     文字型をIntへ変換して返す（数値でないときは０を返す） </summary>
        /// <param name="tempStr">
        ///     文字型の値</param>
        /// <returns>
        ///     Int型の値</returns>
        ///----------------------------------------------------------------------
        public static int StrtoInt(string tempStr)
        {
            if (NumericCheck(tempStr)) return int.Parse(tempStr);
            else return 0;
        }

        ///----------------------------------------------------------------------
        /// <summary>
        ///     文字型をDoubleへ変換して返す（数値でないときは０を返す）</summary>
        /// <param name="tempStr">
        ///     文字型の値</param>
        /// <returns>
        ///     double型の値</returns>
        ///----------------------------------------------------------------------
        public static double StrtoDouble(string tempStr)
        {
            if (NumericCheck(tempStr)) return double.Parse(tempStr);
            else return 0;
        }

        ///-----------------------------------------------------------------------
        /// <summary>
        ///     経過時間を返す </summary>
        /// <param name="s">
        ///     開始時間</param>
        /// <param name="e">
        ///     終了時間</param>
        /// <returns>
        ///     経過時間</returns>
        ///-----------------------------------------------------------------------
        public static TimeSpan GetTimeSpan(DateTime s, DateTime e)
        {
            TimeSpan ts;
            if (s > e)
            {
                TimeSpan j = new TimeSpan(24, 0, 0);
                ts = e + j - s;
            }
            else
            {
                ts = e - s;
            }

            return ts;
        }

        /// ------------------------------------------------------------------------
        /// <summary>
        ///     指定した精度の数値に切り捨てします。</summary>
        /// <param name="dValue">
        ///     丸め対象の倍精度浮動小数点数。</param>
        /// <param name="iDigits">
        ///     戻り値の有効桁数の精度。</param>
        /// <returns>
        ///     iDigits に等しい精度の数値に切り捨てられた数値。</returns>
        /// ------------------------------------------------------------------------
        public static double ToRoundDown(double dValue, int iDigits)
        {
            double dCoef = System.Math.Pow(10, iDigits);

            return dValue > 0 ? System.Math.Floor(dValue * dCoef) / dCoef :
                                System.Math.Ceiling(dValue * dCoef) / dCoef;
        }
        
        ///------------------------------------------------------------------
        /// <summary>
        ///     ファイル選択ダイアログボックスの表示 </summary>
        /// <param name="sTitle">
        ///     タイトル文字列</param>
        /// <param name="sFilter">
        ///     ファイルのフィルター</param>
        /// <returns>
        ///     選択したファイル名</returns>
        ///------------------------------------------------------------------
        public static string userFileSelect(string sTitle, string sFilter)
        {
            DialogResult ret;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //ダイアログボックスの初期設定
            openFileDialog1.Title = sTitle;
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = sFilter;
            //openFileDialog1.Filter = "CSVファイル(*.CSV)|*.csv|全てのファイル(*.*)|*.*";

            //ダイアログボックスの表示
            ret = openFileDialog1.ShowDialog();
            if (ret == System.Windows.Forms.DialogResult.Cancel)
            {
                return string.Empty;
            }

            if (MessageBox.Show(openFileDialog1.FileName + Environment.NewLine + " が選択されました。よろしいですか?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return string.Empty;
            }

            return openFileDialog1.FileName;
        }

        public class frmMode
        {
            public int ID { get; set; }

            public int Mode { get; set; }

            public int rowIndex { get; set; }
        }

        public class xlsShain
        {
            public int sCode { get; set; }
            public string sName { get; set; }
            public int bCode { get; set; }
            public string bName { get; set; }
        }
        
        
        ///---------------------------------------------------------------------
        /// <summary>
        ///     任意のディレクトリのファイルを削除する </summary>
        /// <param name="sPath">
        ///     指定するディレクトリ</param>
        /// <param name="sFileType">
        ///     ファイル名及び形式</param>
        /// --------------------------------------------------------------------
        public static void FileDelete(string sPath, string sFileType)
        {
            //sFileTypeワイルドカード"*"は、すべてのファイルを意味する
            foreach (string files in System.IO.Directory.GetFiles(sPath, sFileType))
            {
                // ファイルを削除する
                System.IO.File.Delete(files);
            }
        }



        ///---------------------------------------------------------------------
        /// <summary>
        ///     文字列を指定文字数をＭＡＸとして返します</summary>
        /// <param name="s">
        ///     文字列</param>
        /// <param name="n">
        ///     文字数</param>
        /// <returns>
        ///     文字数範囲内の文字列</returns>
        /// --------------------------------------------------------------------
        public static string GetStringSubMax(string s, int n)
        {
            string val = string.Empty;

            // 文字間のスペースを除去 2015/03/10
            s = s.Replace(" ", "");

            if (s.Length > n) val = s.Substring(0, n);
            else val = s;

            return val;
        }


        ///-------------------------------------------------------------------------
        /// <summary>
        ///     自らのロックファイルが存在したら削除する </summary>
        /// <param name="fPath">
        ///     パス</param>
        /// <param name="PcK">
        ///     自分のロックファイル文字列</param>
        ///-------------------------------------------------------------------------
        public static void deleteLockFile(string fPath, string PcK)
        {
            string FileName = fPath + global.LOCK_FILEHEAD + PcK + ".loc";

            if (System.IO.File.Exists(FileName))
            {
                System.IO.File.Delete(FileName);
            }
        }

        ///-------------------------------------------------------------------------
        /// <summary>
        ///     データフォルダにロックファイルが存在するか調べる </summary>
        /// <param name="fPath">
        ///     データフォルダパス</param>
        /// <returns>
        ///     true:ロックファイルあり、false:ロックファイルなし</returns>
        ///-------------------------------------------------------------------------
        public static Boolean existsLockFile(string fPath)
        {
            int s = System.IO.Directory.GetFiles(fPath, global.LOCK_FILEHEAD + "*.*", System.IO.SearchOption.TopDirectoryOnly).Count();

            if (s == 0)
            {
                return false; //LOCKファイルが存在しない
            }
            else
            {
                return true;   //存在する
            }
        }

        ///----------------------------------------------------------------
        /// <summary>
        ///     ロックファイルを登録する </summary>
        /// <param name="fPath">
        ///     書き込み先フォルダパス</param>
        /// <param name="LocName">
        ///     ファイル名</param>
        ///----------------------------------------------------------------
        public static void makeLockFile(string fPath, string LocName)
        {
            string FileName = fPath + global.LOCK_FILEHEAD + LocName + ".loc";

            //存在する場合は、処理なし
            if (System.IO.File.Exists(FileName))
            {
                return;
            }

            // ロックファイルを登録する
            try
            {
                System.IO.StreamWriter outFile = new System.IO.StreamWriter(FileName, false, System.Text.Encoding.GetEncoding(932));
                outFile.Close();
            }
            catch
            {
            }

            return;
        }

        ///---------------------------------------------------------------------
        /// <summary>
        ///     楽商商品コードを8桁頭ゼロ埋め文字列に変換する </summary>
        /// <param name="s">
        ///     商品コード</param>
        /// <returns>
        ///     変換後文字列</returns>
        ///---------------------------------------------------------------------
        public static string ptnShohinStr(int s)
        {
            string val = string.Empty;

            if (s == global.flgOff)
            {
                val = string.Empty;
            }
            else
            {
                val = s.ToString().PadLeft(8, '0');
            }

            return val;
        }

        ///-----------------------------------------------------------------------------
        /// <summary>
        ///     得意先情報をDataTableからClsCsvData.ClsCsvTokuisakiクラスに取得 : 
        ///     2020/04/09</summary>
        /// <param name="tID">
        ///     得意先コード</param>
        /// <returns>
        ///     ClsCsvData.ClsCsvTokuisakiクラス</returns>
        ///-----------------------------------------------------------------------------
        public static ClsCsvData.ClsCsvTokuisaki GetTokuisakiFromDataTable(string tID, System.Data.DataTable data)
        {
            // 返り値クラス初期化
            ClsCsvData.ClsCsvTokuisaki cls = new ClsCsvData.ClsCsvTokuisaki
            {
                TOKUISAKI_CD = "",
                YUKO_START_YMD = "",
                YUKO_END_YMD = "",
                TOKUISAKI_NM = "",
                TOKUISAKI_KANA_NM = "",
                TOKUISAKI_YUBIN_NO = "",
                TOKUISAKI_ZYUSYO1 = "",
                TOKUISAKI_ZYUSYO2 = "",
                TOKUISAKI_TEL = "",
                TOKUISAKI_FAX = "",
                DELFLG = global.FLGOFF
            };

            DataRow[] rows = data.AsEnumerable().Where(a => a["TOKUISAKI_CD"].ToString().PadLeft(7, '0') == tID && a["DELFLG"].ToString() == global.FLGOFF).ToArray();

            foreach (var t in rows)
            {
                // 有効開始日、有効終了日を検証する
                int cYuko_Start_Date = Utility.StrtoInt(t["YUKO_START_YMD"].ToString());    // 有効開始日付
                int cYuko_End_Date = Utility.StrtoInt(t["YUKO_END_YMD"].ToString());        // 有効終了日付
                int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                if (cYuko_Start_Date > toDate)
                {
                    continue;
                }

                if (cYuko_End_Date != global.flgOff)
                {
                    if (toDate > cYuko_End_Date)
                    {
                        continue;
                    }
                }

                cls.TOKUISAKI_CD = t["TOKUISAKI_CD"].ToString();                // 得意先コード
                cls.YUKO_START_YMD = t["YUKO_START_YMD"].ToString();            // 有効開始日付
                cls.YUKO_END_YMD = t["YUKO_END_YMD"].ToString();                // 有効終了日付
                cls.TOKUISAKI_NM = t["TOKUISAKI_NM"].ToString();                // 得意先名称
                cls.TOKUISAKI_KANA_NM = t["TOKUISAKI_KANA_NM"].ToString();      // 得意先カナ名称
                cls.TOKUISAKI_YUBIN_NO = t["TOKUISAKI_YUBIN_NO"].ToString();    // 郵便番号
                cls.TOKUISAKI_ZYUSYO1 = t["TOKUISAKI_ZYUSYO1"].ToString();      // 得意先住所
                cls.TOKUISAKI_ZYUSYO2 = t["TOKUISAKI_ZYUSYO2"].ToString();      // 得意先住所
                cls.TOKUISAKI_TEL = t["TOKUISAKI_TEL"].ToString();              // 得意先TEL
                cls.TOKUISAKI_FAX = t["TOKUISAKI_FAX"].ToString();              // 得意先FAX
                cls.DELFLG = t["DELFLG"].ToString();                            // 削除フラグ

                break;
            }

            return cls;
        }



        ///-----------------------------------------------------------------------------
        /// <summary>
        ///     納価売価をDataTableからClsCsvData.ClsCsvNoukaBaikaクラスに取得 : 
        ///     2020/04/10</summary>
        /// <param name="tCD">
        ///     得意先コード </param>
        /// <param name="sCD">
        ///     商品コード </param>
        /// <param name="data">
        ///     System.Data.DataTable</param>
        ///-----------------------------------------------------------------------------
        public static ClsCsvData.ClsCsvNoukaBaika GetNoukaBaikaFromDataTable(string tCD, string sCD, System.Data.DataTable data)
        {
            // 返り値クラス初期化
            ClsCsvData.ClsCsvNoukaBaika cls = new ClsCsvData.ClsCsvNoukaBaika()
            {
                SYOHIN_CD = "",
                TOKUISAKI_CD = "",
                NOUKA = global.flgOff,
                BAIKA = global.flgOff
            };

            DataRow[] rows = data.AsEnumerable().Where(a => a["得意先コード"].ToString().PadLeft(7, '0') == tCD && 
                                                            a["商品コード"].ToString().PadLeft(8, '0') == sCD).ToArray();

            foreach (var t in rows)
            {
                cls.TOKUISAKI_CD = t["得意先コード"].ToString();      // 得意先コード
                cls.SYOHIN_CD = t["商品コード"].ToString();           // 商品コード
                cls.NOUKA = Utility.StrtoInt(t["納価"].ToString());   // 納価
                cls.BAIKA = Utility.StrtoInt(t["売価"].ToString());   // 売価

                break;
            }

            return cls;
        }

        ///---------------------------------------------------------------------------
        /// <summary>
        ///     店着日配列を作成 </summary>
        /// <param name="tenDates">
        ///     店着日配列 </param>
        /// <param name="r">
        ///     ClsOrderクラス </param>
        ///---------------------------------------------------------------------------
        public static void SetTenDate(ClsTenDate [] tenDates, ClsOrder r)
        {
            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Day = r.Day1.Trim();

            if (r.Day1 != string.Empty)
            {
                tenDates[0].Year = r.Year.ToString();
                tenDates[0].Month = r.Month.ToString();
            }
            else
            {
                // 日付が無記入のときは年月も空白
                tenDates[0].Year = string.Empty;
                tenDates[0].Month = string.Empty;
            }

            tenDates[1].Day = r.Day2.Trim();
            tenDates[2].Day = r.Day3.Trim();
            tenDates[3].Day = r.Day4.Trim();
            tenDates[4].Day = r.Day5.Trim();
            tenDates[5].Day = r.Day6.Trim();
            tenDates[6].Day = r.Day7.Trim();

            int sYear = r.Year;
            int sMonth = r.Month;
            bool NextMonth = false;
            string wDay = "";

            // 店着日付（年月日）をセット
            for (int i = 1; i < tenDates.Length; i++)
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
            }
        }

        ///--------------------------------------------------------------------------------
        /// <summary>
        ///     店着日付クラス配列作成 </summary>
        /// <param name="tenDates">
        ///     店着日付クラス配列</param>
        /// <param name="r">
        ///     ClsFaxOrderクラス</param>
        ///--------------------------------------------------------------------------------
        public static void SetTenDate(ClsTenDate[] tenDates, ClsFaxOrder r)
        {
            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Day = r.Day1.Trim();

            if (r.Day1 != string.Empty)
            {
                tenDates[0].Year = r.Year.ToString();
                tenDates[0].Month = r.Month.ToString();
            }
            else
            {
                // 日付が無記入のときは年月も空白
                tenDates[0].Year = string.Empty;
                tenDates[0].Month = string.Empty;
            }

            tenDates[1].Day = r.Day2.Trim();
            tenDates[2].Day = r.Day3.Trim();
            tenDates[3].Day = r.Day4.Trim();
            tenDates[4].Day = r.Day5.Trim();
            tenDates[5].Day = r.Day6.Trim();
            tenDates[6].Day = r.Day7.Trim();

            int sYear = r.Year;
            int sMonth = r.Month;
            bool NextMonth = false;
            string wDay = "";

            // 店着日付（年月日）をセット
            for (int i = 1; i < tenDates.Length; i++)
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
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品マスターデータテーブルから情報を取得する : 2020/04/09 </summary>
        /// <param name="syohins">
        ///     ClsCsvData.ClsCsvSyohin_Newクラス配列</param>
        /// <param name="sSyohinCD">
        ///     商品コード  </param>
        /// <returns>
        ///     商品マスタークラス</returns>
        ///------------------------------------------------------------------------
        public static ClsCsvData.ClsCsvSyohin_New GetSyohinsFromDataTable(System.Data.DataTable data, string sSyohinCD)
        {
            ClsCsvData.ClsCsvSyohin_New cls = new ClsCsvData.ClsCsvSyohin_New
            {
                SYOHIN_CD = "",
                SYOHIN_NM = "",
                SYOHIN_SNM = "",
                //SYOHIN_KANA = "",
                SIRESAKI_CD = "",
                SIRESAKI_NM = "",
                SIRESAKI_KANA_NM = "",
                JAN_CD = "",
                SYOHIN_KIKAKU = "",
                CASE_IRISU = global.flgOff,
                //NOUHIN_KARI_TANKA = global.flgOff,
                //RETAIL_TANKA = global.flgOff,
                //HATYU_LIMIT_DAY_CNT = global.flgOff,
                START_SALE_YMD = "",
                LAST_SALE_YMD = "",
                SHUBAI = false,
                SYOHIN_KIND_L_CD = "",
                SYOHIN_KIND_M_CD = "",
                SYOHIN_KIND_S_CD = "",
                SYOHIN_KIND_CD = ""
            };

            DataRow[] row = data.AsEnumerable().Where(a => a["SYOHIN_CD"].ToString().PadLeft(8, '0') == sSyohinCD.PadLeft(8, '0')).ToArray();

            foreach (var t in row)
            {
                cls.SYOHIN_CD = t["SYOHIN_CD"].ToString();
                cls.SYOHIN_NM = t["SYOHIN_NM"].ToString();
                cls.SYOHIN_SNM = t["SYOHIN_SNM"].ToString();
                //cls.SYOHIN_KANA = syohins[i].SIRESAKI_CD;
                cls.SIRESAKI_CD = t["SIRESAKI_CD"].ToString();
                cls.SIRESAKI_NM = t["SIRESAKI_NM"].ToString();
                cls.SIRESAKI_KANA_NM = t["SIRESAKI_KANA_NM"].ToString();
                cls.JAN_CD = t["JAN_CD"].ToString();
                cls.SYOHIN_KIKAKU = t["SYOHIN_KIKAKU"].ToString();
                cls.CASE_IRISU = Utility.StrtoDouble(t["CASE_IRISU"].ToString());
                cls.START_SALE_YMD = t["START_SALE_YMD"].ToString();
                cls.LAST_SALE_YMD = t["LAST_SALE_YMD"].ToString();
                cls.SHUBAI = Convert.ToBoolean(Utility.StrtoInt(t["SHUBAI"].ToString()));
                cls.SYOHIN_KIND_L_CD = t["SYOHIN_KIND_L_CD"].ToString();
                cls.SYOHIN_KIND_M_CD = t["SYOHIN_KIND_M_CD"].ToString();
                cls.SYOHIN_KIND_S_CD = t["SYOHIN_KIND_S_CD"].ToString();
                cls.SYOHIN_KIND_CD = t["SYOHIN_KIND_CD"].ToString();
                break;
            }

            return cls;
        }


        ///-----------------------------------------------------------------------------------
        /// <summary>
        ///     得意先別画像保存フォルダパス取得 </summary>
        /// <param name="ImgPath">
        ///     画像保存先フォルダパス</param>
        /// <param name="TokuisakiCD">
        ///     得意先コード</param>
        /// <returns>
        ///    フォルダ名</returns>
        ///-----------------------------------------------------------------------------------
        public static string GetImageFilePath(string ImgPath, string TokuisakiCD)
        {
            string DirNM = string.Empty;

            // フォルダ名に得意先コードが含まれるフォルダ
            foreach (var dir in System.IO.Directory.GetDirectories(ImgPath, TokuisakiCD + "*"))
            {
                DirNM = dir;
                break;
            }

            return DirNM;
        }


        public static ClsGoods [] SetGoodsTabla(ClsOrder r, ClsTenDate [] tenDates, Table<ClsOrderHistory> tblHistories)
        {
            ClsGoods[] goods = new ClsGoods[15];
            for (int i = 0; i < global.MAX_GYO; i++)
            {
                goods[i] = new ClsGoods();
                goods[i].Suu = new string[7];
                goods[i].Target = new bool[7];

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

            // 発注対象ステータスを設定：2020/04/13
            for (int i = 0; i < goods.Length; i++)
            {
                for (int iX = 0; iX < tenDates.Length; iX++)
                {
                    // 発注対象ステータス初期値：2020/04/20
                    goods[i].Target[iX] = false;

                    // 終売取消のときネグる
                    if (goods[i].Syubai == global.SYUBAI_TORIKESHI)
                    {
                        continue;
                    }

                    // 空白の店着日はネグる
                    if (tenDates[iX].Day == string.Empty)
                    {
                        continue;
                    }

                    // 昨日以前の店着日はネグる
                    DateTime dt;
                    if (DateTime.TryParse(tenDates[iX].Year + "/" + tenDates[iX].Month + "/" + tenDates[iX].Day, out dt))
                    {
                        if (dt < DateTime.Today)
                        {
                            continue;
                        }
                    }

                    int ss = Utility.StrtoInt(goods[i].Suu[iX]);

                    // 発注あり：2020/04/13
                    if (ss > 0)
                    {
                        // 注文済み（得意先、発注日、商品コード、数量同一）商品はネグる：2020/04/13
                        string dd = tenDates[iX].Year + tenDates[iX].Month.PadLeft(2, '0') + tenDates[iX].Day.PadLeft(2, '0');
                        string syCD = goods[i].Code;
                        if (tblHistories.Any(a => a.TokuisakiCD == r.TokuisakiCode && a.SyohinCD == goods[i].Code && a.OrderDate == dd && a.Suu == ss))
                        {
                            continue;
                        }
                    }

                    // 発注対象ステータス：2020/04/20
                    goods[i].Target[iX] = true;
                }
            }

            return goods;
        }
    }
}
