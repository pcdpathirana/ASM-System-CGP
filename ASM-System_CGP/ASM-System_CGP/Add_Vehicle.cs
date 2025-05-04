using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class Add_Vehicle : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;

        public Add_Vehicle()
        {
            conn = new SqlConnection(connstring);
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnmaxmin_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnminimize_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }
        private byte[] ImageToByteArray(Image img)
        {
            if (img == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"INSERT INTO VehicleDetails 
        (VehicleMake, VehicleModel, YOM, ROM, Fuel, EngineCapacity, RegisteredNumber, Mileage, 
         ChassisNumber, EngineNumber, BodyType, Colour, ConditionType, 
         Image1, Image2, Image3 , Cost ,SellingPrice )
        VALUES 
        (@Make, @Model, @YOM, @ROM, @Fuel, @EngineCapacity, @RegNumber, @Mileage,
         @ChassisNo, @EngineNo, @BodyType, @Colour, @ConditionType,
         @Image1, @Image2, @Image3 ,@Cost ,@SellingPrice)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Make", txtMake.Text);
                        cmd.Parameters.AddWithValue("@Model", txtModel.Text);
                        cmd.Parameters.AddWithValue("@YOM", int.Parse(txtYOM.Text));
                        cmd.Parameters.AddWithValue("@ROM", int.Parse(txtROM.Text));
                        cmd.Parameters.AddWithValue("@Fuel", txtFuel.Text);
                        cmd.Parameters.AddWithValue("@EngineCapacity", txtEngineCapacity.Text);
                        cmd.Parameters.AddWithValue("@RegNumber", txtRegisteredNumber.Text);
                        cmd.Parameters.AddWithValue("@Mileage", txtMileage.Text);
                        cmd.Parameters.AddWithValue("@ChassisNo", txtChassisNumber.Text);
                        cmd.Parameters.AddWithValue("@EngineNo", txtEngineNumber.Text);
                        cmd.Parameters.AddWithValue("@BodyType", txtBodyType.Text);
                        cmd.Parameters.AddWithValue("@Colour", txtColour.Text);
                        cmd.Parameters.AddWithValue("@ConditionType", txtConditionType.Text);

                        // Convert images to byte arrays safely
                        cmd.Parameters.AddWithValue("@Image1", ImageToByteArray(pictureBox1.Image));
                        cmd.Parameters.AddWithValue("@Image2", ImageToByteArray(pictureBox2.Image));
                        cmd.Parameters.AddWithValue("@Image3", ImageToByteArray(pictureBox3.Image));

                        cmd.Parameters.AddWithValue("@Cost", txtcost.Text);
                        cmd.Parameters.AddWithValue("@SellingPrice", txtsellingprice.Text);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        MessageBox.Show("Vehicle details saved successfully!");
                    }
                }
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Invalid number format in YOM or ROM. Please check the values.\n\n" + fe.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error occurred:\n\n" + sqlEx.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred:\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = LoadImageSafely(dlg.FileName);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = LoadImageSafely(dlg.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image = LoadImageSafely(dlg.FileName);
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {


            txtMake.Clear();
            txtModel.Clear();
            txtYOM.Clear();
            txtROM.Clear();
            txtFuel.Clear();
            txtEngineCapacity.Clear();
            txtRegisteredNumber.Clear();
            txtMileage.Clear();
            txtChassisNumber.Clear();
            txtEngineNumber.Clear();
            txtBodyType.Clear();
            txtColour.Clear();


            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;

            txtConditionType.Text = "";
            txtcost.Clear();
            txtsellingprice.Clear();


        }

        private void button8_Click(object sender, EventArgs e)
        {
            Purchase_Details pc = new Purchase_Details();
            pc.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Show_Vehicles show = new Show_Vehicles();
            show.ShowDialog();
        }

        private void txtRegisteredNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT * FROM VehicleDetails WHERE RegisteredNumber LIKE @reg";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reg", txtRegisteredNumber.Text);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtMake.Text = reader["VehicleMake"].ToString();
                                txtModel.Text = reader["VehicleModel"].ToString();
                                txtYOM.Text = reader["YOM"].ToString();
                                txtROM.Text = reader["ROM"].ToString();
                                txtFuel.Text = reader["Fuel"].ToString();
                                txtEngineCapacity.Text = reader["EngineCapacity"].ToString();
                                txtRegisteredNumber.Text = reader["RegisteredNumber"].ToString();
                                txtMileage.Text = reader["Mileage"].ToString();
                                txtChassisNumber.Text = reader["ChassisNumber"].ToString();
                                txtEngineNumber.Text = reader["EngineNumber"].ToString();
                                txtBodyType.Text = reader["BodyType"].ToString();
                                txtColour.Text = reader["Colour"].ToString();
                                txtConditionType.Text = reader["ConditionType"].ToString();

                                if (reader["Image1"] != DBNull.Value)
                                    pictureBox1.Image = ByteArrayToImage((byte[])reader["Image1"]);

                                if (reader["Image2"] != DBNull.Value)
                                    pictureBox2.Image = ByteArrayToImage((byte[])reader["Image2"]);

                                if (reader["Image3"] != DBNull.Value)
                                    pictureBox3.Image = ByteArrayToImage((byte[])reader["Image3"]);

                                decimal cost = reader["Cost"] != DBNull.Value ? Convert.ToDecimal(reader["Cost"]) : 0;
                                decimal sellingPrice = reader["SellingPrice"] != DBNull.Value ? Convert.ToDecimal(reader["SellingPrice"]) : 0;

                                txtcost.Text = cost.ToString("N2");
                                txtsellingprice.Text = sellingPrice.ToString("N2");

                            }
                            else
                            {
                                txtYOM.Focus();
                                MessageBox.Show("Vehicle not found!");
                            }
                        }
                    }
                }
            }
        }


        private Image ByteArrayToImage(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }

        // SAFER image to byte array conversion using PNG format
        private byte[] ImageToByteArray1(Image image)
        {
            if (image == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = @"UPDATE VehicleDetails SET 
                VehicleMake = @Make,
                VehicleModel = @Model,
                YOM = @YOM,
                ROM = @ROM,
                Fuel = @Fuel,
                EngineCapacity = @EngineCapacity,
                Mileage = @Mileage,
                ChassisNumber = @ChassisNo,
                EngineNumber = @EngineNo,
                BodyType = @BodyType,
                Colour = @Colour,
                ConditionType = @ConditionType,
                Cost = @Cost,
                SellingPrice = @SellingPrice
            WHERE RegisteredNumber = @RegNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Get values
                        string regNumber = txtRegisteredNumber.Text.Trim();
                        if (string.IsNullOrWhiteSpace(regNumber))
                        {
                            MessageBox.Show("Registered Number is required to update.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Validate input
                        if (!int.TryParse(txtYOM.Text.Trim(), out int yom) ||
                            !int.TryParse(txtROM.Text.Trim(), out int rom) ||
                            !decimal.TryParse(txtcost.Text.Trim(), out decimal cost) ||
                            !decimal.TryParse(txtsellingprice.Text.Trim(), out decimal sellingPrice))
                        {
                            MessageBox.Show("Ensure numeric fields (YOM, ROM, Cost, Selling Price) are valid numbers.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (pictureBox1.Image == null || pictureBox2.Image == null || pictureBox3.Image == null)
                        {
                            MessageBox.Show("Please upload all 3 images before updating.", "Missing Image", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Assign parameters
                        cmd.Parameters.AddWithValue("@Make", txtMake.Text.Trim());
                        cmd.Parameters.AddWithValue("@Model", txtModel.Text.Trim());
                        cmd.Parameters.AddWithValue("@YOM", yom);
                        cmd.Parameters.AddWithValue("@ROM", rom);
                        cmd.Parameters.AddWithValue("@Fuel", txtFuel.Text.Trim());
                        cmd.Parameters.AddWithValue("@EngineCapacity", txtEngineCapacity.Text.Trim());
                        cmd.Parameters.AddWithValue("@Mileage", txtMileage.Text.Trim());
                        cmd.Parameters.AddWithValue("@ChassisNo", txtChassisNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@EngineNo", txtEngineNumber.Text.Trim());
                        cmd.Parameters.AddWithValue("@BodyType", txtBodyType.Text.Trim());
                        cmd.Parameters.AddWithValue("@Colour", txtColour.Text.Trim());
                        cmd.Parameters.AddWithValue("@ConditionType", txtConditionType.Text.Trim());
                        cmd.Parameters.AddWithValue("@RegNumber", regNumber);
                        cmd.Parameters.AddWithValue("@Cost", cost);
                        cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (result > 0)
                        {
                            MessageBox.Show("Vehicle details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("No matching vehicle found with that Registered Number.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error:\n" + sqlEx.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unexpected error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btndashboard_Click(object sender, EventArgs e)
        {
            SystemDashboard dash = new SystemDashboard();
            dash.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Statistics s = new Statistics();
            s.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sales_History sale = new Sales_History();
            sale.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            logout log = new logout();
            log.Show();
            this.Hide();
        }

        private Image LoadImageSafely(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Image.FromStream(fs).Clone() as Image;
            }
        }

        private void txtMake_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtRegisteredNumber.Focus();
            }
        }

        private void txtYOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtROM.Focus();
            }
        }

        private void txtROM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtEngineCapacity.Focus();
            }
        }

        private void txtEngineCapacity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtFuel.Focus();
            }
        }

        private void txtFuel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtMileage.Focus();
            }
        }

        private void txtMileage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtConditionType.Focus();
            }
        }

        private void txtConditionType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtModel.Focus();
            }
        }

        private void txtModel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtChassisNumber.Focus();
            }
        }

        private void txtChassisNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtEngineNumber.Focus();
            }
        }

        private void txtEngineNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtBodyType.Focus();
            }
        }

        private void txtBodyType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtColour.Focus();
            }
        }

        private void txtColour_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtcost.Focus();
            }
        }

        private void txtcost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtsellingprice.Focus();
            }
        }
    }

}
