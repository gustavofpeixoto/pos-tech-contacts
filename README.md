![Capa com o nome do curso da pós graduação](./assets/thumbnail.png)

![Badge em Desenvolvimento](http://img.shields.io/static/v1?label=STATUS&message=EM%20DESENVOLVIMENTO&color=GREEN&style=for-the-badge)

# Índice 

- [Descrição do projeto](#-descrição-do-projeto)
- [Funcionalidades do aplicativo](#-funcionalidades-do-aplicativo)
- [Técnicas e tecnologias utilizadas](#%EF%B8%8F-técnicas-e-tecnologias-utilizadas)
- [Abrir e rodar o projeto](#-abrir-e-rodar-o-projeto)
- [Contato](#-contato)

## 📚 Descrição do projeto

Projeto em Desenvolvimento para o Tech Challenge da Pós-Graduação em Arquitetura de Sistemas .NET da FIAP.
O objetivo do Tech Challenge é desenvolver um aplicativo utilizando a plataforma .NET 8 para o cadastro de contatos regionais. Este projeto considera a persistência de dados e a qualidade do software

## 🔨 Funcionalidades do aplicativo

### Cadastro de contatos
Cadastro de novos contatos, incluindo nome, sobrenome, telefone e e-mail. Cada contato é associado a um DDD correspondente à região.

### Atualização e exclusão
Atualização e a exclusão de contatos previamente cadastrados.

### Consulta de contatos
Foram implementadas duas funcionalidades para a consulta de contatos:

1 - Recuperação por Identificador Único: Permite recuperar o contato por meio do identificador único informado.

2 - Busca Avançada: Permite recuperar uma lista de contatos utilizando filtros como nome, sobrenome, DDD, e-mail ou telefone.

## ✔️ Técnicas e tecnologias utilizadas

### Arquitetura

Foi adotada a arquitetura limpa para modelar o aplicativo de contatos. Esta é uma estrutura de design de software com várias camadas, promovendo uma organização clara e fácil de compreender, o que é benéfico para o desenvolvimento.

A principal característica da arquitetura limpa é a separação e independência das camadas, desacoplando a lógica de negócios das influências externas, como a interface do usuário (UI), frameworks, bancos de dados, entre outros. Isso é alcançado ao definir uma camada de domínio independente e isolada.

Representação das camadas:

![Capa com o nome do curso da pós graduação](./assets/clean-architecture.png)

### Persistência de dados

A persistência de dados foi realizada com o banco de dados SQL Server, utilizando o ORM Entity Framework para o mapeamento e a criação do banco (metodologia code-first).

### Validações

Para as validações, foi utilizada a biblioteca FluentValidation. Isso garante a consistência dos dados armazenados e impede a manipulação incorreta dos mesmos.

### Mediator

Para facilitar o desenvolvimento, a manutenção e manter o código limpo e legível, é importante seguir os princípios SOLID, padrões de projetos e outras boas práticas, como o desacoplamento dos objetos. Dentro deste grupo de recomendações, a adoção do Mediator Pattern tem ganhado destaque. Neste projeto o Mediator foi implementado utilizando a biblioteca MediatR.

## 📁 Abrir e rodar o projeto

### Preparando o banco de dados

Para criar o banco de dados, basta executar o script **pos-tech-contacts/scripts/1-create-data-base.sql** e para popular as tabelas Regions e Ddds o script **pos-tech-contacts/scripts/2-insert-regions=and-ddds.sql**

### Rodando a aplicação

A aplicação foi testada localmente utilizando o SDK .NET 8.0.11. Para instalar o SDK, visite esta [página](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) e faça o download. 

Para executar a aplicação, utilize o comando **dotnet run** e especifique o projeto **PosTech.Contacts.Api.csproj**, conforme demonstrado a seguir:

![Capa com o nome do curso da pós graduação](./assets/dotnet-run-command.png)

## 📚 Contato

Gustavo Peixoto
gustavo.fonseca.peixoto@gmail.com
