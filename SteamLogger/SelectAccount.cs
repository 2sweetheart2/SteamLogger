using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class SelectAccount : Form
    {


        MainForm main;
        List<MainForm.User> users = new List<MainForm.User>();
        public SelectAccount(MainForm main)
        {
            InitializeComponent();
            this.main = main;
            this.users = main.getUsers();
            foreach(MainForm.User user in users)
            {
                ListBox.Items.Add(user.name);
                
            }
        }

        private void SelectAccount_Load(object sender, EventArgs e)
        {

        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            main.LoginAccount(user.name, user.password, user.link);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            main.removeUser(ListBox.SelectedIndex);
            ListBox.Items.RemoveAt(ListBox.SelectedIndex);
        }

        private MainForm.User getUser()
        {
            if (ListBox.SelectedIndex < 0) return null;
            return users[ListBox.SelectedIndex];
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            main.EditUser2(user);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            main.generateGuardCode(user);
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            if (user.link.Length <= 0) label1.Text = "Steam Guard: DISABLED";
            else label1.Text = "Steam Guard: ENABLED";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MainForm.User user = getUser();
            if (user == null) return;
            if (user.link.Length > 0) return;
            CreateSteamAuthLink newfrm = new CreateSteamAuthLink(user.name, user.password);
            newfrm.Show();
        }

        public void UpdateUsers()
        {
            users = main.getUsers();
            ListBox.Items.Clear();
            foreach (MainForm.User user in users)
            {
                ListBox.Items.Add(user.name);
            }
        }
    }
}
