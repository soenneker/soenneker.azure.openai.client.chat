using Soenneker.Azure.OpenAI.Client.Chat.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Azure.OpenAI.Client.Chat.Tests;

[Collection("Collection")]
public class AzureOpenAIChatClientTests : FixturedUnitTest
{
    private readonly IAzureOpenAIChatClient _util;

    public AzureOpenAIChatClientTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IAzureOpenAIChatClient>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
