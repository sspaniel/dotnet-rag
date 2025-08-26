using DotnetRAG.Books.Actions.EmbedBook.Contracts;

namespace DotnetRAG.Books.Actions.EmbedBook.Orchestration;

public interface IEmbedBookOrchestrator
{
    Task<EmbedBookResponse> ProcessAsync(EmbedBookRequest request, CancellationToken cancellationToken);
}