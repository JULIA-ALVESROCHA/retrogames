using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Interfaces;
using RetroGames.WebApi.ApiModels.Comum;
using RetroGames.WebApi.ApiModels.Plataforma;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

/// <summary>
/// Rotas de plataformas (/api/Plataforma). Sem regra de negócio, SQL nem try/catch:
/// chama o Service, converte em ApiModel e devolve dentro do envelope.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PlataformaController : ControllerBase
{
    private readonly IPlataformaService plataformaService;

    public PlataformaController(IPlataformaService plataformaService)
    {
        this.plataformaService = plataformaService;
    }

    /// <summary>GET /api/Plataforma: lista as plataformas.</summary>
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>>> Listar()
    {
        var plataformas = await plataformaService.ListarAsync();
        var resposta = plataformas.Select(p => p.ParaResponse()).ToList();

        return Ok(RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>.Sucesso(resposta));
    }

    /// <summary>GET /api/Plataforma/{id}: consulta uma plataforma.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> ListarPorId(int id)
    {
        var plataforma = await plataformaService.BuscarPorIdAsync(id);

        return Ok(RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>.Sucesso(plataforma.ParaResponse()));
    }
}
