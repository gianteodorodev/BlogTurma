using Isopoh.Cryptography.Argon2;

public class SenhaService
{
    public string GerarHash(string senha)
    {
        var senhaHash = Argon2.Hash(senha);
        return senhaHash;
    }

    public bool Verificar(string senha, string hashArmazenado)
    {
       return Argon2.Verify(hashArmazenado, senha);
    }
}