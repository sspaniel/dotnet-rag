using Microsoft.Extensions.VectorData;

namespace DotnetRAG.Books.Models;

public class BookEmbedding
{
    [VectorStoreKey]
    public string BookEmbeddingId { get; set; } = string.Empty;

    [VectorStoreData]
    public string BookId { get; set; } = string.Empty;

    [VectorStoreData]
    public string Description { get; set; } = string.Empty;

    [VectorStoreVector(Dimensions: 1536,DistanceFunction = DistanceFunction.CosineSimilarity, IndexKind = IndexKind.Hnsw)]
    public float[] Vector { get; set; } = [];
}