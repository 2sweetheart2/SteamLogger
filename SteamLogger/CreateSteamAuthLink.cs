using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SteamAuth;
using Newtonsoft.Json;

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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
        public static string MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\SteamAuth";
        public static string UsersPath = MainPath + @"\users.json";
        public static string SecretstPath = MainPath + @"\SecretsUsers";
        public static string SecretUser = MainPath + @"\secrets.txt";
        public class User
        {
            public string name { get; set; }
            public string password { get; set; }
            public string link { get; set; }
        }
        public static string ListToJsonString(List<User> users)
        {
            var json = JsonConvert.SerializeObject(users);
            return json;
        }
        public List<User> StringToListUsers(string a)
        {
            List<User> obj = new List<User>();
            obj = JsonConvert.DeserializeObject<List<User>>(a);
            return obj;
        }
        private void v2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(SecretUser))
            {
                MessageBox.Show("Before usage second variant, check how to usage with on github\nLINK", "SteamAuth");
            }
            else
            {
                List<User> users = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
                int index = -1;
                for(int i = 0; i < users.Count; i++)
                {
                    if (users[i].name.Equals(login1))
                    {
                        index = i;
                    }
                }
                string[] lines = File.ReadAllLines(SecretUser);
                foreach (string line in lines)
                {
                    if (line.Split(" ")[0].Equals(login1))
                    {
                        if (index >= 0)
                        {
                            users[index].link = line.Split(" ")[1];
                            File.WriteAllText(UsersPath, ListToJsonString(users));
                            MessageBox.Show("complete", "SteamAuth");
                            File.Delete(SecretUser);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("account not found", "SteamAuth");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("account not found", "SteamAuth");
                        return;
                    }
                }
                this.Close();
            }
            Close();
        }

        private void Console() 
        {
            if (AllocConsole())
            {
                bool end = false;
                while (!end)
                {
                    System.Console.WriteLine("login with param: " + login1 + ", " + pass);
                    string username = login1;
                    string password = pass;
                    List<User> users = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
                    int index = users.IndexOf(new User() { name = login1, password = pass, link = "" });
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
                        string sgFile = JsonConvert.SerializeObject(linker.LinkedAccount, Formatting.Indented);
                        string fileName = SecretstPath + linker.LinkedAccount.AccountName + ".maFile";
                        if (!Directory.Exists(SecretstPath)) Directory.CreateDirectory(SecretstPath);
                        if (!File.Exists(fileName))
                        {
                            StreamWriter sw = File.CreateText(fileName);
                            sw.Flush();
                            sw.Dispose();
                        }
                        File.WriteAllText(fileName, sgFile);
                        users[index].link = linker.LinkedAccount.SharedSecret;
                        File.WriteAllText(UsersPath, ListToJsonString(users));
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
                    end = true;
                    this.Close();
                }
            }
            this.Close();
        }
        
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();
    }
}
