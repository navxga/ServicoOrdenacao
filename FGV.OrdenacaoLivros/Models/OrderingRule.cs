using FGV.OrdenacaoLivros.Models.Enums;

namespace FGV.OrdenacaoLivros.Models;

public class OrderingRule
{
    public AttributeEnum? Attribute { get; set; }
    public bool Ascending { get; set; }

    public OrderingRule(AttributeEnum? attribute, bool ascending)
    {
        Attribute = attribute;
        Ascending = ascending;
    }
}
