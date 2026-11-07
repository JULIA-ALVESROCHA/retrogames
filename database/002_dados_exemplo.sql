-- =============================================================================
-- RetroGames · 002_dados_exemplo.sql
-- Dados das tabelas de tipo e dados de exemplo do catálogo.
-- Execute depois de 001_criar_tabelas.sql.
-- O administrador inicial NÃO é inserido aqui: a API o cria a partir do .env
-- (AdminInicial__Email e AdminInicial__Senha), com a senha em hash.
-- =============================================================================

USE retrogames;

-- Garante que acentos sejam gravados em UTF-8, qualquer que seja o cliente.
SET NAMES utf8mb4;

-- Tabelas de tipo -------------------------------------------------------------

INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
    (1, 'Administrador'),
    (2, 'Cliente');

INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
    (1, 'Lacrado'),
    (2, 'Completo na caixa'),
    (3, 'Somente cartucho ou disco'),
    (4, 'Com marcas de uso');

INSERT INTO tb_situacao_pedido (co_situacao_pedido, ds_situacao_pedido) VALUES
    (1, 'Aguardando pagamento'),
    (2, 'Pago'),
    (3, 'Enviado'),
    (4, 'Entregue'),
    (5, 'Cancelado');

-- Plataformas -----------------------------------------------------------------

INSERT INTO tb_plataforma (co_seq_plataforma, no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
    (1, 'Atari 2600',        'Atari',   1977),
    (2, 'NES',               'Nintendo', 1983),
    (3, 'Master System',     'Sega',    1985),
    (4, 'Super Nintendo',    'Nintendo', 1990),
    (5, 'Mega Drive',        'Sega',    1988),
    (6, 'PlayStation',       'Sony',    1994),
    (7, 'Nintendo 64',       'Nintendo', 1996),
    (8, 'Game Boy',          'Nintendo', 1989);

-- Gêneros ---------------------------------------------------------------------

INSERT INTO tb_genero (co_seq_genero, no_genero) VALUES
    (1, 'Ação'),
    (2, 'Aventura'),
    (3, 'Plataforma'),
    (4, 'RPG'),
    (5, 'Corrida'),
    (6, 'Luta'),
    (7, 'Esporte'),
    (8, 'Puzzle'),
    (9, 'Tiro');

-- Jogos -----------------------------------------------------------------------
-- O jogo 15 está inativo (st_ativo = FALSE) para testar o filtro de inativos.
-- A plataforma 8 (Game Boy) não tem jogos: serve para testar a exclusão.

INSERT INTO tb_jogo (co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) VALUES
    (1,  'Pitfall!',                'Aventura na selva com Pitfall Harry.',                1982,  89.90, 3, TRUE,  '2026-11-01 12:00:00', 1, 3),
    (2,  'River Raid',              'Avião sobre o rio, desviando e atirando.',            1982,  79.90, 2, TRUE,  '2026-11-01 12:00:00', 1, 4),
    (3,  'Super Mario Bros.',       'O clássico de plataforma do encanador.',              1985, 199.90, 4, TRUE,  '2026-11-01 12:00:00', 2, 2),
    (4,  'The Legend of Zelda',     'Exploração e masmorras em Hyrule.',                   1986, 249.90, 1, TRUE,  '2026-11-01 12:00:00', 2, 3),
    (5,  'Alex Kidd in Miracle World', NULL,                                               1986, 149.90, 2, TRUE,  '2026-11-01 12:00:00', 3, 3),
    (6,  'Super Mario World',       'Mario e Yoshi na Dinosaur Land.',                     1990, 279.90, 5, TRUE,  '2026-11-01 12:00:00', 4, 2),
    (7,  'Chrono Trigger',          'RPG com viagens no tempo.',                           1995, 599.90, 1, TRUE,  '2026-11-01 12:00:00', 4, 1),
    (8,  'Street Fighter II',       'Luta um contra um.',                                  1992, 189.90, 0, TRUE,  '2026-11-01 12:00:00', 4, 4),
    (9,  'Sonic the Hedgehog',      'Velocidade em Green Hill Zone.',                      1991, 159.90, 6, TRUE,  '2026-11-01 12:00:00', 5, 2),
    (10, 'Streets of Rage 2',       'Briga de rua em dupla.',                              1992, 219.90, 2, TRUE,  '2026-11-01 12:00:00', 5, 3),
    (11, 'Final Fantasy VII',       'RPG épico em três discos.',                           1997, 299.90, 3, TRUE,  '2026-11-01 12:00:00', 6, 2),
    (12, 'Gran Turismo',            'Simulador de corrida.',                               1997,  99.90, 4, TRUE,  '2026-11-01 12:00:00', 6, 3),
    (13, 'Super Mario 64',          'Mario em três dimensões.',                            1996, 349.90, 2, TRUE,  '2026-11-01 12:00:00', 7, 2),
    (14, 'GoldenEye 007',           'Tiro em primeira pessoa.',                            1997, 229.90, 1, TRUE,  '2026-11-01 12:00:00', 7, 3),
    (15, 'Mario Kart 64',           'Corrida com itens.',                                  1996, 259.90, 0, FALSE, '2026-11-01 12:00:00', 7, 4);

-- Vínculos jogo x gênero ------------------------------------------------------

INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
    (1, 2), (1, 3),
    (2, 1), (2, 9),
    (3, 3),
    (4, 1), (4, 2),
    (5, 3),
    (6, 3),
    (7, 4),
    (8, 6),
    (9, 3),
    (10, 1), (10, 6),
    (11, 4),
    (12, 5), (12, 7),
    (13, 3), (13, 2),
    (14, 9), (14, 1),
    (15, 5);
