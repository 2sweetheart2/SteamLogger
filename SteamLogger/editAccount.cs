using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class editAccount : Form
    {
        MainForm main;
        string oldname;
        string oldpass;
        public editAccount(MainForm main,string oldname,string oldpass)
        {
            InitializeComponent();
            this.main = main;
            this.oldname = oldname;
            this.oldpass = oldpass;
        }

        private void save_Click(object sender, EventArgs e)
        {
            if(label1.Text.Length<0 || label2.Text.Length < 0)
            {
                MessageBox.Show("unccorect login or passwrod!", "SteamAuth");
                loginBox.Text = oldname;
                passBox.Text = oldpass;
                return;
            }
            main.EditUser(oldname, loginBox.Text, passBox.Text);
            MessageBox.Show("successfully saved!", "SteamAuth");
            Close();
        }
    }
}
