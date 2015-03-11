using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DirectAdvert.Properties;
using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using Application = System.Windows.Forms.Application;
using Point = System.Drawing.Point;

namespace DirectAdvert
{
    public partial class daForm : Form
    {
        public daForm()
        {
            InitializeComponent();
            if(teaser_edit.edit_closing)
            { queryteasers();}
        }
        #region "Variables"
        public AutoCompleteStringCollection colValues = new AutoCompleteStringCollection();
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetKeyboardLayout(int WindowsThreadProcessID);
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr handleWindow, out int lpdwProcessID);
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass"), DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();
        private static InputLanguageCollection _InstalledInputLanguages;
        public static List<Input> datalog;
        public List<string> logins, lAnnounces, lBase64pict, lcommands, lpictures, lPrice, lTitles, lUrl;
        private static int _ProcessId;
        private static string _CurrentInputLanguage;
        static public string datenow,datePatt,login_string, password_string, cururlimg,tokenvalue, errorvalue, balance, email, currency, current_title, current_announce, current_url;
        public string textend1, textend2, pathFile, imagestring_pre, fileData, balanseRounded, decrypted,  new_FolderName, filename, filepath, foldername, imagestring, textend;
        static public int errorcode, current_group, teaser_to_edit, selected_group;
        public static double allcounts, max_teasers_end, max_teasers,balance_c, current_price;
        public static byte[] imagebytes;
        public string[] teaser_titlles;
        static public bool accses;
        public int folderid, folderid1, start_pos, selected_row, length_mass,kolvo_strok,today_cliks,today_shows,yesterday_shows,yesterday_clicks;
        public static RootObject userdataT,userdataU, userdataV, userdataW, userdataX, userdataY, userdataZ;
        public static object datasrc;
        public bool flag_create_NF, groupstatus;
        public StringBuilder ads_array;
        public static DateTime Date_end;
        public double today_ctr, yesterday_ctr;
        
        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                // add the drop shadow flag for automatically drawing
                // a drop shadow around the form
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion
        #region "Classes"
        public class Input : IEquatable<Input>
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
            public TeaserInfo teaser_info { get; set; }
            public bool success { get; set; }
            public int error_code { get; set; }
            public string error_message { get; set; }
            public int id { get; set; }
            public class AccountData
            {
                public string email { get; set; }
                public string balance { get; set; }
                public string currency { get; set; }
            }
            public class TeaserInfo
            {
                public int ad_id { get; set; }
                public string title { get; set; }
                public string url { get; set; }
                public string image_url { get; set; }
                public string status { get; set; }
                public Statistics statistics { get; set; }

                public class Statistics
                {
                    public Current current { get; set; }
                    public Delta delta { get; set; }
                    public class Current
                    {
                        public int shows { get; set; }
                        public int clicks { get; set; }
                        public int actions { get; set; }
                        public double expense { get; set; }
                        public double ctr { get; set; }
                        public double cpc { get; set; }
                    }
                    public class Delta
                    {
                        public int shows { get; set; }
                        public int clicks { get; set; }
                        public int actions { get; set; }
                        public double expense { get; set; }
                        public double ctr { get; set; }
                        public double cpc { get; set; }
                    }
                }
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
            
            
        }
  #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            tabControl1.SelectTab("Page1");
            start_pos = 17;
            tabControl1.Visible = false;
            flag_create_NF = false;
            ClientSize = new Size(307, 132);
            loginPage.Visible = true;
            loginButton.Enabled = false;
            pictureBox1.Visible = false;
            datePatt = @"yyyy-MM-dd";
            Date_end = DateTime.Now;
            datenow = Date_end.Date.ToString(datePatt);
            eyepassbox.Image = Resources.eye;
            loginBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            loginBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            comboBox2.Items.Add(5);
            comboBox2.Items.Add(10);
            comboBox2.Items.Add(15);
            comboBox2.Items.Add("Все");
            comboBox2.SelectedIndex=1;
            kolvo_strok = 10;

