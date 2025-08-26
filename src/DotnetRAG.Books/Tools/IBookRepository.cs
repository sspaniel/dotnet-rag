using DotnetRAG.Books.Models;

namespace DotnetRAG.Books.Tools;

public interface IBookRepository
{
    Task EnsureSchemaCreatedAsync(CancellationToken cancellationToken);

    Task AddBookAsync(Book book, CancellationToken cancellationToken);

    Task AddBookEmbeddingAsync(BookEmbedding bookEmbedding, CancellationToken cancellationToken);

    Task<Book?> GetBookAsync(string bookId, CancellationToken cancellationToken);

    Task<(double[], Book[])> SearchAsync(float[] queryVector, int top, CancellationToken cancellationToken);
}