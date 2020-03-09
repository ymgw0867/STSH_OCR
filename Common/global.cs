using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STSH_OCR.Common
{
    class global
    {
        //public static string pblImagePath = "";

        #region 画像表示倍率（%）・座標
        public float miMdlZoomRate = 0f;     // 現在の表示倍率
        public float ZOOM_RATE = 0.36f;      // 標準倍率
        public float ZOOM_MAX = 2.00f;       // 最大倍率
        public float ZOOM_MIN = 0.05f;       // 最小倍率
        public float ZOOM_STEP = 0.05f;      // ステップ倍率
        public float ZOOM_NOW = 0.0f;        // 現在の倍率

        public int RECTD_NOW = 0;            // 現在の座標
        public int RECTS_NOW = 0;            // 現在の座標
        public int RECT_STEP = 20;           // ステップ座標
        #endregion

        //和暦西暦変換
        public const int rekiCnv = 1988;    //西暦、和暦変換

        // 表示色
        public static System.Drawing.Color defaultColor = System.Drawing.Color.DarkBlue;

        // ChangeValueStatus
        public static bool ChangeValueStatus = true;

        #region ローカルMDB関連定数
        public const string MDBFILE = "NHBR_CLI.mdb";           // MDBファイル名
        public const string MDBTEMP = "NHBR_CLI_Temp.mdb";      // 最適化一時ファイル名
        public const string MDBBACK = "Backmdb.mdb";        // 最適化後バックアップファイル名
        #endregion

        #region フラグオン・オフ定数
        public const int flgOn = 1;            //フラグ有り(1)
        public const int flgOff = 0;           //フラグなし(0)
        public const string FLGON = "1";
        public const string FLGOFF = "0";
        #endregion

        public static int pblDenNum = 0;            // データ数

        public const int configKEY = 1;         // 環境設定データキー
        public const int mailKey = 1;           // メール設定データキー

        //ＯＣＲ処理ＣＳＶデータの検証要素
        public const int CSVLENGTH = 197;          //データフィールド数 2011/06/11
        public const int CSVFILENAMELENGTH = 21;   //ファイル名の文字数 2011/06/11  
 
        // 勤務記録表
        public const int STARTTIME = 8;            // 単位記入開始時間帯
        public const int ENDTIME = 22;             // 単位記入終了時間帯
        public const int TANNIMAX = 4;             // 単位最大値
        public const int WEEKLIMIT = 160;          // 週労働時間基準単位：40時間
        public const int DAYLIMIT = 32;            // 一日あたり労働時間基準単位：8時間

        #region 環境設定項目
        public int cnfYear = 0;                  // 対象年
        public int cnfMonth = 0;                 // 対象月
        public string cnfPath = string.Empty;    // 受け渡しデータ作成パス
        public string cnfImgPath = string.Empty; // 画像保存先パス
        public string cnfLogPath = string.Empty; // ログデータ作成パス
        public int cnfArchived = 0;              // データ保管期間（月数）
        #endregion

        public const int MAX_GYO = 15;
        public const int MAX_MIN = 1;
        
        // ＯＣＲモード
        public static string OCR_SCAN = "1";
        public static string OCR_IMAGE = "2";

        public string[] arrayChohyoID = { "社員", "パート", "出向社員" };

        // データ作成画面datagridview表示行数
        public const int _MULTIGYO = 31;

        // フォーム登録モード
        public const int FORM_ADDMODE = 0;
        public const int FORM_EDITMODE = 1;

        // 年月日未設定値
        public static DateTime NODATE = DateTime.Parse("1900/01/01");
        
        // ＣＳＶファイル名
        public static string CSV_LOG = "logdata";   // ログデータ

        public static string LOCK_FILEHEAD = "LOCK-";    //LOCKFILENAMEの前に付加する文字列

        // 出荷基準注文本数単位
        public static string SH_GUUSU = "G";
        public static string SH_KISU = "K";
        public static string SH_EMPTY = "";

        // 出荷基準：グループ
        public static string SH_GROUP_A = "A";
        public static string SH_GROUP_B = "B";
        public static string SH_GROUP_C = "C";
        public static string SH_GROUP_D = "D";
        public static string SH_GROUP_E = "E";
        public static string SH_GROUP_F = "F";
        public static string SH_GROUP_G = "G";

        // 出荷基準：缶、瓶、樽
        public static string SH_PACK_EMPTY = "0";
        public static string SH_PACK_KAN = "1";
        public static string SH_PACK_BIN = "2";
        public static string SH_PACK_TARU = "3";

        // 再FAX文字列 2018/08/02
        public static string REFAX = "REFAX";


        // グリッドビューで次の行の移動先カラム名
        public static string NEXT_COLUMN = string.Empty;

        // 終売処理
        public static string[] SyubaiArray = { "", "取消", "有効" };
        public static int SYUBAI_TORIKESHI = 1;
        public static int SYUBAI_YUKO = 2;
        public static int SYUBAI_NON = 0;

        // CSVデータ
        public static string DTKBN = "1";
    }
}
