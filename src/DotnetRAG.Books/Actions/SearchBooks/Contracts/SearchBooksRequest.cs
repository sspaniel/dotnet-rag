namespace DotnetRAG.Books.Actions.SearchBooks.Contracts;

public class SearchBooksRequest
{
    public string Query { get; set; } = string.Empty;

    public int Top { get; set; } = 3;
}