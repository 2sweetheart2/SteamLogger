using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SteamAuth;
using System.Linq;
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
        public static string UsersPath = MainPath + @"\users.txt";
        public static string SecretstPath = MainPath + @"\SecretsUsers";
        public static string SecretUser = MainPath + @"\secrets.txt";
        public class User
        {
            public string name { get; set; }
            public string password { get; set; }
            public string link { get; set; }
        }
        public static string parseToJson(string v1, string v2, string v3)
        {
            string json = System.Text.Json.JsonSerializer.Serialize<User>(new User() { name = v1, password = v2, link = v3 });
            return json;
        }
        public static User JsonToUser(string json)
        {
            User user = System.Text.Json.JsonSerializer.Deserialize<User>(json);
            return user;
        }
        private void v2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(SecretUser))
            {
                MessageBox.Show("Before usage second variant, check how to usage with on github\nLINK", "SteamAuth");
            }
            else
            {
                List<string> lines = new List<string>();
                User target = new User() { };
                foreach(string line in File.ReadAllLines(UsersPath))
                {
                    lines.Add(line);
                    if (JsonToUser(line).name.Equals(login1)) target = JsonToUser(line);
                }
                foreach(string line in File.ReadAllLines(SecretUser))
                {
                    if(line.Split(" ")[0].Equals(login1))
                    {
                        int index = lines.IndexOf(parseToJson(target.name, target.password, target.link));
                        lines[index] = parseToJson(target.name, target.password, line.Split(" ")[1]);
                        File.WriteAllLines(UsersPath, lines);
                        return;
                    }

                }
                MessageBox.Show("succes", "SteamAuth");
                this.Close();
            }
            Close();
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
                    string[] lines = File.ReadAllLines(UsersPath);
                    int index = Array.IndexOf(lines, parseToJson(login1, pass, ""));
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
                        lines[index] = parseToJson(login1,pass, linker.LinkedAccount.SharedSecret);
                        File.WriteAllLines(UsersPath, lines);
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
