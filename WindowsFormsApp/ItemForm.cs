using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAL;
using WindowsFormsApp.DAL.Models;

namespace WindowsFormsApp
{
    public partial class ItemForm : Form
    {
        public ItemForm()
        {
            InitializeComponent();
            LoadItems();
        }

        private void InitializeComponent()
        {
            this.dgvItems = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblItemName = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToAddRows = false;
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvItems.Size = new System.Drawing.Size(784, 561);
            this.dgvItems.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtItemName);
            this.panel1.Controls.Add(this.txtSize);
            this.panel1.Controls.Add(this.txtPrice);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lblPrice);
            this.panel1.Controls.Add(this.lblItemName);
            this.panel1.Controls.Add(this.lblSize);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(784, 100);
            this.panel1.TabIndex = 0;
            // 
            // lblItemName
            // 
            this.lblItemName.Location = new System.Drawing.Point(20, 20);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(100, 23);
            this.lblItemName.TabIndex = 0;
            this.lblItemName.Text = "Item Name:";
            // 
            // lblSize
            // 
            this.lblSize.Location = new System.Drawing.Point(20, 50);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(100, 23);
            this.lblSize.TabIndex = 1;
            this.lblSize.Text = "Size:";
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(318, 17);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(100, 23);
            this.lblPrice.TabIndex = 2;
            this.lblPrice.Text = "Price:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(100, 17);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(180, 22);
            this.txtItemName.TabIndex = 3;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(100, 47);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(180, 22);
            this.txtSize.TabIndex = 4;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(360, 17);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 22);
            this.txtPrice.TabIndex = 5;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(402, 64);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ItemForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvItems);
            this.Name = "ItemForm";
            this.Text = "Manage Items";
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void LoadItems()
        {
            dgvItems.DataSource = DatabaseHelper.GetAllItems();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtItemName.Text))
                {
                    MessageBox.Show("Please enter item name", "Validation Error");
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Please enter a valid price", "Validation Error");
                    return;
                }

                var item = new Item
                {
                    ItemName = txtItemName.Text,
                    Size = txtSize.Text,
                    Price = price
                };

                DatabaseHelper.AddItem(item);
                MessageBox.Show("Item saved successfully!");

                ClearForm();
                LoadItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving item: {ex.Message}", "Error");
            }
        }

        private void ClearForm()
        {
            txtItemName.Clear();
            txtSize.Clear();
            txtPrice.Clear();
        }

        private DataGridView dgvItems;
        private Panel panel1;
        private TextBox txtItemName;
        private TextBox txtSize;
        private TextBox txtPrice;
        private Button btnSave;
        private Label lblItemName;
        private Label lblSize;
        private Label lblPrice;
    }
}
