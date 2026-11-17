using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services.Interfaces;

namespace RetroGames.Services.Servicos;

/// <summary>
/// Gêneros não têm regra de negócio, mas ganham um Service porque
/// os controllers falam só com Services, nunca direto com o Repository.
/// </summary>
public class GeneroService : IGeneroService
{
    private readonly IRepositorySet repositorio;

    public GeneroService(IRepositorySet repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<List<Genero>> ListarAsync()
    {
        return await repositorio.Genero.ListarAsync();
    }
}
