using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAL;

namespace WindowsFormsApp
{
    public class BestItemsReportForm : Form
    {
        private PrintDocument printDocument;

        public BestItemsReportForm()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.tabControl = new TabControl();
            this.tabBestItems = new TabPage();
            this.tabAllData = new TabPage();
            this.panel1 = new Panel();
            this.btnPrint = new Button();
            this.dgvBestItems = new DataGridView();
            this.dgvAllData = new DataGridView();
            this.printDocument = new PrintDocument();

            // TabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Controls.Add(tabBestItems);
            this.tabControl.Controls.Add(tabAllData);

            // TabPages
            this.tabBestItems.Text = "Best Selling Items";
            this.tabBestItems.Padding = new Padding(3);
            this.tabAllData.Text = "All Orders";
            this.tabAllData.Padding = new Padding(3);

            // Panel
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Height = 50;
            this.panel1.Padding = new Padding(10);

            // Print Button
            this.btnPrint.Text = "Print Report";
            this.btnPrint.Location = new Point(20, 15);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

            // DataGridViews
            this.dgvBestItems.Dock = DockStyle.Fill;
            this.dgvBestItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBestItems.AllowUserToAddRows = false;
            this.dgvBestItems.ReadOnly = true;

            this.dgvAllData.Dock = DockStyle.Fill;
            this.dgvAllData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAllData.AllowUserToAddRows = false;
            this.dgvAllData.ReadOnly = true;

            // Add controls to tabs
            this.tabBestItems.Controls.Add(this.dgvBestItems);
            this.tabAllData.Controls.Add(this.dgvAllData);

            // Add controls to panel
            this.panel1.Controls.Add(this.btnPrint);

            // BestItemsReportForm
            this.Text = "Items Report";
            this.Size = new Size(1000, 600);
            this.Controls.AddRange(new Control[] { this.panel1, this.tabControl });
        }

        private void LoadData()
        {
            try
            {
                dgvBestItems.DataSource = DatabaseHelper.GetBestSellingItems();
                dgvAllData.DataSource = DatabaseHelper.GetAllOrdersDetailed();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                printDialog.Document = printDocument;

                if (printDialog.ShowDialog() == DialogResult.OK)
                {
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing: {ex.Message}", "Error");
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                // Print settings
                Font titleFont = new Font("Arial", 18, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font contentFont = new Font("Arial", 10);
                int yPos = 50;
                int xPos = 50;
                int lineHeight = 20;

                // Print title
                e.Graphics.DrawString("Best Selling Items Report", titleFont, Brushes.Black, xPos, yPos);
                yPos += 40;

                // Print headers
                string[] headers = new string[] { "Item Name", "Total Quantity", "Total Revenue" };
                for (int i = 0; i < headers.Length; i++)
                {
                    e.Graphics.DrawString(headers[i], headerFont, Brushes.Black, xPos + (i * 200), yPos);
                }
                yPos += lineHeight + 10;

                // Print data
                foreach (DataGridViewRow row in dgvBestItems.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        e.Graphics.DrawString(
                            row.Cells[i].Value?.ToString() ?? "",
                            contentFont,
                            Brushes.Black,
                            xPos + (i * 200),
                            yPos
                        );
                    }
                    yPos += lineHeight;
                }

                // Add date and time
                yPos += 20;
                e.Graphics.DrawString(
                    $"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                    contentFont,
                    Brushes.Black,
                    xPos,
                    yPos
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating print layout: {ex.Message}", "Error");
            }
        }

        private TabControl tabControl;
        private TabPage tabBestItems;
        private TabPage tabAllData;
        private Panel panel1;
        private Button btnPrint;
        private DataGridView dgvBestItems;
        private DataGridView dgvAllData;
    }
}