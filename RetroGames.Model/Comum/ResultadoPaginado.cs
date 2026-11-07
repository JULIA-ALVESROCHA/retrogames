namespace RetroGames.Model.Comum;

/// <summary>Uma página de resultados e o total encontrado com o mesmo filtro.</summary>
public class ResultadoPaginado<T>
{
    /// <summary>Itens da página pedida.</summary>
    public List<T> Itens { get; set; } = [];

    /// <summary>Total de registros encontrados, sem paginação.</summary>
    public int Total { get; set; }
}
