﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamLogger;
using SteamAuth;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace SteamLogger
{

    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern UInt32 GetWindowThreadProcessId(IntPtr hwnd, ref Int32 pid);
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_TEST = RegisterWindowMessage("WM_TEST");

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);

        public class User
        {
            public User(string v1, string v2, string v3)
            {
                this.userName = v1;
                this.password = v2;
                this.steamGuardLink = v3;
            }
            public string userName;
            public string password;
            public string steamGuardLink;
        }

        public List<User> users = new List<User>();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public MainForm()
        {
            InitializeComponent();
        }
        
        

        private void load_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                LoginAccount(user.userName, user.password, user.steamGuardLink);
            }else MessageBox.Show("select account before load him", "SteamAuth");
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                List<string> lines = File.ReadAllLines(path).ToList();
                lines.RemoveAt(comboBox1.SelectedIndex);
                File.WriteAllLines(path, lines);
                users.RemoveAt(comboBox1.SelectedIndex);
                comboBox1.Items.Remove(comboBox1.SelectedItem);
            }
            else MessageBox.Show("select account before delete him", "SteamAuth");
        }

        private void add_Click(object sender, EventArgs e)
        {
            if(loginBox.Text.Length > 0 && passwordBox.Text.Length > 0)
            {
                users.Add(new User(loginBox.Text, passwordBox.Text, ""));
                comboBox1.Items.Add(loginBox.Text);
                List<string> lines = File.ReadAllLines(path).ToList();
                lines.Add(loginBox.Text + "/ /,/ /" + passwordBox.Text + "/ /,/ /");
                File.WriteAllLines(path, lines);
                loginBox.Text = "";
                passwordBox.Text = "";
            }
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            SteamGuardText2.Text = "";
            CheckForIllegalCrossThreadCalls = false;
            if (!Directory.Exists(path + @"\SteamAuth")) {
                Directory.CreateDirectory(path + @"\SteamAuth");
            }
            path += @"\SteamAuth\users.txt";
            if (!File.Exists(path))
            {
                StreamWriter sw = File.CreateText(path);
                sw.Flush();
                sw.Dispose();
            }

            List<string> lines = File.ReadAllLines(path).ToList();

            foreach (var line in lines)
            {
                string[] entries = line.Split("/ /,/ /");
                users.Add(new User(entries[0], entries[1], entries[2]));
                comboBox1.Items.Add(entries[0]);
            }
        }


        private void ActivateSteamGuard_Click(object sender, EventArgs e)
        {
            if (SteamGuardText2.Text == "ENABLE")
            {
                MessageBox.Show("Steam guard is enable for this account");
            }
            else if (SteamGuardText2.Text == "DISABLE" && comboBox1.SelectedIndex >=0) {
                User user = users[comboBox1.SelectedIndex];
                CreateSteamAuthLink newfrm = new CreateSteamAuthLink(user.userName, user.password);
                newfrm.Show();
            }
        }

        private void LoginAccount(string login, string pass, string steamGuadLink)
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", "null");
            startInfo.Arguments = " -login \"" + login + "\" \"" + pass + "\"";
            Process.Start(startInfo);
            User user = users[comboBox1.SelectedIndex];
            if (user.steamGuardLink.Length > 0)
            {
                var steamGuard = new SteamGuardAccount();
                steamGuard.SharedSecret = user.steamGuardLink;
                Task.Run(() => PutSteamGuardCode(steamGuard.GenerateSteamGuardCode()));
            }

        }
        void PutSteamGuardCode(string code)
        {
            Thread.Sleep(8000);
            SendKeys.SendWait(code);
            SendKeys.SendWait("{ENTER}");
            if (Process.GetProcessesByName("Steam").Length >= 1) Task.Run(() => PutSteamGuardCode(code));
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.steamGuardLink.Length > 0) {
                    SteamGuardText2.ForeColor = Color.Green;
                    SteamGuardText2.Text = "ENABLE";
                }
                else
                {
                    SteamGuardText2.ForeColor = Color.Red;
                    SteamGuardText2.Text = "DISABLE";
                }
            }
        }

        private void generateGuard_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.steamGuardLink.Length > 0)
                {
                    var steamGuard = new SteamGuardAccount();
                    steamGuard.SharedSecret = user.steamGuardLink;
                    MessageBox.Show(steamGuard.GenerateSteamGuardCode(), "Steam Guard code");
                }
                else MessageBox.Show("activate Guard Code before generate him");
            }
            else MessageBox.Show("Select account before generate Steam Guard code");
        }
    }


}
