using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteamAuth;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;
using System.Reflection;

namespace SteamLogger
{

    public partial class MainForm : Form
    {
        public class User
        {
            public string name { get; set; }
            public string password { get; set; }
            public string link { get; set; }
        }

        User AutoStartUser = null;
        bool closeAfterEnter = false;
        public static string MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal)+@"\SteamAuth";
        public static string UsersPath = MainPath + @"\users.json";
        public static string SecretstPath = MainPath + @"\SecretsUsers";
        public static string SettingsPath = MainPath + @"\settings.txt";
        public MainForm(String[] args)
        {
            
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            if( args != null && args.Length > 0)
            {
                
                if (args[0].Equals("-login"))
                {
                    AutoStartUser = new User();
                    if (args[1].Length <= 0)
                    {
                        MessageBox.Show("uncorrect start params!", "SteamAuth");
                        return;
                    }
                    AutoStartUser.name = args[1];
                    if(args.Length>=3) closeAfterEnter = Boolean.Parse(args[2]);
                }
                MessageBox.Show(String.Format("Start account with login={0}",args[1]), "SteamAuth");
                
            }
            selectAccount = new SelectAccount(this);
            
        }


        public List<User> users = new List<User>();

        public void removeUser(int index)
        {
            users.RemoveAt(index);
            File.WriteAllText(UsersPath, ListToJsonString(users));
        }


