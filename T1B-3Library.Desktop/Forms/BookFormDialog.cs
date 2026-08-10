using System;
using System.Windows.Forms;

namespace T1B_3Library.Desktop.Forms
{
    public partial class BookFormDialog : Form
    {
        // Propriedades públicas para resgatar os dados do livro após salvar
        public string BookTitle { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Isbn { get; private set; } = string.Empty;
        public string Category { get; private set; } = string.Empty;
        public int Year { get; private set; }
        public int Quantity { get; private set; }

        private readonly bool _isEditMode = false;

        // Construtor para NOVO CADASTRO
        public BookFormDialog()
        {
            InitializeComponent();
            _isEditMode = false;
        }

        // Construtor para EDIÇÃO (recebe dados do livro existente)
        public BookFormDialog(string title, string author, string isbn, string category, int year, int quantity) : this()
        {
            _isEditMode = true;
            lblDialogTitle.Text = "Editar Livro";

            txtTitle.Text = title;
            txtAuthor.Text = author;
            cmbCategory.SelectedItem = category;
            txtYear.Text = year.ToString();
            txtQuantity.Text = quantity.ToString();
        }

        private void BookFormDialog_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            // Adicione aqui as categorias desejadas ou busque do seu Banco/Service
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            BookTitle = txtTitle.Text.Trim();
            Author = txtAuthor.Text.Trim();
            Category = cmbCategory.SelectedItem?.ToString() ?? "";
            Year = int.Parse(txtYear.Text.Trim());
            Quantity = int.Parse(txtQuantity.Text.Trim());

            // Define o resultado da janela como OK e fecha a dialog
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