            #region "Проверка на сохраненные логины-пароли"
            pathFile = (@"common.dll");
            #region "Есть файл"
            if (File.Exists(pathFile))
            {
                Console.WriteLine("File founded");
                fileData = File.ReadAllText(pathFile);
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
            Process.Start("http://www.directadvert.ru/password_reminder");
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
                if (i >= 0 && datalog.ElementAt(i).password_in != "")
                {
                    newitem = datalog.ElementAt(i).password_in;
                    passwordBox.Text = newitem;
                    for (int x = 0; x > datalog.Count; x++)
                    {
                        Console.WriteLine(datalog.ElementAt(x).login_in);
                        Console.WriteLine(datalog.ElementAt(x).password_in);
                    }
                }
                else { passwordBox.Text = ""; Console.WriteLine("Совпадений не найдено"); }
            }
        }
        private void passwordBox_TextChanged(object sender, EventArgs e)
        {
            password_string = passwordBox.Text;
            if (GetKeyboardLayoutId() == "RUS") { toolTip1.Show("Включена русская раскладка, пароль может быть введен неверно", passwordBox, 3000); }
            RegexUtilities util = new RegexUtilities();
            if (util.IsValidEmail(loginBox.Text) && passwordBox.Text.Length >= 6)
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
                        alert frm = new alert();
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            int a = datalog.FindIndex(s => s.login_in == loginBox.Text);
                            datalog.RemoveAt(a);
                            datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                            string entropy = null;
                            string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                            File.WriteAllText(pathFile, encrypted);
                            frm.label1.Text = "Данные были перезаписаны";

                            querytoken();
                        }
                        if (frm.ShowDialog() == DialogResult.Cancel)
                        {
                            login_string = "";
                            password_string = "";
                        }
                    }
                    else if (datalog.Exists(x => x.login_in == loginBox.Text && datalog.Exists(y => y.password_in == passwordBox.Text)))
                    { Console.WriteLine("Запись уже существует"); querytoken(); }
                    else
                    {
                        datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                        string entropy = null;
                        string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                        File.WriteAllText(pathFile, encrypted);
                        querytoken();
                    }
                }
                else
                {
                    datalog = new List<Input>(); // JsonConvert.DeserializeObject<List<Input>>(decrypted) ?? 
                    datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                    string entropy = null;
                    string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                    File.WriteAllText(pathFile, encrypted);
                    querytoken();
                }
            }
            else
            {
                if (decrypted != null)
                {
                    datalog = JsonConvert.DeserializeObject<List<Input>>(decrypted) ?? new List<Input>();
                    if (datalog.Exists(x => x.login_in == loginBox.Text))
                    { Console.WriteLine("Запись уже существует"); querytoken(); }
                    else
                    {
                        datalog.Add(new Input() { login_in = login_string, password_in = "" }); decrypted = JsonConvert.SerializeObject(datalog);
                        string entropy = null;
                        string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                        File.WriteAllText(pathFile, encrypted);
                        querytoken();
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
                flagBox.Image = Resources.reino_unido;
            if (GetKeyboardLayoutId() == "RUS")
                flagBox.Image = Resources.russia;
        }
        private void eyepassbox_MouseHover(object sender, EventArgs e)
        {
            eyepassbox.Image = Resources.eye1;
            passwordBox.UseSystemPasswordChar = false;
        }
        private void eyepassbox_MouseLeave(object sender, EventArgs e)
        {
            eyepassbox.Image = Resources.eye;
            passwordBox.UseSystemPasswordChar = true;
        }
        private void loginBox_Leave(object sender, EventArgs e)
        {
            RegexUtilities util = new RegexUtilities();
            if (util.IsValidEmail(loginBox.Text))
                Console.WriteLine("Введен email");
            else
            {
                toolTip1.Show("Проверьте правильность ввода логина", loginBox, 3000);
                Console.WriteLine("Ни фига не Email");
            }
        }
        public void querytoken()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/auth.json?name=" + login_string + "&password=" + password_string);
            string reply = client.DownloadString(address);
            RootObject userdata = new RootObject();
            userdata = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdata.success)
            {
                tokenvalue = userdata.token;
                Console.WriteLine("отработала querytoken " + tokenvalue);

                currency = userdata.account_data.currency;
                if (currency == "RUR") { pictureBox1.Visible = true; pictureBox1.Image = Resources.russia; }
                label7.Text = userdata.account_data.email;
                int i = userdata.account_data.balance.IndexOf(".");
                string balanseRounded_pre = userdata.account_data.balance;
                balanseRounded = balanseRounded_pre.Remove(i + 3);
                label8.Text = balanseRounded;
                ClientSize = new Size(800, 600);
                loginPage.Visible = false;
                tabControl1.Visible = true;
                queryfolders();
            }
            else if (userdata.error_code == 1010) { MessageBox.Show(userdata.error_message.Replace("&nbsp;", " ")); }
        }
        public void queryfolders()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/get_account_details.json" + "?token=" + tokenvalue);
            string reply = client.DownloadString(address);
            userdataX = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataX.success == true)
            {
                Console.WriteLine("отработала queryfolders ");
                folderList.DataSource = userdataX.account_details;
                folderList.DisplayMember = "title";
                folderList.ValueMember = "group_id";
                folderid = Convert.ToInt32(folderList.SelectedValue);
                dataGridView2.DataSource = userdataX.account_details;
                allcounts = 0;
                for (int index = 0; index <= userdataX.account_details.Count - 1; index++)
                {
                    if (dataGridView2.Rows[index].Cells["all_count"].Value != null)
                    {
                        allcounts += Convert.ToDouble(dataGridView2["all_count", index].Value);
                        label9.Text = allcounts.ToString();
                    }
                }
                double activecounts = 0;
                for (int index = 0; index <= userdataX.account_details.Count - 1; index++)
                {
                    if (dataGridView2.Rows[index].Cells["active_count"].Value != null)
                    {
                        activecounts += Convert.ToDouble(dataGridView2["active_count", index].Value);
                        label10.Text = activecounts.ToString();
                    }
                }
                queryteasers();
                //queryteaserstat();
            }
        }
        public void queryteasers()
        {
            Text = "Получаем тизеры...";
            //WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/get_group_teasers.json" + "?token=" + tokenvalue + "&group_id=" + folderid);
            //string reply = client.DownloadString(address);
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(wc_DownloadStringCompleted);
            wc.DownloadStringAsync(new Uri(address));
            
            Text = "Direct/Advert";
        }
        void wc_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            string reply = e.Result;
            userdataY = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataY.success == true)
            {
                Console.WriteLine("отработала queryteasers ");
                radGridView1.DataSource = userdataY.group_teasers;
                radGridView1.Visible = true;
                radGridView1.Columns[0].HeaderText = "ID объявления";
                radGridView1.Columns[1].HeaderText = "ID группы";
                radGridView1.Columns[2].HeaderText = "Статус";
                radGridView1.Columns[3].HeaderText = "Название тизера";
                radGridView1.Columns[4].HeaderText = "Текст";
                radGridView1.Columns[5].IsVisible = false;
                radGridView1.Columns[6].HeaderText = "URL";
                radGridView1.Columns[7].IsVisible = false;
                radGridView1.Columns[8].IsVisible = false;
                radGridView1.Columns[9].IsVisible = false;
                radGridView1.Columns[10].HeaderText = "Цена";
                radGridView1.Columns[11].IsVisible = false;
                radGridView1.Columns[12].IsVisible = false;
                radGridView1.Columns[13].IsVisible = false;
            }
            else if (userdataY.error_code == 1016)
            {
                pictureBox3.Image = null;
                priceText.Text = "";
                urlText.Text = "";
                statusBox.Text = "";
                teaserHead.Text = "";
                teaserDescript.Text = "";
            }
        }

        public void group_add()
        {
            WebClient client = new WebClient();
            new_FolderName = new_group_name.Text;
            string new_folder = "&name=" + Uri.EscapeDataString(new_FolderName);
            string address = ("https://api.directadvert.ru/create_ad_group.json?token=" + tokenvalue + new_folder);
            string reply = client.DownloadString(address);
            RootObject userdataZ = new RootObject();
            userdataZ = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataZ.success == true)
            {
                folderid = userdataZ.id;
                group_pause();
                queryfolders();
                folderList.Refresh();
                comboBox1.Refresh();
                int a = folderList.FindString(new_FolderName);
                folderList.SelectedIndex = a;
                Console.WriteLine("отработала group_add ");
            }
            else if (userdataZ.error_code == 1010) { MessageBox.Show(userdataZ.error_message.Replace("&nbsp;", " ")); }
        }
        public void group_pause()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/set_groups_status.json?token=" + tokenvalue + "&groups_id[]=" + folderid.ToString() + "&status=paused");
            string reply = client.DownloadString(address);
            RootObject userdataV = new RootObject();
            userdataV = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataV.success == true)
            {
                label18.Text = "Возобновить";
                Console.WriteLine("Группа встала на паузу");
                queryfolders();
            }
        }
        public void group_start()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/set_groups_status.json?token=" + tokenvalue + "&groups_id[]=" + folderid.ToString() + "&status=active");
            string reply = client.DownloadString(address);
            RootObject userdataW = new RootObject();
            userdataW = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataW.success == true)
            {
                label18.Text = "Остановить";
                Console.WriteLine("Группа стартовала");
                queryfolders();
            }
        }
        public void group_delete()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/delete_ad_group.json?token=" + tokenvalue + "&ids[]=" + folderid.ToString());
            string reply = client.DownloadString(address);
            RootObject userdata = new RootObject();
            userdata = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdata.success == true)
            {

            }
        }
        private void send_delete_teaser()
        {
            {
                WebClient wc = new WebClient();
                var URI = new Uri("https://api.directadvert.ru/delete_ad.json?token=" + tokenvalue);
                //If any encoding is needed.
                wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                //Or any other encoding type.
                //If any key needed
                //wc.Headers["KEY"] = "Your_Key_Goes_Here";
                wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
                string result = wc.UploadString(URI, "POST", ads_array.ToString());
                RootObject userdata = JsonConvert.DeserializeObject<RootObject>(result);
                if (userdata.success == true)
                {
                    queryfolders();
                    label9.Refresh();
                    radGridView1.Refresh();
                }
            }
            Settings.Default.group = folderList.SelectedIndex;
            Settings.Default.Save();
            
            folderList.SelectedIndex = Settings.Default.group;
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void folderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            folderList.DataSource = userdataX.account_details;
            folderList.DisplayMember = "title";
            folderList.ValueMember = "group_id";
            folderid = Convert.ToInt32(folderList.SelectedValue);
            Settings.Default.folderid = folderid;
            Settings.Default.Save();
            folderList.DisplayMember = "title";
            folderList.ValueMember = "status";
            if (folderList.SelectedValue != null)
            {
                if (folderList.SelectedValue.ToString() == "paused")
                { pictureBox2.Image = Resources.Toolbar_Pause; label18.Text = "Возобновить"; pictureBox4.Image = Resources.Toolbar_Pause; label12.Text = "Возобновить"; groupstatus = false; }
                if (folderList.SelectedValue.ToString() == "active")
                { pictureBox2.Image = Resources.Toolbar_Play; label18.Text = "Остановить"; pictureBox4.Image = Resources.Toolbar_Play; label12.Text = "Остановить"; groupstatus = true; }
            }

            queryteasers();
            selected_group = folderList.SelectedIndex;
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //imagebytes = Convert.FromBase64String((string)dataGridView1.CurrentRow.Cells[8].Value);
            //MemoryStream ms = new MemoryStream(imagebytes, 0, imagebytes.Length);
            //ms.Write(imagebytes, 0, imagebytes.Length);
            //if(dataGridView1.CurrentRow.Cells[8].Value.ToString() != "")
            //pictureBox3.Image = Image.FromStream(ms, true);
            //teaserHead.Text = current_title = (string)dataGridView1.CurrentRow.Cells[3].Value;
            //teaserDescript.Text = current_announce = (string)dataGridView1.CurrentRow.Cells[4].Value;
            //priceText.Text  = ((double)dataGridView1.CurrentRow.Cells[10].Value).ToString();
            //current_price = (double)dataGridView1.CurrentRow.Cells[10].Value;
            //urlText.Text = current_url = (string)dataGridView1.CurrentRow.Cells[6].Value;
            //statusBox.Text = (string)dataGridView1.CurrentRow.Cells[2].Value;
            //current_group = folderid;
            //teaser_to_edit = (int)dataGridView1.CurrentRow.Cells[0].Value;
            //if (dataGridView1.SelectedRows.Count > 1)
            //    button7.Enabled = false;
            //else 
            //{ 
            //    button7.Enabled = true;
            //    WebClient client = new WebClient();
            //    string address = ("https://api.directadvert.ru/get_teaser_info.json" + "?token=" + tokenvalue + "&id=" + teaser_to_edit);
            //    string reply = client.DownloadString(address);
            //    userdataT = JsonConvert.DeserializeObject<RootObject>(reply);
            //    label30.Text = (userdataT.teaser_info.statistics.current.shows).ToString();
            //    label31.Text = (userdataT.teaser_info.statistics.current.clicks).ToString();
            //    label32.Text = (userdataT.teaser_info.statistics.current.ctr).ToString();
            //    label36.Text = (userdataT.teaser_info.statistics.delta.shows).ToString();
            //    label35.Text = (userdataT.teaser_info.statistics.delta.clicks).ToString();
            //    label34.Text = (userdataT.teaser_info.statistics.delta.ctr).ToString();

            //}
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (flag_create_NF == false)
            {
                flag_create_NF = true;
                timer_animation_panel.Start();
            }
            else if (flag_create_NF == true)
            {
                flag_create_NF = false;
                timer_animation_panel.Start();
            }
        }
        private void timer_animation_panel_Tick(object sender, EventArgs e)
        {
            if (flag_create_NF == true)
            {
                if (start_pos >= 142) { timer_animation_panel.Stop(); button2.Image = Resources.up; }
                else start_pos += 5;
                panel3.Location = new Point(163, start_pos);
                panel5.Location = new Point(163, start_pos);
            }
            else
            {
                if (start_pos <= 17) { timer_animation_panel.Stop(); button2.Image = Resources.Folder_Add; }
                else start_pos -= 5;
                panel3.Location = new Point(163, start_pos);
                panel5.Location = new Point(163, start_pos);
            }
        }
        private void button3_Click(object sender, EventArgs e) //create new group
        {
            if (flag_create_NF == true)
            {
                flag_create_NF = false;
                timer_animation_panel.Start();
                group_add();
                newgroup newgrp = new newgroup();
                newgrp.FormBorderStyle = FormBorderStyle.None;
                newgrp.Show();
                newgroup.grp_name = new_FolderName;
                newgrp.Close();
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)///delete group
        {
            group_delete();
            queryfolders();
            folderList.Refresh();
        }

        private void button6_Click(object sender, EventArgs e) //delete teasers
        {
            if (radGridView1.SelectedRows.Count > 0)
            {
                var results = radGridView1.SelectedRows.Cast<DataGridViewRow>().Select(x => Convert.ToString(x.Cells[0].Value));
                Console.WriteLine(results.Count().ToString());
                ads_array = new StringBuilder();
                foreach (string teaser in results)
                    ads_array.Append("&ids[]=" + Uri.EscapeDataString(teaser));
                send_delete_teaser();
            }
        }
        void wc_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            try
            {
                Console.WriteLine(e.Result);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }
        private void label18_Click(object sender, EventArgs e)
        {
            Settings.Default.group = folderList.SelectedIndex;
            Settings.Default.Save();
            Console.WriteLine(folderid.ToString());
            if (groupstatus == false)
            {
                group_start();
            }
            else if (groupstatus == true)
            {
                group_pause();
            }
            folderList.SelectedIndex = Settings.Default.group;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
                Application.Exit();
            else if (checkBox1.Checked == true)
            {
                ClientSize = new Size(307, 132);
                loginPage.Visible = true;
                tabControl1.Visible = false;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1) 
            { 
                ClientSize = new Size(368, 368);
                button11.Enabled = false;
                progressBar1.Visible = false;
                button11.Enabled = false;
                comboBox1.DataSource = userdataX.account_details;
                comboBox1.DisplayMember = "title";
                comboBox1.ValueMember = "group_id";
                comboBox1.SelectedIndex = folderList.SelectedIndex;
                folderid1 = Settings.Default.folderid;
                string balanceRounded_format = balanseRounded.Replace(".", ",");
                double balance_d = Double.Parse(balanceRounded_format);
                label20.Text = "Баланс аккаунта - " + balanseRounded;
                balance_c = Math.Round(balance_d, 0);
                max_teasers = balance_c / 50;
                max_teasers_end = max_teasers - allcounts;
                if (max_teasers_end == 1 || max_teasers_end == 21 || max_teasers_end == 31 || max_teasers_end == 41 || max_teasers_end == 51 || max_teasers_end == 61)
                {
                    textend = "";
                }
                if ((max_teasers_end > 1 && max_teasers_end < 5) || (max_teasers_end > 21 && max_teasers_end < 25) || (max_teasers_end > 31 && max_teasers_end < 35) || (max_teasers_end > 41 && max_teasers_end < 45) || (max_teasers_end > 51 && max_teasers_end < 55) || (max_teasers_end > 61 && max_teasers_end < 65))
                {
                    textend = "а";
                }
                if ((max_teasers_end == 0) || (max_teasers_end > 4 && max_teasers_end < 21) || (max_teasers_end > 24 && max_teasers_end < 31) || (max_teasers_end > 34 && max_teasers_end < 41) || (max_teasers_end > 44 && max_teasers_end < 51) || (max_teasers_end > 54 && max_teasers_end < 61) || (max_teasers_end > 64 && max_teasers_end < 71))
                {
                    textend = "ов";
                }
                if (allcounts == 1 || allcounts == 21 || allcounts == 31 || allcounts == 41 || allcounts == 51 || allcounts == 61)
                {
                    textend1 = "";
                }
                if ((allcounts > 1 && allcounts < 5) || (allcounts > 21 && allcounts < 25) || (allcounts > 31 && allcounts < 35) || (allcounts > 41 && allcounts < 45) || (allcounts > 51 && allcounts < 55) || (allcounts > 61 && allcounts < 65))
                {
                    textend1 = "а";
                }
                if ((allcounts == 0) || (allcounts > 4 && allcounts < 21) || (allcounts > 24 && allcounts < 31) || (allcounts > 34 && allcounts < 41) || (allcounts > 44 && allcounts < 51) || (allcounts > 54 && allcounts < 61) || (allcounts > 64 && allcounts < 71))
                {
                    textend1 = "ов";
                }

                label15.Text = "В аккаунте " + allcounts + " тизер" + textend1;
                label15.Refresh();
                label19.Text = "Вы можете загрузить еще " + max_teasers_end + " тизер" + textend;
                label19.Refresh();
            }
            else if (tabControl1.SelectedIndex == 0) { this.ClientSize = new Size(800, 600); radGridView1.Refresh(); }
        }
        private void label12_Click(object sender, EventArgs e)
        {
            Settings.Default.group = folderList.SelectedIndex;
            Settings.Default.Save();
            Console.WriteLine(folderid.ToString());
            if (groupstatus == false)
            {
                group_start();
            }
            else if (groupstatus == true)
            {
                group_pause();
            }
            folderList.SelectedIndex = Settings.Default.group;
            Console.WriteLine("{0}", Settings.Default.group);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (flag_create_NF == false)
            {
                flag_create_NF = true;
                timer_animation_panel.Start();
            }
            else if (flag_create_NF == true)
            {
                flag_create_NF = false;
                timer_animation_panel.Start();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.DataSource = userdataX.account_details;
            comboBox1.DisplayMember = "title";
            comboBox1.ValueMember = "group_id";

            comboBox1.DisplayMember = "title";
            comboBox1.ValueMember = "status";
            if (comboBox1.SelectedValue != null)
            {
                if (comboBox1.SelectedValue.ToString() == "paused")
                { pictureBox2.Image = Resources.Toolbar_Pause; label18.Text = "Возобновить"; pictureBox4.Image = Resources.Toolbar_Pause; label12.Text = "Возобновить"; groupstatus = false; }
                if (comboBox1.SelectedValue.ToString() == "active")
                { pictureBox2.Image = Resources.Toolbar_Play; label18.Text = "Остановить"; pictureBox4.Image = Resources.Toolbar_Play; label12.Text = "Остановить"; groupstatus = true; }
            }

            queryteasers();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (flag_create_NF == true)
            {
                flag_create_NF = false;
                timer_animation_panel.Start();
                group_add();
                newgroup newgrp = new newgroup();
                newgrp.FormBorderStyle = FormBorderStyle.None;
                newgrp.Show();
                newgroup.grp_name = new_FolderName;
                newgrp.Close();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            new_group_name.Text = textBox1.Text;
        }
        private void passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginButton_Click(sender, e);
            }
        }
        private void button10_Click(object sender, EventArgs e) // Load File
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Файлы DirectAdvert(*.xlsx)|*.xlsx|Файлы DirectAdvert(*.xls)|*.xls|Все файлы(*.*)|*.*";
            openFileDialog1.Title = "Выбор файла для заливки";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
            }
            if (filename != null)
            {
                int i = filename.LastIndexOf(@"\") + 1;
                Console.WriteLine(i.ToString());
                foldername = filename.Remove(i);
                DirectoryInfo directory = new DirectoryInfo(foldername);
                FileInfo[] files = directory.GetFiles("*.jpg");
                FileInfo[] files2 = directory.GetFiles("*.png");
                lpictures = new List<string>();
                lpictures.Clear();
                foreach (FileInfo Files in files)
                {
                    lpictures.Add(Files.Name);
                }
                foreach (FileInfo Files in files2)
                {
                    lpictures.Add(Files.Name);
                }
                button1.Enabled = true;
            }
            else return;
            Microsoft.Office.Interop.Excel.Application ObjExcel = new Microsoft.Office.Interop.Excel.Application();
            Workbook ObjWorkBook = ObjExcel.Workbooks.Open(filename, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Worksheet ObjWorkSheet;
            ObjWorkSheet = (Worksheet)ObjWorkBook.Sheets[1];
            var lastCell = ObjWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell);
            length_mass = lastCell.Row;
            Range titl = ObjWorkSheet.get_Range("A2", "A" + length_mass.ToString());
            Range anon = ObjWorkSheet.get_Range("B2", "B" + length_mass.ToString());
            Range url = ObjWorkSheet.get_Range("C2", "C" + length_mass.ToString());
            Range pr = ObjWorkSheet.get_Range("D2", "D" + length_mass.ToString());
            Array titles = titl.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            Array annons = anon.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            Array urls = url.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            Array price = pr.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            ObjWorkBook.Close(false, Type.Missing, Type.Missing);
            ObjExcel.Quit();
            GC.Collect();
            teaser_titlles = titles.OfType<object>().Select(o => o.ToString()).ToArray();
            string[] anonses = annons.OfType<object>().Select(o => o.ToString()).ToArray();
            string[] urlss = urls.OfType<object>().Select(o => o.ToString()).ToArray();
            string[] prices = price.OfType<object>().Select(o => o.ToString().Replace(',', '.')).ToArray();
            lTitles = new List<string>();
            lAnnounces = new List<string>();
            lUrl = new List<string>();
            lPrice = new List<string>();
            for (int i = 0; i < teaser_titlles.Length; i++)
            {
                lTitles.Add(teaser_titlles[i]);
                lAnnounces.Add(anonses[i]);
                lUrl.Add(urlss[i]);
                lPrice.Add(prices[i]);
            }
            foreach (string pri in lPrice)
            lpictures.Sort();
            List<string> lPict_sort = new List<string>();
            lPict_sort = (lpictures.Take(teaser_titlles.Length).ToList());
            lBase64pict = new List<string>();
            lcommands = new List<string>();
            max_teasers = balance_c / 50;
            max_teasers_end = max_teasers - allcounts;
            Console.WriteLine("{0}" + "  " + "{1}", max_teasers_end, allcounts);
            foreach (var file in lPict_sort)
            {
                filepath = foldername + file;
                using (Image picture = Image.FromFile(filepath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        picture.Save(m, ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        imagestring_pre = Convert.ToBase64String(imageBytes);
                    }
                }
                string imagestring_1 = imagestring_pre.Replace("/", "%2F");
                string imagestring_2 = imagestring_1.Replace("+", "%2B");
                imagestring = imagestring_2.Replace("=", "%3D");
                lBase64pict.Add(imagestring);
                Console.WriteLine(lBase64pict.Count.ToString());
            }
            if (teaser_titlles.Length > max_teasers_end)
            {
                MessageBox.Show("В файле больше тизеров, чем Вы можете загрузить в аккаунт, попробуйте пополнить счет или уменьшить количество тизеров");
                Process.Start("http://directadvert.ru/payment");
                Close();
            }
            StringBuilder command = new StringBuilder();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = teaser_titlles.Length;
            progressBar1.Step = 1;
            progressBar1.Visible = true;
            for (int i = 0; i < teaser_titlles.Length; i++)
            {
                command = new StringBuilder();
                command.Append("&group_id=" + folderid.ToString());
                command.Append("&title=" + Uri.EscapeDataString(lTitles[i]));
                command.Append("&announce=" + Uri.EscapeDataString(lAnnounces[i]));
                command.Append("&url=" + Uri.EscapeDataString(lUrl[i]));
                command.Append("&price=" + Uri.EscapeDataString(lPrice[i]));
                command.Append("&image=" + lBase64pict[i]);
                //Console.WriteLine(lBase64pict[i]);
                lcommands.Add(command.ToString());
                double res = max_teasers_end - lcommands.Count;
                if (lcommands.Count == 1 || lcommands.Count == 21 || lcommands.Count == 31 || lcommands.Count == 41 || lcommands.Count == 51 || lcommands.Count == 61)
                {
                    textend = "";
                }
                if ((lcommands.Count > 1 && lcommands.Count < 5) || (lcommands.Count > 21 && lcommands.Count < 25) || (lcommands.Count > 31 && lcommands.Count < 35) || (lcommands.Count > 41 && lcommands.Count < 45) || (lcommands.Count > 51 && lcommands.Count < 55) || (lcommands.Count > 61 && lcommands.Count < 65))
                {
                    textend = "а";
                }
                if ((lcommands.Count > 4 && lcommands.Count < 21) || (lcommands.Count > 24 && lcommands.Count < 31) || (lcommands.Count > 34 && lcommands.Count < 41) || (lcommands.Count > 44 && lcommands.Count < 51) || (lcommands.Count > 54 && lcommands.Count < 61) || (lcommands.Count > 64 && lcommands.Count < 71))
                {
                    textend = "ов";
                }
                if (allcounts == 1 || allcounts == 21 || allcounts == 31 || allcounts == 41 || allcounts == 51 || allcounts == 61)
                {
                    textend1 = "";
                }
                if ((allcounts > 1 && allcounts < 5) || (allcounts > 21 && allcounts < 25) || (allcounts > 31 && allcounts < 35) || (allcounts > 41 && allcounts < 45) || (allcounts > 51 && allcounts < 55) || (allcounts > 61 && allcounts < 65))
                {
                    textend1 = "а";
                }
                if ((allcounts > 4 && allcounts < 21) || (allcounts > 24 && allcounts < 31) || (allcounts > 34 && allcounts < 41) || (allcounts > 44 && allcounts < 51) || (allcounts > 54 && allcounts < 61) || (allcounts > 64 && allcounts < 71))
                {
                    textend1 = "ов";
                }
                if (res == 1 || res == 21 || res == 31 || res == 41 || res == 51 || res == 61)
                {
                    textend2 = "";
                }
                if ((res > 1 && res < 5) || (res > 21 && res < 25) || (res > 31 && res < 35) || (res > 41 && res < 45) || (res > 51 && res < 55) || (res > 61 && res < 65))
                {
                    textend2 = "а";
                }
                if ((res > 4 && res < 21) || (res > 24 && res < 31) || (res > 34 && res < 41) || (res > 44 && res < 51) || (res > 54 && res < 61) || (res > 64 && res < 71))
                {
                    textend2 = "ов";
                }
                label21.Text = ("Добавлено " + lcommands.Count.ToString() + " тизер" + textend);
                label21.Refresh();
                label15.Text = "В аккаунте " + allcounts + " тизер" + textend1;
                label15.Refresh();
                label19.Text = "Вы можете загрузить еще " + res + " тизер" + textend2;
                label19.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(300);
            }
            button11.Enabled = true;
        }

        private void button11_Click(object sender, EventArgs e) //Start
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = teaser_titlles.Length;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            int d = 0;
            Text = Resources.sendDate;
            foreach (var com in lcommands)
            {
                    WebClient wc = new WebClient();
                    var URI = new Uri("https://api.directadvert.ru/create_ad.json?token=" + tokenvalue);
                    //If any encoding is needed.
                    wc.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                    //Or any other encoding type.
                    //If any key needed
                    //wc.Headers["KEY"] = "Your_Key_Goes_Here";
                    wc.UploadStringCompleted += new UploadStringCompletedEventHandler(wc_UploadStringCompleted);
                    string responce = wc.UploadString(URI, "POST", com.ToString());
                    Console.WriteLine(responce);
                    d++;
                    progressBar1.PerformStep();
                Settings.Default.group = folderList.SelectedIndex;
                Settings.Default.Save();
                }

            group_pause();
            folderList.SelectedIndex = Settings.Default.group;
            radGridView1.Refresh();
            this.Text = "Direct/Advert";
     
                
        }
        private void button7_Click(object sender, EventArgs e) // edit teaser
        {
            teaser_edit teasedit = new teaser_edit();
            selected_row = radGridView1.CurrentRow.Index;
            teasedit.Show();
        }
        private void edit_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);
        }
        private void delete_Click(object sender, EventArgs e)
        {
            button6_Click(sender, e);
        }
        private void stat_Click(object sender, EventArgs e)
        {
            Teaser_stat statteaser = new Teaser_stat();
            statteaser.Show();
            
        }

        public void radGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (userdataY != null)
            {
                if ((string)radGridView1.CurrentRow.Cells["image_string"].Value != null)
                {
                    imagebytes = Convert.FromBase64String((string)radGridView1.CurrentRow.Cells["image_string"].Value);
                    MemoryStream ms = new MemoryStream(imagebytes, 0, imagebytes.Length);
                    ms.Write(imagebytes, 0, imagebytes.Length);
                    if (radGridView1.CurrentRow.Cells[8].Value.ToString() != "")
                        pictureBox3.Image = Image.FromStream(ms, true);
                }
                else
                {
                }
                teaserHead.Text = current_title = (string)radGridView1.CurrentRow.Cells["title"].Value;
            }
            teaserDescript.Text = current_announce = (string) radGridView1.CurrentRow.Cells["announce"].Value;
            priceText.Text = ((double)radGridView1.CurrentRow.Cells["buy_price"].Value).ToString(CultureInfo.InvariantCulture);
            current_price = (double)radGridView1.CurrentRow.Cells["buy_price"].Value;
            urlText.Text = current_url = (string)radGridView1.CurrentRow.Cells["url"].Value;
            statusBox.Text = (string) radGridView1.CurrentRow.Cells["status"].Value;
            current_group = folderid;
            teaser_to_edit = (int)radGridView1.CurrentRow.Cells["ad_id"].Value;
            cururlimg = (string)radGridView1.CurrentRow.Cells["image_url"].Value;
            if (radGridView1.SelectedRows.Count > 1)
                button7.Enabled = false;
            else
            {
            }
            button7.Enabled = true;
                WebClient client = new WebClient();
                string address = ("https://api.directadvert.ru/get_teaser_info.json" + "?token=" + tokenvalue + "&id=" + teaser_to_edit);
                string reply = client.DownloadString(address);
                userdataT = JsonConvert.DeserializeObject<RootObject>(reply);
                label30.Text = (userdataT.teaser_info.statistics.current.shows).ToString();
                label31.Text = (userdataT.teaser_info.statistics.current.clicks).ToString();
                label32.Text = (userdataT.teaser_info.statistics.current.ctr).ToString(CultureInfo.InvariantCulture);
                label36.Text = ((userdataT.teaser_info.statistics.current.shows)-(userdataT.teaser_info.statistics.delta.shows)).ToString();
                label35.Text = ((userdataT.teaser_info.statistics.current.clicks)-(userdataT.teaser_info.statistics.delta.clicks)).ToString();
                label34.Text = ((userdataT.teaser_info.statistics.current.ctr)-(userdataT.teaser_info.statistics.delta.ctr)).ToString(CultureInfo.InvariantCulture);
                today_shows = userdataT.teaser_info.statistics.current.shows;
                today_cliks = userdataT.teaser_info.statistics.current.clicks;
                today_ctr = userdataT.teaser_info.statistics.current.ctr;
                yesterday_shows = (userdataT.teaser_info.statistics.current.shows) - (userdataT.teaser_info.statistics.delta.shows);
            yesterday_clicks = (userdataT.teaser_info.statistics.current.clicks) -
                               (userdataT.teaser_info.statistics.delta.clicks);
            yesterday_ctr = (userdataT.teaser_info.statistics.current.ctr) -
                            (userdataT.teaser_info.statistics.delta.ctr);
            radProgressBar1.Maximum = today_shows + yesterday_shows;
            radProgressBar1.Value1 = today_shows;
            radProgressBar2.Maximum = today_cliks + yesterday_clicks;
            radProgressBar2.Value1 = today_cliks;
            radProgressBar3.Maximum = Convert.ToInt32((today_ctr*100)+(yesterday_ctr*100));
            radProgressBar3.Value1 = Convert.ToInt32(today_ctr*100);
        }

 

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    radGridView1.EnablePaging = true;
                    kolvo_strok = 5;
                    radGridView1.PageSize = kolvo_strok;
                    radGridView1.Refresh();
                    break;
                case 1:
                    radGridView1.EnablePaging = true;
                    kolvo_strok = 10;
                    radGridView1.PageSize = kolvo_strok;
                    radGridView1.Refresh();
                    break;
                case 2:
                    radGridView1.EnablePaging = true;
                    kolvo_strok = 15;
                    radGridView1.PageSize = kolvo_strok;
                    radGridView1.Refresh();
                    break;
                case 3:
                    radGridView1.EnablePaging = false;
                    radGridView1.PageSize = kolvo_strok;
                    radGridView1.Refresh();
                    break;
            }
        }

      }

    }

    