namespace RetroGames.Model.Comum;

/// <summary>
/// Filtro e paginação da listagem de jogos. Cada filtro só vale quando foi informado (não nulo).
/// </summary>
public class FiltroJogo
{
    /// <summary>Parte do nome do jogo (no_jogo LIKE %busca%).</summary>
    public string? Busca { get; set; }

    /// <summary>Id da plataforma (co_plataforma).</summary>
    public int? IdPlataforma { get; set; }

    /// <summary>
    /// Ids dos jogos permitidos. O Service usa para o filtro por gênero:
    /// resolve os ids com LINQ e o Repository filtra e pagina no SQL.
    /// Lista vazia significa "nenhum jogo".
    /// </summary>
    public List<int>? IdsJogos { get; set; }

    /// <summary>Se true, lista também os jogos inativos (somente Administrador).</summary>
    public bool IncluirInativos { get; set; }

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; } = 1;

    /// <summary>Quantidade de jogos por página.</summary>
    public int TamanhoPagina { get; set; } = 12;
}
