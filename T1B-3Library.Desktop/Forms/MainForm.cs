using System; // Recursos básicos do C#

using System.Drawing; // Trabalha com elementos gráficos e posições

using System.Windows.Forms; // Recursos do Windows Forms

using Guna.UI2.WinForms; // Componentes visuais da biblioteca Guna

using T1B_3Library.Desktop.Helpers; // Importa os Helpers da aplicação


namespace T1B_3Library.Desktop.Forms // Define o namespace dos formulários
{
    public partial class MainForm : Form // Cria o formulário principal
    {
        private Form? _activeForm = null; // Guarda o formulário secundário atualmente aberto


        public MainForm() // Construtor do formulário principal
        {
            InitializeComponent(); // Inicializa os componentes da tela
        }


        private void MainForm_Load(object sender, EventArgs e) // Executa quando o formulário é carregado
        {
            if (SessionManager.CurrentUser != null) // Verifica se existe um usuário conectado
            {
                lblUserInfo.Text = $"👤 {SessionManager.CurrentUser.Username}  |  [{SessionManager.CurrentUser.Role}]"; // Mostra usuário e perfil
            }
            else // Executa caso não exista usuário conectado
            {
                lblUserInfo.Text = "👤 Usuário Conectado"; // Mostra uma mensagem padrão
            }
        }


        // Abre um Form secundário dentro do painel principal (pnlContent)
        private void OpenChildForm(Form childForm, string title) // Abre um formulário dentro do painel
        {
            if (_activeForm != null) // Verifica se já existe um formulário aberto
            {
                _activeForm.Close(); // Fecha o formulário anterior
            }

            _activeForm = childForm; // Define o novo formulário como ativo

            lblTitle.Text = title; // Altera o título da tela


            childForm.TopLevel = false; // Define o formulário como secundário

            childForm.FormBorderStyle = FormBorderStyle.None; // Remove a borda do formulário

            childForm.Dock = DockStyle.Fill; // Faz o formulário ocupar todo o painel


            pnlContent.Controls.Add(childForm); // Adiciona o formulário ao painel

            pnlContent.Tag = childForm; // Armazena o formulário na propriedade Tag

            childForm.BringToFront(); // Coloca o formulário na frente dos outros controles

            childForm.Show(); // Exibe o formulário
        }


        private void btnNav_Click(object sender, EventArgs e) // Evento dos botões de navegação
        {
            if (sender is Guna2Button btn) // Verifica se quem chamou o evento é um botão Guna
            {
                switch (btn.Name) // Identifica qual botão foi clicado
                {
                    case "btnDashboard": // Executa quando o Dashboard é selecionado
                        lblTitle.Text = "Dashboard"; // Define o título como Dashboard

                        if (_activeForm != null) // Verifica se existe um formulário aberto
                        {
                            _activeForm.Close(); // Fecha o formulário atual

                            _activeForm = null; // Remove a referência do formulário ativo
                        }
                        break; // Encerra este caso


                    case "btnLivros": // Executa quando o botão Livros é selecionado
                        // Exemplo: Substitua pelo nome real do seu Form de livros
                        // OpenChildForm(new LivrosForm(), "Gerenciamento de Livros");
                        break; // Encerra este caso


                    case "btnCategorias": // Executa quando o botão Categorias é selecionado
                        // Exemplo: Substitua pelo nome real do seu Form de categorias
                        // OpenChildForm(new CategoriasForm(), "Gerenciamento de Categorias");
                        break; // Encerra este caso
                }
            }
        }


        private void btnLogout_Click(object sender, EventArgs e) // Evento do botão de sair
        {
            DialogResult result = MessageBox.Show( // Exibe uma confirmação para o usuário
                "Deseja realmente sair do sistema?", // Mensagem exibida
                "Sair", // Título da janela
                MessageBoxButtons.YesNo, // Mostra os botões Sim e Não
                MessageBoxIcon.Question // Define o ícone de pergunta
            );


            if (result == DialogResult.Yes) // Verifica se o usuário confirmou a saída
            {
                SessionManager.EndSession(); // Encerra a sessão do usuário

                LoginForm loginForm = new LoginForm(); // Cria uma nova tela de Login

                loginForm.Show(); // Exibe a tela de Login

                this.Close(); // Fecha o formulário principal
            }
        }
    }
}