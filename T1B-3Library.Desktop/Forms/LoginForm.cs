
using System;
using System.Drawing;
using System.Windows.Forms;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Helpers;
using T1B_3Library.Desktop.Services;
using T1B_3Library.Desktop.Themes;

namespace T1B_3Library.Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private AuthApiService _authService = null!;

        private bool _isRegisterMode = false;

        public LoginForm()
        {
            InitializeComponent();

        }


        // ================================================================
        // TEMA
        // ================================================================

        private void ApplyTheme()
        {
            pnlBrand.FillColor =
                LibraryTheme.SecondaryColor;

            pnlContent.FillColor =
                LibraryTheme.BackgroundColor;

            LibraryTheme.ApplyPrimaryStyle(btnSubmit);
        }


        // ================================================================
        // ATUALIZAR INTERFACE
        // ================================================================

        private void UpdateModeUI()
        {
            lblStatus.Text = string.Empty;

            if (_isRegisterMode)
            {
                lblTitle.Text = "Criar Nova Conta";

                btnSubmit.Text = "Registrar";

                btnToggleMode.Text =
                    "Já tem uma conta? Faça Login";

                cmbRole.Visible = true;
                lblRole.Visible = true;

                btnSubmit.Location =
                    new Point(40, 290);

                btnToggleMode.Location =
                    new Point(40, 345);
            }
            else
            {
                lblTitle.Text =
                    "Bem-vindo de volta!";

                btnSubmit.Text = "Entrar";

                btnToggleMode.Text =
                    "Não tem uma conta? Cadastre-se";

                cmbRole.Visible = false;
                lblRole.Visible = false;

                btnSubmit.Location =
                    new Point(40, 230);

                btnToggleMode.Location =
                    new Point(40, 285);
            }
        }


        // ================================================================
        // ALTERNAR LOGIN / CADASTRO
        // ================================================================

        private void btnToggleMode_Click(
            object sender,
            EventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;

            UpdateModeUI();
        }


        // ================================================================
        // LOGIN / CADASTRO
        // ================================================================

        private async void btnSubmit_Click(
            object sender,
            EventArgs e)
        {
            //Limpa erros anteriores
            lblStatus.Text = string.Empty;

            //Validação dos campos
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                lblStatus.Text = "⚠️ Informe seu e-mail!";
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                lblStatus.Text = "⚠️ Informe sua senha!";
                txtPassword.Focus();
                return;
            }
            // ===================== Estado de carregamento ======================
            SetCarregando(true);

            try
            {
                // Chamada da API
                var (success, user, errorMessage) = await _authService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text);

                if (success && user != null)
                {
                    // Armazena os dados do usuário na sessão (Singleton)
                    SessionManager.Instance.SetUser(user);

                    // Esconde a tela de login
                    this.Hide();

                    //Abrir a tela principal da aplicação
                    using var mainform = new MainForm();
                    mainform.ShowDialog();

                    // quando o MainForm fechar. fecha o LoginForm também
                    this.Close();
                }
                else
                {
                    lblStatus.Text = $"❌ {errorMessage}";
                    MessageBox.Show($"❌ {errorMessage}");
                }

            }
            catch (HttpRequestException exHttp)
            {
                lblStatus.Text = $"❌ Não foi possível conectar à API. \nVerifique se a API está em execução erro do sistema: {exHttp.Message}";
                MessageBox.Show($"❌ Não foi possível conectar à API. \nVerifique se a API está em execução erro do sistema: {exHttp.Message}");
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"❌ Erro inesperado: {ex.Message}";
                MessageBox.Show($"❌ Erro inesperado: {ex.Message}");
            }
            finally
            {
                SetCarregando(false);
            }

        }

        private void ExibirErro(string mensagem)
        {
            if (string.IsNullOrEmpty(mensagem))
            {
                lblStatus.Visible = false;
                lblStatus.Text = string.Empty;
            }
            else
            {
                lblStatus.Text = mensagem;
                lblStatus.Visible = true;
            }
        }

        // ================================================================
        // FECHAMENTO DA MAINFORM
        // ================================================================

        private void MainForm_FormClosed(
            object? sender,
            FormClosedEventArgs e)
        {
            // Encerra a aplicação quando a MainForm for fechada
            System.Windows.Forms.Application.Exit();
        }


        // ================================================================
        // MENSAGEM
        // ================================================================

        private void ShowMessage(
            string text,
            Color color)
        {
            lblStatus.ForeColor = color;

            lblStatus.Text = text;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            //Guard: não executa em tempo de design
            if (DesignMode) return;

            _authService = new AuthApiService();

            lblStatus.Text = $"API: {AppConfig.ApiBaseUrl}";

            txtUsername.Text = "admin@t1b3library.com";
            txtPassword.Text = "Admin@123";
        }

        private void SetCarregando(bool carregando)
        {
            btnSubmit.Enabled = !carregando;
            txtUsername.Enabled = !carregando;
            txtPassword.Enabled = !carregando;

            if (carregando)
            {
                btnSubmit.Text = "Aguarde...";
                lblStatus.Visible = false;
            }
            else
            {
                btnSubmit.Text = "Entrar";
            }

        }


    }
}
