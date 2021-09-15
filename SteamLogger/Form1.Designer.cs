
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelChiled = new System.Windows.Forms.Panel();
            this.Logo = new System.Windows.Forms.Label();
            this.PanelControls = new System.Windows.Forms.Panel();
            this.AddBtnLeftBorder = new System.Windows.Forms.Panel();
            this.LoadBtnLeftBorder = new System.Windows.Forms.Panel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.discord = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.feedback = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.LoadAccountMenu = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.customButton1 = new SteamLogger.components.CustomButton();
            this.PanelControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelChiled
            // 
            resources.ApplyResources(this.panelChiled, "panelChiled");
            this.panelChiled.Name = "panelChiled";
            // 
            // Logo
            // 
            resources.ApplyResources(this.Logo, "Logo");
            this.Logo.ForeColor = System.Drawing.Color.White;
            this.Logo.Name = "Logo";
            // 
            // PanelControls
            // 
            this.PanelControls.Controls.Add(this.AddBtnLeftBorder);
            this.PanelControls.Controls.Add(this.LoadBtnLeftBorder);
            this.PanelControls.Controls.Add(this.linkLabel2);
            this.PanelControls.Controls.Add(this.discord);
            this.PanelControls.Controls.Add(this.linkLabel1);
            this.PanelControls.Controls.Add(this.feedback);
            this.PanelControls.Controls.Add(this.button1);
            this.PanelControls.Controls.Add(this.LoadAccountMenu);
            resources.ApplyResources(this.PanelControls, "PanelControls");
            this.PanelControls.Name = "PanelControls";
            // 
            // AddBtnLeftBorder
            // 
            this.AddBtnLeftBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(49)))), ((int)(((byte)(137)))));
            resources.ApplyResources(this.AddBtnLeftBorder, "AddBtnLeftBorder");
            this.AddBtnLeftBorder.Name = "AddBtnLeftBorder";
            // 
            // LoadBtnLeftBorder
            // 
            this.LoadBtnLeftBorder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(49)))), ((int)(((byte)(137)))));
            resources.ApplyResources(this.LoadBtnLeftBorder, "LoadBtnLeftBorder");
            this.LoadBtnLeftBorder.Name = "LoadBtnLeftBorder";
            // 
            // linkLabel2
            // 
            this.linkLabel2.ActiveLinkColor = System.Drawing.Color.Indigo;
            resources.ApplyResources(this.linkLabel2, "linkLabel2");
            this.linkLabel2.LinkColor = System.Drawing.Color.White;
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.TabStop = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // discord
            // 
            resources.ApplyResources(this.discord, "discord");
            this.discord.ForeColor = System.Drawing.Color.White;
            this.discord.Name = "discord";
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Indigo;
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.LinkColor = System.Drawing.Color.White;
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // feedback
            // 
            resources.ApplyResources(this.feedback, "feedback");
            this.feedback.ForeColor = System.Drawing.Color.White;
            this.feedback.Name = "feedback";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.button1.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // LoadAccountMenu
            // 
            resources.ApplyResources(this.LoadAccountMenu, "LoadAccountMenu");
            this.LoadAccountMenu.FlatAppearance.BorderSize = 0;
            this.LoadAccountMenu.ForeColor = System.Drawing.Color.White;
            this.LoadAccountMenu.Name = "LoadAccountMenu";
            this.LoadAccountMenu.UseVisualStyleBackColor = true;
            this.LoadAccountMenu.Click += new System.EventHandler(this.LoadAccountMenu_Click);
            this.LoadAccountMenu.MouseEnter += new System.EventHandler(this.LoadAccountMenu_MouseEnter);
            this.LoadAccountMenu.MouseLeave += new System.EventHandler(this.LoadAccountMenu_MouseLeave);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(30)))), ((int)(((byte)(54)))));
            this.panel1.Controls.Add(this.PanelControls);
            this.panel1.Controls.Add(this.Logo);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // customButton1
            // 
            this.customButton1.Color = System.Drawing.Color.Empty;
            resources.ApplyResources(this.customButton1, "customButton1");
            this.customButton1.Name = "customButton1";
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(51)))), ((int)(((byte)(73)))));
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelChiled);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.PanelControls.ResumeLayout(false);
            this.PanelControls.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label Logo;
        private System.Windows.Forms.Panel PanelControls;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button LoadAccountMenu;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelChiled;
        private System.Windows.Forms.Label discord;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label feedback;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private components.CustomButton customButton1;
        private System.Windows.Forms.Panel LoadBtnLeftBorder;
        private System.Windows.Forms.Panel AddBtnLeftBorder;
    }
}

