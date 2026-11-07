using RetroGames.Model.Model;

namespace RetroGames.Repository.Interfaces;

/// <summary>Todo o SQL da tabela tb_genero (somente leitura).</summary>
public interface IGeneroRepository
{
    /// <summary>Os gêneros, ordenados por nome.</summary>
    Task<List<Genero>> ListarAsync();
}
