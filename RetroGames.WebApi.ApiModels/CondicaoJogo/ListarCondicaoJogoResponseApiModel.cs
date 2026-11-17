namespace RetroGames.WebApi.ApiModels.CondicaoJogo;

/// <summary>Uma condição de jogo no JSON: id e descrição.</summary>
public class ListarCondicaoJogoResponseApiModel
{
    public int Id { get; set; }

    public string Descricao { get; set; } = string.Empty;
}
