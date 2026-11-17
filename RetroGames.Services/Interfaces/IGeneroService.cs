using RetroGames.Model.Model;

namespace RetroGames.Services.Interfaces;

/// <summary>Acesso à lista de gêneros (somente leitura).</summary>
public interface IGeneroService
{
    /// <summary>Os gêneros, como o Repository entrega.</summary>
    Task<List<Genero>> ListarAsync();
}
