using Soenneker.Azure.OpenAI.Client.Chat.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Azure.OpenAI.Client.Chat.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class AzureOpenAIChatClientTests : HostedUnitTest
{
    private readonly IAzureOpenAIChatClient _util;

    public AzureOpenAIChatClientTests(Host host) : base(host)
    {
        _util = Resolve<IAzureOpenAIChatClient>(true);
    }

    [Test]
    public void Default()
    {

    }
}
