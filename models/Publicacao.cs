public class Publicacao
{
    public int Id{get;set;}
    public string Titulo{get;set;}
    public string Textos{get;set;}
    public DateTime Data_publicacao{get;set;}
    public int UsuarioId {get;set;}

    public string NomeAutor {get; set;} = string.Empty;
    public List<Comentario> Comentarios {get;set;} = new List<Comentario>();
    public Publicacao() {}
    public Publicacao(int id, string titulo, string textos, DateTime data_publicacao, int usuarioid, string nomeAutor = "")
    {
        Id = id;
        Titulo = titulo;
        Textos = textos;
        Data_publicacao = data_publicacao;
        UsuarioId = usuarioid;
        NomeAutor = nomeAutor;
    }
}