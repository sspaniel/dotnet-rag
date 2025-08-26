using DotnetRAG.Books.Actions.SearchBooks.Contracts;
using DotnetRAG.Books.Tools;

namespace DotnetRAG.Books.Actions.SearchBooks.Orchestration;

public class SearchBookOrchestrator(IBookRepository bookRepository, IEmbeddings embeddings) : ISearchBookOrchestrator
{
    public async Task<SearchBooksResponse> ProcessAsync(SearchBooksRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Query, nameof(request.Query));
        ArgumentOutOfRangeException.ThrowIfLessThan(request.Top, 1, nameof(request.Top));

        var queryEmbedding = await embeddings.GenerateAsync(request.Query, cancellationToken);
        var (scores, books) = await bookRepository.SearchAsync(queryEmbedding, request.Top, cancellationToken);

        var responseItems = books
            .Select((book, index) => new SearchBooksResponseItem { Book = book, Score = scores[index] })
            .ToArray();

        var response = new SearchBooksResponse
        {
            Query = request.Query,
            Books = responseItems
        };

        return response;
    }
}