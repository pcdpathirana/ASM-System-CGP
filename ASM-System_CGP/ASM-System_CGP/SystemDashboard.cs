using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;


namespace WindowsFormsApp1
{
    public partial class SystemDashboard : Form
    {
        string connstring = "Server=localhost;Database=vehicle;User Id=sa;Password=3323;";
        SqlConnection conn;



        public SystemDashboard()
        {
            InitializeComponent();
            conn = new SqlConnection(connstring);
            LoadCashValueToLabel();
            LoadWeeklyChart();


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

        private void btndashboard_Click(object sender, EventArgs e)
        {
            SystemDashboard dash = new SystemDashboard();
            dash.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Vehicle add = new Add_Vehicle();
            add.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Purchase_Details pu = new Purchase_Details();
            pu.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Show_Vehicles show = new Show_Vehicles();
            show.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            createReservation create = new createReservation();
            create.Show();
        }

        private void LoadCashValueToLabel()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string query = "SELECT cash FROM CashValue WHERE id = 1";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            decimal cashValue = Convert.ToDecimal(result);
                            lblcashvalue.Text = "Rs. " + cashValue.ToString("N2");
                        }
                        else
                        {
                            lblcashvalue.Text = "Rs. 0.00";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading cash value: " + ex.Message);
                lblcashvalue.Text = "Rs. N/A";
            }
        }



        private void button5_Click(object sender, EventArgs e)
        {
            decimal addvalue;
            if (decimal.TryParse(txtcashvalue.Text, out addvalue))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        conn.Open();

                        string selectQuery = "SELECT cash FROM cashValue WHERE id = 1";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
                        object result = selectCmd.ExecuteScalar();

                        decimal currentValue = result != null ? Convert.ToDecimal(result) : 0.00m;
                        decimal updatedValue = currentValue + addvalue;

                        string updateQuery = "UPDATE CashValue SET cash = @updatedValue WHERE id = 1";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@updatedValue", updatedValue);

                        updateCmd.ExecuteNonQuery();
                        LoadCashValueToLabel();
                        txtcashvalue.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating cash value: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid selling price. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            decimal addvalue;
            if (decimal.TryParse(txtcashvalue.Text, out addvalue))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        conn.Open();

                        string selectQuery = "SELECT cash FROM cashValue WHERE id = 1";
                        SqlCommand selectCmd = new SqlCommand(selectQuery, conn);
                        object result = selectCmd.ExecuteScalar();

                        decimal currentValue = result != null ? Convert.ToDecimal(result) : 0.00m;
                        decimal updatedValue = currentValue - addvalue;

                        string updateQuery = "UPDATE CashValue SET cash = @updatedValue WHERE id = 1";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@updatedValue", updatedValue);

                        updateCmd.ExecuteNonQuery();
                        LoadCashValueToLabel();
                        txtcashvalue.Clear();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating cash value: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid selling price. Please enter a valid number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Sales_History sale = new Sales_History();
            sale.Show();
            this.Hide();

        }

