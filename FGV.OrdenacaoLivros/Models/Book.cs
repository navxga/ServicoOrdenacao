namespace FGV.OrdenacaoLivros.Models;

public class Book
{
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public int EditionYear { get; set; }

    public Book(string title, string authorName, int editionYear)
    {
        Title = title;
        AuthorName = authorName;
        EditionYear = editionYear;
    }
}
