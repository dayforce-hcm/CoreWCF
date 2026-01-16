using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWCFSuite.Framework
{
    public class ServiceModelConfigBuilder
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private bool _started = false;
        private bool _ended = false;
        private bool _clientSectionStarted = false;
        private bool _clientSectionEnded = false;
        private bool _bindingsSectionStarted = false;
        private bool _bindingsSectionEnded = false;
        private bool _basingHttpBindingSectionStarted = false;
        private bool _basingHttpBindingSectionEnded = false;
        private bool _webHttpBindingSectionStarted = false;
        private bool _webHttpBindingSectionEnded = false;
        private bool _behaviorsSectionStarted = false;
        private bool _behaviorsSectionEnded = false;
        private int _indentLevel = 0;

        private void Indent() => _sb.Append(new string(' ', _indentLevel * 2));

        public ServiceModelConfigBuilder StartConfig()
        {
            if (_started) return this;
            _sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            _sb.AppendLine("<configuration>");
            _sb.AppendLine("  <system.serviceModel>");
            _started = true;
            return this;
        }

        public ServiceModelConfigBuilder StartClientSection()
        {
            if (_clientSectionStarted) return this;
            _sb.AppendLine("    <client>");
            _clientSectionStarted = true;
            _clientSectionEnded = false;
            return this;
        }

        public ServiceModelConfigBuilder CloseClientSection()
        {
            if (!_clientSectionStarted || _clientSectionEnded) return this;
            _sb.AppendLine("    </client>");
            _clientSectionEnded = true;
            return this;
        }

        public ServiceModelConfigBuilder AddEndpoint(
                string address = null,
                string behaviorConfiguration = null,
                string binding = null,
                string bindingConfiguration = null,
                string contract = null,
                string endpointConfiguration = null,
                string kind = null,
                string name = null)
        {
            _sb.Append("      <endpoint");
            if (!string.IsNullOrEmpty(address))
                _sb.Append($" address=\"{address}\"");
            if (!string.IsNullOrEmpty(behaviorConfiguration))
                _sb.Append($" behaviorConfiguration=\"{behaviorConfiguration}\"");
            if (!string.IsNullOrEmpty(binding))
                _sb.Append($" binding=\"{binding}\"");
            if (!string.IsNullOrEmpty(bindingConfiguration))
                _sb.Append($" bindingConfiguration=\"{bindingConfiguration}\"");
            if (!string.IsNullOrEmpty(contract))
                _sb.Append($" contract=\"{contract}\"");
            if (!string.IsNullOrEmpty(endpointConfiguration))
                _sb.Append($" listenUri=\"{endpointConfiguration}\"");
            if (!string.IsNullOrEmpty(kind))
                _sb.Append($" kind=\"{kind}\"");
            if (!string.IsNullOrEmpty(name))
                _sb.Append($" name=\"{name}\"");
            _sb.AppendLine(" />");
            return this;
        }

        public ServiceModelConfigBuilder StartBindingsSection()
        {
            if (_bindingsSectionStarted) return this;
            _sb.AppendLine("    <bindings>");
            _bindingsSectionStarted = true;
            _bindingsSectionEnded = false;
            return this;
        }

        public ServiceModelConfigBuilder CloseBindingsSection()
        {
            if (!_bindingsSectionStarted || _bindingsSectionEnded) return this;
            _sb.AppendLine("    </bindings>");
            _bindingsSectionEnded = true;
            return this;
        }

        public ServiceModelConfigBuilder StartHttpBindingSection()
        {
            if (_basingHttpBindingSectionStarted) return this;
            _sb.AppendLine("      <basicHttpBinding>");
            _basingHttpBindingSectionStarted = true;
            _basingHttpBindingSectionEnded = false;
            return this;
        }

        public ServiceModelConfigBuilder CloseHttpBindingSection()
        {
            if (!_basingHttpBindingSectionStarted || _basingHttpBindingSectionEnded) return this;
            _sb.AppendLine("      </basicHttpBinding>");
            _basingHttpBindingSectionEnded = true;
            return this;
        }

        public ServiceModelConfigBuilder StartWebHttpBindingSection()
        {
            if (_webHttpBindingSectionStarted) return this;
            _sb.AppendLine("      <webHttpBinding>");
            _webHttpBindingSectionStarted = true;
            _webHttpBindingSectionEnded = false;
            return this;
        }

        public ServiceModelConfigBuilder CloseWebHttpBindingSection()
        {
            if (!_webHttpBindingSectionStarted || _webHttpBindingSectionEnded) return this;
            _sb.AppendLine("      </webHttpBinding>");
            _webHttpBindingSectionEnded = true;
            return this;
        }

        // Hierarchical element support
        public ServiceModelConfigBuilder StartElement(string elementName, params (string, string)[] attributes)
        {
            Indent();
            _sb.Append($"<{elementName}");
            foreach (var (attr, val) in attributes)
            {
                if (!string.IsNullOrEmpty(val))
                    _sb.Append($" {attr}=\"{val}\"");
            }
            _sb.AppendLine(">");
            _indentLevel++;
            return this;
        }

        public ServiceModelConfigBuilder EndElement(string elementName)
        {
            _indentLevel--;
            Indent();
            _sb.AppendLine($"</{elementName}>");
            return this;
        }

        public ServiceModelConfigBuilder AddElement(string elementName, params (string, string)[] attributes)
        {
            Indent();
            _sb.Append($"<{elementName}");
            foreach (var (attr, val) in attributes)
            {
                if (!string.IsNullOrEmpty(val))
                    _sb.Append($" {attr}=\"{val}\"");
            }
            _sb.AppendLine(" />");
            return this;
        }

        // Convenience wrappers for WCF config
        public ServiceModelConfigBuilder StartBinding(string name = null, params (string, string)[] attributes)
        {
            var allAttributes = new List<(string, string)> { ("name", name) };
            if (attributes != null && attributes.Length > 0)
            {
                allAttributes.AddRange(attributes);
            }
            return StartElement("binding", allAttributes.ToArray());
        }
        public ServiceModelConfigBuilder CloseBinding()
            => EndElement("binding");

        public ServiceModelConfigBuilder StartSecurity(string mode = null)
            => StartElement("security", ("mode", mode));
        public ServiceModelConfigBuilder CloseSecurity()
            => EndElement("security");

        public ServiceModelConfigBuilder StartTransport(string clientCredentialType = null, string proxyCredentialType = null, string realm = null)
            => StartElement("transport", ("clientCredentialType", clientCredentialType), ("proxyCredentialType", proxyCredentialType), ("realm", realm));
        public ServiceModelConfigBuilder CloseTransport()
            => EndElement("transport");

        public ServiceModelConfigBuilder StartExtendedProtectionPolicy(string policyEnforcement = null, string protectionScenario = null)
           => StartElement("extendedProtectionPolicy", ("policyEnforcement", policyEnforcement), ("protectionScenario", protectionScenario));
        public ServiceModelConfigBuilder CloseExtendedProtectionPolicy()
            => EndElement("extendedProtectionPolicy");

        public ServiceModelConfigBuilder AddMessage(string clientCredentialType = null, string algorithmSuite = null)
            => AddElement("message", ("clientCredentialType", clientCredentialType), ("algorithmSuite", algorithmSuite));

        public ServiceModelConfigBuilder StartEndpointBehaviorsSection()
        {
            if (_behaviorsSectionStarted) return this;
            _sb.AppendLine("    <behaviors>");
            _sb.AppendLine("      <endpointBehaviors>");
            _behaviorsSectionStarted = true;
            _behaviorsSectionEnded = false;
            return this;
        }

        public ServiceModelConfigBuilder CloseEndpointBehaviorsSection()
        {
            if (!_behaviorsSectionStarted || _behaviorsSectionEnded) return this;
            _sb.AppendLine("      </endpointBehaviors>");
            _sb.AppendLine("    </behaviors>");
            _behaviorsSectionEnded = true;
            return this;
        }

        public ServiceModelConfigBuilder AddEndpointBehavior(string behaviorName = null)
        {
            _sb.Append("        <behavior");
            if (!string.IsNullOrEmpty(behaviorName))
                _sb.Append($" name=\"{behaviorName}\"");
            _sb.Append(" >");
            _sb.AppendLine("        </behavior>");
            return this;
        }

        public ServiceModelConfigBuilder EndConfig()
        {
            if (_ended) return this;
            if (_clientSectionStarted && !_clientSectionEnded)
            {
                CloseClientSection();
            }
            if (_bindingsSectionStarted && !_bindingsSectionEnded)
            {
                CloseBindingsSection();
            }
            if (_behaviorsSectionStarted && !_behaviorsSectionEnded)
            {
                CloseEndpointBehaviorsSection();
            }
            _sb.AppendLine("  </system.serviceModel>");
            _sb.AppendLine("</configuration>");
            _ended = true;
            return this;
        }

        public override string ToString()
        {
            if (!_ended) EndConfig();
            return _sb.ToString();
        }
    }
}
