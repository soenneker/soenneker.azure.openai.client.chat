using System;
using System.Threading;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace Soenneker.Azure.OpenAI.Client.Chat.Abstract;

/// <summary>
/// An async thread-safe singleton for the Azure OpenAI Chat (completions) client
/// </summary>
// ReSharper disable once InconsistentNaming
public interface IAzureOpenAIChatClient : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Not required, but can be used to set the deployment and options for the client
    /// </summary>
    /// <param name="deployment"></param>
    void SetOptions(string deployment);

    ValueTask<ChatClient> Get(CancellationToken cancellationToken = default);
}