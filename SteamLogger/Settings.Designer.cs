
namespace SteamLogger
{
    partial class Settings
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
            this.autoRun = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.roll_up_after_run = new System.Windows.Forms.CheckBox();
            this.close_after_run = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // autoRun
            // 
            this.autoRun.AutoSize = true;
            this.autoRun.Location = new System.Drawing.Point(13, 13);
            this.autoRun.Name = "autoRun";
            this.autoRun.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.autoRun.Size = new System.Drawing.Size(71, 19);
            this.autoRun.TabIndex = 0;
            this.autoRun.Text = "auto run";
            this.autoRun.UseVisualStyleBackColor = true;
            this.autoRun.CheckedChanged += new System.EventHandler(this.autoRun_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(13, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "if auto run is disabled \r\nthese functions will not be available:";
            // 
            // roll_up_after_run
            // 
            this.roll_up_after_run.AutoSize = true;
            this.roll_up_after_run.Location = new System.Drawing.Point(13, 100);
            this.roll_up_after_run.Name = "roll_up_after_run";
            this.roll_up_after_run.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.roll_up_after_run.Size = new System.Drawing.Size(108, 19);
            this.roll_up_after_run.TabIndex = 2;
            this.roll_up_after_run.Text = "roll up after run";
            this.roll_up_after_run.UseVisualStyleBackColor = true;
            this.roll_up_after_run.CheckedChanged += new System.EventHandler(this.roll_up_after_run_CheckedChanged);
            // 
            // close_after_run
            // 
            this.close_after_run.AutoSize = true;
            this.close_after_run.BackColor = System.Drawing.Color.Transparent;
            this.close_after_run.Location = new System.Drawing.Point(12, 125);
            this.close_after_run.Name = "close_after_run";
            this.close_after_run.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.close_after_run.Size = new System.Drawing.Size(101, 19);
            this.close_after_run.TabIndex = 3;
            this.close_after_run.Text = "close after run";
            this.close_after_run.UseVisualStyleBackColor = false;
            this.close_after_run.CheckedChanged += new System.EventHandler(this.close_after_run_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(152, 146);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "auto start up account:";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(285, 228);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.close_after_run);
            this.Controls.Add(this.roll_up_after_run);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.autoRun);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Settings";
            this.Text = "Settings \"Steam Auth\"";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoRun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox roll_up_after_run;
        private System.Windows.Forms.CheckBox close_after_run;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
    }
}