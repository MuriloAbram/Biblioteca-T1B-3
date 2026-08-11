
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
        private readonly AuthApiService _authApiService;

        private bool _isRegisterMode = false;

        public LoginForm()
        {
            InitializeComponent();

            // Usa a instância Singleton do HttpClientHelper
            _authApiService =
                new AuthApiService(HttpClientHelper.Instance);

            ApplyTheme();
            UpdateModeUI();
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
            string username =
                txtUsername.Text.Trim();

            string password =
                txtPassword.Text.Trim();


            // ------------------------------------------------------------
            // VALIDAÇÃO
            // ------------------------------------------------------------

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                ShowMessage(
                    "Preencha todos os campos obrigatórios.",
                    Color.Red
                );

                return;
            }


            // Desabilita o botão durante a requisição
            btnSubmit.Enabled = false;

            try
            {
                // ========================================================
                // CADASTRO
                // ========================================================

                if (_isRegisterMode)
                {
                    string role =
                        cmbRole.SelectedItem?.ToString()
                        ?? "Reader";


                    var registerDto =
                        new RegisterRequestDto
                        {
                            Email = username,
                            Password = password,
                            ConfirmPassword = password
                        };


                    var response =
                        await _authApiService.RegisterAsync(
                            registerDto
                        );


                    if (response != null &&
                        response.Success)
                    {
                        ShowMessage(
                            "Conta criada com sucesso! Faça login.",
                            Color.Green
                        );

                        _isRegisterMode = false;

                        UpdateModeUI();
                    }
                    else
                    {
                        ShowMessage(
                            response?.Message
                            ?? "Erro ao realizar cadastro.",
                            Color.Red
                        );
                    }
                }

                // ========================================================
                // LOGIN
                // ========================================================

                else
                {
                    var loginDto =
                        new LoginRequestDto
                        {
                            Email = username,
                            Password = password
                        };


                    var response =
                        await _authApiService.LoginAsync(
                            username,
                            password
                        );


                    if (response.Sucesso &&
                        response.User != null &&
                        !string.IsNullOrWhiteSpace(
                            response.User.Token))
                    {
                        // A sessão já foi criada pelo
                        // AuthApiService.LoginAsync()

                        MainForm mainForm =
                            new MainForm();

                        mainForm.FormClosed +=
                            MainForm_FormClosed;

                        mainForm.Show();

                        Hide();
                    }
                    else
                    {
                        ShowMessage(
                            response.ErrorMessage
                            ?? "Credenciais inválidas.",
                            Color.Red
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(
                    $"Erro de conexão:\n{ex.Message}",
                    Color.Red
                );
            }
            finally
            {
                btnSubmit.Enabled = true;
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
            Application.Exit();
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
    }
}
