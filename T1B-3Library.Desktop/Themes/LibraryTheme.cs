using System.Drawing; // Importa namespaces de desenho para manipulação de cores
using Guna.UI2.WinForms; // Importa os componentes gráficos do Guna2

namespace T1B_3Library.Desktop.Themes
{
    // Paleta de cores personalizada para a aplicação, definindo cores primárias, secundárias e de fundo
    public static class LibraryTheme
    {
        // Define a cor primária (Roxo principal) utilizada em botões e destaques
        public static readonly Color PrimaryColor = Color.FromArgb(94, 23, 235);

        // Define a cor secundária (Escura) para painéis laterais
        public static readonly Color SecondaryColor = Color.FromArgb(30, 30, 46);

        // Define a cor de fundo padrão para os formulários
        public static readonly Color BackgroundColor = Color.FromArgb(248, 249, 250);

        // Define a cor de texto padrão para botões primários
        public static readonly Color TextOnPrimary = Color.White;

        // Método utilitário para aplicar o estilo padrão aos botões do Guna2
        public static void ApplyPrimaryStyle(Guna2Button button)
        {
            button.FillColor = PrimaryColor; // Define a cor de fundo do botão
            button.ForeColor = TextOnPrimary; // Define a cor do texto do botão
            button.BorderRadius = 8; // Define o arredondamento das bordas
            button.Font = new Font("Segoe UI", 10F, FontStyle.Bold); // Aplica a fonte padrão
        }
    }
}