using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.AccessControl;
using Microsoft.Office.Interop.Excel;

namespace DirectAdvert
{
    public partial class daForm : Form
    {
        public daForm()
        {
            InitializeComponent();
        }
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
        public List<string> logins, lAnnounces, lBase64pict, lcommands, lpictures, lPrice, lTitles, lUrl;
        private static int _ProcessId;
        private static string _CurrentInputLanguage;
        static public string login_string, password_string, tokenvalue, errorvalue, balance, email, currency;
        public string pathFile, fileData, decrypted, current_title, new_FolderName, filename, filepath, foldername, imagestring, textend;
        static public int errorcode;
        public double max_teasers_end;
        public string[] teaser_titlles;
        static public bool accses;
        public int folderid, start_pos, zas, length_mass;
        public RootObject userdataV, userdataW, userdataX, userdataY, userdataZ;
        public static object datasrc;
        public bool flag_create_NF, groupstatus;
        public StringBuilder ads_array;
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
            public bool success { get; set; }
            public int error_code { get; set; }
            public string error_message { get; set; }
            public int id { get; set; }
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
            start_pos = 17;
            tabControl1.Visible = false;
            flag_create_NF = false;
            this.ClientSize = new System.Drawing.Size(307, 112);
            loginPage.Visible = true;
            loginButton.Enabled = false;
            pictureBox1.Visible = false;
            label7.Text = "";
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            label11.Visible = false;
            eyepassbox.Image = DirectAdvert.Properties.Resources.eye;
            loginBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            loginBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
                if (i >= 0 && datalog.ElementAt(i).password_in.ToString() != "")
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
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            int a = datalog.FindIndex(s => s.login_in == loginBox.Text);
                            datalog.RemoveAt(a);
                            datalog.Add(new Input() { login_in = login_string, password_in = password_string }); decrypted = JsonConvert.SerializeObject(datalog);
                            string entropy = null;
                            string encrypted = DPAPI.Encrypt(DPAPI.KeyType.UserKey, decrypted, entropy);
                            File.WriteAllText(pathFile, encrypted);
                            frm.label1.Text = "Данные были перезаписаны";
                            label7.Text = "";
                            label8.Text = "";
                            label9.Text = "";
                            label10.Text = "";
                            pictureBox3.Image = null;
                            priceText.Text = "";
                            urlText.Text = "";
                            statusBox.Text = "";
                            teaserHead.Text = "";
                            teaserDescript.Text = "";
                            querytoken();
                        }
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
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
            if (userdata.success == true)
            {
                Console.WriteLine("отработала querytoken ");
                tokenvalue = userdata.token;
                currency = userdata.account_data.currency;
                if (currency == "RUR") { pictureBox1.Visible = true; pictureBox1.Image = DirectAdvert.Properties.Resources.russia; }
                label7.Text = userdata.account_data.email;
                int i = userdata.account_data.balance.IndexOf(".");
                string balanseRounded_pre = userdata.account_data.balance;
                string balanseRounded = balanseRounded_pre.Remove(i + 3);
                label8.Text = balanseRounded;
                this.ClientSize = new System.Drawing.Size(800, 600);
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
                double allcounts = 0;
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
            }
        }
        public void queryteasers()
        {
            this.Text = "Идет получение данных...";
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/get_group_teasers.json" + "?token=" + tokenvalue + "&group_id=" + folderid);
            string reply = client.DownloadString(address);
            userdataY = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataY.success == true)
            {
                Console.WriteLine("отработала queryteasers ");
                dataGridView1.DataSource = userdataY.group_teasers;
                label11.Visible = false;
                dataGridView1.Visible = true;
                dataGridView1.RowHeadersVisible = false;
                dataGridView1.Columns[0].HeaderText = "ID объявления";
                dataGridView1.Columns[1].HeaderText = "ID группы";
                dataGridView1.Columns[2].HeaderText = "Статус";
                dataGridView1.Columns[3].HeaderText = "Название тизера";
                dataGridView1.Columns[4].HeaderText = "Текст";
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].HeaderText = "URL";
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].HeaderText = "Цена";
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoResizeRows();
                dataGridView1.Columns[10].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            else if (userdataY.error_code == 1016)
            {
                label11.Visible = true; label7.Text = "";
                label8.Text = "";
                label9.Text = "";
                label10.Text = "";
                pictureBox3.Image = null;
                priceText.Text = "";
                urlText.Text = "";
                statusBox.Text = "";
                teaserHead.Text = "";
                teaserDescript.Text = "";
            }
            this.Text = "Direct/Advert";
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
                wc.UploadString(URI, "POST", ads_array.ToString());
            }
            DirectAdvert.Properties.Settings.Default.group = folderList.SelectedIndex;
            Properties.Settings.Default.Save();
            queryteasers();
            dataGridView1.Refresh();
            folderList.SelectedIndex = DirectAdvert.Properties.Settings.Default.group;
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void folderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            folderList.DataSource = userdataX.account_details;
            folderList.DisplayMember = "title";
            folderList.ValueMember = "group_id";
            folderid = Convert.ToInt32(folderList.SelectedValue);
            folderList.DisplayMember = "title";
            folderList.ValueMember = "status";
            if (folderList.SelectedValue != null)
            {
                if (folderList.SelectedValue.ToString() == "paused")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.Toolbar_Pause; label18.Text = "Возобновить"; pictureBox4.Image = DirectAdvert.Properties.Resources.Toolbar_Pause; label12.Text = "Возобновить"; groupstatus = false; }
                if (folderList.SelectedValue.ToString() == "active")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.Toolbar_Play; label18.Text = "Остановить"; pictureBox4.Image = DirectAdvert.Properties.Resources.Toolbar_Play; label12.Text = "Остановить"; groupstatus = true; }
            }
            queryteasers();
        }
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            byte[] imagebytes = Convert.FromBase64String((string)dataGridView1.CurrentRow.Cells[8].Value);
            MemoryStream ms = new MemoryStream(imagebytes, 0, imagebytes.Length);
            ms.Write(imagebytes, 0, imagebytes.Length);
            pictureBox3.Image = Image.FromStream(ms, true);
            teaserHead.Text = (string)dataGridView1.CurrentRow.Cells[3].Value;
            teaserDescript.Text = (string)dataGridView1.CurrentRow.Cells[4].Value;
            priceText.Text = ((double)dataGridView1.CurrentRow.Cells[10].Value).ToString();
            urlText.Text = (string)dataGridView1.CurrentRow.Cells[6].Value;
            statusBox.Text = (string)dataGridView1.CurrentRow.Cells[2].Value;
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
                if (start_pos >= 142) { timer_animation_panel.Stop(); button2.Image = DirectAdvert.Properties.Resources.up; }
                else start_pos += 5;
                panel3.Location = new System.Drawing.Point(163, start_pos);
                panel5.Location = new System.Drawing.Point(163, start_pos);
            }
            else
            {
                if (start_pos <= 17) { timer_animation_panel.Stop(); button2.Image = DirectAdvert.Properties.Resources.Folder_Add; }
                else start_pos -= 5;
                panel3.Location = new System.Drawing.Point(163, start_pos);
                panel5.Location = new System.Drawing.Point(163, start_pos);
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
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        private void button6_Click(object sender, EventArgs e) //delete teasers
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var results = dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(x => Convert.ToString(x.Cells[0].Value));
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
            DirectAdvert.Properties.Settings.Default.group = folderList.SelectedIndex;
            Properties.Settings.Default.Save();
            Console.WriteLine(folderid.ToString());
            if (groupstatus == false)
            {
                group_start();
            }
            else if (groupstatus == true)
            {
                group_pause();
            }
            folderList.SelectedIndex = DirectAdvert.Properties.Settings.Default.group;
            Console.WriteLine("{0}", DirectAdvert.Properties.Settings.Default.group);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
                System.Windows.Forms.Application.Exit();
            else if (checkBox1.Checked == true)
            {
                this.ClientSize = new System.Drawing.Size(307, 112);
                loginPage.Visible = true;
                tabControl1.Visible = false;
            }
        }
        private void signcount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CalculateTotalPages();
            //Console.WriteLine(TotalPage.ToString());
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1) { this.ClientSize = new System.Drawing.Size(368, 368); }
            else if (tabControl1.SelectedIndex == 0) { this.ClientSize = new System.Drawing.Size(800, 600); }
        }
        private void label12_Click(object sender, EventArgs e)
        {
            DirectAdvert.Properties.Settings.Default.group = folderList.SelectedIndex;
            Properties.Settings.Default.Save();
            Console.WriteLine(folderid.ToString());
            if (groupstatus == false)
            {
                group_start();
            }
            else if (groupstatus == true)
            {
                group_pause();
            }
            folderList.SelectedIndex = DirectAdvert.Properties.Settings.Default.group;
            Console.WriteLine("{0}", DirectAdvert.Properties.Settings.Default.group);
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
            folderid = Convert.ToInt32(folderList.SelectedValue);
            comboBox1.DisplayMember = "title";
            comboBox1.ValueMember = "status";
            if (comboBox1.SelectedValue != null)
            {
                if (comboBox1.SelectedValue.ToString() == "paused")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.Toolbar_Pause; label18.Text = "Возобновить"; pictureBox4.Image = DirectAdvert.Properties.Resources.Toolbar_Pause; label12.Text = "Возобновить"; groupstatus = false; }
                if (comboBox1.SelectedValue.ToString() == "active")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.Toolbar_Play; label18.Text = "Остановить"; pictureBox4.Image = DirectAdvert.Properties.Resources.Toolbar_Play; label12.Text = "Остановить"; groupstatus = true; }
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
        private void button10_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Файлы DirectAdvert(*.xlsx)|*.xlsx|Файлы DirectAdvert(*.xls)|*.xls|Все файлы(*.*)|*.*";
            openFileDialog1.Title = "Выбор файла для заливки";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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
            Microsoft.Office.Interop.Excel.Workbook ObjWorkBook = ObjExcel.Workbooks.Open(filename, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
            Microsoft.Office.Interop.Excel.Worksheet ObjWorkSheet;
            ObjWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ObjWorkBook.Sheets[1];
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Microsoft.Office.Interop.Excel.XlCellType.xlCellTypeLastCell);
            length_mass = lastCell.Row;
            Range titl = ObjWorkSheet.get_Range("A2", "A" + length_mass.ToString());
            Range anon = ObjWorkSheet.get_Range("B2", "B" + length_mass.ToString());
            Range url = ObjWorkSheet.get_Range("C2", "C" + length_mass.ToString());
            Range pr = ObjWorkSheet.get_Range("D2", "D" + length_mass.ToString());
            System.Array titles = titl.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
            System.Array annons = anon.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
            System.Array urls = url.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
            System.Array price = pr.get_Value(Microsoft.Office.Interop.Excel.XlRangeValueDataType.xlRangeValueDefault);
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
                Console.WriteLine(pri.ToString());
            lpictures.Sort();
            List<string> lPict_sort = new List<string>();
            lPict_sort = (lpictures.Take(teaser_titlles.Length).ToList());
            lBase64pict = new List<string>();
            lcommands = new List<string>();
            foreach (var file in lPict_sort)
            {
                filepath = foldername + file;
                using (Image picture = Image.FromFile(filepath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        picture.Save(m, ImageFormat.Jpeg);
                        byte[] imageBytes = m.ToArray();
                        imagestring = Convert.ToBase64String(imageBytes);
                    }
                }
                lBase64pict.Add(imagestring);
                Console.WriteLine(lBase64pict.Count.ToString());
            }
            if (teaser_titlles.Length > max_teasers_end)
            {
                MessageBox.Show("В файле больше тизеров, чем Вы можете загрузить в аккаунт, попробуйте пополнить счет или уменьшить количество тизеров");
                System.Diagnostics.Process.Start("http://directadvert.ru/payment");
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
                command.Append("&group_id=" + Uri.EscapeDataString(folderid.ToString()));
                command.Append("&title=" + Uri.EscapeDataString(lTitles[i]));
                command.Append("&announce=" + Uri.EscapeDataString(lAnnounces[i]));
                command.Append("&url=" + Uri.EscapeDataString(lUrl[i]));
                command.Append("&price=" + Uri.EscapeDataString(lPrice[i]));
                command.Append("&image=" + Uri.EscapeDataString(lBase64pict[i]));
                lcommands.Add(command.ToString());
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
                label3.Text = ("Добавлено " + lcommands.Count.ToString() + " тизер" + textend);
                label3.Refresh();

                label5.Refresh();
                label6.Refresh();
                progressBar1.PerformStep();
                Thread.Sleep(300);
            }
            button11.Enabled = true;
        }
    }
}