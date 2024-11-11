using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAL;
using WindowsFormsApp.DAL.Models;

namespace WindowsFormsApp
{
    public class CustomerPurchasesForm : Form
    {
        private PrintDocument printDocument;

        public CustomerPurchasesForm()
        {
            InitializeComponent();
            LoadAgents();
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.cboAgent = new ComboBox();
            this.lblAgent = new Label();
            this.btnSearch = new Button();
            this.btnPrint = new Button();
            this.dgvPurchases = new DataGridView();
            this.printDocument = new PrintDocument();

            // Form
            this.Text = "Customer Purchase History";
            this.Size = new Size(900, 600);
            this.MinimumSize = new Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Panel
            this.panel1.Top = 0;
            this.panel1.Left = 0;
            this.panel1.Width = this.ClientSize.Width;
            this.panel1.Height = 50;
            this.panel1.BackColor = SystemColors.Control;

            // Label
            this.lblAgent.Text = "Select Agent:";
            this.lblAgent.Location = new Point(10, 15);
            this.lblAgent.AutoSize = true;

            // ComboBox
            this.cboAgent.Location = new Point(85, 12);
            this.cboAgent.Size = new Size(200, 23);
            this.cboAgent.DropDownStyle = ComboBoxStyle.DropDownList;

            // Buttons
            this.btnSearch.Text = "Search";
            this.btnSearch.Location = new Point(295, 11);
            this.btnSearch.Size = new Size(75, 25);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            this.btnPrint.Text = "Print";
            this.btnPrint.Location = new Point(380, 11);
            this.btnPrint.Size = new Size(75, 25);
            this.btnPrint.Click += new EventHandler(btnPrint_Click);

            // DataGridView
            this.dgvPurchases.Top = this.panel1.Bottom;
            this.dgvPurchases.Left = 0;
            this.dgvPurchases.Width = this.ClientSize.Width;
            this.dgvPurchases.Height = this.ClientSize.Height - this.panel1.Height;
            this.dgvPurchases.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.dgvPurchases.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPurchases.AllowUserToAddRows = false;
            this.dgvPurchases.ReadOnly = true;
            this.dgvPurchases.BackgroundColor = Color.White;
            this.dgvPurchases.RowHeadersVisible = false;
            this.dgvPurchases.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurchases.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            // Panel Controls
            this.panel1.Controls.AddRange(new Control[] {
                this.lblAgent,
                this.cboAgent,
                this.btnSearch,
                this.btnPrint
            });

            // Form Controls
            this.Controls.AddRange(new Control[] {
                this.panel1,
                this.dgvPurchases
            });

            // Anchor panel
            this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            // Setup columns
            SetupDataGridView();

            // Handle resize
            this.Resize += new EventHandler(CustomerPurchasesForm_Resize);
        }
        private void SetupDataGridView()
        {
            dgvPurchases.AutoGenerateColumns = false;

            dgvPurchases.Columns.Clear();
            dgvPurchases.Columns.AddRange(new DataGridViewColumn[] {
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderDate",
                    HeaderText = "Order Date",
                    Name = "OrderDate",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ItemName",
                    HeaderText = "Item",
                    Name = "ItemName"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity",
                    HeaderText = "Quantity",
                    Name = "Quantity"
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "UnitAmount",
                    HeaderText = "Unit Price",
                    Name = "UnitAmount",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "TotalAmount",
                    HeaderText = "Total",
                    Name = "TotalAmount",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                }
            });

            foreach (DataGridViewColumn column in dgvPurchases.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void CustomerPurchasesForm_Resize(object sender, EventArgs e)
        {
            // Update grid size on form resize
            dgvPurchases.Width = this.ClientSize.Width;
            dgvPurchases.Height = this.ClientSize.Height - panel1.Height;
        }
        private void LoadAgents()
        {
            try
            {
                cboAgent.DisplayMember = "AgentName";
                cboAgent.ValueMember = "AgentID";
                cboAgent.DataSource = DatabaseHelper.GetAllAgents();
                dgvPurchases.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading agents: {ex.Message}", "Error");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboAgent.SelectedValue == null)
                {
                    MessageBox.Show("Please select an agent", "Validation Error");
                    return;
                }

                int agentId = (int)cboAgent.SelectedValue;
                var purchaseHistory = DatabaseHelper.GetAgentPurchaseHistory(agentId);

                if (purchaseHistory.Rows.Count == 0)
                {
                    MessageBox.Show("No purchase history found for this agent.", "Information",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dgvPurchases.DataSource = purchaseHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching purchases: {ex.Message}", "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPurchases.Rows.Count == 0)
                {
                    MessageBox.Show("No data to print", "Information");
                    return;
                }

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
                string title = $"Purchase History - {cboAgent.Text}";
                e.Graphics.DrawString(title, titleFont, Brushes.Black, xPos, yPos);
                yPos += 40;

                // Print headers
                for (int i = 0; i < dgvPurchases.Columns.Count; i++)
                {
                    e.Graphics.DrawString(
                        dgvPurchases.Columns[i].HeaderText,
                        headerFont,
                        Brushes.Black,
                        xPos + (i * 150),
                        yPos
                    );
                }
                yPos += lineHeight + 10;

                // Print data
                foreach (DataGridViewRow row in dgvPurchases.Rows)
                {
                    if (row.Cells[0].Value != null) // Check if the row has data
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string value = row.Cells[i].Value?.ToString() ?? "";
                            if (i == 0 && DateTime.TryParse(value, out DateTime date))
                            {
                                value = date.ToString("dd/MM/yyyy HH:mm");
                            }
                            else if (i == 3 || i == 4) // Unit Amount and Total Amount columns
                            {
                                if (decimal.TryParse(value, out decimal amount))
                                {
                                    value = amount.ToString("C2");
                                }
                            }

                            e.Graphics.DrawString(
                                value,
                                contentFont,
                                Brushes.Black,
                                xPos + (i * 150),
                                yPos
                            );
                        }
                        yPos += lineHeight;
                    }
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

        private Panel panel1;
        private ComboBox cboAgent;
        private Label lblAgent;
        private Button btnSearch;
        private Button btnPrint;
        private DataGridView dgvPurchases;
    }
}
