public class Comentario
{
    public int Id{get;set;}
    public string Texto{get;set;}
    public DateTime DataCriacao{get;set;}
    public int UsuarioId{get;set;}
    public int PublicacaoId{get;set;}
    public string NomeUsuario{get; set;} = string.Empty;

    public Comentario(int id, string texto, DateTime datacriacao, int usuarioid, int publicacaoid, string nomeUsuario = "")
    {
        Id = id;
        Texto = texto;
        DataCriacao = datacriacao;
        UsuarioId = usuarioid;
        PublicacaoId = publicacaoid;
        NomeUsuario = nomeUsuario;
    }
}