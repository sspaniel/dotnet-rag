namespace DotnetRAG.Books.Actions.AddBook.Contracts;

public class AddBookRequest
{
    public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}