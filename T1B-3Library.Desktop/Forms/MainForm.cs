using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // <-- Adicionado para usar o .ToList()
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using T1B_3Library.Desktop.Helpers;
using T1B_3Library.Desktop.DTOs;
using T1B_3Library.Desktop.Services;

namespace T1B_3Library.Desktop.Forms
{
    public partial class MainForm : Form
    {
        /// =====================================
        /// SERVIÇOS (Inicializados no load) 
        /// =====================================
        private BooksApiService? _BookService = null;

        /// =====================================
        /// Dados 
        /// =====================================
        private List<BookDto> _todosLivros = new();

        // Guarda o formulário secundário atualmente aberto no painel
        private Form? _activeForm = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Exibe as informações do usuário logado no topo
            if (SessionManager.Instance.CurrentUser != null)
            {
                lblUserInfo.Text = $"👤 {SessionManager.Instance.CurrentUser.Username}  |  [{SessionManager.Instance.CurrentUser.Role}]";
            }
            else
            {
                lblUserInfo.Text = "👤 Usuário Conectado";
            }
        }

        private async Task CarregarDadosAsync()
        {
            try
            {
                // Certifique-se de que o serviço foi instanciado antes de usar
                if (_BookService == null) return;

                var livros = await _BookService.GetAllAsync();
                gridLivros.Rows.Clear();

                if (livros != null)
                {
                    // Converte IEnumerable<BookDto> para List<BookDto> usando .ToList()
                    _todosLivros = livros.ToList();

                    foreach (var livro in _todosLivros)
                    {
                        // Adiciona as colunas na mesma ordem em que foram criadas no DataGridView
                        gridLivros.Rows.Add(livro.Id, livro.Title, livro.Author, livro.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar livros: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void ConfigurarPermissões()
        {
            //Verifica se o usuário logado é administrador
            bool isAdmin = SessionManager.Instance.IsAdmin;
            //Se não for admin, desabilita os botões de gerenciamento
            btnNovo.Enabled = isAdmin;

            btnExcluir.Enabled = isAdmin;
        }

        private void txtPesquisa_TextChanged(object sender, EventArgs e) => FiltrarLivros(txtPesquisa.Text);

        private void FiltrarLivros(string filtro)
        {
            var livrosFiltrados = _todosLivros
                .Where(l => l.Title.Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                            l.Author.Contains(filtro, StringComparison.OrdinalIgnoreCase))
                .ToList();
            gridLivros.Rows.Clear();
            foreach (var livro in livrosFiltrados)
            {
                gridLivros.Rows.Add(livro.Id, livro.Title, livro.Author, livro.Status);
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
                // Encerra a sessão e retorna à tela de Login
                SessionManager.EndSession();

                LoginForm loginForm = new LoginForm();
                loginForm.Show();

                this.Close();
            }
        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            using var form = new BookFormDialog();
            if (form.ShowDialog() == DialogResult.OK && form.BookDto != null)
            {
                var (success, _, error) = await _BookService.CreateAsync(form.BookDto);
                if (success)
                {
                    MessageBox.Show("✅ Usuário criado com sucesso!",
                        "Sucesso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    await CarregarDadosAsync();
                }
                else
                {
                    MessageBox.Show($"❌ {error}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                }
            }
        }
    }
}