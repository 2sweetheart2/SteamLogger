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

        
        public static string MainPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal)+@"\SteamAuth";
        public static string UsersPath = MainPath + @"\users.json";
        public static string SecretstPath = MainPath + @"\SecretsUsers";
        public static string SettingsPath = MainPath + @"\settings.txt";
        public MainForm()
        {
            
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
        }


        public List<User> users = new List<User>();
        private void load_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                LoginAccount(user.name, user.password, user.link);
            }
            else MessageBox.Show("select account before load him", "SteamAuth");

        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                int index = comboBox1.SelectedIndex;
                comboBox1.Items.RemoveAt(index);
                comboBox1.Text = "";
                users.RemoveAt(index);
                File.WriteAllText(UsersPath, ListToJsonString(users));
                
            }
            else MessageBox.Show("select account before delete him", "SteamAuth");
        }

        private void add_Click(object sender, EventArgs e)
        {
            if (loginBox.Text.Length > 0 && passwordBox.Text.Length > 0)
            {
                User user = new User() { name = loginBox.Text, password = passwordBox.Text, link = "" };
                comboBox1.Items.Add(loginBox.Text);
                AddUserToFile(user);
                users.Add(user);
                loginBox.Text = "";
                passwordBox.Text = "";
            }
        }
        bool auto_run = false;
        bool roll_up_after_run = false;
        bool close_after_run = false;
        int auto_start_up_index = -1;
        private void getSettings()
        {
            if (!File.Exists(SettingsPath))
            {
                StreamWriter sw = File.CreateText(UsersPath);
                sw.Flush();
                sw.Dispose();
                List<string> lines = new List<string>() {"auto_run=false", "roll_up_after_run=false","close_after_run=false" };
                File.WriteAllLines(SettingsPath, lines);
                auto_run = false;
                roll_up_after_run = false;
                close_after_run = false;
                auto_start_up_index = -1;
            }
            else
            {
                string[] lines = File.ReadAllLines(SettingsPath);
                auto_run = bool.Parse(lines[0].Split("=")[1]);
                if (!auto_run)
                {
                    roll_up_after_run = bool.Parse(lines[1].Split("=")[1]);
                    close_after_run = bool.Parse(lines[2].Split("=")[1]);
                    auto_start_up_index = int.Parse(lines[3].Split("=")[1]);
                }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            if (!Directory.Exists(MainPath)) Directory.CreateDirectory(MainPath);
            getSettings();

            if (auto_run)
            {
                string b = Environment.CurrentDirectory+@"\SteamLogger.exe";
                String fullPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string a = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run\", "SteamLogger", "null");
                if (a.Equals("null"))
                {
                    RegistryKey reg;
                    reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                    reg.SetValue("SteamLogger", b);
                    reg.Close();
                }
                
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
            }  
            if (!Directory.Exists(SecretstPath)) Directory.CreateDirectory(SecretstPath);
            if (!File.Exists(UsersPath))
            {
                StreamWriter sw = File.CreateText(UsersPath);
                sw.Flush();
                sw.Dispose();
            }
            OpenFileAndRead();
            if (users.Count > 0) comboBox1.SelectedIndex = 0;
            if (auto_run && auto_start_up_index >= 0)
            {
                if (users.Count - 1 < auto_start_up_index)
                {
                    User user = users[auto_start_up_index];
                    LoginAccount(user.name, user.password, user.link);
                }
            }
        }

        private void ActivateSteamGuard_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.link.Length <= 0)
                {
                    CreateSteamAuthLink newfrm = new CreateSteamAuthLink(user.name, user.password);
                    newfrm.Show();
                }
                else MessageBox.Show("Steam Guard active for this account", "SteamAuth");
            }
            else MessageBox.Show("Select aacount before activate Steam Guard");
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
            if (user.link.Length > 0)
            {
                SteamGuardAccount steamGuard = new SteamGuardAccount();
                steamGuard.SharedSecret = user.link;
                Task.Run(() => PutSteamGuardCode(steamGuard,true));
            }

        }
        void PutSteamGuardCode(SteamGuardAccount steamGuard,bool wait)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.link.Length > 0)
                {
                    SteamGuardText2.ForeColor = Color.Green;
                    SteamGuardText2.Text = "ENABLE";
                }
                else
                {
                    SteamGuardText2.ForeColor = Color.Red;
                    SteamGuardText2.Text = "DISABLE";
                }
            }
            else SteamGuardText2.Text = "";
        }

        private void generateGuard_Click(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex >= 0)
            {
                User user = users[comboBox1.SelectedIndex];
                if (user.link.Length > 0)
                {
                    var steamGuard = new SteamGuardAccount();
                    steamGuard.SharedSecret = user.link;
                    Task.Run(() => PutSteamGuardCode(steamGuard, false));
                    MessageBox.Show(steamGuard.GenerateSteamGuardCode(), "Steam Guard code");
                }
                else MessageBox.Show("activate Guard Code before generate him");
            }
            else MessageBox.Show("Select account before generate Steam Guard code");
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
        public void OpenFileAndRead()
        {
            if (File.ReadAllText(UsersPath).Length > 0)
            {
                users = StringToListUsers(File.ReadAllLines(UsersPath)[0]);
                foreach (User user in users)
                {
                    comboBox1.Items.Add(user.name);
                }

            }
        }

        private void settingsBut_Click(object sender, EventArgs e)
        {
            Settings setting = new Settings();
            setting.auto_run = auto_run;
            setting.rool_up = roll_up_after_run;
            setting.close_after = close_after_run;
            setting.path = SettingsPath;
            setting.users = users;
            setting.Show();
        }
        bool use = false;
        private void timer1_Tick(object sender, EventArgs e)
        {


        }
    }


}
