using FGV.OrdenacaoLivros.Exceptions;
using FGV.OrdenacaoLivros.Interfaces;
using FGV.OrdenacaoLivros.Models;
using FGV.OrdenacaoLivros.Models.Enums;
using FGV.OrdenacaoLivros.Services;

namespace FGV.OrdenacaoLivros.Tests;

public class BooksOrdererTests
{
    private readonly IBooksOrderer _orderer = new BooksOrderer();

    /// <summary>
    /// Conjunto fixo de livros conforme especificado no caso de teste.
    /// </summary>
    private static readonly IEnumerable<Book> _books =
    [
        new Book("Java How to Program", "Deitel & Deitel", 2007), // Livro 1
        new Book("Patterns of Enterprise Application Architecture", "Martin Fowler", 2002), // Livro 2
        new Book("Head First Design Patterns", "Elisabeth Freeman", 2004),  // Livro 3
        new Book("Internet & World Wide Web: How to Program", "Deitel & Deitel", 2007), // Livro 4
    ];

    #region Casos do documento

    //Executar por configuração sem mexer no código (através do appsettings.json)
    [Fact]
    public void Order_ComConfiguracaoDoArquivoJson_DeveRetornarOrdemCorreta()
    {
        var rules = OrderingRuleConfig.Load();
        var result = _orderer.Order(_books, rules).ToList();

        Assert.NotEmpty(result);
        Assert.Equal(4, result.Count);
    }

    /// <summary>
    /// Título ascendente → esperado: Livros 3, 4, 1, 2
    /// </summary>
    [Fact]
    public void Order_TituloAscendente_DeveRetornarOrdemCorreta()
    {
        List<OrderingRule> rules = [ new(AttributeEnum.Title, ascending: true) ];

        var result = _orderer.Order(_books, rules).ToList();

        Assert.Equal("Head First Design Patterns", result[0].Title);                        // Livro 3
        Assert.Equal("Internet & World Wide Web: How to Program", result[1].Title);         // Livro 4
        Assert.Equal("Java How to Program", result[2].Title);                               // Livro 1
        Assert.Equal("Patterns of Enterprise Application Architecture", result[3].Title);   // Livro 2
    }

    /// <summary>
    /// Autor ascendente + Título descendente → esperado: Livros 1, 4, 3, 2
    /// </summary>
    [Fact]
    public void Order_AutorAscendenteTituloDescendente_DeveRetornarOrdemCorreta()
    {
        List<OrderingRule> rules =
        [
            new(AttributeEnum.AuthorName, ascending: true),
            new(AttributeEnum.Title, ascending: false)
        ];

        var result = _orderer.Order(_books, rules).ToList();

        Assert.Equal("Java How to Program", result[0].Title);                               // Livro 1
        Assert.Equal("Internet & World Wide Web: How to Program", result[1].Title);         // Livro 4
        Assert.Equal("Head First Design Patterns", result[2].Title);                        // Livro 3
        Assert.Equal("Patterns of Enterprise Application Architecture", result[3].Title);   // Livro 2
    }

    /// <summary>
    /// Edição descendente + Autor descendente + Título ascendente → esperado: Livros 4, 1, 3, 2
    /// </summary>
    [Fact]
    public void Order_EdicaoDescendenteAutorDescendenteTituloAscendente_DeveRetornarOrdemCorreta()
    {
        List<OrderingRule> rules =
        [
            new(AttributeEnum.EditionYear, ascending: false),
            new(AttributeEnum.AuthorName, ascending: false),
            new(AttributeEnum.Title, ascending: true)
        ];

        var result = _orderer.Order(_books, rules).ToList();

        Assert.Equal("Internet & World Wide Web: How to Program", result[0].Title);         // Livro 4
        Assert.Equal("Java How to Program", result[1].Title);                               // Livro 1
        Assert.Equal("Head First Design Patterns", result[2].Title);                        // Livro 3
        Assert.Equal("Patterns of Enterprise Application Architecture", result[3].Title);   // Livro 2
    }

    /// <summary>
    /// Regras nulas → deve lançar OrdenacaoException
    /// </summary>
    [Fact]
    public void Order_RegrasNulas_DeveLancarOrdenacaoException()
    {
        Assert.Throws<OrdenacaoException>(() => _orderer.Order(_books, null!));
    }

    /// <summary>
    /// Conjunto vazio de livros → deve retornar conjunto vazio
    /// </summary>
    [Fact]
    public void Order_RegrasVaziss_DeveRetornarVazio()
    {
        Assert.Empty(_orderer.Order(_books, []));
    }

    #endregion

    #region Casos extras de validação

    /// <summary>
    /// Livros nulos → deve lançar BooksException
    /// </summary>
    [Fact]
    public void Order_LivrosNulos_DeveLancarBooksException()
    {
        List<OrderingRule> rules =
        [
            new(AttributeEnum.Title, ascending: true)
        ];

        Assert.Throws<BooksException>(() =>
            _orderer.Order(null!, rules));
    }

    /// <summary>
    /// Livros vazios → deve retornar conjunto vazio
    /// </summary>
    [Fact]
    public void Order_LivrosVazios_DeveRetornarVazio()
    {
        List<OrderingRule> rules =
        [
            new(AttributeEnum.Title, ascending: true)
        ];

        Assert.Empty(_orderer.Order([], rules));
    }

    /// <summary>
    /// Lista de regras contendo elementos nulos → deve lançar OrdenacaoException
    /// </summary>
    [Fact]
    public void Order_RegrasComNulos_DeveLancarOrdenacaoException()
    {
        List<OrderingRule> rules =
        [
            null, null, null
        ];

        Assert.Throws<OrdenacaoException>(() =>
            _orderer.Order(_books, rules));
    }

    #endregion
}
