using MySqlConnector;

namespace RetroGames.Repository.Base;

/// <summary>
/// Base de todos os repositórios. Concentra o que todo Repository repete:
/// abrir conexão, criar comando, adicionar parâmetro e executar gravação.
/// Não tem SQL de nenhuma tabela.
/// </summary>
public abstract class RepositoryBase
{
    /// <summary>Código do MySQL para "Duplicate entry" (valor repetido em UNIQUE ou PK).</summary>
    private const int ErroRegistroDuplicado = 1062;

    private readonly RepositoryConfig config;

    protected RepositoryBase(RepositoryConfig config)
    {
        this.config = config;
    }

    /// <summary>
    /// Abre uma conexão com o banco. Quem chama fecha com <c>await using</c>.
    /// Se houver um TransactionScope ativo (aberto pelo Service), a conexão entra nele sozinha.
    /// </summary>
    protected async Task<MySqlConnection> CriarConexaoAsync()
    {
        var conexao = new MySqlConnection(config.ConnectionString);
        try
        {
            await conexao.OpenAsync();
            return conexao;
        }
        catch
        {
            // Se a abertura falhar, a conexão não chega a quem chamou: fecha aqui.
            await conexao.DisposeAsync();
            throw;
        }
    }

    /// <summary>Cria um comando com o texto SQL, ligado à conexão.</summary>
    protected static MySqlCommand CriarComando(MySqlConnection conexao, string sql)
    {
        return new MySqlCommand(sql, conexao);
    }

    /// <summary>
    /// Adiciona um parâmetro (@nome) ao comando. O valor nunca é concatenado no SQL:
    /// é isso que impede SQL injection. <c>null</c> vira NULL no banco.
    /// </summary>
    protected static void AdicionarParametro(MySqlCommand comando, string nome, object? valor)
    {
        comando.Parameters.AddWithValue(nome, valor ?? DBNull.Value);
    }

    /// <summary>
    /// Executa uma gravação (INSERT ou UPDATE) e devolve o número de linhas afetadas.
    /// É o único lugar que converte o erro 1062 do MySQL em <see cref="RegistroDuplicadoException"/>.
    /// </summary>
    protected static async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            return await comando.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.Number == ErroRegistroDuplicado)
        {
            throw new RegistroDuplicadoException("Registro duplicado: o valor já existe no banco.", ex);
        }
    }
}
