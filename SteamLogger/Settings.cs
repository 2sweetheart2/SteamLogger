using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        public bool auto_run = false;
        public bool rool_up = false;
        public bool close_after = false;
        public string path;
        internal List<MainForm.User> users;

        private void Settings_Load(object sender, EventArgs e)
        {
            autoRun.Checked = auto_run;
            if (!auto_run)
            {
                Color c = Color.FromArgb(117, 117, 117);
                roll_up_after_run.BackColor = c;
                close_after_run.BackColor = c;
            }
            else
            {
                roll_up_after_run.Checked = rool_up;
                close_after_run.Checked = close_after;
            }
            for (int i = 0; i < users.Count; i++)
            {
                comboBox1.Items.Add(users[i].name);
            }
        }

        private void roll_up_after_run_CheckedChanged(object sender, EventArgs e)
        {
            if (!auto_run)
            {
                roll_up_after_run.Checked = false;
                roll_up_after_run.BackColor = Color.FromArgb(117, 117, 117);
            }
            else
            {
                rool_up = roll_up_after_run.Checked;
                roll_up_after_run.BackColor = Color.Transparent; 
            }
        }

        private void close_after_run_CheckedChanged(object sender, EventArgs e)
        {
            if (!auto_run)
            {
                close_after_run.Checked = false;
                close_after_run.BackColor = Color.FromArgb(117, 117, 117);
            }
            else
            {
                close_after = close_after_run.Checked;
                close_after_run.BackColor = Color.Transparent;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = File.ReadAllLines(path);
            lines[0] = "auto_run=" + auto_run;
            lines[1] = "roll_up_after_run=" + rool_up;
            lines[2] = "close_after_run=" + close_after;
            lines[3] = "auto_start_up_index=" + comboBox1.SelectedIndex;
            File.WriteAllLines(path, lines);
            MessageBox.Show("Saved", "SteamAuth");
            this.Close();
        }

        private void autoRun_CheckedChanged(object sender, EventArgs e)
        {
            auto_run = autoRun.Checked;
            if (autoRun.Checked)
            {
                close_after_run.BackColor = Color.Transparent;
                roll_up_after_run.BackColor = Color.Transparent;
            }
            else
            {
                close_after_run.BackColor = Color.FromArgb(117, 117, 117);
                roll_up_after_run.BackColor = Color.FromArgb(117, 117, 117);
            }
        }
    }
}
