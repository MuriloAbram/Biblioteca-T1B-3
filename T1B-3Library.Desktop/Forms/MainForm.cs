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
using T1B_3Library.Application.DTOs;

namespace T1B_3Library.Desktop.Forms
{
    public partial class MainForm : Form
    {
        //=======================================
        // CAMPOS PRIVADOS
        //=======================================

        /// <summary>
        /// UserControl atualmente exibido no painel de conteudo (pnlConteudo)
        /// </summary>
        private UserControl? _controleAtual;

        /// <summary>
        /// Botão da sidebar atualmente ativo.
        /// </summary>
        private Guna2Button? _botaoAtivo;

        /// <summary>
        /// Serviço de autenticação para logout.
        /// </summary>
        private AuthApiService _authService = null;


        /// =====================================
        /// SERVIÇOS (Inicializados no load) 
        /// =====================================
        private BooksApiService? _BookService = null;

        /// =====================================
        /// Dados 
        /// =====================================
        private List<BookResponseDto> _todosLivros = new();

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
                _BookService = new BooksApiService();
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
                lblUserInfo.Text = $"👤 {SessionManager.Instance.CurrentUser.Email}  |  [{SessionManager.Instance.CurrentUser.Roles}]";
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
            gridLivros.Rows.Clear();

            try
            {
                var tarefaGames = _BookService.GetAllAsync();

                await Task.WhenAll(tarefaGames);

                _todosLivros = tarefaGames.Result;


                PopularGrid(_todosLivros);
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Erro ao carregar livros: {ex.Message}",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            }
        }

        private void PopularGrid(List<BookResponseDto> livros)
        {
            gridLivros.Rows.Clear();

            foreach (var l in livros)
            {
                gridLivros.Rows.Add(
                    l.Id,
                    l.Title,
                    l.RealeaseYear,
                    l.IsFeatured,
                    l.CreatedAt.ToString("dd/MM/yyyy HH:mm"));

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
                gridLivros.Rows.Add(livro.Id, livro.Title, livro.Author);
            }

        }



        private async void btnLogout_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show(
                "Deseja realmente sair do sistema?",
                "Confirmar Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (resposta != DialogResult.Yes) return;

            try
            {
                await _authService.LogoutAsync();
            }
            catch
            {
                // Mesmo se a API falhar, limpa a sessão local
            }
            finally
            {
                SessionManager.Instance.Clear();
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

                    if (created.Success)
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
            var g = ObterLivroSelecionado();
            if (g == null)
            {
                MessageBox.Show("Selecione um livro para excluir.", "Aviso",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }


            var conf = MessageBox.Show($"Deseja excluir o game \"{g.Title}\"?",
                "Confirmar Exclusão",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (conf != DialogResult.Yes) return;

            var (sucess, error) = await _BookService.DeleteAsync(g.Id);
            if (sucess)
            {
                MessageBox.Show(
                   "Game Excluído com sucesso!",
                   "Sucesso",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Information);
                await CarregarDadosAsync();
            }
            else
            {
                MessageBox.Show(
                   $"{error}", "Erro",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
            }
        }

        private BookResponseDto? ObterLivroSelecionado()
        {
            if (gridLivros.SelectedRows.Count == 0) return null;
            var row = gridLivros.SelectedRows[0];
            var cellValue = row.Cells["colId"].Value;
            if (cellValue == null) return null;

            // Se o valor já for um int, usa diretamente
            if (cellValue is int id)
            {
                return _todosLivros.FirstOrDefault(b => b.Id == id);
            }

            // Se for string ou outro, tenta converter para int
            if (int.TryParse(cellValue.ToString(), out var parsedId))
            {
                return _todosLivros.FirstOrDefault(b => b.Id == parsedId);
            }

            return null;
        }

        private async void btnAtualizar_Click(object sender, EventArgs e) => await CarregarDadosAsync();

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            var livro = ObterLivroSelecionado();
            if (livro == null)
            {
                MessageBox.Show("Selecione um livro para editar.",
                    "Aviso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (_BookService == null)
            {
                MessageBox.Show("Serviço de livros não inicializado.",
                    "Erro",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Usa o construtor existente do BookFormDialog (title, author, category, year, quantity)
            using var form = new BookFormDialog(livro.Title, livro.Author, string.Empty, livro.RealeaseYear, 0);
            if (form.ShowDialog() == DialogResult.OK && form.BookUpdateDto != null)
            {
                var (success, _, error) = await _BookService.UpdateAsync(livro.Id, form.BookUpdateDto);
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
                    MessageBox.Show($"❌ {error}",
                        "Erro",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void txtPesquisa_TextChanged_1(object sender, EventArgs e) => FiltrarLivros();

        private void FiltrarLivros()
        {
            var termo = txtPesquisa.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(termo))
            {
                PopularGrid(_todosLivros);
                return;
            }

            var filtrados = _todosLivros
                .Where(g => g.Title.Contains(termo, StringComparison.OrdinalIgnoreCase))
                .ToList();

            PopularGrid(filtrados);

        }

        private void txtPesquisa_KeyUp(object sender, KeyEventArgs e) => FiltrarLivros();
    }
}