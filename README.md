
# dotnet-rag
A .NET RAG solution demonstrating agent-driven semantic search using Redis and Azure AI.


## Getting Started

For this example, I used the embed-v-4-0 and gpt-4.1-nano models. For instructions on deploying these models, see the [Azure documentation](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/create-model-deployments?pivots=ai-foundry-portal).

Once deployed, update the `model`, `endpoint` , and `apiKey` settings in the appsettings.json file.

To start Redis and RabbitMQ in Docker, run the following command:

`[root]\docker> docker-compose -p dotnet-rag up -d`

You can build and run the hosts directly from your IDE or by using the command line:

`[root]\src\DotnetRAG.WebAPI> dotnet run`

`[root]\src\DotnetRAG.Worker> dotnet run`

`[root]\src\DotnetRAG.Agent> dotnet run`

Once the hosts are running, add some books by navigating to `http://localhost:5555/swagger` and using the `/books` endpoint.

The embeddings will be generated and stored in Redis, based on each book's description.

After adding books, you can interact with the agent by switching to the Agent's console window.


## Additional Resources

You can view records in Redis by navigating to `http://localhost:5540` and connecting to the local database using `redis://default@redis:6379`.

To learn more about the technologies used in this project, see the following documentation:

- [Semantic Kernel](https://learn.microsoft.com/en-us/semantic-kernel/overview/)
- [Vector Store](https://learn.microsoft.com/en-us/semantic-kernel/concepts/vector-store-connectors/vector-search?pivots=programming-language-csharp)
- [Azure AI Models](https://learn.microsoft.com/en-us/azure/ai-foundry/foundry-models/how-to/create-model-deployments?pivots=ai-foundry-portal)
- [Redis Indexes](https://redis.io/docs/latest/develop/ai/search-and-query/vectors/#hnsw-index)
- [Cosine Similarity](https://redis.io/docs/latest/develop/ai/search-and-query/vectors/#distance-metrics)
- [NanoWorks Messaging Library](https://github.com/NanoWorks-Project/NanoWorks/tree/main/src/Messaging)


