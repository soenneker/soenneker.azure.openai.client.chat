[![](https://img.shields.io/nuget/v/soenneker.azure.openai.client.chat.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.azure.openai.client.chat/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.azure.openai.client.chat/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.azure.openai.client.chat/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.azure.openai.client.chat.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.azure.openai.client.chat/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.azure.openai.client.chat/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.azure.openai.client.chat/actions/workflows/codeql.yml)

# Soenneker.Azure.OpenAI.Client.Chat

Creates and caches an OpenAI SDK `ChatClient` for an Azure OpenAI deployment.

## Installation

```bash
dotnet add package Soenneker.Azure.OpenAI.Client.Chat
```

## Configuration and registration

```json
{
  "Azure": {
    "OpenAI": {
      "Uri": "https://your-resource.openai.azure.com",
      "ApiKey": "your-api-key",
      "Chat": {
        "Deployment": "chat-deployment-name"
      }
    }
  }
}
```

```csharp
using Soenneker.Azure.OpenAI.Client.Chat.Registrars;

builder.Services.AddAzureOpenAIChatClientAsSingleton();
```

The registrar includes the shared Azure OpenAI client. Keep the API key in a secret provider.

## Complete a chat request

```csharp
using OpenAI.Chat;
using Soenneker.Azure.OpenAI.Client.Chat.Abstract;

public sealed class ChatService(IAzureOpenAIChatClient chatClientUtil)
{
    public async Task<string> Complete(
        string prompt,
        CancellationToken cancellationToken)
    {
        ChatClient client = await chatClientUtil.Get(cancellationToken);

        ChatCompletion completion = await client.CompleteChatAsync(
            [new UserChatMessage(prompt)],
            options: null,
            cancellationToken: cancellationToken);

        return completion.Content[0].Text;
    }
}
```

Handle empty content and model finish reasons according to the requirements of the consuming application.

## Deployment and lifecycle

- `Azure:OpenAI:Chat:Deployment` is required unless `SetOptions(deployment)` is called before the first `Get()`.
- `SetOptions()` overrides configuration for that utility instance.
- Calling `SetOptions()` after client creation throws; it does not silently change the cached deployment.
- The chat client and underlying Azure client are cached. Replace the DI scope or singleton to switch deployments or credentials.
- Prompts and completions may contain sensitive data. Apply appropriate logging, retention, content-safety, and access controls in the consuming application.
