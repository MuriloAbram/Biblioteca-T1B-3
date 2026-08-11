// Importa funcionalidades básicas da linguagem C#
using System;
using System.Windows.Forms;
using T1B_3Library.Desktop.DTOs;

namespace T1B_3Library.Desktop.Forms
{
    public partial class BookFormDialog : Form
    {
        // ============================================================
        // PROPRIEDADES
        // ============================================================
        public string BookTitle { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public int Quantity { get; private set; }

        // ============================================================
        // CONTROLE DO MODO DO FORMULÁRIO E DTOS
        // ============================================================
        private readonly bool _isEditMode = false;

        /// <summary>
        /// DTO preenchido após clicar em "Salvar" com sucesso (modo criação).
        /// </summary>
        public CreateBookDto? BookDto { get; private set; }

        /// <summary>
        /// DTO preenchido após clicar em "Salvar" com sucesso (modo edição).
        /// </summary>
        public UpdateBookDto? BookUpdateDto { get; private set; }

        // ============================================================
        // CONSTRUTOR - NOVO CADASTRO
        // ============================================================
        public BookFormDialog()
        {
            InitializeComponent();
            _isEditMode = false;
        }

        // ============================================================
        // CONSTRUTOR - EDIÇÃO
        // ============================================================
        public BookFormDialog(
            string title,
            string author,
            string category,
            int year,
            int quantity
        ) : this()
        {
            _isEditMode = true;

            lblDialogTitle.Text = "Editar Livro";
            txtTitle.Text = title;
            txtAuthor.Text = author;
            cmbCategory.SelectedItem = category;
            txtYear.Text = year.ToString();
            txtQuantity.Text = quantity.ToString();
        }

        // ============================================================
        // CARREGAMENTO DO FORMULÁRIO E CATEGORIAS
        // ============================================================
        private void BookFormDialog_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            cmbCategory.Items.Clear();
            cmbCategory.Items.Add("Romance");
            cmbCategory.Items.Add("Ficção Científica");
            cmbCategory.Items.Add("Fantasia");
            cmbCategory.Items.Add("História");
            cmbCategory.Items.Add("Biografia");
            cmbCategory.Items.Add("Tecnologia");

            if (cmbCategory.Items.Count > 0 && cmbCategory.SelectedIndex == -1)
            {
                cmbCategory.SelectedIndex = 0;
            }
        }

        // ============================================================
        // BOTÃO SALVAR
        // ============================================================
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Valida os campos antes de prosseguir
            if (!ValidateInputs())
                return;

            BookTitle = txtTitle.Text.Trim();
            Author = txtAuthor.Text.Trim();
            Category = cmbCategory.SelectedItem?.ToString() ?? "";
            Year = int.Parse(txtYear.Text.Trim());
            Quantity = int.Parse(txtQuantity.Text.Trim());

            // 💡 CORREÇÃO PRINCIPAL: Instanciar os DTOs para o MainForm conseguir ler!
            if (_isEditMode)
            {
                BookUpdateDto = new UpdateBookDto
                {
                    Title = BookTitle,
                    Author = Author,
                    RealeaseYear = Year
                    // Adicione propriedades extras do seu UpdateBookDto aqui se houver
                };
            }
            else
            {
                BookDto = new CreateBookDto
                {
                    Title = BookTitle,
                    Author = Author,
                    RealeaseYear = Year
                    // Adicione propriedades extras do seu CreateBookDto aqui se houver
                };
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        // ============================================================
        // VALIDAÇÃO DOS CAMPOS
        // ============================================================
        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Por favor, informe o título do livro.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                MessageBox.Show("Por favor, informe o autor do livro.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAuthor.Focus();
                return false;
            }

            if (!int.TryParse(txtYear.Text.Trim(), out int year) || year <= 0)
            {
                MessageBox.Show("Por favor, informe um ano válido.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtYear.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                MessageBox.Show("Por favor, informe uma quantidade válida.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        // ============================================================
        // BOTÃO CANCELAR
        // ============================================================
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}