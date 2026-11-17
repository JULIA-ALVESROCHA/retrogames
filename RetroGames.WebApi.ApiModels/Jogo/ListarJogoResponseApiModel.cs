namespace RetroGames.WebApi.ApiModels.Jogo;

/// <summary>Um jogo na listagem do catálogo.</summary>
public class ListarJogoResponseApiModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public int AnoLancamento { get; set; }

    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    public bool Ativo { get; set; }

    /// <summary>Nome da plataforma (vem de tb_plataforma).</summary>
    public string NomePlataforma { get; set; } = string.Empty;

    /// <summary>Descrição da condição (vem de tb_condicao_jogo).</summary>
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;

    /// <summary>Nomes dos gêneros do jogo (vêm de tb_genero).</summary>
    public List<string> NomesGeneros { get; set; } = [];
}
