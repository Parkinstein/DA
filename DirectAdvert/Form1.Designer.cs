namespace DirectAdvert
{
    partial class daForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(daForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Page1 = new System.Windows.Forms.TabPage();
            this.Page2 = new System.Windows.Forms.TabPage();
            this.loginPage = new System.Windows.Forms.Panel();
            this.flagBox = new System.Windows.Forms.PictureBox();
            this.eyepassbox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.savePasswordchkbx = new System.Windows.Forms.CheckBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.forgotPasslink = new System.Windows.Forms.LinkLabel();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.loginBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.loginPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flagBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eyepassbox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Page1);
            this.tabControl1.Controls.Add(this.Page2);
            this.tabControl1.Location = new System.Drawing.Point(2, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(892, 414);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // Page1
            // 
            this.Page1.Location = new System.Drawing.Point(4, 22);
            this.Page1.Name = "Page1";
            this.Page1.Padding = new System.Windows.Forms.Padding(3);
            this.Page1.Size = new System.Drawing.Size(884, 388);
            this.Page1.TabIndex = 0;
            this.Page1.Text = "Управление тизерами";
            this.Page1.ToolTipText = "Управление тизерами";
            this.Page1.UseVisualStyleBackColor = true;
            // 
            // Page2
            // 
            this.Page2.Location = new System.Drawing.Point(4, 22);
            this.Page2.Name = "Page2";
            this.Page2.Padding = new System.Windows.Forms.Padding(3);
            this.Page2.Size = new System.Drawing.Size(884, 388);
            this.Page2.TabIndex = 1;
            this.Page2.Text = "Пакетная загрузка тизеров";
            this.Page2.UseVisualStyleBackColor = true;
            // 
            // loginPage
            // 
            this.loginPage.Controls.Add(this.flagBox);
            this.loginPage.Controls.Add(this.eyepassbox);
            this.loginPage.Controls.Add(this.label2);
            this.loginPage.Controls.Add(this.cancelButton);
            this.loginPage.Controls.Add(this.savePasswordchkbx);
            this.loginPage.Controls.Add(this.loginButton);
            this.loginPage.Controls.Add(this.forgotPasslink);
            this.loginPage.Controls.Add(this.passwordBox);
            this.loginPage.Controls.Add(this.loginBox);
            this.loginPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loginPage.Location = new System.Drawing.Point(0, 0);
            this.loginPage.Name = "loginPage";
            this.loginPage.Size = new System.Drawing.Size(315, 108);
            this.loginPage.TabIndex = 1;
            // 
            // flagBox
            // 
            this.flagBox.Location = new System.Drawing.Point(135, 69);
            this.flagBox.Name = "flagBox";
            this.flagBox.Size = new System.Drawing.Size(30, 30);
            this.flagBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.flagBox.TabIndex = 12;
            this.flagBox.TabStop = false;
            // 
            // eyepassbox
            // 
            this.eyepassbox.Image = global::DirectAdvert.Properties.Resources.eye;
            this.eyepassbox.Location = new System.Drawing.Point(146, 40);
            this.eyepassbox.Name = "eyepassbox";
            this.eyepassbox.Size = new System.Drawing.Size(18, 18);
            this.eyepassbox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.eyepassbox.TabIndex = 11;
            this.eyepassbox.TabStop = false;
            this.eyepassbox.MouseLeave += new System.EventHandler(this.eyepassbox_MouseLeave);
            this.eyepassbox.MouseHover += new System.EventHandler(this.eyepassbox_MouseHover);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Текущая раскладка - ";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(180, 38);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(123, 20);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Выход";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // savePasswordchkbx
            // 
            this.savePasswordchkbx.AutoSize = true;
            this.savePasswordchkbx.Location = new System.Drawing.Point(180, 69);
            this.savePasswordchkbx.Name = "savePasswordchkbx";
            this.savePasswordchkbx.Size = new System.Drawing.Size(123, 30);
            this.savePasswordchkbx.TabIndex = 5;
            this.savePasswordchkbx.Text = "Сохранить данные \r\nдля входа";
            this.savePasswordchkbx.UseVisualStyleBackColor = true;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(180, 12);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(123, 20);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Вход";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // forgotPasslink
            // 
            this.forgotPasslink.AutoSize = true;
            this.forgotPasslink.Location = new System.Drawing.Point(5, 86);
            this.forgotPasslink.Name = "forgotPasslink";
            this.forgotPasslink.Size = new System.Drawing.Size(91, 13);
            this.forgotPasslink.TabIndex = 4;
            this.forgotPasslink.TabStop = true;
            this.forgotPasslink.Text = "Забыли пароль?";
            this.forgotPasslink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.forgotPasslink_LinkClicked);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(5, 39);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(160, 20);
            this.passwordBox.TabIndex = 1;
            this.passwordBox.UseSystemPasswordChar = true;
            this.passwordBox.TextChanged += new System.EventHandler(this.passwordBox_TextChanged);
            // 
            // loginBox
            // 
            this.loginBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.loginBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.loginBox.CausesValidation = false;
            this.loginBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.loginBox.Location = new System.Drawing.Point(5, 12);
            this.loginBox.Name = "loginBox";
            this.loginBox.Size = new System.Drawing.Size(160, 20);
            this.loginBox.TabIndex = 0;
            this.loginBox.WordWrap = false;
            this.loginBox.TextChanged += new System.EventHandler(this.loginBox_TextChanged);
            this.loginBox.Leave += new System.EventHandler(this.loginBox_Leave);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // daForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 108);
            this.Controls.Add(this.loginPage);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "daForm";
            this.Text = "Direct/Advert";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.loginPage.ResumeLayout(false);
            this.loginPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flagBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eyepassbox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage Page1;
        private System.Windows.Forms.TabPage Page2;
        private System.Windows.Forms.Panel loginPage;
        private System.Windows.Forms.LinkLabel forgotPasslink;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.TextBox loginBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox savePasswordchkbx;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox eyepassbox;
        private System.Windows.Forms.PictureBox flagBox;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

