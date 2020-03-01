using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace STSH_OCR.Common
{
    public class ClsSqliteMap
    {

    }

    // 環境設定
    [Table(Name = "system_Config")]
    public class ClsSystemConfig
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "CSVデータ作成先パス")]
        public string DataPath { get; set; }

        [Column(Name = "画像保存先パス")]
        public string ImgPath { get; set; }

        [Column(Name = "データ保存月数")]
        public int DataSpan { get; set; }

        [Column(Name = "ログ保存月数")]
        public int LogSpan { get; set; }

        [Column(Name = "同名ファイル書き込み処理")]
        public int FileWriteStatus { get; set; }

        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }
    }

    // 発注書編集ログ
    [Table(Name = "DataEditLog")]
    public class ClsDataEditLog
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "年月日時刻")]
        public string Date_Time { get; set; }

        [Column(Name = "得意先コード")]
        public int TokuisakiCode { get; set; }

        [Column(Name = "年")]
        public int Year { get; set; }

        [Column(Name = "月")]
        public int Month { get; set; }

        [Column(Name = "発注書番号")]
        public int FaxOrderNum { get; set; }

        [Column(Name = "商品コード")]
        public string ShohinCode { get; set; }

        [Column(Name = "店着日付")]
        public string TenchakuDate { get; set; }

        [Column(Name = "行番号")]
        public int RowNumber { get; set; }

        [Column(Name = "列番号")]
        public int ColNumber { get; set; }

        [Column(Name = "項目名")]
        public string FieldName { get; set; }

        [Column(Name = "変更前値")]
        public string BeforeValue { get; set; }

        [Column(Name = "変更後値")]
        public string AfterValue { get; set; }

        [Column(Name = "画像名")]
        public string ImageFileName { get; set; }

        [Column(Name = "編集アカウントID")]
        public string Edit_AccountID { get; set; }

        [Column(Name = "コンピュータ名")]
        public string ComputerName { get; set; }

        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }
    }

    // ＦＡＸ発注書データ
    [Table(Name = "FAX_Order")]
    public class ClsFaxOrder
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public string ID { get; set; }

        [Column(Name = "画像名")]
        public string ImageFileName { get; set; }

        [Column(Name = "得意先コード")]
        public int TokuisakiCode { get; set; }

        [Column(Name = "patternID")]
        public int patternID { get; set; }

        [Column(Name = "SeqNumber")]
        public int SeqNumber { get; set; }

        [Column(Name = "年")]
        public int Year { get; set; }

        [Column(Name = "月")]
        public int Month { get; set; }

        // 店着日
        [Column(Name = "Day1")]
        public string Day1 { get; set; }

        [Column(Name = "Day2")]
        public string Day2 { get; set; }

        [Column(Name = "Day3")]
        public string Day3 { get; set; }

        [Column(Name = "Day4")]
        public string Day4 { get; set; }

        [Column(Name = "Day5")]
        public string Day5 { get; set; }

        [Column(Name = "Day6")]
        public string Day6 { get; set; }

        [Column(Name = "Day7")]
        public string Day7 { get; set; }

        // 商品１
        [Column(Name = "Goods1_1")]
        public string Goods1_1 { get; set; }

        [Column(Name = "Goods1_2")]
        public string Goods1_2 { get; set; }

        [Column(Name = "Goods1_3")]
        public string Goods1_3 { get; set; }

        [Column(Name = "Goods1_4")]
        public string Goods1_4 { get; set; }

        [Column(Name = "Goods1_5")]
        public string Goods1_5 { get; set; }

        [Column(Name = "Goods1_6")]
        public string Goods1_6 { get; set; }

        [Column(Name = "Goods1_7")]
        public string Goods1_7 { get; set; }

        // 商品２
        [Column(Name = "Goods2_1")]
        public string Goods2_1 { get; set; }

        [Column(Name = "Goods2_2")]
        public string Goods2_2 { get; set; }

        [Column(Name = "Goods2_3")]
        public string Goods2_3 { get; set; }

        [Column(Name = "Goods2_4")]
        public string Goods2_4 { get; set; }

        [Column(Name = "Goods2_5")]
        public string Goods2_5 { get; set; }

        [Column(Name = "Goods2_6")]
        public string Goods2_6 { get; set; }

        [Column(Name = "Goods2_7")]
        public string Goods2_7 { get; set; }

        // 商品３
        [Column(Name = "Goods3_1")]
        public string Goods3_1 { get; set; }

        [Column(Name = "Goods3_2")]
        public string Goods3_2 { get; set; }

        [Column(Name = "Goods3_3")]
        public string Goods3_3 { get; set; }

        [Column(Name = "Goods3_4")]
        public string Goods3_4 { get; set; }

        [Column(Name = "Goods3_5")]
        public string Goods3_5 { get; set; }

        [Column(Name = "Goods3_6")]
        public string Goods3_6 { get; set; }

        [Column(Name = "Goods3_7")]
        public string Goods3_7 { get; set; }

        // 商品４
        [Column(Name = "Goods4_1")]
        public string Goods4_1 { get; set; }

        [Column(Name = "Goods4_2")]
        public string Goods4_2 { get; set; }

        [Column(Name = "Goods4_3")]
        public string Goods4_3 { get; set; }

        [Column(Name = "Goods4_4")]
        public string Goods4_4 { get; set; }

        [Column(Name = "Goods4_5")]
        public string Goods4_5 { get; set; }

        [Column(Name = "Goods4_6")]
        public string Goods4_6 { get; set; }

        [Column(Name = "Goods4_7")]
        public string Goods4_7 { get; set; }

        // 商品５
        [Column(Name = "Goods5_1")]
        public string Goods5_1 { get; set; }

        [Column(Name = "Goods5_2")]
        public string Goods5_2 { get; set; }

        [Column(Name = "Goods5_3")]
        public string Goods5_3 { get; set; }

        [Column(Name = "Goods5_4")]
        public string Goods5_4 { get; set; }

        [Column(Name = "Goods5_5")]
        public string Goods5_5 { get; set; }

        [Column(Name = "Goods5_6")]
        public string Goods5_6 { get; set; }

        [Column(Name = "Goods5_7")]
        public string Goods5_7 { get; set; }

        // 商品６
        [Column(Name = "Goods6_1")]
        public string Goods6_1 { get; set; }

        [Column(Name = "Goods6_2")]
        public string Goods6_2 { get; set; }

        [Column(Name = "Goods6_3")]
        public string Goods6_3 { get; set; }

        [Column(Name = "Goods6_4")]
        public string Goods6_4 { get; set; }

        [Column(Name = "Goods6_5")]
        public string Goods6_5 { get; set; }

        [Column(Name = "Goods6_6")]
        public string Goods6_6 { get; set; }

        [Column(Name = "Goods6_7")]
        public string Goods6_7 { get; set; }

        // 商品７
        [Column(Name = "Goods7_1")]
        public string Goods7_1 { get; set; }

        [Column(Name = "Goods7_2")]
        public string Goods7_2 { get; set; }

        [Column(Name = "Goods7_3")]
        public string Goods7_3 { get; set; }

        [Column(Name = "Goods7_4")]
        public string Goods7_4 { get; set; }

        [Column(Name = "Goods7_5")]
        public string Goods7_5 { get; set; }

        [Column(Name = "Goods7_6")]
        public string Goods7_6 { get; set; }

        [Column(Name = "Goods7_7")]
        public string Goods7_7 { get; set; }

        // 商品８
        [Column(Name = "Goods8_1")]
        public string Goods8_1 { get; set; }

        [Column(Name = "Goods8_2")]
        public string Goods8_2 { get; set; }

        [Column(Name = "Goods8_3")]
        public string Goods8_3 { get; set; }

        [Column(Name = "Goods8_4")]
        public string Goods8_4 { get; set; }

        [Column(Name = "Goods8_5")]
        public string Goods8_5 { get; set; }

        [Column(Name = "Goods8_6")]
        public string Goods8_6 { get; set; }

        [Column(Name = "Goods8_7")]
        public string Goods8_7 { get; set; }

        // 商品９
        [Column(Name = "Goods9_1")]
        public string Goods9_1 { get; set; }

        [Column(Name = "Goods9_2")]
        public string Goods9_2 { get; set; }

        [Column(Name = "Goods9_3")]
        public string Goods9_3 { get; set; }

        [Column(Name = "Goods9_4")]
        public string Goods9_4 { get; set; }

        [Column(Name = "Goods9_5")]
        public string Goods9_5 { get; set; }

        [Column(Name = "Goods9_6")]
        public string Goods9_6 { get; set; }

        [Column(Name = "Goods9_7")]
        public string Goods9_7 { get; set; }

        // 商品10
        [Column(Name = "Goods10_1")]
        public string Goods10_1 { get; set; }

        [Column(Name = "Goods10_2")]
        public string Goods10_2 { get; set; }

        [Column(Name = "Goods10_3")]
        public string Goods10_3 { get; set; }

        [Column(Name = "Goods10_4")]
        public string Goods10_4 { get; set; }

        [Column(Name = "Goods10_5")]
        public string Goods10_5 { get; set; }

        [Column(Name = "Goods10_6")]
        public string Goods10_6 { get; set; }

        [Column(Name = "Goods10_7")]
        public string Goods10_7 { get; set; }

        // 商品11
        [Column(Name = "Goods11_1")]
        public string Goods11_1 { get; set; }

        [Column(Name = "Goods11_2")]
        public string Goods11_2 { get; set; }

        [Column(Name = "Goods11_3")]
        public string Goods11_3 { get; set; }

        [Column(Name = "Goods11_4")]
        public string Goods11_4 { get; set; }

        [Column(Name = "Goods11_5")]
        public string Goods11_5 { get; set; }

        [Column(Name = "Goods11_6")]
        public string Goods11_6 { get; set; }

        [Column(Name = "Goods11_7")]
        public string Goods11_7 { get; set; }

        // 商品12
        [Column(Name = "Goods12_1")]
        public string Goods12_1 { get; set; }

        [Column(Name = "Goods12_2")]
        public string Goods12_2 { get; set; }

        [Column(Name = "Goods12_3")]
        public string Goods12_3 { get; set; }

        [Column(Name = "Goods12_4")]
        public string Goods12_4 { get; set; }

        [Column(Name = "Goods12_5")]
        public string Goods12_5 { get; set; }

        [Column(Name = "Goods12_6")]
        public string Goods12_6 { get; set; }

        [Column(Name = "Goods12_7")]
        public string Goods12_7 { get; set; }

        // 商品13
        [Column(Name = "Goods13_1")]
        public string Goods13_1 { get; set; }

        [Column(Name = "Goods13_2")]
        public string Goods13_2 { get; set; }

        [Column(Name = "Goods13_3")]
        public string Goods13_3 { get; set; }

        [Column(Name = "Goods13_4")]
        public string Goods13_4 { get; set; }

        [Column(Name = "Goods13_5")]
        public string Goods13_5 { get; set; }

        [Column(Name = "Goods13_6")]
        public string Goods13_6 { get; set; }

        [Column(Name = "Goods13_7")]
        public string Goods13_7 { get; set; }

        // 商品14
        [Column(Name = "Goods14_1")]
        public string Goods14_1 { get; set; }

        [Column(Name = "Goods14_2")]
        public string Goods14_2 { get; set; }

        [Column(Name = "Goods14_3")]
        public string Goods14_3 { get; set; }

        [Column(Name = "Goods14_4")]
        public string Goods14_4 { get; set; }

        [Column(Name = "Goods14_5")]
        public string Goods14_5 { get; set; }

        [Column(Name = "Goods14_6")]
        public string Goods14_6 { get; set; }

        [Column(Name = "Goods14_7")]
        public string Goods14_7 { get; set; }

        // 商品15
        [Column(Name = "Goods15_1")]
        public string Goods15_1 { get; set; }

        [Column(Name = "Goods15_2")]
        public string Goods15_2 { get; set; }

        [Column(Name = "Goods15_3")]
        public string Goods15_3 { get; set; }

        [Column(Name = "Goods15_4")]
        public string Goods15_4 { get; set; }

        [Column(Name = "Goods15_5")]
        public string Goods15_5 { get; set; }

        [Column(Name = "Goods15_6")]
        public string Goods15_6 { get; set; }

        [Column(Name = "Goods15_7")]
        public string Goods15_7 { get; set; }

        // 商品16
        [Column(Name = "Goods16_1")]
        public string Goods16_1 { get; set; }

        [Column(Name = "Goods16_2")]
        public string Goods16_2 { get; set; }

        [Column(Name = "Goods16_3")]
        public string Goods16_3 { get; set; }

        [Column(Name = "Goods16_4")]
        public string Goods16_4 { get; set; }

        [Column(Name = "Goods16_5")]
        public string Goods16_5 { get; set; }

        [Column(Name = "Goods16_6")]
        public string Goods16_6 { get; set; }

        [Column(Name = "Goods16_7")]
        public string Goods16_7 { get; set; }

        // 商品17
        [Column(Name = "Goods17_1")]
        public string Goods17_1 { get; set; }

        [Column(Name = "Goods17_2")]
        public string Goods17_2 { get; set; }

        [Column(Name = "Goods17_3")]
        public string Goods17_3 { get; set; }

        [Column(Name = "Goods17_4")]
        public string Goods17_4 { get; set; }

        [Column(Name = "Goods17_5")]
        public string Goods17_5 { get; set; }

        [Column(Name = "Goods17_6")]
        public string Goods17_6 { get; set; }

        [Column(Name = "Goods17_7")]
        public string Goods17_7 { get; set; }

        // 商品18
        [Column(Name = "Goods18_1")]
        public string Goods18_1 { get; set; }

        [Column(Name = "Goods18_2")]
        public string Goods18_2 { get; set; }

        [Column(Name = "Goods18_3")]
        public string Goods18_3 { get; set; }

        [Column(Name = "Goods18_4")]
        public string Goods18_4 { get; set; }

        [Column(Name = "Goods18_5")]
        public string Goods18_5 { get; set; }

        [Column(Name = "Goods18_6")]
        public string Goods18_6 { get; set; }

        [Column(Name = "Goods18_7")]
        public string Goods18_7 { get; set; }

        // 商品19
        [Column(Name = "Goods19_1")]
        public string Goods19_1 { get; set; }

        [Column(Name = "Goods19_2")]
        public string Goods19_2 { get; set; }

        [Column(Name = "Goods19_3")]
        public string Goods19_3 { get; set; }

        [Column(Name = "Goods19_4")]
        public string Goods19_4 { get; set; }

        [Column(Name = "Goods19_5")]
        public string Goods19_5 { get; set; }

        [Column(Name = "Goods19_6")]
        public string Goods19_6 { get; set; }

        [Column(Name = "Goods19_7")]
        public string Goods19_7 { get; set; }

        // 商品20
        [Column(Name = "Goods20_1")]
        public string Goods20_1 { get; set; }

        [Column(Name = "Goods20_2")]
        public string Goods20_2 { get; set; }

        [Column(Name = "Goods20_3")]
        public string Goods20_3 { get; set; }

        [Column(Name = "Goods20_4")]
        public string Goods20_4 { get; set; }

        [Column(Name = "Goods20_5")]
        public string Goods20_5 { get; set; }

        [Column(Name = "Goods10_6")]
        public string Goods20_6 { get; set; }

        [Column(Name = "Goods10_7")]
        public string Goods20_7 { get; set; }

        // 商品コード１
        [Column(Name = "G_Code1")]
        public string G_Code1 { get; set; }

        // 商品コード２
        [Column(Name = "G_Code2")]
        public string G_Code2 { get; set; }

        // 商品コード３
        [Column(Name = "G_Code3")]
        public string G_Code3 { get; set; }

        // 商品コード４
        [Column(Name = "G_Code4")]
        public string G_Code4 { get; set; }

        // 商品コード５
        [Column(Name = "G_Code5")]
        public string G_Code5 { get; set; }

        // 商品コード６
        [Column(Name = "G_Code6")]
        public string G_Code6 { get; set; }

        // 商品コード７
        [Column(Name = "G_Code7")]
        public string G_Code7 { get; set; }

        // 商品コード８
        [Column(Name = "G_Code8")]
        public string G_Code8 { get; set; }

        // 商品コード９
        [Column(Name = "G_Code9")]
        public string G_Code9 { get; set; }

        // 商品コード10
        [Column(Name = "G_Code10")]
        public string G_Code10 { get; set; }

        // 商品コード11
        [Column(Name = "G_Code11")]
        public string G_Code11 { get; set; }

        // 商品コード12
        [Column(Name = "G_Code12")]
        public string G_Code12 { get; set; }

        // 商品コード13
        [Column(Name = "G_Code13")]
        public string G_Code13 { get; set; }

        // 商品コード14
        [Column(Name = "G_Code14")]
        public string G_Code14 { get; set; }

        // 商品コード15
        [Column(Name = "G_Code15")]
        public string G_Code15 { get; set; }

        // 商品コード16
        [Column(Name = "G_Code16")]
        public string G_Code16 { get; set; }

        // 商品コード17
        [Column(Name = "G_Code17")]
        public string G_Code17 { get; set; }

        // 商品コード18
        [Column(Name = "G_Code18")]
        public string G_Code18 { get; set; }

        // 商品コード19
        [Column(Name = "G_Code19")]
        public string G_Code19 { get; set; }

        // 商品コード20
        [Column(Name = "G_Code20")]
        public string G_Code20 { get; set; }

        // 納価１
        [Column(Name = "G_Nouka1")]
        public int G_Nouka1 { get; set; }

        // 納価２
        [Column(Name = "G_Nouka2")]
        public int G_Nouka2 { get; set; }

        // 納価３
        [Column(Name = "G_Nouka3")]
        public int G_Nouka3 { get; set; }

        // 納価４
        [Column(Name = "G_Nouka4")]
        public int G_Nouka4 { get; set; }

        // 納価５
        [Column(Name = "G_Nouka5")]
        public int G_Nouka5 { get; set; }

        // 納価６
        [Column(Name = "G_Nouka6")]
        public int G_Nouka6 { get; set; }

        // 納価７
        [Column(Name = "G_Nouka7")]
        public int G_Nouka7 { get; set; }

        // 納価８
        [Column(Name = "G_Nouka8")]
        public int G_Nouka8 { get; set; }

        // 納価９
        [Column(Name = "G_Nouka9")]
        public int G_Nouka9 { get; set; }

        // 納価10
        [Column(Name = "G_Nouka10")]
        public int G_Nouka10 { get; set; }

        // 納価11
        [Column(Name = "G_Nouka11")]
        public int G_Nouka11 { get; set; }

        // 納価12
        [Column(Name = "G_Nouka12")]
        public int G_Nouka12 { get; set; }

        // 納価13
        [Column(Name = "G_Nouka13")]
        public int G_Nouka13 { get; set; }

        // 納価14
        [Column(Name = "G_Nouka14")]
        public int G_Nouka14 { get; set; }

        // 納価15
        [Column(Name = "G_Nouka15")]
        public int G_Nouka15 { get; set; }

        // 納価16
        [Column(Name = "G_Nouka16")]
        public int G_Nouka16 { get; set; }

        // 納価17
        [Column(Name = "G_Nouka17")]
        public int G_Nouka17 { get; set; }

        // 納価18
        [Column(Name = "G_Nouka18")]
        public int G_Nouka18 { get; set; }

        // 納価19
        [Column(Name = "G_Nouka19")]
        public int G_Nouka19 { get; set; }

        // 納価20
        [Column(Name = "G_Nouka20")]
        public int G_Nouka20 { get; set; }

        // 売価１
        [Column(Name = "G_Baika1")]
        public int G_Baika1 { get; set; }

        // 売価２
        [Column(Name = "G_Baika2")]
        public int G_Baika2 { get; set; }

        // 売価３
        [Column(Name = "G_Baika3")]
        public int G_Baika3 { get; set; }

        // 売価４
        [Column(Name = "G_Baika4")]
        public int G_Baika4 { get; set; }

        // 売価５
        [Column(Name = "G_Baika5")]
        public int G_Baika5 { get; set; }

        // 売価６
        [Column(Name = "G_Baika6")]
        public int G_Baika6 { get; set; }

        // 売価７
        [Column(Name = "G_Baika7")]
        public int G_Baika7 { get; set; }

        // 売価８
        [Column(Name = "G_Baika8")]
        public int G_Baika8 { get; set; }

        // 売価９
        [Column(Name = "G_Baika9")]
        public int G_Baika9 { get; set; }

        // 売価10
        [Column(Name = "G_Baika10")]
        public int G_Baika10 { get; set; }

        // 売価11
        [Column(Name = "G_Baika11")]
        public int G_Baika11 { get; set; }

        // 売価12
        [Column(Name = "G_Baika12")]
        public int G_Baika12 { get; set; }

        // 売価13
        [Column(Name = "G_Baika13")]
        public int G_Baika13 { get; set; }

        // 売価14
        [Column(Name = "G_Baika14")]
        public int G_Baika14 { get; set; }

        // 売価15
        [Column(Name = "G_Baika15")]
        public int G_Baika15 { get; set; }

        // 売価16
        [Column(Name = "G_Baika16")]
        public int G_Baika16 { get; set; }

        // 売価17
        [Column(Name = "G_Baika17")]
        public int G_Baika17 { get; set; }

        // 売価18
        [Column(Name = "G_Baika18")]
        public int G_Baika18 { get; set; }

        // 売価19
        [Column(Name = "G_Baika19")]
        public int G_Baika19 { get; set; }

        // 売価20
        [Column(Name = "G_Baika20")]
        public int G_Baika20 { get; set; }

        // リード日数１
        [Column(Name = "G_Read1")]
        public int G_Read1 { get; set; }

        // リード日数２
        [Column(Name = "G_Read2")]
        public int G_Read2 { get; set; }

        // リード日数３
        [Column(Name = "G_Read3")]
        public int G_Read3 { get; set; }

        // リード日数４
        [Column(Name = "G_Read4")]
        public int G_Read4 { get; set; }

        // リード日数５
        [Column(Name = "G_Read5")]
        public int G_Read5 { get; set; }

        // リード日数６
        [Column(Name = "G_Read6")]
        public int G_Read6 { get; set; }

        // リード日数７
        [Column(Name = "G_Read7")]
        public int G_Read7 { get; set; }

        // リード日数８
        [Column(Name = "G_Read8")]
        public int G_Read8 { get; set; }

        // リード日数９
        [Column(Name = "G_Read9")]
        public int G_Read9 { get; set; }

        // リード日数10
        [Column(Name = "G_Read10")]
        public int G_Read10 { get; set; }

        // リード日数11
        [Column(Name = "G_Read11")]
        public int G_Read11 { get; set; }

        // リード日数12
        [Column(Name = "G_Read12")]
        public int G_Read12 { get; set; }

        // リード日数13
        [Column(Name = "G_Read13")]
        public int G_Read13 { get; set; }

        // リード日数14
        [Column(Name = "G_Read14")]
        public int G_Read14 { get; set; }

        // リード日数15
        [Column(Name = "G_Read15")]
        public int G_Read15 { get; set; }

        // リード日数16
        [Column(Name = "G_Read16")]
        public int G_Read16 { get; set; }

        // リード日数17
        [Column(Name = "G_Read17")]
        public int G_Read17 { get; set; }

        // リード日数18
        [Column(Name = "G_Read18")]
        public int G_Read18 { get; set; }

        // リード日数19
        [Column(Name = "G_Read19")]
        public int G_Read19 { get; set; }

        // リード日数20
        [Column(Name = "G_Read20")]
        public int G_Read20 { get; set; }

        // メモ
        [Column(Name = "メモ")]
        public string memo { get; set; }

        // 確認
        [Column(Name = "確認")]
        public int Veri { get; set; }

        // パターンロード
        [Column(Name = "パターンロード")]
        public int PatternLoad { get; set; }

        // 更新年月日
        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }

        // 商品１終売処理
        [Column(Name = "G_Syubai1")]
        public int G_Syubai1 { get; set; }

        // 商品2終売処理
        [Column(Name = "G_Syubai2")]
        public int G_Syubai2 { get; set; }

        // 商品3終売処理
        [Column(Name = "G_Syubai3")]
        public int G_Syubai3 { get; set; }

        // 商品4終売処理
        [Column(Name = "G_Syubai4")]
        public int G_Syubai4 { get; set; }

        // 商品5終売処理
        [Column(Name = "G_Syubai5")]
        public int G_Syubai5 { get; set; }

        // 商品6終売処理
        [Column(Name = "G_Syubai6")]
        public int G_Syubai6 { get; set; }

        // 商品7終売処理
        [Column(Name = "G_Syubai7")]
        public int G_Syubai7 { get; set; }

        // 商品8終売処理
        [Column(Name = "G_Syubai8")]
        public int G_Syubai8 { get; set; }

        // 商品9終売処理
        [Column(Name = "G_Syubai9")]
        public int G_Syubai9 { get; set; }

        // 商品10終売処理
        [Column(Name = "G_Syubai10")]
        public int G_Syubai10 { get; set; }

        // 商品11終売処理
        [Column(Name = "G_Syubai11")]
        public int G_Syubai11 { get; set; }

        // 商品12終売処理
        [Column(Name = "G_Syubai12")]
        public int G_Syubai12 { get; set; }

        // 商品13終売処理
        [Column(Name = "G_Syubai13")]
        public int G_Syubai13 { get; set; }

        // 商品14終売処理
        [Column(Name = "G_Syubai14")]
        public int G_Syubai14 { get; set; }

        // 商品15終売処理
        [Column(Name = "G_Syubai15")]
        public int G_Syubai15 { get; set; }

    }


    // 保留ＦＡＸ発注書データ
    [Table(Name = "Hold_Fax")]
    public class ClsHoldFax
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "画像名")]
        public string ImageFileName { get; set; }

        [Column(Name = "得意先コード")]
        public int TokuisakiCode { get; set; }

        [Column(Name = "patternID")]
        public int patternID { get; set; }

        [Column(Name = "年")]
        public int Year { get; set; }

        [Column(Name = "月")]
        public int Month { get; set; }

        // 店着日
        [Column(Name = "Day1")]
        public int Day1 { get; set; }

        [Column(Name = "Day2")]
        public int Day2 { get; set; }

        [Column(Name = "Day3")]
        public int Day3 { get; set; }

        [Column(Name = "Day4")]
        public int Day4 { get; set; }

        [Column(Name = "Day5")]
        public int Day5 { get; set; }

        [Column(Name = "Day6")]
        public int Day6 { get; set; }

        [Column(Name = "Day7")]
        public int Day7 { get; set; }

        // 商品１
        [Column(Name = "Goods1_1")]
        public int Goods1_1 { get; set; }

        [Column(Name = "Goods1_2")]
        public int Goods1_2 { get; set; }

        [Column(Name = "Goods1_3")]
        public int Goods1_3 { get; set; }

        [Column(Name = "Goods1_4")]
        public int Goods1_4 { get; set; }

        [Column(Name = "Goods1_5")]
        public int Goods1_5 { get; set; }

        [Column(Name = "Goods1_6")]
        public int Goods1_6 { get; set; }

        [Column(Name = "Goods1_7")]
        public int Goods1_7 { get; set; }

        // 商品２
        [Column(Name = "Goods2_1")]
        public int Goods2_1 { get; set; }

        [Column(Name = "Goods2_2")]
        public int Goods2_2 { get; set; }

        [Column(Name = "Goods2_3")]
        public int Goods2_3 { get; set; }

        [Column(Name = "Goods2_4")]
        public int Goods2_4 { get; set; }

        [Column(Name = "Goods2_5")]
        public int Goods2_5 { get; set; }

        [Column(Name = "Goods2_6")]
        public int Goods2_6 { get; set; }

        [Column(Name = "Goods2_7")]
        public int Goods2_7 { get; set; }

        // 商品３
        [Column(Name = "Goods3_1")]
        public int Goods3_1 { get; set; }

        [Column(Name = "Goods3_2")]
        public int Goods3_2 { get; set; }

        [Column(Name = "Goods3_3")]
        public int Goods3_3 { get; set; }

        [Column(Name = "Goods3_4")]
        public int Goods3_4 { get; set; }

        [Column(Name = "Goods3_5")]
        public int Goods3_5 { get; set; }

        [Column(Name = "Goods3_6")]
        public int Goods3_6 { get; set; }

        [Column(Name = "Goods3_7")]
        public int Goods3_7 { get; set; }

        // 商品４
        [Column(Name = "Goods4_1")]
        public int Goods4_1 { get; set; }

        [Column(Name = "Goods4_2")]
        public int Goods4_2 { get; set; }

        [Column(Name = "Goods4_3")]
        public int Goods4_3 { get; set; }

        [Column(Name = "Goods4_4")]
        public int Goods4_4 { get; set; }

        [Column(Name = "Goods4_5")]
        public int Goods4_5 { get; set; }

        [Column(Name = "Goods4_6")]
        public int Goods4_6 { get; set; }

        [Column(Name = "Goods4_7")]
        public int Goods4_7 { get; set; }

        // 商品５
        [Column(Name = "Goods5_1")]
        public int Goods5_1 { get; set; }

        [Column(Name = "Goods5_2")]
        public int Goods5_2 { get; set; }

        [Column(Name = "Goods5_3")]
        public int Goods5_3 { get; set; }

        [Column(Name = "Goods5_4")]
        public int Goods5_4 { get; set; }

        [Column(Name = "Goods5_5")]
        public int Goods5_5 { get; set; }

        [Column(Name = "Goods5_6")]
        public int Goods5_6 { get; set; }

        [Column(Name = "Goods5_7")]
        public int Goods5_7 { get; set; }

        // 商品６
        [Column(Name = "Goods6_1")]
        public int Goods6_1 { get; set; }

        [Column(Name = "Goods6_2")]
        public int Goods6_2 { get; set; }

        [Column(Name = "Goods6_3")]
        public int Goods6_3 { get; set; }

        [Column(Name = "Goods6_4")]
        public int Goods6_4 { get; set; }

        [Column(Name = "Goods6_5")]
        public int Goods6_5 { get; set; }

        [Column(Name = "Goods6_6")]
        public int Goods6_6 { get; set; }

        [Column(Name = "Goods6_7")]
        public int Goods6_7 { get; set; }

        // 商品７
        [Column(Name = "Goods7_1")]
        public int Goods7_1 { get; set; }

        [Column(Name = "Goods7_2")]
        public int Goods7_2 { get; set; }

        [Column(Name = "Goods7_3")]
        public int Goods7_3 { get; set; }

        [Column(Name = "Goods7_4")]
        public int Goods7_4 { get; set; }

        [Column(Name = "Goods7_5")]
        public int Goods7_5 { get; set; }

        [Column(Name = "Goods7_6")]
        public int Goods7_6 { get; set; }

        [Column(Name = "Goods7_7")]
        public int Goods7_7 { get; set; }

        // 商品８
        [Column(Name = "Goods8_1")]
        public int Goods8_1 { get; set; }

        [Column(Name = "Goods8_2")]
        public int Goods8_2 { get; set; }

        [Column(Name = "Goods8_3")]
        public int Goods8_3 { get; set; }

        [Column(Name = "Goods8_4")]
        public int Goods8_4 { get; set; }

        [Column(Name = "Goods8_5")]
        public int Goods8_5 { get; set; }

        [Column(Name = "Goods8_6")]
        public int Goods8_6 { get; set; }

        [Column(Name = "Goods8_7")]
        public int Goods8_7 { get; set; }

        // 商品９
        [Column(Name = "Goods9_1")]
        public int Goods9_1 { get; set; }

        [Column(Name = "Goods9_2")]
        public int Goods9_2 { get; set; }

        [Column(Name = "Goods9_3")]
        public int Goods9_3 { get; set; }

        [Column(Name = "Goods9_4")]
        public int Goods9_4 { get; set; }

        [Column(Name = "Goods9_5")]
        public int Goods9_5 { get; set; }

        [Column(Name = "Goods9_6")]
        public int Goods9_6 { get; set; }

        [Column(Name = "Goods9_7")]
        public int Goods9_7 { get; set; }

        // 商品10
        [Column(Name = "Goods10_1")]
        public int Goods10_1 { get; set; }

        [Column(Name = "Goods10_2")]
        public int Goods10_2 { get; set; }

        [Column(Name = "Goods10_3")]
        public int Goods10_3 { get; set; }

        [Column(Name = "Goods10_4")]
        public int Goods10_4 { get; set; }

        [Column(Name = "Goods10_5")]
        public int Goods10_5 { get; set; }

        [Column(Name = "Goods10_6")]
        public int Goods10_6 { get; set; }

        [Column(Name = "Goods10_7")]
        public int Goods10_7 { get; set; }

        // 商品11
        [Column(Name = "Goods11_1")]
        public int Goods11_1 { get; set; }

        [Column(Name = "Goods11_2")]
        public int Goods11_2 { get; set; }

        [Column(Name = "Goods11_3")]
        public int Goods11_3 { get; set; }

        [Column(Name = "Goods11_4")]
        public int Goods11_4 { get; set; }

        [Column(Name = "Goods11_5")]
        public int Goods11_5 { get; set; }

        [Column(Name = "Goods11_6")]
        public int Goods11_6 { get; set; }

        [Column(Name = "Goods11_7")]
        public int Goods11_7 { get; set; }

        // 商品12
        [Column(Name = "Goods12_1")]
        public int Goods12_1 { get; set; }

        [Column(Name = "Goods12_2")]
        public int Goods12_2 { get; set; }

        [Column(Name = "Goods12_3")]
        public int Goods12_3 { get; set; }

        [Column(Name = "Goods12_4")]
        public int Goods12_4 { get; set; }

        [Column(Name = "Goods12_5")]
        public int Goods12_5 { get; set; }

        [Column(Name = "Goods12_6")]
        public int Goods12_6 { get; set; }

        [Column(Name = "Goods12_7")]
        public int Goods12_7 { get; set; }

        // 商品13
        [Column(Name = "Goods13_1")]
        public int Goods13_1 { get; set; }

        [Column(Name = "Goods13_2")]
        public int Goods13_2 { get; set; }

        [Column(Name = "Goods13_3")]
        public int Goods13_3 { get; set; }

        [Column(Name = "Goods13_4")]
        public int Goods13_4 { get; set; }

        [Column(Name = "Goods13_5")]
        public int Goods13_5 { get; set; }

        [Column(Name = "Goods13_6")]
        public int Goods13_6 { get; set; }

        [Column(Name = "Goods13_7")]
        public int Goods13_7 { get; set; }

        // 商品14
        [Column(Name = "Goods14_1")]
        public int Goods14_1 { get; set; }

        [Column(Name = "Goods14_2")]
        public int Goods14_2 { get; set; }

        [Column(Name = "Goods14_3")]
        public int Goods14_3 { get; set; }

        [Column(Name = "Goods14_4")]
        public int Goods14_4 { get; set; }

        [Column(Name = "Goods14_5")]
        public int Goods14_5 { get; set; }

        [Column(Name = "Goods14_6")]
        public int Goods14_6 { get; set; }

        [Column(Name = "Goods14_7")]
        public int Goods14_7 { get; set; }

        // 商品15
        [Column(Name = "Goods15_1")]
        public int Goods15_1 { get; set; }

        [Column(Name = "Goods15_2")]
        public int Goods15_2 { get; set; }

        [Column(Name = "Goods15_3")]
        public int Goods15_3 { get; set; }

        [Column(Name = "Goods15_4")]
        public int Goods15_4 { get; set; }

        [Column(Name = "Goods15_5")]
        public int Goods15_5 { get; set; }

        [Column(Name = "Goods15_6")]
        public int Goods15_6 { get; set; }

        [Column(Name = "Goods15_7")]
        public int Goods15_7 { get; set; }

        // 商品16
        [Column(Name = "Goods16_1")]
        public int Goods16_1 { get; set; }

        [Column(Name = "Goods16_2")]
        public int Goods16_2 { get; set; }

        [Column(Name = "Goods16_3")]
        public int Goods16_3 { get; set; }

        [Column(Name = "Goods16_4")]
        public int Goods16_4 { get; set; }

        [Column(Name = "Goods16_5")]
        public int Goods16_5 { get; set; }

        [Column(Name = "Goods16_6")]
        public int Goods16_6 { get; set; }

        [Column(Name = "Goods16_7")]
        public int Goods16_7 { get; set; }

        // 商品17
        [Column(Name = "Goods17_1")]
        public int Goods17_1 { get; set; }

        [Column(Name = "Goods17_2")]
        public int Goods17_2 { get; set; }

        [Column(Name = "Goods17_3")]
        public int Goods17_3 { get; set; }

        [Column(Name = "Goods17_4")]
        public int Goods17_4 { get; set; }

        [Column(Name = "Goods17_5")]
        public int Goods17_5 { get; set; }

        [Column(Name = "Goods17_6")]
        public int Goods17_6 { get; set; }

        [Column(Name = "Goods17_7")]
        public int Goods17_7 { get; set; }

        // 商品18
        [Column(Name = "Goods18_1")]
        public int Goods18_1 { get; set; }

        [Column(Name = "Goods18_2")]
        public int Goods18_2 { get; set; }

        [Column(Name = "Goods18_3")]
        public int Goods18_3 { get; set; }

        [Column(Name = "Goods18_4")]
        public int Goods18_4 { get; set; }

        [Column(Name = "Goods18_5")]
        public int Goods18_5 { get; set; }

        [Column(Name = "Goods18_6")]
        public int Goods18_6 { get; set; }

        [Column(Name = "Goods18_7")]
        public int Goods18_7 { get; set; }

        // 商品19
        [Column(Name = "Goods19_1")]
        public int Goods19_1 { get; set; }

        [Column(Name = "Goods19_2")]
        public int Goods19_2 { get; set; }

        [Column(Name = "Goods19_3")]
        public int Goods19_3 { get; set; }

        [Column(Name = "Goods19_4")]
        public int Goods19_4 { get; set; }

        [Column(Name = "Goods19_5")]
        public int Goods19_5 { get; set; }

        [Column(Name = "Goods19_6")]
        public int Goods19_6 { get; set; }

        [Column(Name = "Goods19_7")]
        public int Goods19_7 { get; set; }

        // 商品20
        [Column(Name = "Goods20_1")]
        public int Goods20_1 { get; set; }

        [Column(Name = "Goods20_2")]
        public int Goods20_2 { get; set; }

        [Column(Name = "Goods20_3")]
        public int Goods20_3 { get; set; }

        [Column(Name = "Goods20_4")]
        public int Goods20_4 { get; set; }

        [Column(Name = "Goods20_5")]
        public int Goods20_5 { get; set; }

        [Column(Name = "Goods10_6")]
        public int Goods20_6 { get; set; }

        [Column(Name = "Goods10_7")]
        public int Goods20_7 { get; set; }

        // 商品コード１
        [Column(Name = "G_Code1")]
        public int G_Code1 { get; set; }

        // 商品コード２
        [Column(Name = "G_Code2")]
        public int G_Code2 { get; set; }

        // 商品コード３
        [Column(Name = "G_Code3")]
        public int G_Code3 { get; set; }

        // 商品コード４
        [Column(Name = "G_Code4")]
        public int G_Code4 { get; set; }

        // 商品コード５
        [Column(Name = "G_Code5")]
        public int G_Code5 { get; set; }

        // 商品コード６
        [Column(Name = "G_Code6")]
        public int G_Code6 { get; set; }

        // 商品コード７
        [Column(Name = "G_Code7")]
        public int G_Code7 { get; set; }

        // 商品コード８
        [Column(Name = "G_Code8")]
        public int G_Code8 { get; set; }

        // 商品コード９
        [Column(Name = "G_Code9")]
        public int G_Code9 { get; set; }

        // 商品コード10
        [Column(Name = "G_Code10")]
        public int G_Code10 { get; set; }

        // 商品コード11
        [Column(Name = "G_Code11")]
        public int G_Code11 { get; set; }

        // 商品コード12
        [Column(Name = "G_Code12")]
        public int G_Code12 { get; set; }

        // 商品コード13
        [Column(Name = "G_Code13")]
        public int G_Code13 { get; set; }

        // 商品コード14
        [Column(Name = "G_Code14")]
        public int G_Code14 { get; set; }

        // 商品コード15
        [Column(Name = "G_Code15")]
        public int G_Code15 { get; set; }

        // 商品コード16
        [Column(Name = "G_Code16")]
        public int G_Code16 { get; set; }

        // 商品コード17
        [Column(Name = "G_Code17")]
        public int G_Code17 { get; set; }

        // 商品コード18
        [Column(Name = "G_Code18")]
        public int G_Code18 { get; set; }

        // 商品コード19
        [Column(Name = "G_Code19")]
        public int G_Code19 { get; set; }

        // 商品コード20
        [Column(Name = "G_Code20")]
        public int G_Code20 { get; set; }

        // 納価１
        [Column(Name = "G_Nouka1")]
        public int G_Nouka1 { get; set; }

        // 納価２
        [Column(Name = "G_Nouka2")]
        public int G_Nouka2 { get; set; }

        // 納価３
        [Column(Name = "G_Nouka3")]
        public int G_Nouka3 { get; set; }

        // 納価４
        [Column(Name = "G_Nouka4")]
        public int G_Nouka4 { get; set; }

        // 納価５
        [Column(Name = "G_Nouka5")]
        public int G_Nouka5 { get; set; }

        // 納価６
        [Column(Name = "G_Nouka6")]
        public int G_Nouka6 { get; set; }

        // 納価７
        [Column(Name = "G_Nouka7")]
        public int G_Nouka7 { get; set; }

        // 納価８
        [Column(Name = "G_Nouka8")]
        public int G_Nouka8 { get; set; }

        // 納価９
        [Column(Name = "G_Nouka9")]
        public int G_Nouka9 { get; set; }

        // 納価10
        [Column(Name = "G_Nouka10")]
        public int G_Nouka10 { get; set; }

        // 納価11
        [Column(Name = "G_Nouka11")]
        public int G_Nouka11 { get; set; }

        // 納価12
        [Column(Name = "G_Nouka12")]
        public int G_Nouka12 { get; set; }

        // 納価13
        [Column(Name = "G_Nouka13")]
        public int G_Nouka13 { get; set; }

        // 納価14
        [Column(Name = "G_Nouka14")]
        public int G_Nouka14 { get; set; }

        // 納価15
        [Column(Name = "G_Nouka15")]
        public int G_Nouka15 { get; set; }

        // 納価16
        [Column(Name = "G_Nouka16")]
        public int G_Nouka16 { get; set; }

        // 納価17
        [Column(Name = "G_Nouka17")]
        public int G_Nouka17 { get; set; }

        // 納価18
        [Column(Name = "G_Nouka18")]
        public int G_Nouka18 { get; set; }

        // 納価19
        [Column(Name = "G_Nouka19")]
        public int G_Nouka19 { get; set; }

        // 納価20
        [Column(Name = "G_Nouka20")]
        public int G_Nouka20 { get; set; }

        // 売価１
        [Column(Name = "G_Baika1")]
        public int G_Baika1 { get; set; }

        // 売価２
        [Column(Name = "G_Baika2")]
        public int G_Baika2 { get; set; }

        // 売価３
        [Column(Name = "G_Baika3")]
        public int G_Baika3 { get; set; }

        // 売価４
        [Column(Name = "G_Baika4")]
        public int G_Baika4 { get; set; }

        // 売価５
        [Column(Name = "G_Baika5")]
        public int G_Baika5 { get; set; }

        // 売価６
        [Column(Name = "G_Baika6")]
        public int G_Baika6 { get; set; }

        // 売価７
        [Column(Name = "G_Baika7")]
        public int G_Baika7 { get; set; }

        // 売価８
        [Column(Name = "G_Baika8")]
        public int G_Baika8 { get; set; }

        // 売価９
        [Column(Name = "G_Baika9")]
        public int G_Baika9 { get; set; }

        // 売価10
        [Column(Name = "G_Baika10")]
        public int G_Baika10 { get; set; }

        // 売価11
        [Column(Name = "G_Baika11")]
        public int G_Baika11 { get; set; }

        // 売価12
        [Column(Name = "G_Baika12")]
        public int G_Baika12 { get; set; }

        // 売価13
        [Column(Name = "G_Baika13")]
        public int G_Baika13 { get; set; }

        // 売価14
        [Column(Name = "G_Baika14")]
        public int G_Baika14 { get; set; }

        // 売価15
        [Column(Name = "G_Baika15")]
        public int G_Baika15 { get; set; }

        // 売価16
        [Column(Name = "G_Baika16")]
        public int G_Baika16 { get; set; }

        // 売価17
        [Column(Name = "G_Baika17")]
        public int G_Baika17 { get; set; }

        // 売価18
        [Column(Name = "G_Baika18")]
        public int G_Baika18 { get; set; }

        // 売価19
        [Column(Name = "G_Baika19")]
        public int G_Baika19 { get; set; }

        // 売価20
        [Column(Name = "G_Baika20")]
        public int G_Baika20 { get; set; }

        // リード日数１
        [Column(Name = "G_Read1")]
        public int G_Read1 { get; set; }

        // リード日数２
        [Column(Name = "G_Read2")]
        public int G_Read2 { get; set; }

        // リード日数３
        [Column(Name = "G_Read3")]
        public int G_Read3 { get; set; }

        // リード日数４
        [Column(Name = "G_Read4")]
        public int G_Read4 { get; set; }

        // リード日数５
        [Column(Name = "G_Read5")]
        public int G_Read5 { get; set; }

        // リード日数６
        [Column(Name = "G_Read6")]
        public int G_Read6 { get; set; }

        // リード日数７
        [Column(Name = "G_Read7")]
        public int G_Read7 { get; set; }

        // リード日数８
        [Column(Name = "G_Read8")]
        public int G_Read8 { get; set; }

        // リード日数９
        [Column(Name = "G_Read9")]
        public int G_Read9 { get; set; }

        // リード日数10
        [Column(Name = "G_Read10")]
        public int G_Read10 { get; set; }

        // リード日数11
        [Column(Name = "G_Read11")]
        public int G_Read11 { get; set; }

        // リード日数12
        [Column(Name = "G_Read12")]
        public int G_Read12 { get; set; }

        // リード日数13
        [Column(Name = "G_Read13")]
        public int G_Read13 { get; set; }

        // リード日数14
        [Column(Name = "G_Read14")]
        public int G_Read14 { get; set; }

        // リード日数15
        [Column(Name = "G_Read15")]
        public int G_Read15 { get; set; }

        // リード日数16
        [Column(Name = "G_Read16")]
        public int G_Read16 { get; set; }

        // リード日数17
        [Column(Name = "G_Read17")]
        public int G_Read17 { get; set; }

        // リード日数18
        [Column(Name = "G_Read18")]
        public int G_Read18 { get; set; }

        // リード日数19
        [Column(Name = "G_Read19")]
        public int G_Read19 { get; set; }

        // リード日数20
        [Column(Name = "G_Read20")]
        public int G_Read20 { get; set; }

        // メモ
        [Column(Name = "メモ")]
        public string memo { get; set; }

        // 確認
        [Column(Name = "確認")]
        public int Veri { get; set; }

        // 更新年月日
        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }
    }


    // 発注パターンマスター
    [Table(Name = "orderpattern")]
    public class ClsOrderPattern
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "得意先コード")]
        public int TokuisakiCode { get; set; }

        [Column(Name = "連番")]
        public int SeqNum { get; set; }

        [Column(Name = "枝番")]
        public int SecondNum { get; set; }

        // 商品コード１
        [Column(Name = "商品1")]
        public string G_Code1 { get; set; }

        // 商品コード２
        [Column(Name = "商品2")]
        public string G_Code2 { get; set; }

        // 商品コード３
        [Column(Name = "商品3")]
        public string G_Code3 { get; set; }

        // 商品コード４
        [Column(Name = "商品4")]
        public string G_Code4 { get; set; }

        // 商品コード５
        [Column(Name = "商品5")]
        public string G_Code5 { get; set; }

        // 商品コード６
        [Column(Name = "商品6")]
        public string G_Code6 { get; set; }

        // 商品コード７
        [Column(Name = "商品7")]
        public string G_Code7 { get; set; }

        // 商品コード８
        [Column(Name = "商品8")]
        public string G_Code8 { get; set; }

        // 商品コード９
        [Column(Name = "商品9")]
        public string G_Code9 { get; set; }

        // 商品コード10
        [Column(Name = "商品10")]
        public string G_Code10 { get; set; }

        // 商品コード11
        [Column(Name = "商品11")]
        public string G_Code11 { get; set; }

        // 商品コード12
        [Column(Name = "商品12")]
        public string G_Code12 { get; set; }

        // 商品コード13
        [Column(Name = "商品13")]
        public string G_Code13 { get; set; }

        // 商品コード14
        [Column(Name = "商品14")]
        public string G_Code14 { get; set; }

        // 商品コード15
        [Column(Name = "商品15")]
        public string G_Code15 { get; set; }

        // 商品コード16
        [Column(Name = "商品16")]
        public string G_Code16 { get; set; }

        // 商品コード17
        [Column(Name = "商品17")]
        public string G_Code17 { get; set; }

        // 商品コード18
        [Column(Name = "商品18")]
        public string G_Code18 { get; set; }

        // 商品コード19
        [Column(Name = "商品19")]
        public string G_Code19 { get; set; }

        // 商品コード20
        [Column(Name = "商品20")]
        public string G_Code20 { get; set; }

        [Column(Name = "商品名1")]
        public string G_Name1 { get; set; }

        [Column(Name = "商品名2")]
        public string G_Name2 { get; set; }

        [Column(Name = "商品名3")]
        public string G_Name3 { get; set; }

        [Column(Name = "商品名4")]
        public string G_Name4 { get; set; }

        [Column(Name = "商品名5")]
        public string G_Name5 { get; set; }

        [Column(Name = "商品名6")]
        public string G_Name6 { get; set; }

        [Column(Name = "商品名7")]
        public string G_Name7 { get; set; }

        [Column(Name = "商品名8")]
        public string G_Name8 { get; set; }

        [Column(Name = "商品名9")]
        public string G_Name9 { get; set; }

        [Column(Name = "商品名10")]
        public string G_Name10 { get; set; }

        [Column(Name = "商品名11")]
        public string G_Name11 { get; set; }

        [Column(Name = "商品名12")]
        public string G_Name12 { get; set; }

        [Column(Name = "商品名13")]
        public string G_Name13 { get; set; }

        [Column(Name = "商品名14")]
        public string G_Name14 { get; set; }

        [Column(Name = "商品名15")]
        public string G_Name15 { get; set; }

        [Column(Name = "商品名16")]
        public string G_Name16 { get; set; }

        [Column(Name = "商品名17")]
        public string G_Name17 { get; set; }

        [Column(Name = "商品名18")]
        public string G_Name18 { get; set; }

        [Column(Name = "商品名19")]
        public string G_Name19 { get; set; }

        [Column(Name = "商品名20")]
        public string G_Name20 { get; set; }

        // リード日数１
        [Column(Name = "商品1リード日数")]
        public int G_Read1 { get; set; }

        // リード日数２
        [Column(Name = "商品2リード日数")]
        public int G_Read2 { get; set; }

        // リード日数３
        [Column(Name = "商品3リード日数")]
        public int G_Read3 { get; set; }

        // リード日数４
        [Column(Name = "商品4リード日数")]
        public int G_Read4 { get; set; }

        // リード日数５
        [Column(Name = "商品5リード日数")]
        public int G_Read5 { get; set; }

        // リード日数６
        [Column(Name = "商品6リード日数")]
        public int G_Read6 { get; set; }

        // リード日数７
        [Column(Name = "商品7リード日数")]
        public int G_Read7 { get; set; }

        // リード日数８
        [Column(Name = "商品8リード日数")]
        public int G_Read8 { get; set; }

        // リード日数９
        [Column(Name = "商品9リード日数")]
        public int G_Read9 { get; set; }

        // リード日数10
        [Column(Name = "商品10リード日数")]
        public int G_Read10 { get; set; }

        // リード日数11
        [Column(Name = "商品11リード日数")]
        public int G_Read11 { get; set; }

        // リード日数12
        [Column(Name = "商品12リード日数")]
        public int G_Read12 { get; set; }

        // リード日数13
        [Column(Name = "商品13リード日数")]
        public int G_Read13 { get; set; }

        // リード日数14
        [Column(Name = "商品14リード日数")]
        public int G_Read14 { get; set; }

        // リード日数15
        [Column(Name = "商品15リード日数")]
        public int G_Read15 { get; set; }

        // リード日数16
        [Column(Name = "商品16リード日数")]
        public int G_Read16 { get; set; }

        // リード日数17
        [Column(Name = "商品17リード日数")]
        public int G_Read17 { get; set; }

        // リード日数18
        [Column(Name = "商品18リード日数")]
        public int G_Read18 { get; set; }

        // リード日数19
        [Column(Name = "商品19リード日数")]
        public int G_Read19 { get; set; }

        // リード日数20
        [Column(Name = "商品20リード日数")]
        public int G_Read20 { get; set; }

        // 備考
        [Column(Name = "備考")]
        public string Memo { get; set; }

        // 更新年月日
        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }
    }
    
    // 返信ＦＡＸ定型コメント
    [Table(Name = "ReFaxComment")]
    public class ClsReFaxComment
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "定型コメント")]
        public string Comment { get; set; }

        [Column(Name = "更新年月日")]
        public string YyMmDd { get; set; }
    }

    // ＣＳＶデータ作成履歴
    [Table(Name = "CsvOutHistory")]
    public class ClsCsvOutHistory
    {
        [Column(Name = "ID", IsPrimaryKey = true)]
        public int ID { get; set; }

        [Column(Name = "作成年月日時刻")]
        public string WriteDateTime { get; set; }

        [Column(Name = "コンピュータ名")]
        public string PcName { get; set; }

        [Column(Name = "書き込みモード")]
        public int WriteMode { get; set; }

        [Column(Name = "出力件数")]
        public int OutPutCount { get; set; }
    }

}
