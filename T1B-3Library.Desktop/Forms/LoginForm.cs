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
            _authApiService = new AuthApiService(new HttpClientHelper());
            ApplyTheme();
            UpdateModeUI();
        }

        private void ApplyTheme()
        {
            pnlBrand.FillColor = LibraryTheme.SecondaryColor;
            pnlContent.FillColor = LibraryTheme.BackgroundColor;

            LibraryTheme.ApplyPrimaryStyle(btnSubmit);
        }

        private void UpdateModeUI()
        {
            lblStatus.Text = string.Empty;

            if (_isRegisterMode)
            {
                lblTitle.Text = "Criar Nova Conta";
                btnSubmit.Text = "Registar";
                btnToggleMode.Text = "Já tem uma conta? Faça Login";

                cmbRole.Visible = true;
                lblRole.Visible = true;

                btnSubmit.Location = new Point(40, 290);
                btnToggleMode.Location = new Point(40, 345);
            }
            else
            {
                lblTitle.Text = "Bem-vindo de volta!";
                btnSubmit.Text = "Entrar";
                btnToggleMode.Text = "Não tem uma conta? Cadastre-se";

                cmbRole.Visible = false;
                lblRole.Visible = false;

                btnSubmit.Location = new Point(40, 230);
                btnToggleMode.Location = new Point(40, 285);
            }
        }

        private void btnToggleMode_Click(object sender, EventArgs e)
        {
            _isRegisterMode = !_isRegisterMode;
            UpdateModeUI();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Preencha todos os campos obrigatórios.", Color.Red);
                return;
            }

            btnSubmit.Enabled = false;

            try
            {
                if (_isRegisterMode)
                {
                    string role = cmbRole.SelectedItem?.ToString() ?? "Reader";
                    var registerDto = new RegisterUserDto(username, password, role);
                    var response = await _authApiService.RegisterAsync(registerDto);

                    if (response != null && response.Success)
                    {
                        ShowMessage("Conta criada com sucesso! Faça login.", Color.Green);
                        _isRegisterMode = false;
                        UpdateModeUI();
                    }
                    else
                    {
                        ShowMessage(response?.Message ?? "Erro ao realizar cadastro.", Color.Red);
                    }
                }
                else
                {
                    var loginDto = new LoginRequestDto(username, password);
                    var response = await _authApiService.LoginAsync(loginDto);

                    if (response != null && response.Success)
                    {
                        MainForm mainForm = new MainForm();
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        ShowMessage(response?.Message ?? "Credenciais inválidas.", Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Erro de conexão: {ex.Message}", Color.Red);
            }
            finally
            {
                btnSubmit.Enabled = true;
            }
        }

        private void ShowMessage(string text, Color color)
        {
            lblStatus.ForeColor = color;
            lblStatus.Text = text;
        }
    }
}