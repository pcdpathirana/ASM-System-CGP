using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM_System_CGP
{
    public partial class SystemDashboard : Form
    {
        public SystemDashboard()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            Show_Vehicles show = new Show_Vehicles();
            show.ShowDialog();
        }
    }
}
