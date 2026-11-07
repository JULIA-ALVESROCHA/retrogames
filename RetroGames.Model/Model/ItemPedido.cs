namespace RetroGames.Model.Model;

/// <summary>Tabela tb_item_pedido.</summary>
public class ItemPedido
{
    /// <summary>co_seq_item_pedido</summary>
    public int Id { get; set; }

    /// <summary>qt_item</summary>
    public int Quantidade { get; set; }

    /// <summary>vl_unitario</summary>
    public decimal ValorUnitario { get; set; }

    /// <summary>co_pedido</summary>
    public int IdPedido { get; set; }

    /// <summary>co_jogo</summary>
    public int IdJogo { get; set; }
}
