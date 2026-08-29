using System;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace Soenneker.Azure.OpenAI.Client.Chat.Abstract;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Creates and caches an Azure OpenAI chat client for a configured deployment.
/// </summary>
public interface IAzureOpenAIChatClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Overrides the configured deployment before the client is first created.
    /// </summary>
    /// <param name="deployment">Azure OpenAI deployment name.</param>
    /// <exception cref="ArgumentException"><paramref name="deployment"/> is blank.</exception>
    /// <exception cref="InvalidOperationException">The chat client has already been created.</exception>
    void SetOptions(string deployment);

    /// <summary>
    /// Returns the configured chat Client used by the azure openai chat client.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task whose result is the requested chat Client.</returns>
    ValueTask<ChatClient> Get(CancellationToken cancellationToken = default);
}
