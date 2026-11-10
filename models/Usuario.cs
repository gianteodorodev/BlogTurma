using System.ComponentModel.DataAnnotations;

public class Usuario
{
    public int Id{get;set;}
    public string Nome{get;set;}
    public string Email{get;set;}
    private string SenhaHash{get;set;}


     public Usuario(int id, string nome, string email)
    {
        Id = id;
        Nome = nome;
        Email = email;
    }
    
    public Usuario(int id, string nome, string email, string senhahash)
    {
        Id = id;
        Nome = nome;
        Email = email;
        SenhaHash = senhahash;
    }
    
}