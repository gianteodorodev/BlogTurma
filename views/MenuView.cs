public class MenuView
{
    /// <summary>
    /// Exibe o menu principal contendo o fluxo completo do sistema com opções numeradas de 0 a 9.
    /// </summary>
    /// <param name="estaAutenticado">Indica se há uma sessão de usuáio ativa no momento.</param>
    /// <returns>Número referente á opção escolhida ou -1 em caso de entrada inválida.</returns>
    public int ExibirMenu(bool estaAutenticado)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("        Bem vindo ao nosso Blog!        ");
        Console.WriteLine("========================================");

        string status = estaAutenticado ? "Autenticado" : "Visitante (Não autenticado)";
       Console.WriteLine($"Status da sessão: {status}");
        Console.WriteLine("----------------------------------------");

        // --- CONTA E ACESSO ---
        Console.WriteLine("1. Cadastrar Novo Usuário");
        Console.WriteLine("2. Entrar (Login)");

        // --- PUBLICAÇÕES E INTERAÇÕES ---
        Console.WriteLine("3. Criar Publicação");
        Console.WriteLine("4. Visualizar Publicação");
        Console.WriteLine("5. Editar Publicação");
        Console.WriteLine("6. Excluir Publicação");
        Console.WriteLine("7. Adicionar Comentário");

        // --- SESSÃO E SISTEMA ---
        Console.WriteLine("8. Encerrar Sessão (Logout)");
        Console.WriteLine("9. Limpar Tela");
        Console.WriteLine("0. Sair do Sistema");
        Console.WriteLine("========================================");
        Console.WriteLine("Escolha uma opção (0 a 9): ");

        string entrada = Console.ReadLine()!;

        if (int.TryParse(entrada, out int escolha))
        {
            return escolha;
        }
        return -1; // Retorno padrão para entradas inválidas
    }
}