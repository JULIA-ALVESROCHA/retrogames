using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Interfaces;
using RetroGames.WebApi.ApiModels.Comum;
using RetroGames.WebApi.ApiModels.Genero;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

/// <summary>Rotas de gêneros (/api/Genero). Só leitura: lista usada nos filtros e formulários.</summary>
[ApiController]
[Route("api/[controller]")]
public class GeneroController : ControllerBase
{
    private readonly IGeneroService generoService;

    public GeneroController(IGeneroService generoService)
    {
        this.generoService = generoService;
    }

    /// <summary>GET /api/Genero: lista os gêneros.</summary>
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarGeneroResponseApiModel>>>> Listar()
    {
        var generos = await generoService.ListarAsync();
        var resposta = generos.Select(g => g.ParaResponse()).ToList();

        return Ok(RetroGamesResponseApiModel<List<ListarGeneroResponseApiModel>>.Sucesso(resposta));
    }
}
