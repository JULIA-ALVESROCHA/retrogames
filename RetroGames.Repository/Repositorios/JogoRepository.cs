using MySqlConnector;
using RetroGames.Model.Comum;
using RetroGames.Model.Model;
using RetroGames.Repository.Base;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository.Repositorios;

/// <summary>
/// Todo o SQL da tabela tb_jogo. Nenhuma consulta junta tabelas:
/// plataforma e gêneros são combinados pelo Service, com LINQ.
/// </summary>
public class JogoRepository : RepositoryBase, IJogoRepository
{
    public JogoRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<ResultadoPaginado<Jogo>> ListarAsync(FiltroJogo filtro)
    {
        // O mesmo WHERE serve à contagem e à página, para o total bater com o filtro.
        // Cada filtro só vale quando foi informado: "@x IS NULL OR ..." ignora o filtro nulo.
        var where =
            "WHERE (@busca IS NULL OR no_jogo LIKE CONCAT('%', @busca, '%')) " +
            "AND (@idPlataforma IS NULL OR co_plataforma = @idPlataforma) " +
            "AND (@incluirInativos OR st_ativo = TRUE)";

        // Lista de ids (filtro por gênero resolvido no Service): um parâmetro por id.
        // Só os nomes @id0, @id1... entram no texto do SQL; os valores vão como parâmetros.
        if (filtro.IdsJogos is not null)
        {
            where += filtro.IdsJogos.Count == 0
                ? " AND FALSE"
                : " AND co_seq_jogo IN (" + NomesParametros("@id", filtro.IdsJogos.Count) + ")";
        }

        await using var conexao = await CriarConexaoAsync();

        // 1. Total encontrado
        int total;
        await using (var comandoTotal = CriarComando(conexao, "SELECT COUNT(*) FROM tb_jogo " + where))
        {
            AdicionarParametrosFiltro(comandoTotal, filtro);
            total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());
        }

        // 2. Itens da página
        var jogos = new List<Jogo>();
        await using (var comandoPagina = CriarComando(conexao,
            "SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo " +
            "FROM tb_jogo " + where + " " +
            "ORDER BY no_jogo, co_seq_jogo " +
            "LIMIT @tamanhoPagina OFFSET @deslocamento"))
        {
            AdicionarParametrosFiltro(comandoPagina, filtro);
            AdicionarParametro(comandoPagina, "@tamanhoPagina", filtro.TamanhoPagina);
            AdicionarParametro(comandoPagina, "@deslocamento", (filtro.Pagina - 1) * filtro.TamanhoPagina);

            await using var leitor = await comandoPagina.ExecuteReaderAsync();
            while (await leitor.ReadAsync())
            {
                jogos.Add(Mapear(leitor));
            }
        }

        return new ResultadoPaginado<Jogo> { Itens = jogos, Total = total };
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo " +
            "FROM tb_jogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, " +
            "st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo) " +
            "VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque, " +
            "@ativo, @dataCadastro, @idPlataforma, @idCondicaoJogo)");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@dataCadastro", jogo.DataCadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET no_jogo = @nome, ds_jogo = @descricao, " +
            "nu_ano_lancamento = @anoLancamento, vl_preco = @preco, qt_estoque = @estoque, " +
            "st_ativo = @ativo, co_plataforma = @idPlataforma, co_condicao_jogo = @idCondicaoJogo " +
            "WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);
        AdicionarParametro(comando, "@id", jogo.Id);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    /// <summary>Adiciona os parâmetros do filtro, iguais na contagem e na página.</summary>
    private static void AdicionarParametrosFiltro(MySqlCommand comando, FiltroJogo filtro)
    {
        // Busca em branco vale como "sem busca".
        var busca = string.IsNullOrWhiteSpace(filtro.Busca) ? null : filtro.Busca.Trim();

        AdicionarParametro(comando, "@busca", busca);
        AdicionarParametro(comando, "@idPlataforma", filtro.IdPlataforma);
        AdicionarParametro(comando, "@incluirInativos", filtro.IncluirInativos);

        if (filtro.IdsJogos is not null)
        {
            for (var i = 0; i < filtro.IdsJogos.Count; i++)
            {
                AdicionarParametro(comando, "@id" + i, filtro.IdsJogos[i]);
            }
        }
    }

    /// <summary>Gera "@id0, @id1, @id2": só nomes de parâmetro, nunca valores.</summary>
    private static string NomesParametros(string prefixo, int quantidade)
    {
        return string.Join(", ", Enumerable.Range(0, quantidade).Select(i => prefixo + i));
    }

    /// <summary>Converte a linha atual do leitor em um Jogo, coluna por coluna.</summary>
    private static Jogo Mapear(MySqlDataReader leitor)
    {
        var ordinalDescricao = leitor.GetOrdinal("ds_jogo");

        return new Jogo
        {
            Id = leitor.GetInt32("co_seq_jogo"),
            Nome = leitor.GetString("no_jogo"),
            Descricao = leitor.IsDBNull(ordinalDescricao) ? null : leitor.GetString(ordinalDescricao),
            AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
            Preco = leitor.GetDecimal("vl_preco"),
            Estoque = leitor.GetInt32("qt_estoque"),
            Ativo = leitor.GetBoolean("st_ativo"),
            DataCadastro = leitor.GetDateTime("dt_cadastro"),
            IdPlataforma = leitor.GetInt32("co_plataforma"),
            IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo"),
        };
    }
}
