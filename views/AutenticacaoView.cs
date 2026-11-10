public class AutenticacaoView
{
    public (string Nome, string Email, string Senha) SolicitarDadosCadastro()
    {
        Console.Clear();
        Console.WriteLine("\n--- CADASTRO DE USUÁRIO ---");
        Console.Write("\nInforme seu nome: ");
        string nome = Console.ReadLine()?.Trim() ?? string.Empty;
        
        Console.Write("Informe seu e-mail: ");
        string email = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Informe a senha: ");
        string senha = LerSenhaOculta();

        return (nome, email, senha);
    }

    public (string Email, string Senha) SocilitarCredenciais()
    {
        Console.Clear();
        Console.WriteLine("\n--- LOGIN DE USUÁRIO ---");
        Console.Write("\n E-mail: ");
        string email = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Senha: ");
        string senha = LerSenhaOculta();

        return (email, senha);
    }

    public void ExibirMensagem(string mensagem)
    {
        Console.WriteLine($"\n[SISTEMA]: {mensagem}");
    }

    private string LerSenhaOculta()
    {
        string senha = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && senha.Length > 0)
            {
                senha = senha[..^1];
                Console.Write("\b \b");
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                senha += keyInfo.KeyChar;
                Console.Write("*");
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return senha;
    }
}