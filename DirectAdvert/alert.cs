using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectAdvert
{
    public partial class alert : Form
    {
        public alert()
        {
            InitializeComponent();
        }
        public static bool flag;
        private void alert_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            flag = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flag = false;
            daForm.login_string = "";
            daForm.password_string = "";
            this.Close();
        }
    }
}
