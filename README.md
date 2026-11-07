# RetroGames

Loja web de jogos retrô usados. Projeto integrador do curso de Desenvolvimento Web Full Stack com IA.

Escopo atual: Módulo de Catálogo e Administração. A especificação completa está no documento
**RetroGames — Arquitetura e Stack**, fornecido pelo professor. As regras para assistentes de IA
estão em [AGENTS.md](AGENTS.md).

## Estrutura

```
retrogames/
├── database/                 scripts SQL (estrutura e dados de exemplo)
├── docs/                     MER, histórias de usuário e diagrama
├── RetroGames.Model/         entidades (uma classe por tabela) e tipos compartilhados
├── RetroGames.Repository/    acesso a dados com ADO.NET + MySqlConnector
├── RetroGames.sln
├── .env.example              modelo do .env (sem senha)
└── .gitignore
```

Os projetos `RetroGames.Services`, `RetroGames.WebApi.ApiModels`, `RetroGames.WebApi`,
`RetroGames.WebApi.UnitTests` e o front-end `retrogames.web` entram nas próximas aulas.

## Como preparar o ambiente

1. No MySQL Workbench, execute, nesta ordem:
   - `database/001_criar_tabelas.sql`
   - `database/002_dados_exemplo.sql`
2. Na raiz do repositório, copie `.env.example` para `.env` e troque `SUA_SENHA` pela sua senha do MySQL.
   O `.env` está no `.gitignore` e nunca vai para o GitHub.
3. Compile a solução:

```bash
dotnet build
```

## Progresso

| Aula | Conteúdo | Situação |
| --- | --- | --- |
| 1 | Banco de dados, dados de exemplo e classes de modelo | Concluída |
| 2 | Camada Repository e conexão com o banco | Concluída |
