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
    public partial class newgroup : Form
    {
        public newgroup()
        {
            InitializeComponent();
        }
        public static string grp_name;

        private void newgroup_Load(object sender, EventArgs e)
        {
            label19.Text = grp_name;
        }
    }
}
