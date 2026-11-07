using RetroGames.Model.Model;
using RetroGames.Repository.Base;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository.Repositorios;

/// <summary>Todo o SQL da tabela tb_genero.</summary>
public class GeneroRepository : RepositoryBase, IGeneroRepository
{
    public GeneroRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<List<Genero>> ListarAsync()
    {
        await using var conexao = await CriarConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero");
        await using var leitor = await comando.ExecuteReaderAsync();

        var generos = new List<Genero>();
        while (await leitor.ReadAsync())
        {
            generos.Add(new Genero
            {
                Id = leitor.GetInt32("co_seq_genero"),
                Nome = leitor.GetString("no_genero"),
            });
        }
        return generos;
    }
}
