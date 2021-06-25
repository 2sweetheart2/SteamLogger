
namespace SteamLogger
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.loginBox = new System.Windows.Forms.TextBox();
            this.loginPas = new System.Windows.Forms.Label();
            this.passwordPas = new System.Windows.Forms.Label();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.load = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.SteamGuardText1 = new System.Windows.Forms.Label();
            this.SteamGuardText2 = new System.Windows.Forms.Label();
            this.ActivateSteamGuard = new System.Windows.Forms.Button();
            this.generateGuard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // loginBox
            // 
            this.loginBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.loginBox, "loginBox");
            this.loginBox.Name = "loginBox";
            // 
            // loginPas
            // 
            resources.ApplyResources(this.loginPas, "loginPas");
            this.loginPas.Name = "loginPas";
            // 
            // passwordPas
            // 
            resources.ApplyResources(this.passwordPas, "passwordPas");
            this.passwordPas.Name = "passwordPas";
            // 
            // passwordBox
            // 
            this.passwordBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.passwordBox, "passwordBox");
            this.passwordBox.Name = "passwordBox";
            // 
            // add
            // 
            resources.ApplyResources(this.add, "add");
            this.add.Name = "add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // load
            // 
            resources.ApplyResources(this.load, "load");
            this.load.Name = "load";
            this.load.UseVisualStyleBackColor = true;
            this.load.Click += new System.EventHandler(this.load_Click);
            // 
            // remove
            // 
            resources.ApplyResources(this.remove, "remove");
            this.remove.Name = "remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // SteamGuardText1
            // 
            resources.ApplyResources(this.SteamGuardText1, "SteamGuardText1");
            this.SteamGuardText1.Name = "SteamGuardText1";
            // 
            // SteamGuardText2
            // 
            resources.ApplyResources(this.SteamGuardText2, "SteamGuardText2");
            this.SteamGuardText2.ForeColor = System.Drawing.Color.Red;
            this.SteamGuardText2.Name = "SteamGuardText2";
            // 
            // ActivateSteamGuard
            // 
            resources.ApplyResources(this.ActivateSteamGuard, "ActivateSteamGuard");
            this.ActivateSteamGuard.Name = "ActivateSteamGuard";
            this.ActivateSteamGuard.UseVisualStyleBackColor = true;
            this.ActivateSteamGuard.Click += new System.EventHandler(this.ActivateSteamGuard_Click);
            // 
            // generateGuard
            // 
            resources.ApplyResources(this.generateGuard, "generateGuard");
            this.generateGuard.Name = "generateGuard";
            this.generateGuard.UseVisualStyleBackColor = true;
            this.generateGuard.Click += new System.EventHandler(this.generateGuard_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.generateGuard);
            this.Controls.Add(this.ActivateSteamGuard);
            this.Controls.Add(this.SteamGuardText2);
            this.Controls.Add(this.SteamGuardText1);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.load);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.add);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.passwordPas);
            this.Controls.Add(this.loginPas);
            this.Controls.Add(this.loginBox);
            this.Controls.Add(this.comboBox1);
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox loginBox;
        private System.Windows.Forms.Label loginPas;
        private System.Windows.Forms.Label passwordPas;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button load;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Label SteamGuardText1;
        private System.Windows.Forms.Label SteamGuardText2;
        private System.Windows.Forms.Button ActivateSteamGuard;
        private System.Windows.Forms.Button generateGuard;
    }
}

