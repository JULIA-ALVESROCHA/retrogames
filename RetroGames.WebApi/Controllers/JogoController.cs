using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Interfaces;
using RetroGames.WebApi.ApiModels.Comum;
using RetroGames.WebApi.ApiModels.Jogo;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

/// <summary>
/// Rotas do catálogo de jogos (/api/Jogo). Sem regra de negócio, SQL nem try/catch:
/// o JogoService monta o catálogo e o GlobalExceptionHandler trata os erros.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class JogoController : ControllerBase
{
    private readonly IJogoService jogoService;

    public JogoController(IJogoService jogoService)
    {
        this.jogoService = jogoService;
    }

    /// <summary>
    /// GET /api/Jogo?busca=zelda&amp;idPlataforma=2&amp;idGenero=4&amp;pagina=1&amp;tamanhoPagina=12:
    /// uma página do catálogo, só com jogos ativos.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>>> Listar(
        [FromQuery] ListarJogoRequestApiModel request)
    {
        var filtro = request.ParaFiltro();
        var resultado = await jogoService.ListarAsync(filtro);

        return Ok(RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>
            .Sucesso(resultado.ParaResponse(filtro)));
    }

    /// <summary>GET /api/Jogo/{id}: o detalhe de um jogo.</summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var detalhe = await jogoService.BuscarPorIdAsync(id);

        return Ok(RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>.Sucesso(detalhe.ParaResponsePorId()));
    }
}
