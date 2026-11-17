using RetroGames.Repository.Base;
using RetroGames.Repository.Interfaces;
using RetroGames.Repository.Repositorios;
using RetroGames.Services.Interfaces;
using RetroGames.Services.Servicos;

namespace RetroGames.WebApi.Configuration;

/// <summary>
/// Registro dos repositórios e dos Services na injeção de dependência.
/// O Program.cs chama os dois métodos; quem precisar recebe tudo pelo construtor.
/// </summary>
public static class InjecaoDependenciaConfig
{
    /// <summary>
    /// RepositoryConfig uma única vez (singleton), com a connection string do .env.
    /// Cada Repository e o RepositorySet, um por requisição (scoped).
    /// </summary>
    public static IServiceCollection AddRepositorios(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString });

        services.AddScoped<IPlataformaRepository, PlataformaRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IJogoGeneroRepository, JogoGeneroRepository>();
        services.AddScoped<IGeneroRepository, GeneroRepository>();
        services.AddScoped<ICondicaoJogoRepository, CondicaoJogoRepository>();
        services.AddScoped<IRepositorySet, RepositorySet>();

        return services;
    }

    /// <summary>Cada Service com a sua interface, um por requisição (scoped).</summary>
    public static IServiceCollection AddServicosDeNegocio(this IServiceCollection services)
    {
        services.AddScoped<IPlataformaService, PlataformaService>();
        services.AddScoped<IJogoService, JogoService>();
        services.AddScoped<IGeneroService, GeneroService>();
        services.AddScoped<ICondicaoJogoService, CondicaoJogoService>();

        return services;
    }
}
