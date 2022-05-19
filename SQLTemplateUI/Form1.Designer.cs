
namespace SQLTemplateUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.stagingBox = new System.Windows.Forms.ListBox();
            this.slugBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Generate = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.dbSelect = new System.Windows.Forms.NumericUpDown();
            this.outputText = new System.Windows.Forms.TextBox();
            this.sqlpathBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sqlButton = new System.Windows.Forms.Button();
            this.exactData = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.exactButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.AllInOne = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label8 = new System.Windows.Forms.Label();
            this.SQLServer = new System.Windows.Forms.TextBox();
            this.ZipButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.siteID = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.threads = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dbSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threads)).BeginInit();
            this.SuspendLayout();
            // 
            // stagingBox
            // 
            this.stagingBox.FormattingEnabled = true;
            this.stagingBox.Items.AddRange(new object[] {
            "staging",
            "production"});
            this.stagingBox.Location = new System.Drawing.Point(2, 140);
            this.stagingBox.Name = "stagingBox";
            this.stagingBox.Size = new System.Drawing.Size(206, 30);
            this.stagingBox.TabIndex = 0;
            // 
            // slugBox
            // 
            this.slugBox.Location = new System.Drawing.Point(3, 25);
            this.slugBox.Name = "slugBox";
            this.slugBox.Size = new System.Drawing.Size(206, 20);
            this.slugBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Slug";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Environment";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-1, 173);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Database size in GB";
            // 
            // Generate
            // 
            this.Generate.Location = new System.Drawing.Point(2, 315);
            this.Generate.Name = "Generate";
            this.Generate.Size = new System.Drawing.Size(206, 27);
            this.Generate.TabIndex = 8;
            this.Generate.Text = "Generate Template";
            this.Generate.UseVisualStyleBackColor = true;
            this.Generate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // dbSelect
            // 
            this.dbSelect.Location = new System.Drawing.Point(2, 189);
            this.dbSelect.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.dbSelect.Name = "dbSelect";
            this.dbSelect.Size = new System.Drawing.Size(150, 20);
            this.dbSelect.TabIndex = 10;
            // 
            // outputText
            // 
            this.outputText.Location = new System.Drawing.Point(424, 9);
            this.outputText.Multiline = true;
            this.outputText.Name = "outputText";
            this.outputText.ReadOnly = true;
            this.outputText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputText.Size = new System.Drawing.Size(380, 333);
            this.outputText.TabIndex = 11;
            // 
            // sqlpathBox
            // 
            this.sqlpathBox.Location = new System.Drawing.Point(2, 64);
            this.sqlpathBox.Name = "sqlpathBox";
            this.sqlpathBox.Size = new System.Drawing.Size(206, 20);
            this.sqlpathBox.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "SQL Folder Path";
            // 
            // sqlButton
            // 
            this.sqlButton.Location = new System.Drawing.Point(212, 63);
            this.sqlButton.Name = "sqlButton";
            this.sqlButton.Size = new System.Drawing.Size(151, 21);
            this.sqlButton.TabIndex = 14;
            this.sqlButton.Text = "Select SQL Folder...";
            this.sqlButton.UseVisualStyleBackColor = true;
            this.sqlButton.Click += new System.EventHandler(this.sqlButton_Click);
            // 
            // exactData
            // 
            this.exactData.Location = new System.Drawing.Point(2, 101);
            this.exactData.Name = "exactData";
            this.exactData.Size = new System.Drawing.Size(205, 20);
            this.exactData.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(0, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "EXACTData Folder Path";
            // 
            // exactButton
            // 
            this.exactButton.Location = new System.Drawing.Point(213, 101);
            this.exactButton.Name = "exactButton";
            this.exactButton.Size = new System.Drawing.Size(151, 20);
            this.exactButton.TabIndex = 21;
            this.exactButton.Text = "Select EXACTData Folder...";
            this.exactButton.UseVisualStyleBackColor = true;
            this.exactButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(0, 299);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Manual Process (if problem with auto process)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(0, 253);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Automated Process";
            // 
            // AllInOne
            // 
            this.AllInOne.Location = new System.Drawing.Point(1, 269);
            this.AllInOne.Name = "AllInOne";
            this.AllInOne.Size = new System.Drawing.Size(206, 27);
            this.AllInOne.TabIndex = 24;
            this.AllInOne.Text = "Run Upsize Process";
            this.AllInOne.UseVisualStyleBackColor = true;
            this.AllInOne.Click += new System.EventHandler(this.AllInOne_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 348);
            this.progressBar1.MarqueeAnimationSpeed = 0;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(801, 20);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(259, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "SQL Server";
            // 
            // SQLServer
            // 
            this.SQLServer.Location = new System.Drawing.Point(262, 25);
            this.SQLServer.Name = "SQLServer";
            this.SQLServer.Size = new System.Drawing.Size(154, 20);
            this.SQLServer.TabIndex = 27;
            // 
            // ZipButton
            // 
            this.ZipButton.Location = new System.Drawing.Point(296, 140);
            this.ZipButton.Name = "ZipButton";
            this.ZipButton.Size = new System.Drawing.Size(68, 26);
            this.ZipButton.TabIndex = 28;
            this.ZipButton.Text = "Zip Backup";
            this.ZipButton.UseVisualStyleBackColor = true;
            this.ZipButton.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-1, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Practice ID (Optional)";
            // 
            // siteID
            // 
            this.siteID.Location = new System.Drawing.Point(2, 230);
            this.siteID.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.siteID.Name = "siteID";
            this.siteID.Size = new System.Drawing.Size(149, 20);
            this.siteID.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(213, 269);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(203, 27);
            this.button1.TabIndex = 32;
            this.button1.Text = "Restart SQL Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // threads
            // 
            this.threads.Location = new System.Drawing.Point(335, 315);
            this.threads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threads.Name = "threads";
            this.threads.Size = new System.Drawing.Size(81, 20);
            this.threads.TabIndex = 33;
            this.threads.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(332, 299);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Upsizer Threads";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 380);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.threads);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.siteID);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ZipButton);
            this.Controls.Add(this.SQLServer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.AllInOne);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.exactButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.exactData);
            this.Controls.Add(this.sqlButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sqlpathBox);
            this.Controls.Add(this.outputText);
            this.Controls.Add(this.dbSelect);
            this.Controls.Add(this.Generate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.slugBox);
            this.Controls.Add(this.stagingBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "All In One Upsize Tool v1.6";
            ((System.ComponentModel.ISupportInitialize)(this.dbSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.siteID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox stagingBox;
        private System.Windows.Forms.TextBox slugBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Generate;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.NumericUpDown dbSelect;
        private System.Windows.Forms.TextBox outputText;
        private System.Windows.Forms.TextBox sqlpathBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sqlButton;
        private System.Windows.Forms.TextBox exactData;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button exactButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button AllInOne;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox SQLServer;
        private System.Windows.Forms.Button ZipButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown siteID;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown threads;
        private System.Windows.Forms.Label label10;
    }
}

