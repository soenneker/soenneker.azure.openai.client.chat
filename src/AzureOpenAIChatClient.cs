using System.Threading;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using Soenneker.Azure.OpenAI.Client.Abstract;
using Soenneker.Azure.OpenAI.Client.Chat.Abstract;
using Soenneker.Extensions.String;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

// ReSharper disable InconsistentNaming

namespace Soenneker.Azure.OpenAI.Client.Chat;

/// <inheritdoc cref="IAzureOpenAIChatClient"/>
public sealed class AzureOpenAIChatClient : IAzureOpenAIChatClient
{
    private readonly AsyncSingleton<ChatClient> _client;

    private string? _deployment;

    public AzureOpenAIChatClient(ILogger<ChatClient> logger, IConfiguration configuration, IAzureOpenAIClientUtil azureOpenAiClientUtil)
    {
        _client = new AsyncSingleton<ChatClient>(async (ct, _) =>
        {
            AzureOpenAIClient azureClient = await azureOpenAiClientUtil.Get(ct).NoSync();

            var deployment = configuration.GetValue<string?>("Azure:OpenAI:Chat:Deployment");

            if (!_deployment.IsNullOrEmpty())
                deployment = _deployment;

            deployment.ThrowIfNullOrWhiteSpace();

            logger.LogDebug("Creating Azure OpenAI Chat client with deployment ({deployment})...", deployment);

            return azureClient.GetChatClient(deployment);
        });
    }

    public void SetOptions(string deployment)
    {
        _deployment = deployment;
    }

    public ValueTask<ChatClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}