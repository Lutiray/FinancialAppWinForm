namespace PersonalFinanceApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.txtbAmount = new System.Windows.Forms.TextBox();
            this.btnExpenses = new System.Windows.Forms.Button();
            this.btnIncome = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dgvHistory = new System.Windows.Forms.DataGridView();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.cmbDescription = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtBalance = new System.Windows.Forms.TextBox();
            this.chartExpenses = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.rtxtIncomeAndExpenses = new System.Windows.Forms.RichTextBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnShowTransaction = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnResetDate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartExpenses)).BeginInit();
            this.SuspendLayout();
            // 
            // txtbAmount
            // 
            this.txtbAmount.Location = new System.Drawing.Point(583, 279);
            this.txtbAmount.Name = "txtbAmount";
            this.txtbAmount.Size = new System.Drawing.Size(219, 20);
            this.txtbAmount.TabIndex = 1;
            this.txtbAmount.TextChanged += new System.EventHandler(this.txtbAmount_TextChanged);
            // 
            // btnExpenses
            // 
            this.btnExpenses.BackColor = System.Drawing.SystemColors.Window;
            this.btnExpenses.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnExpenses.Location = new System.Drawing.Point(708, 446);
            this.btnExpenses.Name = "btnExpenses";
            this.btnExpenses.Size = new System.Drawing.Size(94, 34);
            this.btnExpenses.TabIndex = 2;
            this.btnExpenses.Text = "Výdaje";
            this.btnExpenses.UseVisualStyleBackColor = false;
            this.btnExpenses.Click += new System.EventHandler(this.btnExpenses_Click);
            // 
            // btnIncome
            // 
            this.btnIncome.BackColor = System.Drawing.SystemColors.Window;
            this.btnIncome.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnIncome.Location = new System.Drawing.Point(583, 446);
            this.btnIncome.Name = "btnIncome";
            this.btnIncome.Size = new System.Drawing.Size(94, 34);
            this.btnIncome.TabIndex = 3;
            this.btnIncome.Text = "Příjmy";
            this.btnIncome.UseVisualStyleBackColor = false;
            this.btnIncome.Click += new System.EventHandler(this.btnIncome_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(580, 302);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Kategorie";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(580, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Suma";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(586, 387);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "Datum";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(570, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 19);
            this.label4.TabIndex = 12;
            this.label4.Text = "Balance";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(338, 25);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(205, 20);
            this.dtpStartDate.TabIndex = 16;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.dtpStartDate_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(570, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 19);
            this.label5.TabIndex = 17;
            this.label5.Text = "Příjmy a výdaje";
            // 
            // dgvHistory
            // 
            this.dgvHistory.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistory.Location = new System.Drawing.Point(12, 344);
            this.dgvHistory.Name = "dgvHistory";
            this.dgvHistory.ReadOnly = true;
            this.dgvHistory.Size = new System.Drawing.Size(530, 153);
            this.dgvHistory.TabIndex = 18;
            this.dgvHistory.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistory_CellContentClick);
            // 
            // cmbCategories
            // 
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(583, 321);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(219, 21);
            this.cmbCategories.TabIndex = 21;
            this.cmbCategories.SelectedIndexChanged += new System.EventHandler(this.cmbCategories_SelectedIndexChanged);
            // 
            // cmbDescription
            // 
            this.cmbDescription.FormattingEnabled = true;
            this.cmbDescription.Location = new System.Drawing.Point(583, 363);
            this.cmbDescription.Name = "cmbDescription";
            this.cmbDescription.Size = new System.Drawing.Size(219, 21);
            this.cmbDescription.TabIndex = 22;
            this.cmbDescription.SelectedIndexChanged += new System.EventHandler(this.cmbDescription_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(582, 344);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 16);
            this.label6.TabIndex = 23;
            this.label6.Text = "Popís";
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(583, 406);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(219, 20);
            this.dtpDate.TabIndex = 24;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // txtBalance
            // 
            this.txtBalance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtBalance.Location = new System.Drawing.Point(574, 99);
            this.txtBalance.Name = "txtBalance";
            this.txtBalance.ReadOnly = true;
            this.txtBalance.Size = new System.Drawing.Size(177, 20);
            this.txtBalance.TabIndex = 25;
            this.txtBalance.TextChanged += new System.EventHandler(this.txtBalance_TextChanged);
            // 
            // chartExpenses
            // 
            chartArea2.Name = "ChartArea1";
            this.chartExpenses.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartExpenses.Legends.Add(legend2);
            this.chartExpenses.Location = new System.Drawing.Point(32, 18);
            this.chartExpenses.Name = "chartExpenses";
            this.chartExpenses.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SemiTransparent;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartExpenses.Series.Add(series2);
            this.chartExpenses.Size = new System.Drawing.Size(300, 300);
            this.chartExpenses.TabIndex = 26;
            this.chartExpenses.Text = "Expences";
            this.chartExpenses.Click += new System.EventHandler(this.chartExpenses_Click_1);
            // 
            // rtxtIncomeAndExpenses
            // 
            this.rtxtIncomeAndExpenses.Location = new System.Drawing.Point(574, 25);
            this.rtxtIncomeAndExpenses.Name = "rtxtIncomeAndExpenses";
            this.rtxtIncomeAndExpenses.ReadOnly = true;
            this.rtxtIncomeAndExpenses.Size = new System.Drawing.Size(219, 49);
            this.rtxtIncomeAndExpenses.TabIndex = 28;
            this.rtxtIncomeAndExpenses.Text = "";
            this.rtxtIncomeAndExpenses.TextChanged += new System.EventHandler(this.rtxtIncomeAndExpenses_TextChanged);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(338, 76);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(205, 20);
            this.dtpEndDate.TabIndex = 29;
            this.dtpEndDate.ValueChanged += new System.EventHandler(this.dtpEndDate_ValueChanged);
            // 
            // btnShowTransaction
            // 
            this.btnShowTransaction.BackColor = System.Drawing.SystemColors.Window;
            this.btnShowTransaction.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnShowTransaction.Location = new System.Drawing.Point(341, 102);
            this.btnShowTransaction.Name = "btnShowTransaction";
            this.btnShowTransaction.Size = new System.Drawing.Size(84, 23);
            this.btnShowTransaction.TabIndex = 31;
            this.btnShowTransaction.Text = "Ukázat";
            this.btnShowTransaction.UseVisualStyleBackColor = false;
            this.btnShowTransaction.Click += new System.EventHandler(this.btnShowTransaction_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(438, 503);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 26);
            this.button1.TabIndex = 32;
            this.button1.Text = "Smazat";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnDeleteTransaction_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(338, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 16);
            this.label7.TabIndex = 33;
            this.label7.Text = "Do";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(338, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 16);
            this.label8.TabIndex = 34;
            this.label8.Text = "Od";
            // 
            // btnResetDate
            // 
            this.btnResetDate.BackColor = System.Drawing.SystemColors.Window;
            this.btnResetDate.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnResetDate.Location = new System.Drawing.Point(451, 102);
            this.btnResetDate.Name = "btnResetDate";
            this.btnResetDate.Size = new System.Drawing.Size(91, 23);
            this.btnResetDate.TabIndex = 35;
            this.btnResetDate.Text = "Resetovat";
            this.btnResetDate.UseVisualStyleBackColor = false;
            this.btnResetDate.Click += new System.EventHandler(this.btnResetDate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(12, 320);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 19);
            this.label9.TabIndex = 36;
            this.label9.Text = "Přehled plateb";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Moccasin;
            this.ClientSize = new System.Drawing.Size(814, 541);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnResetDate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnShowTransaction);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.rtxtIncomeAndExpenses);
            this.Controls.Add(this.chartExpenses);
            this.Controls.Add(this.txtBalance);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbDescription);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.dgvHistory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnIncome);
            this.Controls.Add(this.btnExpenses);
            this.Controls.Add(this.txtbAmount);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartExpenses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtbAmount;
        private System.Windows.Forms.Button btnExpenses;
        private System.Windows.Forms.Button btnIncome;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvHistory;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.ComboBox cmbDescription;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox txtBalance;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartExpenses;
        private System.Windows.Forms.RichTextBox rtxtIncomeAndExpenses;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnShowTransaction;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnResetDate;
        private System.Windows.Forms.Label label9;
    }
}

