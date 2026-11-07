using RetroGames.Model.Comum;
using RetroGames.Model.Model;

namespace RetroGames.Repository.Interfaces;

/// <summary>Todo o SQL da tabela tb_jogo.</summary>
public interface IJogoRepository
{
    /// <summary>
    /// Os jogos de uma página e o total encontrado. Filtro opcional por parte do nome,
    /// plataforma e lista de ids; inativos só quando pedido. Paginação e contagem no SQL.
    /// </summary>
    Task<ResultadoPaginado<Jogo>> ListarAsync(FiltroJogo filtro);

    /// <summary>O jogo com o id informado (ativo ou não), ou null se não existir.</summary>
    Task<Jogo?> BuscarPorIdAsync(int id);

    /// <summary>Grava um novo jogo e devolve o id gerado pelo banco.</summary>
    Task<int> InserirAsync(Jogo jogo);

    /// <summary>Atualiza os dados do jogo (não altera a data de cadastro).</summary>
    Task AlterarAsync(Jogo jogo);

    /// <summary>Marca o jogo como inativo (st_ativo = 0). Jogo nunca é apagado.</summary>
    Task DesativarAsync(int id);
}
