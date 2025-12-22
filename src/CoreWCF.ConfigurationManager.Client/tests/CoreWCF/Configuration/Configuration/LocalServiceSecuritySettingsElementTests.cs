using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;
using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class LocalServiceSecuritySettingsElementTests
    {
        [Fact]
        public void Properties_SetAndGet()
        {
            var element = new LocalServiceSecuritySettingsElement();
            element.DetectReplays = true;
            element.IssuedCookieLifetime = TimeSpan.FromMinutes(5);
            element.MaxStatefulNegotiations = 10;
            element.ReplayCacheSize = 20;
            element.MaxClockSkew = TimeSpan.FromMinutes(1);
            element.NegotiationTimeout = TimeSpan.FromMinutes(2);
            element.ReplayWindow = TimeSpan.FromMinutes(3);
            element.InactivityTimeout = TimeSpan.FromMinutes(4);
            element.SessionKeyRenewalInterval = TimeSpan.FromMinutes(6);
            element.SessionKeyRolloverInterval = TimeSpan.FromMinutes(7);
            element.ReconnectTransportOnFailure = true;
            element.MaxPendingSessions = 30;
            element.MaxCachedCookies = 40;
            element.TimestampValidityDuration = TimeSpan.FromMinutes(8);
            Assert.True(element.DetectReplays);
            Assert.Equal(TimeSpan.FromMinutes(5), element.IssuedCookieLifetime);
            Assert.Equal(10, element.MaxStatefulNegotiations);
            Assert.Equal(20, element.ReplayCacheSize);
            Assert.Equal(TimeSpan.FromMinutes(1), element.MaxClockSkew);
            Assert.Equal(TimeSpan.FromMinutes(2), element.NegotiationTimeout);
            Assert.Equal(TimeSpan.FromMinutes(3), element.ReplayWindow);
            Assert.Equal(TimeSpan.FromMinutes(4), element.InactivityTimeout);
            Assert.Equal(TimeSpan.FromMinutes(6), element.SessionKeyRenewalInterval);
            Assert.Equal(TimeSpan.FromMinutes(7), element.SessionKeyRolloverInterval);
            Assert.True(element.ReconnectTransportOnFailure);
            Assert.Equal(30, element.MaxPendingSessions);
            Assert.Equal(40, element.MaxCachedCookies);
            Assert.Equal(TimeSpan.FromMinutes(8), element.TimestampValidityDuration);
        }

        [Fact]
        public void CopyFrom_CopiesProperties()
        {
            var source = new LocalServiceSecuritySettingsElement();
            source.DetectReplays = true;
            source.IssuedCookieLifetime = TimeSpan.FromMinutes(5);
            source.MaxStatefulNegotiations = 10;
            source.ReplayCacheSize = 20;
            source.MaxClockSkew = TimeSpan.FromMinutes(1);
            source.NegotiationTimeout = TimeSpan.FromMinutes(2);
            source.ReplayWindow = TimeSpan.FromMinutes(3);
            source.InactivityTimeout = TimeSpan.FromMinutes(4);
            source.SessionKeyRenewalInterval = TimeSpan.FromMinutes(6);
            source.SessionKeyRolloverInterval = TimeSpan.FromMinutes(7);
            source.ReconnectTransportOnFailure = true;
            source.MaxPendingSessions = 30;
            source.MaxCachedCookies = 40;
            source.TimestampValidityDuration = TimeSpan.FromMinutes(8);
            var target = new LocalServiceSecuritySettingsElement();
            target.CopyFrom(source);
            Assert.Equal(source.DetectReplays, target.DetectReplays);
            Assert.Equal(source.IssuedCookieLifetime, target.IssuedCookieLifetime);
            Assert.Equal(source.MaxStatefulNegotiations, target.MaxStatefulNegotiations);
            Assert.Equal(source.ReplayCacheSize, target.ReplayCacheSize);
            Assert.Equal(source.MaxClockSkew, target.MaxClockSkew);
            Assert.Equal(source.NegotiationTimeout, target.NegotiationTimeout);
            Assert.Equal(source.ReplayWindow, target.ReplayWindow);
            Assert.Equal(source.InactivityTimeout, target.InactivityTimeout);
            Assert.Equal(source.SessionKeyRenewalInterval, target.SessionKeyRenewalInterval);
            Assert.Equal(source.SessionKeyRolloverInterval, target.SessionKeyRolloverInterval);
            Assert.Equal(source.ReconnectTransportOnFailure, target.ReconnectTransportOnFailure);
            Assert.Equal(source.MaxPendingSessions, target.MaxPendingSessions);
            Assert.Equal(source.MaxCachedCookies, target.MaxCachedCookies);
            Assert.Equal(source.TimestampValidityDuration, target.TimestampValidityDuration);
        }

        [Fact]
        public void CopyFrom_ThrowsIfSourceNull()
        {
            var element = new LocalServiceSecuritySettingsElement();
            Assert.Throws<ArgumentNullException>(() => element.CopyFrom(null));
        }
    }
}
