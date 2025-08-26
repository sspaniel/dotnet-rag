namespace DotnetRAG.Books.Actions.SearchBooks.Contracts;

public class SearchBooksResponse
{
    public string Query { get; set; } = string.Empty;

    public SearchBooksResponseItem[] Books { get; set; } = [];
}