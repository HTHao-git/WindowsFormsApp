using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAL.Models;
using WindowsFormsApp.DAL;
using System.Drawing.Printing;

namespace WindowsFormsApp
{
    public class OrderForm : Form
    {
        private List<OrderDetailViewModel> orderDetails = new List<OrderDetailViewModel>();
        private List<Item> itemsList;

        // View model for displaying order details in grid
        public class OrderDetailViewModel
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public decimal UnitAmount { get; set; }
            public decimal TotalAmount { get { return Quantity * UnitAmount; } }
        }

        public OrderForm()
        {
            InitializeComponent();
            LoadInitialData();
            SetupDataGridView();
        }

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAgent = new System.Windows.Forms.Label();
            this.cboAgent = new System.Windows.Forms.ComboBox();
            this.btnSaveOrder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblItem = new System.Windows.Forms.Label();
            this.cboItem = new System.Windows.Forms.ComboBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.lblUnitPrice = new System.Windows.Forms.Label();
            this.txtUnitPrice = new System.Windows.Forms.TextBox();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.dgvOrderDetails = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboAgent);
            this.panel1.Controls.Add(this.btnSaveOrder);
            this.panel1.Controls.Add(this.lblAgent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 60);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(884, 60);
            this.panel1.TabIndex = 0;
            // 
            // lblAgent
            // 
            this.lblAgent.Location = new System.Drawing.Point(20, 20);
            this.lblAgent.Name = "lblAgent";
            this.lblAgent.Size = new System.Drawing.Size(100, 23);
            this.lblAgent.TabIndex = 0;
            this.lblAgent.Text = "Select Agent:";
            // 
            // cboAgent
            // 
            this.cboAgent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAgent.Location = new System.Drawing.Point(100, 17);
            this.cboAgent.Name = "cboAgent";
            this.cboAgent.Size = new System.Drawing.Size(200, 20);
            this.cboAgent.TabIndex = 1;
            // 
            // btnSaveOrder
            // 
            this.btnSaveOrder.Location = new System.Drawing.Point(680, 15);
            this.btnSaveOrder.Name = "btnSaveOrder";
            this.btnSaveOrder.Size = new System.Drawing.Size(75, 23);
            this.btnSaveOrder.TabIndex = 2;
            this.btnSaveOrder.Text = "Save Order";
            this.btnSaveOrder.Click += new System.EventHandler(this.btnSaveOrder_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cboItem);
            this.panel2.Controls.Add(this.txtQuantity);
            this.panel2.Controls.Add(this.txtUnitPrice);
            this.panel2.Controls.Add(this.btnAddItem);
            this.panel2.Controls.Add(this.lblItem);
            this.panel2.Controls.Add(this.lblQuantity);
            this.panel2.Controls.Add(this.lblUnitPrice);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(884, 60);
            this.panel2.TabIndex = 1;
            // 
            // lblItem
            // 
            this.lblItem.Location = new System.Drawing.Point(20, 20);
            this.lblItem.Name = "lblItem";
            this.lblItem.Size = new System.Drawing.Size(100, 23);
            this.lblItem.TabIndex = 0;
            this.lblItem.Text = "Select Item:";
            // 
            // cboItem
            // 
            this.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboItem.Location = new System.Drawing.Point(100, 17);
            this.cboItem.Name = "cboItem";
            this.cboItem.Size = new System.Drawing.Size(300, 20);
            this.cboItem.TabIndex = 1;
            this.cboItem.SelectedIndexChanged += new System.EventHandler(this.cboItem_SelectedIndexChanged);
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(420, 20);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(100, 23);
            this.lblQuantity.TabIndex = 2;
            this.lblQuantity.Text = "Quantity:";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(480, 17);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(80, 22);
            this.txtQuantity.TabIndex = 3;
            // 
            // lblUnitPrice
            // 
            this.lblUnitPrice.Location = new System.Drawing.Point(580, 20);
            this.lblUnitPrice.Name = "lblUnitPrice";
            this.lblUnitPrice.Size = new System.Drawing.Size(100, 23);
            this.lblUnitPrice.TabIndex = 4;
            this.lblUnitPrice.Text = "Unit Price:";
            // 
            // txtUnitPrice
            // 
            this.txtUnitPrice.Location = new System.Drawing.Point(640, 17);
            this.txtUnitPrice.Name = "txtUnitPrice";
            this.txtUnitPrice.ReadOnly = true;
            this.txtUnitPrice.Size = new System.Drawing.Size(80, 22);
            this.txtUnitPrice.TabIndex = 5;
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(730, 15);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(75, 23);
            this.btnAddItem.TabIndex = 6;
            this.btnAddItem.Text = "Add Item";
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // dgvOrderDetails
            // 
            this.dgvOrderDetails.AllowUserToAddRows = false;
            this.dgvOrderDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrderDetails.Location = new System.Drawing.Point(0, 0);
            this.dgvOrderDetails.Name = "dgvOrderDetails";
            this.dgvOrderDetails.Size = new System.Drawing.Size(884, 561);
            this.dgvOrderDetails.TabIndex = 2;
            // 
            // OrderForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.dgvOrderDetails);
            this.Name = "OrderForm";
            this.Text = "Create Order";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetails)).EndInit();
            this.ResumeLayout(false);

        }

        private void SetupDataGridView()
        {
            dgvOrderDetails.AutoGenerateColumns = false;

            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ItemName",
                HeaderText = "Item Name",
                Name = "ItemName"
            });

            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity",
                HeaderText = "Quantity",
                Name = "Quantity"
            });

            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitAmount",
                HeaderText = "Unit Price",
                Name = "UnitAmount",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalAmount",
                HeaderText = "Total",
                Name = "TotalAmount",
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });
        }

        private void LoadInitialData()
        {
            try
            {
                // Load Agents
                cboAgent.DisplayMember = "AgentName";
                cboAgent.ValueMember = "AgentID";
                cboAgent.DataSource = DatabaseHelper.GetAllAgents();

                // Load Items
                itemsList = DatabaseHelper.GetAllItemsList(); // We'll add this method to DatabaseHelper
                cboItem.DisplayMember = "ItemName";
                cboItem.ValueMember = "ItemID";
                cboItem.DataSource = itemsList;

                RefreshOrderDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error");
            }
        }

        private void cboItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboItem.SelectedItem is Item selectedItem)
            {
                txtUnitPrice.Text = selectedItem.Price.ToString("N2");
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboItem.SelectedItem == null)
                {
                    MessageBox.Show("Please select an item", "Validation Error");
                    return;
                }

                if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
                {
                    MessageBox.Show("Please enter a valid quantity", "Validation Error");
                    return;
                }

                var selectedItem = (Item)cboItem.SelectedItem;

                var detail = new OrderDetailViewModel
                {
                    ItemID = selectedItem.ItemID,
                    ItemName = selectedItem.ItemName,
                    Quantity = quantity,
                    UnitAmount = selectedItem.Price
                };

                orderDetails.Add(detail);
                RefreshOrderDetails();
                ClearItemInputs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding item: {ex.Message}", "Error");
            }
        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (orderDetails.Count == 0)
                {
                    MessageBox.Show("Please add items to the order", "Validation Error");
                    return;
                }

                var order = new Order
                {
                    AgentID = (int)cboAgent.SelectedValue,
                    OrderDate = DateTime.Now,
                    OrderDetails = orderDetails.ConvertAll(od => new OrderDetail
                    {
                        ItemID = od.ItemID,
                        Quantity = od.Quantity,
                        UnitAmount = od.UnitAmount
                    })
                };

                int orderId = DatabaseHelper.CreateOrder(order);
                MessageBox.Show($"Order {orderId} created successfully!");

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving order: {ex.Message}", "Error");
            }
        }

        private void RefreshOrderDetails()
        {
            dgvOrderDetails.DataSource = null;
            dgvOrderDetails.DataSource = orderDetails;
        }

        private void ClearItemInputs()
        {
            cboItem.SelectedIndex = -1;
            txtQuantity.Clear();
            txtUnitPrice.Clear();
        }

        private Panel panel1;
        private Panel panel2;
        private ComboBox cboAgent;
        private ComboBox cboItem;
        private TextBox txtQuantity;
        private TextBox txtUnitPrice;
        private Button btnAddItem;
        private Button btnSaveOrder;
        private DataGridView dgvOrderDetails;
        private Label lblAgent;
        private Label lblItem;
        private Label lblQuantity;
        private Label lblUnitPrice;
    }
}
