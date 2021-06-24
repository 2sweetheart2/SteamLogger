
namespace SteamLogger
{
    partial class CreateSteamAuthLink
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateSteamAuthLink));
            this.label1 = new System.Windows.Forms.Label();
            this.ENAddition = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.v1 = new System.Windows.Forms.Button();
            this.v2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(644, 231);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // ENAddition
            // 
            this.ENAddition.Location = new System.Drawing.Point(12, 303);
            this.ENAddition.Name = "ENAddition";
            this.ENAddition.Size = new System.Drawing.Size(110, 23);
            this.ENAddition.TabIndex = 2;
            this.ENAddition.Text = "Variant 1 Addition";
            this.ENAddition.UseVisualStyleBackColor = true;
            this.ENAddition.Click += new System.EventHandler(this.ENAddition_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(143, 303);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Variant 2 Addition";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // v1
            // 
            this.v1.Location = new System.Drawing.Point(12, 376);
            this.v1.Name = "v1";
            this.v1.Size = new System.Drawing.Size(74, 23);
            this.v1.TabIndex = 5;
            this.v1.Text = "Variant 1";
            this.v1.UseVisualStyleBackColor = true;
            this.v1.Click += new System.EventHandler(this.v1_Click);
            // 
            // v2
            // 
            this.v2.Location = new System.Drawing.Point(143, 376);
            this.v2.Name = "v2";
            this.v2.Size = new System.Drawing.Size(74, 23);
            this.v2.TabIndex = 6;
            this.v2.Text = "Variant 2";
            this.v2.UseVisualStyleBackColor = true;
            this.v2.Click += new System.EventHandler(this.v2_Click);
            // 
            // CreateSteamAuthLink
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.v2);
            this.Controls.Add(this.v1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ENAddition);
            this.Controls.Add(this.label1);
            this.Name = "CreateSteamAuthLink";
            this.Text = "CreateSteamAuthLink";
            this.Load += new System.EventHandler(this.CreateSteamAuthLink_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ENAddition;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button v1;
        private System.Windows.Forms.Button v2;
    }
}