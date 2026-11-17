using System.Text.Json.Serialization;

namespace RetroGames.WebApi.ApiModels.Comum;

/// <summary>
/// Envelope de toda resposta da API: { "responseCode", "data", "errorMessage" }.
/// Esta versão, sem Data, serve para respostas de erro.
/// </summary>
public class RetroGamesResponseApiModel
{
    /// <summary>Código da resposta: ResponseCodes.SUCESSO, ResponseCodes.ERRO ou o código do erro de negócio.</summary>
    [JsonPropertyOrder(0)]
    public int ResponseCode { get; set; }

    /// <summary>Mensagem de erro; nula quando deu certo.</summary>
    [JsonPropertyOrder(2)]
    public string? ErrorMessage { get; set; }
}

/// <summary>Envelope com os dados da resposta em Data.</summary>
public class RetroGamesResponseApiModel<T> : RetroGamesResponseApiModel
{
    /// <summary>Os dados devolvidos.</summary>
    [JsonPropertyOrder(1)]
    public T? Data { get; set; }

    /// <summary>Envelope de sucesso: ResponseCode = SUCESSO e os dados em Data.</summary>
    public static RetroGamesResponseApiModel<T> Sucesso(T data)
    {
        return new RetroGamesResponseApiModel<T> { ResponseCode = ResponseCodes.SUCESSO, Data = data };
    }
}
