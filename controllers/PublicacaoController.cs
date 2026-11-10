//chama o service para executar o servico e a view para exibir o resultado
public class PublicacaoController
{
    private readonly PublicacaoView _view;
    private readonly PublicacaoService _publicacaoService;
    private readonly ComentarioService _comentarioService;
    private readonly SessaoUsuario _sessaoUsuario;

    public PublicacaoController(
        PublicacaoView view, 
        PublicacaoService publicacaoService, 
        ComentarioService comentarioService, 
        SessaoUsuario sessaoUsuario)
    {
        _view = view;
        _publicacaoService = publicacaoService;
        _sessaoUsuario = sessaoUsuario;
        _comentarioService = comentarioService;
    }

    public void Listar()
    {
        var publicacoes = _publicacaoService.ListarRecentes();
        _view.ListarPublicacoes(publicacoes);
    }

    public void Criar()
    {
        if (!VerificarSessao()) return;
        
        var (titulo, textos) = _view.SolicitarDadosPublicacao();

        bool sucesso = _publicacaoService.Criar(titulo, textos, DateTime.Now, _sessaoUsuario.UsuarioId);

        if (sucesso)
        {
            Console.WriteLine("\n[SISTEMA]: Publicação realizada com sucesso!");    
        }
        else
        {
           Console.WriteLine("\n[SISTEMA]: Falha ao criar publicação. Verifique os dados informados."); 
        }
    }

    public void Visualizar()
    {
        if (!VerificarSessao()) return;

        int id = _view.SolicitarId();
        if (id <= 0)
        {
            Console.WriteLine("\n[SISTEMA]: ID inválido.");
            return;
        }

        var publicacao = _publicacaoService.ObterPorId(id);

        if (publicacao == null)
        {
            _view.ExibirDetalhes(null);
            return;
        }

        string? nomeAutor = _publicacaoService.ObterNomeAutor(publicacao.UsuarioId);
        var comentarios = _comentarioService.ListarPorPublicacao(publicacao.Id);
        _view.ExibirDetalhes(publicacao, nomeAutor, comentarios);
    }

    public void Editar()
    {
        if (!VerificarSessao()) return;

        int id = _view.SolicitarId();
        if (id <= 0)
        {
            Console.WriteLine("\n[SISTEMA]: ID inválido.");
            return;
        }

        var (novoTitulo, novoTexto) = _view.SolicitarDadosPublicacao();

        bool sucesso = _publicacaoService.Editar(id, novoTitulo, novoTexto);

        if (sucesso)
        {
            Console.WriteLine("\n[SISTEMA]: Publicação alterada com sucesso!");
        }
        else
        {
            Console.WriteLine("\n[SISTEMA]: Falha ao editar. Verifique se a publicação existe e se você é o autor.");
        }
    }

    public void Excluir()
    {
        if (!VerificarSessao()) return;

        int id = _view.SolicitarId();
        if (id <= 0)
        {
            Console.WriteLine("\n[SISTEMA]: ID inválido.");
            return;
        }

        bool confirmou = _view.ConfirmarExclusao();
        if (!confirmou)
        {
            Console.WriteLine("\n[SISTEMA]: Operação de exclusão cancelada pelo usuário.");
        }

        bool sucesso = _publicacaoService.Excluir(id);

        if (sucesso)
        {
            Console.WriteLine("\n[SISTEMA]: Publicação excluída com sucesso!");
        }
        else
        {
            Console.WriteLine("\n[SISTEMA]: Falha ao excluir. Verifique se a publicação existe e se você é o autor.");
        }
    }

    public void Comentar()
    {
        if (!VerificarSessao()) return;

        int publicacaoId = _view.SolicitarId();
        if (publicacaoId <=0)
        {
            Console.WriteLine("\n[SISTEMA]: ID inválido.");
            return;
        }

        string texto = _view.SolicitarTextoComentario();

        bool sucesso = _comentarioService.Adicionar(publicacaoId, texto);

        if (sucesso)
        {
            Console.WriteLine("\n[SISTEMA]: Comentário adicionado com sucesso!");
        }
        else
        {
            Console.WriteLine("\n[SISTEMA]: Falha ao adicionar comentário. Verifique o texto ou a existência da publicação.");
        }
    }

    #region Métodos Privados Auxiliares

    private bool VerificarSessao()
    {
        if (!_sessaoUsuario.EstaAutenticado)
        {
            Console.WriteLine("\n[SISTEMA]: Acesso negado. Você precisa estar autenticado para realizar esta ação.");
            return false;
        }

        return true;
    }

    #endregion
}