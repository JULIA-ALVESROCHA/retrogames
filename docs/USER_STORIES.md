# Histórias de usuário — RetroGames

Escopo: Módulo de Catálogo e Administração (Espec. 1.2). Regras de negócio (RN) e requisitos de
segurança (RS) conforme a especificação.

## Visitante (público)

**US01 — Navegar pelo catálogo**
Como visitante, quero ver a lista de jogos à venda em páginas, para escolher o que comprar.
- Só jogos ativos aparecem (RN04).
- A lista é paginada.

**US02 — Buscar e filtrar jogos**
Como visitante, quero buscar por parte do nome e filtrar por plataforma e gênero, para achar um jogo rápido.
- Filtros podem ser combinados.
- Os filtros ficam na query string, para a busca poder ser compartilhada.

**US03 — Ver o detalhe de um jogo**
Como visitante, quero ver os dados completos de um jogo, com plataforma, condição e gêneros.
- Jogo inexistente ou inativo responde "Jogo não encontrado" (404).

## Administrador

**US04 — Entrar e sair**
Como administrador, quero entrar com e-mail e senha, para acessar a área de administração.
- Sessão em cookie HttpOnly e SameSite=Strict, com proteção CSRF (RS01, RS02).
- Credenciais inválidas: "E-mail ou senha inválidos."

**US05 — Manter plataformas**
Como administrador, quero incluir, consultar, alterar e excluir plataformas.
- Não pode haver duas plataformas com o mesmo nome (RN01, 409).
- Plataforma com jogos não pode ser excluída (RN02, 409).

**US06 — Manter jogos**
Como administrador, quero incluir e alterar jogos, com plataforma, condição e gêneros.
- Plataforma, condição e ao menos um gênero devem existir (RN05, 400).
- Preço maior que zero, estoque maior ou igual a zero, ano entre 1970 e 2100 (RN06).
- Jogo e gêneros são gravados juntos, em uma transação.

**US07 — Desativar jogos**
Como administrador, quero desativar um jogo, para tirá-lo do catálogo sem perder o histórico.
- O jogo nunca é apagado: recebe st_ativo = 0 (RN03).
- Na administração, posso listar também os inativos (RN04).

**US08 — Consultar gêneros e condições**
Como administrador, quero ver os gêneros e as condições de jogo, para preencher o cadastro de jogos.
