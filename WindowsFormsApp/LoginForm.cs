using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp.DAL;

namespace WindowsFormsApp
{
    public class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.panel1 = new Panel();

            // Panel
            this.panel1.SuspendLayout();
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Padding = new Padding(10);

            // Label1 - Username
            this.label1.AutoSize = true;
            this.label1.Location = new Point(50, 50);
            this.label1.Size = new Size(58, 13);
            this.label1.Text = "Username:";

            // Label2 - Password
            this.label2.AutoSize = true;
            this.label2.Location = new Point(50, 80);
            this.label2.Size = new Size(56, 13);
            this.label2.Text = "Password:";

            // Username TextBox
            this.txtUsername.Location = new Point(120, 47);
            this.txtUsername.Size = new Size(200, 20);

            // Password TextBox
            this.txtPassword.Location = new Point(120, 77);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(200, 20);

            // Login Button
            this.btnLogin.Location = new Point(120, 110);
            this.btnLogin.Size = new Size(200, 30);
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new EventHandler(btnLogin_Click);

            // Add controls to panel
            this.panel1.Controls.AddRange(new Control[] {
                this.label1,
                this.label2,
                this.txtUsername,
                this.txtPassword,
                this.btnLogin
            });

            // LoginForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(384, 211);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Login";

            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Please enter both username and password", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (DatabaseHelper.ValidateUser(txtUsername.Text, txtPassword.Text))
                {
                    MainForm mainForm = new MainForm();
                    this.Hide();
                    mainForm.FormClosed += (s, args) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form controls
        private Panel panel1;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label label1;
        private Label label2;
    }
}
