using MySqlConnector;

public class Conexao
{
    private string configuracaoConexao = "";
    public Conexao()
    {
        configuracaoConexao = "server=127.0.0.1; port=3307; uid=dev_gian; password=1234; database=BlogTurma;";
    }

    public MySqlConnection CriarConexao()
    {
        return new MySqlConnection(configuracaoConexao);
    }

    private string LerVariavel(string configuracaoConexao)
    {
        var builder = new MySqlConnectionStringBuilder(configuracaoConexao);

        if(builder.TryGetValue(configuracaoConexao, out object valor))
        {
            return valor?.ToString();
        }
        return null;
    } 
} 
