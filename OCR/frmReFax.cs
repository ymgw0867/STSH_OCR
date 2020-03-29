using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using OpenCvSharp;
using Excel = Microsoft.Office.Interop.Excel;

namespace STSH_OCR.OCR
{
    public partial class frmReFax : Form
    {
        public frmReFax(string ImgFile, string TokuisakiNM, string TokuisakiFax)
        {
            InitializeComponent();

            _Img = ImgFile;
            _UserNM = TokuisakiNM;
            _UserFAX = TokuisakiFax;
        }

        string _Img = string.Empty;
        string _UserNM = string.Empty;
        string _UserFAX = string.Empty;

        // openCvSharp 関連　2019/08/19
        const float B_WIDTH = 0.35f;
        const float B_HEIGHT = 0.35f;
        const float A_WIDTH = 0.05f;
        const float A_HEIGHT = 0.05f;

        float n_width = 0f;
        float n_height = 0f;

        Mat mMat = new Mat();

        private void button1_Click(object sender, EventArgs e)
        {
            ImageToExcel(Properties.Settings.Default.ReFAXTempXlsx, _Img);
        }

        private void ImageToExcel(string xPath, string ImgPath)
        {
            Cursor = Cursors.WaitCursor;

            string fileName = "";

            try
            {
                IXLWorkbook bk;
                using (bk = new XLWorkbook(xPath, XLEventTracking.Disabled))
                {
                    var sheet1 = bk.Worksheet(1);

                    var image = sheet1.AddPicture(ImgPath);
                    image.MoveTo(sheet1.Cell(5, 5));

                    image.Height = 580;
                    image.Width = 829;

                    DateTime dt = DateTime.Now;

                    sheet1.Cell(1, 29).Value = lblName.Text + "　様";
                    sheet1.Cell(2, 2).Value = textBox1.Text;
                    sheet1.Cell(29, 2).Value = dt.ToShortDateString() + " " + dt.Hour + ":" + dt.Minute + ":" + dt.Second;

                    ////ダイアログボックスの初期設定
                    //SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                    //saveFileDialog1.Title = "返信FAX";
                    //saveFileDialog1.OverwritePrompt = true;
                    //saveFileDialog1.RestoreDirectory = true;
                    //saveFileDialog1.FileName = "返信FAX_" + dt.Year + dt.Month + dt.Day;
                    //saveFileDialog1.Filter = "Microsoft Office Excelファイル(*.xlsx)|*.xlsx|全てのファイル(*.*)|*.*";

                    ////ダイアログボックスを表示し「保存」ボタンが選択されたらファイル名を表示
                    //string fileName;
                    //var ret = saveFileDialog1.ShowDialog();

                    //if (ret == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    // エクセル保存
                    //    fileName = saveFileDialog1.FileName;
                    //    bk.SaveAs(fileName);

                    //    // メッセージ
                    //    MessageBox.Show("返信FAXを出力しました", "確認", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                    // エクセル保存
                    fileName = Properties.Settings.Default.ReFaxSaveFile;
                    bk.SaveAs(fileName);
                }

                Cursor = Cursors.Default;

                sReport(fileName, comboBox1.SelectedItem.ToString());
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        ///----------------------------------------------------------------------
        /// <summary>
        ///     返信ＦＡＸ原稿作成 </summary>
        /// <param name="xlsPath">
        ///     エクセルシートパス</param>
        /// <param name="sPrinter">
        ///     印刷プリンタ</param>
        ///----------------------------------------------------------------------
        public static void sReport(string xlsPath, string sPrinter)
        {
            try
            {
                Excel.Application oXls = new Excel.Application();
                Excel.Workbook oXlsBook = (Excel.Workbook)(oXls.Workbooks.Open(xlsPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                   Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                   Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                   Type.Missing, Type.Missing));

                // シート
                Excel.Worksheet oxlsSheet = (Excel.Worksheet)oXlsBook.Sheets[1];

                Excel.Range[] rng = new Microsoft.Office.Interop.Excel.Range[2];

                try
                {
                    // 確認のためExcelのウィンドウを表示する
                    //oXls.Visible = true;

                    //印刷
                    oXlsBook.PrintOut(1,Type.Missing, 1, false, sPrinter, Type.Missing, Type.Missing, Type.Missing);

                    //// ウィンドウを非表示にする
                    //oXls.Visible = false;

                    //保存処理
                    //oXls.DisplayAlerts = false;

                    // 終了メッセージ
                    MessageBox.Show("発行が終了しました", "返信ＦＡＸ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "返信ＦＡＸ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                finally
                {
                    // ウィンドウを非表示にする
                    oXls.Visible = false;

                    // 保存処理
                    oXls.DisplayAlerts = false;

                    // Bookをクローズ
                    oXlsBook.Close(Type.Missing, Type.Missing, Type.Missing);

                    // Excelを終了
                    oXls.Quit();

                    // COMオブジェクトの参照カウントを解放する 
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oxlsSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oXlsBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oXls);

                    // カーソルをデフォルトへ戻す
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "返信ＦＡＸ発行処理", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
     

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            Dispose();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblName.Text = _UserNM;
            txtFaxNum.Text = _UserFAX;

            // 画像表示
            if (System.IO.File.Exists(_Img))
            {
                showImage_openCv(_Img);
            }

            //comboBox1.DataSource = System.Drawing.Printing.PrinterSettings.InstalledPrinters;

            foreach (var item in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox1.Items.Add(item);
            }
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
        }


        // GUI上に画像を表示するには、OpenCV上で扱うMat形式をBitmap形式に変換する必要がある
        public static Bitmap MatToBitmap(Mat image)
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
        private void imgShow(string filePath, float w, float h)
        {
            mMat = new Mat(filePath, ImreadModes.Grayscale);
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

        private void button3_Click(object sender, EventArgs e)
        {
            Master.frmComment comment = new Master.frmComment();
            comment.ShowDialog();

            string _Comment = comment.MyComment;
            comment.Dispose();

            if (_Comment != string.Empty)
            {
                textBox1.Text = _Comment;
            }
        }
    }
}
