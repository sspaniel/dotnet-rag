using DotnetRAG.Books;
using DotnetRAG.Books.Actions.AddBook.Contracts;
using DotnetRAG.Books.Actions.SearchBooks.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRAG.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpPost]
    public async Task<AddBookResponse> AddBook([FromBody] AddBookRequest request, CancellationToken cancellationToken)
    {
        var response = await bookService.AddBookAsync(request, cancellationToken);
        return response;
    }

    [HttpPost("search")]
    public async Task<SearchBooksResponse> SearchBooks([FromBody] SearchBooksRequest request, CancellationToken cancellationToken)
    {
        var response = await bookService.SearchBooksAsync(request, cancellationToken);
        return response;
    }
}