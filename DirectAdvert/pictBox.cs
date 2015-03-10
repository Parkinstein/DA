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
    public partial class pictBox : Form
    {
        public pictBox()
        {
            InitializeComponent();
        }

        private void pictBox_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = teaser_edit.Teapic;
        }
    }
}
