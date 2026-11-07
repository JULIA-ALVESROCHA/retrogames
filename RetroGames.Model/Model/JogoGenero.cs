namespace RetroGames.Model.Model;

/// <summary>Tabela tb_jogo_genero (associativa N:N entre jogo e gênero).</summary>
public class JogoGenero
{
    /// <summary>co_jogo</summary>
    public int IdJogo { get; set; }

    /// <summary>co_genero</summary>
    public int IdGenero { get; set; }
}
