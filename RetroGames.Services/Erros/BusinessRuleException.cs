namespace RetroGames.Services.Erros;

/// <summary>
/// Lançada pelo Service quando uma regra de negócio é violada.
/// Guarda o erro do catálogo (<see cref="BusinessErrors"/>); a API responde com o status
/// e o código dele (400, 404 ou 409) e usa a mensagem da exceção.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(BusinessError erro)
        : base(erro.Message)
    {
        Erro = erro;
    }

    /// <summary>O erro do catálogo que foi violado.</summary>
    public BusinessError Erro { get; }
}
