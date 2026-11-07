namespace RetroGames.Repository.Base;

/// <summary>
/// Lançada quando o MySQL recusa um valor repetido em coluna UNIQUE ou chave primária (erro 1062).
/// O Repository não decide o que isso significa: o Service converte esta exceção
/// no erro de negócio correspondente (exemplo: PlataformaNomeDuplicado).
/// </summary>
public class RegistroDuplicadoException : Exception
{
    public RegistroDuplicadoException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
