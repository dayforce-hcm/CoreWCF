using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class IDefaultCommunicationTimeoutsTests
    {
        private class TestTimeouts : IDefaultCommunicationTimeouts
        {
            public TimeSpan CloseTimeout { get; set; }
            public TimeSpan OpenTimeout { get; set; }
            public TimeSpan ReceiveTimeout { get; set; }
            public TimeSpan SendTimeout { get; set; }
        }

        [Fact]
        public void Properties_SetAndGet()
        {
            var timeouts = new TestTimeouts
            {
                CloseTimeout = TimeSpan.FromSeconds(1),
                OpenTimeout = TimeSpan.FromSeconds(2),
                ReceiveTimeout = TimeSpan.FromSeconds(3),
                SendTimeout = TimeSpan.FromSeconds(4)
            };
            Assert.Equal(TimeSpan.FromSeconds(1), timeouts.CloseTimeout);
            Assert.Equal(TimeSpan.FromSeconds(2), timeouts.OpenTimeout);
            Assert.Equal(TimeSpan.FromSeconds(3), timeouts.ReceiveTimeout);
            Assert.Equal(TimeSpan.FromSeconds(4), timeouts.SendTimeout);
        }
    }
}
