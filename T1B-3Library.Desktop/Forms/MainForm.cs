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

        // Tornado async para poder inicializar serviços e carregar dados
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // Inicializa o serviço HTTP / API
            try
            {
                _BookService = new BooksApiService(HttpClientHelper.Instance);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao inicializar serviço de livros: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Exibe as informações do usuário logado no topo
            if (SessionManager.Instance.CurrentUser != null)
            {
                lblUserInfo.Text = $"👤 {SessionManager.Instance.CurrentUser.Username}  |  [{SessionManager.Instance.CurrentUser.Role}]";
            }
            else
            {
                lblUserInfo.Text = "👤 Usuário Conectado";
            }

            // Configura permissões e carrega os dados iniciais
            ConfigurarPermissões();
            await CarregarDadosAsync();
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
                if (_BookService == null)
                {
                    MessageBox.Show("Serviço de livros não inicializado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var created = await _BookService.CreateAsync(form.BookDto);

                    if (created != null)
                    {
                        MessageBox.Show("✅ Livro criado com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        await CarregarDadosAsync();
                    }
                    else
                    {
                        MessageBox.Show("❌ Falha ao criar livro.",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao criar livro: {ex.Message}",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            var u = ObterLivroSelecionado();
            if (u == null)
            {
                MessageBox.Show("Selecione um livro para excluir.", "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var conf = MessageBox.Show($"Deseja excluir o livro \"{u.Title}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (conf != DialogResult.Yes) return;

            if (_BookService == null)
            {
                MessageBox.Show("Serviço de livros não inicializado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Desestrutura a tupla retornada por DeleteAsync
                var (success, error) = await _BookService.DeleteAsync(u.Id);
                if (success)
                {
                    MessageBox.Show("✅ Livro excluído com sucesso!",
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
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao excluir livro: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private BookDto? ObterLivroSelecionado()
        {
            if (gridLivros.SelectedRows.Count == 0)
                return null;

            var row = gridLivros.SelectedRows[0];
            var idValue = row.Cells["colId"].Value;
            if (idValue == null)
                return null;

            // Se o valor já for um Guid, usa diretamente.
            if (idValue is Guid guidValue)
            {
                return _todosLivros.FirstOrDefault(u => u.Id == guidValue);
            }

            // Caso venha como string (mais comum), tenta converter.
            if (Guid.TryParse(idValue.ToString(), out var parsedGuid))
            {
                return _todosLivros.FirstOrDefault(u => u.Id == parsedGuid);
            }

            // Não foi possível interpretar o ID
            return null;
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var livro = ObterLivroSelecionado();
            if (livro == null)
            {
                MessageBox.Show("Selecione um Livro para editar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Abre o formulário de edição preenchendo os campos conhecidos.
            // Como BookFormDialog tem um construtor com campos individuais,
            // passamos valores mínimos quando não existirem (categoria/ano/qt).
            using var form = new BookFormDialog(livro.Title, livro.Author, string.Empty, 0, 0);
            if (form.ShowDialog() == DialogResult.OK && form.BookUpdateDto != null)
            {
                if (_BookService == null)
                {
                    MessageBox.Show("Serviço de livros não inicializado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    var success = await _BookService.UpdateAsync(livro.Id, form.BookUpdateDto);
                    if (success)
                    {
                        MessageBox.Show("✅ Livro atualizado com sucesso!",
                            "Sucesso",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        await CarregarDadosAsync();
                    }
                    else
                    {
                        MessageBox.Show("❌ Falha ao atualizar livro.",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao atualizar livro: {ex.Message}",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}