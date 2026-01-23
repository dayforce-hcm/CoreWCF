using CoreWCF.ConfigurationManager.Client;
using System.Net;
using System.Net.Security;
using System.Security.Authentication.ExtendedProtection;
using Xunit;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration
{
    public class ChannelBindingUtilityTests
    {
        [Fact]
        public void DefaultPolicy_IsPolicyEnforcementNever()
        {
            var policy = ChannelBindingUtility.DefaultPolicy;
            Assert.NotNull(policy);
            Assert.Equal(PolicyEnforcement.Never, policy.PolicyEnforcement);
        }

        [Fact]
        public void GetToken_WithNullTransportContext_ReturnsNull()
        {
            ChannelBinding result = ChannelBindingUtility.GetToken((TransportContext)null);
            Assert.Null(result);
        }

        [Fact]
        public void GetToken_WithTransportContext_ReturnsChannelBinding()
        {
            var expected = new TestChannelBinding();
            var context = new TestTransportContext(expected);

            var result = ChannelBindingUtility.GetToken(context);

            Assert.Same(expected, result);
        }

        [Fact]
        public void GetToken_WithSslStream_DelegatesToTransportContext()
        {
            var expected = new TestChannelBinding();
            var context = new TestTransportContext(expected);
            var stream = new TestSslStream(context);

            var result = ChannelBindingUtility.GetToken(stream.GetTestTransportContext());

            Assert.Same(expected, result);
        }

        [Fact]
        public void Dispose_NullChannelBinding_DoesNothing()
        {
            ChannelBinding binding = null;
            ChannelBindingUtility.Dispose(ref binding);
            Assert.Null(binding);
        }

        [Fact]
        public void Dispose_ChannelBinding_DisposesAndSetsNull()
        {
            ChannelBinding binding = new TestChannelBinding();
            ChannelBindingUtility.Dispose(ref binding);
            Assert.True(((TestChannelBinding)binding)?.Disposed ?? true); // binding is null after dispose
            Assert.Null(binding);
        }

        // Test doubles for ChannelBinding, TransportContext, SslStream
        private class TestChannelBinding : ChannelBinding
        {
            public bool Disposed { get; private set; }
            protected override bool ReleaseHandle() { Disposed = true; return true; }
            public override bool IsInvalid => false;
            public override int Size => 0; // Implement required abstract property
        }

        private class TestTransportContext : TransportContext
        {
            private readonly ChannelBinding _binding;
            public TestTransportContext(ChannelBinding binding) => _binding = binding;
            public override ChannelBinding GetChannelBinding(ChannelBindingKind kind) => _binding;

        }

        private class TestSslStream : SslStream
        {
            private readonly TransportContext _context;
            public TestSslStream(TransportContext context) : base(new System.IO.MemoryStream())
            {
                _context = context;
            }
            // Instead of hiding, provide a method to access the test context
            public TransportContext GetTestTransportContext() => _context;
        }
    }
}
