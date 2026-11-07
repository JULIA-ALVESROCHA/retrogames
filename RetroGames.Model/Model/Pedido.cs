namespace RetroGames.Model.Model;

/// <summary>Tabela tb_pedido.</summary>
public class Pedido
{
    /// <summary>co_seq_pedido</summary>
    public int Id { get; set; }

    /// <summary>dt_pedido (UTC)</summary>
    public DateTime Data { get; set; }

    /// <summary>vl_total</summary>
    public decimal ValorTotal { get; set; }

    /// <summary>co_usuario</summary>
    public int IdUsuario { get; set; }

    /// <summary>co_situacao_pedido</summary>
    public int IdSituacaoPedido { get; set; }
}
