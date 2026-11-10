
using Isopoh.Cryptography.Argon2;
using MySqlConnector;

public class UsuarioService
{
    private readonly Conexao _conexao;
    private readonly SenhaService _senhaService;

    public UsuarioService(Conexao conexao)
    {
        _conexao = conexao ?? throw new ArgumentNullException(nameof(conexao));
        _senhaService = new SenhaService();    
    }

    public bool Cadastrar(string nome, string email, string senha)
    {
        if(string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
            return false;

        try
        {
        string senhaCriptografada = Argon2.Hash(senha);

        using var Connection = _conexao.CriarConexao();
       
        Connection.Open();

        using var command = new MySqlCommand("INSERT INTO usuario (nome, email, senha) VALUES (@nome, @email, @senha)", Connection);
        
        command.Parameters.AddWithValue("@nome", nome);
        command.Parameters.AddWithValue("@email", email);
        command.Parameters.AddWithValue("@senha", senhaCriptografada);

        var resultado = command.ExecuteNonQuery();

        if(resultado == 1)
            {
                return true;
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return false;
    }
    public Usuario? Autenticar(string email, string senha)
    {
        if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(senha))
        return null;

        using var Connection = _conexao.CriarConexao();
        Connection.Open();
        
        using var command = new MySqlCommand (("SELECT * FROM usuario WHERE email = @email"), Connection);
        command.Parameters.AddWithValue("@email", email);

        using var reader = command.ExecuteReader();

        string hashArmazenado = "";

        if(reader.Read())
        {
            hashArmazenado = reader.GetString("senha");
            if(_senhaService.Verificar(senha, hashArmazenado))
            {
                return new Usuario(
                    reader.GetInt32("id_usuario"),
                    reader.GetString("nome"),
                    reader.GetString("email")
                );
            }

        }
    
        return null;
    }
}