        private void button11_Click(object sender, EventArgs e)
        {
            useraccount user = new useraccount();
            user.ShowDialog();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            shopdetails shop = new shopdetails();
            shop.ShowDialog();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to run the Day-End Process?",
                                         "Confirm Day-End",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RunDayEndProcess();
            }
            else
            {
                MessageBox.Show("Day-End Process cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RunDayEndProcess()
        {
            decimal totalSales = 0;
            decimal totalPurchases = 0;
            decimal cashOnHand = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    // Get today's totals
                    string salesQuery = "SELECT ISNULL(SUM(SellingPrice), 0) FROM PurchasingDetails WHERE CAST(PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)";
                    string purchaseQuery = "SELECT ISNULL(SUM(v.Cost), 0) FROM VehicleDetails v WHERE v.RegisteredNumber IN (   SELECT p.VehicleRegNumber  FROM PurchasingDetails p   WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE))";
                    string cashQuery = "SELECT ISNULL(cash, 0) FROM CashValue WHERE Id = 1";

                    using (SqlCommand cmd = new SqlCommand(salesQuery, conn))
                        totalSales = Convert.ToDecimal(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand(purchaseQuery, conn))
                        totalPurchases = Convert.ToDecimal(cmd.ExecuteScalar());

                    using (SqlCommand cmd = new SqlCommand(cashQuery, conn))
                        cashOnHand = Convert.ToDecimal(cmd.ExecuteScalar());

                    // Insert into DayEndSummary
                    string insertSummary = @"
                INSERT INTO DayEndSummary (DayDate, TotalSales, TotalPurchases, CashOnHand, Notes)
                VALUES (@DayDate, @TotalSales, @TotalPurchases, @CashOnHand, @Notes)";

                    using (SqlCommand cmd = new SqlCommand(insertSummary, conn))
                    {
                        cmd.Parameters.AddWithValue("@DayDate", DateTime.Today);
                        cmd.Parameters.AddWithValue("@TotalSales", totalSales);
                        cmd.Parameters.AddWithValue("@TotalPurchases", totalPurchases);
                        cmd.Parameters.AddWithValue("@CashOnHand", cashOnHand);
                        cmd.Parameters.AddWithValue("@Notes", "Automated day-end closing.");

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Day-End process completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Day-End Process failed:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RunMonthEndProcess()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    // Step 1: Get total sales for the current month
                    string salesQuery = @"SELECT ISNULL(SUM(SellingPrice), 0) 
                                  FROM PurchasingDetails
                                  WHERE MONTH(PurchaseDate) = MONTH(GETDATE()) 
                                  AND YEAR(PurchaseDate) = YEAR(GETDATE())";

                    SqlCommand cmd = new SqlCommand(salesQuery, conn);
                    decimal totalSales = Convert.ToDecimal(cmd.ExecuteScalar());

                    // Step 2: Insert summary into MonthEndLog
                    string insertLog = @"INSERT INTO MonthEndLog (MonthName, TotalSales, PerformedBy) 
                                 VALUES (@MonthName, @TotalSales, @User)";

                    SqlCommand insertCmd = new SqlCommand(insertLog, conn);
                    insertCmd.Parameters.AddWithValue("@MonthName", DateTime.Now.ToString("MMMM"));
                    insertCmd.Parameters.AddWithValue("@TotalSales", totalSales);
                    insertCmd.Parameters.AddWithValue("@User", Environment.UserName); // or your login system username

                    insertCmd.ExecuteNonQuery();

                    // Optional: Archive or clean up data here

                    MessageBox.Show("Month-End Process completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Month-End Process: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to run the Month-End Process?",
                                         "Confirm Day-End",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RunMonthEndProcess();
            }
            else
            {
                MessageBox.Show("Month-End Process cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RunYearEndProcess()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();

                    // Get current year
                    int currentYear = DateTime.Now.Year;

                    // 1. Get Total Sales
                    string salesQuery = @"SELECT ISNULL(SUM(SellingPrice), 0) 
                                  FROM PurchasingDetails 
                                  WHERE YEAR(PurchaseDate) = @Year";
                    SqlCommand salesCmd = new SqlCommand(salesQuery, conn);
                    salesCmd.Parameters.AddWithValue("@Year", currentYear);
                    decimal totalSales = Convert.ToDecimal(salesCmd.ExecuteScalar());

                    // 2. Get Total Cost
                    string costQuery = @"
    SELECT ISNULL(SUM(v.Cost), 0)
    FROM VehicleDetails v
    WHERE v.RegisteredNumber IN (
        SELECT p.VehicleRegNumber
        FROM PurchasingDetails p
        WHERE CAST(p.PurchaseDate AS DATE) = CAST(GETDATE() AS DATE)
    )";
                    SqlCommand costCmd = new SqlCommand(costQuery, conn);
                    costCmd.Parameters.AddWithValue("@Year", currentYear);
                    decimal totalCost = Convert.ToDecimal(costCmd.ExecuteScalar());

                    decimal profit = totalSales - totalCost;

                    // 3. Insert into YearEndLog
                    string insertQuery = @"INSERT INTO YearEndLog (Year, TotalSales, TotalCost, Profit, PerformedBy)
                                   VALUES (@Year, @Sales, @Cost, @Profit, @User)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Year", currentYear);
                    insertCmd.Parameters.AddWithValue("@Sales", totalSales);
                    insertCmd.Parameters.AddWithValue("@Cost", totalCost);
                    insertCmd.Parameters.AddWithValue("@Profit", profit);
                    insertCmd.Parameters.AddWithValue("@User", Environment.UserName); // or your custom login

                    insertCmd.ExecuteNonQuery();

                    // Optional: Archive or reset data if needed

                    MessageBox.Show("Year-End Process completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during Year-End Process: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to run the Year-End Process?",
                                          "Confirm Month-End",
                                          MessageBoxButtons.YesNo,
                                          MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                RunYearEndProcess();
            }
            else
            {
                MessageBox.Show("Year-End Process cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void LoadWeeklyChart()
        {
            Dictionary<string, decimal> salesData = new Dictionary<string, decimal>();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string query = @"
SELECT WeekDay, TotalSales
FROM (
    SELECT 
        DATENAME(WEEKDAY, PurchaseDate) AS WeekDay,
        DATEPART(WEEKDAY, PurchaseDate) AS WeekDayOrder,
        SUM(SellingPrice) AS TotalSales
    FROM PurchasingDetails
    WHERE PurchaseDate >= DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
      AND PurchaseDate < DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
    GROUP BY 
        DATENAME(WEEKDAY, PurchaseDate),
        DATEPART(WEEKDAY, PurchaseDate)
) AS WeeklySales
ORDER BY WeekDayOrder";


                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    salesData[reader["WeekDay"].ToString()] = Convert.ToDecimal(reader["TotalSales"]);
                }
            }

            // Clear previous data
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();

            ChartArea area = new ChartArea("MainArea");
            area.AxisX.Title = "Day";
            area.AxisY.Title = "Sales Amount";
            chart1.ChartAreas.Add(area);

            Series series = new Series("Weekly Sales");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SteelBlue;
            series.IsValueShownAsLabel = true;

            foreach (var day in salesData)
            {
                series.Points.AddXY(day.Key, day.Value);
            }

            chart1.Series.Add(series);
            chart1.Legends[0].Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Statistics s = new Statistics();
            s.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            createcustomer cus = new createcustomer();
            cus.ShowDialog();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            createpayment pay = new createpayment();
            pay.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            logout log = new logout();
            log.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void lblcashvalue_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtcashvalue_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelChart_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button15_Click_1(object sender, EventArgs e)
        {

        }

        private void lblcashvalue_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void panelChart_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button16_Click_1(object sender, EventArgs e)
        {

        }

        private void button17_Click_1(object sender, EventArgs e)
        {

        }

        private void button14_Click_1(object sender, EventArgs e)
        {

        }

        private void button13_Click_1(object sender, EventArgs e)
        {

        }

        private void button12_Click_1(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void panel6_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void txtcashvalue_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void panel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnmaxmin_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnminimize_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void btndashboard_Click_1(object sender, EventArgs e)
        {

        }
    }

}
