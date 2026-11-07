using RetroGames.Model.Model;

namespace RetroGames.Repository.Interfaces;

/// <summary>Todo o SQL da tabela tb_condicao_jogo (somente leitura).</summary>
public interface ICondicaoJogoRepository
{
    /// <summary>As condições de jogo, na ordem do código.</summary>
    Task<List<CondicaoJogo>> ListarAsync();
}
