using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services.Erros;
using RetroGames.WebApi.ApiModels.Comum;

namespace RetroGames.WebApi.Middlewares;

/// <summary>
/// Único ponto que transforma exceções em resposta HTTP, em vez de um try/catch em cada action.
/// BusinessRuleException: status e código do erro do catálogo, com a mensagem dele.
/// Qualquer outra: 500 com ResponseCodes.ERRO e mensagem genérica; os detalhes vão só para o log.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private const string MensagemErroInesperado =
        "Ocorreu um erro inesperado. Tente novamente mais tarde.";

    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        RetroGamesResponseApiModel resposta;

        if (exception is BusinessRuleException erroNegocio)
        {
            httpContext.Response.StatusCode = (int)erroNegocio.Erro.Status;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = erroNegocio.Erro.Code,
                ErrorMessage = erroNegocio.Message,
            };
        }
        else
        {
            // O erro completo (SQL, tabela, pilha) fica só no log do servidor, nunca na resposta.
            logger.LogError(exception, "Erro inesperado em {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERRO,
                ErrorMessage = MensagemErroInesperado,
            };
        }

        await httpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);
        return true;
    }
}
