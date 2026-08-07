using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using T1B_3Library.Desktop.Helpers;
using T1B_3Library.Desktop.Themes;

namespace T1B_3Library.Desktop.Forms
{
    public partial class MainForm : Form
    {
        private Form? _activeForm = null;

        public MainForm()
        {
            InitializeComponent();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            pnlSidebar.FillColor = LibraryTheme.SecondaryColor;
            pnlHeader.FillColor = Color.White;
            pnlContent.FillColor = LibraryTheme.BackgroundColor;

            lblTitle.ForeColor = LibraryTheme.TextOnPrimary;
            lblUserInfo.ForeColor = Color.Gray;

            btnLogout.FillColor = Color.FromArgb(220, 53, 69);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (SessionManager.CurrentUser != null)
            {
                lblUserInfo.Text = $"Olá, {SessionManager.CurrentUser.Username} ({SessionManager.CurrentUser.Role})";
            }
        }

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
                    case "btnGames":
                        // Exemplo: OpenChildForm(new GameFormDialog(), "Gestão de Jogos");
                        break;

                    case "btnCategorias":
                        // Exemplo: OpenChildForm(new CategoriaForm(), "Gestão de Categorias");
                        break;

                    case "btnPerfil":
                        // Exemplo: OpenChildForm(new PerfilForm(), "Meu Perfil");
                        break;
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SessionManager.EndSession();
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}