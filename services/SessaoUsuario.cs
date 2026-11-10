using System;


public class SessaoUsuario
{
    private int ?_usuarioId;
    private string _nomeUsuario;

    public int UsuarioId => _usuarioId ?? -1;
    public string NomeUsario => _nomeUsuario;

    public bool EstaAutenticado => _usuarioId.HasValue;

    public SessaoUsuario()
    {
        Encerrar();
    }

    public void Iniciar(Usuario user)
    {
        if(user == null)
        {
            return;
        }
        
        _usuarioId = user.Id;
        _nomeUsuario = user.Nome;
    }

    public void Encerrar()
    {
        _usuarioId = null;
        _nomeUsuario = "Anônimo";
    }
}