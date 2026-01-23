// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Configuration;

namespace CoreWCF.ConfigurationManager.Client
{
    public sealed class ServiceModelSectionGroup : ConfigurationSectionGroup
    {
        public ServiceModelSectionGroup()
        {
        }

        public BindingsSection Bindings
        {
            get { return (BindingsSection)Sections[ConfigurationStrings.BindingsSectionGroupName]; }
        }

        public ClientSection Client
        {
            get { return (ClientSection)Sections[ConfigurationStrings.ClientSectionName]; }
        }


        public static ServiceModelSectionGroup GetSectionGroup(System.Configuration.Configuration config)
        {
            if (config == null)
            {
                throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull(nameof(config));
            }

            return (ServiceModelSectionGroup)config.SectionGroups[ConfigurationStrings.SectionGroupName];
        }
    }
}
