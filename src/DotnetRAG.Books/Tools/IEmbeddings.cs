namespace DotnetRAG.Books.Tools;

public interface IEmbeddings
{
    Task<float[]> GenerateAsync(string value, CancellationToken cancellationToken);
}