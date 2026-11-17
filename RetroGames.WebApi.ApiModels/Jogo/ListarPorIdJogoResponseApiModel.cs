using RetroGames.WebApi.ApiModels.Genero;

namespace RetroGames.WebApi.ApiModels.Jogo;

/// <summary>
/// O detalhe do jogo: os dados da listagem, mais a descrição,
/// os ids de plataforma e condição e os gêneros com id e nome.
/// </summary>
public class ListarPorIdJogoResponseApiModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public int AnoLancamento { get; set; }

    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    public bool Ativo { get; set; }

    public int IdPlataforma { get; set; }

    /// <summary>Nome da plataforma (vem de tb_plataforma).</summary>
    public string NomePlataforma { get; set; } = string.Empty;

    public int IdCondicaoJogo { get; set; }

    /// <summary>Descrição da condição (vem de tb_condicao_jogo).</summary>
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;

    /// <summary>Gêneros do jogo, com id e nome.</summary>
    public List<ListarGeneroResponseApiModel> Generos { get; set; } = [];
}
