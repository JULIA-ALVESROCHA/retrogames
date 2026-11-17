namespace RetroGames.WebApi.ApiModels.Comum;

/// <summary>
/// Catálogo dos códigos da própria API (Espec. 5.3). Sequenciais: SUCESSO = 0, ERRO = 1
/// e, a partir de 2, tuplas (Code, DefaultMessage). Código novo recebe o próximo número livre.
/// Os erros de negócio (100 em diante) ficam no BusinessErrors, no RetroGames.Services.
/// </summary>
public static class ResponseCodes
{
    /// <summary>A requisição deu certo.</summary>
    public const int SUCESSO = 0;

    /// <summary>Erro inesperado (500). Os detalhes vão só para o log.</summary>
    public const int ERRO = 1;
}
