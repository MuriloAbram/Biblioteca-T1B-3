using System; // Recursos básicos do C#

using System.Drawing; // Trabalha com cores e elementos gráficos

using System.Windows.Forms; // Recursos do Windows Forms

using T1B_3Library.Desktop.DTOs; // Importa os DTOs usados no Login

using T1B_3Library.Desktop.Helpers; // Importa os Helpers da aplicação

using T1B_3Library.Desktop.Services; // Importa os serviços da aplicação

using T1B_3Library.Desktop.Themes; // Importa o sistema de temas


namespace T1B_3Library.Desktop.Forms // Define o namespace dos formulários
{
    public partial class LoginForm : Form // Cria o formulário de Login
    {
        private readonly AuthApiService _authApiService; // Serviço responsável pela autenticação

        private bool _isRegisterMode = false; // Define se está no modo cadastro


        public LoginForm() // Construtor do formulário
        {
            InitializeComponent(); // Inicializa os componentes da tela

            _authApiService = new AuthApiService(new HttpClientHelper()); // Cria o serviço de autenticação

            ApplyTheme(); // Aplica o tema visual

            UpdateModeUI(); // Atualiza a interface inicial
        }


        private void ApplyTheme() // Aplica as configurações de aparência
        {
            pnlBrand.FillColor = LibraryTheme.SecondaryColor; // Define a cor do painel de marca

            pnlContent.FillColor = LibraryTheme.BackgroundColor; // Define a cor do painel principal

            LibraryTheme.ApplyPrimaryStyle(btnSubmit); // Aplica o estilo principal ao botão
        }


        private void UpdateModeUI() // Atualiza a tela conforme o modo atual
        {
            lblStatus.Text = string.Empty; // Limpa a mensagem de status

            if (_isRegisterMode) // Verifica se está no modo cadastro
            {
                lblTitle.Text = "Criar Nova Conta"; // Define o título do cadastro

                btnSubmit.Text = "Registar"; // Altera o texto do botão

                btnToggleMode.Text = "Já tem uma conta? Faça Login"; // Texto para voltar ao Login

                cmbRole.Visible = true; // Mostra o campo de perfil

                lblRole.Visible = true; // Mostra o texto do perfil

                btnSubmit.Location = new Point(40, 290); // Define a posição do botão

                btnToggleMode.Location = new Point(40, 345); // Define a posição do botão de alternância
            }
            else // Executa quando está no modo Login
            {
                lblTitle.Text = "Bem-vindo de volta!"; // Define o título do Login

                btnSubmit.Text = "Entrar"; // Define o texto do botão

                btnToggleMode.Text = "Não tem uma conta? Cadastre-se"; // Texto para abrir cadastro

                cmbRole.Visible = false; // Esconde o campo de perfil

                lblRole.Visible = false; // Esconde o texto do perfil

                btnSubmit.Location = new Point(40, 230); // Define a posição do botão

                btnToggleMode.Location = new Point(40, 285); // Define a posição do botão de alternância
            }
        }


        private void btnToggleMode_Click(object sender, EventArgs e) // Evento do botão de alternância
        {
            _isRegisterMode = !_isRegisterMode; // Inverte entre Login e cadastro

            UpdateModeUI(); // Atualiza a interface
        }


        private async void btnSubmit_Click(object sender, EventArgs e) // Evento do botão principal
        {
            string username = txtUsername.Text.Trim(); // Obtém o usuário digitado

            string password = txtPassword.Text.Trim(); // Obtém a senha digitada


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) // Verifica se os campos estão vazios
            {
                ShowMessage("Preencha todos os campos obrigatórios.", Color.Red); // Mostra mensagem de erro

                return; // Interrompe a execução
            }


            btnSubmit.Enabled = false; // Desabilita o botão durante a requisição


            try // Inicia o tratamento de possíveis erros
            {
                if (_isRegisterMode) // Verifica se o usuário está cadastrando
                {
                    string role = cmbRole.SelectedItem?.ToString() ?? "Reader"; // Obtém o perfil selecionado

                    var registerDto = new RegisterUserDto(username, password, role); // Cria o DTO de cadastro

                    var response = await _authApiService.RegisterAsync(registerDto); // Envia o cadastro para a API


                    if (response != null && response.Success) // Verifica se o cadastro foi realizado
                    {
                        ShowMessage("Conta criada com sucesso! Faça login.", Color.Green); // Mostra mensagem de sucesso

                        _isRegisterMode = false; // Volta para o modo Login

                        UpdateModeUI(); // Atualiza a interface
                    }
                    else // Executa quando o cadastro falha
                    {
                        ShowMessage(response?.Message ?? "Erro ao realizar cadastro.", Color.Red); // Mostra o erro
                    }
                }
                else // Executa quando está no modo Login
                {
                    var loginDto = new LoginRequestDto(username, password); // Cria o DTO de Login

                    var response = await _authApiService.LoginAsync(loginDto); // Envia o Login para a API


                    if (response != null && response.Success) // Verifica se o Login foi realizado
                    {
                        MainForm mainForm = new MainForm(); // Cria o formulário principal

                        mainForm.Show(); // Exibe o formulário principal

                        this.Hide(); // Esconde a tela de Login
                    }
                    else // Executa quando o Login falha
                    {
                        ShowMessage(response?.Message ?? "Credenciais inválidas.", Color.Red); // Mostra o erro
                    }
                }
            }
            catch (Exception ex) // Captura erros durante a execução
            {
                ShowMessage($"Erro de conexão: {ex.Message}", Color.Red); // Mostra o erro de conexão
            }
            finally // Executa sempre após o try/catch
            {
                btnSubmit.Enabled = true; // Habilita novamente o botão
            }
        }


        private void ShowMessage(string text, Color color) // Método para mostrar mensagens
        {
            lblStatus.ForeColor = color; // Define a cor da mensagem

            lblStatus.Text = text; // Define o texto da mensagem
        }
    }
}