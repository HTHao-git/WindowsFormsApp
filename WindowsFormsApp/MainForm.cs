using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.menuStrip1 = new MenuStrip();
            this.masterDataToolStripMenuItem = new ToolStripMenuItem();
            this.itemsToolStripMenuItem = new ToolStripMenuItem();
            this.agentsToolStripMenuItem = new ToolStripMenuItem();
            this.transactionsToolStripMenuItem = new ToolStripMenuItem();
            this.newOrderToolStripMenuItem = new ToolStripMenuItem();
            this.reportsToolStripMenuItem = new ToolStripMenuItem();
            this.bestItemsToolStripMenuItem = new ToolStripMenuItem();
            this.customerPurchasesToolStripMenuItem = new ToolStripMenuItem();

            // MenuStrip
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
                this.masterDataToolStripMenuItem,
                this.transactionsToolStripMenuItem,
                this.reportsToolStripMenuItem
            });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Size = new Size(800, 24);

            // Master Data Menu
            this.masterDataToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.itemsToolStripMenuItem,
                this.agentsToolStripMenuItem
            });
            this.masterDataToolStripMenuItem.Text = "Master Data";

            // Items Menu Item
            this.itemsToolStripMenuItem.Text = "Items";
            this.itemsToolStripMenuItem.Click += new EventHandler(itemsToolStripMenuItem_Click);

            // Agents Menu Item
            this.agentsToolStripMenuItem.Text = "Agents";
            this.agentsToolStripMenuItem.Click += new EventHandler(agentsToolStripMenuItem_Click);

            // Transactions Menu
            this.transactionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.newOrderToolStripMenuItem
            });
            this.transactionsToolStripMenuItem.Text = "Transactions";

            // New Order Menu Item
            this.newOrderToolStripMenuItem.Text = "New Order";
            this.newOrderToolStripMenuItem.Click += new EventHandler(newOrderToolStripMenuItem_Click);

            // Reports Menu
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.bestItemsToolStripMenuItem,
                this.customerPurchasesToolStripMenuItem
            });
            this.reportsToolStripMenuItem.Text = "Reports";

            // Best Items Menu Item
            this.bestItemsToolStripMenuItem.Text = "Best Items";
            this.bestItemsToolStripMenuItem.Click += new EventHandler(bestItemsToolStripMenuItem_Click);

            // Customer Purchases Menu Item
            this.customerPurchasesToolStripMenuItem.Text = "Customer Purchases";
            this.customerPurchasesToolStripMenuItem.Click += new EventHandler(customerPurchasesToolStripMenuItem_Click);

            // MainForm
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Order Management System";
        }

        private void itemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemForm itemForm = new ItemForm();
            ShowForm(itemForm);
        }

        private void agentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AgentForm agentForm = new AgentForm();
            ShowForm(agentForm);
        }

        private void newOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            ShowForm(orderForm);
        }

        private void bestItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BestItemsReportForm reportForm = new BestItemsReportForm();
            ShowForm(reportForm);
        }

        private void customerPurchasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerPurchasesForm purchasesForm = new CustomerPurchasesForm();
            ShowForm(purchasesForm);
        }

        private void ShowForm(Form form)
        {
            form.MdiParent = this;
            form.Show();
        }

        private MenuStrip menuStrip1;
        private ToolStripMenuItem masterDataToolStripMenuItem;
        private ToolStripMenuItem itemsToolStripMenuItem;
        private ToolStripMenuItem agentsToolStripMenuItem;
        private ToolStripMenuItem transactionsToolStripMenuItem;
        private ToolStripMenuItem newOrderToolStripMenuItem;
        private ToolStripMenuItem reportsToolStripMenuItem;
        private ToolStripMenuItem bestItemsToolStripMenuItem;
        private ToolStripMenuItem customerPurchasesToolStripMenuItem;
    }
}
