namespace FGV.OrdenacaoLivros.Exceptions;

/// <summary>
/// Essa excetpion será lançada quando a OrderingRule passada for nula
/// </summary>
public class OrdenacaoException : Exception
{
    public OrdenacaoException() : base("As regras de ordenação são inválidas ou nulas.") { }
    public OrdenacaoException(string message) : base(message) { }
}
