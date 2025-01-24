using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Azure.OpenAI.Client.Chat.Abstract;
using Soenneker.Azure.OpenAI.Client.Registrars;

// ReSharper disable InconsistentNaming

namespace Soenneker.Azure.OpenAI.Client.Chat.Registrars;

/// <summary>
/// An async thread-safe singleton for the Azure OpenAI Chat (completions) client
/// </summary>
public static class AzureOpenAIChatClientRegistrar
{
    /// <summary>
    /// Adds <see cref="IAzureOpenAIChatClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddAzureOpenAIChatClientAsSingleton(this IServiceCollection services)
    {
        services.AddAzureOpenAIClientUtilAsSingleton();
        services.TryAddSingleton<IAzureOpenAIChatClient, AzureOpenAIChatClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IAzureOpenAIChatClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddAzureOpenAIChatClientAsScoped(this IServiceCollection services)
    {
        services.AddAzureOpenAIClientUtilAsScoped();
        services.TryAddScoped<IAzureOpenAIChatClient, AzureOpenAIChatClient>();

        return services;
    }
}
