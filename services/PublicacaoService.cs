using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Security.Cryptography;
using MySqlConnector;

public class PublicacaoService
{
    private readonly SessaoUsuario _sessaoUsuario; 
    private readonly Conexao _conexao;
    private ComentarioService _comentarioService;

    public PublicacaoService(Conexao conexao, SessaoUsuario sessaoUsuario, ComentarioService comentarioService)
    {
        _sessaoUsuario = sessaoUsuario;
        _conexao = conexao ?? throw new ArgumentNullException(nameof(conexao));
        _comentarioService = comentarioService;
    }

    public List<Publicacao> ListarRecentes()
    {

        var publicacoes = new List<Publicacao>();

        try
        {
            using (var connection = _conexao.CriarConexao())
            {
                connection.Open();

                string sql = @"
                SELECT 
                    p.id_publicacao, 
                    p.titulo, 
                    p.textos, 
                    p.data_publicacao, 
                    p.id_usuario, 
                    u.nome AS nome_autor
                FROM publicacao p
                INNER JOIN usuario u ON p.id_usuario = u.id_usuario
                ORDER BY p.data_publicacao DESC;";
                
                using var command = new MySqlCommand(sql, connection);
                using var reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    var pub = new Publicacao(
                        reader.GetInt32("id_publicacao"),
                        reader.GetString("titulo"),
                        reader.GetString("textos"),
                        reader.GetDateTime("data_publicacao"),
                        reader.GetInt32("id_usuario"),
                        reader.GetString("nome_autor")
                    );

                    publicacoes.Add(pub);
                }
            }
            
            foreach (var pub in publicacoes)
            {
                pub.Comentarios = _comentarioService.ListarPorPublicacao(pub.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
        }

        return publicacoes;
    }

    public bool Criar(string titulo, string textos, DateTime data_publicacao, int usuarioId)
    {
        if (!ValidarDadosPublicacao(titulo,textos))
            return false;

        usuarioId = ObterUsuarioAutenticadoId();

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand("INSERT INTO publicacao (titulo, data_publicacao, textos, id_usuario) VALUES (@titulo, @data_publicacao, @textos, @usuarioId)", connection);
            command.Parameters.AddWithValue("@titulo", titulo);
            command.Parameters.AddWithValue("@data_publicacao", data_publicacao);
            command.Parameters.AddWithValue("@textos", textos);
            command.Parameters.AddWithValue("@usuarioId", usuarioId);

            return command.ExecuteNonQuery() > 0;
        }
        catch
        {
            return false;
        }
    }

    public Publicacao? ObterPorId(int Id)
    {
        if (Id <= 0) return null;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand("SELECT id_publicacao, titulo, data_publicacao, textos, id_usuario FROM publicacao WHERE id_publicacao = @id", connection);
            command.Parameters.AddWithValue("@id", Id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Publicacao
                (
                  reader.GetInt32("id"),
                  reader.GetString("titulo"),
                  reader.GetString("texto"),
                  reader.GetDateTime("data_publicacao"),
                  reader.GetInt32("usuario_id")
                );
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
    
    public string? ObterNomeAutor(int usuarioId)
    {
        if (usuarioId <= 0) return null;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand("SELECT nome FROM usuario WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", usuarioId);

            var resultado = command.ExecuteScalar();
            return resultado?.ToString();
        }
        catch
        {
            return null;
        }
    }

    public bool Editar(int id_publicacao, string novoTitulo, string novoTexto)
    {
        if (id_publicacao <= 0 || string.IsNullOrWhiteSpace(novoTitulo) || string.IsNullOrWhiteSpace(novoTexto))
            return false;
        
        if (!_sessaoUsuario.EstaAutenticado)
            return false;

        int id_usuario = _sessaoUsuario.UsuarioId;

        // Garante que apenas o autor original possa editar a publicacao
        if (!VerificarAutoria(id_publicacao, id_usuario))
            return false;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand(
                "UPDATE publicacao SET titulo = @titulo, textos = @textos WHERE id_publicacao = @id_publicacao",
                connection
            );
            command.Parameters.AddWithValue("@id_publicacao", id_publicacao);
            command.Parameters.AddWithValue("@titulo", novoTitulo.Trim());
            command.Parameters.AddWithValue("@textos", novoTexto.Trim());
            

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
            return false;
        }
    }

    public bool Excluir(int id_publicacao)
    {
        if (id_publicacao <= 0)
            return false;
        
        if (!_sessaoUsuario.EstaAutenticado)
            return false;
        
        int id_usuario = _sessaoUsuario.UsuarioId;

        // Garante que apenas o autor original possa excluir a publicacao
        if (!VerificarAutoria(id_publicacao, id_usuario))
            return false;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand(
                "DELETE FROM publicacao WHERE id_publicacao = @id_publicacao",
                connection
            );
            command.Parameters.AddWithValue("@id_publicacao", id_publicacao);

            return command.ExecuteNonQuery() > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
            return false;
        }
    }

    #region Métodos Privados Auxiliares 

    private int ObterUsuarioAutenticadoId()
    {
        if (!_sessaoUsuario.EstaAutenticado)
        {
            throw new InvalidOperationException("Usuario não está autenticado para realizar esta operação.");
        }
        return _sessaoUsuario.UsuarioId;
    }

    private bool ValidarDadosPublicacao(string titulo, string textos)
    {
        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(textos))
            return false;

        if (titulo.Trim().Length < 3 || textos.Trim().Length < 5)
            return false;

        return true;
    }

    private bool VerificarAutoria(int id_publicacao, int id_usuario)
    {
        if (id_publicacao <= 0 || id_usuario <= 0)
            return false;

        try
        {
            using var connection = _conexao.CriarConexao();
            connection.Open();

            using var command = new MySqlCommand(
                "SELECT COUNT(1) FROM publicacao WHERE id_publicacao = @id_publicacao AND id_usuario = @id_usuario",
                connection
            );
            command.Parameters.AddWithValue("@id_publicacao", id_publicacao);
            command.Parameters.AddWithValue("@id_usuario", id_usuario);

            var resultado = Convert.ToInt32(command.ExecuteScalar());
            return resultado > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n[ERRO BANCO/SQL]: {ex.Message}");
            return false;
        }
    }
    #endregion
}