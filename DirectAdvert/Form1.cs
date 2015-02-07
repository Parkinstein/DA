using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectAdvert
{
    public partial class daForm : Form
    {
        public daForm()
        {
            InitializeComponent();

        }
        #region "Variables"
        public AutoCompleteStringCollection sourceLogin;
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();
        private static InputLanguageCollection _InstalledInputLanguages;
        private static int _ProcessId;
        private static string _CurrentInputLanguage;
        public string login_string;
        public string password_string;
        public string pathFile;
        public string fileData;
        static public string tokenvalue;
        static public string errorvalue;
        static public string errorcode;
        static public string balance;
        static public string email;
        static public string currency;
        static public bool accses;

        #endregion
        #region "Classes"
        public class Input : List<string>
        {
            public string login_in { get; set; }
            public string password_in { get; set; }
        }
        public class RootObject
        {
            public string token { get; set; }
            public AccountData account_data { get; set; }
            public List<AccountDetail> account_details { get; set; }
            public List<GroupTeaser> group_teasers { get; set; }
            public bool success { get; set; }
            public int error_code { get; set; }
            public string error_message { get; set; }
        }
        public class AccountData
        {
            public string email { get; set; }
            public string balance { get; set; }
            public string currency { get; set; }

        }
        public class AccountDetail
        {
            public int group_id { get; set; }
            public string title { get; set; }
            public object max_clicks_workday { get; set; }
            public object max_clicks_weekend { get; set; }
            public int? all_count { get; set; }
            public int? active_count { get; set; }
            public string status { get; set; }
            public bool success { get; set; }
        }
        public class GroupTeaser
        {
            public int ad_id { get; set; }
            public int group_id { get; set; }
            public string status { get; set; }
            public string title { get; set; }
            public string announce { get; set; }
            public int is_banner { get; set; }
            public string url { get; set; }
            public string image_url { get; set; }
            public string image_string { get; set; }
            public object action_url { get; set; }
            public double buy_price { get; set; }
            public int action_price { get; set; }
            public int action_price_fixed { get; set; }
            public int action_price_allow { get; set; }
        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectTab("Page1");
            tabControl1.Visible = false;
            loginPage.Visible = true;
            eyepassbox.Image = DirectAdvert.Properties.Resources.eye;
            loginBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            loginBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            pathFile = (@"common.dll");
            if (File.Exists(pathFile))
            {
                Console.WriteLine("File founded");
                fileData = System.IO.File.ReadAllText(pathFile);
            }
            else 
            {
                File.WriteAllText(pathFile, fileData);
            }
          
        }

        private void forgotPasslink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.directadvert.ru/password_reminder");
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (savePasswordchkbx.Checked == true) 
            { 
            
            }
        }
        private static string GetKeyboardLayoutId()
        {
 
            _InstalledInputLanguages = InputLanguage.InstalledInputLanguages;
            IntPtr hWnd = GetForegroundWindow();
            int WinThreadProcId = GetWindowThreadProcessId(hWnd, out _ProcessId);
            IntPtr KeybLayout = GetKeyboardLayout(WinThreadProcId);
            for (int i = 0; i < _InstalledInputLanguages.Count; i++)
                {
                    if (KeybLayout == _InstalledInputLanguages[i].Handle)
                    {
                        _CurrentInputLanguage = _InstalledInputLanguages[i].Culture.ThreeLetterWindowsLanguageName.ToString();
                    }       
                }
            return _CurrentInputLanguage;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = GetKeyboardLayoutId();
        }

        private void eyepassbox_MouseHover(object sender, EventArgs e)
        {
            eyepassbox.Image = DirectAdvert.Properties.Resources.eye1;
            passwordBox.UseSystemPasswordChar = false;

        }

        private void eyepassbox_MouseLeave(object sender, EventArgs e)
        {
            eyepassbox.Image = DirectAdvert.Properties.Resources.eye;
            passwordBox.UseSystemPasswordChar = true;

        }

    }
}
        