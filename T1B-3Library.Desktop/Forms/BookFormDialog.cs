// Importa funcionalidades básicas da linguagem C#,
// como tipos, métodos, strings etc.
using System;

// Importa as classes necessárias para trabalhar com Windows Forms,
// como Form, MessageBox, DialogResult, TextBox, Button etc.
using System.Windows.Forms;
using T1B_3Library.Desktop.DTOs;

// Define o namespace onde essa classe está localizada.
// O namespace serve para organizar as classes do projeto.
namespace T1B_3Library.Desktop.Forms
{
    // Declara a classe BookFormDialog.
    // "partial" significa que a classe está dividida em mais de um arquivo.
    // Parte dela fica neste arquivo e outra parte é gerada pelo Designer do Windows Forms.
    // ": Form" significa que BookFormDialog herda da classe Form,
    // portanto ela representa uma janela do Windows Forms.
    public partial class BookFormDialog : Form
    {
        // ============================================================
        // PROPRIEDADES
        // ============================================================

        // Guarda o título do livro.
        // "public" permite que outras classes acessem essa propriedade.
        // "get" permite ler o valor.
        // "private set" permite alterar o valor somente dentro desta classe.
        // "= string.Empty" inicia a propriedade com uma string vazia.
        public string BookTitle { get; private set; } = string.Empty;

        // Guarda o nome do autor do livro.
        public string Author { get; private set; } = string.Empty;


        // Guarda a categoria do livro.
        public string Category { get; private set; } = string.Empty;

        // Guarda o ano de publicação do livro.
        public int Year { get; private set; }

        // Guarda a quantidade de exemplares do livro.
        public int Quantity { get; private set; }


        // ============================================================
        // CONTROLE DO MODO DO FORMULÁRIO
        // ============================================================

        // Variável privada que informa se o formulário está sendo usado
        // para editar um livro existente.
        //
        // "private" significa que só pode ser acessada dentro desta classe.
        //
        // "readonly" significa que, depois de inicializada, a variável
        // só poderá ser alterada dentro do construtor.
        //
        // O valor inicial é false, indicando que, por padrão,
        // o formulário começa no modo de novo cadastro.
        private readonly bool _isEditMode = false;

        /// <summary>
        /// DTO preenchido após clicar em "Salvar" com sucesso (modo criação).
        /// Fica null se o usuário cancelar o formulário ou se estiver em modo edição.
        /// </summary>
        public CreateBookDto? BookDto { get; private set; }

        /// <summary>
        /// DTO preenchido após clicar em "Salvar" com sucesso (modo edição).
        /// Fica null se o usuário cancelar o formulário ou se estiver em modo criação.
        /// </summary>
        public UpdateBookDto? BookUpdateDto  { get; private set; }

        /// <summary>
        /// Id do usuário sendo editado. Só é preenchido em modo edição.
        /// </summary>
        public string? UsuarioId { get; private set; }

        private readonly bool _modoEdicao;

        // ============================================================
        // CONSTRUTOR - NOVO CADASTRO
        // ============================================================

        // Construtor padrão da classe.
        //
        // Ele é executado quando fazemos:
        //
        // new BookFormDialog();
        //
        // É utilizado quando queremos cadastrar um livro novo.
        public BookFormDialog()
        {
            // Inicializa todos os componentes visuais criados
            // pelo Windows Forms Designer.
            //
            // Por exemplo:
            // TextBox, Button, Label, ComboBox etc.
            InitializeComponent();

            // Define que o formulário está no modo de novo cadastro.
            //
            // false = novo livro
            // true  = edição de livro existente
            _isEditMode = false;
        }


        // ============================================================
        // CONSTRUTOR - EDIÇÃO
        // ============================================================

