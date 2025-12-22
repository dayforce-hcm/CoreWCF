using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Elements.Bindings.WebHttp.Tests
{
    public class WebContentTypeMapperTests
    {
        private class TestMapper : WebContentTypeMapper
        {
            public override WebContentFormat GetMessageFormatForContentType(string contentType)
            {
                if (contentType == "application/json") return WebContentFormat.Json;
                if (contentType == "application/xml") return WebContentFormat.Xml;
                return WebContentFormat.Default;
            }
        }

        [Theory]
        [InlineData("application/json", WebContentFormat.Json)]
        [InlineData("application/xml", WebContentFormat.Xml)]
        [InlineData("other", WebContentFormat.Default)]
        public void GetMessageFormatForContentType_ReturnsExpected(string contentType, WebContentFormat expected)
        {
            var mapper = new TestMapper();
            var result = mapper.GetMessageFormatForContentType(contentType);
            Assert.Equal(expected, result);
        }
    }
}
