using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SteamLogger
{
    public partial class CreateSteamAuthLink : Form
    {
        public CreateSteamAuthLink()
        {
            InitializeComponent();
        }
        private void CreateSteamAuthLink_Load(object sender, EventArgs e)
        {

        }


        private void ENAddition_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A new mobile autifier is being created on your computer\n(You can no longer create a new one)", "SteamAuth");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The computer copies the data from the Steam application from your phone \nrecompiles it and installs it back\n(SteamGuard remains on two devices)", "SteamAuth");
        }

        private void v1_Click(object sender, EventArgs e)
        {

        }

        private void v2_Click(object sender, EventArgs e)
        {

        }
    }
}
