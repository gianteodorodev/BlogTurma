// See https://aka.ms/new-console-template for more information
using MySqlConnector.Logging;

Conexao conexao = new Conexao();
MenuView menuView = new MenuView();
SessaoUsuario sessaoUsuario = new SessaoUsuario();
ComentarioService comentarioService = new ComentarioService(conexao, sessaoUsuario);

PublicacaoView publicacaoView = new PublicacaoView();
PublicacaoService publicacaoService = new PublicacaoService(conexao,sessaoUsuario, comentarioService);
PublicacaoController publicacaoController = new PublicacaoController(publicacaoView,publicacaoService,comentarioService,sessaoUsuario);


UsuarioService usuarioService= new UsuarioService(conexao);
AutenticacaoView autenticacaoView = new AutenticacaoView();
AutenticacaoController autenticacaoController = new AutenticacaoController(
    autenticacaoView,
    usuarioService,
    sessaoUsuario
);

try
{
    var conection = conexao.CriarConexao();
    conection.Open();
    Console.WriteLine("Conexão com o BD estabelecida!");
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

bool executando = true;

while(executando)
{
    //Criar objeto do menu view e chamar a funcao de exibir menu
    int escolha = menuView.ExibirMenu(sessaoUsuario.EstaAutenticado);
    //se opcao listar chamar o publicacao controller listar
    if(escolha == 1)
    {
        autenticacaoController.Cadastrar();
    }
    
    else if(escolha == 2)
    {
        autenticacaoController.Entrar();
    }

    else if(escolha == 3)
    {
        publicacaoController.Criar();
    }

    else if(escolha == 4)
    {
        publicacaoController.Listar();
    }

    else if(escolha == 5)
    {
        publicacaoController.Editar();
    }

    else if(escolha == 6)
    {
        publicacaoController.Excluir();
    }

    else if(escolha == 7)
    {
        publicacaoController.Comentar();
    }

    else if(escolha == 8)
    {
        if (sessaoUsuario.EstaAutenticado)
        {
            sessaoUsuario.Encerrar();
            Console.WriteLine("\n[SISTEMA]: Sessão encerrada com sucesso!");
        }
        else
        {
            Console.WriteLine("\n[SISTEMA]: Nenhum usuário autenticado no momento.");
        }
    }

    else if(escolha == 9)
    {
        Console.Clear();
    }

    else if(escolha == 0)
    {
        executando = false;
        Console.WriteLine("\nSaindo do sistema... Até Logo!");
    }
    else
    {
        Console.WriteLine("\n[ERRO]: Opção inválida. Digite um número de 0 a 9.");
    }
}   