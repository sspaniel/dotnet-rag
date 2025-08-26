using DotnetRAG.Books.Actions.EmbedBook.Contracts;
using DotnetRAG.Books.Models;
using DotnetRAG.Books.Tools;

namespace DotnetRAG.Books.Actions.EmbedBook.Orchestration;

public class EmbedBookOrchestrator(IBookRepository bookRepository, IEmbeddings embeddings) : IEmbedBookOrchestrator
{
    public async Task<EmbedBookResponse> ProcessAsync(EmbedBookRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.BookId);

        var book = await bookRepository.GetBookAsync(request.BookId, cancellationToken);

        ArgumentNullException.ThrowIfNull(book, nameof(Book));

        var bookEmbedding = new BookEmbedding
        {
            BookEmbeddingId = Guid.NewGuid().ToString(),
            BookId = book.BookId,
            Description = book.Description,
            Vector = await embeddings.GenerateAsync(book.Description, cancellationToken)
        };

        await bookRepository.AddBookEmbeddingAsync(bookEmbedding, cancellationToken);

        var response = new EmbedBookResponse
        {
            BookEmbeddingId = bookEmbedding.BookEmbeddingId
        };

        return response;
    }
}