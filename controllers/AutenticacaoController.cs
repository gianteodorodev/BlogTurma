public class AutenticacaoController
{
    private readonly AutenticacaoView _view;
    private readonly UsuarioService _usuarioService;
    private readonly SessaoUsuario _sessaoUsuario;

    public AutenticacaoController (AutenticacaoView view, UsuarioService usuarioService, SessaoUsuario sessaoUsuario)
    {
        _view = view;
        _usuarioService = usuarioService;
        _sessaoUsuario = sessaoUsuario;
    }

    public void Cadastrar()
    {
        var(nome, email, senha) = _view.SolicitarDadosCadastro();
        bool cadastro = _usuarioService.Cadastrar(nome, email, senha);

        if (cadastro)
        {
            _view.ExibirMensagem("Cadastro realizado com sucesso!");
        }
        else 
        {
            _view.ExibirMensagem("Falha no cadastro, verifique as informações fornecidadas.");
        }
    }
    public void Entrar()
    {
        var(email, senha) = _view.SocilitarCredenciais();

        Usuario user = _usuarioService.Autenticar(email, senha);

        if(user == null)
        {
            _view.ExibirMensagem("Credenciais Inválidas, tente novamante.");
            return;
        }
        _sessaoUsuario.Iniciar(user);
        _view.ExibirMensagem("Login realizado com sucesso!");
    }
}