using System; // Importa tipos básicos
using System.Windows.Forms; // Importa suporte para a aplicação Windows Forms
using T1B_3Library.Desktop.Forms; // Importa as telas da aplicação

namespace T1B_3Library.Desktop
{
    // Classe principal de entrada do executável da aplicação
    internal static class Program
    {
        // Ponto de entrada principal para a aplicação C#
        [STAThread] // Indica que o modelo de threading para o aplicativo é de thread única
        static void Main()
        {
            // Ativa os estilos visuais modernos para a aplicação
            Application.EnableVisualStyles();

            // Configura o valor padrão de renderização de texto compatível com controles do sistema
            Application.SetCompatibleTextRenderingDefault(false);

            // Inicia a execução da aplicação abrindo primeiro a tela de Login
            Application.Run(new LoginForm());
        }
    }
}