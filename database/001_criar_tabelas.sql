-- =============================================================================
-- RetroGames · 001_criar_tabelas.sql
-- Cria o banco e as tabelas. O banco guarda só a estrutura:
-- PK, FK (comportamento padrão), tipos, NOT NULL e UNIQUE.
-- Sem trigger, function, CHECK, DEFAULT ou ações referenciais (ON DELETE).
-- Script já executado não é alterado: mudanças entram em um novo script.
-- =============================================================================

SET NAMES utf8mb4;

CREATE DATABASE IF NOT EXISTS retrogames
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_0900_ai_ci;

USE retrogames;

-- -----------------------------------------------------------------------------
-- Catálogo e Administração
-- -----------------------------------------------------------------------------

-- Tabela de tipo: condição de conservação do jogo
CREATE TABLE tb_condicao_jogo (
    co_condicao_jogo INT         NOT NULL,
    ds_condicao_jogo VARCHAR(50) NOT NULL,
    CONSTRAINT pk_condicao_jogo PRIMARY KEY (co_condicao_jogo),
    CONSTRAINT uq_condicao_jogo_ds UNIQUE (ds_condicao_jogo)
);

-- Tabela de tipo: perfis de acesso
CREATE TABLE tb_perfil (
    co_perfil INT         NOT NULL,
    ds_perfil VARCHAR(50) NOT NULL,
    CONSTRAINT pk_perfil PRIMARY KEY (co_perfil),
    CONSTRAINT uq_perfil_ds UNIQUE (ds_perfil)
);

-- Cadastro: console ou plataforma do jogo
CREATE TABLE tb_plataforma (
    co_seq_plataforma INT         NOT NULL AUTO_INCREMENT,
    no_plataforma     VARCHAR(80) NOT NULL,
    no_fabricante     VARCHAR(80) NOT NULL,
    nu_ano_lancamento INT         NOT NULL,
    CONSTRAINT pk_plataforma PRIMARY KEY (co_seq_plataforma),
    CONSTRAINT uq_plataforma_no UNIQUE (no_plataforma)
);

-- Cadastro (somente leitura no sistema): gênero do jogo
CREATE TABLE tb_genero (
    co_seq_genero INT         NOT NULL AUTO_INCREMENT,
    no_genero     VARCHAR(50) NOT NULL,
    CONSTRAINT pk_genero PRIMARY KEY (co_seq_genero),
    CONSTRAINT uq_genero_no UNIQUE (no_genero)
);

-- Principal: jogo à venda
CREATE TABLE tb_jogo (
    co_seq_jogo       INT            NOT NULL AUTO_INCREMENT,
    no_jogo           VARCHAR(150)   NOT NULL,
    ds_jogo           VARCHAR(1000)  NULL,
    nu_ano_lancamento INT            NOT NULL,
    vl_preco          DECIMAL(10,2)  NOT NULL,
    qt_estoque        INT            NOT NULL,
    st_ativo          BOOLEAN        NOT NULL,
    dt_cadastro       DATETIME       NOT NULL,
    co_plataforma     INT            NOT NULL,
    co_condicao_jogo  INT            NOT NULL,
    CONSTRAINT pk_jogo PRIMARY KEY (co_seq_jogo),
    CONSTRAINT fk_jogo_plataforma FOREIGN KEY (co_plataforma)
        REFERENCES tb_plataforma (co_seq_plataforma),
    CONSTRAINT fk_jogo_condicao_jogo FOREIGN KEY (co_condicao_jogo)
        REFERENCES tb_condicao_jogo (co_condicao_jogo)
);

-- Índice para a busca por nome do catálogo
CREATE INDEX ix_jogo_no ON tb_jogo (no_jogo);

-- Associativa N:N entre jogo e gênero
CREATE TABLE tb_jogo_genero (
    co_jogo   INT NOT NULL,
    co_genero INT NOT NULL,
    CONSTRAINT pk_jogo_genero PRIMARY KEY (co_jogo, co_genero),
    CONSTRAINT fk_jogo_genero_jogo FOREIGN KEY (co_jogo)
        REFERENCES tb_jogo (co_seq_jogo),
    CONSTRAINT fk_jogo_genero_genero FOREIGN KEY (co_genero)
        REFERENCES tb_genero (co_seq_genero)
);

-- Cadastro: usuário do sistema (senha guardada só como hash)
CREATE TABLE tb_usuario (
    co_seq_usuario INT          NOT NULL AUTO_INCREMENT,
    no_usuario     VARCHAR(100) NOT NULL,
    ds_email       VARCHAR(256) NOT NULL,
    ds_senha_hash  VARCHAR(500) NOT NULL,
    st_ativo       BOOLEAN      NOT NULL,
    dt_cadastro    DATETIME     NOT NULL,
    co_perfil      INT          NOT NULL,
    CONSTRAINT pk_usuario PRIMARY KEY (co_seq_usuario),
    CONSTRAINT uq_usuario_email UNIQUE (ds_email),
    CONSTRAINT fk_usuario_perfil FOREIGN KEY (co_perfil)
        REFERENCES tb_perfil (co_perfil)
);

-- -----------------------------------------------------------------------------
-- Compras (tarefa)
-- -----------------------------------------------------------------------------

-- Tabela de tipo: situação do pedido
CREATE TABLE tb_situacao_pedido (
    co_situacao_pedido INT         NOT NULL,
    ds_situacao_pedido VARCHAR(50) NOT NULL,
    CONSTRAINT pk_situacao_pedido PRIMARY KEY (co_situacao_pedido),
    CONSTRAINT uq_situacao_pedido_ds UNIQUE (ds_situacao_pedido)
);

-- Pedido de um usuário
CREATE TABLE tb_pedido (
    co_seq_pedido      INT           NOT NULL AUTO_INCREMENT,
    dt_pedido          DATETIME      NOT NULL,
    vl_total           DECIMAL(10,2) NOT NULL,
    co_usuario         INT           NOT NULL,
    co_situacao_pedido INT           NOT NULL,
    CONSTRAINT pk_pedido PRIMARY KEY (co_seq_pedido),
    CONSTRAINT fk_pedido_usuario FOREIGN KEY (co_usuario)
        REFERENCES tb_usuario (co_seq_usuario),
    CONSTRAINT fk_pedido_situacao_pedido FOREIGN KEY (co_situacao_pedido)
        REFERENCES tb_situacao_pedido (co_situacao_pedido)
);

-- Item de um pedido (um jogo aparece no máximo uma vez por pedido)
CREATE TABLE tb_item_pedido (
    co_seq_item_pedido INT           NOT NULL AUTO_INCREMENT,
    qt_item            INT           NOT NULL,
    vl_unitario        DECIMAL(10,2) NOT NULL,
    co_pedido          INT           NOT NULL,
    co_jogo            INT           NOT NULL,
    CONSTRAINT pk_item_pedido PRIMARY KEY (co_seq_item_pedido),
    CONSTRAINT uq_item_pedido_pedido_jogo UNIQUE (co_pedido, co_jogo),
    CONSTRAINT fk_item_pedido_pedido FOREIGN KEY (co_pedido)
        REFERENCES tb_pedido (co_seq_pedido),
    CONSTRAINT fk_item_pedido_jogo FOREIGN KEY (co_jogo)
        REFERENCES tb_jogo (co_seq_jogo)
);
