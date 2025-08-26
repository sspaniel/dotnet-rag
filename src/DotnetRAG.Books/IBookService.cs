using DotnetRAG.Books.Actions.AddBook.Contracts;
using DotnetRAG.Books.Actions.EmbedBook.Contracts;
using DotnetRAG.Books.Actions.SearchBooks.Contracts;

namespace DotnetRAG.Books;

public interface IBookService
{
    Task<AddBookResponse> AddBookAsync(AddBookRequest request, CancellationToken cancellationToken);

    Task<EmbedBookResponse> EmbedBookAsync(EmbedBookRequest request, CancellationToken cancellationToken);

    Task<SearchBooksResponse> SearchBooksAsync(SearchBooksRequest request, CancellationToken cancellationToken);
}