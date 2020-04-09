using OpenCvSharp;
using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Data.Linq;
using System.Data.SQLite;
using System.Linq;
using STSH_OCR.Common;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace STSH_OCR.OCR
{
    public partial class frmOrderIndex : Form
    {
        public frmOrderIndex()
        {
            InitializeComponent();
        }
        
        // データベース：Sqlite3
        SQLiteConnection cn = null;
        DataContext context = null;

        string db_file = Properties.Settings.Default.DB_File;

        // 発注書データ
        Table<Common.ClsOrder> tblOrder = null;
        ClsOrder Order = null;

        // 環境設定
        Table<Common.ClsSystemConfig> tblConfig = null;
        ClsSystemConfig Config = null;

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
        
        // 2020/04/08 コメント化
        //ClsCsvData.ClsCsvTokuisaki[] tokuisakis = null;

        bool dgRows = false;

        private void frmNgRecovery_Load(object sender, EventArgs e)
        {
            MaximumSize = new System.Drawing.Size(Width, Height);
            MinimumSize = new System.Drawing.Size(Width, Height);

            // データベース接続
            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);

            // 発注書データ読み込み
            tblOrder = context.GetTable<Common.ClsOrder>();

            // 環境設定
            tblConfig = context.GetTable<Common.ClsSystemConfig>();
            Config = (ClsSystemConfig)tblConfig.Single(a => a.ID == global.configKEY);

            // 2020/04/08 コメント化
            //// 得意先マスタークラス配列取得
            //string[] tk_Array = System.IO.File.ReadAllLines(Properties.Settings.Default.得意先マスター, Encoding.Default);
            //int sDate = DateTime.Today.Year * 10000 + DateTime.Today.Month * 100 + DateTime.Today.Day;
            //tokuisakis = ClsCsvData.ClsCsvTokuisaki.Load(tk_Array, sDate);

            // グリッドビュー設定
            GridViewSetting(dataGridView1);

            // 発注書リスト
            GetOrderList(dataGridView1);

            // ボタン
            trackBar1.Enabled = false;

            dgRows = true;
        }

        string colDate = "c1";
        string colTkNM = "c2";
        string colID = "c3";

        public void GridViewSetting(DataGridView tempDGV)
        {
            //フォームサイズ定義

            // 列スタイルを変更する

            tempDGV.EnableHeadersVisualStyles = false;
            tempDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            tempDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tempDGV.EnableHeadersVisualStyles = false;

            // 列ヘッダー表示位置指定
            tempDGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 列ヘッダーフォント指定
            tempDGV.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ ゴシック", 9, FontStyle.Regular);

            // データフォント指定
            tempDGV.DefaultCellStyle.Font = new Font("ＭＳ ゴシック", (float)10.5, FontStyle.Regular);

            // 行の高さ
            tempDGV.ColumnHeadersHeight = 20;
            tempDGV.RowTemplate.Height = 20;

            // 全体の高さ
            tempDGV.Height = 782;

            // 奇数行の色
            tempDGV.AlternatingRowsDefaultCellStyle.BackColor = Color.Lavender;

            // 各列幅指定
            tempDGV.Columns.Add(colDate, "受信日時");
            tempDGV.Columns.Add(colTkNM, "得意先名");
            tempDGV.Columns.Add(colID, "発注書ID");

            tempDGV.Columns[colDate].Width = 160;
            tempDGV.Columns[colTkNM].Width = 300;
            tempDGV.Columns[colID].Width = 100;
            tempDGV.Columns[colID].Visible = false;

            tempDGV.Columns[colTkNM].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            tempDGV.Columns[colDate].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 編集可否
            tempDGV.ReadOnly = true;

            // 行ヘッダを表示しない
            tempDGV.RowHeadersVisible = false;

            // 選択モード
            tempDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tempDGV.MultiSelect = false;

            // 追加行表示しない
            tempDGV.AllowUserToAddRows = false;

            // データグリッドビューから行削除を禁止する
            tempDGV.AllowUserToDeleteRows = false;

            // 手動による列移動の禁止
            tempDGV.AllowUserToOrderColumns = false;

            // 列サイズ変更禁止
            //tempDGV.AllowUserToResizeColumns = false;

            // 行サイズ変更禁止
            tempDGV.AllowUserToResizeRows = false;

            // 罫線
            tempDGV.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None;
            tempDGV.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }

        private void frmNgRecovery_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }


        /// ----------------------------------------------------------------------
        /// <summary>
        ///     グリッドビュー表示 </summary>
        /// <param name="tempDGV">
        ///     DataGridViewオブジェクト名</param>
        /// <param name="sCode">
        ///     指定所属コード</param>
        /// ----------------------------------------------------------------------
        private void GetOrderList(DataGridView g)
        {
            // カーソル待機中
            this.Cursor = Cursors.WaitCursor;

            // データグリッド行クリア
            g.Rows.Clear();

            try
            {
                foreach (var t in tblOrder.OrderByDescending(a => a.ID))
                {
                    string jDate = t.ID.Substring(0, 4) + "/" + t.ID.Substring(4, 2) + "/" + t.ID.Substring(6, 2) + " " + t.ID.Substring(8, 2) + ":" + t.ID.Substring(10, 2) + ":" + t.ID.Substring(12, 2);
                    string tkNM = null;

                    // 2020/04/08 コメント化
                    //for (int i = 0; i < tokuisakis.Length; i++)
                    //{
                    //    if (tokuisakis[i].TOKUISAKI_CD == t.TokuisakiCode.ToString("D7"))
                    //    {
                    //        tkNM = tokuisakis[i].TOKUISAKI_NM;
                    //        break;
                    //    }
                    //}

                    // 2020/04/09
                    tkNM = Utility.GetTokuisakiFromDataTable(t.TokuisakiCode.ToString("D7"), global.dtTokuisaki).TOKUISAKI_NM;

                    g.Rows.Add();

                    g[colDate, g.Rows.Count - 1].Value = jDate;
                    g[colTkNM, g.Rows.Count - 1].Value = tkNM;
                    g[colID, g.Rows.Count - 1].Value = t.ID;
                }

                g.CurrentCell = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "エラー", MessageBoxButtons.OK);
            }
            finally
            {
                // カーソルを戻す
                this.Cursor = Cursors.Default;
            }

            // 該当するデータがないとき
            if (g.RowCount == 0)
            {
                MessageBox.Show("発注データは現在、登録されていません", "発注データ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                button2.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private class clsNG
        {
            public string ngFileName { get; set; }
            public string ngRecDate { get; set; }
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
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
            //メモリクリア
            mMat.Dispose();

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

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            n_width = B_WIDTH + (float)trackBar1.Value * 0.05f;
            n_height = B_HEIGHT + (float)trackBar1.Value * 0.05f;

            imgShow(mMat, n_width, n_height);
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!dgRows)
            {
                return;
            }

            var s = tblOrder.Single(a => a.ID == dataGridView1[colID, dataGridView1.SelectedRows[0].Index].Value.ToString());
            string _img = Utility.GetImageFilePath(Config.ImgPath, s.TokuisakiCode.ToString("D7")) + @"\" + s.ImageFileName;

            if (System.IO.File.Exists(_img))
            {
                //画像イメージ表示
                showImage_openCv(_img);

                trackBar1.Enabled = true;
            }
            else
            {
                pictureBox1.Image = null;
                trackBar1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            frmOrder frm = new frmOrder(dataGridView1[colID, dataGridView1.SelectedRows[0].Index].Value.ToString());
            frm.ShowDialog();
            Show();

            // 発注書リスト再表示
            dgRows = false;
            GetOrderList(dataGridView1);
            dgRows = true;

            // 画像表示欄初期化
            pictureBox1.Image = null;
            trackBar1.Enabled = false;
        }
    }
}
