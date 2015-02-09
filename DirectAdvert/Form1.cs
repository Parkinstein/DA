using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
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
        static public string login_string;
        static public string password_string;
        public string pathFile;
        public string fileData;
        static public string tokenvalue;
        static public string errorvalue;
        static public int errorcode;
        static public string balance;
        static public string email;
        static public string currency;
        static public bool accses;
        public string decrypted;
        public int folderid;
        public RootObject userdataX;
        public RootObject userdataY;
        public RootObject userdataZ;
        public static object datasrc;
        public bool flag_create_NF;
        public int start_pos;
        public StringBuilder ads_array;
        public bool groupstatus;
        public string current_title;
        public string new_FolderName;


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
            listBox1.Items.Clear();
            flag_create_NF = false;
            this.ClientSize = new System.Drawing.Size(307, 112); 
            loginPage.Visible = true;
            loginButton.Enabled = false;
            pictureBox1.Visible = false;
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
            else if (userdata.error_code == 1010) { MessageBox.Show(userdata.error_message.Replace("&nbsp;"," ")); }
        }
        public void queryfolders()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/get_account_details.json"+"?token="+tokenvalue);
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
            //else if (userdata.error_code == 1016) { Console.WriteLine("1016"); }
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
                //dataGridView1.AutoSize = true;
                
            }
            else if (userdataY.error_code == 1016) { label11.Visible = true; }
            this.Text = "Direct/Advert";
        }

        public void group_add()
        {
            WebClient client = new WebClient();
            new_FolderName = new_group_name.Text;
            string new_folder = "&name=" + Uri.EscapeDataString(new_FolderName);
            string address = ("https://api.directadvert.ru/create_ad_group.json?token=" + tokenvalue  + new_folder);
            string reply = client.DownloadString(address);
            RootObject userdataZ = new RootObject();
            userdataZ = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdataZ.success == true)
            {

                folderid = userdataZ.id;
                group_pause();
                queryfolders();
                folderList.Refresh();
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
            RootObject userdata = new RootObject();
            userdata = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdata.success == true)
            {
                Console.WriteLine("отработала pause group ");
            }
            else if (userdata.error_code == 1010) { MessageBox.Show(userdata.error_message.Replace("&nbsp;", " ")); }
        }
        public void group_start()
        {
            WebClient client = new WebClient();
            string address = ("https://api.directadvert.ru/set_groups_status.json?token=" + tokenvalue + "&groups_id[]=" + folderid.ToString() + "&status=active");
            
            string reply = client.DownloadString(address);
            RootObject userdata = new RootObject();
            userdata = JsonConvert.DeserializeObject<RootObject>(reply);
            if (userdata.success == true)
            {
                int group_to_set = folderid;

                Console.WriteLine("отработала active group "+Environment.NewLine + group_to_set.ToString());
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
                Console.WriteLine("отработала delete group ");
            }
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
            current_title = folderList.DisplayMember.ToString();
            folderid = Convert.ToInt32(folderList.SelectedValue);
            folderList.DisplayMember = "title";
            folderList.ValueMember = "status";
            if (folderList.SelectedValue != null)
            {
                if (folderList.SelectedValue.ToString() == "paused")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.pause_blue; groupstatus = false; }
                if (folderList.SelectedValue.ToString() == "active")
                { pictureBox2.Image = DirectAdvert.Properties.Resources.play_blue; groupstatus = true; }
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
            if( flag_create_NF == false)
            {
                flag_create_NF = true; 
                timer_animation_panel.Start();
            }
            else if (flag_create_NF == true)
            { flag_create_NF = false; 
                timer_animation_panel.Start(); }
        }

        private void timer_animation_panel_Tick(object sender, EventArgs e)
        {
            if (flag_create_NF == true)
            {
                if (start_pos >= 134) { timer_animation_panel.Stop(); button2.Image = DirectAdvert.Properties.Resources.arr_up; }

                else start_pos += 5;
                panel3.Location = new Point(163, start_pos);

            }
            else 
            {
                if (start_pos <= 17) { timer_animation_panel.Stop(); button2.Image = DirectAdvert.Properties.Resources.arr_dn; }
                else start_pos -= 5;
                panel3.Location = new Point(163, start_pos);

            }

        }

        private void button3_Click(object sender, EventArgs e) //create new group
        {
            if (flag_create_NF == true)
            {
                flag_create_NF = false;
                timer_animation_panel.Start();
                group_add();
                label18.Visible = true;
                label19.Text = new_FolderName;
                label19.Visible = true;
                Thread.Sleep(2000);
                label18.Visible = false;
                label19.Visible = false;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (groupstatus == false)
            {
                group_start(); //queryfolders(); 
                int a = folderList.FindString(current_title);
                folderList.SelectedIndex = a;
                Console.WriteLine(folderid.ToString());
                queryfolders();
                folderList.SelectedIndex = a;
                Console.WriteLine(folderid.ToString());
            }
            if (groupstatus == true)
            {
                group_pause(); //queryfolders();
                int a = folderList.FindString(current_title);
                Console.WriteLine(folderid.ToString() + " 1  " + a.ToString());
                queryfolders();
                Console.WriteLine(folderid.ToString() + " 2  " + a.ToString());
                folderList.SelectedIndex = a;
                Console.WriteLine(folderid.ToString() + " 3  " + a.ToString());
                
            }
        }

        private void button1_Click(object sender, EventArgs e)///delete group
        {
            group_delete();
            queryfolders();
            folderList.Refresh();
        }
    }
}
        