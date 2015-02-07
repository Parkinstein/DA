using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;

namespace DirectAdvert
{
    public partial class daForm : Form
    {
        public daForm()
        {
            InitializeComponent();

        }
        alert alert;
        #region "Variables"
        public AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();
        private static InputLanguageCollection _InstalledInputLanguages;
        public static List<Input> datalog;
        public List<string> logins;
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
        public string decrypted;

        #endregion
        #region "Classes"
        public class Input : IEquatable<Input>//List<string>
        {
            public string login_in { get; set; }
            public string password_in { get; set; }
            public int notused { get; set; }
            public override string ToString()
            {
                return "login: " + login_in + "   password: " + password_in;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Input objAsPart = obj as Input;
                if (objAsPart == null) return false;
                else return Equals(objAsPart);
            }
            public override int GetHashCode()
            {
                return notused;
            }
            public bool Equals(Input other)
            {
                if (other == null) return false;
                return (this.notused.Equals(other.notused));
            }
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
            loginButton.Enabled = false;
            eyepassbox.Image = DirectAdvert.Properties.Resources.eye;
            loginBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            loginBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            Form alert = new alert();
            #region "Проверка на сохраненные логины-пароли"
            pathFile = (@"common.dll");
            #region "Есть файл"
            if (File.Exists(pathFile))
            {
                Console.WriteLine("File founded");
                fileData = System.IO.File.ReadAllText(pathFile);
                if (fileData != "")
                {
                    try
                    {
                        string entropy = null;
                        string description = "<<<>>>";
                        decrypted = DPAPI.Decrypt(fileData, entropy, out description);
                        Console.WriteLine(decrypted + " - decrypted");
                    }
                    #region"Ошибка декодирования"
                    catch (Exception ex)
                    {
                        while (ex != null)
                        {
                            Console.WriteLine(ex.Message);
                            ex = ex.InnerException;
                        }
                    }
                    #endregion
                    datalog = JsonConvert.DeserializeObject<List<Input>>(decrypted)
                                                              ?? new List<Input>();
                    logins = new List<string>();
                    foreach (var stroke in datalog)
                    {
                        Console.WriteLine(stroke.login_in.ToString());
                        logins.Add(stroke.login_in.ToString());
                    }
                    colValues.AddRange(logins.ToArray());
                    loginBox.AutoCompleteCustomSource = colValues;
                    
                }
                else //Пустой файл
                {
                    passwordBox.Text = ""; Console.WriteLine("В файле пусто");
                }
            }
            #endregion
            #region"Нет файла"
            else //Нет файла
            {
                File.WriteAllText(pathFile, ""); Console.WriteLine("Файла не было, создали");
            }
            #endregion
            #endregion
        }
        

        private void forgotPasslink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.directadvert.ru/password_reminder");
        }
        private void loginBox_TextChanged(object sender, EventArgs e)
        {
            login_string = loginBox.Text;
            RegexUtilities util = new RegexUtilities();
            if (util.IsValidEmail(loginBox.Text))
            {
                loginBox.BackColor = Color.Green;
                loginBox.ForeColor = Color.White;
            }
            else
            {
                loginBox.BackColor = Color.Red;
                loginBox.ForeColor = Color.White;
            }
            if (loginBox.Text.Length > 2 && datalog != null)
            {
                int i = datalog.FindIndex(s => s.login_in == loginBox.Text);
                Console.WriteLine(i);
                string newitem;
                
                if (i >= 0)
                {
                    newitem = datalog.ElementAt(i).password_in.ToString();
                    passwordBox.Text = newitem;
                    for (int x = 0; x > datalog.Count; x++)
                    {
                        Console.WriteLine(datalog.ElementAt(x).login_in.ToString());
                        Console.WriteLine(datalog.ElementAt(x).password_in.ToString());
                    }
                }
                else { passwordBox.Text = ""; Console.WriteLine("Совпадений не найдено"); }
            }
        }
        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            password_string = passwordBox.Text;
            RegexUtilities util = new RegexUtilities();
            if (util.IsValidEmail(loginBox.Text)&&passwordBox.Text.Length >= 6)
                loginButton.Enabled = true;
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            if (savePasswordchkbx.Checked == true)
            {
                if (decrypted != null)
                {
                    datalog = JsonConvert.DeserializeObject<List<Input>>(decrypted) ?? new List<Input>();
                    if (datalog.Exists(x => x.login_in == loginBox.Text && datalog.Exists(y => y.password_in != passwordBox.Text)))
                    {
                        alert.Show(Form.ActiveForm);
                        //int a = datalog.FindIndex(s => s.login_in == loginBox.Text);
                        //datalog.RemoveAt(a);
                        //datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                        //string entropy = null;
                        //string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                        //File.WriteAllText(pathFile, encrypted);
                    
                    }
                    if (datalog.Exists(x => x.login_in == loginBox.Text && datalog.Exists(y => y.password_in == passwordBox.Text)))
                    { Console.WriteLine("Запись уже существует"); }
                    else
                    {
                        datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                        string entropy = null;
                        string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                        File.WriteAllText(pathFile, encrypted);
                    }
                }
                else
                {
                    datalog = new List<Input>(); // JsonConvert.DeserializeObject<List<Input>>(decrypted) ?? 
                    datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                    string entropy = null;
                    string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                    File.WriteAllText(pathFile, encrypted);
                    
                }
            }
            else 
            {
                if (decrypted != null)
                {
                    datalog = JsonConvert.DeserializeObject<List<Input>>(decrypted) ?? new List<Input>();
                    if (datalog.Exists(x => x.login_in == loginBox.Text))
                    { Console.WriteLine("Запись уже существует"); }
                    else
                    {
                        datalog.Add(new Input() { login_in = login_string, password_in = null }); decrypted = JsonConvert.SerializeObject(datalog);
                        string entropy = null;
                        string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                        File.WriteAllText(pathFile, encrypted);
                    }
                }
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
            if (GetKeyboardLayoutId() == "ENU")
                flagBox.Image = DirectAdvert.Properties.Resources.reino_unido;
            if (GetKeyboardLayoutId() == "RUS")
                flagBox.Image = DirectAdvert.Properties.Resources.russia;
    
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

        private void loginBox_Leave(object sender, EventArgs e)
        {
            RegexUtilities util = new RegexUtilities();
            if (util.IsValidEmail(loginBox.Text))
                Console.WriteLine("Введен email");
            else
                //toolTip1.SetToolTip(loginBox, "...");
            toolTip1.Show("Проверьте правильность ввода логина",loginBox,3000);
                Console.WriteLine("Ни фига не Email");
        }





    }
}
        