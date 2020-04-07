using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using STSH_OCR.Common;

namespace STSH_OCR.OCR
{
    public partial class frmNgRecovery : Form
    {
        public frmNgRecovery()
        {
            InitializeComponent();
        }

        string _InPath = Properties.Settings.Default.NgPath;
        string _OutPath = Properties.Settings.Default.MyDataPath;

        clsNG[] ngf;

        string _img = string.Empty;
        global gl = new global();

        // openCvSharp 関連　2019/08/19
        const float B_WIDTH = 0.45f;
        const float B_HEIGHT = 0.45f;
        const float A_WIDTH = 0.05f;
        const float A_HEIGHT = 0.05f;

        float n_width = 0f;
        float n_height = 0f;

        Mat mMat = new Mat();

        private void frmNgRecovery_Load(object sender, EventArgs e)
        {
            MaximumSize = new System.Drawing.Size(Width, Height);
            MinimumSize = new System.Drawing.Size(Width, Height);

            // NGリスト
            GetNgList();

            // ボタン
            trackBar1.Enabled = false;
            btnLeft.Enabled = false;
        }

        private void frmNgRecovery_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     ＮＧ画像リストを表示する </summary>
        ///----------------------------------------------------------
        private void GetNgList()
        {
            checkedListBox1.Items.Clear();
            string[] f = System.IO.Directory.GetFiles(_InPath, "*.tif");

            if (f.Length == 0)
            {
                label2.Text = "NG画像はありませんでした";

                linkLblOn.Enabled = false;
                linkLblOff.Enabled = false;

                button2.Enabled = false;
                button5.Enabled = false;
                btnPrn.Enabled = false;
                btnDelete.Enabled = false;

                return;
            }

            ngf = new clsNG[f.Length];

            int Cnt = 0;

            foreach (string files in System.IO.Directory.GetFiles(_InPath, "*.tif"))
            {
                ngf[Cnt] = new clsNG();
                ngf[Cnt].ngFileName = files;
                string fn = System.IO.Path.GetFileName(files);
                ngf[Cnt].ngRecDate = fn.Substring(0, 4) + "年" + fn.Substring(4, 2) + "月" + fn.Substring(6, 2) + "日" +
                                     fn.Substring(8, 2) + "時" + fn.Substring(10, 2) + "分" + fn.Substring(12, 2) + "秒";

                checkedListBox1.Items.Add(System.IO.Path.GetFileName(ngf[Cnt].ngRecDate));
                Cnt++;
            }

            label2.Text = f.Length.ToString() + "件のＮＧ画像があります";

            linkLblOn.Enabled = true;
            linkLblOff.Enabled = true;

            button2.Enabled = true;
            button5.Enabled = true;
            btnPrn.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem == null)
            {
                return;
            }
            else
            {
                trackBar1.Enabled = true;
                btnLeft.Enabled = true;
            }

            if (!System.IO.File.Exists(ngf[checkedListBox1.SelectedIndex].ngFileName))
            {
                trackBar1.Enabled = false;
                btnLeft.Enabled = false;
                return;
            }

            //画像イメージ表示
            showImage_openCv(ngf[checkedListBox1.SelectedIndex].ngFileName);

            trackBar1.Enabled = true;
            btnLeft.Enabled = true;
        }

        private class clsNG
        {
            public string ngFileName { get; set; }
            public string ngRecDate { get; set; }
        }

        ///--------------------------------------------------------------
        /// <summary>
        ///     ＮＧファイルリカバリ </summary>
        ///--------------------------------------------------------------
        private void NgRecovery()
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                if (MessageBox.Show(ngFileCount().ToString() + "件の画像を発注書データとしてリカバリします。よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            DateTime dt = DateTime.Now;
            string _ID = string.Format("{0:0000}", dt.Year) + string.Format("{0:00}", dt.Month) +
                         string.Format("{0:00}", dt.Day) + string.Format("{0:00}", dt.Hour) +
                         string.Format("{0:00}", dt.Minute) + string.Format("{0:00}", dt.Second);

            // ＮＧファイルリカバリ処理
            int fCnt = 1;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    NgToData(_ID, fCnt, i);
                    fCnt++;
                }
            }

            // 終了メッセージ
            MessageBox.Show(ngFileCount().ToString() + "件の画像を発注書データとしてリカバリし受信フォルダへ移動しました", "リカバリー処理完了", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // ＮＧ画像リスト再表示
            GetNgList();

            // イメージ表示初期化
            pictureBox1.Image = null;
            trackBar1.Enabled = false;
            btnLeft.Enabled = false;
        }

