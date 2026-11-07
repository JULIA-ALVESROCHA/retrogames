namespace RetroGames.Model.Model;

/// <summary>Tabela tb_situacao_pedido (tabela de tipo).</summary>
public class SituacaoPedido
{
    /// <summary>co_situacao_pedido</summary>
    public int Id { get; set; }

    /// <summary>ds_situacao_pedido</summary>
    public string Descricao { get; set; } = string.Empty;
}
