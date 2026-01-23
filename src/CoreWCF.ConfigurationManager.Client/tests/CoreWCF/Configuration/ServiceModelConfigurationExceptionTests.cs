using System;
using System.Runtime.Serialization;
using System.Reflection;
using CoreWCF.ConfigurationManager.Client;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ServiceModelConfigurationExceptionTests
    {
        [Fact]
        public void DefaultConstructor_CreatesInstance()
        {
            var ex = new ServiceModelConfigurationException();
            Assert.NotNull(ex);
        }

        [Fact]
        public void Constructor_WithMessage_SetsMessage()
        {
            var ex = new ServiceModelConfigurationException("Test message");
            Assert.Equal("Test message", ex.Message);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsProperties()
        {
            var inner = new Exception("Inner");
            var ex = new ServiceModelConfigurationException("Test", inner);
            Assert.Equal("Test", ex.Message);
            Assert.Equal(inner, ex.InnerException);
        }

        [Fact]
        public void Constructor_WithSerializationInfo_SetsProperties()
        {
#pragma warning disable SYSLIB0050
            var info = new SerializationInfo(typeof(ServiceModelConfigurationException), new FormatterConverter());
#pragma warning restore SYSLIB0050
            info.AddValue("ClassName", typeof(ServiceModelConfigurationException).FullName);
            info.AddValue("Message", "Serialized message");
            info.AddValue("InnerException", null, typeof(Exception));
            info.AddValue("HelpURL", null, typeof(string));
            info.AddValue("StackTraceString", null, typeof(string));
            info.AddValue("RemoteStackTraceString", null, typeof(string));
            info.AddValue("RemoteStackIndex", 0, typeof(int));
            info.AddValue("ExceptionMethod", null, typeof(string));
            info.AddValue("HResult", -2146233088, typeof(int));
            info.AddValue("Source", null, typeof(string));
            var context = new StreamingContext();
            var ctor = typeof(ServiceModelConfigurationException).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);
            var ex = (ServiceModelConfigurationException)ctor.Invoke(new object[] { info, context });
            Assert.NotNull(ex);
        }
    }
}
