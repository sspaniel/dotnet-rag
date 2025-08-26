using DotnetRAG.Books.Actions.AddBook.Contracts;

namespace DotnetRAG.Books.Actions.AddBook.Orchestration;

public interface IAddBookOrchestrator
{
    Task<AddBookResponse> ProcessAsync(AddBookRequest request, CancellationToken cancellationToken);
}