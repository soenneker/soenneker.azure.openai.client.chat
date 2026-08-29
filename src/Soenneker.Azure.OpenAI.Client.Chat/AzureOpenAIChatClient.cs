using System;
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
    private readonly ILogger<ChatClient> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAzureOpenAIClientUtil _azureOpenAiClientUtil;
    private readonly object _optionsLock = new();

    private string? _deployment;
    private bool _clientCreated;

    public AzureOpenAIChatClient(ILogger<ChatClient> logger, IConfiguration configuration, IAzureOpenAIClientUtil azureOpenAiClientUtil)
    {
        _logger = logger;
        _configuration = configuration;
        _azureOpenAiClientUtil = azureOpenAiClientUtil;
        _client = new AsyncSingleton<ChatClient>(CreateClient);
    }

    private async ValueTask<ChatClient> CreateClient(CancellationToken ct)
    {
        AzureOpenAIClient azureClient = await _azureOpenAiClientUtil.Get(ct).NoSync();

        string? deployment = _configuration.GetValue<string?>("Azure:OpenAI:Chat:Deployment");

        lock (_optionsLock)
        {
            if (!_deployment.IsNullOrEmpty())
                deployment = _deployment;

            deployment.ThrowIfNullOrWhiteSpace();

            _logger.LogDebug("Creating Azure OpenAI Chat client with deployment ({deployment})...", deployment);

            ChatClient client = azureClient.GetChatClient(deployment);
            _clientCreated = true;
            return client;
        }
    }

    public void SetOptions(string deployment)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deployment);

        lock (_optionsLock)
        {
            if (_clientCreated)
                throw new InvalidOperationException("The deployment must be set before the Azure OpenAI chat client is created.");

            _deployment = deployment;
        }
    }

    public ValueTask<ChatClient> Get(CancellationToken cancellationToken = default)
    {
        return _client.Get(cancellationToken);
    }

    /// <summary>
    /// Releases resources used by the current instance.
    /// </summary>
    public void Dispose()
    {
        _client.Dispose();
    }

    /// <summary>
    /// Asynchronously releases resources used by the current instance.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public ValueTask DisposeAsync()
    {
        return _client.DisposeAsync();
    }
}
