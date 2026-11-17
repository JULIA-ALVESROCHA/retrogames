using RetroGames.Model.Model;

namespace RetroGames.Services.Interfaces;

/// <summary>Acesso à lista de condições de jogo (somente leitura).</summary>
public interface ICondicaoJogoService
{
    /// <summary>As condições de jogo, como o Repository entrega.</summary>
    Task<List<CondicaoJogo>> ListarAsync();
}
