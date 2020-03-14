using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Data.Linq;
using System.Data.SQLite;
using STSH_OCR.Common;

namespace STSH_OCR.OCR
{
    partial class frmOrder
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
        ///     データを画面に表示します </summary>
        /// <param name="iX">
        ///     ヘッダデータインデックス</param>
        ///------------------------------------------------------------------------------------
        private void showOcrData(string sID)
        {
            Cursor = Cursors.WaitCursor;
            showStatus = true;

            // 非ログ書き込み状態とする
            editLogStatus = false;

            // フォーム初期化
            formInitialize(sID);

            // 発注データを取得
            Order = tblOrder.Single(a => a.ID == sID);

            global.ChangeValueStatus = false;   // これ以下ChangeValueイベントを発生させない

            string Sql = "select * from OrderData WHERE ID = '" + sID + "'";

            using (SQLiteCommand com = new SQLiteCommand(Sql, cn))
            {
                SQLiteDataReader dataReader = com.ExecuteReader();

                while (dataReader.Read())
                {
                    txtYear.Text = dataReader["年"].ToString();
                    txtMonth.Text = dataReader["月"].ToString();
                    txtTokuisakiCD.Text = dataReader["得意先コード"].ToString().PadLeft(7, '0');
                    txtPID.Text = dataReader["patternID"].ToString();
                    txtSeqNum.Text = dataReader["SeqNumber"].ToString();

                    // 店着日
                    txtTenDay1.Text = dataReader["Day1"].ToString();
                    txtTenDay2.Text = dataReader["Day2"].ToString();
                    txtTenDay3.Text = dataReader["Day3"].ToString();
                    txtTenDay4.Text = dataReader["Day4"].ToString();
                    txtTenDay5.Text = dataReader["Day5"].ToString();
                    txtTenDay6.Text = dataReader["Day6"].ToString();
                    txtTenDay7.Text = dataReader["Day7"].ToString();

                    checkBox1.Checked = Convert.ToBoolean(Utility.StrtoInt(Utility.NulltoStr(dataReader["確認"])));
                    txtMemo.Text = dataReader["メモ"].ToString();

                    global.ChangeValueStatus = true;    // これ以下ChangeValueイベントを発生させる

                    // FAX発注書データ表示
                    showItem(dataReader, dg1);

                    // エラー情報表示初期化
                    lblErrMsg.Visible = false;
                    lblErrMsg.Text = string.Empty;

                    // 画像表示
                    _img = Utility.GetImageFilePath(Config.ImgPath, dataReader["得意先コード"].ToString().PadLeft(7, '0')) + @"\" + dataReader["画像名"].ToString();
                    showImage_openCv(_img);
                    trackBar1.Enabled = true;

                    label3.Text = "[" + dataReader["ID"].ToString() + "]";
                }

                dataReader.Close();
            }

            // ログ書き込み状態とする
            editLogStatus = true;

            showStatus = false;
            Cursor = Cursors.Default;
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
        private void showItem(SQLiteDataReader r, DataGridView dataGrid)
        {
            global.ChangeValueStatus = false;
            bool g_Ptn = false;

            // 発注書パターン未読み込みの発注データのとき
            if (Utility.StrtoInt(r["パターンロード"].ToString()) == global.flgOff)
            {
                // 該当するＦＡＸ発注書パターンが存在するとき
                if (tblPtn.Any(a => a.TokuisakiCode == Utility.StrtoInt(r["得意先コード"].ToString()) &&
                                    a.SeqNum == Utility.StrtoInt(r["patternID"].ToString()) &&
                                    a.SecondNum == Utility.StrtoInt(r["SeqNumber"].ToString())))
                {
                    ClsOrderPattern = tblPtn.Single(a => a.TokuisakiCode == Utility.StrtoInt(r["得意先コード"].ToString()) &&
                                    a.SeqNum == Utility.StrtoInt(r["patternID"].ToString()) &&
                                    a.SecondNum == Utility.StrtoInt(r["SeqNumber"].ToString()));

                    // ＦＡＸ発注書パターンの商品構成とする
                    g_Ptn = true;
                }
            }

            // 商品発注明細クラス
            ClsGoods[] goods = new ClsGoods[15];
            for (int i = 0; i < 15; i++)
            {
                goods[i] = new ClsGoods();
                goods[i].Suu = new string[7];

                switch (i)
                {
                    case 0:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code1;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code1"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka1"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika1"].ToString());
                        goods[i].Suu[0] = r["Goods1_1"].ToString();
                        goods[i].Suu[1] = r["Goods1_2"].ToString();
                        goods[i].Suu[2] = r["Goods1_3"].ToString();
                        goods[i].Suu[3] = r["Goods1_4"].ToString();
                        goods[i].Suu[4] = r["Goods1_5"].ToString();
                        goods[i].Suu[5] = r["Goods1_6"].ToString();
                        goods[i].Suu[6] = r["Goods1_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai1"].ToString());

                        break;

                    case 1:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code2;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code2"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka2"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika2"].ToString());
                        goods[i].Suu[0] = r["Goods2_1"].ToString();
                        goods[i].Suu[1] = r["Goods2_2"].ToString();
                        goods[i].Suu[2] = r["Goods2_3"].ToString();
                        goods[i].Suu[3] = r["Goods2_4"].ToString();
                        goods[i].Suu[4] = r["Goods2_5"].ToString();
                        goods[i].Suu[5] = r["Goods2_6"].ToString();
                        goods[i].Suu[6] = r["Goods2_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai2"].ToString());

                        break;


                    case 2:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code3;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code3"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka3"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika3"].ToString());
                        goods[i].Suu[0] = r["Goods3_1"].ToString();
                        goods[i].Suu[1] = r["Goods3_2"].ToString();
                        goods[i].Suu[2] = r["Goods3_3"].ToString();
                        goods[i].Suu[3] = r["Goods3_4"].ToString();
                        goods[i].Suu[4] = r["Goods3_5"].ToString();
                        goods[i].Suu[5] = r["Goods3_6"].ToString();
                        goods[i].Suu[6] = r["Goods3_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai3"].ToString());

                        break;


                    case 3:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code4;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code4"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka4"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika4"].ToString());
                        goods[i].Suu[0] = r["Goods4_1"].ToString();
                        goods[i].Suu[1] = r["Goods4_2"].ToString();
                        goods[i].Suu[2] = r["Goods4_3"].ToString();
                        goods[i].Suu[3] = r["Goods4_4"].ToString();
                        goods[i].Suu[4] = r["Goods4_5"].ToString();
                        goods[i].Suu[5] = r["Goods4_6"].ToString();
                        goods[i].Suu[6] = r["Goods4_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai4"].ToString());

