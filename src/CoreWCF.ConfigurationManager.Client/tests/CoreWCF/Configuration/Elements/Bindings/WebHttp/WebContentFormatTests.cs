using Xunit;
using WebContentFormat = CoreWCF.Channels.WebContentFormat;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebContentFormatTests
    {
        [Theory]
        [InlineData(WebContentFormat.Default)]
        [InlineData(WebContentFormat.Xml)]
        [InlineData(WebContentFormat.Json)]
        [InlineData(WebContentFormat.Raw)]
        public void WebContentFormat_Values_AreDefined(WebContentFormat format)
        {
            Assert.True(System.Enum.IsDefined(typeof(WebContentFormat), format));
        }
    }
}
