using MySqlConnector;

public class ComentarioService
{
    private readonly Conexao _conexao;
    private readonly SessaoUsuario _sessaoUsuario;

    public ComentarioService(Conexao conexao, SessaoUsuario sessaoUsuario)
    {
        _conexao = conexao;
        _sessaoUsuario = sessaoUsuario;
    }

    public bool Adicionar(int publicaçãoId, string texto)
    {
        if (publicaçãoId <= 0 || string.IsNullOrWhiteSpace(texto))
        return false;

        if(!_sessaoUsuario.EstaAutenticado)
        return false;

        int usuarioId = _sessaoUsuario.UsuarioId;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand("INSERT INTO comentario (texto, data_comentario, id_usuario, id_publicacao) VALUE (@texto, @data_comentario, @id_usuario,  @id_publicacao)", connection);

            command.Parameters.AddWithValue("@texto", texto.Trim());
            command.Parameters.AddWithValue("@data_comentario", DateTime.Now);
            command.Parameters.AddWithValue("@id_usuario", usuarioId);
            command.Parameters.AddWithValue("@id_publicacao", publicaçãoId);
            

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
            return false;
        }
    }

    public List<Comentario> ListarPorPublicacao(int id_publicacao)
    {
        var listaComentarios = new List<Comentario>();

        if (id_publicacao <= 0)
        return listaComentarios;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            string sql = @"
            SELECT
                c.id_comentario,
                c.texto,
                c.data_comentario,
                c.id_publicacao,
                c.id_usuario,
                u.nome AS nome_usuario
            FROM comentario c
            INNER JOIN usuario u ON c.id_usuario = u.id_usuario
            WHERE c.id_usuario = @idPublicacao
            ORDER BY c.data_comentario ASC";

            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id_publicacao", id_publicacao);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var comentario = new Comentario(
                    reader.GetInt32("id_comentario"),
                    reader.GetString("texto"),
                    reader.GetDateTime("data_comentario"),
                    reader.GetInt32("id_publicacao"),
                    reader.GetInt32("id_usuario"),
                    reader.GetString("nome_usuario")
                );

                listaComentarios.Add(comentario);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
        }

        return listaComentarios;
    }
}