        ///--------------------------------------------------------------------------
        /// <summary>
        ///     ＣＳＶデータファイル作成・ＮＧ画像→データ画像へ </summary>
        /// <param name="fCnt">
        ///     リカバリファイル番号</param>
        /// <param name="ind">
        ///     リストボックスインデックス</param>
        ///--------------------------------------------------------------------------
        private void NgToData(string _ID, int fCnt, int ind)
        {
            // イメージ表示初期化
            pictureBox1.Image = null;
            pictureBox1.Dispose();

            System.Threading.Thread.Sleep(100);

            // IDを取得します
            _ID += fCnt.ToString().PadLeft(3, '0');

            // 出力ファイルインスタンス作成
            StreamWriter outFile = new StreamWriter(_OutPath + _ID + ".csv", false, System.Text.Encoding.GetEncoding(932));

            StringBuilder sb = new StringBuilder();

            try
            {
                sb.Clear();

                // ヘッダ情報
                sb.Append("*").Append(",");
                sb.Append(_ID + ".tif").Append(",");    // 画像ファイル名
                sb.Append(string.Empty).Append(",");    // 年
                sb.Append(string.Empty).Append(",");    // 月
                sb.Append(string.Empty).Append(",");    // 発注書ID
                sb.Append(string.Empty).Append(",");    // 得意先コード
                sb.Append(string.Empty).Append(",");    // 発注書ID連番
                sb.Append(",,,,,,").Append(",");        // 店着日
                sb.Append(Environment.NewLine);         // 派遣先コード

                // 商品明細
                for (int i = 0; i < 15; i++)
                {
                    sb.Append(",,,,,,").Append(Environment.NewLine);
                }

                // ＣＳＶファイル作成
                outFile.WriteLine(sb.ToString());

                // 画像ファイル移動
                File.Copy(ngf[ind].ngFileName, _OutPath + _ID + ".tif");
                File.Delete(ngf[ind].ngFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ＮＧ画像リカバリ処理", MessageBoxButtons.OK);
            }
            finally
            {
                outFile.Close();
            }
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     チェックボックス選択数取得 </summary>
        /// <returns>
        ///     選択アイテム数</returns>
        ///-------------------------------------------------------------
        private int ngFileCount()
        {
            return checkedListBox1.CheckedItems.Count;
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     ＮＧ画像削除処理 </summary>
        ///-------------------------------------------------------------
        private void NgFileDelete()
        {
            // ＮＧファイル削除
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    imgDelete(ngf[i].ngFileName);
                }
            }

            // ＮＧ画像リスト再表示
            GetNgList();

            // イメージ表示初期化
            pictureBox1.Image = null;
            trackBar1.Enabled = false;
            btnLeft.Enabled = false;
        }

        ///-------------------------------------------------------------
        /// <summary>
        ///     ファイル削除 </summary>
        /// <param name="imgPath">
        ///     画像ファイルパス</param>
        ///-------------------------------------------------------------
        private void imgDelete(string imgPath)
        {
            // ファイルを削除する
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        ///---------------------------------------------------
        /// <summary>
        ///     ＮＧ画像印刷 </summary>
        ///---------------------------------------------------
        private void NgImagePrint()
        {
            PrintDialog pd = new PrintDialog();
            pd.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

            // ＮＧ画像印刷
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    _img = ngf[i].ngFileName;

                    // デフォルトプリンタ設定
                    printDocument1.PrinterSettings.PrinterName = pd.PrinterSettings.PrinterName;

                    // 印刷実行
                    printDocument1.Print();
                }
            }

            // 後片付け：2017/11/18
            printDocument1.Dispose();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image img;

            img = Image.FromFile(_img);
            //e.Graphics.DrawImage(img, 0, 0);

            // 2017/12/12 縮小
            //e.Graphics.DrawImage(img, 0, 0, img.Width * 49 / 100, img.Height * 49 / 100);

            // 2018/06/21 元画像のピクセル調整を行わないことによる縮小調整
            e.Graphics.DrawImage(img, 0, 0, img.Width * 47 / 100, img.Height * 47 / 100);
            e.HasMorePages = false;

            // 後片付け 2017/11/18
            img.Dispose();
        }

