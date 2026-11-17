using RetroGames.Model.Comum;
using RetroGames.Services.ServiceModels;
using RetroGames.WebApi.ApiModels.Comum;
using RetroGames.WebApi.ApiModels.Jogo;

namespace RetroGames.WebApi.Mappers;

/// <summary>
/// Converte o request da listagem no filtro do Service e o
/// JogoDetalheServiceModel nos ApiModels de saída.
/// </summary>
public static class JogoMapper
{
    /// <summary>Query string da listagem → filtro do JogoService.</summary>
    public static FiltroJogo ParaFiltro(this ListarJogoRequestApiModel request)
    {
        return new FiltroJogo
        {
            Busca = string.IsNullOrWhiteSpace(request.Busca) ? null : request.Busca.Trim(),
            IdPlataforma = request.IdPlataforma,
            IdGenero = request.IdGenero,
            Pagina = request.Pagina,
            TamanhoPagina = request.TamanhoPagina,
        };
    }

    /// <summary>Um jogo na listagem.</summary>
    public static ListarJogoResponseApiModel ParaResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            NomePlataforma = detalhe.NomePlataforma,
            DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
            NomesGeneros = detalhe.Generos.Select(g => g.Nome).ToList(),
        };
    }

    /// <summary>O detalhe de um jogo (GET /api/Jogo/{id}).</summary>
    public static ListarPorIdJogoResponseApiModel ParaResponsePorId(this JogoDetalheServiceModel detalhe)
    {
        return new ListarPorIdJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            Descricao = detalhe.Jogo.Descricao,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            IdPlataforma = detalhe.Jogo.IdPlataforma,
            NomePlataforma = detalhe.NomePlataforma,
            IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
            DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
            Generos = detalhe.Generos.Select(g => g.ParaResponse()).ToList(),
        };
    }

    /// <summary>A página do Service → página da API, com os totais.</summary>
    public static PaginaResponseApiModel<ListarJogoResponseApiModel> ParaResponse(
        this ResultadoPaginado<JogoDetalheServiceModel> resultado, FiltroJogo filtro)
    {
        return new PaginaResponseApiModel<ListarJogoResponseApiModel>
        {
            Itens = resultado.Itens.Select(d => d.ParaResponse()).ToList(),
            Pagina = filtro.Pagina,
            TamanhoPagina = filtro.TamanhoPagina,
            TotalItens = resultado.Total,
            TotalPaginas = filtro.TamanhoPagina > 0
                ? (int)Math.Ceiling(resultado.Total / (double)filtro.TamanhoPagina)
                : 0,
        };
    }
}
