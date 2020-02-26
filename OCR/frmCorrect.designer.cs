﻿namespace STSH_OCR.OCR
{
    partial class frmCorrect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCorrect));
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.lblNoImage = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnErrCheck = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.btnBefore = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
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
            this.textBox142 = new System.Windows.Forms.TextBox();
            this.label89 = new System.Windows.Forms.Label();
            this.textBox135 = new System.Windows.Forms.TextBox();
            this.textBox136 = new System.Windows.Forms.TextBox();
            this.textBox138 = new System.Windows.Forms.TextBox();
            this.textBox137 = new System.Windows.Forms.TextBox();
            this.textBox140 = new System.Windows.Forms.TextBox();
            this.textBox139 = new System.Windows.Forms.TextBox();
            this.dg1 = new STSH_OCR.DataGridViewEx();
            this.btnData = new System.Windows.Forms.Button();
            this.lblTokuisakiName = new System.Windows.Forms.Label();
            this.lblPage = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).BeginInit();
            this.SuspendLayout();
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(12, 826);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(308, 24);
            this.hScrollBar1.TabIndex = 13;
            this.toolTip1.SetToolTip(this.hScrollBar1, "出勤簿を移動します");
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.LemonChiffon;
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.SystemColors.Control;
            this.btnRight.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRight.Image = ((System.Drawing.Image)(resources.GetObject("btnRight.Image")));
            this.btnRight.Location = new System.Drawing.Point(967, 824);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(35, 35);
            this.btnRight.TabIndex = 360;
            this.btnRight.TabStop = false;
            this.toolTip1.SetToolTip(this.btnRight, "画像を右回転");
            this.btnRight.UseVisualStyleBackColor = false;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.SystemColors.Control;
            this.btnLeft.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLeft.Image")));
            this.btnLeft.Location = new System.Drawing.Point(1002, 824);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(35, 35);
            this.btnLeft.TabIndex = 361;
            this.btnLeft.TabStop = false;
            this.toolTip1.SetToolTip(this.btnLeft, "画像を左回転");
            this.btnLeft.UseVisualStyleBackColor = false;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // lblNoImage
            // 
            this.lblNoImage.Font = new System.Drawing.Font("メイリオ", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNoImage.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblNoImage.Location = new System.Drawing.Point(123, 353);
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
            this.panel1.Location = new System.Drawing.Point(1053, 824);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(718, 44);
            this.panel1.TabIndex = 162;
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrMsg.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(0, 0);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Size = new System.Drawing.Size(714, 40);
            this.lblErrMsg.TabIndex = 0;
            this.lblErrMsg.Text = "label33";
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox1.Location = new System.Drawing.Point(1777, 836);
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
            this.txtMemo.Location = new System.Drawing.Point(74, 865);
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(963, 21);
            this.txtMemo.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Lavender;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(12, 865);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 21);
            this.label2.TabIndex = 307;
            this.label2.Text = "メモ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnErrCheck
            // 
            this.btnErrCheck.BackColor = System.Drawing.SystemColors.Control;
            this.btnErrCheck.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnErrCheck.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnErrCheck.Location = new System.Drawing.Point(1383, 896);
            this.btnErrCheck.Name = "btnErrCheck";
            this.btnErrCheck.Size = new System.Drawing.Size(144, 33);
            this.btnErrCheck.TabIndex = 13;
            this.btnErrCheck.Text = "エラーチェック：F10";
            this.btnErrCheck.UseVisualStyleBackColor = false;
            this.btnErrCheck.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDelete.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDelete.Location = new System.Drawing.Point(1129, 896);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 33);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "発注書削除：F8";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button5.Location = new System.Drawing.Point(1774, 896);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(91, 33);
            this.button5.TabIndex = 18;
            this.button5.Text = "終了（＆E)";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Location = new System.Drawing.Point(599, 826);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(67, 24);
            this.btnPlus.TabIndex = 7;
            this.btnPlus.Text = "画像拡大";
            this.btnPlus.UseVisualStyleBackColor = true;
            this.btnPlus.Click += new System.EventHandler(this.button6_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Location = new System.Drawing.Point(666, 826);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(67, 24);
            this.btnMinus.TabIndex = 8;
            this.btnMinus.Text = "画像縮小";
            this.btnMinus.UseVisualStyleBackColor = true;
            this.btnMinus.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // btnFirst
            // 
            this.btnFirst.Location = new System.Drawing.Point(331, 826);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(67, 24);
            this.btnFirst.TabIndex = 9;
            this.btnFirst.Text = "先頭データ";
            this.btnFirst.UseVisualStyleBackColor = true;
            this.btnFirst.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btnBefore
            // 
            this.btnBefore.Location = new System.Drawing.Point(398, 826);
            this.btnBefore.Name = "btnBefore";
            this.btnBefore.Size = new System.Drawing.Size(67, 24);
            this.btnBefore.TabIndex = 10;
            this.btnBefore.Text = "前データ";
            this.btnBefore.UseVisualStyleBackColor = true;
            this.btnBefore.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(465, 826);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(67, 24);
            this.btnNext.TabIndex = 11;
            this.btnNext.Text = "次データ";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Location = new System.Drawing.Point(532, 826);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(67, 24);
            this.btnEnd.TabIndex = 12;
            this.btnEnd.Text = "最終データ";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnHold
            // 
            this.btnHold.BackColor = System.Drawing.SystemColors.Control;
            this.btnHold.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnHold.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnHold.Location = new System.Drawing.Point(1531, 896);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(91, 33);
            this.btnHold.TabIndex = 16;
            this.btnHold.Text = "保留：F11";
            this.btnHold.UseVisualStyleBackColor = false;
            this.btnHold.Click += new System.EventHandler(this.button11_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.SystemColors.Control;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPrint.Location = new System.Drawing.Point(1256, 896);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(123, 33);
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
            this.txtErrStatus.Location = new System.Drawing.Point(10, 896);
            this.txtErrStatus.Name = "txtErrStatus";
            this.txtErrStatus.ReadOnly = true;
            this.txtErrStatus.Size = new System.Drawing.Size(23, 19);
            this.txtErrStatus.TabIndex = 358;
            this.txtErrStatus.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.lblNoImage);
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1025, 806);
            this.panel2.TabIndex = 363;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(-1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 117);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // txtYear
            // 
            this.txtYear.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtYear.Location = new System.Drawing.Point(1055, 12);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.Size = new System.Drawing.Size(74, 31);
            this.txtYear.TabIndex = 365;
            this.txtYear.Text = "2020";
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Lavender;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(1127, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 31);
            this.label4.TabIndex = 366;
            this.label4.Text = "年";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Lavender;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(1208, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 31);
            this.label5.TabIndex = 367;
            this.label5.Text = "月";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMonth
            // 
            this.txtMonth.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMonth.Location = new System.Drawing.Point(1159, 12);
            this.txtMonth.MaxLength = 2;
            this.txtMonth.Name = "txtMonth";
            this.txtMonth.Size = new System.Drawing.Size(50, 31);
            this.txtMonth.TabIndex = 368;
            this.txtMonth.Text = "10";
            this.txtMonth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Lavender;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label7.Location = new System.Drawing.Point(1250, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 31);
            this.label7.TabIndex = 369;
            this.label7.Text = "発注書番号";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPID
            // 
            this.txtPID.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtPID.Location = new System.Drawing.Point(1370, 12);
            this.txtPID.MaxLength = 3;
            this.txtPID.Name = "txtPID";
            this.txtPID.Size = new System.Drawing.Size(58, 31);
            this.txtPID.TabIndex = 370;
            this.txtPID.Text = "100";
            this.txtPID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPID.TextChanged += new System.EventHandler(this.txtPID_TextChanged);
            // 
            // txtSeqNum
            // 
            this.txtSeqNum.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSeqNum.Location = new System.Drawing.Point(1427, 12);
            this.txtSeqNum.MaxLength = 2;
            this.txtSeqNum.Name = "txtSeqNum";
            this.txtSeqNum.Size = new System.Drawing.Size(41, 31);
            this.txtSeqNum.TabIndex = 371;
            this.txtSeqNum.Text = "10";
            this.txtSeqNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSeqNum.TextChanged += new System.EventHandler(this.txtSeqNum_TextChanged);
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Lavender;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label8.Location = new System.Drawing.Point(1055, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 31);
            this.label8.TabIndex = 372;
            this.label8.Text = "得意先名";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Lavender;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label9.Location = new System.Drawing.Point(1479, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 31);
            this.label9.TabIndex = 373;
            this.label9.Text = "得意先コード";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTokuisakiCD
            // 
            this.txtTokuisakiCD.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtTokuisakiCD.Location = new System.Drawing.Point(1618, 12);
            this.txtTokuisakiCD.MaxLength = 7;
            this.txtTokuisakiCD.Name = "txtTokuisakiCD";
            this.txtTokuisakiCD.Size = new System.Drawing.Size(247, 31);
            this.txtTokuisakiCD.TabIndex = 374;
            this.txtTokuisakiCD.Text = "1001001";
            this.txtTokuisakiCD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTokuisakiCD.TextChanged += new System.EventHandler(this.txtTokuisakiCD_TextChanged);
            // 
            // textBox142
            // 
            this.textBox142.BackColor = System.Drawing.Color.White;
            this.textBox142.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox142.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox142.Location = new System.Drawing.Point(1823, 101);
            this.textBox142.MaxLength = 2;
            this.textBox142.Name = "textBox142";
            this.textBox142.Size = new System.Drawing.Size(41, 22);
            this.textBox142.TabIndex = 579;
            this.textBox142.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label89
            // 
            this.label89.BackColor = System.Drawing.Color.Lavender;
            this.label89.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label89.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label89.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label89.Location = new System.Drawing.Point(1583, 83);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(281, 19);
            this.label89.TabIndex = 586;
            this.label89.Text = "店着日";
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox135
            // 
            this.textBox135.BackColor = System.Drawing.Color.White;
            this.textBox135.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox135.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox135.Location = new System.Drawing.Point(1583, 101);
            this.textBox135.MaxLength = 2;
            this.textBox135.Name = "textBox135";
            this.textBox135.Size = new System.Drawing.Size(41, 22);
            this.textBox135.TabIndex = 573;
            this.textBox135.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox136
            // 
            this.textBox136.BackColor = System.Drawing.Color.White;
            this.textBox136.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox136.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox136.Location = new System.Drawing.Point(1623, 101);
            this.textBox136.MaxLength = 2;
            this.textBox136.Name = "textBox136";
            this.textBox136.Size = new System.Drawing.Size(41, 22);
            this.textBox136.TabIndex = 574;
            this.textBox136.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox138
            // 
            this.textBox138.BackColor = System.Drawing.Color.White;
            this.textBox138.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox138.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox138.Location = new System.Drawing.Point(1663, 101);
            this.textBox138.MaxLength = 2;
            this.textBox138.Name = "textBox138";
            this.textBox138.Size = new System.Drawing.Size(41, 22);
            this.textBox138.TabIndex = 575;
            this.textBox138.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox137
            // 
            this.textBox137.BackColor = System.Drawing.Color.White;
            this.textBox137.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox137.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox137.Location = new System.Drawing.Point(1703, 101);
            this.textBox137.MaxLength = 2;
            this.textBox137.Name = "textBox137";
            this.textBox137.Size = new System.Drawing.Size(41, 22);
            this.textBox137.TabIndex = 576;
            this.textBox137.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox140
            // 
            this.textBox140.BackColor = System.Drawing.Color.White;
            this.textBox140.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox140.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox140.Location = new System.Drawing.Point(1743, 101);
            this.textBox140.MaxLength = 2;
            this.textBox140.Name = "textBox140";
            this.textBox140.Size = new System.Drawing.Size(41, 22);
            this.textBox140.TabIndex = 577;
            this.textBox140.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox139
            // 
            this.textBox139.BackColor = System.Drawing.Color.White;
            this.textBox139.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox139.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBox139.Location = new System.Drawing.Point(1783, 101);
            this.textBox139.MaxLength = 2;
            this.textBox139.Name = "textBox139";
            this.textBox139.Size = new System.Drawing.Size(41, 22);
            this.textBox139.TabIndex = 578;
            this.textBox139.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dg1
            // 
            this.dg1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dg1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg1.Location = new System.Drawing.Point(1053, 128);
            this.dg1.Name = "dg1";
            this.dg1.RowTemplate.Height = 21;
            this.dg1.Size = new System.Drawing.Size(812, 690);
            this.dg1.TabIndex = 364;
            this.dg1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dataGridViewEx1_CellPainting);
            this.dg1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dg1_CurrentCellDirtyStateChanged);
            // 
            // btnData
            // 
            this.btnData.BackColor = System.Drawing.SystemColors.Control;
            this.btnData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnData.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnData.Location = new System.Drawing.Point(1626, 896);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(144, 33);
            this.btnData.TabIndex = 587;
            this.btnData.Text = "発注データ登録：F12";
            this.btnData.UseVisualStyleBackColor = false;
            // 
            // lblTokuisakiName
            // 
            this.lblTokuisakiName.BackColor = System.Drawing.Color.White;
            this.lblTokuisakiName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTokuisakiName.Font = new System.Drawing.Font("ＭＳ ゴシック", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTokuisakiName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTokuisakiName.Location = new System.Drawing.Point(1175, 46);
            this.lblTokuisakiName.Name = "lblTokuisakiName";
            this.lblTokuisakiName.Size = new System.Drawing.Size(690, 31);
            this.lblTokuisakiName.TabIndex = 588;
            this.lblTokuisakiName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPage.Location = new System.Drawing.Point(1050, 106);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(49, 14);
            this.lblPage.TabIndex = 589;
            this.lblPage.Text = "label1";
            // 
            // frmCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1877, 932);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.lblTokuisakiName);
            this.Controls.Add(this.btnData);
            this.Controls.Add(this.label89);
            this.Controls.Add(this.textBox142);
            this.Controls.Add(this.textBox139);
            this.Controls.Add(this.textBox140);
            this.Controls.Add(this.textBox137);
            this.Controls.Add(this.textBox138);
            this.Controls.Add(this.textBox136);
            this.Controls.Add(this.textBox135);
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
            this.Controls.Add(this.dg1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btnLeft);
            this.Controls.Add(this.btnRight);
            this.Controls.Add(this.txtErrStatus);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnHold);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBefore);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnErrCheck);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmCorrect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FAX発注書データ作成";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCorrect_FormClosing);
            this.Load += new System.EventHandler(this.frmCorrect_Load);
            this.Shown += new System.EventHandler(this.frmCorrect_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCorrect_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HScrollBar hScrollBar1;
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
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnFirst;
        private System.Windows.Forms.Button btnBefore;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.TextBox txtErrStatus;
        private System.Windows.Forms.Button btnRight;
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
        private System.Windows.Forms.TextBox textBox142;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.TextBox textBox135;
        private System.Windows.Forms.TextBox textBox136;
        private System.Windows.Forms.TextBox textBox138;
        private System.Windows.Forms.TextBox textBox137;
        private System.Windows.Forms.TextBox textBox140;
        private System.Windows.Forms.TextBox textBox139;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Label lblTokuisakiName;
        private System.Windows.Forms.Label lblPage;
    }
}