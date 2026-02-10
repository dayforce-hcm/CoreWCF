using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace TestWCFSuite.Framework
{
    internal class MessageSinkTestOutputHelper : ITestOutputHelper
    {
        private readonly IMessageSink m_messageSink;

        public MessageSinkTestOutputHelper(IMessageSink messageSink)
        {
            m_messageSink = messageSink;
        }

        public string Output => string.Empty;

        public void Write(string message) => m_messageSink.OnMessage(new DiagnosticMessage(message));

        public void Write(string format, params object[] args) => m_messageSink.OnMessage(new DiagnosticMessage(format, args));

        public void WriteLine(string message) => Write(message);

        public void WriteLine(string format, params object[] args) => Write(format, args);
    }
}
