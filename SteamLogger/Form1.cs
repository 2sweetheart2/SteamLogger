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
            leftBorderPanel = new Panel();
            leftBorderPanel.Size = new Size(5, 37);
            PanelControls.Controls.Add(leftBorderPanel);
            LoadBtnLeftBorder.BackColor = Color.FromArgb(0, 66, 49, 137);
            AddBtnLeftBorder.BackColor = Color.FromArgb(0, 66, 49, 137);
        }

        internal void confirm_store(User user)
        {
            throw new NotImplementedException();
        }

        private Panel leftBorderPanel;

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
                var t =Task.Run(()=> PutSteamGuardCode(steamGuard,true));
                t.Wait();
                if (closeAfterEnter) Close();
            }

        }
        private async void PutSteamGuardCode(SteamGuardAccount steamGuard,bool wait)
        {
            await Task.Run(() =>
            {
                IntPtr steamGuardWindow = GetSteamGuardWindow();
                while (steamGuardWindow.Equals(IntPtr.Zero))
                {
                    Thread.Sleep(100);
                    steamGuardWindow = GetSteamGuardWindow();
                }

                Process steamGuardProcess = WaitForSteamProcess(steamGuardWindow);
                steamGuardProcess.WaitForInputIdle();
                

                DialogResult vibor2 = MessageBox.Show("Вставить код SteamGuard?\nInsert SteamGuard code?", "SteamAuth", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (vibor2 == DialogResult.Yes)
                {
                    RECT rt = new RECT();
                    SetForegroundWindow(steamGuardWindow);
                    GetWindowRect(steamGuardWindow,out rt);
                    SetCursorPos(rt.Left + 45, rt.Top + 45);
                    mouse_event(0x00000002, rt.Left + 45, rt.Top + 45,0,0);
                    mouse_event(0x00000004, rt.Left + 45, rt.Top + 45, 0, 0);
                    SendTab(steamGuardWindow);
                    foreach (char c in steamGuard.GenerateSteamGuardCode().ToCharArray())
                    {
                        SendKey(steamGuardWindow, c);
                    }
                    SendEnter(steamGuardWindow);
                }

            });

        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
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
                (windowText.Contains("Steam") && !windowText.Equals("Steam Client WebHelper")))
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

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
        private static void SendTab(IntPtr hwnd)
        {

            SetFocus(hwnd);
            SetForegroundWindow(hwnd);
            Thread.Sleep(50);
            SendKeys.SendWait("{TAB}");
            Thread.Sleep(50);
            SendMessage(hwnd, (uint)System.Windows.Forms.Keys.Tab, 0, IntPtr.Zero);

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

        public void ConfirmStore(User user)
        {
            var steamGuard = new SteamGuardAccount();
            steamGuard.SharedSecret = user.link;
            Confirmation[] conf = steamGuard.FetchConfirmations();
            steamGuard.AcceptMultipleConfirmations(conf);
            
            MessageBox.Show("All confirmations allow", "Steam Guard code");

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
        Panel saveStatePanel = new Panel();
        Color mainColor = Color.FromArgb(0, 66, 49, 137);
        private void LoadAccountMenu_Click(object sender, EventArgs e)
        {
            if (saveStatePanel.Name.Equals(AddBtnLeftBorder.Name)) AddBtnLeftBorder.BackColor = Color.FromArgb(0, mainColor);

            saveStatePanel = LoadBtnLeftBorder;
            activateStateForButton((Button)sender, LoadBtnLeftBorder);
            selectAccount = new SelectAccount(this);
            OpenChildForm(selectAccount);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveStatePanel.Name.Equals(LoadBtnLeftBorder.Name)) LoadBtnLeftBorder.BackColor = Color.FromArgb(0, mainColor);

            saveStatePanel = AddBtnLeftBorder;
            activateStateForButton((Button)sender, AddBtnLeftBorder);
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
        int g = 255;
        int b = 100;
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
        private void activateStateForButton(Button button, Panel panel)
        {
            panel.BackColor = Color.DarkSlateBlue;
            panel.Location = new Point(0, button.Location.Y);
            panel.Visible = true;
            panel.BringToFront();
        }
        private void DisableStateForButton(Panel panel)
        {
            panel.Visible = false;
        }
        public bool onLoadBtn = false;
        public bool onAddBtn = false;
        private async void ChangedStateLoadBorder(Panel panel,bool minus)
        {
            if(saveStatePanel!=null)
            {
                if (saveStatePanel.Name.Equals(panel.Name)){
                    panel.BackColor = Color.FromArgb(255, mainColor);
                    return;
                }
            }
            await Task.Run(() =>
            {
                int a = 0;
                if (minus) a = panel.BackColor.A;
                if (!minus)
                {
                    while (a < 255)
                    {
                        if (!onLoadBtn) break;
                        panel.BackColor = Color.FromArgb(a,mainColor);
                        a += 15;
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    while (a > 0)
                    {
                        if (onLoadBtn) break;
                        panel.BackColor = Color.FromArgb(a, mainColor);
                        a -= 15;
                        Thread.Sleep(1);
                    }
                }
            });
        }

        private async void ChangedStateAddBorder(Panel panel, bool minus)
        {
            if (saveStatePanel != null)
            {
                if (saveStatePanel.Name.Equals(panel.Name))
                {
                    panel.BackColor = Color.FromArgb(255, mainColor);
                    return;
                }
            }
            await Task.Run(() =>
            {
                int a = 0;
                if (minus) a = panel.BackColor.A;
                if (!minus)
                {
                    while (a < 255)
                    {
                        if (!onAddBtn) break;
                        panel.BackColor = Color.FromArgb(a, mainColor);
                        a += 15;
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    while (a > 0)
                    {
                        if (onAddBtn) break;
                        panel.BackColor = Color.FromArgb(a, mainColor);
                        a -= 15;
                        Thread.Sleep(1);
                    }
                }
            });
        }

        private void LoadAccountMenu_MouseEnter(object sender, EventArgs e)
        {
            onLoadBtn = true;
            ChangedStateLoadBorder(LoadBtnLeftBorder,false) ;
        }

        private void LoadAccountMenu_MouseLeave(object sender, EventArgs e)
        {
            onLoadBtn = false;
            ChangedStateLoadBorder(LoadBtnLeftBorder, true);
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            onAddBtn = true;
            ChangedStateAddBorder(AddBtnLeftBorder, false);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            onAddBtn = false;
            ChangedStateAddBorder(AddBtnLeftBorder, true);
        }

    }


}
