namespace FGV.OrdenacaoLivros.Exceptions;

/// <summary>
/// Essa excetpion será lançada quando Books for nulo
/// </summary>
public class BooksException : Exception
{
    public BooksException() : base("O conjunto de livros é inválido ou nulo.") { }
    public BooksException(string message) : base(message) { }
}
