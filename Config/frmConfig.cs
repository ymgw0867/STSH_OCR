using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using STSH_OCR.Common;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Data.Linq;

namespace STSH_OCR.Config
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }
        
        SQLiteConnection cn = null;
        DataContext context = null;
        string db_file = Properties.Settings.Default.DB_File;
        Table<Common.ClsSystemConfig> conf = null;
        ClsSystemConfig ClsConf = null;


        private void frmConfig_Load(object sender, EventArgs e)
        {
            Utility.WindowsMaxSize(this, this.Width, this.Height);
            Utility.WindowsMinSize(this, this.Width, this.Height);

            cn = new SQLiteConnection("DataSource=" + db_file);
            context = new DataContext(cn);
            conf = context.GetTable<Common.ClsSystemConfig>();

            // 環境設定データを表示
            ClsConf = conf.Single(a => a.ID == global.configKEY);

            txtPath2.Text = ClsConf.DataPath;
            txtImgPath.Text = ClsConf.ImgPath;
            txtDataSpan.Text = ClsConf.DataSpan.ToString();
            txtLogSpan.Text = ClsConf.LogSpan.ToString();
            comboBox1.SelectedIndex = ClsConf.FileWriteStatus;
            txtNouhinTankaMst.Text = ClsConf.NouhinTankaPath;
        }

        ///------------------------------------------------------------------------
        /// <summary>
        ///     フォルダダイアログ選択 </summary>
        /// <returns>
        ///     フォルダー名</returns>
        ///------------------------------------------------------------------------
        private string userFolderSelect()
        {
            string fName = string.Empty;

            //出力フォルダの選択ダイアログの表示
            // FolderBrowserDialog の新しいインスタンスを生成する (デザイナから追加している場合は必要ない)
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();

            // ダイアログの説明を設定する
            folderBrowserDialog1.Description = "フォルダを選択してください";

            // ルートになる特殊フォルダを設定する (初期値 SpecialFolder.Desktop)
            folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.Desktop;

            // 初期選択するパスを設定する
            folderBrowserDialog1.SelectedPath = @"C:\STSH_OCR";

            // [新しいフォルダ] ボタンを表示する (初期値 true)
            folderBrowserDialog1.ShowNewFolderButton = true;

            // ダイアログを表示し、戻り値が [OK] の場合は、選択したディレクトリを表示する
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                fName = folderBrowserDialog1.SelectedPath + @"\";
            }
            else
            {
                // 不要になった時点で破棄する
                folderBrowserDialog1.Dispose();
                return fName;
            }

            // 不要になった時点で破棄する
            folderBrowserDialog1.Dispose();

            return fName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // データ更新
            DataUpdate();
        }

        private void DataUpdate()
        {
            // エラーチェック
            if (!errCheck())
            {
                return;
            }

            if (MessageBox.Show("データを更新してよろしいですか", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            
            // データ更新
            ClsConf.DataPath = txtPath2.Text;
            ClsConf.ImgPath = txtImgPath.Text;
            ClsConf.DataSpan = Utility.StrtoInt(txtDataSpan.Text);
            ClsConf.LogSpan = Utility.StrtoInt(txtLogSpan.Text);
            ClsConf.FileWriteStatus = comboBox1.SelectedIndex;
            ClsConf.YyMmDd = DateTime.Now.ToString();
            ClsConf.NouhinTankaPath = txtNouhinTankaMst.Text;

            context.SubmitChanges();

            // 終了
            this.Close();
        }

        /// ------------------------------------------------------------------------------------
        /// <summary>
        ///     エラーチェック </summary>
        /// <returns>
        ///     エラーなし：true, エラーあり：false</returns>
        /// ------------------------------------------------------------------------------------
        private bool errCheck()
        {
            // 画像保存先パス
            if (txtImgPath.Text.Trim() == string.Empty)
            {
                MessageBox.Show("画像保存先パスを入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtImgPath.Focus();
                return false;
            }

            if (!System.IO.Directory.Exists(txtImgPath.Text))
            {
                MessageBox.Show("指定した画像保存先フォルダは存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtImgPath.Focus();
                return false;
            }
            
            // CSVデータ出力先パス
            if (txtPath2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("CSVデータ出力先フォルダを入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPath2.Focus();
                return false;
            }

            if (!System.IO.Directory.Exists(txtPath2.Text))
            {
                MessageBox.Show("指定したCSVデータ出力先フォルダは存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPath2.Focus();
                return false;
            }

            // データ保存月数
            if (txtDataSpan.Text.Trim() == string.Empty)
            {
                MessageBox.Show("データ保存月数を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDataSpan.Focus();
                return false;
            }

            // ログ保存月数
            if (txtLogSpan.Text.Trim() == string.Empty)
            {
                MessageBox.Show("ログデータ保存月数を入力してください", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLogSpan.Focus();
                return false;
            }

            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 後片付け
            this.Dispose();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
                return;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //フォルダーを選択する
            string sPath = userFolderSelect();
            if (sPath != string.Empty)
            {
                txtPath2.Text = sPath;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //フォルダーを選択する
            string sPath = userFolderSelect();
            if (sPath != string.Empty)
            {
                txtImgPath.Text = sPath;
            }
        }

        private void txtLogSpan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '\t')
            {
                e.Handled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fn = OpenFilDialog("納品単価マスターCSVファイル");
            if (fn != string.Empty)
            {
                txtNouhinTankaMst.Text = fn;
            }
        }

        ///----------------------------------------------------------
        /// <summary>
        ///     CSVファイル選択ダイアログ </summary>
        /// <param name="title">
        ///     ダイアログ画面タイトル</param>
        /// <returns>
        ///     ファイル名</returns>
        ///----------------------------------------------------------
        private string OpenFilDialog(string title)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = title;
            openFileDialog1.FileName = string.Empty;
            openFileDialog1.Filter = "CSVファイル(*.csv)|*.csv|全てのファイル(*.*)|*.*";

            //ダイアログボックスを表示し「保存」ボタンが選択されたらファイル名を表示
            string fileName;
            DialogResult ret = openFileDialog1.ShowDialog();

            if (ret == System.Windows.Forms.DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
            }
            else
            {
                fileName = string.Empty;
            }

            // 不要になった時点で破棄する
            openFileDialog1.Dispose();

            return fileName;
        }
    }
}
