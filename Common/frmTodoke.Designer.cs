namespace STSH_OCR.Common
{
    partial class frmTodoke
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTodoke));
            this.label1 = new System.Windows.Forms.Label();
            this.sName = new System.Windows.Forms.TextBox();
            this.sFuri = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnS = new System.Windows.Forms.Button();
            this.sCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(205, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "得意先名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sName
            // 
            this.sName.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.sName.Location = new System.Drawing.Point(307, 28);
            this.sName.Name = "sName";
            this.sName.Size = new System.Drawing.Size(288, 22);
            this.sName.TabIndex = 1;
            // 
            // sFuri
            // 
            this.sFuri.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sFuri.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.sFuri.Location = new System.Drawing.Point(696, 28);
            this.sFuri.Name = "sFuri";
            this.sFuri.Size = new System.Drawing.Size(248, 22);
            this.sFuri.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(594, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "得意先カナ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(15, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1017, 502);
            this.dataGridView1.StandardTab = true;
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(934, 584);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(98, 33);
            this.button2.TabIndex = 7;
            this.button2.TabStop = false;
            this.button2.Text = "終了(&E)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Control;
            this.button3.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.Location = new System.Drawing.Point(830, 584);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 33);
            this.button3.TabIndex = 6;
            this.button3.TabStop = false;
            this.button3.Text = "選択(&S)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnS
            // 
            this.btnS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnS.BackColor = System.Drawing.SystemColors.Control;
            this.btnS.Image = ((System.Drawing.Image)(resources.GetObject("btnS.Image")));
            this.btnS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnS.Location = new System.Drawing.Point(972, 16);
            this.btnS.Name = "btnS";
            this.btnS.Size = new System.Drawing.Size(60, 50);
            this.btnS.TabIndex = 4;
            this.btnS.Text = "(&R)";
            this.btnS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnS.UseVisualStyleBackColor = true;
            this.btnS.Click += new System.EventHandler(this.btnS_Click);
            // 
            // sCode
            // 
            this.sCode.Font = new System.Drawing.Font("ＭＳ ゴシック", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.sCode.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.sCode.Location = new System.Drawing.Point(117, 28);
            this.sCode.MaxLength = 7;
            this.sCode.Name = "sCode";
            this.sCode.Size = new System.Drawing.Size(89, 22);
            this.sCode.TabIndex = 0;
            this.sCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sCode_KeyPress);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label4.Location = new System.Drawing.Point(15, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 22);
            this.label4.TabIndex = 16;
            this.label4.Text = "得意先コード";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmTodoke
            // 
            this.AcceptButton = this.btnS;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 625);
            this.Controls.Add(this.sCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnS);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.sFuri);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sName);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmTodoke";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "得意先検索";
            this.Load += new System.EventHandler(this.frmTodoke_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmTodoke_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sName;
        private System.Windows.Forms.TextBox sFuri;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnS;
        private System.Windows.Forms.TextBox sCode;
        private System.Windows.Forms.Label label4;
    }
}