using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;


public class PublicacaoView
{
    //exibir mensagem(string msg) escreve na tela

    public void ExibirMensagem (string mensagem)
    {
        Console.WriteLine($"[INFO]: {mensagem}");
    }
    //listarpublicações recebe a lista de publicações e escreve na tela
    public void ListarPublicacoes(List<Publicacao> publicacoes)
    {
        //se a lista tiver vazia impimir não tem publicacoes para exibir 
        if(publicacoes == null || publicacoes.Count == 0)
        {
            Console.WriteLine("\n[SISTEMA]: Nenhuma publicação encontrada.");
            return;
        }
        //Se nao IMprimir a lista

        Console.WriteLine("\n=========================================");
        Console.WriteLine("           FEED DE PUBLICAÇÕES             ");
        Console.WriteLine("===========================================");

        foreach (var pub in publicacoes)
        {
            Console.WriteLine($"\nID: {pub.Id} | Título: {pub.Titulo}");
            Console.WriteLine($"Autor: {(string.IsNullOrEmpty(pub.NomeAutor) ? "Desconhecido" : pub.NomeAutor)}");
            Console.WriteLine($"Data:{pub.Data_publicacao:dd/MM/yyyy HH:mm}");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine(pub.Textos);
            Console.WriteLine("---------------------------------------");

            Console.WriteLine(" [Comentários]:");
            if (pub.Comentarios == null || pub.Comentarios.Count == 0)
            {
                Console.WriteLine(" > Nenhum comentário registrado.");
            }
            else
            {
                foreach (var com in pub.Comentarios)
                {
                    string autorComentario = string.IsNullOrEmpty(com.NomeUsuario) ? "Desconhecido": com.NomeUsuario;
                    Console.WriteLine($"  > [{com.DataCriacao:dd/MM/yyyy HH;mm}] Usuário #{com.NomeUsuario}: {com.Texto}");
                }
            }
        }
    }

    public(string Titulo, string Textos) SolicitarDadosPublicacao()
    {
        Console.WriteLine("\n--- NOVA PUBLICAÇÃO ---");

        Console.Write("Título: ");
        string titulo = Console.ReadLine()?.Trim() ?? string.Empty;

        Console.Write("Conteúdo: ");
        string textos = Console.ReadLine()?.Trim() ?? string.Empty;

        return (titulo,textos);
    }

    public int SolicitarId()
    {
        Console.WriteLine("\nInforme o ID da publicação: ");
        
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }

        return 0;
    }

    public string SolicitarTextoComentario()
    {
        Console.WriteLine("\nDigite o seu comentário: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    public bool ConfirmarExclusao()
    {
        Console.WriteLine("\nTem certeza que deseja excluir esta publicação? (S/N); ");
        string resposta = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

        return resposta == "S" || resposta == "SIM";
    }

    public void ExibirDetalhes(Publicacao? publicacao, string? nomeAutor = null, List<Comentario>? comentarios = null)
    {
        if (publicacao == null)
        {
            Console.WriteLine("\n[SISTEMA]: Nenhuma publicação encontrada.");
            return;
        }

        Console.WriteLine("\n====================================");
        Console.WriteLine("        LISTA DE PUBLICAÇÕES          ");
        Console.WriteLine("======================================");

        Console.WriteLine($"\nID: {publicacao.Id} | Título: {publicacao.Titulo}");
        Console.WriteLine($"Autor: {publicacao.NomeAutor}");
        Console.WriteLine($"Data: {publicacao.Data_publicacao:dd/MM/yyyy HH:mm}");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(publicacao.Textos);
        Console.WriteLine("----------------------------------");

        Console.WriteLine("\n[COMENTÁRIOS]:");

        if (comentarios == null || comentarios.Count == 0)
            {
                Console.WriteLine("  > Nenhum comentário até o momento.");
            }
        else
            {
                foreach (var com in comentarios)
                {
                    Console.WriteLine($"  > [{com.DataCriacao:dd/MM/yyyy HH:mm}] Usuário #{publicacao.NomeAutor}: {com.Texto}");
                }
            }
            Console.WriteLine("======================================");    
    }
}