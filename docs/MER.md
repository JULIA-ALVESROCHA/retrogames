# MER — RetroGames

Modelo relacional do banco `retrogames`. Script: [`database/001_criar_tabelas.sql`](../database/001_criar_tabelas.sql).
Diagrama: [`modelo-relacional.png`](modelo-relacional.png).

O banco guarda só a estrutura: PK, FK com comportamento padrão, tipos, `NOT NULL` e `UNIQUE`.
Não há trigger, function, `CHECK` nem `ON DELETE`. Toda regra de negócio fica no Service.

## Convenção de nomes

| Prefixo | Significado | Exemplo |
| --- | --- | --- |
| `tb_` | tabela | `tb_jogo` |
| `co_seq_` | chave primária gerada pelo banco (AUTO_INCREMENT) | `co_seq_jogo` |
| `co_` | chave fixa de tabela de tipo, ou chave estrangeira | `co_condicao_jogo` |
| `no_` / `ds_` | nome / descrição | `no_jogo`, `ds_email` |
| `nu_` / `qt_` / `vl_` | número / quantidade / valor `DECIMAL(10,2)` | `qt_estoque`, `vl_preco` |
| `dt_` | data e hora em UTC | `dt_cadastro` |
| `st_` | booleano (situação) | `st_ativo` |

## Tabelas

Legenda: PK chave primária · FK chave estrangeira · UQ `UNIQUE` · IX índice · NULL coluna opcional.
Colunas sem NULL são `NOT NULL`.

### Catálogo e Administração

**tb_condicao_jogo** (tipo) → classe `CondicaoJogo`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_condicao_jogo | INT | PK | Id |
| ds_condicao_jogo | VARCHAR(50) | UQ | Descricao |

**tb_perfil** (tipo) → classe `Perfil`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_perfil | INT | PK | Id |
| ds_perfil | VARCHAR(50) | UQ | Descricao |

**tb_plataforma** (cadastro) → classe `Plataforma`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_plataforma | INT | PK | Id |
| no_plataforma | VARCHAR(80) | UQ | Nome |
| no_fabricante | VARCHAR(80) | | Fabricante |
| nu_ano_lancamento | INT | | AnoLancamento |

**tb_genero** (cadastro, somente leitura) → classe `Genero`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_genero | INT | PK | Id |
| no_genero | VARCHAR(50) | UQ | Nome |

**tb_jogo** (principal) → classe `Jogo`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_jogo | INT | PK | Id |
| no_jogo | VARCHAR(150) | IX | Nome |
| ds_jogo | VARCHAR(1000) | NULL | Descricao |
| nu_ano_lancamento | INT | | AnoLancamento |
| vl_preco | DECIMAL(10,2) | | Preco |
| qt_estoque | INT | | Estoque |
| st_ativo | BOOLEAN | | Ativo |
| dt_cadastro | DATETIME | | DataCadastro |
| co_plataforma | INT | FK → tb_plataforma | IdPlataforma |
| co_condicao_jogo | INT | FK → tb_condicao_jogo | IdCondicaoJogo |

**tb_jogo_genero** (associativa N:N) → classe `JogoGenero`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_jogo | INT | PK, FK → tb_jogo | IdJogo |
| co_genero | INT | PK, FK → tb_genero | IdGenero |

**tb_usuario** (cadastro) → classe `Usuario`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_usuario | INT | PK | Id |
| no_usuario | VARCHAR(100) | | Nome |
| ds_email | VARCHAR(256) | UQ | Email |
| ds_senha_hash | VARCHAR(500) | | SenhaHash |
| st_ativo | BOOLEAN | | Ativo |
| dt_cadastro | DATETIME | | DataCadastro |
| co_perfil | INT | FK → tb_perfil | IdPerfil |

### Compras (tarefa)

**tb_situacao_pedido** (tipo) → classe `SituacaoPedido`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_situacao_pedido | INT | PK | Id |
| ds_situacao_pedido | VARCHAR(50) | UQ | Descricao |

**tb_pedido** → classe `Pedido`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_pedido | INT | PK | Id |
| dt_pedido | DATETIME | | Data |
| vl_total | DECIMAL(10,2) | | ValorTotal |
| co_usuario | INT | FK → tb_usuario | IdUsuario |
| co_situacao_pedido | INT | FK → tb_situacao_pedido | IdSituacaoPedido |

**tb_item_pedido** → classe `ItemPedido`

| Coluna | Tipo | Restrição | Propriedade |
| --- | --- | --- | --- |
| co_seq_item_pedido | INT | PK | Id |
| qt_item | INT | | Quantidade |
| vl_unitario | DECIMAL(10,2) | | ValorUnitario |
| co_pedido | INT | FK → tb_pedido, UQ¹ | IdPedido |
| co_jogo | INT | FK → tb_jogo, UQ¹ | IdJogo |

¹ `UNIQUE` composto (`co_pedido`, `co_jogo`): um jogo aparece uma vez por pedido.

## Relacionamentos

| Relação | Tipo |
| --- | --- |
| tb_plataforma → tb_jogo | 1:N |
| tb_condicao_jogo → tb_jogo | 1:N |
| tb_jogo ↔ tb_genero (por tb_jogo_genero) | N:N |
| tb_perfil → tb_usuario | 1:N |
| tb_usuario → tb_pedido | 1:N |
| tb_situacao_pedido → tb_pedido | 1:N |
| tb_pedido → tb_item_pedido | 1:N |
| tb_jogo → tb_item_pedido | 1:N |
