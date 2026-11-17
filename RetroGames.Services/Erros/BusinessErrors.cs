using System.Net;

namespace RetroGames.Services.Erros;

/// <summary>
/// Um erro de negócio do catálogo: código (a partir de 100), mensagem e status HTTP.
/// O status é só um valor: quem transforma o erro em resposta HTTP é a API (aula 4).
/// </summary>
public record BusinessError(int Code, string Message, HttpStatusCode Status);

/// <summary>
/// Catálogo dos erros de negócio (Espec. 5.3). Cada erro é definido uma vez só, aqui.
/// Uso: <c>throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);</c>
/// </summary>
public static class BusinessErrors
{
    /// <summary>Plataforma pedida ou do jogo não existe (RN05).</summary>
    public static readonly BusinessError PLATAFORMA_NAO_ENCONTRADA =
        new(100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    /// <summary>Nome de plataforma já usado (RN01).</summary>
    public static readonly BusinessError PLATAFORMA_NOME_DUPLICADO =
        new(101, "Já existe uma plataforma com esse nome.", HttpStatusCode.Conflict);

    /// <summary>Plataforma com jogos não pode ser excluída (RN02).</summary>
    public static readonly BusinessError PLATAFORMA_POSSUI_JOGOS =
        new(102, "A plataforma possui jogos e não pode ser excluída.", HttpStatusCode.Conflict);

    /// <summary>O id do jogo não existe.</summary>
    public static readonly BusinessError JOGO_NAO_ENCONTRADO =
        new(103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    /// <summary>Condição do jogo não existe (RN05).</summary>
    public static readonly BusinessError CONDICAO_NAO_ENCONTRADA =
        new(104, "Condição de jogo não encontrada.", HttpStatusCode.NotFound);

    /// <summary>Gênero do jogo não existe (RN05).</summary>
    public static readonly BusinessError GENERO_NAO_ENCONTRADO =
        new(105, "Gênero não encontrado.", HttpStatusCode.NotFound);
}
