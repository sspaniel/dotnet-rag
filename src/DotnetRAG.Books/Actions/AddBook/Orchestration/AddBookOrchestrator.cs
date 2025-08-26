using DotnetRAG.Books.Actions.AddBook.Contracts;
using DotnetRAG.Books.Models;
using DotnetRAG.Books.Tools;
using NanoWorks.Messaging.MessagePublishers;

namespace DotnetRAG.Books.Actions.AddBook.Orchestration;

public class AddBookOrchestrator(IBookRepository bookRepository, IMessagePublisher messagePublisher) : IAddBookOrchestrator
{
    public async Task<AddBookResponse> ProcessAsync(AddBookRequest request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Title);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Author);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.Description);

        var book = new Book
        {
            BookId = Guid.NewGuid().ToString(),
            Title = request.Title,
            Author = request.Author,
            Description = request.Description
        };

        await bookRepository.AddBookAsync(book, cancellationToken);

        var @event = new BookAddedEvent
        {
            BookId = book.BookId,
        };

        await messagePublisher.BroadcastAsync(@event, cancellationToken);

        var response = new AddBookResponse
        {
            BookId = book.BookId
        };

        return response;
    }
}