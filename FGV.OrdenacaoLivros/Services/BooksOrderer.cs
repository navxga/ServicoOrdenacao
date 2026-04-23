using FGV.OrdenacaoLivros.Exceptions;
using FGV.OrdenacaoLivros.Interfaces;
using FGV.OrdenacaoLivros.Models;
using FGV.OrdenacaoLivros.Models.Enums;

namespace FGV.OrdenacaoLivros.Services;

public class BooksOrderer : IBooksOrderer
{
    public IEnumerable<Book> Order(IEnumerable<Book> books, IEnumerable<OrderingRule> orderingRules)
    {
        IOrderedEnumerable<Book>? ordered = null;

        #region Validações

        if (books is null)
            throw new BooksException();

        if (orderingRules is null)
            throw new OrdenacaoException();

        if (!books.Any() || !orderingRules.Any())
            return [];

        //Se passar uma lista de nulos
        if (orderingRules.Any(r => r is null)) 
            throw new OrdenacaoException();

        #endregion

        foreach (var rule in orderingRules)
        {
            Func<Book, object> selector = rule.Attribute switch
            {
                AttributeEnum.Title => b => b.Title,
                AttributeEnum.AuthorName => b => b.AuthorName,
                AttributeEnum.EditionYear => b => b.EditionYear,

                _ => throw new NotImplementedException(
                    $"Atributo {rule.Attribute} não está mapeado pra ser ordenado"
                )
            };

            if (ordered is null)
            {
                ordered = rule.Ascending
                    ? books.OrderBy(selector)
                    : books.OrderByDescending(selector);
            }
            else
            {
                ordered = rule.Ascending
                    ? ordered.ThenBy(selector)
                    : ordered.ThenByDescending(selector);
            }
        }

        // ordered nunca é nulo aqui: o retorno antecipado e as validações acima
        // garantem que o foreach só executa com orderingRules não-nulo e não-vazio,
        // portanto ao menos uma iteração ocorreu e ordered foi atribuído.
        return ordered!;
    }
}
