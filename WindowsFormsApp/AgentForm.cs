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
    public class AgentForm : Form
    {
        public AgentForm()
        {
            InitializeComponent();
            LoadAgents();
        }

        private void InitializeComponent()
        {
            this.dgvAgents = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAgentName = new System.Windows.Forms.Label();
            this.lblAddress = new System.Windows.Forms.Label();
            this.txtAgentName = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgents)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvAgents
            // 
            this.dgvAgents.AllowUserToAddRows = false;
            this.dgvAgents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAgents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAgents.Location = new System.Drawing.Point(0, 0);
            this.dgvAgents.Name = "dgvAgents";
            this.dgvAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAgents.Size = new System.Drawing.Size(784, 561);
            this.dgvAgents.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAgentName);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.lblAgentName);
            this.panel1.Controls.Add(this.lblAddress);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(784, 100);
            this.panel1.TabIndex = 0;
            // 
            // lblAgentName
            // 
            this.lblAgentName.Location = new System.Drawing.Point(20, 20);
            this.lblAgentName.Name = "lblAgentName";
            this.lblAgentName.Size = new System.Drawing.Size(100, 23);
            this.lblAgentName.TabIndex = 0;
            this.lblAgentName.Text = "Agent Name:";
            // 
            // lblAddress
            // 
            this.lblAddress.Location = new System.Drawing.Point(20, 50);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new System.Drawing.Size(100, 23);
            this.lblAddress.TabIndex = 1;
            this.lblAddress.Text = "Address:";
            // 
            // txtAgentName
            // 
            this.txtAgentName.Location = new System.Drawing.Point(100, 17);
            this.txtAgentName.Name = "txtAgentName";
            this.txtAgentName.Size = new System.Drawing.Size(250, 22);
            this.txtAgentName.TabIndex = 2;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(100, 47);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(250, 22);
            this.txtAddress.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(370, 45);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // AgentForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvAgents);
            this.Name = "AgentForm";
            this.Text = "Manage Agents";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAgents)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        private void LoadAgents()
        {
            dgvAgents.DataSource = DatabaseHelper.GetAllAgents();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAgentName.Text))
                {
                    MessageBox.Show("Please enter agent name", "Validation Error");
                    return;
                }

                var agent = new Agent
                {
                    AgentName = txtAgentName.Text,
                    Address = txtAddress.Text
                };

                DatabaseHelper.AddAgent(agent);
                MessageBox.Show("Agent saved successfully!");

                ClearForm();
                LoadAgents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving agent: {ex.Message}", "Error");
            }
        }

        private void ClearForm()
        {
            txtAgentName.Clear();
            txtAddress.Clear();
        }

        private DataGridView dgvAgents;
        private Panel panel1;
        private TextBox txtAgentName;
        private TextBox txtAddress;
        private Button btnSave;
        private Label lblAgentName;
        private Label lblAddress;
    }
}