        // Segundo construtor da classe.
        //
        // Ele recebe os dados de um livro que já existe.
        //
        // Dessa forma, quando abrirmos o formulário para editar,
        // os campos já serão preenchidos com os dados existentes.
        public BookFormDialog(
            string title,
            string author,
            string category,
            int year,
            int quantity
        ) : this()
        {
            // ": this()" chama o construtor padrão:
            //
            // public BookFormDialog()
            //
            // Isso faz com que o InitializeComponent()
            // seja executado antes do restante deste construtor.
            _isEditMode = true;

            // Altera o texto do título da janela/formulário
            // para informar ao usuário que ele está editando um livro.
            lblDialogTitle.Text = "Editar Livro";

            // Coloca o título recebido no campo de texto.
            txtTitle.Text = title;

            // Coloca o autor recebido no campo de texto.
            txtAuthor.Text = author;

            cmbCategory.SelectedItem = category;

            // Converte o número do ano para texto
            // e coloca no campo correspondente.
            txtYear.Text = year.ToString();

            // Converte a quantidade para texto
            // e coloca no campo correspondente.
            txtQuantity.Text = quantity.ToString();
        }


        // ============================================================
        // EVENTO DE CARREGAMENTO DO FORMULÁRIO
        // ============================================================

        // Este método é executado quando o formulário é carregado.
        //
        // "object sender" representa o objeto que disparou o evento.
        //
        // "EventArgs e" contém informações relacionadas ao evento.
        //
        // "private" significa que esse método é utilizado apenas
        // dentro da própria classe.
        private void BookFormDialog_Load(object sender, EventArgs e)
        {
            // Chama o método responsável por carregar
            // as categorias no ComboBox.
            LoadCategories();
        }


        // ============================================================
        // CARREGAMENTO DAS CATEGORIAS
        // ============================================================

        // Método responsável por preencher o ComboBox
        // com as categorias disponíveis.
        private void LoadCategories()
        {
            // Este comentário indica que as categorias poderiam
            // futuramente vir de um banco de dados ou serviço.
            // Atualmente elas estão sendo adicionadas manualmente.

            // Remove todas as categorias que já estejam no ComboBox.
            //
            // Isso evita que as categorias sejam duplicadas
            // caso o método seja executado mais de uma vez.
            cmbCategory.Items.Clear();

            // Adiciona a categoria "Romance" ao ComboBox.
            cmbCategory.Items.Add("Romance");

            // Adiciona a categoria "Ficção Científica".
            cmbCategory.Items.Add("Ficção Científica");

            // Adiciona a categoria "Fantasia".
            cmbCategory.Items.Add("Fantasia");

            // Adiciona a categoria "História".
            cmbCategory.Items.Add("História");

            // Adiciona a categoria "Biografia".
            cmbCategory.Items.Add("Biografia");

            // Adiciona a categoria "Tecnologia".
            cmbCategory.Items.Add("Tecnologia");


            // Verifica duas condições:
            //
            // 1. Se existe pelo menos uma categoria.
            //
            // 2. Se nenhuma categoria está selecionada.
            //
            // "&&" significa E.
            if (cmbCategory.Items.Count > 0 && cmbCategory.SelectedIndex == -1)
            {
                // Seleciona automaticamente a primeira categoria.
                //
                // O índice começa em 0:
                //
                // 0 = Romance
                // 1 = Ficção Científica
                // 2 = Fantasia
                // etc.
                cmbCategory.SelectedIndex = 0;
            }
        }


        // ============================================================
        // BOTÃO SALVAR
        // ============================================================

        // Método executado quando o usuário clica no botão Salvar.
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Executa a validação de todos os campos.
            //
            // Se ValidateInputs() retornar false,
            // o método retorna imediatamente e não continua.
            if (!ValidateInputs())
                return;


            // Pega o texto digitado no campo de título.
            //
            // ".Trim()" remove espaços desnecessários
            // no começo e no final do texto.
            //
            // Exemplo:
            //
            // "  Harry Potter  "
            //
            // vira:
            //
            // "Harry Potter"
            BookTitle = txtTitle.Text.Trim();

            // Obtém o nome do autor e remove espaços extras.
            Author = txtAuthor.Text.Trim();

            // Obtém a categoria selecionada no ComboBox.
            //
            // "SelectedItem" representa o item selecionado.
            //
            // "?.ToString()" significa:
            // se existir um item selecionado, converta para string.
            //
            // Caso não exista, não ocorrerá erro.
            //
            // "?? """ significa:
            // se o resultado for null, utilize uma string vazia.
            Category = cmbCategory.SelectedItem?.ToString() ?? "";


