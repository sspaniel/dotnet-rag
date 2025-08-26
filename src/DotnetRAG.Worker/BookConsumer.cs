using DotnetRAG.Books;
using DotnetRAG.Books.Actions.EmbedBook.Contracts;
using DotnetRAG.Books.Models;

namespace DotnetRAG.Worker;

public class BookConsumer(IBookService bookService)
{
    public async Task OnBookAddedAsync(BookAddedEvent @event, CancellationToken token)
    {
        var request = new EmbedBookRequest
        {
            BookId = @event.BookId,
        };

        await bookService.EmbedBookAsync(request, token);
    }
}
