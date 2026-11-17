using RetroGames.Model.Model;

namespace RetroGames.Services.Interfaces;

/// <summary>Regras de negócio das plataformas (RN01, RN02).</summary>
public interface IPlataformaService
{
    /// <summary>Todas as plataformas, como o Repository entrega.</summary>
    Task<List<Plataforma>> ListarAsync();

    /// <summary>A plataforma do id. Se não existir, lança PLATAFORMA_NAO_ENCONTRADA.</summary>
    Task<Plataforma> BuscarPorIdAsync(int id);

    /// <summary>Grava uma nova plataforma e a devolve com o id gerado. Nome repetido: PLATAFORMA_NOME_DUPLICADO.</summary>
    Task<Plataforma> InserirAsync(Plataforma plataforma);

    /// <summary>Altera a plataforma. Manter o próprio nome não é duplicidade.</summary>
    Task AlterarAsync(Plataforma plataforma);

    /// <summary>Exclui a plataforma. Se ela tiver jogos, lança PLATAFORMA_POSSUI_JOGOS.</summary>
    Task ExcluirAsync(int id);
}