        public void addUser(string login, string passwrod)
        {
            User user = new User() { name = login, password = passwrod, link = "" };
            AddUserToFile(user);
            users.Add(user);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(MainPath)) Directory.CreateDirectory(MainPath);
/*            getSettings();

            if (auto_run)
            {
                string b = Environment.CurrentDirectory+@"\SteamLogger.exe";
                string a = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", "SteamLogger", "null");
                RegistryKey reg;
                reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                if (!a.Equals("null"))
                {
                    reg.DeleteValue("SteamLogger");

                }
                reg.SetValue("SteamLogger", b);
                reg.Close();

            }
            else
            {
                string a = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", "SteamLogger", "null");
                if (!a.Equals("null"))
                {
                    RegistryKey reg;
                    reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                    reg.DeleteValue("SteamLogger");
                    reg.Close();
                }
            }*/  
            if (!Directory.Exists(SecretstPath)) Directory.CreateDirectory(SecretstPath);
            if (!File.Exists(UsersPath))
            {
                StreamWriter sw = File.CreateText(UsersPath);
                sw.Flush();
                sw.Dispose();
            }
            OpenFileAndRead();
            if (AutoStartUser != null)
            {
                foreach (User user in users)
                {
                    if (user.name.Equals(AutoStartUser.name))
                    {
                        LoginAccount(user.name, user.password, user.link);
                    }
                }
            }
        }

        private void ActivateSteamGuard_Click(object sender, EventArgs e)
        {
/*            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.link.Length <= 0)
                {
                    CreateSteamAuthLink newfrm = new CreateSteamAuthLink(user.name, user.password);
                    newfrm.Show();
                }
                else MessageBox.Show("Steam Guard active for this account", "SteamAuth");
            }
       */     MessageBox.Show("Select aacount before activate Steam Guard");
        }


        public static Process WaitForSteamProcess(IntPtr hwnd)
        {
            Process process = null;
            while (process == null)
            {
                int procId = 0;
                GetWindowThreadProcessId(hwnd, out procId);

                // Wait for valid process id from handle.
                while (procId == 0)
                {
                    Thread.Sleep(100);
                    GetWindowThreadProcessId(hwnd, out procId);
                }

                try
                {
                    process = Process.GetProcessById(procId);
                }
                catch
                {
                    process = null;
                }
            }

            return process;
        }

        public async void LoginAccount(string login, string pass, string steamGuadLink)
        {
            foreach (var process in Process.GetProcessesByName("steam"))
            {
                process.Kill();
            }
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", "null");
            startInfo.Arguments = " -login \"" + login + "\" \"" + pass + "\"";
            Process.Start(startInfo);
            if (steamGuadLink.Length > 0)
            {
                SteamGuardAccount steamGuard = new SteamGuardAccount();
                steamGuard.SharedSecret = steamGuadLink;
                var t = Task.Run(() => PutSteamGuardCode(steamGuard,true));
                t.Wait();
                if (closeAfterEnter) Close();
            }

        }
        private async void PutSteamGuardCode(SteamGuardAccount steamGuard,bool wait)
        {
            IntPtr steamGuardWindow = GetSteamGuardWindow();
            while (steamGuardWindow.Equals(IntPtr.Zero))
            {
                Thread.Sleep(100);
                steamGuardWindow = GetSteamGuardWindow();
            }
            Process steamGuardProcess = WaitForSteamProcess(steamGuardWindow);
            steamGuardProcess.WaitForInputIdle();
            if(wait) Thread.Sleep(2000);
            foreach (char c in steamGuard.GenerateSteamGuardCode().ToCharArray())
            {
                SendKey(steamGuardWindow, c);
            }
            SendEnter(steamGuardWindow);
            
        }


        public void generateGuardCode(User user)
        {
                if (user.link.Length > 0)
                {
                    var steamGuard = new SteamGuardAccount();
                    steamGuard.SharedSecret = user.link;
                    Task.Run(() => PutSteamGuardCode(steamGuard, false));
                    MessageBox.Show(steamGuard.GenerateSteamGuardCode(), "Steam Guard code");
                }
                else MessageBox.Show("activate Guard Code before generate him");
        }

        const uint WM_KEYDOWN = 0x0100;
        const uint WM_KEYUP = 0x0101;
        const uint WM_CHAR = 0x0102;
        const int VK_RETURN = 0x0D;

        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowProc callback, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool SendMessage(IntPtr hWnd, uint Msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern bool GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        private static bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder ClassNameSb = new StringBuilder(256);
            GetClassName(hWnd, ClassNameSb, ClassNameSb.Capacity + 1);
            string className = ClassNameSb.ToString();
            StringBuilder windowTextSb = new StringBuilder(256);
            GetWindowText(hWnd, windowTextSb, windowTextSb.Capacity + 1);
            string windowText = windowTextSb.ToString();
            if (className.Equals("vguiPopupWindow") &&
                (windowText.StartsWith("Steam Guard") ||
                windowText.StartsWith("Steam 令牌") ||
                windowText.StartsWith("Steam ガード")))
            {
                GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

                List<IntPtr> validHandles = gcChildhandlesList.Target as List<IntPtr>;
                validHandles.Add(hWnd);
                return false;
            }
            return true;
        }
        public static IntPtr GetSteamGuardWindow()
        {
            List<IntPtr> validHandles = new List<IntPtr>();

            GCHandle gcValidHandlesList = GCHandle.Alloc(validHandles);
            IntPtr pointerValidHandlesList = GCHandle.ToIntPtr(gcValidHandlesList);

            try
            {
                EnumWindowProc validProc = new EnumWindowProc(EnumWindow);
                EnumWindows(validProc, pointerValidHandlesList);
            }
            finally
            {
                gcValidHandlesList.Free();
            }

            if (validHandles.Count > 0)
            {
                return validHandles[0];
            }
            return IntPtr.Zero;
        }

        private static void SendKey(IntPtr hwnd, char c)
        {
            SetForegroundWindow(hwnd);
            Thread.Sleep(10);
            SendMessage(hwnd, WM_CHAR, c, IntPtr.Zero);
        }

        private static void SendEnter(IntPtr hwnd)
        {
            SendMessage(hwnd, WM_KEYDOWN, VK_RETURN, IntPtr.Zero);
            SendMessage(hwnd, WM_KEYUP, VK_RETURN, IntPtr.Zero);
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
        public void AddUserToFile(User user)
        {
            List<User> usersList = new List<User>();
            if (File.ReadAllText(UsersPath).Length > 0)
            {
                usersList = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
            }
            usersList.Add(user);
            File.WriteAllText(UsersPath, ListToJsonString(usersList));
        }
        public void EditUser(string oldname,string name, string pass)
        {
            List<User> usersList = new List<User>();
            if (File.ReadAllText(UsersPath).Length > 0)
            {
                usersList = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
            }
            foreach(User user in usersList)
            {
                if (user.name.Equals(oldname))
                {
                    user.name = name;
                    user.password = pass;
                    users = usersList;
                    continue;
                }
            }
            selectAccount.UpdateUsers();
            File.WriteAllText(UsersPath, ListToJsonString(usersList));
        }
        public void OpenFileAndRead()
        {
            if (File.ReadAllText(UsersPath).Length > 0)
            {
                users = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
            }
        }


        public void EditUser2(User user)
        {
            bool hasSteamGuard = false;
            if (user.link.Length > 0) hasSteamGuard = true;
            editAccount eda = new editAccount(this, user.name, user.password,hasSteamGuard);
            eda.loginBox.Text = user.name;
            eda.passBox.Text = user.password;
            eda.Show();
        }
        SelectAccount selectAccount;
        private void LoadAccountMenu_Click(object sender, EventArgs e)
        {
            selectAccount = new SelectAccount(this);
            OpenChildForm(selectAccount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AddAccount(this));
        }
        private Form activeForm = null;
        private void OpenChildForm(Form ChiledForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = ChiledForm;
            ChiledForm.TopLevel = false;
            ChiledForm.FormBorderStyle = FormBorderStyle.None;
            ChiledForm.Dock = DockStyle.Fill;
            panelChiled.Controls.Add(ChiledForm);
            panelChiled.Tag = ChiledForm;
            ChiledForm.BringToFront();
            ChiledForm.Show();
        }


        public List<User> getUsers()
        {
            return users;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {"https://vk.com/id532148734"}") { CreateNoWindow = true });
        }

        int r = 0;
        int g = 0;
        int b = 0;
        bool reverse = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!reverse) plus();
            else minus();
            linkLabel1.LinkColor = Color.FromArgb(r, g, b);
            linkLabel2.LinkColor = Color.FromArgb(g, b, r);
        }

        public void plus()
        {
            
            if (r != 255) r++;
            else
            {
                if (g != 255) g++;
                else
                {
                    if (b != 255) b++;
                    else reverse = true;
                }

            }
        }
        public void minus()
        {
            if (r != 0) r--;
            else
            {
                if (g != 0) g--;
                else
                {
                    if (b != 0) b--;
                    else reverse = false;
                }
            }
           
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {"https://github.com/2sweetheart2"}") { CreateNoWindow = true });
        }
    }


}
