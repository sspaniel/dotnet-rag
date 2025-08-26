using DotnetRAG.Books.Actions.AddBook.Contracts;
using DotnetRAG.Books.Actions.AddBook.Orchestration;
using DotnetRAG.Books.Actions.EmbedBook.Contracts;
using DotnetRAG.Books.Actions.EmbedBook.Orchestration;
using DotnetRAG.Books.Actions.SearchBooks.Contracts;
using DotnetRAG.Books.Actions.SearchBooks.Orchestration;

namespace DotnetRAG.Books;

public class BookService(
    IAddBookOrchestrator addBookOrchestrator,
    IEmbedBookOrchestrator embedBookOrchestrator,
    ISearchBookOrchestrator searchBookOrchestrator) : IBookService
{
    public async Task<AddBookResponse> AddBookAsync(AddBookRequest request, CancellationToken cancellationToken)
    {
        var response = await addBookOrchestrator.ProcessAsync(request, cancellationToken);
        return response;
    }

    public async Task<EmbedBookResponse> EmbedBookAsync(EmbedBookRequest request, CancellationToken cancellationToken)
    {
        var response = await embedBookOrchestrator.ProcessAsync(request, cancellationToken);
        return response;
    }

    public async Task<SearchBooksResponse> SearchBooksAsync(SearchBooksRequest request, CancellationToken cancellationToken)
    {
        var response = await searchBookOrchestrator.ProcessAsync(request, cancellationToken);
        return response;
    }
}
