using System;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace Soenneker.Azure.OpenAI.Client.Chat.Abstract;

/// <summary>
/// An async thread-safe singleton for the Azure OpenAI Chat (completions) client
/// </summary>
// ReSharper disable once InconsistentNaming
/// <summary>
/// Defines the azure open ai chat client contract.
/// </summary>
public interface IAzureOpenAIChatClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Not required, but can be used to set the deployment and options for the client
    /// </summary>
    /// <param name="deployment"></param>
    void SetOptions(string deployment);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task containing the result of the operation.</returns>
    ValueTask<ChatClient> Get(CancellationToken cancellationToken = default);
}