using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services.Interfaces;

namespace RetroGames.Services.Servicos;

/// <summary>
/// Condições de jogo não têm regra de negócio, mas ganham um Service porque
/// os controllers falam só com Services, nunca direto com o Repository.
/// </summary>
public class CondicaoJogoService : ICondicaoJogoService
{
    private readonly IRepositorySet repositorio;

    public CondicaoJogoService(IRepositorySet repositorio)
    {
        this.repositorio = repositorio;
    }

    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        return await repositorio.CondicaoJogo.ListarAsync();
    }
}