            // Pega o texto do campo de ano,
            // remove espaços e converte para inteiro.
            //
            // Exemplo:
            //
            // "2026" -> 2026
            Year = int.Parse(txtYear.Text.Trim());


            // Pega o texto da quantidade,
            // remove espaços e converte para inteiro.
            //
            // Exemplo:
            //
            // "10" -> 10
            Quantity = int.Parse(txtQuantity.Text.Trim());


            // Define o resultado da janela como OK.
            //
            // Isso informa para o formulário que a operação
            // foi concluída com sucesso.
            //
            // Normalmente o formulário que abriu este Dialog
            // poderá verificar:
            //
            // if (dialog.ShowDialog() == DialogResult.OK)
            this.DialogResult = DialogResult.OK;

            // Fecha a janela atual.
            this.Close();
        }


        // ============================================================
        // VALIDAÇÃO DOS CAMPOS
        // ============================================================

        // Método responsável por verificar se os dados digitados
        // pelo usuário são válidos.
        //
        // Retorna:
        //
        // true  = dados válidos
        // false = dados inválidos
        private bool ValidateInputs()
        {
            // Verifica se o campo do título está vazio
            // ou contém apenas espaços.
            //
            // string.IsNullOrWhiteSpace() retorna true quando:
            //
            // null
            // ""
            // "   "
            //
            // são encontrados.
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                // Exibe uma mensagem informando ao usuário
                // que o título precisa ser preenchido.
                MessageBox.Show(
                    "Por favor, informe o título do livro.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Coloca o cursor novamente no campo de título.
                txtTitle.Focus();

                // Informa que a validação falhou.
                return false;
            }


            // Verifica se o campo do autor está vazio
            // ou contém apenas espaços.
            if (string.IsNullOrWhiteSpace(txtAuthor.Text))
            {
                // Exibe uma mensagem informando que o autor
                // precisa ser preenchido.
                MessageBox.Show(
                    "Por favor, informe o autor do livro.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Coloca o cursor no campo do autor.
                txtAuthor.Focus();

                // Interrompe a validação e informa que os dados
                // são inválidos.
                return false;
            }


            // Tenta converter o texto do campo Year para um número inteiro.
            //
            // "TryParse" é utilizado para tentar fazer a conversão
            // sem causar uma exceção caso o usuário digite algo inválido.
            //
            // "out int year" cria uma variável chamada "year"
            // que receberá o número convertido.
            //
            // A segunda condição:
            //
            // year <= 0
            //
            // impede que sejam aceitos valores 0 ou negativos.
            if (!int.TryParse(txtYear.Text.Trim(), out int year) || year <= 0)
            {
                // Exibe uma mensagem informando que o ano é inválido.
                MessageBox.Show(
                    "Por favor, informe um ano válido.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Coloca o cursor no campo do ano.
                txtYear.Focus();

                // Informa que a validação falhou.
                return false;
            }


            // Tenta converter o texto da quantidade para inteiro.
            //
            // "out int qty" cria uma variável chamada qty
            // para armazenar o valor convertido.
            //
            // "qty < 0" impede que a quantidade seja negativa.
            //
            // Portanto:
            //
            // 0 = permitido
            // 1 = permitido
            // 10 = permitido
            // -1 = não permitido
            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty < 0)
            {
                // Exibe uma mensagem informando que a quantidade
                // digitada é inválida.
                MessageBox.Show(
                    "Por favor, informe uma quantidade válida.",
                    "Atenção",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                // Coloca o cursor no campo da quantidade.
                txtQuantity.Focus();

                // Informa que a validação falhou.
                return false;
            }


            // Se o código chegou até aqui,
            // significa que todos os campos passaram pelas validações.
            //
            // Portanto, retorna true.
            return true;
        }


        // ============================================================
        // BOTÃO CANCELAR
        // ============================================================

        // Método executado quando o usuário clica no botão Cancelar.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Define que o resultado da janela foi Cancelar.
            //
            // O formulário que abriu este Dialog poderá verificar
            // esse resultado usando DialogResult.Cancel.
            this.DialogResult = DialogResult.Cancel;

            // Fecha a janela atual.
            this.Close();
        }
    }
}