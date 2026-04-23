using FGV.OrdenacaoLivros.Models;

namespace FGV.OrdenacaoLivros.Interfaces;

public interface IBooksOrderer
{
    IEnumerable<Book> Order(IEnumerable<Book> books, IEnumerable<OrderingRule> orderingRules);
}
