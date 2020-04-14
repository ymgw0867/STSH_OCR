namespace STSH_OCR.OCR
{
    partial class frmOrder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOrder));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnLeft = new System.Windows.Forms.Button();
            this.lblNoImage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnErrCheck = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.txtErrStatus = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtYear = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMonth = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPID = new System.Windows.Forms.TextBox();
            this.txtSeqNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtTokuisakiCD = new System.Windows.Forms.TextBox();
            this.txtTenDay7 = new System.Windows.Forms.TextBox();
            this.label89 = new System.Windows.Forms.Label();
            this.txtTenDay1 = new System.Windows.Forms.TextBox();
            this.txtTenDay2 = new System.Windows.Forms.TextBox();
            this.txtTenDay3 = new System.Windows.Forms.TextBox();
            this.txtTenDay4 = new System.Windows.Forms.TextBox();
            this.txtTenDay5 = new System.Windows.Forms.TextBox();
            this.txtTenDay6 = new System.Windows.Forms.TextBox();
            this.lblTokuisakiName = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dg1 = new STSH_OCR.DataGridViewEx();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblPages = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.LemonChiffon;
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.SystemColors.Control;
            this.btnLeft.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(488, 830);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(35, 35);
            this.btnLeft.TabIndex = 361;
            this.btnLeft.TabStop = false;
            this.toolTip1.SetToolTip(this.btnLeft, "画像を左回転");
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click_1);
            // 
            // lblNoImage
            // 
            this.lblNoImage.Font = new System.Drawing.Font("メイリオ", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNoImage.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblNoImage.Location = new System.Drawing.Point(331, 354);
            this.lblNoImage.Name = "lblNoImage";
            this.lblNoImage.Size = new System.Drawing.Size(322, 42);
            this.lblNoImage.TabIndex = 119;
            this.lblNoImage.Text = "画像はありません";
            this.lblNoImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblErrMsg);
            this.panel1.Location = new System.Drawing.Point(1063, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(828, 44);
            this.panel1.TabIndex = 162;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrMsg.Font = new System.Drawing.Font("ＭＳ ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(0, 0);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(824, 40);
            this.lblErrMsg.TabIndex = 0;
            this.lblErrMsg.Text = "label33";
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox1.Location = new System.Drawing.Point(1801, 792);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(88, 23);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "確認済";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtMemo
            // 
            this.txtMemo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMemo.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(72, 776);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(985, 44);
            this.txtMemo.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Lavender;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(12, 776);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 44);
            this.label2.TabIndex = 307;
            this.label2.Text = "メモ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnErrCheck
            // 
            this.btnErrCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnErrCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnErrCheck.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnErrCheck.Location = new System.Drawing.Point(1651, 850);
            this.btnErrCheck.Name = "btnErrCheck";
            this.btnErrCheck.Size = new System.Drawing.Size(144, 47);
            this.btnErrCheck.TabIndex = 13;
            this.btnErrCheck.Text = "エラーチェック：F10";
            this.btnErrCheck.UseVisualStyleBackColor = false;
            this.btnErrCheck.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDelete.Location = new System.Drawing.Point(1357, 850);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(144, 47);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "発注書削除：F8";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPrint.Location = new System.Drawing.Point(1504, 850);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(144, 47);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "画像印刷：F9";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.button9_Click);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // txtErrStatus
            // 
            this.txtErrStatus.Location = new System.Drawing.Point(1064, 852);
            this.txtErrStatus.Name = "txtErrStatus";
            this.txtErrStatus.ReadOnly = true;
            this.txtErrStatus.Size = new System.Drawing.Size(23, 19);
            this.txtErrStatus.TabIndex = 358;
            this.txtErrStatus.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblNoImage);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1045, 760);
            this.panel2.TabIndex = 363;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1036, 749);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txtYear
            // 
            this.txtYear.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtYear.Location = new System.Drawing.Point(1063, 59);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(83, 34);
            this.txtYear.TabIndex = 365;
            this.txtYear.Text = "2020";
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtYear.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtYear.Enter += new System.EventHandler(this.txtYear_Enter);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Lavender;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(1145, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 34);
            this.label4.TabIndex = 366;
            this.label4.Text = "年";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Lavender;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(1232, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 34);
            this.label5.TabIndex = 367;
            this.label5.Text = "月";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMonth
            // 
            this.txtMonth.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMonth.Location = new System.Drawing.Point(1179, 59);
            this.txtMonth.MaxLength = 2;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(54, 34);
            this.txtMonth.TabIndex = 368;
            this.txtMonth.Text = "10";
            this.txtMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMonth.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtMonth.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtMonth.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Lavender;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(1266, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(137, 34);
            this.label7.TabIndex = 369;
            this.label7.Text = "発注書番号";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPID
            // 
            this.txtPID.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPID.Location = new System.Drawing.Point(1402, 59);
            this.txtPID.MaxLength = 3;
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(60, 34);
            this.txtPID.TabIndex = 370;
            this.txtPID.Text = "100";
            this.txtPID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPID.TextChanged += new System.EventHandler(this.txtPID_TextChanged);
            this.txtPID.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtPID.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtSeqNum
            // 
            this.txtSeqNum.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSeqNum.Location = new System.Drawing.Point(1461, 59);
            this.txtSeqNum.MaxLength = 2;
            this.txtSeqNum.Name = "txtSeqNum";
            this.txtSeqNum.Size = new System.Drawing.Size(45, 34);
            this.txtSeqNum.TabIndex = 371;
            this.txtSeqNum.Text = "10";
            this.txtSeqNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeqNum.TextChanged += new System.EventHandler(this.txtSeqNum_TextChanged);
            this.txtSeqNum.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtSeqNum.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Lavender;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(1063, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(117, 32);
            this.label8.TabIndex = 372;
            this.label8.Text = "得意先名";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Lavender;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(1505, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(171, 34);
            this.label9.TabIndex = 373;
            this.label9.Text = "得意先コード";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label9.DoubleClick += new System.EventHandler(this.label9_DoubleClick);
            // 
            // txtTokuisakiCD
            // 
            this.txtTokuisakiCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTokuisakiCD.Location = new System.Drawing.Point(1675, 59);
            this.txtTokuisakiCD.MaxLength = 7;
            this.txtTokuisakiCD.Name = "txtTokuisakiCD";
            this.txtTokuisakiCD.Size = new System.Drawing.Size(121, 34);
            this.txtTokuisakiCD.TabIndex = 374;
            this.txtTokuisakiCD.Text = "1001001";
            this.txtTokuisakiCD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTokuisakiCD.TextChanged += new System.EventHandler(this.txtTokuisakiCD_TextChanged);
            this.txtTokuisakiCD.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTokuisakiCD.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay7
            // 
            this.txtTenDay7.BackColor = System.Drawing.Color.White;
            this.txtTenDay7.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay7.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay7.Location = new System.Drawing.Point(1795, 123);
            this.txtTenDay7.MaxLength = 2;
            this.txtTenDay7.Name = "txtTenDay7";
            this.txtTenDay7.Size = new System.Drawing.Size(40, 31);
            this.txtTenDay7.TabIndex = 579;
            this.txtTenDay7.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay7.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay7.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay7.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // label89
            // 
            this.label89.BackColor = System.Drawing.Color.Lavender;
            this.label89.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label89.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label89.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label89.Location = new System.Drawing.Point(1505, 123);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(51, 31);
            this.label89.TabIndex = 586;
            this.label89.Text = "店着日";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTenDay1
            // 
            this.txtTenDay1.BackColor = System.Drawing.Color.White;
            this.txtTenDay1.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay1.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay1.Location = new System.Drawing.Point(1555, 123);
            this.txtTenDay1.MaxLength = 2;
            this.txtTenDay1.Name = "txtTenDay1";
            this.txtTenDay1.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay1.TabIndex = 573;
            this.txtTenDay1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay1.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay1.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay1.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay2
            // 
            this.txtTenDay2.BackColor = System.Drawing.Color.White;
            this.txtTenDay2.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay2.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay2.Location = new System.Drawing.Point(1595, 123);
            this.txtTenDay2.MaxLength = 2;
            this.txtTenDay2.Name = "txtTenDay2";
            this.txtTenDay2.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay2.TabIndex = 574;
            this.txtTenDay2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay2.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay2.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay2.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay3
            // 
            this.txtTenDay3.BackColor = System.Drawing.Color.White;
            this.txtTenDay3.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay3.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay3.Location = new System.Drawing.Point(1635, 123);
            this.txtTenDay3.MaxLength = 2;
            this.txtTenDay3.Name = "txtTenDay3";
            this.txtTenDay3.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay3.TabIndex = 575;
            this.txtTenDay3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay3.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay3.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay3.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay4
            // 
            this.txtTenDay4.BackColor = System.Drawing.Color.White;
            this.txtTenDay4.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay4.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay4.Location = new System.Drawing.Point(1675, 123);
            this.txtTenDay4.MaxLength = 2;
            this.txtTenDay4.Name = "txtTenDay4";
            this.txtTenDay4.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay4.TabIndex = 576;
            this.txtTenDay4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay4.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay4.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay4.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay5
            // 
            this.txtTenDay5.BackColor = System.Drawing.Color.White;
            this.txtTenDay5.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay5.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay5.Location = new System.Drawing.Point(1715, 123);
            this.txtTenDay5.MaxLength = 2;
            this.txtTenDay5.Name = "txtTenDay5";
            this.txtTenDay5.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay5.TabIndex = 577;
            this.txtTenDay5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay5.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay5.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay5.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // txtTenDay6
            // 
            this.txtTenDay6.BackColor = System.Drawing.Color.White;
            this.txtTenDay6.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTenDay6.ForeColor = System.Drawing.SystemColors.MenuText;
            this.txtTenDay6.Location = new System.Drawing.Point(1755, 123);
            this.txtTenDay6.MaxLength = 2;
            this.txtTenDay6.Name = "txtTenDay6";
            this.txtTenDay6.Size = new System.Drawing.Size(41, 31);
            this.txtTenDay6.TabIndex = 578;
            this.txtTenDay6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTenDay6.TextChanged += new System.EventHandler(this.txtTenDay1_TextChanged);
            this.txtTenDay6.Enter += new System.EventHandler(this.txtYear_Enter);
            this.txtTenDay6.Leave += new System.EventHandler(this.txtYear_Leave);
            // 
            // lblTokuisakiName
            // 
            this.lblTokuisakiName.BackColor = System.Drawing.Color.White;
            this.lblTokuisakiName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTokuisakiName.Font = new System.Drawing.Font("ＭＳ ゴシック", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTokuisakiName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTokuisakiName.Location = new System.Drawing.Point(1179, 92);
            this.lblTokuisakiName.Name = "lblTokuisakiName";
            this.lblTokuisakiName.Size = new System.Drawing.Size(711, 32);
            this.lblTokuisakiName.TabIndex = 588;
            this.lblTokuisakiName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(175, 830);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(297, 45);
            this.trackBar1.TabIndex = 590;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(1798, 850);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(91, 47);
            this.button1.TabIndex = 591;
            this.button1.Text = "終了(&E)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button5_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.dg1);
            this.panel3.Location = new System.Drawing.Point(1063, 153);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(827, 619);
            this.panel3.TabIndex = 592;
            // 
            // dg1
            // 
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Location = new System.Drawing.Point(0, 0);
            this.dg1.Name = "dg1";
            this.dg1.RowTemplate.Height = 21;
            this.dg1.Size = new System.Drawing.Size(825, 618);
            this.dg1.TabIndex = 364;
            this.dg1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg1_CellContentDoubleClick);
            this.dg1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dg1.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGV_CellLeave);
            this.dg1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewEx1_CellPainting);
            this.dg1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg1_CellValueChanged);
            this.dg1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dg1_CurrentCellDirtyStateChanged);
            this.dg1.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dg1_EditingControlShowing);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Lavender;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(1063, 123);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(443, 31);
            this.label1.TabIndex = 593;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("ＭＳ ゴシック", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLabel1.Location = new System.Drawing.Point(12, 830);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(135, 15);
            this.linkLabel1.TabIndex = 595;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "返信ファクス送信";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lblPages
            // 
            this.lblPages.BackColor = System.Drawing.Color.Lavender;
            this.lblPages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPages.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPages.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblPages.Location = new System.Drawing.Point(1834, 123);
            this.lblPages.Name = "lblPages";
            this.lblPages.Size = new System.Drawing.Size(56, 31);
            this.lblPages.TabIndex = 596;
            this.lblPages.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 889);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 17);
            this.label3.TabIndex = 597;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPage
            // 
            this.lblPage.BackColor = System.Drawing.SystemColors.Control;
            this.lblPage.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPage.ForeColor = System.Drawing.SystemColors.MenuText;
            this.lblPage.Location = new System.Drawing.Point(1802, 59);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(89, 33);
            this.lblPage.TabIndex = 598;
            this.lblPage.Text = "100/100";
            this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWarning
            // 
            this.lblWarning.BackColor = System.Drawing.Color.Red;
            this.lblWarning.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblWarning.ForeColor = System.Drawing.Color.White;
            this.lblWarning.Location = new System.Drawing.Point(1063, 824);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(826, 22);
            this.lblWarning.TabIndex = 600;
            this.lblWarning.Text = "同じ発注書が受信されています";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(1063, 776);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(728, 44);
            this.label6.TabIndex = 601;
            this.label6.Text = "注文済商品 ①発注数同じ：グレー、ロック済・発注書データ対象外、②発注数違い：赤、編集可・発注書データ作成";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 906);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblPages);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.lblTokuisakiName);
            this.Controls.Add(this.label89);
            this.Controls.Add(this.txtTenDay7);
            this.Controls.Add(this.txtTenDay6);
            this.Controls.Add(this.txtTenDay5);
            this.Controls.Add(this.txtTenDay4);
            this.Controls.Add(this.txtTenDay3);
            this.Controls.Add(this.txtTenDay2);
            this.Controls.Add(this.txtTenDay1);
            this.Controls.Add(this.txtTokuisakiCD);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSeqNum);
            this.Controls.Add(this.txtPID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtMonth);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtYear);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.txtErrStatus);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnErrCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmOrder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FAX発注書データ作成";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCorrect_FormClosing);
            this.Load += new System.EventHandler(this.frmCorrect_Load);
            this.Shown += new System.EventHandler(this.frmCorrect_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCorrect_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblNoImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblErrMsg;
        //private Template1 template11;
        //private Template2 template21;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnErrCheck;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TextBox txtErrStatus;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private DataGridViewEx dg1;
        private System.Windows.Forms.TextBox txtYear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMonth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPID;
        private System.Windows.Forms.TextBox txtSeqNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtTokuisakiCD;
        private System.Windows.Forms.TextBox txtTenDay7;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.TextBox txtTenDay1;
        private System.Windows.Forms.TextBox txtTenDay2;
        private System.Windows.Forms.TextBox txtTenDay3;
        private System.Windows.Forms.TextBox txtTenDay4;
        private System.Windows.Forms.TextBox txtTenDay5;
        private System.Windows.Forms.TextBox txtTenDay6;
        private System.Windows.Forms.Label lblTokuisakiName;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblPages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label label6;
    }
}