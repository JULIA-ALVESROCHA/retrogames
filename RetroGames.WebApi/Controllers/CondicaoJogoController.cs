using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Interfaces;
using RetroGames.WebApi.ApiModels.Comum;
using RetroGames.WebApi.ApiModels.CondicaoJogo;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

/// <summary>Rotas de condições de jogo (/api/CondicaoJogo). Só leitura: lista usada nos formulários.</summary>
[ApiController]
[Route("api/[controller]")]
public class CondicaoJogoController : ControllerBase
{
    private readonly ICondicaoJogoService condicaoJogoService;

    public CondicaoJogoController(ICondicaoJogoService condicaoJogoService)
    {
        this.condicaoJogoService = condicaoJogoService;
    }

    /// <summary>GET /api/CondicaoJogo: lista as condições de jogo.</summary>
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarCondicaoJogoResponseApiModel>>>> Listar()
    {
        var condicoes = await condicaoJogoService.ListarAsync();
        var resposta = condicoes.Select(c => c.ParaResponse()).ToList();

        return Ok(RetroGamesResponseApiModel<List<ListarCondicaoJogoResponseApiModel>>.Sucesso(resposta));
    }
}
