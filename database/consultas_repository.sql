-- =============================================================================
-- RetroGames · consultas_repository.sql
-- O SQL de cada método do Repository, para testar no Workbench com valores reais.
-- No código, as variáveis @x viram parâmetros do comando (AdicionarParametro).
-- Os comandos de gravação ficam dentro de uma transação desfeita no fim (ROLLBACK).
-- =============================================================================

USE retrogames;

-- PlataformaRepository --------------------------------------------------------

-- Listar
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma ORDER BY no_plataforma;

-- Buscar por id
SET @id = 4;
SELECT co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento
FROM tb_plataforma WHERE co_seq_plataforma = @id;

-- Existe com nome (troque @idIgnorar por 4 para simular a alteração da própria plataforma)
SET @nome = 'Super Nintendo', @idIgnorar = NULL;
SELECT COUNT(*) FROM tb_plataforma
WHERE no_plataforma = @nome AND (@idIgnorar IS NULL OR co_seq_plataforma <> @idIgnorar);

-- Possui jogos (4 tem jogos; 8 não tem)
SET @id = 8;
SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id;

-- Inserir, alterar e excluir
START TRANSACTION;
SET @nome = 'Neo Geo', @fabricante = 'SNK', @anoLancamento = 1990;
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento)
VALUES (@nome, @fabricante, @anoLancamento);
SET @id = LAST_INSERT_ID();
UPDATE tb_plataforma
SET no_plataforma = 'Neo Geo AES', no_fabricante = @fabricante, nu_ano_lancamento = 1991
WHERE co_seq_plataforma = @id;
DELETE FROM tb_plataforma WHERE co_seq_plataforma = @id;
ROLLBACK;

-- JogoRepository --------------------------------------------------------------

-- Listar: mesmo WHERE na contagem e na página. Teste sem filtro, com cada filtro e na página 2.
SET @busca = NULL, @idPlataforma = NULL, @incluirInativos = FALSE;
SET @tamanhoPagina = 5, @pagina = 2;
SET @deslocamento = (@pagina - 1) * @tamanhoPagina;

SELECT COUNT(*) FROM tb_jogo
WHERE (@busca IS NULL OR no_jogo LIKE CONCAT('%', @busca, '%'))
  AND (@idPlataforma IS NULL OR co_plataforma = @idPlataforma)
  AND (@incluirInativos OR st_ativo = TRUE);

-- LIMIT/OFFSET não aceitam variável de sessão no Workbench: use os números (no código são parâmetros)
SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
       st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo
WHERE (@busca IS NULL OR no_jogo LIKE CONCAT('%', @busca, '%'))
  AND (@idPlataforma IS NULL OR co_plataforma = @idPlataforma)
  AND (@incluirInativos OR st_ativo = TRUE)
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 5;

-- Filtro por lista de ids (vem do filtro por gênero, resolvido no Service)
SELECT co_seq_jogo, no_jogo FROM tb_jogo
WHERE st_ativo = TRUE AND co_seq_jogo IN (3, 6, 9, 13)
ORDER BY no_jogo, co_seq_jogo;

-- Buscar por id
SET @id = 5;
SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
       st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo
FROM tb_jogo WHERE co_seq_jogo = @id;

-- Inserir, alterar e desativar (jogo nunca é apagado)
START TRANSACTION;
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
VALUES ('Donkey Kong Country', NULL, 1994, 199.90, 2, TRUE, UTC_TIMESTAMP(), 4, 2);
SET @id = LAST_INSERT_ID();
UPDATE tb_jogo SET no_jogo = 'Donkey Kong Country 2', ds_jogo = 'Diddy e Dixie',
       nu_ano_lancamento = 1995, vl_preco = 189.90, qt_estoque = 1, st_ativo = TRUE,
       co_plataforma = 4, co_condicao_jogo = 2
WHERE co_seq_jogo = @id;
UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id;

-- JogoGeneroRepository (ainda dentro da transação)
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES (@id, 3), (@id, 2);
SELECT co_jogo, co_genero FROM tb_jogo_genero ORDER BY co_jogo, co_genero;
SELECT co_jogo, co_genero FROM tb_jogo_genero WHERE co_jogo IN (1, @id) ORDER BY co_jogo, co_genero;
DELETE FROM tb_jogo_genero WHERE co_jogo = @id;
ROLLBACK;

-- GeneroRepository ------------------------------------------------------------
SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero;

-- CondicaoJogoRepository ------------------------------------------------------
SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY co_condicao_jogo;
