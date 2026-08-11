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
            // evita ambiguidade com outros namespaces chamados "Application"
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Executa o formulário de login
            System.Windows.Forms.Application.Run(new LoginForm());
        }
    }
}