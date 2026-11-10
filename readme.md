# BlogTurma

Sistema de blog em C# com interface de console, desenvolvido para gerenciar usuários, publicações e comentários em um banco de dados MySQL.

## Visão geral

O BlogTurma permite:

- cadastro de usuários;
- autenticação com senha protegida por hash;
- criação, listagem, edição e exclusão de publicações;
- adição de comentários em publicações;
- controle de sessão do usuário autenticado;
- menu interativo em terminal.

A aplicação segue uma estrutura simples em camadas, com separação entre:

- `controllers` — fluxo de ações do sistema;
- `views` — interação com o usuário no console;
- `services` — regras de negócio e acesso ao banco;
- `models` — entidades do domínio;
- `data` — conexão com a base de dados.

## Tecnologias

- C#
- .NET 10
- MySQL
- MySqlConnector
- Argon2 para hash de senhas

## Estrutura do projeto

```text
BlogTurma/
├── controllers/
│   ├── AutenticacaoController.cs
│   └── PublicacaoController.cs
├── data/
│   └── Conexao.cs
├── models/
│   ├── Comentario.cs
│   ├── Publicacao.cs
│   └── Usuario.cs
├── services/
│   ├── ComentarioService.cs
│   ├── PublicacaoService.cs
│   ├── SenhaService.cs
│   ├── SessaoUsuario.cs
│   └── UsuarioService.cs
├── views/
│   ├── AutenticacaoView.cs
│   ├── MenuView.cs
│   └── PublicacaoView.cs
├── BlogTurma.csproj
├── Program.cs
└── readme.md
```

## Requisitos

Antes de executar a aplicação, certifique-se de ter instalado:

- SDK do .NET 10
- MySQL Server
- cliente/driver MySQL acessível localmente

## Configuração do banco de dados

A aplicação conecta-se a um banco MySQL local. A string de conexão está em `data/Conexao.cs` e deve ser ajustada conforme o ambiente local.

Exemplo de configuração esperada:

```csharp
server=127.0.0.1; port=3307; uid=dev_gian; password=...; database=BlogTurma;
```

As tabelas esperadas pelo projeto incluem os nomes abaixo:

- `usuario`
- `publicacao`
- `comentario`

O código utiliza consultas com esses nomes, então a estrutura do banco deve seguir o mesmo padrão.

## Como executar

1. Abra o terminal na raiz do projeto.
2. Ajuste as credenciais e o banco em `data/Conexao.cs`.
3. Execute:

```bash
dotnet restore
dotnet run
```

## Menu principal

Ao iniciar, o sistema exibe um menu com as opções:

1. Cadastrar Novo Usuário
2. Entrar (Login)
3. Criar Publicação
4. Visualizar Publicação
5. Editar Publicação
6. Excluir Publicação
7. Adicionar Comentário
8. Encerrar Sessão
9. Limpar Tela
0. Sair do Sistema

## Funcionalidades principais

### Autenticação

- cadastro de usuário com nome, email e senha;
- verificação de dados vazios;
- hash seguro de senha usando Argon2;
- login com validação de email e senha;
- sessão ativa por usuário autenticado.

### Publicações

- criação com título e texto;
- listagem em ordem de publicação mais recente;
- edição apenas pelo autor da publicação;
- exclusão apenas pelo autor da publicação;
- validação de títulos e conteúdos mínimos.

### Comentários

- inclusão de comentários vinculados à publicação;
- associação com o usuário autenticado;
- listagem dos comentários da publicação.

## Observações

Este projeto é uma aplicação de console, voltada para fins didáticos e de prática de desenvolvimento em C# com acesso a banco de dados relacional.

Se você quiser evoluir o sistema, sugestões futuras incluem:

- painel web com ASP.NET Core;
- relatórios de publicações por usuário;
- paginação e busca;
- upload de imagens;
- autenticação com JWT ou cookies.

## Licença

Este projeto foi desenvolvido como trabalho acadêmico/educacional e pode ser adaptado conforme necessidade do usuário.
