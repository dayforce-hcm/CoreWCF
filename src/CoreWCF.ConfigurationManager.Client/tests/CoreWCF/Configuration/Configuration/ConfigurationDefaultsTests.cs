using System;
using Xunit;
using CoreWCF.ConfigurationManager.Client;

namespace CoreWCF.ConfigurationManager.Client.Tests.CoreWCF.Configuration.Configuration
{
    public class ConfigurationDefaultsTests
    {
        [Fact]
        public void BinaryEncoderDefaults_EnvelopeVersion_IsSoap12()
        {
            Assert.Equal(System.ServiceModel.EnvelopeVersion.Soap12, BinaryEncoderDefaults.EnvelopeVersion);
        }

        [Fact]
        public void BinaryEncoderDefaults_MaxSessionSize_Is2048()
        {
            Assert.Equal(2048, BinaryEncoderDefaults.MaxSessionSize);
        }

        [Fact]
        public void ConnectionOrientedTransportDefaults_Values_AreExpected()
        {
            Assert.Equal(8192, ConnectionOrientedTransportDefaults.ConnectionBufferSize);
            Assert.Equal(System.Net.Security.ProtectionLevel.EncryptAndSign, ConnectionOrientedTransportDefaults.ProtectionLevel);
            Assert.Equal(10, ConnectionOrientedTransportDefaults.MaxOutboundConnectionsPerEndpoint);
        }

        [Fact]
        public void HttpTransportDefaults_Values_AreExpected()
        {
            Assert.False(HttpTransportDefaults.AllowCookies);
            Assert.True(HttpTransportDefaults.DecompressionEnabled);
            Assert.True(HttpTransportDefaults.KeepAliveEnabled);
            Assert.True(HttpTransportDefaults.UseDefaultWebProxy);
        }

        [Fact]
        public void MtomEncoderDefaults_MaxBufferSize_Is65536()
        {
            Assert.Equal(65536, MtomEncoderDefaults.MaxBufferSize);
        }

        [Fact]
        public void OneWayDefaults_Values_AreExpected()
        {
            Assert.False(OneWayDefaults.PacketRoutable);
            Assert.Equal(10, OneWayDefaults.MaxOutboundChannelsPerEndpoint);
        }

        [Fact]
        public void SecurityBindingDefaults_Values_AreExpected()
        {
            Assert.True(SecurityBindingDefaults.DefaultRequireDerivedKeys);
            Assert.Equal(900000, SecurityBindingDefaults.DefaultMaxCachedNonces);
        }

        [Fact]
        public void TcpTransportDefaults_Values_AreExpected()
        {
            Assert.False(TcpTransportDefaults.PortSharingEnabled);
            Assert.False(TcpTransportDefaults.TeredoEnabled);
        }

        [Fact]
        public void TextEncoderDefaults_EncodingString_IsUtf8()
        {
            Assert.Equal("utf-8", TextEncoderDefaults.EncodingString);
        }

        [Fact]
        public void TransportDefaults_Values_AreExpected()
        {
            Assert.Equal(65536, TransportDefaults.MaxReceivedMessageSize);
            Assert.False(TransportDefaults.RequireClientCertificate);
        }

        [Fact]
        public void WebSocketDefaults_Values_AreExpected()
        {
            Assert.Equal(16 * 1024, WebSocketDefaults.BufferSize);
            Assert.Equal(100, WebSocketDefaults.DefaultMaxConcurrentSessions);
            Assert.Equal("Upgrade", WebSocketDefaults.WebSocketConnectionHeaderValue);
        }
    }
}
