using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
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

        ///-------------------------------------------------------------------
        /// <summary>
        ///     得意先情報取得 </summary>
        /// <param name="tID">
        ///     得意先番号</param>
        /// <param name="sTel">
        ///     電話番号</param>
        /// <param name="sJyu">
        ///     住所</param>
        /// <returns>
        ///     得意先名</returns>
        ///-------------------------------------------------------------------
        public static string getNouhinName(string tID, out string sTel, out string sJyu)
        {
            string val = string.Empty;
            sTel = string.Empty;
            sJyu = string.Empty;


            // 得意先CSVデータ配列読み込み
            string [] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);

            int cnt = 0;

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
                string cYuko_End_Date = t[3].Replace("\"", "");     // 有効終了日付

                int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                if (Utility.StrtoInt(cYuko_Start_Date) > toDate)
                {
                    continue;
                }

                if (toDate > Utility.StrtoInt(cYuko_End_Date))
                {
                    continue;
                }

                // 得意先コード
                string cTkCD = t[1].Replace("\"", "");             

                if (cTkCD == tID)
                {
                    string cTkNM = t[4].Replace("\"", "");      // 得意先名称
                    string cTkJyu1 = t[25].Replace("\"", "");   // 得意先住所
                    string cTkJyu2 = t[26].Replace("\"", "");   // 得意先住所
                    string cTkTel = t[27].Replace("\"", "");    // 得意先TEL

                    val = cTkNM;
                    sTel = cTkTel;
                    sJyu = cTkJyu1 + " " + cTkJyu2;

                    break;
                }
            }

            return val;
        }


        ///-------------------------------------------------------------------
        /// <summary>
        ///     得意先情報取得 </summary>
        /// <param name="tID">
        ///     得意先番号</param>
        /// <param name="sTel">
        ///     電話番号</param>
        /// <param name="sJyu">
        ///     住所</param>
        /// <returns>
        ///     得意先名</returns>
        ///-------------------------------------------------------------------
        public static ClsCsvData.ClsCsvTokuisaki GetTokuisaki(string tID)
        {
            // 返り値クラス初期化
            ClsCsvData.ClsCsvTokuisaki cls = new ClsCsvData.ClsCsvTokuisaki
            {
                TOKUISAKI_CD = "",
                YUKO_START_YMD = "",
                YUKO_END_YMD = "",
                TOKUISAKI_NM = "",
                TOKUISAKI_YUBIN_NO = "",
                TOKUISAKI_ZYUSYO1 = "",
                TOKUISAKI_ZYUSYO2 = "",
                TOKUISAKI_TEL = "",
                TOKUISAKI_FAX = "",
                DELFLG = global.FLGOFF
            };

            // 得意先CSVデータ配列読み込み
            string[] Tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);
            
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
                string cYuko_End_Date = t[3].Replace("\"", "");     // 有効終了日付

                int toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                if (Utility.StrtoInt(cYuko_Start_Date) > toDate)
                {
                    continue;
                }

                if (toDate > Utility.StrtoInt(cYuko_End_Date))
                {
                    continue;
                }

                // 得意先コード
                string cTkCD = t[1].Replace("\"", "");

                if (cTkCD == tID)
                {
                    cls.TOKUISAKI_CD = t[1].Replace("\"", "");          // 得意先コード
                    cls.YUKO_START_YMD = t[2].Replace("\"", "");        // 有効開始日付
                    cls.YUKO_END_YMD = t[3].Replace("\"", "");          // 有効終了日付
                    cls.TOKUISAKI_NM = t[4].Replace("\"", "");          // 得意先名称
                    cls.TOKUISAKI_YUBIN_NO = t[24].Replace("\"", "");   // 郵便番号
                    cls.TOKUISAKI_ZYUSYO1 = t[25].Replace("\"", "");    // 得意先住所
                    cls.TOKUISAKI_ZYUSYO2 = t[26].Replace("\"", "");    // 得意先住所
                    cls.TOKUISAKI_TEL = t[27].Replace("\"", "");        // 得意先TEL
                    cls.TOKUISAKI_FAX = t[28].Replace("\"", "");        // 得意先FAX
                    cls.DELFLG = t[119].Replace("\"", "");              // 削除フラグ
                    
                    break;
                }
            }

            return cls;
        }


        ///-------------------------------------------------------------------
        /// <summary>
        ///     商品情報取得 </summary>
        /// <param name="Sy_Array">
        ///     商品マスター配列</param>
        /// <param name="SySz_Array">
        ///     商品在庫マスター配列</param>
        /// <param name="Shiire_Array">
        ///     仕入マスター配列</param>
        /// <param name="tID">
        ///     商品コード</param>
        /// <returns>
        ///     clsCsvSyohinクラス</returns>
        ///-------------------------------------------------------------------
        public static ClsCsvData.ClsCsvSyohin GetSyohinData(string[] Sy_Array, string[] SySz_Array, string [] Shiire_Array,　string tID)
        {
            // 返り値クラス初期化
            ClsCsvData.ClsCsvSyohin cls = new ClsCsvData.ClsCsvSyohin
            {
                SYOHIN_CD = "",
                SYOHIN_NM = "",
                SYOHIN_SNM = "",
                SIRESAKI_CD = "",
                SIRESAKI_NM = "",
                JAN_CD = "",
                SYOHIN_KIKAKU = "",
                CASE_IRISU = global.flgOff,
                NOUHIN_KARI_TANKA = global.flgOff,
                RETAIL_TANKA = global.flgOff,
                HATYU_LIMIT_DAY_CNT = global.flgOff,
                START_SALE_YMD = "",
                LAST_SALE_YMD = "",
                SHUBAI = false
            };

            int toDate = 0;

            foreach (var item in Sy_Array)
            {
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

                // 該当商品か？
                if (t[1].Replace("\"", "") != tID)
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

                    // 該当商品か？
                    if (t[1].Replace("\"", "") != z[2].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 有効開始日、有効終了日を検証する
                    string cStart_Sale_YMD = z[3].Replace("\"", "");    // 商品販売開始日付
                    string cLast_Sale_YMD = t[4].Replace("\"", "");     // 商品販売終了日付（終売日）

                    toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cStart_Sale_YMD) > toDate)
                    {
                        continue;
                    }

                    if (toDate > Utility.StrtoInt(cLast_Sale_YMD))
                    {
                        continue;
                    }

                    Shubai = true;
                    break;
                }

                // 仕入先取得
                string SIRESAKI_NM = string.Empty;
                string SIRESAKI_KANA = string.Empty;

                foreach (var si in Shiire_Array)
                {
                    string[] z = si.Split(',');

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

                    if (t[13].Replace("\"", "") != z[1].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 仕入先名称取得
                    SIRESAKI_NM = z[4].Replace("\"", "");
                    SIRESAKI_KANA = z[6].Replace("\"", "");
                    break;
                }

                // 返り値
                cls.SYOHIN_CD = t[1].Replace("\"", "");
                cls.SYOHIN_NM = t[2].Replace("\"", "");
                cls.SYOHIN_SNM = t[3].Replace("\"", "");
                cls.SYOHIN_KANA = t[4].Replace("\"", "");
                cls.SIRESAKI_CD = t[13].Replace("\"", "");
                cls.SIRESAKI_NM = SIRESAKI_NM;
                cls.SIRESAKI_KANA_NM = SIRESAKI_KANA;
                cls.JAN_CD = t[16].Replace("\"", "");
                cls.SYOHIN_KIKAKU = t[19].Replace("\"", "");
                cls.CASE_IRISU = StrtoDouble(t[24].Replace("\"", ""));
                cls.NOUHIN_KARI_TANKA = StrtoDouble(t[31].Replace("\"", ""));

                // 小売り単価：新単価適用日で判断
                if (toDate < Utility.StrtoInt(t[34].Replace("\"", "")))
                {
                    cls.RETAIL_TANKA = StrtoDouble(t[32].Replace("\"", ""));
                }
                else
                {
                    cls.RETAIL_TANKA = StrtoDouble(t[35].Replace("\"", ""));
                }

                cls.HATYU_LIMIT_DAY_CNT = StrtoDouble(t[39].Replace("\"", ""));

                break;
            }

            return cls;
        }
        
        ///-------------------------------------------------------------------
        /// <summary>
        ///     商品情報取得 </summary>
        /// <param name="SyPath">
        ///     商品マスターCSVファイル</param>
        /// <param name="SySzPath">
        ///     商品在庫マスターCSVファイル</param>
        /// <param name="ShiirePath">
        ///     仕入マスターCSVファイル</param>
        /// <param name="tID">
        ///     商品コード</param>
        /// <returns>
        ///     clsCsvSyohinクラス</returns>
        ///-------------------------------------------------------------------
        public static ClsCsvData.ClsCsvSyohin GetSyohinData(string tID)
        {
            // 商品CSVデータ配列読み込み
            string[] Sy_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.商品マスター, Encoding.Default);

            // 商品在庫CSVデータ配列読み込み
            string[] SySz_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.商品在庫マスター, Encoding.Default);

            // 仕入先CSVデータ配列読み込み
            string[] Shiire_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.仕入先マスター, Encoding.Default);

            // 返り値クラス初期化
            ClsCsvData.ClsCsvSyohin cls = new ClsCsvData.ClsCsvSyohin
            {
                SYOHIN_CD = "",
                SYOHIN_NM = "",
                SYOHIN_SNM = "",
                SIRESAKI_CD = "",
                SIRESAKI_NM = "",
                JAN_CD = "",
                SYOHIN_KIKAKU = "",
                CASE_IRISU = global.flgOff,
                NOUHIN_KARI_TANKA = global.flgOff,
                RETAIL_TANKA = global.flgOff,
                START_SALE_YMD = "",
                LAST_SALE_YMD = "",
                SHUBAI = false              
            };

            int toDate = 0;

            foreach (var item in Sy_Array)
            {
                string[] t = item.Split(',');
                string cStart_Sale_YMD = "";    // 商品販売開始日付
                string cLast_Sale_YMD = "";     // 商品販売終了日付

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

                // 該当商品か？
                if (t[1].Replace("\"", "") != tID)
                {
                    continue;
                }

                // 商品在庫マスターで終売を調べる
                bool Syubai = false;
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

                    // 該当商品か？
                    if (t[1].Replace("\"", "") != z[2].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 有効開始日、有効終了日を検証する
                    cStart_Sale_YMD = z[3].Replace("\"", "");    // 商品販売開始日付
                    cLast_Sale_YMD = z[4].Replace("\"", "");     // 商品販売終了日付（終売日）

                    toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cStart_Sale_YMD) > toDate)
                    {
                        continue;
                    }

                    if (toDate > Utility.StrtoInt(cLast_Sale_YMD))
                    {
                        // 終売商品
                        Syubai = true;
                    }
                    else
                    {
                        // 終売ではない
                        Syubai = false;
                    }

                    break;
                }

                // 仕入先取得
                string SIRESAKI_NM = string.Empty;
                string SIRESAKI_KANA = string.Empty;

                foreach (var si in Shiire_Array)
                {
                    string[] z = si.Split(',');

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

                    if (t[13].Replace("\"", "") != z[1].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 仕入先名称取得
                    SIRESAKI_NM = z[4].Replace("\"", "");
                    SIRESAKI_KANA = z[6].Replace("\"", "");
                    break;
                }

                // 返り値
                cls.SYOHIN_CD = t[1].Replace("\"", "");
                cls.SYOHIN_NM = t[2].Replace("\"", "");
                cls.SYOHIN_SNM = t[3].Replace("\"", "");
                cls.SYOHIN_KANA = t[4].Replace("\"", "");
                cls.SIRESAKI_CD = t[13].Replace("\"", "");
                cls.SIRESAKI_NM = SIRESAKI_NM;
                cls.SIRESAKI_KANA_NM = SIRESAKI_KANA;
                cls.JAN_CD = t[16].Replace("\"", "");
                cls.SYOHIN_KIKAKU = t[19].Replace("\"", "");
                cls.CASE_IRISU = StrtoDouble(t[24].Replace("\"", ""));
                cls.NOUHIN_KARI_TANKA = StrtoDouble(t[31].Replace("\"", ""));

                // 小売り単価：新単価適用日で判断
                if (toDate < Utility.StrtoInt(t[34].Replace("\"", "")))
                {
                    cls.RETAIL_TANKA = StrtoDouble(t[32].Replace("\"", ""));
                }
                else
                {
                    cls.RETAIL_TANKA = StrtoDouble(t[35].Replace("\"", ""));
                }

                cls.HATYU_LIMIT_DAY_CNT = StrtoDouble(t[39].Replace("\"", ""));
                cls.START_SALE_YMD = cStart_Sale_YMD;
                cls.LAST_SALE_YMD = cLast_Sale_YMD;
                cls.SHUBAI = Syubai;

                break;
            }

            return cls;
        }


        ///-------------------------------------------------------------------
        /// <summary>
        ///     商品情報取得 </summary>
        /// <param name="SyPath">
        ///     商品マスターCSVファイル</param>
        /// <param name="SySzPath">
        ///     商品在庫マスターCSVファイル</param>
        /// <param name="ShiirePath">
        ///     仕入マスターCSVファイル</param>
        /// <param name="tID">
        ///     商品コード</param>
        /// <returns>
        ///     clsCsvSyohinクラス配列</returns>
        ///-------------------------------------------------------------------
        public static ClsCsvData.ClsCsvSyohin [] GetSyohinData(string SyPath, string SySzPath, string ShiirePath)
        {
            // 商品CSVデータ配列読み込み
            string[] Sy_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.商品マスター, Encoding.Default);

            // 商品在庫CSVデータ配列読み込み
            string[] SySz_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.商品在庫マスター, Encoding.Default);

            // 仕入先CSVデータ配列読み込み
            string[] Shiire_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.仕入先マスター, Encoding.Default);
            
            int toDate = 0;
            int x = 0;

            ClsCsvData.ClsCsvSyohin[] syohins = null;

            foreach (var item in Sy_Array)
            {
                string[] t = item.Split(',');
                string cStart_Sale_YMD = "";    // 商品販売開始日付
                string cLast_Sale_YMD = "";     // 商品販売終了日付

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
                bool Syubai = false;
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

                    // 該当商品か？
                    if (t[1].Replace("\"", "") != z[2].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 有効開始日、有効終了日を検証する
                    cStart_Sale_YMD = z[3].Replace("\"", "");    // 商品販売開始日付
                    cLast_Sale_YMD = z[4].Replace("\"", "");     // 商品販売終了日付（終売日）

                    toDate = Utility.StrtoInt(DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2"));

                    if (Utility.StrtoInt(cStart_Sale_YMD) > toDate)
                    {
                        continue;
                    }

                    if (toDate > Utility.StrtoInt(cLast_Sale_YMD))
                    {
                        // 終売商品
                        Syubai = true;
                    }
                    else
                    {
                        // 終売ではない
                        Syubai = false;
                    }

                    break;
                }

                // 仕入先取得
                string SIRESAKI_NM = string.Empty;
                string SIRESAKI_KANA = string.Empty;

                foreach (var si in Shiire_Array)
                {
                    string[] z = si.Split(',');

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

                    if (t[13].Replace("\"", "") != z[1].Replace("\"", ""))
                    {
                        continue;
                    }

                    // 仕入先名称取得
                    SIRESAKI_NM = z[4].Replace("\"", "");
                    SIRESAKI_KANA = z[6].Replace("\"", "");
                    break;
                }

                // 小売り単価：新単価適用日で判断
                double _RETAIL_TANKA = 0;
                if (toDate < Utility.StrtoInt(t[34].Replace("\"", "")))
                {
                    _RETAIL_TANKA = StrtoDouble(t[32].Replace("\"", ""));
                }
                else
                {
                    _RETAIL_TANKA = StrtoDouble(t[35].Replace("\"", ""));
                }

                Array.Resize(ref syohins, x + 1);

                // 返り値
                syohins[x] = new ClsCsvData.ClsCsvSyohin
                {
                    SYOHIN_CD = t[1].Replace("\"", ""),
                    SYOHIN_NM = t[2].Replace("\"", ""),
                    SYOHIN_SNM = t[3].Replace("\"", ""),
                    SYOHIN_KANA = t[4].Replace("\"", ""),
                    SIRESAKI_CD = t[13].Replace("\"", ""),
                    SIRESAKI_NM = SIRESAKI_NM,
                    SIRESAKI_KANA_NM = SIRESAKI_KANA,
                    JAN_CD = t[16].Replace("\"", ""),
                    SYOHIN_KIKAKU = t[19].Replace("\"", ""),
                    CASE_IRISU = StrtoDouble(t[24].Replace("\"", "")),
                    NOUHIN_KARI_TANKA = StrtoDouble(t[31].Replace("\"", "")),
                    RETAIL_TANKA = _RETAIL_TANKA,
                    HATYU_LIMIT_DAY_CNT = StrtoDouble(t[39].Replace("\"", "")),
                    START_SALE_YMD = cStart_Sale_YMD,
                    LAST_SALE_YMD = cLast_Sale_YMD,
                    SHUBAI = Syubai
                };

                x++;
            }

            return syohins;
        }

        public static void SetTenDate(ClsTenDate [] tenDates, ClsOrder r)
        {
            // 店着日付クラス
            //tenDates = new ClsTenDate[7];

            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Year = r.Year;
            tenDates[0].Month = r.Month;
            tenDates[0].Day = Utility.StrtoInt(r.Day1.Trim());

            tenDates[1].Day = Utility.StrtoInt(r.Day2.Trim());
            tenDates[2].Day = Utility.StrtoInt(r.Day3.Trim());
            tenDates[3].Day = Utility.StrtoInt(r.Day4.Trim());
            tenDates[4].Day = Utility.StrtoInt(r.Day5.Trim());
            tenDates[5].Day = Utility.StrtoInt(r.Day6.Trim());
            tenDates[6].Day = Utility.StrtoInt(r.Day7.Trim());

            int sYear = r.Year;
            int sMonth = r.Month;

            for (int i = 1; i < tenDates.Length; i++)
            {
                if (tenDates[i].Day == 1)
                {
                    // ここから翌月
                    sMonth++;

                    if (sMonth > 12)
                    {
                        // 翌年
                        sMonth -= 12;
                        sYear++;
                    }
                }

                tenDates[i].Year = sYear;
                tenDates[i].Month = sMonth;
            }
        }

        public static void SetTenDate(ClsTenDate[] tenDates, ClsFaxOrder r)
        {
            // 店着日付クラス
            //tenDates = new ClsTenDate[7];

            // 初期化
            for (int i = 0; i < 7; i++)
            {
                tenDates[i] = new ClsTenDate();
            }

            tenDates[0].Year = r.Year;
            tenDates[0].Month = r.Month;
            tenDates[0].Day = Utility.StrtoInt(r.Day1.Trim());

            tenDates[1].Day = Utility.StrtoInt(r.Day2.Trim());
            tenDates[2].Day = Utility.StrtoInt(r.Day3.Trim());
            tenDates[3].Day = Utility.StrtoInt(r.Day4.Trim());
            tenDates[4].Day = Utility.StrtoInt(r.Day5.Trim());
            tenDates[5].Day = Utility.StrtoInt(r.Day6.Trim());
            tenDates[6].Day = Utility.StrtoInt(r.Day7.Trim());

            int sYear = r.Year;
            int sMonth = r.Month;

            for (int i = 1; i < tenDates.Length; i++)
            {
                if (tenDates[i].Day == 1)
                {
                    // ここから翌月
                    sMonth++;

                    if (sMonth > 12)
                    {
                        // 翌年
                        sMonth -= 12;
                        sYear++;
                    }
                }

                tenDates[i].Year = sYear;
                tenDates[i].Month = sMonth;
            }
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     商品マスター配列から情報を取得する </summary>
        /// <param name="sSyohinCD">
        ///     商品コード  </param>
        /// <returns>
        ///     商品マスタークラス</returns>
        ///------------------------------------------------------------------------
        public static ClsCsvData.ClsCsvSyohin GetSyohins(ClsCsvData.ClsCsvSyohin [] syohins, string sSyohinCD)
        {
            ClsCsvData.ClsCsvSyohin cls = new ClsCsvData.ClsCsvSyohin
            {
                SYOHIN_CD = "",
                SYOHIN_NM = "",
                SYOHIN_SNM = "",
                SYOHIN_KANA = "",
                SIRESAKI_CD = "",
                SIRESAKI_NM = "",
                SIRESAKI_KANA_NM = "",
                JAN_CD = "",
                SYOHIN_KIKAKU = "",
                CASE_IRISU = global.flgOff,
                NOUHIN_KARI_TANKA = global.flgOff,
                RETAIL_TANKA = global.flgOff,
                HATYU_LIMIT_DAY_CNT = global.flgOff,
                START_SALE_YMD = "",
                LAST_SALE_YMD = "",
                SHUBAI = false
            };

            for (int i = 0; i < syohins.Length; i++)
            {
                if (syohins[i].SYOHIN_CD == sSyohinCD.PadLeft(8, '0'))
                {
                    cls.SYOHIN_CD = syohins[i].SYOHIN_CD;
                    cls.SYOHIN_NM = syohins[i].SYOHIN_NM;
                    cls.SYOHIN_SNM = syohins[i].SYOHIN_SNM;
                    cls.SYOHIN_KANA = syohins[i].SIRESAKI_CD;
                    cls.SIRESAKI_CD = syohins[i].SIRESAKI_CD;
                    cls.SIRESAKI_NM = syohins[i].SIRESAKI_NM;
                    cls.SIRESAKI_KANA_NM = syohins[i].SIRESAKI_KANA_NM;
                    cls.JAN_CD = syohins[i].JAN_CD;
                    cls.SYOHIN_KIKAKU = syohins[i].SYOHIN_KIKAKU;
                    cls.CASE_IRISU = syohins[i].CASE_IRISU;
                    cls.NOUHIN_KARI_TANKA = syohins[i].NOUHIN_KARI_TANKA;
                    cls.RETAIL_TANKA = syohins[i].RETAIL_TANKA;
                    cls.HATYU_LIMIT_DAY_CNT = syohins[i].HATYU_LIMIT_DAY_CNT;
                    cls.START_SALE_YMD = syohins[i].START_SALE_YMD;
                    cls.LAST_SALE_YMD = syohins[i].LAST_SALE_YMD;
                    cls.SHUBAI = syohins[i].SHUBAI;
                    break;
                }
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
    }
}
