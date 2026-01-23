using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Elements.Bindings
{
    public class ConfigurationChannelBindingUtilityTests
    {
        [Fact]
        public void IsDefaultPolicy_ReturnsTrueForStaticDefaultPolicy()
        {
            // Use the static default policy from the utility
            var defaultPolicy = typeof(ConfigurationChannelBindingUtility)
                .GetField("s_defaultPolicy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(null) as ExtendedProtectionPolicy;
            Assert.True(ConfigurationChannelBindingUtility.IsDefaultPolicy(defaultPolicy));
        }

        [Fact]
        public void AreEqual_ReturnsTrueForIdenticalPolicies()
        {
            var policy1 = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
            var policy2 = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
            Assert.True(ConfigurationChannelBindingUtility.AreEqual(policy1, policy2));
        }

        [Fact]
        public void AreEqual_DifferentPolicyEnforcement_ReturnsFalse()
        {
            var policy1 = new ExtendedProtectionPolicy(PolicyEnforcement.Always);
            var policy2 = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
            Assert.False(ConfigurationChannelBindingUtility.AreEqual(policy1, policy2));
        }

        [Fact]
        public void AreEqual_NullArguments_Throw()
        {
            var policy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
            Assert.Throws<ArgumentNullException>(() => ConfigurationChannelBindingUtility.AreEqual(null!, policy));
            Assert.Throws<ArgumentNullException>(() => ConfigurationChannelBindingUtility.AreEqual(policy, null!));
        }

        [Fact]
        public void CopyFrom_CopiesAllPropertiesAndServiceNames()
        {
            var source = new ExtendedProtectionPolicyElement
            {
                PolicyEnforcement = PolicyEnforcement.Always,
                ProtectionScenario = ProtectionScenario.TransportSelected
            };
            source.CustomServiceNames.Add(new ServiceNameElement { Name = "spn/http/foo" });
            source.CustomServiceNames.Add(new ServiceNameElement { Name = "spn/http/bar" });

            var dest = new ExtendedProtectionPolicyElement();

            ConfigurationChannelBindingUtility.CopyFrom(source, dest);

            Assert.Equal(source.PolicyEnforcement, dest.PolicyEnforcement);
            Assert.Equal(source.ProtectionScenario, dest.ProtectionScenario);
            Assert.Equal(source.CustomServiceNames.Count, dest.CustomServiceNames.Count);

            var names = new System.Collections.Generic.List<string>();
            foreach (ServiceNameElement e in (IEnumerable)dest.CustomServiceNames)
            {
                names.Add(e.Name);
            }
            Assert.Equal(new[] { "spn/http/foo", "spn/http/bar" }, names);
        }

        [Fact]
        public void InitializeFrom_NonDefaultPolicy_PopulatesElement()
        {
            var names = new ServiceNameCollection(new[] { "spn/http/x", "spn/http/y" });
            var policy = new ExtendedProtectionPolicy(PolicyEnforcement.Always, ProtectionScenario.TrustedProxy, names);
            var dest = new ExtendedProtectionPolicyElement();

            ConfigurationChannelBindingUtility.InitializeFrom(policy, dest);

            Assert.Equal(policy.PolicyEnforcement, dest.PolicyEnforcement);
            Assert.Equal(policy.ProtectionScenario, dest.ProtectionScenario);
            Assert.Equal(2, dest.CustomServiceNames.Count);

            var copied = new System.Collections.Generic.List<string>();
            foreach (ServiceNameElement e in (IEnumerable)dest.CustomServiceNames)
            {
                copied.Add(e.Name);
            }
            Assert.Equal(new[] { "spn/http/x", "spn/http/y" }, copied);
        }

        [Fact]
        public void IsSubset_EmptySubset_ReturnsTrue()
        {
            var primary = new ServiceNameCollection(Array.Empty<string>());
            var subset = new ServiceNameCollection(Array.Empty<string>());
            Assert.True(ConfigurationChannelBindingUtility.IsSubset(primary, subset));
        }

        [Fact]
        public void IsSubset_SubsetLargerThanPrimary_ReturnsFalse()
        {
            var primary = new ServiceNameCollection(Array.Empty<string>());
            var subset = new ServiceNameCollection(new[] { "a", "b" });
            Assert.False(ConfigurationChannelBindingUtility.IsSubset(primary, subset));
        }

        [Fact]
        public void IsSubset_ProperSubset_ReturnsTrue()
        {
            var primary = new ServiceNameCollection(new[] { "a", "b" });
            var subset = new ServiceNameCollection(new[] { "a" });
            Assert.True(ConfigurationChannelBindingUtility.IsSubset(primary, subset));
        }

        [Fact]
        public void GetToken_NullContext_ReturnsNull()
        {
            Assert.Null(ConfigurationChannelBindingUtility.GetToken((TransportContext)null));
        }

        private sealed class FakeChannelBinding : ChannelBinding
        {
            private readonly int _size;
            private readonly bool _invalid;

            public FakeChannelBinding(int size, bool invalid = false)
            {
                _size = size;
                _invalid = invalid;
                // allocate native buffer and set handle so DangerousGetHandle returns a valid pointer
                SetHandle(Marshal.AllocHGlobal(size));
            }

            public override int Size => _size;
            public override bool IsInvalid => _invalid;

            protected override bool ReleaseHandle()
            {
                if (handle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(handle);
                    SetHandle(IntPtr.Zero);
                }
                return true;
            }
        }

        [Fact]
        public void DuplicateToken_NullSource_ReturnsNull()
        {
            Assert.Null(ConfigurationChannelBindingUtility.DuplicateToken(null));
        }

        [Fact]
        public void DuplicateToken_InvalidOrClosed_Throws()
        {
            var invalid = new FakeChannelBinding(10, invalid: true);
            Assert.Throws<ObjectDisposedException>(() => ConfigurationChannelBindingUtility.DuplicateToken(invalid));

            using var normal = new FakeChannelBinding(10);
            normal.Dispose(); // marks SafeHandle closed
            Assert.Throws<ObjectDisposedException>(() => ConfigurationChannelBindingUtility.DuplicateToken(normal));
        }

        [Fact]
        public void DuplicateToken_Valid_CreatesCopyWithSameSize()
        {
            using var source = new FakeChannelBinding(16);
            var copy = ConfigurationChannelBindingUtility.DuplicateToken(source);
            Assert.NotNull(copy);
            Assert.Equal(source.Size, copy.Size);
            copy.Dispose();
        }

        [Fact]
        public void Dispose_SetsRefToNullAndDisposes()
        {
            ChannelBinding cb = new FakeChannelBinding(8);
            ConfigurationChannelBindingUtility.Dispose(ref cb);
            Assert.Null(cb);
        }
    }
}
