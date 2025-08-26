using DotnetRAG.Books.Models;
using DotnetRAG.Books.Tools;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.VectorData;
using System.Text.Json;

namespace DotnetRAG.Tools.Redis;

public class RedisBookRepository(VectorStore vectorStore, IDistributedCache cache) : IBookRepository
{
    public async Task EnsureSchemaCreatedAsync(CancellationToken cancellationToken)
    {
        var bookDescriptionEmbeddings = vectorStore.GetCollection<string, BookEmbedding>(nameof(BookEmbedding));
        await bookDescriptionEmbeddings.EnsureCollectionExistsAsync(cancellationToken);
    }

    public async Task AddBookAsync(Book book, CancellationToken cancellationToken)
    {
        var bookJson = JsonSerializer.Serialize(book);
        await cache.SetStringAsync($"books:{book.BookId}", bookJson, cancellationToken);
    }

    public async Task AddBookEmbeddingAsync(BookEmbedding bookEmbedding, CancellationToken cancellationToken)
    {
        var bookEmbeddings = vectorStore.GetCollection<string, BookEmbedding>(nameof(BookEmbedding));
        await bookEmbeddings.UpsertAsync(bookEmbedding, cancellationToken);
    }

    public async Task<Book?> GetBookAsync(string bookId, CancellationToken cancellationToken)
    {
        var bookJson = await cache.GetStringAsync($"books:{bookId}", cancellationToken);

        if (bookJson == null)
        {
            return null;
        }

        var book = JsonSerializer.Deserialize<Book>(bookJson);
        return book;
    }

    public async Task<(double[], Book[])> SearchAsync(float[] queryVector, int top, CancellationToken cancellationToken)
    {
        var searchOptions = new VectorSearchOptions<BookEmbedding>
        {
            VectorProperty = b => b.Vector,
            IncludeVectors = false
        };

        var bookDescriptionEmbeddings = vectorStore.GetCollection<string, BookEmbedding>(nameof(BookEmbedding));
        var searchResult = bookDescriptionEmbeddings.SearchAsync(queryVector, top: top, searchOptions, cancellationToken);

        var scores = new List<double>();
        var books = new List<Book>();

        await foreach (var result in searchResult)
        {
            var book = await GetBookAsync(result.Record.BookId, cancellationToken);

            if (book is null || !result.Score.HasValue)
            {
                continue;
            }

            books.Add(book);
            scores.Add(result.Score.Value);
        }

        return (scores.ToArray(), books.ToArray());
    }
}