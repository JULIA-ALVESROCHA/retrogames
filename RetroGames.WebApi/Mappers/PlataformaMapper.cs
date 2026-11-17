using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels.Plataforma;

namespace RetroGames.WebApi.Mappers;

/// <summary>Converte a classe Plataforma do modelo no ApiModel que sai no JSON.</summary>
public static class PlataformaMapper
{
    public static ListarPlataformaResponseApiModel ParaResponse(this Plataforma plataforma)
    {
        return new ListarPlataformaResponseApiModel
        {
            Id = plataforma.Id,
            Nome = plataforma.Nome,
            Fabricante = plataforma.Fabricante,
            AnoLancamento = plataforma.AnoLancamento,
        };
    }
}
