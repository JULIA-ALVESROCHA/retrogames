using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Base;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository.Repositorios;

/// <summary>Todo o SQL da tabela tb_jogo_genero.</summary>
public class JogoGeneroRepository : RepositoryBase, IJogoGeneroRepository
{
    public JogoGeneroRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_jogo, co_genero FROM tb_jogo_genero ORDER BY co_jogo, co_genero");
        await using var leitor = await comando.ExecuteReaderAsync();

        return await LerVinculosAsync(leitor);
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(IEnumerable<int> idsJogos)
    {
        var ids = idsJogos.ToList();
        if (ids.Count == 0)
        {
            // "IN ()" é SQL inválido; sem jogos, não há vínculos.
            return [];
        }

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_jogo, co_genero FROM tb_jogo_genero " +
            "WHERE co_jogo IN (" + NomesParametros("@idJogo", ids.Count) + ") " +
            "ORDER BY co_jogo, co_genero");
        for (var i = 0; i < ids.Count; i++)
        {
            AdicionarParametro(comando, "@idJogo" + i, ids[i]);
        }
        await using var leitor = await comando.ExecuteReaderAsync();

        return await LerVinculosAsync(leitor);
    }

    public async Task InserirAsync(int idJogo, IEnumerable<int> idsGeneros)
    {
        var generos = idsGeneros.ToList();
        if (generos.Count == 0)
        {
            return;
        }

        // Um único INSERT com várias linhas: (@idJogo, @idGenero0), (@idJogo, @idGenero1)...
        var linhas = string.Join(", ", Enumerable.Range(0, generos.Count).Select(i => "(@idJogo, @idGenero" + i + ")"));

        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES " + linhas);
        AdicionarParametro(comando, "@idJogo", idJogo);
        for (var i = 0; i < generos.Count; i++)
        {
            AdicionarParametro(comando, "@idGenero" + i, generos[i]);
        }

        // A PK composta (co_jogo, co_genero) recusa gênero repetido: vira RegistroDuplicadoException.
        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo");
        AdicionarParametro(comando, "@idJogo", idJogo);

        await comando.ExecuteNonQueryAsync();
    }

    private static async Task<List<JogoGenero>> LerVinculosAsync(MySqlDataReader leitor)
    {
        var vinculos = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            vinculos.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero"),
            });
        }
        return vinculos;
    }

    /// <summary>Gera "@idJogo0, @idJogo1": só nomes de parâmetro, nunca valores.</summary>
    private static string NomesParametros(string prefixo, int quantidade)
    {
        return string.Join(", ", Enumerable.Range(0, quantidade).Select(i => prefixo + i));
    }
}
