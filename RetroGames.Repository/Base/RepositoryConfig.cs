namespace RetroGames.Repository.Base;

/// <summary>
/// Configuração dos repositórios. Guarda só a connection string.
/// O valor chega na aula 4, quando a API ler o arquivo .env.
/// </summary>
public class RepositoryConfig
{
    /// <summary>Connection string do MySQL (ConnectionStrings__Default no .env).</summary>
    public string ConnectionString { get; set; } = string.Empty;
}