                        break;

                    case 4:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code5;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code5"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka5"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika5"].ToString());
                        goods[i].Suu[0] = r["Goods5_1"].ToString();
                        goods[i].Suu[1] = r["Goods5_2"].ToString();
                        goods[i].Suu[2] = r["Goods5_3"].ToString();
                        goods[i].Suu[3] = r["Goods5_4"].ToString();
                        goods[i].Suu[4] = r["Goods5_5"].ToString();
                        goods[i].Suu[5] = r["Goods5_6"].ToString();
                        goods[i].Suu[6] = r["Goods5_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai5"].ToString());

                        break;

                    case 5:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code6;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code6"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka6"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika6"].ToString());
                        goods[i].Suu[0] = r["Goods6_1"].ToString();
                        goods[i].Suu[1] = r["Goods6_2"].ToString();
                        goods[i].Suu[2] = r["Goods6_3"].ToString();
                        goods[i].Suu[3] = r["Goods6_4"].ToString();
                        goods[i].Suu[4] = r["Goods6_5"].ToString();
                        goods[i].Suu[5] = r["Goods6_6"].ToString();
                        goods[i].Suu[6] = r["Goods6_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai6"].ToString());

                        break;

                    case 6:

                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code7;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code7"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka7"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika7"].ToString());
                        goods[i].Suu[0] = r["Goods7_1"].ToString();
                        goods[i].Suu[1] = r["Goods7_2"].ToString();
                        goods[i].Suu[2] = r["Goods7_3"].ToString();
                        goods[i].Suu[3] = r["Goods7_4"].ToString();
                        goods[i].Suu[4] = r["Goods7_5"].ToString();
                        goods[i].Suu[5] = r["Goods7_6"].ToString();
                        goods[i].Suu[6] = r["Goods7_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai7"].ToString());

                        break;

                    case 7:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code8;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code8"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka8"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika8"].ToString());
                        goods[i].Suu[0] = r["Goods8_1"].ToString();
                        goods[i].Suu[1] = r["Goods8_2"].ToString();
                        goods[i].Suu[2] = r["Goods8_3"].ToString();
                        goods[i].Suu[3] = r["Goods8_4"].ToString();
                        goods[i].Suu[4] = r["Goods8_5"].ToString();
                        goods[i].Suu[5] = r["Goods8_6"].ToString();
                        goods[i].Suu[6] = r["Goods8_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai8"].ToString());

                        break;


                    case 8:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code9;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code9"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka9"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika9"].ToString());
                        goods[i].Suu[0] = r["Goods9_1"].ToString();
                        goods[i].Suu[1] = r["Goods9_2"].ToString();
                        goods[i].Suu[2] = r["Goods9_3"].ToString();
                        goods[i].Suu[3] = r["Goods9_4"].ToString();
                        goods[i].Suu[4] = r["Goods9_5"].ToString();
                        goods[i].Suu[5] = r["Goods9_6"].ToString();
                        goods[i].Suu[6] = r["Goods9_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai9"].ToString());

                        break;


                    case 9:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code10;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code10"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka10"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika10"].ToString());
                        goods[i].Suu[0] = r["Goods10_1"].ToString();
                        goods[i].Suu[1] = r["Goods10_2"].ToString();
                        goods[i].Suu[2] = r["Goods10_3"].ToString();
                        goods[i].Suu[3] = r["Goods10_4"].ToString();
                        goods[i].Suu[4] = r["Goods10_5"].ToString();
                        goods[i].Suu[5] = r["Goods10_6"].ToString();
                        goods[i].Suu[6] = r["Goods10_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai10"].ToString());

                        break;


                    case 10:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code11;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code11"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka11"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika11"].ToString());
                        goods[i].Suu[0] = r["Goods11_1"].ToString();
                        goods[i].Suu[1] = r["Goods11_2"].ToString();
                        goods[i].Suu[2] = r["Goods11_3"].ToString();
                        goods[i].Suu[3] = r["Goods11_4"].ToString();
                        goods[i].Suu[4] = r["Goods11_5"].ToString();
                        goods[i].Suu[5] = r["Goods11_6"].ToString();
                        goods[i].Suu[6] = r["Goods11_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai11"].ToString());

                        break;


                    case 11:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code12;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code12"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka12"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika12"].ToString());
                        goods[i].Suu[0] = r["Goods12_1"].ToString();
                        goods[i].Suu[1] = r["Goods12_2"].ToString();
                        goods[i].Suu[2] = r["Goods12_3"].ToString();
                        goods[i].Suu[3] = r["Goods12_4"].ToString();
                        goods[i].Suu[4] = r["Goods12_5"].ToString();
                        goods[i].Suu[5] = r["Goods12_6"].ToString();
                        goods[i].Suu[6] = r["Goods12_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai12"].ToString());

                        break;


                    case 12:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code13;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code13"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka13"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika13"].ToString());
                        goods[i].Suu[0] = r["Goods13_1"].ToString();
                        goods[i].Suu[1] = r["Goods13_2"].ToString();
                        goods[i].Suu[2] = r["Goods13_3"].ToString();
                        goods[i].Suu[3] = r["Goods13_4"].ToString();
                        goods[i].Suu[4] = r["Goods13_5"].ToString();
                        goods[i].Suu[5] = r["Goods13_6"].ToString();
                        goods[i].Suu[6] = r["Goods13_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai13"].ToString());

                        break;


                    case 13:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code14;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code14"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka14"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika14"].ToString());
                        goods[i].Suu[0] = r["Goods14_1"].ToString();
                        goods[i].Suu[1] = r["Goods14_2"].ToString();
                        goods[i].Suu[2] = r["Goods14_3"].ToString();
                        goods[i].Suu[3] = r["Goods14_4"].ToString();
                        goods[i].Suu[4] = r["Goods14_5"].ToString();
                        goods[i].Suu[5] = r["Goods14_6"].ToString();
                        goods[i].Suu[6] = r["Goods14_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai14"].ToString());

                        break;

                    case 14:
                        if (g_Ptn)
                        {
                            goods[i].Code = ClsOrderPattern.G_Code15;
                        }
                        else
                        {
                            goods[i].Code = r["G_Code15"].ToString();
                        }

                        goods[i].Nouka = Utility.StrtoInt(r["G_Nouka15"].ToString());
                        goods[i].Baika = Utility.StrtoInt(r["G_Baika15"].ToString());
                        goods[i].Suu[0] = r["Goods15_1"].ToString();
                        goods[i].Suu[1] = r["Goods15_2"].ToString();
                        goods[i].Suu[2] = r["Goods15_3"].ToString();
                        goods[i].Suu[3] = r["Goods15_4"].ToString();
                        goods[i].Suu[4] = r["Goods15_5"].ToString();
                        goods[i].Suu[5] = r["Goods15_6"].ToString();
                        goods[i].Suu[6] = r["Goods15_7"].ToString();
                        goods[i].Syubai = Utility.StrtoInt(r["G_Syubai15"].ToString());

                        break;

                    default:
                        break;
                }

            }


            for (int i = 0; i < 15; i++)
            {
                global.ChangeValueStatus = true;
                dataGrid[colHinCode, i * 2 + 1].Value = goods[i].Code;
                global.ChangeValueStatus = false;

                if (goods[i].Nouka != 0)
                {
                    dataGrid[colNouka, i * 2 + 1].Value = goods[i].Nouka;
                }

                if (goods[i].Baika != 0)
                {
                    dataGrid[colBaika, i * 2 + 1].Value = goods[i].Baika;
                }
                
                dataGrid[colDay1, i * 2 + 1].Value = goods[i].Suu[0];
                dataGrid[colDay2, i * 2 + 1].Value = goods[i].Suu[1];
                dataGrid[colDay3, i * 2 + 1].Value = goods[i].Suu[2];
                dataGrid[colDay4, i * 2 + 1].Value = goods[i].Suu[3];
                dataGrid[colDay5, i * 2 + 1].Value = goods[i].Suu[4];
                dataGrid[colDay6, i * 2 + 1].Value = goods[i].Suu[5];
                dataGrid[colDay7, i * 2 + 1].Value = goods[i].Suu[6];

                dg1.Rows[i * 2 + 1].Cells[colSyubai].Value = global.SyubaiArray[goods[i].Syubai];
            }


            //カレントセル選択状態としない
            dg1.CurrentCell = null;

            //カレントセル選択状態としない
            dg1.CurrentCell = null;
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
        private void formInitialize(string sID)
        {
            global.ChangeValueStatus = false;   // これ以下ChangeValueイベントを発生させない

            // テキストボックス表示色設定
            txtYear.BackColor = Color.White;
            txtMonth.BackColor = Color.White;
            txtPID.BackColor = Color.White;
            txtSeqNum.BackColor = Color.White;
            txtTokuisakiCD.BackColor = Color.White;
            txtTenDay1.BackColor = Color.White;
            txtTenDay2.BackColor = Color.White;
            txtTenDay3.BackColor = Color.White;
            txtTenDay4.BackColor = Color.White;
            txtTenDay5.BackColor = Color.White;
            txtTenDay6.BackColor = Color.White;
            txtTenDay7.BackColor = Color.White;
            checkBox1.BackColor = SystemColors.Control;

            txtYear.ForeColor = global.defaultColor;
            txtMonth.ForeColor = global.defaultColor;
            txtPID.ForeColor = global.defaultColor;
            txtSeqNum.ForeColor = global.defaultColor;
            txtTokuisakiCD.ForeColor = global.defaultColor;
            txtTenDay1.ForeColor = global.defaultColor;
            txtTenDay2.ForeColor = global.defaultColor;
            txtTenDay3.ForeColor = global.defaultColor;
            txtTenDay4.ForeColor = global.defaultColor;
            txtTenDay5.ForeColor = global.defaultColor;
            txtTenDay6.ForeColor = global.defaultColor;
            txtTenDay7.ForeColor = global.defaultColor;
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

            // 編集不可行
            for (int i = 0; i < dg1.RowCount; i+= 2)
            {
                dg1[colNouka, i].ReadOnly = true;
                dg1[colBaika, i].ReadOnly = true;
                dg1[colDay1, i].ReadOnly = true;
                dg1[colDay2, i].ReadOnly = true;
                dg1[colDay3, i].ReadOnly = true;
                dg1[colDay4, i].ReadOnly = true;
                dg1[colDay5, i].ReadOnly = true;
                dg1[colDay6, i].ReadOnly = true;
                dg1[colDay7, i].ReadOnly = true;
                dg1[colSyubai, i].ReadOnly = true;
            }

            for (int i = 1; i < dg1.RowCount; i += 2)
            {
                if (i % 2 == 0)
                {
                    dg1[colNouka, i].ReadOnly = true;
                    dg1[colBaika, i].ReadOnly = true;
                    dg1[colDay1, i].ReadOnly = true;
                    dg1[colDay2, i].ReadOnly = true;
                    dg1[colDay3, i].ReadOnly = true;
                    dg1[colDay4, i].ReadOnly = true;
                    dg1[colDay5, i].ReadOnly = true;
                    dg1[colDay6, i].ReadOnly = true;
                    dg1[colDay7, i].ReadOnly = true;
                    dg1[colSyubai, i].ReadOnly = true;
                }

                dg1[colSyubai, i] = new DataGridViewComboBoxCell();
                dg1[colSyubai, i].ReadOnly = false; // 編集可能に設定

                // コンボボックスにアイテムを追加する
                for (int iX = 0; iX < global.SyubaiArray.Length; iX++)
                {
                    ((DataGridViewComboBoxCell)dg1[colSyubai, i]).Items.Add(global.SyubaiArray[iX]);
                }
            }

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

            // ヘッダ情報
            txtYear.ReadOnly = false;
            txtMonth.ReadOnly = false;

            //// スクロールバー設定
            //hScrollBar1.Enabled = true;
            //hScrollBar1.Minimum = 0;
            //hScrollBar1.Maximum = cID.Length - 1;
            //hScrollBar1.Value = cIx;
            //hScrollBar1.LargeChange = 1;
            //hScrollBar1.SmallChange = 1;

            ////移動ボタン制御
            //btnFirst.Enabled = true;
            //btnNext.Enabled = true;
            //btnBefore.Enabled = true;
            //btnEnd.Enabled = true;

            ////最初のレコード
            //if (cIx == 0)
            //{
            //    btnBefore.Enabled = false;
            //    btnFirst.Enabled = false;
            //}

            ////最終レコード
            //if ((cIx + 1) == cID.Length)
            //{
            //    btnNext.Enabled = false;
            //    btnEnd.Enabled = false;
            //}

            // その他のボタンを有効とする
            btnErrCheck.Visible = true;
            btnDelete.Visible = true;
            btnPrint.Visible = true;
            btnUpdate.Visible = true;

            //データ数表示
            lblPage.Text = "";

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

                // 年月
                if (ocr._errNumber == ocr.eYearMonth)
                {
                    txtYear.BackColor = Color.Yellow;
                    txtYear.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eMonth)
                {
                    txtMonth.BackColor = Color.Yellow;
                    txtMonth.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 得意先コード
                if (ocr._errNumber == ocr.eTdkNo)
                {
                    txtTokuisakiCD.BackColor = Color.Yellow;
                    txtTokuisakiCD.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // パターンID
                if (ocr._errNumber == ocr.ePattern)
                {
                    txtPID.BackColor = Color.Yellow;
                    txtPID.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 店着日付
                if (ocr._errNumber == ocr.eTenDate1)
                {
                    txtTenDay1.BackColor = Color.Yellow;
                    txtTenDay1.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate2)
                {
                    txtTenDay2.BackColor = Color.Yellow;
                    txtTenDay2.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate3)
                {
                    txtTenDay3.BackColor = Color.Yellow;
                    txtTenDay3.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate4)
                {
                    txtTenDay4.BackColor = Color.Yellow;
                    txtTenDay4.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate5)
                {
                    txtTenDay5.BackColor = Color.Yellow;
                    txtTenDay5.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate6)
                {
                    txtTenDay6.BackColor = Color.Yellow;
                    txtTenDay6.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                if (ocr._errNumber == ocr.eTenDate7)
                {
                    txtTenDay7.BackColor = Color.Yellow;
                    txtTenDay7.Focus();

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }
                
                // 商品コード
                if (ocr._errNumber == ocr.eHinCode)
                {
                    dg1[colHinCode,  ocr._errRow - 1].Style.BackColor = Color.Yellow;
                    dg1[colHinCode,  ocr._errRow].Style.BackColor = Color.Yellow;
                    dg1.Focus();
                    dg1.CurrentCell = dg1[colHinCode, ocr._errRow];

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }

                // 終売コンボボックス
                if (ocr._errNumber == ocr.eShubai)
                {
                    dg1[colSyubai, ocr._errRow - 1].Style.BackColor = Color.Yellow;
                    dg1.Focus();
                    dg1.CurrentCell = dg1[colSyubai, ocr._errRow];

                    // エラー有りフラグ
                    txtErrStatus.Text = global.FLGON;
                }


                // グリッドビューCellEnterイベントステータスを戻す
                gridViewCellEnterStatus = true;
            }
        }

    }
}
