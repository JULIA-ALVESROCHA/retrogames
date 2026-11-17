using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels.Genero;

namespace RetroGames.WebApi.Mappers;

/// <summary>Converte a classe Genero do modelo no ApiModel que sai no JSON.</summary>
public static class GeneroMapper
{
    public static ListarGeneroResponseApiModel ParaResponse(this Genero genero)
    {
        return new ListarGeneroResponseApiModel
        {
            Id = genero.Id,
            Nome = genero.Nome,
        };
    }
}
