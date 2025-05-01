using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class saller_dashboard : Form
    {
        public saller_dashboard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            createcustomer cus = new createcustomer();
            cus.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            createReservation res = new createReservation();
            res.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            createpayment pay = new createpayment();
            pay.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Show_Vehicles show = new Show_Vehicles();
            show.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            logout log = new logout();
            log.Show();
            this.Hide();
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnminimize_Click(object sender, EventArgs e)
        {

        }

        private void btnmaxmin_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
