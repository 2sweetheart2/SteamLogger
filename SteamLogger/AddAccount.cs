using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class AddAccount : Form
    {
        MainForm main;
        public AddAccount(MainForm main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = loginBox.Text;
            string password = passBox.Text;
            if (login.Length <= 0 || password.Length <= 0) return;
            main.addUser(login, password);
            loginBox.Text = "";
            passBox.Text = "";
        }

    }
}
