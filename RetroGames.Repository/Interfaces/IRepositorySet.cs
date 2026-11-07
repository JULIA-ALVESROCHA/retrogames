namespace RetroGames.Repository.Interfaces;

/// <summary>
/// Reúne todos os repositórios num só objeto. O Service recebe um IRepositorySet
/// e usa repositorio.Plataforma, repositorio.Jogo, repositorio.JogoGenero,
/// repositorio.Genero e repositorio.CondicaoJogo.
/// </summary>
public interface IRepositorySet
{
    IPlataformaRepository Plataforma { get; }
    IJogoRepository Jogo { get; }
    IJogoGeneroRepository JogoGenero { get; }
    IGeneroRepository Genero { get; }
    ICondicaoJogoRepository CondicaoJogo { get; }
}
