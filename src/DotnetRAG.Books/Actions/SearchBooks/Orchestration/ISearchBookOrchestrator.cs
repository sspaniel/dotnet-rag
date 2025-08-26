using DotnetRAG.Books.Actions.SearchBooks.Contracts;

namespace DotnetRAG.Books.Actions.SearchBooks.Orchestration;

public interface ISearchBookOrchestrator
{
    Task<SearchBooksResponse> ProcessAsync(SearchBooksRequest request, CancellationToken cancellationToken);
}