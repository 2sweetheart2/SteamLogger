using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class AccountSettings : Form
    {
        MainForm main;
        string name;
        public AccountSettings(MainForm form, string oldname)
        {
            InitializeComponent();
            main = form;
            name = oldname;
        }

        private void save_Click(object sender, EventArgs e)
        {
            if(loginBox.Text.Length<=0 || passBox.Text.Length<= 0)
            {
                MessageBox.Show("uncorrect login or password!", "SteamAuth");
                return;
            }
            main.editUser(name, passBox.Text, loginBox.Text);
            Close();
        }
    }
}
