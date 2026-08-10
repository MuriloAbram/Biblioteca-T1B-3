using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using T1B_3Library.Desktop.Helpers;

namespace T1B_3Library.Desktop.Forms
{
    public partial class MainForm : Form
    {
        private Form? _activeForm = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (SessionManager.CurrentUser != null)
            {
                lblUserInfo.Text = $"👤 {SessionManager.CurrentUser.Username}  |  [{SessionManager.CurrentUser.Role}]";
            }
            else
            {
                lblUserInfo.Text = "👤 Usuário Conectado";
            }
        }

        // Abre um Form secundário dentro do painel principal (pnlContent)
        private void OpenChildForm(Form childForm, string title)
        {
            if (_activeForm != null)
            {
                _activeForm.Close();
            }

            _activeForm = childForm;
            lblTitle.Text = title;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnNav_Click(object sender, EventArgs e)
        {
            if (sender is Guna2Button btn)
            {
                switch (btn.Name)
                {
                    case "btnDashboard":
                        lblTitle.Text = "Dashboard";
                        if (_activeForm != null)
                        {
                            _activeForm.Close();
                            _activeForm = null;
                        }
                        break;

                    case "btnLivros":
                        // Exemplo: Substitua pelo nome real do seu Form de livros
                        // OpenChildForm(new LivrosForm(), "Gerenciamento de Livros");
                        break;

                    case "btnCategorias":
                        // Exemplo: Substitua pelo nome real do seu Form de categorias
                        // OpenChildForm(new CategoriasForm(), "Gerenciamento de Categorias");
                        break;
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Sair",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                SessionManager.EndSession();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}