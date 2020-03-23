namespace STSH_OCR.Pattern
{
    partial class frmPtnAdd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPtnAdd));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMemo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaker = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSyohinName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_SyohinAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.最後に追加AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_SyohinInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Insert = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Before = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_After = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_ReadDays = new System.Windows.Forms.ToolStripMenuItem();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cmbSyohin_L = new System.Windows.Forms.ComboBox();
            this.cmbSyohin_M = new System.Windows.Forms.ComboBox();
            this.cmbSyohin_S = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView3 = new STSH_OCR.DataGridViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(14, 320);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(744, 638);
            this.dataGridView1.StandardTab = true;
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView1_SortCompare);
            this.dataGridView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseUp);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(774, 972);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "備考";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMemo
            // 
            this.txtMemo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMemo.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMemo.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMemo.Location = new System.Drawing.Point(847, 972);
            this.txtMemo.MaxLength = 255;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new System.Drawing.Size(575, 21);
            this.txtMemo.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.Location = new System.Drawing.Point(659, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "商品一覧(&A)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.Location = new System.Drawing.Point(327, 289);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(99, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "商品履歴(&L)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(14, 12);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 21;
            this.dataGridView2.Size = new System.Drawing.Size(744, 202);
            this.dataGridView2.StandardTab = true;
            this.dataGridView2.TabIndex = 12;
            this.dataGridView2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellValueChanged);
            this.dataGridView2.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView2_CurrentCellDirtyStateChanged);
            this.dataGridView2.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView2_EditingControlShowing);
            this.dataGridView2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView2_KeyDown);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "期間指定なし",
            "１ヶ月以内",
            "３ヶ月以内",
            "６ヶ月以内",
            "１年以内",
            "期間を指定"});
            this.comboBox1.Location = new System.Drawing.Point(14, 292);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(128, 21);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(148, 292);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(146, 21);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(295, 298);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "～";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.SystemColors.Control;
            this.button7.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button7.Location = new System.Drawing.Point(659, 219);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(99, 28);
            this.button7.TabIndex = 0;
            this.button7.Text = "得意先選択(&T)";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(16, 247);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 19);
            this.label3.TabIndex = 16;
            this.label3.Text = "仕入先名：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtMaker
            // 
            this.txtMaker.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMaker.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtMaker.Location = new System.Drawing.Point(14, 264);
            this.txtMaker.Name = "txtMaker";
            this.txtMaker.Size = new System.Drawing.Size(128, 21);
            this.txtMaker.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(298, 247);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 19);
            this.label4.TabIndex = 18;
            this.label4.Text = "商品 大分類：";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSyohinName
            // 
            this.txtSyohinName.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtSyohinName.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.txtSyohinName.Location = new System.Drawing.Point(148, 264);
            this.txtSyohinName.Name = "txtSyohinName";
            this.txtSyohinName.Size = new System.Drawing.Size(146, 21);
            this.txtSyohinName.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(148, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 19);
            this.label5.TabIndex = 20;
            this.label5.Text = "商品名：";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_SyohinAdd,
            this.ToolStripMenuItem_Insert,
            this.ToolStripMenuItem_Delete,
            this.ToolStripMenuItem_Before,
            this.ToolStripMenuItem_After,
            this.ToolStripMenuItem_ReadDays});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(173, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem_SyohinAdd
            // 
            this.toolStripMenuItem_SyohinAdd.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最後に追加AToolStripMenuItem,
            this.ToolStripMenuItem_SyohinInsert});
            this.toolStripMenuItem_SyohinAdd.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripMenuItem_SyohinAdd.Name = "toolStripMenuItem_SyohinAdd";
            this.toolStripMenuItem_SyohinAdd.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem_SyohinAdd.Text = "選択商品登録";
            // 
            // 最後に追加AToolStripMenuItem
            // 
            this.最後に追加AToolStripMenuItem.Name = "最後に追加AToolStripMenuItem";
            this.最後に追加AToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.最後に追加AToolStripMenuItem.Text = "最後に追加(&A)";
            this.最後に追加AToolStripMenuItem.Click += new System.EventHandler(this.最後に追加AToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_SyohinInsert
            // 
            this.ToolStripMenuItem_SyohinInsert.Name = "ToolStripMenuItem_SyohinInsert";
            this.ToolStripMenuItem_SyohinInsert.Size = new System.Drawing.Size(184, 22);
            this.ToolStripMenuItem_SyohinInsert.Text = "選択行の前に挿入(&I)";
            this.ToolStripMenuItem_SyohinInsert.Click += new System.EventHandler(this.現在の行に挿入IToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Insert
            // 
            this.ToolStripMenuItem_Insert.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F);
            this.ToolStripMenuItem_Insert.Name = "ToolStripMenuItem_Insert";
            this.ToolStripMenuItem_Insert.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_Insert.Text = "行挿入(&I)";
            this.ToolStripMenuItem_Insert.Click += new System.EventHandler(this.行挿入ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Delete
            // 
            this.ToolStripMenuItem_Delete.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ToolStripMenuItem_Delete.Name = "ToolStripMenuItem_Delete";
            this.ToolStripMenuItem_Delete.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_Delete.Text = "行削除(&D)";
            this.ToolStripMenuItem_Delete.Click += new System.EventHandler(this.行削除ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_Before
            // 
            this.ToolStripMenuItem_Before.Name = "ToolStripMenuItem_Before";
            this.ToolStripMenuItem_Before.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_Before.Text = "前へ移動(&B)";
            this.ToolStripMenuItem_Before.Click += new System.EventHandler(this.前へ移動BToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem_After
            // 
            this.ToolStripMenuItem_After.Name = "ToolStripMenuItem_After";
            this.ToolStripMenuItem_After.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_After.Text = "後へ移動(&A)";
            this.ToolStripMenuItem_After.Click += new System.EventHandler(this.ToolStripMenuItem_After_Click);
            // 
            // ToolStripMenuItem_ReadDays
            // 
            this.ToolStripMenuItem_ReadDays.Name = "ToolStripMenuItem_ReadDays";
            this.ToolStripMenuItem_ReadDays.Size = new System.Drawing.Size(172, 22);
            this.ToolStripMenuItem_ReadDays.Text = "リード日数設定(&R)";
            this.ToolStripMenuItem_ReadDays.Click += new System.EventHandler(this.ToolStripMenuItem_ReadDays_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLabel1.Location = new System.Drawing.Point(628, 303);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(65, 12);
            this.linkLabel1.TabIndex = 22;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "すべて選択";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.linkLabel2.Location = new System.Drawing.Point(702, 303);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(53, 12);
            this.linkLabel2.TabIndex = 23;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "選択解除";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button5.Location = new System.Drawing.Point(14, 966);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(129, 33);
            this.button5.TabIndex = 24;
            this.button5.Text = "パターン呼出(&P)";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button6.Location = new System.Drawing.Point(149, 966);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(129, 33);
            this.button6.TabIndex = 25;
            this.button6.Text = "画面取消(&C)";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button8.Location = new System.Drawing.Point(284, 966);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(129, 33);
            this.button8.TabIndex = 26;
            this.button8.Text = "注文商品取消(&T)";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // btnDel
            // 
            this.btnDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDel.Location = new System.Drawing.Point(419, 966);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(129, 33);
            this.btnDel.TabIndex = 27;
            this.btnDel.Text = "パターン削除(&D)";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.Location = new System.Drawing.Point(1448, 966);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(95, 33);
            this.button3.TabIndex = 28;
            this.button3.Text = "登録(&U)";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button4.Location = new System.Drawing.Point(1547, 966);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(95, 33);
            this.button4.TabIndex = 29;
            this.button4.Text = "終了(&E)";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cmbSyohin_L
            // 
            this.cmbSyohin_L.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSyohin_L.FormattingEnabled = true;
            this.cmbSyohin_L.Location = new System.Drawing.Point(298, 264);
            this.cmbSyohin_L.Name = "cmbSyohin_L";
            this.cmbSyohin_L.Size = new System.Drawing.Size(117, 21);
            this.cmbSyohin_L.TabIndex = 30;
            this.cmbSyohin_L.SelectedValueChanged += new System.EventHandler(this.cmbSyohin_L_SelectedValueChanged);
            // 
            // cmbSyohin_M
            // 
            this.cmbSyohin_M.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSyohin_M.FormattingEnabled = true;
            this.cmbSyohin_M.Location = new System.Drawing.Point(418, 263);
            this.cmbSyohin_M.Name = "cmbSyohin_M";
            this.cmbSyohin_M.Size = new System.Drawing.Size(117, 21);
            this.cmbSyohin_M.TabIndex = 31;
            this.cmbSyohin_M.SelectedValueChanged += new System.EventHandler(this.cmbSyohin_M_SelectedValueChanged);
            // 
            // cmbSyohin_S
            // 
            this.cmbSyohin_S.Font = new System.Drawing.Font("ＭＳ ゴシック", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSyohin_S.FormattingEnabled = true;
            this.cmbSyohin_S.Location = new System.Drawing.Point(537, 263);
            this.cmbSyohin_S.Name = "cmbSyohin_S";
            this.cmbSyohin_S.Size = new System.Drawing.Size(117, 21);
            this.cmbSyohin_S.TabIndex = 32;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(418, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 19);
            this.label6.TabIndex = 33;
            this.label6.Text = "中分類：";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(537, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 19);
            this.label7.TabIndex = 34;
            this.label7.Text = "小分類：";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGridView3
            // 
            this.dataGridView3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(774, 12);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.RowTemplate.Height = 21;
            this.dataGridView3.Size = new System.Drawing.Size(868, 946);
            this.dataGridView3.TabIndex = 15;
            this.dataGridView3.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView3_CellValueChanged);
            this.dataGridView3.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView3_EditingControlShowing);
            this.dataGridView3.Leave += new System.EventHandler(this.dataGridView3_Leave);
            this.dataGridView3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView3_MouseClick);
            // 
            // frmPtnAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1654, 1008);
            this.Controls.Add(this.cmbSyohin_S);
            this.Controls.Add(this.cmbSyohin_M);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbSyohin_L);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.txtSyohinName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMaker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView3);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtMemo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmPtnAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "発注書パターン登録";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPtnAdd_FormClosing);
            this.Load += new System.EventHandler(this.frmPtnAdd_Load);
            this.Shown += new System.EventHandler(this.frmPtnAdd_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPtnAdd_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMemo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button7;
        private DataGridViewEx dataGridView3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaker;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSyohinName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Insert;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Delete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_SyohinAdd;
        private System.Windows.Forms.ToolStripMenuItem 最後に追加AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_SyohinInsert;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Before;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_After;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_ReadDays;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cmbSyohin_L;
        private System.Windows.Forms.ComboBox cmbSyohin_M;
        private System.Windows.Forms.ComboBox cmbSyohin_S;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
    }
}