using RetroGames.Model.Comum;
using RetroGames.Model.Model;
using RetroGames.Services.ServiceModels;

namespace RetroGames.Services.Interfaces;

/// <summary>Regras de negócio dos jogos (RN03, RN05) e montagem do catálogo.</summary>
public interface IJogoService
{
    /// <summary>
    /// Uma página do catálogo, com o detalhe de cada jogo e o total encontrado.
    /// O filtro por gênero é resolvido com LINQ; a paginação continua no SQL.
    /// </summary>
    Task<ResultadoPaginado<JogoDetalheServiceModel>> ListarAsync(FiltroJogo filtro);

    /// <summary>O detalhe do jogo. Se não existir, lança JOGO_NAO_ENCONTRADO.</summary>
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);

    /// <summary>Grava o jogo e os seus gêneros numa transação e devolve o detalhe do jogo criado.</summary>
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, List<int> idsGeneros);

    /// <summary>Numa transação, grava o jogo, remove os vínculos antigos e grava os novos.</summary>
    Task AlterarAsync(Jogo jogo, List<int> idsGeneros);

    /// <summary>Marca o jogo como inativo. Jogo nunca é apagado (RN03).</summary>
    Task DesativarAsync(int id);
}
