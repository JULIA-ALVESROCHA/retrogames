namespace RetroGames.WebApi.ApiModels.Comum;

/// <summary>Uma página de resultados, com os totais para montar a navegação.</summary>
public class PaginaResponseApiModel<T>
{
    /// <summary>Os itens da página pedida.</summary>
    public List<T> Itens { get; set; } = [];

    /// <summary>Número da página, começando em 1.</summary>
    public int Pagina { get; set; }

    /// <summary>Quantidade de itens por página.</summary>
    public int TamanhoPagina { get; set; }

    /// <summary>Total de itens encontrados com o mesmo filtro.</summary>
    public int TotalItens { get; set; }

    /// <summary>Total de páginas: TotalItens dividido por TamanhoPagina, arredondado para cima.</summary>
    public int TotalPaginas { get; set; }
}