        private void btnPrn_Click(object sender, EventArgs e)
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show(ngFileCount().ToString() + "件の画像を印刷します。よろしいですか？", "印刷確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            // ＮＧ画像印刷
            NgImagePrint();

            // 印刷が終了しました
            MessageBox.Show("終了しました", "NG画像印刷");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show(ngFileCount().ToString() + "件の画像を削除します。よろしいですか？", "削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            // ＮＧ画像削除処理
            NgFileDelete();

            // 削除が完了しました
            MessageBox.Show("削除が完了しました");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            // ＮＧファイルリカバリ
            NgRecovery();
        }

        private void linkLblOn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("全てのNG画像をチェックします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void linkLblOff_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show("全てのNG画像をチェックオフします。よろしいですか。", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            if (ngFileCount() == 0)
            {
                MessageBox.Show("画像が選択されていません", "画像未選択", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (MessageBox.Show(ngFileCount().ToString() + "件の画像を印刷した後、削除します。よろしいですか？", "一括印刷・削除確認", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            // ＮＧ画像印刷
            NgImagePrint();

            // ＮＧ画像削除処理
            NgFileDelete();

            // 処理が完了しました
            MessageBox.Show("印刷・削除が完了しました");
        }

        private void printDocument1_EndPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            MessageBox.Show(printDocument1.DocumentName +
               " has finished printing.");
        }



        ///-----------------------------------------------------------
        /// <summary>
        ///     画像表示 openCV：2018/10/24 </summary>
        /// <param name="img">
        ///     表示画像ファイル名</param>
        ///-----------------------------------------------------------
        private void showImage_openCv(string img)
        {
            n_width = B_WIDTH;
            n_height = B_HEIGHT;

            imgShow(img, n_width, n_height);

            trackBar1.Value = 0;
        }


        // GUI上に画像を表示するには、OpenCV上で扱うMat形式をBitmap形式に変換する必要がある
        private Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }


        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン openCV : 2018/10/24 </summary>
        /// <param name="mImg">
        ///     Mat形式イメージ</param>
        /// <param name="w">
        ///     width</param>
        /// <param name="h">
        ///     height</param>
        ///---------------------------------------------------------
        private void imgShow(Mat mImg, float w, float h)
        {
            int cWidth = 0;
            int cHeight = 0;

            Bitmap bt = MatToBitmap(mImg);

            // Bitmapサイズ
            if (panel1.Width < (bt.Width * w) || panel1.Height < (bt.Height * h))
            {
                cWidth = (int)(bt.Width * w);
                cHeight = (int)(bt.Height * h);
            }
            else
            {
                cWidth = panel1.Width;
                cHeight = panel1.Height;
            }

            // Bitmap を生成
            Bitmap canvas = new Bitmap(cWidth, cHeight);

            // ImageオブジェクトのGraphicsオブジェクトを作成する
            Graphics g = Graphics.FromImage(canvas);

            // 画像をcanvasの座標(0, 0)の位置に指定のサイズで描画する
            g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

            //メモリクリア
            bt.Dispose();
            g.Dispose();

            // PictureBox1に表示する
            pictureBox1.Image = canvas;
        }



        ///---------------------------------------------------------
        /// <summary>
        ///     画像表示メイン openCV : 2018/10/24 </summary>
        /// <param name="mImg">
        ///     Mat形式イメージ</param>
        /// <param name="w">
        ///     width</param>
        /// <param name="h">
        ///     height</param>
        ///---------------------------------------------------------
        private void imgShow(string filePath, float w, float h)
        {
            try
            {
                //メモリクリア
                mMat.Dispose();

                //mMat = new Mat(filePath, ImreadModes.Grayscale);
                mMat = new Mat(@filePath);

                Bitmap bt = MatToBitmap(mMat);

                // Bitmap を生成
                Bitmap canvas = new Bitmap((int)(bt.Width * w), (int)(bt.Height * h));

                Graphics g = Graphics.FromImage(canvas);

                g.DrawImage(bt, 0, 0, bt.Width * w, bt.Height * h);

                //メモリクリア
                bt.Dispose();
                g.Dispose();

                pictureBox1.Image = canvas;
            }
            catch (Exception ex)
            {
                pictureBox1.Image = null;
                MessageBox.Show(ex.Message);
            }
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            n_width = B_WIDTH + (float)trackBar1.Value * 0.05f;
            n_height = B_HEIGHT + (float)trackBar1.Value * 0.05f;

            imgShow(mMat, n_width, n_height);
        }

        private void CheckedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            ImageRotate(pictureBox1.Image);
        }

        private void ImageRotate(Image img)
        {
            Bitmap bmp = (Bitmap)img;

            // 反転せず時計回りに90度回転する
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

            //表示
            pictureBox1.Image = img;
        }
    }
}
