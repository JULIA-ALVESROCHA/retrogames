namespace RetroGames.WebApi.ApiModels.Jogo;

/// <summary>
/// Parâmetros da listagem de jogos, lidos da query string:
/// /api/Jogo?busca=zelda&amp;idPlataforma=2&amp;idGenero=4&amp;pagina=1&amp;tamanhoPagina=12
/// </summary>
public class ListarJogoRequestApiModel
{
    /// <summary>Parte do nome do jogo.</summary>
    public string? Busca { get; set; }

    public int? IdPlataforma { get; set; }

    public int? IdGenero { get; set; }

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; } = 1;

    /// <summary>Quantidade de jogos por página.</summary>
    public int TamanhoPagina { get; set; } = 12;
}
