namespace RetroGames.WebApi.ApiModels.Genero;

/// <summary>Um gênero no JSON: id e nome.</summary>
public class ListarGeneroResponseApiModel
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;
}
