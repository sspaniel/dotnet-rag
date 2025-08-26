using DotnetRAG.Books.Models;

namespace DotnetRAG.Books.Actions.SearchBooks.Contracts;

public class SearchBooksResponseItem
{
    public double Score { get; set; }

    public required Book Book { get; set; }
}